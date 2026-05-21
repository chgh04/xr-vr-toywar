using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public XRRayInteractor leftRay;
    public GameObject mainLight;
    string currentSceneName;

    public bool gameInStage;
    public bool stopSpawnEnemy;
    public bool isPlayerGameOver;
    public BuildManager buildManager;
    public PlayerScoreManager playerScoreManager;
    public PlayerWeaponeController[] playerWeaponeController;
    bool isGameEnd;

    public Canvas noticeCanvas;
    public Canvas stageInfoCanvas;
    public Text stageInfoText;
    public Text stageInfoText_sub;
    //public Text playerPointText;
    public Text showRemainEnemyCountText;
    public Canvas gameEndCanvas;
    public Text entireScore;
    public Canvas gameOverCanvas;

    public GameObject[] spawnPoint_Stage1;
    public GameObject[] spawnPoint_Stage2;
    public GameObject[] spawnPoint_Stage3;
    public GameObject[] spawnPoint_Stage4;
    public GameObject[] spawnPoint_Stage5;
    public GameObject[] spawnPoint_Stage6;
    public GameObject[] spawnPoint_Stage7;

    public int[] StageKillRequirement;
    int stageNum;
    int killCount;
    int remainEnemyCount;

    public int testStageNum = 1;
    public int startScore;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageNum = 0;
        stageNum = testStageNum-1;
        killCount = 0;
        gameInStage = false;
        isPlayerGameOver = false;
        isGameEnd = false;
        playerScoreManager.PlayerScore = startScore;
        gameEndCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        currentSceneName = SceneManager.GetActiveScene().name;

        StageEnd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StageStart()
    {
        deleteAllUI();
        gameInStage = true;
        stopSpawnEnemy = false;
        leftRay.gameObject.SetActive(false);
        mainLight.gameObject.SetActive(false);

        showRemainEnemyCountText.gameObject.SetActive(true);
        showRemainEnemyCountText.text = "남은 적: " + StageKillRequirement[stageNum - 1];
        killCount = 0;
        remainEnemyCount = 0;

        if (stageNum == 1)
        {
            foreach(GameObject point in spawnPoint_Stage1)
            {
                point.SetActive(true);
            }
        }
        else if (stageNum == 2)
        {
            foreach (GameObject point in spawnPoint_Stage1)
            {
                point.SetActive(false);
            }

            foreach (GameObject point in spawnPoint_Stage2)
            {
                point.SetActive(true);
            }
        }

        else if (stageNum == 3)
        {
            foreach (GameObject point in spawnPoint_Stage2)
            {
                point.SetActive(false);
            }

            foreach (GameObject point in spawnPoint_Stage3)
            {
                point.SetActive(true);
            }
        }

        else if (stageNum == 4)
        {
            foreach (GameObject point in spawnPoint_Stage3)
            {
                point.SetActive(false);
            }

            foreach (GameObject point in spawnPoint_Stage4)
            {
                point.SetActive(true);
            }
        }
        else if (stageNum == 5)
        {
            foreach (GameObject point in spawnPoint_Stage4)
            {
                point.SetActive(false);
            }

            foreach (GameObject point in spawnPoint_Stage5)
            {
                point.SetActive(true);
            }
        }
        else if (stageNum == 6)
        {
            foreach (GameObject point in spawnPoint_Stage5)
            {
                point.SetActive(false);
            }

            foreach (GameObject point in spawnPoint_Stage6)
            {
                point.SetActive(true);
            }
        }
        else if (stageNum == 7)
        {
            foreach (GameObject point in spawnPoint_Stage6)
            {
                point.SetActive(false);
            }

            foreach (GameObject point in spawnPoint_Stage7)
            {
                point.SetActive(true);
            }
        }

    }

    public void StageEnd()
    {
        stageNum++;
        mainLight.gameObject.SetActive(true);

        if (stageNum == 8)
        {
            EndOfTheGame();
            return;
        }

        UIUpdate();
        gameInStage = false;
        if (stopSpawnEnemy == false) stopSpawnEnemy = true;
        leftRay.gameObject.SetActive(true);

        showRemainEnemyCountText.gameObject.SetActive(false);
        killCount = 0;
        remainEnemyCount = 0;
    }

    public void AddKillCount()
    {
        if (isGameEnd == true) return;

        killCount++;

        showRemainEnemyCountText.text = "남은 적: " + (StageKillRequirement[stageNum - 1] - killCount).ToString();

        if (killCount >= StageKillRequirement[stageNum - 1])
        {
            StageEnd();
        }

    }

    public void AddRemainEnemyCount()
    {
        if (isGameEnd == true) return;

        remainEnemyCount++;

        if (remainEnemyCount >= StageKillRequirement[stageNum - 1])
        {
            stopSpawnEnemy = true;
        }
    }

    void deleteAllUI()
    {
        buildManager.deleteAllUI();
        stageInfoCanvas.gameObject.SetActive(false);
        noticeCanvas.gameObject.SetActive(false);
    }

    void UIUpdate()
    {
        stageInfoCanvas.gameObject.SetActive(true);
        noticeCanvas.gameObject.SetActive(true);

        stageInfoText.text = stageNum.ToString() + "일째 밤";
        //playerPointText.text = "현재 포인트: " + playerScoreManager.PlayerScore.ToString();

        if (stageNum == 1)
        {
            stageInfoText_sub.text = "밤이되면 괴물들이 당신을 습격해 옵니다!\n" +
                "장난감 총을 들고, 바닥에 인형을 설치해 스스로를 보호하세요\n" +
                "인형 설치: 왼쪽 컨트롤러로 바닥을 조준하고 트리거 버튼 누르기\n" +
                "장비 강화: 왼쪽 컨트롤러로 침대 옆 상자를 조준하고 트리거 버튼 누르기";
        }
        else if (stageNum == 2)
        {
            stageInfoText_sub.text = "더 많은 괴물들이 습격해 옵니다!\n" +
                "인형들을 강화하고 더 좋은 장난감을 장착해 괴물들을 막으세요\n" +
                "인형 강화: 왼쪽 컨트롤러로 인형을 조준하고 트리거 버튼 누르기\n" +
                "장비 강화: 왼쪽 컨트롤러로 침대 옆 상자를 조준하고 트리거 버튼 누르기";
        }
        else if (stageNum == 3)
        {
            stageInfoText_sub.text = "더 많고 강화된 괴물들이 습격해 옵니다!\n" +
                "창문을 통해 침입하는 유령들이 등장합니다!\n" +
                "인형과 상자가 파괴되면, 인형과 상자의 회복속도가 크게 증가합니다.\n" +
                "절반 이상의 체력이 회복되면 다시 가동합니다.";
        }
        else if (stageNum == 4)
        {
            stageInfoText_sub.text = "강화된 괴물들이 습격해 옵니다!\n" +
                "창문을 통해 강화된 유령이 침입합니다.\n" +
                "인형이 파괴되면 인형의 회복속도가 크게 증가합니다.\n" +
                "절반 이상의 체력이 회복되면 다시 가동합니다.";
        }
        else if (stageNum == 5)
        {
            stageInfoText_sub.text = "!!강력한 괴물들이 습격해 옵니다!!\n" +
                "무시무시한 검은 미라가 등장합니다....";
        }
        else if (stageNum == 6)
        {
            stageInfoText_sub.text = "!!강력한 괴물들이 습격해 옵니다!!\n" +
                "으스스한 유령이 등장합니다....";
        }
        else if (stageNum == 7)
        {
            stageInfoText_sub.text = "마지막 날 밤이 되었습니다!\n" +
                "커다란 로봇이 등장합니다....";
        }
    }

    void EndOfTheGame()
    {
        isGameEnd = true;
        gameEndCanvas.gameObject.SetActive(true);
        gameInStage = false;
        if (stopSpawnEnemy == false) stopSpawnEnemy = true;
        leftRay.gameObject.SetActive(true);

        showRemainEnemyCountText.gameObject.SetActive(false);
        killCount = 0;
        remainEnemyCount = 0;
        entireScore.text = "당신의 점수! : " + playerScoreManager.OverallScore.ToString();
    }

    public void GameOver()
    {
        isPlayerGameOver = true;
        gameOverCanvas.gameObject.SetActive(true);
        leftRay.gameObject.SetActive(true);

        foreach (PlayerWeaponeController weapone in playerWeaponeController)
        {
            if (weapone.gameObject.activeSelf == true)
            {
                weapone.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(currentSceneName);
    }
}
