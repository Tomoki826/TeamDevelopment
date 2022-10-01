using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageController : MonoBehaviour
{
    [Header("General")]
    public GameObject playerObject;     // プレイヤーのオブジェクト
    public GameObject zoomCamera;       // カメラのGameObject
    public GameObject timer;            // タイマーのGameObject
    public GameObject destinationCount; // 目的地カウントのGameObject
    public Image displayEffect;         // ディスプレイエフェクトのImage
    public static bool skipStageIntro;  // ステージ名表示をスキップするか

    [Header("Destination")]
    public string destinationTagName;   // 目的地のタグ名

    [Header("StageStart")]
    public GameObject stageText;        // ステージテキストのGameObject
    public TextMeshProUGUI stageNumber; // ステージ番号のTextMeshPro
    public TextMeshProUGUI stageName;   // ステージ名のTextMeshPro

    [Header("StageClear")]
    public bool existNextStage = true;  // 次のステージが存在するか
    public string nextSceneName;        // 次のステージの名前
    public Material playerMaterial;     // プレイヤーのマテリアル
    public GameObject goalObject;       // ゴールオブジェクト
    public GameObject goalLightPrefab;  // ゴールライトのPrefab
    public AudioSource goalAudio1;      // ゴールのオーディオ
    public AudioSource goalAudio2;
    public AudioSource goalAudio3;

    [Header("TimeUp")]
    public GameObject DeathBallPrefab;  // DeathBallのPrefab
    public AudioSource timeUpAudio1;    // タイムアップのオーディオ
    public AudioSource timeUpAudio2;

    private GameObject[] destinationObjects;    // 複数の目的地のゲームオブジェクトを格納
    private int destinationTotalCounts;         // 目的地の総数をカウント
    private int destinationReachedCounts = 0;   // 到達した目的地をカウント

    private bool reachedFlag = false;
    private int stageStatus = 0; // ステージの状態

    // コンストラクタ処理
    void Start()
    {
        destinationObjects = GameObject.FindGameObjectsWithTag(destinationTagName);
        destinationTotalCounts = destinationObjects.Length;
        stageText.SetActive(false);
        if (!skipStageIntro)
        {
            StartCoroutine("StageStart");
        }
        else
        {
            StartCoroutine("Restart");
        }
    }

    void FixedUpdate()
    {
        // ステージクリア
        if (stageStatus == 1)
        {
            StartCoroutine("StageClear");
        }
        // タイムアップ
        else if (stageStatus == 2)
        {
            StartCoroutine("TimeUp");
        }
    }

    // ステージ状態を変化させる
    public void StageCondition(string status)
    {
        // ステージクリア
        if (status == "StageClear")
        {
            stageStatus = 1;
        }
        // タイムアップ
        else if (status == "TimeUp")
        {
            stageStatus = 2;
        }
    }

    // 到達した目的地を一つ増やす
    public void destinationReached()
    {
        reachedFlag = true;
        destinationReachedCounts++;
        Invoke("ReachedFlagOff", Time.deltaTime);
    }

    // 到達フラグをオフにする
    private void ReachedFlagOff()
    {
        reachedFlag = false;
    }

    // 到達フラグがオンなのか?
    public bool ReachedFlagCheck()
    {
        return reachedFlag;
    }

    // 到達した目的地を返す
    public int ReachDestinationCount()
    {
        return destinationReachedCounts;
    }

    // 全ての目的地を返す
    public int TotalDestinationCount()
    {
        return destinationTotalCounts;
    }

    // 全ての目的地に到達しているか
    public bool goalCondition()
    {
        if (destinationReachedCounts >= destinationTotalCounts)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ステージを開始する
    private IEnumerator StageStart()
    {
        Time.timeScale = 0;

        // 画面を暗転
        Vector3 cameraPos = zoomCamera.transform.position;
        Vector3 stageTextPos = Vector3.zero;
        cameraPos.z -= 5f;
        Color darkColor = new Color(0, 0, 0, 1f);
        displayEffect.color = darkColor;

        // ステージテキストを表示する
        stageText.SetActive(true);
        stageTextPos = stageText.transform.localPosition;
        stageNumber.color = new Color(1f, 1f, 1f, 0);
        stageName.color = new Color(1f, 1f, 1f, 0);
        for (int i = 0; i < 150; i++)
        {
            if (i < 100)
            {
                stageNumber.color += new Color(0, 0, 0, 0.01f);
            }
            if (i >= 50)
            {
                stageName.color += new Color(0, 0, 0, 0.01f);
            }
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime * 10);

        // 画面をズームインする
        Vector3 stageTextScale = stageText.transform.localScale;
        for (int i = 0; i < 100; i++)
        {
            cameraPos.z += 0.05f;
            darkColor.a -= 0.01f;
            zoomCamera.transform.position = cameraPos;
            displayEffect.color = darkColor;
            
            stageTextPos.y += 5f;
            stageTextScale += new Vector3(0.1f, 0.1f, 0);
            stageText.transform.localPosition = stageTextPos;
            stageText.transform.localScale = stageTextScale;

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        displayEffect.color = new Color32(0, 0, 0, 0);
        stageText.SetActive(false);
        Time.timeScale = 1;
    }

    // リスタートする
    private IEnumerator Restart()
    {   
        Time.timeScale = 0;

        // 画面を暗転
        Vector3 cameraPos = zoomCamera.transform.position;
        Vector3 stageTextPos = Vector3.zero;
        cameraPos.z -= 5f;
        Color darkColor = new Color(0, 0, 0, 1f);
        displayEffect.color = darkColor;
        
        // 画面をズームインする
        Vector3 stageTextScale = stageText.transform.localScale;
        for (int i = 0; i < 100; i++)
        {
            cameraPos.z += 0.05f;
            darkColor.a -= 0.01f;
            zoomCamera.transform.position = cameraPos;
            displayEffect.color = darkColor;

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        displayEffect.color = new Color32(0, 0, 0, 0);
        Time.timeScale = 1;
    }

    // ステージクリア
    private IEnumerator StageClear()
    {
        Time.timeScale = 0;
        timer.SetActive(false);
        destinationCount.SetActive(false);

        // 効果音を演奏
        goalAudio1.Play();

        // ライトを設置
        Vector3 goalPos = goalObject.transform.position;
        Vector3 goalLightClonePos = goalObject.transform.position;
        goalLightClonePos.z = -10;
        GameObject goalLightClone = Instantiate(goalLightPrefab, goalLightClonePos, Quaternion.identity);
        Light goalLightCloneLight = goalLightClone.GetComponent<Light>();
        goalLightCloneLight.intensity = 0f;
        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        // ゴールボタンを押す・ライトを明るくする
        float goalVaritate = 0.1f;
        float intensityVaritate = 0.2f;
        for (int i = 0; i < 60; i++)
        {
            if (i < 8)
            {
                goalPos.z += goalVaritate;
                goalObject.transform.position = goalPos;
            }
            goalLightCloneLight.intensity += intensityVaritate;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        
        // 効果音を演奏
        goalAudio2.Play();
        // 主人公を透明にする・ズームアウト
        Color tmpColor = playerMaterial.color;
        Vector3 cameraPos = zoomCamera.transform.position;
        for (int i = 0; i < 180; i++)
        {
            if (i < 50)
            {
                tmpColor.a -= 0.02f;
                playerMaterial.color = tmpColor;
            }
            cameraPos.z -= 0.04f;
            zoomCamera.transform.position = cameraPos;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        
        //暗転
        goalAudio3.Play();
        Color darkColor = new Color(0, 0, 0, 1f);
        displayEffect.color = darkColor;
        tmpColor.a = 1f;
        playerMaterial.color = tmpColor;
        yield return new WaitForSecondsRealtime(5f);

        //次のステージへ
        Time.timeScale = 1;
        skipStageIntro = false;
        if (existNextStage)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

    // タイムアップ
    private IEnumerator TimeUp()
    {
        Time.timeScale = 0;
        timer.SetActive(false);
        destinationCount.SetActive(false);

        // 効果音を演奏
        timeUpAudio1.Play();
        timeUpAudio2.Play();

        Color redColor = new Color(1, 0, 0, 0);
        displayEffect.color = redColor;
        yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

        // 画面を赤くする
        for (int i = 0; i < 10; i++)
        {
            redColor.a += 0.1f;
            displayEffect.color = redColor;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        // デスボールを置く
        Instantiate(DeathBallPrefab, playerObject.transform.position, Quaternion.identity);
        for (int i = 0; i < 125; i++)
        {
            redColor.a -= 0.008f;
            displayEffect.color = redColor;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        // 暗転
        redColor.r = 0f;
        redColor.a = 0f;
        for (int i = 0; i < 100; i++)
        {
            redColor.a += 0.01f;
            displayEffect.color = redColor;
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        // 再ロードする
        Time.timeScale = 1;
        skipStageIntro = true;
        Scene loadScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadScene.name);
    }
}
