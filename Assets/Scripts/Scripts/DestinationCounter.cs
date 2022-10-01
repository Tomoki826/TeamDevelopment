using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationCounter : MonoBehaviour
{
    public StageController controllerScript;
    public RectTransform canvas;
    public Material allGetMaterial;
    private Vector3 CounterPos;

    void Start()
    {
        //座標を下端に合わせる
        Vector3 position = gameObject.transform.localPosition;
        position.y = canvas.sizeDelta.y / -2;
        gameObject.transform.localPosition = position;
        //カウンターの設定
        CounterPos = this.transform.localPosition;
        CounterPos.x = -1f * canvas.sizeDelta.x;
        this.transform.localPosition = CounterPos;
    }

    void FixedUpdate()
    {
        if (controllerScript.ReachedFlagCheck())
        {
            StartCoroutine("CounterAnimation");
        }
    }

    //増加するアニメーション
    private IEnumerator CounterAnimation()
    {
        Vector3 CurrentPos = this.transform.localPosition; 
        Vector3 ArrivePos = this.transform.localPosition;
        ArrivePos.x = -1f * canvas.sizeDelta.x * (1f - (float)controllerScript.ReachDestinationCount() / (float)controllerScript.TotalDestinationCount());
        float Varitate = (ArrivePos.x - CurrentPos.x) / 10f;
        for (int i = 0; i < 10; i++)
        {
            CurrentPos.x += Varitate;
            this.transform.localPosition = CurrentPos;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (controllerScript.goalCondition())
        {
            // マテリアルを変更
            Material[] materials = this.GetComponent<Renderer>().sharedMaterials;
            materials[0] = allGetMaterial;
            this.GetComponent<Renderer>().sharedMaterials = materials;
        }
    }
}