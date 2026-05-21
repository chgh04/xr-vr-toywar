using UnityEngine;

public enum NPCType {
    ToyRobot_Level1,
    ToyRobot_Level2,
    ToyRobot_Level3,
    Mummy_Level1,
    Mummy_Level2,
    Mummy_Level3,
    Ghost_Level1,
    Ghost_Level2,
    Ghost_Level3,
    RobotSoldier_Level1,
    RobotSoldier_Level2,
    RobotSoldier_Level3
}

public class NPCTypeComponent : MonoBehaviour
{
    public NPCType npcType;
}
