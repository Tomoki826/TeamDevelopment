using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointAudio : MonoBehaviour
{
    public AudioSource searchAudio;
    public AudioSource reachAudio;
    public float playSpan = 3f;
    public float playStartOffset = 1f;

    private float currentTime;
    private bool reachedFlag;
    private CheckPoint script;

    void Start()
    {
        currentTime = playStartOffset;
        reachedFlag = false;
        script = gameObject.GetComponent<CheckPoint>();
    }

    void FixedUpdate()
    {
        if (! script.CheckPointCheck())
        {
            currentTime += Time.deltaTime;
            if (currentTime > playSpan){
                // 効果音を発する
                searchAudio.Play();
                currentTime = 0f;
            }
        }
        else if (! reachedFlag)
        {
            reachedFlag = true;
            searchAudio.Stop();
            reachAudio.Play();
        }
    }

}