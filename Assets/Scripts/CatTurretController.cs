using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System.Collections;

public class CatTurretController : MonoBehaviour
{
    public bool isSquirrel;
    TurretDisabled turretDisabled;

    public float detectRadius = 10f;
    public LayerMask enemyLayer;
    public float scanCooldown = 1f;
    public float rotationSpeed = 5f;

    Collider[] targets;
    Transform closestTarget;
    float closestDistance;
    float currentTime;

    public GameObject bulletFac;
    public List<GameObject> catBulletPool = new List<GameObject>();
    public int catbulletPoolSize = 30;
    public Transform firePos;
    public Transform secondFirePos;
    public float fireLatency = 0.3f;
    public float fireCooldown = 3f;
    public float accuracyFactor = 0.2f;
    public int bulletIndex;

    bool isFire;

    AudioSource audioSource;
    public AudioClip fireSound;
    public float audioWaitTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        
    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fireSound;

        turretDisabled = GetComponent<TurretDisabled>();

        closestDistance = 99f;
        closestTarget = null;
        isFire = false;

        List<GameObject> usedBullets = new List<GameObject>();
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag("bullet") == true)
                {
                    usedBullets.Add(child.gameObject);
                }
            }
        } // 비활성화 되어있는 bullet 오브젝트 탐색

        //Debug.Log("Used Bullet: " + usedBullets.Count);
        if (usedBullets != null)
        {   
            foreach (GameObject bullet in usedBullets)
            {   
                if (bullet.GetComponent<BulletTypeComponent>() != null && this.GetComponent<BulletTypeComponent>() != null)
                {
                    //Debug.Log("same Bullet type: " + bullet);
                    if (bullet.GetComponent<BulletTypeComponent>().bulletType == this.GetComponent<BulletTypeComponent>().bulletType
                        && bullet.GetComponent<MotherTurretCheck>().GetHasMotherTurret() == false)
                    {
                        bullet.SetActive(true);
                        bullet.GetComponent<MotherTurretCheck>().SetMotherTurret(this.gameObject);
                        bullet.SetActive(false); // disable되면서 총알 List에 추가됨(SetMotherTurret으로 총알이 이 터렛의 총알이라고 명시)
                    }
                }
                if (catBulletPool.Count >= catbulletPoolSize) break; // catBulletPool보다 커진다면 종료, bulletIndex 이상의 탄을 가져가는걸 방지
            }
        }
        //Debug.Log("Bullet Count(befor create new bullet) = " + catBulletPool.Count + ", bulletIndex = " + catbulletPoolSize + ", " + (catBulletPool.Count < catbulletPoolSize));
        if (catBulletPool.Count < catbulletPoolSize)           // 첫 활성화시 동작
        {
            for (int i = 0; i < catbulletPoolSize; i++)
            {
                GameObject bullet = Instantiate(bulletFac);
                bullet.GetComponent<MotherTurretCheck>().SetMotherTurret(this.gameObject);
                bullet.SetActive(true);
                bullet.SetActive(false); // 비활성화 하면서 리스트에 자동 포함
            }
            return;
        }

        //Debug.Log("bullet pull: " + catBulletPool.Count);
    }

    private void OnDisable()
    {   
        //while (catBulletPool.Count == 0)
        //{
        //    catBulletPool[0].GetComponent<MotherTurretCheck>().RemoveMotherTurret();
        //    catBulletPool.RemoveAt(0);
        //}

        foreach (GameObject bullet in catBulletPool)
        {
            bullet.GetComponent<MotherTurretCheck>().RemoveMotherTurret();
            //Debug.Log("MotherTurret 제거");
        }
        this.catBulletPool.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.instance.gameInStage == false)
        //{   
        //    return;
        //}

        if (turretDisabled.TurretBreak == true || GameManager.instance.isPlayerGameOver == true)
        {
            //StopAllCoroutines();
            return;
        }

        currentTime += Time.deltaTime;
        if (currentTime > scanCooldown)
        {
            FindCloesetEnemy();
            currentTime = 0;
            closestDistance = 99f;
        }

        if (closestTarget != null)
        {
            //Debug.Log("isFire: " + isFire);
            Vector3 direction = (closestTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            if (isFire == false)
            {
                StartCoroutine(CatTowerFire());
            }
        }

    }

    void FindCloesetEnemy()
    {
        targets = Physics.OverlapSphere(transform.position, detectRadius, enemyLayer);
        //Debug.Log("Target count: " +  targets.Length);

        if (targets.Length == 0)
        {
            closestTarget = null;
            return;
        }

        foreach (Collider target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            //Debug.Log("감지된 콜라이더: " + target.name + ", 콜라이더와의 거리: " + distance);

            if (closestDistance > distance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        //Debug.Log("가장 가까운 타겟: " + closestTarget);
    }

    public float GetClosestDistance()
    {
        return closestDistance;
    }

    IEnumerator CatTowerFire()
    {
        isFire = true;
        //Debug.Log("발사 시작, 남은 총알 수" + catBulletPool.Count);
        audioSource.Play();

        for (int i = 0; i < bulletIndex; i++)
        {
            if (closestTarget == null) break;

            if (catBulletPool.Count > 0)
            {
                GameObject bullet = catBulletPool[0];
                bullet.SetActive(true);                                 //총알 활성화

                Vector3 dir = (closestTarget.position - firePos.position);
                bullet.transform.position = firePos.position;           // 총알의 위치를 총구로 지정

                //Debug.Log((this.secondFirePos != null) + ", i = " + i + ", activeSelf = " + (bullet.activeSelf));
                //Debug.Log("before check: " + bullet.transform.position + "i = " + i);

                if (this.secondFirePos != null && i % 2 == 0)
                {
                    dir = (closestTarget.position - secondFirePos.position);
                    bullet.transform.position = secondFirePos.position; // 두 번째 firepos를 가지고 있다면, 짝수사격마다 변경
                }

                firePos.rotation = Quaternion.LookRotation(dir);
                dir.x += Random.Range(-accuracyFactor, accuracyFactor);
                dir.y += Random.Range(-accuracyFactor, accuracyFactor);
                dir.z += Random.Range(-accuracyFactor, accuracyFactor); // 총알의 정확도 랜덤화
                bullet.transform.rotation = Quaternion.LookRotation(dir);

                if (isSquirrel == false)
                {
                    bullet.GetComponent<CatTurretBullet>().bulletAddForce();
                    //Debug.Log("발사시작, i = " + i);
                } 
                else if (isSquirrel == true)
                {
                    bullet.GetComponent<SquirrelBombController>().bulletAddForce(Vector3.Distance(transform.position, closestTarget.position));
                }
                bullet.GetComponent<TrailRenderer>().Clear();
                catBulletPool.RemoveAt(0);

                yield return new WaitForSeconds(fireLatency);
            }
        }
        yield return new WaitForSeconds(audioWaitTime);
        audioSource.Stop();

        yield return new WaitForSeconds(fireCooldown);
        isFire = false;
    }

}
