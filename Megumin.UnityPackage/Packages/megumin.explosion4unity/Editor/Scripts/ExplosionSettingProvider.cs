using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

[InitializeOnLoad]
public class ExplosionSettingProvider : SettingsProvider
{
    static ExplosionSettingProvider()
    {
        //Debug.Log("ExplosionSettingProvider");
        if (Nagetive.value)
        {
            InspectorNavigation.Register();
        }

        ClonePathModeSetting.PathMode = ClonePathMode.value;
    }



    [SettingsProvider]
    static SettingsProvider CreateSettingsProvider()
    {
        var provider = new ExplosionSettingProvider("Preferences/Explosion", SettingsScope.User);
        //Debug.Log("CreateSettingsProvider");
        return provider;
    }

    static Settings settings = new Settings(new ISettingsRepository[] { new UserSettingsRepository() });

    class Value<T> : UserSetting<T>
    {
        public Value(string key, T value)
            : base(ExplosionSettingProvider.settings, key, value, SettingsScope.User)
        { }
    }

    class SymbolValue : Value<bool>
    {
        public SymbolValue(string key, bool value) : base(key, value)
        {
        }

        public void DrawSymbol(string searchContext)
        {
            var old = value;
            value = SettingsGUILayout.SearchableToggle(key, value, searchContext);

            if (value != old)
            {
                if (value)
                {
                    PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out var array);

                    if (!array.Contains(key))
                    {
                        var list = array.ToList();
                        list.Add(key);
                        array = list.ToArray();
                        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, array);
                        Debug.Log($"Add Scripting Define Symbol [{key}]");
                    }
                }
                else
                {
                    PlayerSettings.GetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, out var array);

                    if (array.Contains(key))
                    {
                        var list = array.ToList();
                        list.Remove(key);
                        array = list.ToArray();
                        PlayerSettings.SetScriptingDefineSymbols(UnityEditor.Build.NamedBuildTarget.Standalone, array);
                        Debug.Log($"Remove Scripting Define Symbol [{key}]");
                    }
                }
            }


        }
    }

    static Value<bool> Nagetive = new Value<bool>("InspectorNavigation", true);
    static SymbolValue DISABLE_SCROBJ_DRAWER = new SymbolValue("DISABLE_SCROBJ_DRAWER", false);
    static SymbolValue DISABLE_EDITORBUTTONATTRIBUTE = new SymbolValue("DISABLE_EDITORBUTTONATTRIBUTE", false);
    static SymbolValue DISABLE_MEGUMIN_PROPERTYDRWAER = new SymbolValue(nameof(DISABLE_MEGUMIN_PROPERTYDRWAER), false);

    static Value<ClonePathModeSetting.ClonePathMode> ClonePathMode
        = new Value<ClonePathModeSetting.ClonePathMode>("ClonePathMode", ClonePathModeSetting.ClonePathMode.ParentPath);

    public GUIStyle TipStype => EditorStyles.helpBox;
    public override void OnGUI(string searchContext)
    {
        base.OnGUI(searchContext);
        var old = Nagetive.value;
        Nagetive.value = SettingsGUILayout.SearchableToggle("InspectorNavigation", Nagetive.value, searchContext);
        if (Nagetive.value != old)
        {
            if (Nagetive.value)
            {
                //注册
                InspectorNavigation.Register();
            }
            else
            {
                //取消注册
                InspectorNavigation.UnRegister();
            }
        }

        EditorGUILayout.BeginFoldoutHeaderGroup(true, "new clone 按钮");
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"用于关闭ScriptObject 的new clone 按钮.", TipStype);
        DISABLE_SCROBJ_DRAWER.DrawSymbol(searchContext);

        EditorGUILayout.LabelField($"new clone 按钮新对象的默认路径。 按住Alt临时进入SubAsset模式。", TipStype);
        var pathModeOld = ClonePathMode.value;
        ClonePathMode.value = (ClonePathModeSetting.ClonePathMode)EditorGUILayout.EnumPopup(ClonePathMode.key, ClonePathMode.value);
        if (pathModeOld != ClonePathMode.value)
        {
            ClonePathModeSetting.PathMode = ClonePathMode.value;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"用于关闭EditorButton,提高编辑器性能", TipStype);
        DISABLE_EDITORBUTTONATTRIBUTE.DrawSymbol(searchContext);

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"用于关闭所有特性PropertyDrawer,提高编辑器性能", TipStype);
        DISABLE_MEGUMIN_PROPERTYDRWAER.DrawSymbol(searchContext);

        if (GUILayout.Button("Foward"))
        {
            InspectorNavigation.Forward();
        }

        if (GUILayout.Button("Back"))
        {
            InspectorNavigation.Back();
        }

        if (GUILayout.Button("Log"))
        {
            Debug.Log("Preferences");
        }
    }

    public ExplosionSettingProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
        : base(path, scopes, keywords)
    {
    }

    public override void OnActivate(string searchContext, VisualElement rootElement)
    {
        base.OnActivate(searchContext, rootElement);
        //Debug.Log("OnActivate");
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        //Debug.Log("OnDeactivate");
        settings.Save();
    }
}
