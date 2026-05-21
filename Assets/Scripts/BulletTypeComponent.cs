using UnityEngine;

public enum BulletType
{
    bbBulet_Level1,
    bbBulet_Level2,
    bbBulet_Level3,
    cakeBomb_Level1,
    cakeBomb_Level2,
    cakeBomb_Level3,
    ToyBullet_Level1,
    ToyBullet_Level2,
    ToyBullet_Level3
}

public class BulletTypeComponent : MonoBehaviour
{   
    public BulletType bulletType;
}
