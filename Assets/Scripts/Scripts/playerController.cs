using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float forwardSpeed = 7f;
    public float rotateSpeed = 4f;
    public Rigidbody playerRigidbody;

    public bool playerMove = false;
    private int moveDirection = 0;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerMove = false;
    }

    void FixedUpdate()
    {
        // プレイヤーの並進移動
        Vector3 moveVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            playerMove = true;
            moveDirection = 1;
            moveVelocity = transform.forward * forwardSpeed;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            playerMove = true;
            moveDirection = -1;
            moveVelocity = transform.forward * forwardSpeed * moveDirection;
        }
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S))
        {
            playerMove = false;
        }
        playerRigidbody.velocity = moveVelocity;

        // プレイヤーの回転移動
        Vector3 rotateDir = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            rotateDir.y = rotateSpeed * -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotateDir.y = rotateSpeed;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 100f);
    }
}
