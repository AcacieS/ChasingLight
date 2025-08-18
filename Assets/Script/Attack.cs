
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Camera camera;


    // ------------------- //
    // ATTACKING BEHAVIOUR // 
    // ------------------- //

    [Header("Attacking")]
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float minSpawnDistance = 2f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private LayerMask attackLayer;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private AudioClip swordSwing;
    [SerializeField] private AudioClip hitSound;

    private bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;

    // ---------- //
    // ANIMATIONS // 
    // ---------- //

    // [Header("Animation")]
    // [SerializeField] private Animator animator;
    // [SerializeField] private const string IDLE = "Idle";
    // [SerializeField] private const string ATTACK1 = "Attack 1";
    // string currentAnimationState;


    [Header("Catcher Animation")]
    [SerializeField] Transform rodRoot;
    [SerializeField] Transform netTip;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float returnRotateSpeed = 5f;
    private Quaternion targetRotation;
    private Quaternion originalRotation;

    private GameObject target;
    private bool rotating = false;
    private bool returning = false;
    Vector3 dir;

    private InputSystem_Actions inputActions;


    void Start()
    {
        originalRotation = rodRoot.rotation;

    }

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {

        // // Your attack logic here

        // if (!readyToAttack || attacking) return;
        // readyToAttack = false;
        // attacking = true;

        // Invoke(nameof(ResetAttack), attackSpeed);
        // Invoke(nameof(AttackRayCast), attackDelay);

        // audioSource.pitch = Random.Range(0.9f, 1.1f);
        // audioSource.PlayOneShot(swordSwing);

    }
    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
        //ChangeAnimationState(IDLE);
    }
    void AttackRayCast()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            Debug.Log("Attack on spark");
            HitTarget(hit.point);
            target = hit.collider.gameObject;

            //    Vector3 tipTargetWorld = hit.point;
            //     Vector3 targetDirWorld = (tipTargetWorld - rodRoot.position).normalized;

            //     // Get local-space direction of target relative to rodRoot
            //     Vector3 targetDirLocal = rodRoot.parent.InverseTransformDirection(targetDirWorld);

            //     // // Project onto X–Y plane (assuming Z is hinge axis)
            //     // targetDirLocal.z = 0f;
            //     targetDirLocal.Normalize();

            //     // Current local forward direction (tip vector)
            //     Vector3 currentDirLocal = rodRoot.parent.InverseTransformDirection(
            //         (netTip.position - rodRoot.position).normalized);
            //     //currentDirLocal.z = 0f;
            //     currentDirLocal.Normalize();

            //     // Signed angle around Z axis
            //     float angleDelta = Vector3.SignedAngle(currentDirLocal, targetDirLocal, Vector3.forward);

            //     // Build target rotation in local space, only applying Z swing
            //     targetRotation = rodRoot.localRotation * Quaternion.AngleAxis(angleDelta, Vector3.forward);

            //     rotating = true;
            //     returning = false;

            // Compute world-space vector from rodRoot to netTip
            Vector3 tipOffsetWorld = netTip.position - rodRoot.position;

            // Compute world-space vector from rodRoot to target
            Vector3 targetDirWorld = hit.point - rodRoot.position;
            // Convert to local space relative to rodRoot.parent (camera)
            Vector3 tipOffsetLocal = rodRoot.parent.InverseTransformDirection(tipOffsetWorld);
            Vector3 targetDirLocal = rodRoot.parent.InverseTransformDirection(targetDirWorld);


            // Compute rotation that rotates tipOffset to targetDir
            Quaternion deltaRot = Quaternion.FromToRotation(tipOffsetLocal, targetDirLocal);

            // Apply delta to current rod rotation to get target rotation
            targetRotation = deltaRot * rodRoot.localRotation;

            rotating = true;
            returning = false;
            rodRoot.localRotation = originalRotation;
        }
        else
        {
            //target = null;
            
        }
    }

    void Update()
    {
        if (rotating)
        {

            rodRoot.localRotation = Quaternion.RotateTowards(
                rodRoot.localRotation,
                targetRotation,
                rotateSpeed * Time.deltaTime * 60f
            );

            if (Quaternion.Angle(rodRoot.localRotation, targetRotation) < 0.1f)
            {
                rotating = false;
                target.GetComponent<Actor>().Catched();
                Debug.Log("Finish rotate");
                returning = true;
            }
        }
        else if (returning)
        {

            rodRoot.localRotation = Quaternion.RotateTowards(
            rodRoot.localRotation,
            originalRotation,
            returnRotateSpeed * Time.deltaTime * 60f
            );

            if (Quaternion.Angle(rodRoot.localRotation, originalRotation) < 0.1f)
            {
                rodRoot.localRotation = originalRotation;
                returning = false;
                Debug.Log("Back to original rotation");
            }


        }
    }

    void HitTarget(Vector3 pos)
    {
        // audioSource.pitch = 1;
        // audioSource.PlayOneShot(hitSound);
        // GameObject HitEffect = Instantiate(hitEffect, pos, Quaternion.identity);
        // Destroy(HitEffect, 20);
    }



    void OnDrawGizmosSelected()
    {
        if (camera != null)
        {
            // Gizmos.color = Color.red;
            // // Draw the ray in Scene view
            // Gizmos.DrawRay(camera.transform.position, camera.transform.forward * attackDistance);

            // // Optional: Draw a sphere at the end point
            // Vector3 endPoint = camera.transform.position + camera.transform.forward * attackDistance;
            // Gizmos.DrawWireSphere(endPoint, 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(camera.transform.position, attackDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position, 0.1f);
        }
    }
    public float getAttackDistance()
    {
        return attackDistance;
    }
    public float getSpawnMinDistance()
    {
        return minSpawnDistance;
    }
    
     // public void ChangeAnimationState(string newState)
    // {
    //     //Stop same animation from interrupting with itself
    //     if (currentAnimationState == newState) return;

    //     //Play Animation
    //     currentAnimationState = newState;
    //     animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    // }

}
