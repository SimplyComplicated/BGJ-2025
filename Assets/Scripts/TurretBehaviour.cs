using UnityEngine;

namespace MainNameSpace
{
    public sealed class TurretBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform _origin;

        [SerializeField]
        private float _rotationSmoothFactor = 0.2f;

        [SerializeField]
        private TurretBasicSettings settings;

        [SerializeField]
        public LayerMask LayerMask;

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

        private float _elapsedTime = 0;
        private bool _isHacked = false;

        private Ray _ray;

        void Start()
        {
            LayerMask = settings.LayerMask;
            AngleSpeed = settings.AngleSpeed;
            VisionDistance = settings.VisionDistance;
            disableTime = settings.DisableTime;
            TimeToEscape = settings.TimeToEscape;
            breakChance = settings.BreakChance;
            brokenMultiplier = settings.BrokenMultiplier;
            brokenStateTimer = 0;
            timer = 0;
            brokenFactor = Random.Range(0, 100);
        }

        void Update()
        {
            HandleHackedState();

            _ray = new Ray(_origin.position, _origin.forward);

            target = Aim();

            Debug.DrawRay(
                _ray.origin,
                _ray.direction * VisionDistance,
                target != null ? Color.green : Color.red
            );

            if (target == null)
            {
                SearchForTarget();
                timer = 0;
            }
            else
            {
                LockOnTarget();
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            var color = Color.red;
            color.a = 0.1f;

            var normal = transform.up;
            var forward =
                Quaternion.AngleAxis(AngleOfView * 0.5f * -1f, normal) * transform.forward;

            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawSolidArc(
                transform.position,
                normal,
                forward,
                AngleOfView,
                VisionDistance
            );
#endif
        }

        private void SearchForTarget()
        {
            _elapsedTime += AngleSpeed * Time.deltaTime;
            movementFactor = Mathf.Sin(_elapsedTime) * AngleOfView * 0.5f;
            _origin.localRotation = Quaternion.Euler(0, movementFactor, 0);
        }

        private void LockOnTarget()
        {
            var dir = target.transform.position - _origin.position;

            _origin.rotation = Quaternion.Lerp(
                _origin.rotation,
                Quaternion.LookRotation(dir),
                _rotationSmoothFactor * 50 * Time.deltaTime
            );

            timer += Time.deltaTime;
            if (timer >= TimeToEscape)
            {
                Fire();
                timer = 0;
            }
        }

        private bool IsValidAngle()
        {
            return Mathf.Abs(_origin.localRotation.eulerAngles.y) <= AngleOfView * 0.5f;
        }

        private void Fire()
        {
            Debug.Log("You're dead");
        }

        GameObject Aim()
        {
            if (Physics.Raycast(_ray, out var obj, VisionDistance, LayerMask))
            {
                if (obj.collider.gameObject.CompareTag("Player") && IsValidAngle())
                {
                    return obj.collider.gameObject;
                }
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
            AngleSpeed *= brokenMultiplier;
            brokenFactor = Random.Range(0, 100);
        }

        private void HackedSuccessfully()
        {
            AngleSpeed = 0;
            _isHacked = true;
        }

        private void HandleHackedState()
        {
            if (!_isHacked)
            {
                return;
            }

            brokenStateTimer += Time.deltaTime;
            if (brokenStateTimer >= disableTime)
            {
                AngleSpeed = settings.AngleSpeed;
                brokenStateTimer = 0;
                brokenFactor = Random.Range(0, 100);
            }
        }
    }
}
