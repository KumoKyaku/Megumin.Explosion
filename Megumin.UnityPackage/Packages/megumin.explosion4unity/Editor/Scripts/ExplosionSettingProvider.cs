using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;
using UnityEngine.UIElements;

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

    static Value<bool> Nagetive = new Value<bool>("InspectorNavigation", true);

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

        if (GUILayout.Button("Foward"))
        {
            InspectorNavigation.Forward();
        }

        if (GUILayout.Button("Back"))
        {
            InspectorNavigation.Back();
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
