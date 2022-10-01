using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    public Vector3 movePosition;    // 移動先 
    public float waitTime = 3f;     // 待ち時間
    public float moveTime = 5f;     // 移動時間
    public float timeOffset = 1f;   // 時間のオフセット
    public AudioSource moveSound;   // 動く際のサウンド
    public StageController script;

    private Vector3 initiatePosition;
    private float elapsedTime = 0f;
    private int moveFlag = 0;

    private void Start()
    {
        initiatePosition = this.gameObject.transform.position;
        elapsedTime += timeOffset;
    }

    private void FixedUpdate()
    {
        if (moveFlag == 0 || moveFlag == 2)
        {
            WallWait(); // 壁の待ち時間
        }
        else if (moveFlag == 1)
        {
            IsMove(initiatePosition, initiatePosition + movePosition); // 壁の動作
        }
        else if (moveFlag == 3)
        {
            IsMove(initiatePosition + movePosition, initiatePosition); // 壁の動作
        }
    }

    // 壁の待ち時間
    private void WallWait()
    {
        if (elapsedTime >= waitTime)
        {
            elapsedTime = 0f;
            moveSound.Play();
            moveFlag += 1;
            return;
        }
        elapsedTime += Time.deltaTime;
    }

    // 壁の動作
    private void IsMove(Vector3 preposition, Vector3 postposition)
    {
        elapsedTime += Time.deltaTime;
        float rate = Mathf.Clamp01(elapsedTime / moveTime);
        this.gameObject.transform.position = Vector3.Lerp(preposition, postposition, rate);
        if (rate >= 1f)
        {
            elapsedTime = 0f;
            moveFlag = (moveFlag + 1) % 4;
            moveSound.Stop();
        }
    }
}
