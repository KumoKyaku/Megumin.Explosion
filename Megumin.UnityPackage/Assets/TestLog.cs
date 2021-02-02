using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    public string teststr = "TestStr";
    public string ret = "";
    // Start is called before the first frame update
    void Start()
    {
        ret = Color.red.Html(teststr);
        Debug.Log(ret);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
