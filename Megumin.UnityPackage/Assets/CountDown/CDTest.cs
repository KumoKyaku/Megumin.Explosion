using System;
using UnityEngine;

public class CDTest : MonoBehaviour
{
    ICountDownable countDown;
    void Start()
    {
        countDown = GetComponent<CountDownManager>();
        Random();
    }

    async void Random()
    {
        var count = new System.Random().Next(10, 30);
        var result = await countDown.StartCD(TimeSpan.FromSeconds(count));
        if (result == 0)
        {
            Random();
        }
    }
}



