using System.Collections;
using UnityEngine;

public class CatTurretBullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public float upwardForce = 2f;
    public float disableTime = 4f;

    Rigidbody rb;
    Vector3 forceDirection;
    float currentTime;

    void Start()
    {
        
        //forceDirection = transform.forward * bulletSpeed + transform.up * upwardForce;
        //rb.AddForce(forceDirection, ForceMode.Impulse);
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > disableTime)
        {   
            if(this.gameObject != null) this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        currentTime = 0f;
        RBReset();
    }

    private void OnDisable()
    {   
        GameObject mother = this.gameObject.GetComponent<MotherTurretCheck>().GetMotherTurret();

        if (!mother.GetComponent<CatTurretController>().catBulletPool.Contains(this.gameObject) && this.gameObject != null)
        {
            mother.GetComponent<CatTurretController>().catBulletPool.Add(this.gameObject);
        }
    }

    public void RBReset()
    {   
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    public void bulletAddForce()
    {
        forceDirection = transform.forward * bulletSpeed + transform.up * upwardForce;
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }

    public void BulletRemove()
    {
        currentTime = 0;
        if (this.gameObject != null) this.gameObject.SetActive(false);
    }
}
