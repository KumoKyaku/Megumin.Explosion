using UnityEngine;

public class Pref
{
    public string Key { get; protected set; }

    public static T GetPref<T>(string k, T defaultValue = default)
    {
        if (!PlayerPrefs.HasKey(k))
        {
            return defaultValue;
        }

        switch (defaultValue)
        {
            case string def:
                {
                    var ret = PlayerPrefs.GetString(k, def);
                    return (T)(object)ret;
                }
            case int def:
                {
                    var ret = PlayerPrefs.GetInt(k, def);
                    return (T)(object)ret;
                }
            case float def:
                {
                    var ret = PlayerPrefs.GetFloat(k, def);
                    return (T)(object)ret;
                }
            case bool def:
                {
                    int temp = def ? 1 : 0;
                    var v = PlayerPrefs.GetInt(k, temp);
                    var ret = v == 0 ? false : true;
                    return (T)(object)ret;
                }
            default:
                return default;
        }
    }

    public static void SetPref<T>(string k, T value)
    {
        switch (value)
        {
            case string def:
                {
                    PlayerPrefs.SetString(k, def);
                    break;
                }
            case int def:
                {
                    PlayerPrefs.SetInt(k, def);
                    break;
                }
            case float def:
                {
                    PlayerPrefs.SetFloat(k, def);
                    break;
                }
            case bool def:
                {
                    int temp = def ? 1 : 0;
                    PlayerPrefs.SetInt(k, temp);
                    break;
                }
            default:
                {
                    var str = value.ToString();
                    PlayerPrefs.SetString(k, str);
                    break;
                }
        }
    }
}

/// <summary>
/// 简易用户配置
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class Pref<T> : Pref
{
    private T value;

    public T Value
    {
        get => value;
        set
        {
            this.value = value;
            SetPref(Key, value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="IsPerProjectInstace">同一个项目多开时是不是每个项目实例单独配置</param>
    public Pref(string key, T defaultValue = default, bool IsPerProjectInstace = false)
    {
        if (IsPerProjectInstace)
        {
            key = Application.productName + key + Application.dataPath;
        }
        else
        {
            key = Application.productName + key;
        }
        Key = key;

        value = GetPref(key, defaultValue);
    }

    public static implicit operator T(Pref<T> pref)
    {
        if (pref == null)
        {
            //防止配置没有初始化
            Debug.LogWarning($"{nameof(Pref<T>)} is null, return default");
            return default;
        }

        return pref.value;
    }
}


