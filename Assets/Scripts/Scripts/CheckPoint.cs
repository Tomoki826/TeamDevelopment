using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool first_checkpoint;
    public GameObject next_check_point;
    public bool last_check_point;
    public GameObject goal;
    public Material reachedColor;

    private GameObject guide;
    private Guide guideScript;
    private bool checkPointReached;
 
    void Start()
    {
        checkPointReached = false;
        // ガイドのスクリプトを読み込む
        guide = GameObject.Find("PlayerGuide");
        guideScript = guide.GetComponent<Guide>();
        // チェックポイントをオン・オフするか
        if (!first_checkpoint)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !checkPointReached) 
        {
            checkPointReached = true;
            if (!last_check_point)
            {
                guideScript.checkPointObjectUpdate(next_check_point);
                next_check_point.SetActive(true);
            }
            else 
            {
                guideScript.checkPointObjectUpdate(goal);
            }
            StartCoroutine("CheckPointReachAnimation");
        }
    }

    // チェックポイントに到着しているか
    public bool CheckPointCheck()
    {
        return checkPointReached;
    }

    // 到着後のアニメーション
    private IEnumerator CheckPointReachAnimation()
    {
        // マテリアルを変更
        Material[] materials = this.GetComponent<Renderer>().sharedMaterials;
        materials[0] = reachedColor;
        this.GetComponent<Renderer>().sharedMaterials = materials;
        // 押されたアニメーション
        Vector3 CurrentPos = this.transform.localPosition; 
        Vector3 ArrivePos = this.transform.localPosition;
        ArrivePos.z = 0.8f;
        float Varitate = (ArrivePos.z) / 5f;
        for (int i = 0; i < 5; i++)
        {
            CurrentPos.z += Varitate;
            this.transform.localPosition = CurrentPos;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
