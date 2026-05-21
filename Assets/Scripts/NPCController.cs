using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEditor;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class NPCController : MonoBehaviour
{
    enum Enemy_1State
    {
        IDLE,
        MOVE,
        ATTACK,
        DAMAGE,
        DIE
    }

    Enemy_1State state = Enemy_1State.MOVE;

    public float detectRadius = 50f;
    public LayerMask targetLayerMask;
    float scanCooldown = 1f;

    public float moveSpeed = 2;
    public float attackDistance = 2;
    public float attackDelay = 2;
    public int attackDamage = 10;
    bool isAttacking;
    bool isDie;

    float currentTime = 0;
    NavMeshAgent agent;
    Collider[] targets;
    Transform closestTarget = null;
    float closestDistance = 99f;

    Animator animator;
    AudioSource audioSource;
    public AudioClip effectSound;
    public AudioClip dieSound;

    //public HPUIController hpUI;
    public GameObject attackEffect = null;

    void Start()
    {
        //currentHp = maxHp; // for test
    }
    private void OnEnable()
    {   
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.clip = null;
        currentTime = 0f;
        isAttacking = false;
        //currentHp = maxHp;
        agent.speed = moveSpeed;
        isDie = false;

        if (attackEffect != null) attackEffect.SetActive(false);
    }

    private void OnDisable()
    {
        GameObject mother = this.gameObject.GetComponent<MotherTurretCheck>().GetMotherTurret();
        //Debug.Log(gameObject.name + "의 mother 지정됨: " + mother.name);

        if (!mother.GetComponent<NPCSpawnController>().NPCPool.Contains(this.gameObject) && this.gameObject != null)
        {
            mother.GetComponent<NPCSpawnController>().NPCPool.Add(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isDie == true)
        {
            return;
        }

        currentTime += Time.deltaTime;
        if (currentTime > scanCooldown)
        {
            FindClosestTarget();
            currentTime = 0;
            closestDistance = 99f;
            //Debug.Log("agent상태: " + agent.enabled);
        }

        switch (state)
        {
            case Enemy_1State.IDLE:
                Idle();
                break;
            case Enemy_1State.MOVE:
                Move();
                break;
            case Enemy_1State.ATTACK:
                Attack();
                break;
            case Enemy_1State.DAMAGE:
                //Damage(); 
                break;
            case Enemy_1State.DIE:
                //Die(); 
                break;
        }

        //Debug.Log(gameObject.name + "현재 상태: " + state);
    }
    void FindClosestTarget()    // 가장 가까운 적을 찾음, 범위에 적이 있다면 MOVE, 없다면 IDLE
    {
        targets = Physics.OverlapSphere(transform.position, detectRadius, targetLayerMask);
        //Debug.Log(this.gameObject.name + "'s Target Count in detectRadius: " + targets.Length);

        if (targets.Length == 0)
        {
            closestTarget = null;
            state = Enemy_1State.IDLE;
            return;
        }

        foreach (Collider target in targets)
        {   
            if (target.gameObject.CompareTag("Turret"))
            {
                if (target.gameObject.GetComponent<TurretDisabled>().TurretBreak == true)
                {
                    continue;
                }
            }

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (closestDistance > distance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        if (isAttacking == true) return;
        state = Enemy_1State.MOVE;
        //Debug.Log("가장 가까운 타겟: " + closestTarget);
    }

    void Idle()
    {
        animator.SetBool("isMove", false);
        agent.enabled = false;
        //Debug.Log("IDLE 로 전환됨");
        StopAllCoroutines();
    }

    void Move()
    {
        //Debug.Log(gameObject.name + "은 MOVE 로 전환됨");
        if (closestTarget != null)
        {
            animator.SetBool("isMove", true);

            agent.enabled = true;
            agent.SetDestination(closestTarget.position);
            //Debug.Log("목표 지정: " + closestTarget.name);

            if (Vector3.Distance(closestTarget.position, transform.position) <= attackDistance)
            {
                //Debug.Log("ATTACK 로 전환됨");
                animator.SetBool("isMove", false);
                agent.enabled = false;
                state = Enemy_1State.ATTACK;
            }
        }
        else if (closestTarget == null)
        {
            //animator.SetBool("isMove", false);
            state = Enemy_1State.IDLE;
        }
    }

    void Attack()
    {
        if (isAttacking == true) return;

        if (!gameObject.CompareTag("Friend"))
        {
            Vector3 targetPos = closestTarget.position;
            targetPos.y = transform.position.y;
            gameObject.transform.LookAt(targetPos);
        }

        StartCoroutine(AttackSequence());
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true;

        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(attackDelay * 0.4f);
        
        if (closestTarget == null || Vector3.Distance(closestTarget.position, transform.position) > attackDistance)
        {
            isAttacking = false;
            animator.SetBool("isAttack", false);
            state = Enemy_1State.IDLE;
            yield break;
        }

        if (closestTarget.GetComponent<NPCController>() != null)
        {
            if (closestTarget.GetComponent<NPCController>().isDie == true) {
                isAttacking = false;
                animator.SetBool("isAttack", false);
                state = Enemy_1State.IDLE;
                yield break;
            }
        }
        
        GameObject target = closestTarget.gameObject;
        audioSource.Stop();
        audioSource.clip = effectSound;
        audioSource.Play();

        if (attackEffect != null)
        {
            this.attackEffect.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            this.attackEffect.SetActive(false);
        }

        target.GetComponent<HitPoint>().HP -= attackDamage;
        
        yield return new WaitForSeconds(attackDelay * 0.6f);
        animator.SetBool("isAttack", false);
        isAttacking = false;
        audioSource.Stop();
    }

    public void GetDamaged(int damage)      // 피해을 받았을때
    {
        //this.currentHp -= damage;
        //float ratio = (float)currentHp / maxHp;
        //Debug.Log("currentHP: " + currentHp + ", ratio: " + ratio);
        //hpUI.setHP(ratio);                  // 체력게이지 줄어듬
        this.gameObject.GetComponent<HitPoint>().HP -= damage;
        //if (this.currentHp < 0) Die();      // 사망 메소드 실행
    }

    public void Die()               // 이 오브젝트 사망시, HitPoint 스크립트에서 수행
    {   
        isDie = true;
        agent.enabled = false;
        animator.SetTrigger("isDie");

        if (this.gameObject.GetComponent<EnemyScore>() != null)
        {
            gameObject.GetComponent<EnemyScore>().scoreVisualize();
        }

        if (this.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.AddKillCount();
        }

        StartCoroutine(DieSequence());
    }

    IEnumerator DieSequence()
    {   
        if (dieSound != null)
        {
            audioSource.PlayOneShot(dieSound, 2f);
        }
        isAttacking = true;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public Transform GetClosestTargetLoc()
    {
        if (this.closestTarget != null)
        {
            return this.closestTarget.transform;
        }
        else
        {
            return null;
        }
    }

    private void OnCollisionEnter(Collision collision)              // 총알 맞았을때 
    {
        //Debug.Log("충돌감지, tag: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("bullet"))
        {   
            if (collision.gameObject.GetComponent<BulletDamage>() != null)
            {
                int bulletD = collision.gameObject.GetComponent<BulletDamage>().bulletDamage;
                GetDamaged(bulletD);
            }
        }
    }
}
