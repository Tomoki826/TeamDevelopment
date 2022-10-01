using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public AudioSource searchAudio;     //探索時の効果音
    public AudioSource OKAudio;         //到達時の効果音
    public AudioSource GoalOKAudio;     //全て到達時の効果音
    public float playSpan = 3f;         //演奏間隔の秒数
    public float playStartOffset = 1f;  //演奏開始のオフセット時間(秒)
    public GameObject children;
    public GameObject playerObject;
    public StageController script;

    private Renderer render;                    //マテリアルの色を決定
    private bool destinationReached = false;    //目的地に到達したか
    private Collider destinationCollider;       //目的地のCollider
    private Rigidbody destinationRigidbody;     //目的地のRigidbody
    private float currentTime = 0f;

    void Start()
    {
        //演奏オフセット時間を設定
        currentTime = playStartOffset;
        //目的地の衝突判定を取得
        destinationCollider = this.GetComponent<BoxCollider>();
        //目的地のRigidbodyを取得
        destinationRigidbody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (! destinationReached)
        {
            currentTime += Time.deltaTime;
            if (currentTime > playSpan){
                // 効果音を発する
                searchAudio.Play();
                currentTime = 0f;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && ! destinationReached)
        {
            destinationReached = true;
            //到達した目的地カウントを１増やす
            script.destinationReached();
            //効果音を発する
            searchAudio.Stop();
            if (!script.goalCondition())
            {
                OKAudio.Play();
            }
            else
            {
                GoalOKAudio.Play();
            }
            //目的地をKinematicにする
            destinationRigidbody.isKinematic = true;
            //当たり判定を無効化する
            destinationCollider.enabled = false;
            //アニメーションを開始する
            StartCoroutine("DestinationReachAnimation");
        }
    }

    // 目的地に到達したか
    public bool DestinationReached()
    {
        return destinationReached;
    }

    // 到達後のアニメーション
    private IEnumerator DestinationReachAnimation()
    {
        MeshRenderer mesh = this.GetComponent<MeshRenderer>();
        MeshRenderer meshchildren = children.GetComponent<MeshRenderer>();
        for (int i = 0; i < 127; i++)
        {
            mesh.material.color -= new Color32(0, 0, 0, 2);
            meshchildren.material.color -= new Color32(0, 0, 0, 2);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
