using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAudio : MonoBehaviour
{
    public AudioSource searchAudio;
    public AudioSource failAudio;
    public float playSpan = 3.6f;
    public float playStartOffset = 1f;
    public StageController script;

    private float currentTime;

    void Start()
    {
        currentTime = playStartOffset;
    }

    void FixedUpdate()
    {
        if (script.goalCondition())
        {
            currentTime += Time.deltaTime;
            if (currentTime > playSpan){
                // 効果音を発する
                searchAudio.Play();
                currentTime = 0f;
            }
        }
    }

    //　衝突時の効果音
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !script.goalCondition())
        {
            failAudio.Play();
        }
    }
}