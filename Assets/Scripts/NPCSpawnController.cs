using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class NPCSpawnController : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject NPCFac;
    
    public List<GameObject> NPCPool = new List<GameObject>();
    public int poolSize;
    public float respawnTime;

    float currentTime;

    TurretDisabled turretDisabled = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {   
        currentTime = 0;
        turretDisabled = GetComponent<TurretDisabled>();

        List<GameObject> usedNPC = new List<GameObject>();
        foreach (GameObject root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.CompareTag("Friend") == true || child.CompareTag("Enemy"))
                {
                    usedNPC.Add(child.gameObject);
                }
            }
        } // 비활성화 되어있는 NPC 오브젝트 탐색

        if (usedNPC != null)
        {
            foreach (GameObject NPC in usedNPC)
            {
                if (NPC.GetComponent<NPCTypeComponent>() != null && this.GetComponent<NPCTypeComponent>() != null)
                {
                    if (NPC.GetComponent<NPCTypeComponent>().npcType == this.GetComponent<NPCTypeComponent>().npcType
                        && NPC.GetComponent<MotherTurretCheck>().GetHasMotherTurret() == false)
                    {
                        NPC.SetActive(true);
                        NPC.GetComponent<MotherTurretCheck>().SetMotherTurret(this.gameObject);
                        NPC.SetActive(false); // disable되면서 List에 추가됨
                    }
                }
                if (NPCPool.Count >= poolSize) break; // Pool보다 커진다면 종료
            }
        }
        
        if (NPCPool.Count <  poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {   
                GameObject npc = Instantiate(NPCFac);
                npc.GetComponent<MotherTurretCheck>().SetMotherTurret(this.gameObject);
                npc.SetActive(true);
                //Debug.Log(npc.name + " 생성됨");
                npc.SetActive(false);
                //Debug.Log(npc.name + " 비활성화");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (GameManager.instance.stopSpawnEnemy == true)
        {
            return;
        }

        if (turretDisabled != null)
        {
            if (turretDisabled.TurretBreak == true)
            {
                return;
            }
        }

        if (NPCPool.Count > 0)
        {
            currentTime += Time.deltaTime;
            if (currentTime > respawnTime)
            {
                //Debug.Log(gameObject.name + "'s poolSize: " + NPCPool.Count + ", now start respawn");
                //Debug.Log(gameObject.name + "'s respawn position: " + spawnPoint.position);
                GameObject npc = NPCPool[0];
                npc.SetActive(true);

                NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.Warp(spawnPoint.position);
                    if (agent.enabled && agent.isOnNavMesh)
                    {
                        agent.ResetPath();
                    }
                }
                else
                {
                    npc.transform.position = spawnPoint.position;
                }

                if (this.gameObject.CompareTag("EnemySpawner"))
                {
                    GameManager.instance.AddRemainEnemyCount();
                }

                NPCPool.RemoveAt(0);
                currentTime = 0;
            }
        }
    }
}
