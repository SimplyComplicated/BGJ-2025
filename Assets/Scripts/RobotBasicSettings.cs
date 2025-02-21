using UnityEngine;

namespace MainNameSpace
{
    [CreateAssetMenu(fileName = "RobotBasicSettings", menuName = "Scriptable Objects/RobotBasicSettings")]
    public class RobotBasicSettings : ScriptableObject
    {
        public int BrokenMultiplier;
        public float MoveSpeed;
        public float ChaseSpeedMultiplier = 1;
        public float RotationSpeed;
        public float VisionAngle;
        public float DisableTime;
        public int BreakChance;

    }

}