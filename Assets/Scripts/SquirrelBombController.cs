using Unity.VisualScripting;
using UnityEngine;

public class SquirrelBombController : MonoBehaviour
{
    public int bombDamage = 60;
    public float bombEffectRange = 3f;
    public float explosionDelay = 2.5f;
    public float baseBulletSpeed = 0.7f;
    public float baseUpwardForce = 0.6f;
    float upwardForce = 5f;
    float bulletSpeed = 7f;

    public GameObject explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;
    GameObject explosionEffect;

    Rigidbody rb;
    Vector3 forceDirection;
    float currentTime;

    GameObject motherTurret;
    bool hasMotherTurret;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {   
        if (explosionEffect == null)
        {
            explosionEffect = Instantiate(explosion);
            expEffect = explosionEffect.GetComponent<ParticleSystem>();
            expAudio = explosionEffect.GetComponent<AudioSource>();
        }
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

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > explosionDelay)
        {
            Explosion();
        }
    }

    void Explosion()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] targets = Physics.OverlapSphere(transform.position, bombEffectRange, layerMask);

        foreach (Collider target in targets)
        {   
            if (target.gameObject.GetComponent<NPCController>() && target.gameObject.CompareTag("Enemy"))
            {
                target.gameObject.GetComponent<NPCController>().GetDamaged(bombDamage);
            }
        }

        explosionEffect.transform.position = this.transform.position;
        expEffect.Play();
        expAudio.Play();
        this.gameObject.SetActive(false);
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

    public void bulletAddForce(float distance)
    {
        upwardForce = distance * baseUpwardForce;
        bulletSpeed = distance * baseBulletSpeed;
        //Debug.Log("upwardForce = " + upwardForce + ", distace = " + distance);

        forceDirection = transform.forward * bulletSpeed + transform.up * upwardForce;
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }

    public void BulletRemove()
    {
        currentTime = 0;
        Explosion();
    }
}
