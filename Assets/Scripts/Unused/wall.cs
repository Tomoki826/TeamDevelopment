using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    //OnTriggerEnter関数
    //接触したオブジェクトが引数otherとして渡される
    void OnTriggerEnter(Collider other)
    {
        //接触したオブジェクトのタグ
        if (other.CompareTag("range"))
        {
            //オブジェクトの色を赤に変更する
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //接触したオブジェクトのタグ
        if (other.CompareTag("range"))
        {
            //オブジェクトの色を赤に変更する
            GetComponent<Renderer>().material.color = Color.black;
        }
    }

}