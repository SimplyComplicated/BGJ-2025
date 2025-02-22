using UnityEngine;

namespace MainNameSpace
{
    public class RobotBehaviour : MonoBehaviour
    {
        [Header("Waypoints")]
        [SerializeField]
        private int _waypointStartIndex = 0;

        [SerializeField]
        private Transform _waypointsContainer;

        [SerializeField]
        private Transform[] _waypoints;

        [Header("Params")]
        [SerializeField]
        private float _searchRadius = 10;

        [SerializeField]
        private RobotBasicSettings settings;

        [SerializeField]
        private int breakChance;

        [SerializeField]
        private int brokenMultiplier;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float chaseSpeedMultiplier;

        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private float visionAngle;

        [SerializeField]
        private float disableTime;

        private int _waypointIncrementalValue = 1;
        private int _waypointCurrentIndex;

        private GameObject target;
        private float timer = 0;
        private int brokenFactor;

        private bool _isHacked = false;

        void Start()
        {
            brokenMultiplier = settings.BrokenMultiplier;
            moveSpeed = settings.MoveSpeed;
            chaseSpeedMultiplier = settings.ChaseSpeedMultiplier;
            rotationSpeed = settings.RotationSpeed;
            visionAngle = settings.VisionAngle;
            disableTime = settings.DisableTime;
            breakChance = settings.BreakChance;
            brokenFactor = Random.Range(0, 100);

            SetupWaypointsIfNeeded();

            _waypointCurrentIndex = _waypointStartIndex;
            transform.position = _waypoints[_waypointCurrentIndex].position;
        }

        void Update()
        {
            HandleHackedState();

            target = Vision();
            if (target != null && isPlayerVisible())
            {
                TerminateTarget();
                Rotate(target.transform.position);
            }
            else
            {
                Patrol();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _searchRadius);

            if (_waypoints != null && _waypoints.Length >= 2)
            {
                DrawWaypoints();
            }
        }

        private void DrawWaypoints()
        {
            for (int i = 0, length = _waypoints.Length; i < length; i++)
            {
                Gizmos.DrawWireSphere(_waypoints[i].position, 0.5f);
            }

            for (int i = 0, length = _waypoints.Length - 1; i < length; i++)
            {
                var a = _waypoints[i];
                var b = _waypoints[i + 1];
                Gizmos.DrawLine(a.position, b.position);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Youre dead!");
            }
        }

        [ContextMenu(nameof(SetupWaypointsIfNeeded))]
        private void SetupWaypointsIfNeeded()
        {
            if (_waypointsContainer != null)
            {
                _waypoints = new Transform[_waypointsContainer.childCount];

                for (int i = 0; i < _waypoints.Length; i++)
                {
                    _waypoints[i] = _waypointsContainer.GetChild(i);
                }
            }
        }

        private void TerminateTarget()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target.transform.position,
                moveSpeed * Time.deltaTime * chaseSpeedMultiplier
            );
        }

        private void Patrol()
        {
            var destination = GetDestination();

            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                if (_waypointCurrentIndex + _waypointIncrementalValue >= _waypoints.Length)
                {
                    _waypointIncrementalValue = -1;
                    _waypointCurrentIndex = _waypoints.Length - 1 - 1;
                }
                else if (_waypointCurrentIndex + _waypointIncrementalValue <= 0)
                {
                    _waypointIncrementalValue = 1;
                    _waypointCurrentIndex = 0;
                }
                else
                {
                    _waypointCurrentIndex += _waypointIncrementalValue;
                }

                destination = GetDestination();
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                moveSpeed * Time.deltaTime
            );

            Rotate(destination);
        }

        private Vector3 GetDestination()
        {
            return _waypoints[_waypointCurrentIndex].position;
        }

        private void Rotate(Vector3 rotationVector)
        {
            var dir = rotationVector - transform.position;

            if (dir == Vector3.zero)
            {
                return;
            }

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                rotationSpeed * Time.deltaTime
            );
        }

        private bool isPlayerVisible()
        {
            var player = isPlayerOnSightLine();

            if (player != null && player.tag == "Player" && isPlayerInSightAngle())
                return true;
            else
                return false;
        }

        private bool isPlayerInSightAngle()
        {
            return Mathf.Abs(
                    Vector3.SignedAngle(
                        transform.position - target.transform.position,
                        transform.forward,
                        Vector3.up
                    )
                )
                > 180 - visionAngle * 0.5f;
        }

        private GameObject isPlayerOnSightLine()
        {
            Ray ray = new Ray(transform.position, target.transform.position - transform.position);
            Debug.DrawRay(ray.origin, ray.direction * _searchRadius, Color.red);
            RaycastHit obj;
            if (Physics.Raycast(ray, out obj))
            {
                return obj.collider.gameObject;
            }
            else
                return null;
        }

        private GameObject Vision()
        {
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(
                transform.position,
                _searchRadius,
                hitColliders
            );
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].gameObject.tag == "Player")
                    return hitColliders[i].gameObject;
            }
            return null;
        }

        [ContextMenu(nameof(ProcessHacking))]
        public void ProcessHacking()
        {
            if (brokenFactor <= breakChance)
            {
                HackedFailure();
            }
            else
            {
                HackedSuccessfully();
            }
        }

        private void HackedFailure()
        {
            moveSpeed *= brokenMultiplier;
            rotationSpeed *= brokenMultiplier;
            brokenFactor = Random.Range(0, 100);
        }

        private void HackedSuccessfully()
        {
            moveSpeed = 0;
            rotationSpeed = 0;

            _isHacked = true;
        }

        private void HandleHackedState()
        {
            if (!_isHacked)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= disableTime)
            {
                moveSpeed = settings.MoveSpeed;
                rotationSpeed = settings.RotationSpeed;
                timer = 0;
                brokenFactor = Random.Range(0, 100);

                _isHacked = false;
            }
        }
    }
}
