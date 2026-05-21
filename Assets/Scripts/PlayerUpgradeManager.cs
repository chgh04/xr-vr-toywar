using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeManager : MonoBehaviour
{
    public GameObject[] playerWeapones = new GameObject[3];
    
    public PlayerScoreManager playerScoreManager;
    public Canvas upgradeUI;
    public Transform upgradeModuleLoc;

    public Text weaponeUpText;
    public Text HPUpText;
    public Text availablePointText;

    public Text buttonWeaponeUpText;
    public Text buttonHPUpText;

    GameObject selectedWeapone;
    int _HPUpgradeStatus;
    int weaponeUpgradeStatus;

    public HitPoint playerHitPoint;

    public int[] playerUpgradeCosts;

    public int HPUpgradeStatus
    {
        get { return _HPUpgradeStatus; }
        set 
        {  
            _HPUpgradeStatus = value;
            if (_HPUpgradeStatus > 2) _HPUpgradeStatus = 2;
        }
    }

    private void OnEnable()
    {
        selectedWeapone = playerWeapones[0];
        playerWeapones[1].SetActive(false);
        playerWeapones[2].SetActive(false);

        _HPUpgradeStatus = 0;
        weaponeUpgradeStatus = 0;
        upgradeUI.gameObject.SetActive(false);
    }

    public void PlayerUpgradeUI()
    {
        Vector3 moduleLock = upgradeModuleLoc.position;

        upgradeUI.gameObject.SetActive(true);             // UI설정
        upgradeUI.transform.position = new Vector3(moduleLock.x, moduleLock.y + 0.9f, moduleLock.z + 0.4f);
        upgradeUI.transform.LookAt(Camera.main.transform.position);
        upgradeUI.transform.Rotate(0, 180, 0);

        availablePointText.text = "사용가능 포인트: " + playerScoreManager.PlayerScore.ToString();

        if (weaponeUpgradeStatus < 2)
        {
            buttonWeaponeUpText.text = playerUpgradeCosts[weaponeUpgradeStatus].ToString();
        }
        else
        {
            buttonWeaponeUpText.text = "";
        }

        if (weaponeUpgradeStatus == 0)
        {
            weaponeUpText.text = "플레이어의 무기를 강화합니다.\n공격력이 증가합니다.";
        }
        else if (weaponeUpgradeStatus == 1)
        {
            weaponeUpText.text = "공격력이 크게 증가합니다.\n공격속도가 크게 증가합니다.";
        }
        else if (weaponeUpgradeStatus == 2)
        {
            weaponeUpText.text = "최대 강화 상태입니다.";
        }


        if (HPUpgradeStatus < 2)
        {
            buttonHPUpText.text = playerUpgradeCosts[_HPUpgradeStatus + 2].ToString();
        }
        else
        {
            buttonHPUpText.text = "";
        }

        if (HPUpgradeStatus == 0)
        {
            HPUpText.text = "플레이어의 체력이 증가하고\n서서히 회복됩니다.\n100 -> 200";
        }
        else if (HPUpgradeStatus == 1)
        {
            HPUpText.text = "플레이어의 체력이 크게 증가하고\n회복속도가 빨라집니다.\n200 -> 300";
        }
        else if (HPUpgradeStatus == 2)
        {
            HPUpText.text = "최대 강화 상태입니다.";
        }
    }

    public void weaponeUpgrade()
    {

        if (weaponeUpgradeStatus == 2) return;

        if (playerScoreManager.PlayerScore < playerUpgradeCosts[weaponeUpgradeStatus])
        {
            buttonWeaponeUpText.text = "포인트 부족!";
            return;
        }

        selectedWeapone.SetActive(false);
        selectedWeapone = playerWeapones[weaponeUpgradeStatus + 1];
        selectedWeapone.SetActive(true);

        playerScoreManager.PlayerScore -= playerUpgradeCosts[weaponeUpgradeStatus];
        weaponeUpgradeStatus++;
        upgradeUI.gameObject.SetActive(false);
    }

    public void HPUpgrade()
    {
        if (HPUpgradeStatus == 2) return;

        if (playerScoreManager.PlayerScore < playerUpgradeCosts[HPUpgradeStatus + 2])
        {
            buttonWeaponeUpText.text = "포인트 부족!";
            return;
        }

        if (HPUpgradeStatus == 0)
        {
            playerHitPoint.maxHP = 200;
            playerHitPoint.HP = 200;
            playerHitPoint.autoHealingPoint = 5;
            playerHitPoint.autoHealingTime = 5;
        }
        else if (HPUpgradeStatus == 1)
        {
            playerHitPoint.maxHP = 300;
            playerHitPoint.HP = 300;
            playerHitPoint.autoHealingPoint = 5;
            playerHitPoint.autoHealingTime = 3;
        }

        playerScoreManager.PlayerScore -= playerUpgradeCosts[HPUpgradeStatus + 2];
        HPUpgradeStatus++;
        upgradeUI.gameObject.SetActive(false);
    }

    public void deleteUI()
    {
        upgradeUI.gameObject.SetActive(false);
    }
}
