using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayEffect : MonoBehaviour
{
    public RectTransform canvas;

    void Start()
    {
        //Imageの大きさを解像度に合わせる
        RectTransform image = GetComponent<RectTransform>();
        image.sizeDelta = canvas.sizeDelta;
    }
}
