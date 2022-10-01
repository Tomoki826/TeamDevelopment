using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCameraController : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        Vector3 Cpos = this.transform.position;
        Vector3 Ppos = player.transform.position;
        this.transform.position = new Vector3(Ppos.x, Ppos.y, Cpos.z);
    }

    void FixedUpdate()
    {
        Vector3 Cpos = this.transform.position;
        Vector3 Ppos = player.transform.position;
        this.transform.position = new Vector3(Ppos.x, Ppos.y, Cpos.z);
    }
}
