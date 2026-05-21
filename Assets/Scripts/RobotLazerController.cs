using Unity.VisualScripting;
using UnityEngine;

public class RobotLazerController : MonoBehaviour
{
    Transform closestTarget;
    float attackDistance;
    
    Vector3 lazerScale;

    Vector3 originLazerScale;
    Vector3 originLazerPosition;

    public bool isEnemyRobot = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        attackDistance = transform.parent.GetComponent<NPCController>().attackDistance;
        closestTarget = transform.parent.GetComponent<NPCController>().GetClosestTargetLoc();

        originLazerScale = transform.localScale;
        originLazerPosition = transform.localPosition;

        lazerScale = transform.localScale;

        if (closestTarget != null)
        {
            float distance = Vector3.Distance(closestTarget.position, transform.parent.position);

            if (distance <= attackDistance)
            {
                lazerScale.y = (distance / attackDistance) * originLazerScale.y;
                if (lazerScale.y < 0.5f)
                {
                    lazerScale.y = 0.5f;
                }
                float lengthDiff = (originLazerScale.y - lazerScale.y);
                //if (-lengthDiff < 2.3f) lengthDiff = 2.3f;

                if (isEnemyRobot == true)
                {
                    transform.localPosition = new Vector3(originLazerPosition.x, transform.localPosition.y, transform.localPosition.z - lengthDiff);
                    transform.localScale = lazerScale;
                }
                else
                {
                    transform.localPosition = new Vector3(originLazerPosition.x + lengthDiff, transform.localPosition.y, transform.localPosition.z);
                    transform.localScale = lazerScale;
                }    
            }
        }
    }

    private void OnDisable()
    {
        transform.localScale = originLazerScale;
        transform.localPosition = originLazerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
