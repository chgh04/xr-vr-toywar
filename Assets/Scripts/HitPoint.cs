using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public HPUIController hpUI;
    public int maxHP;
    private int _HP;

    BoxCollider col;
    CapsuleCollider cCol = null;
    TurretDisabled turretDisabled;
    
    public int autoHealingPoint = 0;
    public float autoHealingTime = 0;
    float currentTime;

    public int HP
    {
        get 
        { 
            if (_HP < 0)
            {
                return 0;
            }
            else
            {
                return _HP;
            } 
        }
        set 
        {   
            _HP = value;
            if (_HP > 0)
            {
                if (hpUI != null)
                {   
                    if (_HP > maxHP) _HP = maxHP;
                    float ratio = (float)_HP / maxHP;
                    hpUI.setHP(ratio);
                }
            }
            else
            {
                hpUI.setHP(0);
                Die();
            }
        }
    }

    private void Start()
    {
        //_HP = maxHP; // for test or player
    }

    private void OnEnable()
    {
        _HP = maxHP; // for objectpolling
        col = GetComponent<BoxCollider>();
        if (col != null)
        { 
            col.enabled = true;
        }
        else if (col == null) 
        {
            cCol = GetComponent<CapsuleCollider>();
            cCol.enabled = true;
        }

        turretDisabled = GetComponent<TurretDisabled>();

        currentTime = 0;
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player") || gameObject.CompareTag("Turret"))
        {
            if (maxHP > HP)
            {
                currentTime += Time.deltaTime;

                if (currentTime > autoHealingTime)
                {   
                    if (gameObject.CompareTag("Player") && HP <= 0)
                    {
                        return;
                    }
                    Healling();
                    currentTime = 0;
                }
            }
        }

        if (turretDisabled != null)
        {
            if (turretDisabled.TurretBreak == true)
            {
                if (HP >= maxHP/2)
                {
                    turretDisabled.TurretFix();
                }
            }
        }

        if (GameManager.instance.gameInStage == false)
        {   
            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Turret"))
            {
                HP = maxHP;
            }
        }
    }

    void Healling()
    {
        HP += autoHealingPoint;
        float ratio = (float)HP / maxHP;
        hpUI.setHP(ratio);
    }

    void Die()
    {
        if (gameObject.CompareTag("Player"))
        {   
            if (GameManager.instance.isPlayerGameOver == false)
            {
                GameManager.instance.GameOver();
            }
        }
        else if(gameObject.CompareTag("Turret"))
        {
            if (turretDisabled.TurretBreak == false)
            {
                turretDisabled.TurretBreakdown();
            }
        }
        else if(gameObject.CompareTag("Barricade"))
        {
            gameObject.SetActive(false);
        }
        else if(gameObject.CompareTag("Enemy") || gameObject.CompareTag("Friend"))
        {
            gameObject.GetComponent<NPCController>().Die();
            if (col != null) col.enabled = false;
        }
    }
}
