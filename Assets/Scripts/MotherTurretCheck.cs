using UnityEngine;

public class MotherTurretCheck : MonoBehaviour
{
    GameObject motherTurret;
    bool hasMotherTurret;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMotherTurret(GameObject mother)
    {
        this.motherTurret = mother;
        this.hasMotherTurret = true;
        //Debug.Log("SetMotherTurret »£√‚, " + gameObject.name + "¿« mother turret: " + mother.name);
    }

    public GameObject GetMotherTurret()
    {
        return this.motherTurret;
    }

    public bool GetHasMotherTurret()
    {
        return this.hasMotherTurret;
    }

    public void RemoveMotherTurret()
    {
        this.motherTurret = null;
        this.hasMotherTurret = false;
    }
}
