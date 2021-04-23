using UnityEngine;

/// <summary>
/// ¼òÒ×ÓÃ»§ÅäÖÃ
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class Pref<T>
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

    public string Key { get; protected set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="IsPerProjectInstace">Í¬Ò»¸öÏîÄ¿¶à¿ªÊ±ÊÇ²»ÊÇÃ¿¸öÏîÄ¿µ¥¶ÀÅäÖÃ</param>
    public Pref(string key, T defaultValue = default, bool IsPerProjectInstace = true)
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

    static T GetPref(string k, T defaultValue = default)
    {
        if (!PlayerPrefs.HasKey(k))
        {
            return defaultValue;
        }

        var o = (object)defaultValue;

        if (typeof(T) == typeof(string))
        {
            o = PlayerPrefs.GetString(k, (string)o);
        }
        else if (typeof(T) == typeof(bool))
        {
            var strV = PlayerPrefs.GetString(k, defaultValue.ToString());
            var b = bool.Parse(strV);
            o = b;
        }
        else if (typeof(T) == typeof(float))
        {
            o = PlayerPrefs.GetFloat(k, (float)o);
        }
        else if (typeof(T) == typeof(int))
        {
            o = PlayerPrefs.GetInt(k, (int)o);
        }

        return (T)o;
    }

    static void SetPref(string k, T value)
    {
        if (typeof(T) == typeof(string))
        {
            PlayerPrefs.SetString(k, (string)(object)value);
        }
        else if (typeof(T) == typeof(bool))
        {
            PlayerPrefs.SetString(k, value.ToString());
        }
        else if (typeof(T) == typeof(float))
        {
            PlayerPrefs.SetFloat(k, (float)(object)value);
        }
        else if (typeof(T) == typeof(int))
        {
            PlayerPrefs.SetInt(k, (int)(object)value);
        }
    }

    public static implicit operator T(Pref<T> pref)
    {
        if (pref == null)
        {
            //ï¿½ï¿½ï¿½ï¿½ï¿½Ö¹ï¿½ï¿½ï¿½ï¿½Î´ï¿½ï¿½Ê¼ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Â¿ï¿½ï¿½ï¿½ï¿½Ã¡ï¿½
            Debug.LogWarning($"{nameof(Pref<T>)} is null, return default");
            return default;
        }

        return pref.value;
    }
}


