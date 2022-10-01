using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTimeChange : MonoBehaviour
{
    public float addTime = 10f;
    public CheckPoint checkPointScript;
    public TimerManager timerScript;

    private bool addTimeFlag;

    void Start()
    {
        if (addTime < 10.1f)
        {
            addTime = 10.1f;
        }
        addTimeFlag = false;
    }

    void FixedUpdate()
    {
        if (!addTimeFlag && checkPointScript.CheckPointCheck())
        {
            addTimeFlag = true;
            //時間を増加させる
            StartCoroutine(timerScript.TimeIncrease(addTime));
        }
    }
}
