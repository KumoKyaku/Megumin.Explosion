using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(UnityEngine.UI.Text))]
public class TextBinding : MonoBehaviour
{
    [HideInInspector]
    public Component BindingScript;
    // Use this for initialization
    public PropertyInfo targetFeild;
    [HideInInspector]
    [SerializeField]
    public string FeildName = "";

    private Text Text;
    void Start () {
        Text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (BindingScript == null)
        {
            return;
        }

        if (targetFeild == null || targetFeild.Name != FeildName)
        {
            targetFeild = BindingScript.GetType().GetProperty(FeildName);
            if (targetFeild == null )
            {
                return;
            }
        }
        Text.text = targetFeild.GetValue(BindingScript,null).ToString();
    }
}
