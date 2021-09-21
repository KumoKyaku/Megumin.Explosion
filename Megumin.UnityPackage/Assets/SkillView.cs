using Megumin;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour
{
    public Image CDImage;
    public Text CanUseCount;
    public GameObject[] MainSkillState;
    public GameObject KeyTip;


    public IUseable<float> CDTimer;

    private void Update()
    {
        if (CDTimer == null)
        {
            return;
        }

        CanUseCount.text = CDTimer.ResidualCanUseCount.ToString();
        if (CDTimer.MaxCanUseCount == CDTimer.ResidualCanUseCount)
        {
            CDImage.fillAmount = 1;
        }
        else
        {
            CDImage.fillAmount = CDTimer.NextCanUse / CDTimer.PerSpan;
        }

        MainSkillState[0].gameObject.SetActive(CDTimer.CanUse);
        MainSkillState[1].gameObject.SetActive(!CDTimer.CanUse);
        MainSkillState[2].gameObject.SetActive(CDTimer.IsUsing);

        KeyTip.SetActive(CDTimer.ResidualCanUseCount > 0);
    }
}
