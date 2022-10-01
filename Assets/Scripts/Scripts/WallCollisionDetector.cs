using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionDetector : MonoBehaviour
{
    public GameObject wallSoundPrefab;  // 壁の衝突音のプレハブ
    public GameObject moveWallSoundPrefab;  // 動く壁の衝突音のプレハブ
    public playerController script;     // PlayerControllerのスクリプト
    private Rigidbody playerRigidbody;  // 主人公のRigidbody
    private float timeElapesd;

    void Start()
    {
        playerRigidbody = this.gameObject.GetComponent<Rigidbody>();
        timeElapesd = 0f;
    }

    // 壁と衝突したか判定する
    void OnCollisionStay(Collision other)
    {
        timeElapesd += Time.deltaTime;
        if (timeElapesd >= 0.1f)
        {
            timeElapesd = 0f;
            if (script.playerMove)
            {
                if (other.gameObject.CompareTag("Wall"))
                {
                    WallSoundGenerator(other, wallSoundPrefab);
                }
                else if (other.gameObject.CompareTag("MoveWall"))
                {
                    WallSoundGenerator(other, moveWallSoundPrefab);
                }
            }
        }
    }

    // 壁の衝突音を立体音響で表現
    void WallSoundGenerator(Collision other, GameObject soundGenerator)
    {
        // 衝突座標の平均値を出す
        Vector3 averagePos = new Vector3(0f, 0f, 0f);
        foreach (ContactPoint point in other.contacts)
        {
            averagePos += point.point;
        }
        averagePos /= (float) other.contacts.Length;
        // プレハブからインスタンスを作成
        GameObject cloneObject = Instantiate(soundGenerator, averagePos, Quaternion.identity);
        StartCoroutine(CloneDelete(cloneObject));
    }

    // インスタンスを消す
    private IEnumerator CloneDelete(GameObject cloneObject)
    {
        yield return new WaitForSeconds(1f);
        Destroy(cloneObject);
    }
}