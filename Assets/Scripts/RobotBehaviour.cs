using UnityEngine;

namespace MainNameSpace
{
    public class RobotBehaviour : MonoBehaviour
    {
        [SerializeField] private RobotBasicSettings settings;
        [SerializeField] private Vector3 endPoint;
        [SerializeField] public bool HasBeenHacked;
        [SerializeField] private int breakChance;
        [SerializeField] private int brokenMultiplier;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float chaseSpeedMultiplier;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float visionAngle;
        [SerializeField] private float disableTime;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 destination;
        private GameObject target;
        private float timer = 0;
        private int brokenFactor;

        void Start()
        {
            brokenMultiplier = settings.BrokenMultiplier;
            moveSpeed = settings.MoveSpeed;
            chaseSpeedMultiplier = settings.ChaseSpeedMultiplier;
            rotationSpeed = settings.RotationSpeed;
            visionAngle = settings.VisionAngle;
            disableTime = settings.DisableTime;
            breakChance = settings.BreakChance;
            startPosition = transform.position;
            endPosition = startPosition + endPoint;
            destination = endPosition;
            brokenFactor = Random.Range(0, 100);
        }

        void Update()
        {
            if (HasBeenHacked == true) ProcessHacking();
            target = Vision();
            if (target != null && isPlayerVisible())
            {
                TerminateTarget();
                Rotate(target.transform.position);
            }
            else
            {
                Patrol();
                Rotate(destination);
            }
        }


        void OnCollisionEnter(Collision collision)
        {
            if (HasBeenHacked == false)
            {
               Debug.Log("Youre dead!");
            }
        }

        private void TerminateTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime * chaseSpeedMultiplier);
        }

        private void Patrol()
        {

            if (transform.position == destination)
            {
                ResertDestination();
            }

            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }

        private void ResertDestination()
        {
            if (destination == endPosition)
            {
                destination = startPosition;
            }
            else if (destination == startPosition)
            {
                destination = endPosition;
            }
        }

        private void Rotate(Vector3 _rotationVector)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(_rotationVector - transform.position), rotationSpeed * Time.deltaTime);
        }

        private bool isPlayerVisible()
        {
            if (isPlayerOnSightLine() != null && isPlayerOnSightLine().tag == "Player" && isPlayerInSightAngle()) return true;
            else return false;
        }

        private bool isPlayerInSightAngle()
        {
            return Mathf.Abs(Vector3.SignedAngle(transform.position - target.transform.position, transform.forward, Vector3.up)) > 180 - visionAngle * 0.5f;
        }
        private GameObject isPlayerOnSightLine()
        {
            Ray ray = new Ray(transform.position, target.transform.position - transform.position);
            Debug.DrawRay(transform.position, (target.transform.position - transform.position) * 200, Color.red);
            RaycastHit obj;
            if (Physics.Raycast(ray, out obj))
            {
                return obj.collider.gameObject;
            }
            else return null;
        }

        private GameObject Vision()
        {
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, 20, hitColliders);
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].gameObject.tag == "Player") return hitColliders[i].gameObject;
            }
            return null;
        }

        private void ProcessHacking()
        {
            Debug.Log(timer);
            if (brokenFactor <= breakChance)
            {
                moveSpeed *= brokenMultiplier;
                rotationSpeed *= brokenMultiplier;
                brokenFactor = Random.Range(0, 100);
                HasBeenHacked = false;
            }
            else
            {
                moveSpeed = 0;
                rotationSpeed = 0;
                timer += Time.deltaTime;
                if (timer >= disableTime)
                {
                    moveSpeed = settings.MoveSpeed;
                    rotationSpeed = settings.RotationSpeed;
                    timer = 0;
                    brokenFactor = Random.Range(0, 100);
                    HasBeenHacked = false;
                }
            }
        }
    }
}
