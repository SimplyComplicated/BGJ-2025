using UnityEngine;

namespace MainNameSpace
{
    [CreateAssetMenu(fileName = "TurretBasicSettings", menuName = "Scriptable Objects/TurretBasicSettings")]
    public class TurretBasicSettings : ScriptableObject
    {
        public LayerMask LayerMask;
        public float AngleSpeed;
        public float VisionDistance;
        public float DisableTime;
        public int TimeToEscape;
        public int BreakChance;
        public int BrokenMultiplier;
    }
}
