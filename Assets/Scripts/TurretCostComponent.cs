using UnityEngine;

public class TurretCostComponent : MonoBehaviour
{
    public int turretCost = 0;
    public int TurretCost
    {
        get { return turretCost; }
        set { turretCost = value; }
    }
}
