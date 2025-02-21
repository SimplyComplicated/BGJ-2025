using UnityEngine;

namespace MainNameSpace
{
    public sealed class TurretBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TurretBasicSettings settings;

        [SerializeField]
        public LayerMask LayerMask;

        [SerializeField]
        public bool HasBeenHacked;

        [SerializeField]
        private int breakChance;

        [SerializeField]
        private int TimeToEscape;

        [SerializeField]
        private int brokenMultiplier;

        [SerializeField]
        private float AngleSpeed;

        [SerializeField]
        private float AngleOfView;

        [SerializeField]
        private float VisionDistance;

        [SerializeField]
        private float disableTime;
        private GameObject target;
        private float movementFactor;
        private float brokenStateTimer;
        private float timer;
        private int brokenFactor;

        void Start()
        {
            LayerMask = settings.LayerMask;
            AngleSpeed = settings.AngleSpeed;
            VisionDistance = settings.VisionDistance;
            disableTime = settings.DisableTime;
            TimeToEscape = settings.TimeToEscape;
            breakChance = settings.BreakChance;
            brokenMultiplier = settings.BrokenMultiplier;
            HasBeenHacked = false;
            brokenStateTimer = 0;
            timer = 0;
            brokenFactor = Random.Range(0, 100);
        }

        void Update()
        {
            if (HasBeenHacked == true)
                ProcessHacking();

            target = Aim();

            if (target == null || target.tag != "Player")
            {
                SearchForTarget();
                timer = 0;
            }
            else if (target.tag == "Player")
            {
                LockOnTarget();
            }
        }

        private void SearchForTarget()
        {
            movementFactor = Mathf.PingPong(Time.time * AngleSpeed, AngleOfView);
            transform.rotation = Quaternion.Euler(0, movementFactor, 0);
        }

        private void LockOnTarget()
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(target.transform.position - transform.position),
                AngleSpeed * Time.deltaTime
            );
            movementFactor = transform.rotation.y;
            timer += Time.deltaTime;
            if (timer >= TimeToEscape)
                Fire();
            if (timer >= 1000)
                timer = 0;
        }

        private void Fire()
        {
            Debug.Log("You're dead");
        }

        GameObject Aim()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.forward * 200, Color.red);
            RaycastHit obj;
            if (Physics.Raycast(ray, out obj, VisionDistance, LayerMask))
            {
                return obj.collider.gameObject;
            }
            else
                return null;
        }

        private void ProcessHacking()
        {
            if (brokenFactor <= breakChance)
            {
                AngleSpeed *= brokenMultiplier;
                brokenFactor = Random.Range(0, 100);
                HasBeenHacked = false;
            }
            else
            {
                AngleSpeed = 0;
                brokenStateTimer += Time.deltaTime;
                if (brokenStateTimer >= disableTime)
                {
                    AngleSpeed = settings.AngleSpeed;
                    brokenStateTimer = 0;
                    brokenFactor = Random.Range(0, 100);
                    HasBeenHacked = false;
                }
            }
        }
    }
}
