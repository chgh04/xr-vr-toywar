using UnityEngine;

public class ToyGunBullet : MonoBehaviour
{
    public float bulletSpeed = 17f;
    public float upwardForce = 2f;

    Rigidbody rb;
    Vector3 forceDirection;
    float currentTime;

    AudioSource audioSource;
    public AudioClip shotSound;

    void Start()
    {
        //forceDirection = transform.forward * bulletSpeed + transform.up * upwardForce;
        //rb.AddForce(forceDirection, ForceMode.Impulse);
        //transform.Rotate(90f, 0f, 0f);
    }

    private void OnEnable()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        currentTime = 0;
        RBReset();
        audioSource.PlayOneShot(shotSound);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > 1.25f)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (!PlayerWeaponeController.bulletPool.Contains(this.gameObject))
        {
            PlayerWeaponeController.bulletPool.Add(this.gameObject);
        }
    }

    void RBReset()
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
        transform.Rotate(90f, 0f, 0f);
    }
}
