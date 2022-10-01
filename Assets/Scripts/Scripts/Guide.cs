using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public GameObject firstCheckPointOrGoal;
    public GameObject player;
    public bool isUse = true;

    private GameObject currentCheckPoint;
    private bool guideTransparent;
    private SpriteRenderer sprite;

    void Start()
    {
        if (isUse)
        {
            guideTransparent = false;
            currentCheckPoint = firstCheckPointOrGoal;
            sprite = this.GetComponent<SpriteRenderer>();
            StartCoroutine(GuideFadeIn());
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        Transform guide_Transform = this.transform;
        Transform cpoint_Transform = currentCheckPoint.transform;
        Transform player_Transform = player.transform;

        Vector3 Gpos = guide_Transform.position;
        Vector3 Cpos = cpoint_Transform.position;
        Vector3 Ppos = player_Transform.position;

        Vector3 pos = new Vector3(Cpos.x - Ppos.x, Cpos.y - Ppos.y, 0);
        float angle = Vector3.SignedAngle(new Vector3(0, 1, 0), pos, new Vector3(0, 0, 1));
        float length = pos.magnitude;
        Gpos = new Vector3(Ppos.x + pos.x * 4f / length, Ppos.y + pos.y * 4f / length, Gpos.z);

        Vector3 GuideVector = new Vector3(Cpos.x - Gpos.x, Cpos.y - Gpos.y, 0);
        if (GuideVector.magnitude <= 4f)
        {
            guideTransparent = true;
            Color32 color = sprite.color;
            if (color.a - 4 < 0)
            {
                color.a = 0;
            }
            else
            {
                color.a -= 4;
            }
            sprite.color = color;
        }
        else if (guideTransparent)
        {
            Color32 color = sprite.color;
            if (color.a + 8 > 255)
            {
                color.a = 255;
                guideTransparent = false;
            }
            else
            {
                color.a += 8;
            }
            sprite.color = color;
        }

        transform.position = Gpos;
        transform.eulerAngles = new Vector3(0, 0, angle);

    }

    //チェックポイントのオブジェクトをアップデートする
    public void checkPointObjectUpdate(GameObject newObject)
    {
        currentCheckPoint = newObject;
    }

    //ガイドをフェードインする
    private IEnumerator GuideFadeIn()
    {
        Color32 color = sprite.color;
        color.a = 0;
        sprite.color = color;
        yield return new WaitForSeconds(Time.deltaTime);
        for (int i = 0; i < 32; i++)
        {
            if (guideTransparent)
            {
                yield break;
            }
            color = sprite.color;
            if (color.a + 8 > 255)
            {
                color.a = 255;
            }
            else
            {
                color.a += 8;
            }
            sprite.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}