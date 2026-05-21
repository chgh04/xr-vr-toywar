using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BuildManager : MonoBehaviour
{
    public InputActionReference leftTriggerButton;
    public XRRayInteractor leftRayInteractor;

    public GameObject[] buildAvailable;
    public GameObject buildPlaceLocator;
    public Canvas buildUI;
    public Text buildCost_Cat;
    public Text buildCost_Squirrel;
    public Text buildCost_Toybox;

    Vector3 buildLoc = Vector3.zero;

    GameObject upgradeTarget;
    public Canvas upgradeUI;
    public Text upgradeName;
    public Text upgradeDescription;
    public Text upgradeCostUI;
    int upgradableNum;

    public int[] buildCosts;
    public Text availablePlayerCostInBuild;
    public Text availablePlayerCostInUpgrade;

    public PlayerScoreManager playerScoreManager;
    public PlayerUpgradeManager playerUpgradeManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnEnable()
    {
        leftTriggerButton.action.performed += OnLeftTriggerPressedInBuild;
        buildPlaceLocator.SetActive(false);
        buildUI.gameObject.SetActive(false);
        upgradeUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLeftTriggerPressedInBuild(InputAction.CallbackContext context)
    {   
        if (GameManager.instance.gameInStage == true)
        {
            return;
        }

        if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
        {
            //deleteAllUI();

            if (hitInfo.transform.CompareTag("Map")) 
            {
                BuildUI(hitInfo);
            }
            else if (hitInfo.transform.CompareTag("Turret"))
            {
                UpgradeUI(hitInfo);
            }
            else if (hitInfo.transform.CompareTag("UpgradeModule"))
            {
                playerUpgradeManager.PlayerUpgradeUI();
            }
        }
    }

    void BuildUI(RaycastHit hitInfo)
    {
        buildLoc = hitInfo.point;   // 설치장소 지정

        buildPlaceLocator.SetActive(true);              // 설치장소 표시
        buildPlaceLocator.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.1f, hitInfo.point.z);   
        buildPlaceLocator.transform.forward = hitInfo.normal;

        buildUI.gameObject.SetActive(true);             // UI설정
        buildUI.transform.position = new Vector3(buildLoc.x, buildLoc.y + 0.9f, buildLoc.z);
        buildUI.transform.LookAt(Camera.main.transform.position);
        buildUI.transform.Rotate(0, 180, 0);

        buildCost_Cat.text = buildCosts[0].ToString();
        buildCost_Squirrel.text = buildCosts[1].ToString();
        buildCost_Toybox.text = buildCosts[2].ToString();
        availablePlayerCostInBuild.text = "사용가능 포인트: " + playerScoreManager.PlayerScore.ToString();
    }

    void UpgradeUI(RaycastHit hitInfo)
    {
        upgradableNum = -1;

        upgradeTarget = hitInfo.transform.gameObject;
        TurretType tType = upgradeTarget.GetComponent<TurretTypeComponent>().turretType;
        Vector3 upgradLoc = upgradeTarget.transform.position;

        upgradeUI.gameObject.SetActive(true);
        upgradeUI.transform.position = new Vector3(upgradLoc.x, upgradLoc.y + 1.3f, upgradLoc.z);
        upgradeUI.transform.LookAt(Camera.main.transform.position);
        upgradeUI.transform.Rotate(0, 180, 0);

        availablePlayerCostInUpgrade.text = "사용가능 포인트: " + playerScoreManager.PlayerScore.ToString();

        if (tType == TurretType.cat_Level1)
        {
            upgradeName.text = "고양이 Level2";
            upgradeDescription.text = "공격속도와 공격력이 증가합니다.\n사격 후 대기시간이 있습니다.";

            upgradableNum = 3;
            upgradeCostUI.text = buildCosts[upgradableNum].ToString();
        }
        else if (tType == TurretType.cat_Level2)
        {
            upgradeName.text = "고양이 Level3";
            upgradeDescription.text = "공격속도와 공격력이 크게 증가합니다.\n사격 대기시간이 감소합니다.";

            upgradableNum = 6;
            upgradeCostUI.text = buildCosts[upgradableNum].ToString();
        }
        else if (tType == TurretType.squirrel_Level1)
        {
            upgradeName.text = "다람쥐 Level2";
            upgradeDescription.text = "공격범위, 공격력이 증가합니다.\n폭탄의 범위가 조금 넓어집니다.";

            upgradableNum = 4;
            upgradeCostUI.text = buildCosts[upgradableNum].ToString();
        }
        else if (tType == TurretType.squirrel_Level2)
        {
            upgradeName.text = "다람쥐 Level3";
            upgradeDescription.text = "다람쥐가 강력한 로켓을 쏩니다.\n공격범위, 공격력, 공격속도가 크게 증가합니다.";

            upgradableNum = 7;
            upgradeCostUI.text = buildCosts[upgradableNum].ToString();
        }
        else if (tType == TurretType.toybox_Level1)
        {
            upgradeName.text = "장난감상자 Level2";
            upgradeDescription.text = "강화된 로봇을 2기 소환합니다.\n로봇의 체력, 공격력, 속도가 증가합니다.";

            upgradableNum = 5;
            upgradeCostUI.text = buildCosts[upgradableNum].ToString();
        }
        else if (tType == TurretType.toybox_Level2)
        {
            upgradeName.text = "장난감상자 Level3";
            upgradeDescription.text = "강력한 로봇을 3기 소환합니다.\n로봇의 체력, 공격력, 속도가 크게 증가합니다.";

            upgradableNum = 8;
            upgradeCostUI.text = buildCosts[upgradableNum].ToString();
        }
        else if (tType == TurretType.toybox_Level3 || tType == TurretType.squirrel_Level3 || tType == TurretType.cat_Level3)
        {
            upgradeName.text = "";
            upgradeDescription.text = "더 이상 강화할 수 없습니다.";
            upgradeCostUI.text = "";
            upgradableNum = -1;
        }
    }

    public void BuildCat_Level1()
    {   
        if (playerScoreManager.PlayerScore < buildCosts[0])
        {
            buildCost_Cat.text = "포인트 부족!";
            return;
        }

        GameObject newTurret = Instantiate(buildAvailable[0], this.buildLoc, Quaternion.identity);
        newTurret.SetActive(true);
        buildPlaceLocator.SetActive(false);
        buildUI.gameObject.SetActive(false);
        playerScoreManager.PlayerScore -= buildCosts[0];
    }

    public void BuildSquirrel_Level1()
    {
        if (playerScoreManager.PlayerScore < buildCosts[1])
        {
            buildCost_Squirrel.text = "포인트 부족!";
            return;
        }

        GameObject newTurret = Instantiate(buildAvailable[1], this.buildLoc, Quaternion.identity);
        newTurret.SetActive(true);
        buildPlaceLocator.SetActive(false);
        buildUI.gameObject.SetActive(false);
        playerScoreManager.PlayerScore -= buildCosts[1];
    }

    public void BuildToyBox_Level1()
    {
        if (playerScoreManager.PlayerScore < buildCosts[2])
        {
            buildCost_Toybox.text = "포인트 부족!";
            return;
        }

        GameObject newTurret = Instantiate(buildAvailable[2], this.buildLoc, Quaternion.identity);
        newTurret.SetActive(true);
        buildPlaceLocator.SetActive(false);
        buildUI.gameObject.SetActive(false);
        playerScoreManager.PlayerScore -= buildCosts[2];
    }

    public void BuildUpgrade()
    {
        if (upgradableNum < 0) return;

        if (playerScoreManager.PlayerScore < buildCosts[upgradableNum])
        {
            upgradeCostUI.text = "포인트 부족!";
            return;
        }

        Transform upgradeLoc = upgradeTarget.transform;
        upgradeTarget.SetActive(false);

        GameObject upTurret = Instantiate(buildAvailable[upgradableNum], upgradeLoc.position, upgradeLoc.rotation);
        playerScoreManager.PlayerScore -= buildCosts[upgradableNum];

        upgradeUI.gameObject.SetActive(false);
        upgradableNum = -1;
    }

    public void deleteAllUI()
    {
        buildPlaceLocator.SetActive(false);
        buildUI.gameObject.SetActive(false);
        upgradeUI.gameObject.SetActive(false);
        playerUpgradeManager.deleteUI();
    }
}
