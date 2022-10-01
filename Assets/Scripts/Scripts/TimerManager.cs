using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float initiateTime = 30f;
    public GameObject timer;
    public AudioSource timerAudio;
    public Image timerEffect;
    public RectTransform canvas;
    public StageController script;

    private float maxTime;
    private float restTime;
    private bool timeUp;
    private Material timerMat;

    void Start()
    {
        //　座標を上端に合わせる
        Vector3 position = gameObject.transform.localPosition;
        position.y = canvas.sizeDelta.y / 2;
        gameObject.transform.localPosition = position;
        //　タイマーの初期設定
        timerEffect.color = Color.clear;
        if (initiateTime < 10.1f)
        {
            initiateTime = 10.1f;
        }
        restTime = initiateTime;
        maxTime = restTime;
        timeUp = false;
        timerMat = timer.GetComponent<Renderer>().material;
    }

    void FixedUpdate()
    {
        if (!timeUp)
        {
            TimeDecrease();
        }
        else
        {
            // タイムアップの処理
            script.StageCondition("TimeUp");
        }
    }

    void TimeDecrease()
    {
        // タイマーゲージを変更する
        int previousTime = Mathf.CeilToInt(restTime);
        restTime -= Time.deltaTime;
        int currentTime = Mathf.CeilToInt(restTime);

        Vector3 timerPos = timer.transform.localPosition;
        timerPos.x = -1f * canvas.sizeDelta.x * (1f - restTime / maxTime);
        timer.transform.localPosition = timerPos;
        timerMat.color = new Color(1f, restTime / maxTime, restTime / maxTime, 1f);
        timerEffect.color = Color.Lerp(timerEffect.color, Color.clear, Time.deltaTime);

        if (previousTime != currentTime)
        {
            if (currentTime > 0 && currentTime <= 10)
            {
                timerEffect.color = new Color32(128, 0, 0, 128);
                if (currentTime % 2 == 0) 
                {
                    timerAudio.Play();
                }
            }
            else if (currentTime == 0)
            {
                timeUp = true;
            }
        }
    }

    // タイマーを増加する
    public IEnumerator TimeIncrease(float addTime)
    {
        timerAudio.Stop();
        float tmpTime = restTime + addTime;
        if (maxTime < tmpTime)
        {
            maxTime = tmpTime;
        }
        // 移動の変化量を計算
        Vector3 currentPos = timer.transform.localPosition;
        Vector3 arrivePos = currentPos;
        arrivePos.x = -1f * canvas.sizeDelta.x * (1f - tmpTime / maxTime);
        float varitate = (arrivePos.x - currentPos.x) / 20f;
        // タイマーを増加
        for (int i = 0; i < 20; i++)
        {
            restTime += addTime / 20f;
            currentPos = timer.transform.localPosition;
            currentPos.x += varitate;
            timer.transform.localPosition = currentPos;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
