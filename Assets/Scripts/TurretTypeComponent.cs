using UnityEngine;

public enum TurretType
{
    cat_Level1,
    cat_Level2,
    cat_Level3,
    squirrel_Level1,
    squirrel_Level2,
    squirrel_Level3,
    toybox_Level1,
    toybox_Level2,
    toybox_Level3
}

public class TurretTypeComponent : MonoBehaviour
{
    public TurretType turretType;
}
