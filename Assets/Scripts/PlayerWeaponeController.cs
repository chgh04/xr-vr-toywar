using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponeController : MonoBehaviour
{
    public InputActionReference triggerButton;

    public static List<GameObject> bulletPool = new List<GameObject>();
    public GameObject bulletFac;
    public Transform firePos;
    public int bulletPoolSize = 10;

    public float fireLatency = 1;
    bool canFire = true;

    private void OnEnable()
    {
        triggerButton.action.performed += OnTriggerPressed;

        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletFac);
            bulletPool.Add(bullet);
            bullet.SetActive(false);
        }

        for (int i = bulletPool.Count - 1; i >= 0; i--)
        {
            if (bulletPool[i].GetComponent<BulletTypeComponent>().bulletType != gameObject.GetComponent<BulletTypeComponent>().bulletType)
            {
                bulletPool.RemoveAt(i);
            }
        }
    }

    private void OnDisable()    //이벤트 연결 해제
    {
        triggerButton.action.performed -= OnTriggerPressed;
    }

    void OnTriggerPressed(InputAction.CallbackContext context)
    {   

        if (canFire == true)
        {
            Debug.Log("bullet Fire");
            StartCoroutine(FireWithCooldown());
        }
        
    }

    IEnumerator FireWithCooldown()
    {
        canFire = false;
        //Instantiate(bullet, firePos.position, firePos.rotation);
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool[0];
            bullet.SetActive (true);
            bullet.transform.position = firePos.position;
            bullet.transform.rotation = firePos.rotation;
            bullet.GetComponent<ToyGunBullet>().bulletAddForce();
            bullet.GetComponent<TrailRenderer>().Clear();

            bulletPool.RemoveAt(0);
        }


        yield return new WaitForSeconds(fireLatency);
        canFire = true;
    }
}
