using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public StageController script;
    public Material normalColor;
    public Material approvedColor;
    public Material denyedColor;
    private bool goalCondition;
    private Material[] materials;

    void Start() 
    {
        //マテリアルを取得
        materials = this.GetComponent<Renderer>().sharedMaterials;
        //ゴールの状態を表示
        goalCondition = false;
    }

    void FixedUpdate()
    {
        // ゴール可能ならアニメーションする
        if (!goalCondition && script.goalCondition())
        {
            goalCondition = true;
            StartCoroutine("GoalApprovedAnimation");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // クリア可能か確認する
            if (goalCondition)
            {
                // ステージクリアの処理
                script.StageCondition("StageClear");
            }
            else
            {
                // マテリアルを変更
                materials[0] = denyedColor;
                this.GetComponent<Renderer>().sharedMaterials = materials;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (goalCondition)
            {
                // マテリアルを変更
                materials[0] = approvedColor;
                this.GetComponent<Renderer>().sharedMaterials = materials;
            }
            else
            {
                // マテリアルを変更
                materials[0] = normalColor;
                this.GetComponent<Renderer>().sharedMaterials = materials;
            }
        }
    }

    // ゴール可能のアニメーション
    private IEnumerator GoalApprovedAnimation()
    {
        // マテリアルを変更
        materials[0] = approvedColor;
        this.GetComponent<Renderer>().sharedMaterials = materials;
        // 飛び出すアニメーション
        Vector3 CurrentPos = this.transform.localPosition; 
        Vector3 ArrivePos = this.transform.localPosition;
        ArrivePos.z = -0.8f;
        float Varitate = (ArrivePos.z) / 8f;
        for (int i = 0; i < 8; i++)
        {
            CurrentPos.z += Varitate;
            this.transform.localPosition = CurrentPos;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
