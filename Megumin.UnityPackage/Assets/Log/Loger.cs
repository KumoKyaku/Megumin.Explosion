using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Megumin;

[CreateAssetMenu(fileName = "Loger", menuName = "Loger")]
public class Loger : ScriptableObject
{
    public Color Color;
    public string Name;
    public int level;
    [MetaGUID]
    public string TestMetaGUID;
}
