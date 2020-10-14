using System;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : BaseCountDown
{
    public Image[] CDImages;
    public Text[] CDText;

    public Gradient Gradient;

    public override void Show(TimeSpan delta2End)
    {
        double leftSeconds = 0;
        float progress = 0;
        if (delta2End.TotalSeconds > 0)
        {
            leftSeconds = delta2End.TotalSeconds;
            if (CDTimeSpan.TotalMilliseconds == 0)
            {
                progress = 1;
            }
            else
            {
                progress = (float)(delta2End.TotalMilliseconds / CDTimeSpan.TotalMilliseconds);
            }
        }

        var color = Gradient.Evaluate(progress);
        foreach (var item in CDImages)
        {
            if (item)
            {
                item.fillAmount = (float)progress;
                item.color = color;
            }
        }

        var str = leftSeconds.ToString("00.00");
        foreach (var item in CDText)
        {
            if (item)
            {
                item.text = str;
            }
        }
    }

    private void Update()
    {
        var now = DateTimeOffset.UtcNow;
        Tick(now);
    }
}
