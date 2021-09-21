using Megumin;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float perCD = 5;
    public int maxCount = 3;

    public CDTimer<float> CD { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CD = CDTicker.Instance.Create(maxCount, maxCount);
        CD.PerSpan = perCD;
        var view = GetComponent<SkillView>();
        view.CDTimer = CD;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (CD.CanUse)
            {
                CD.TryUse();
            }
        }
    }
}
