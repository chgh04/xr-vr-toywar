using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class RobotRotationFix : MonoBehaviour
{
    NavMeshAgent nav;
    Vector3 direction;
    Transform targetLoc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        nav = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        nav.updateRotation = false;
        targetLoc = this.gameObject.GetComponent<NPCController>().GetClosestTargetLoc();

        if (targetLoc != null)
        {
            direction = (targetLoc.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 90f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
        }

        
    }
}
