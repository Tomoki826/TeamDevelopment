using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationAround : MonoBehaviour
{
    public float forwardSpeed = 1f;     // 直進の速さ
    public float rotationSpeed = 1f;    // 回転の速さ
    public Destination script;          // 目的地のスクリプト

    private Rigidbody targetRigidbody;
    private Vector3 originalPosition;
    private bool destinationAnimated;

    void Start()
    {
        targetRigidbody = GetComponent<Rigidbody>();
        originalPosition = this.transform.position;
        destinationAnimated = false;
    }

    void FixedUpdate()
    {
        if (!destinationAnimated && !script.DestinationReached())
        {
            int rnd = Random.Range(0, 100);
            if (rnd == 0)
            {
                StartCoroutine(RotateAnimation(Random.Range(-180, 180), rotationSpeed));
            }
            else if (rnd == 1)
            {
                Vector3 reachPos = this.transform.up * Random.Range(1f, 2f);
                if (Vector3.Distance(originalPosition, this.transform.position + reachPos) <= 2f)
                {
                    StartCoroutine(MoveForwardAnimation(this.transform.position + reachPos, forwardSpeed));
                }
            }
        }
        else if (script.DestinationReached())
        {
            StopCoroutine("RotateAnimation");
            StopCoroutine("MoveForwardAnimation");
        }
    }

    // 回転するアニメーション
    private IEnumerator RotateAnimation(int angle, float speed)
    {
        destinationAnimated = true;
        Quaternion rot = Quaternion.Euler(0, 0, Mathf.Sign(angle));
        for (int turn = 0; turn < Mathf.Abs(angle); turn++)
        {
            this.transform.rotation *= rot;
            yield return new WaitForSeconds(Time.deltaTime * speed);
        }
        destinationAnimated = false;
    }

    // 移動するアニメーション
    private IEnumerator MoveForwardAnimation(Vector3 targetPos, float speed)
    {
        destinationAnimated = true;
        Vector3 pos = targetPos - this.transform.position;
        Vector3 vel = pos.normalized * speed;
        targetRigidbody.velocity = vel;
        yield return new WaitForSeconds(pos.magnitude / vel.magnitude);
        targetRigidbody.velocity = Vector3.zero;
        destinationAnimated = false;
    }
}