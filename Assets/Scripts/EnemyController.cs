using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    private InputAction m_CrouchAction;
    private InputAction m_SprintAction;
    private NavMeshAgent agent;
    private Light m_Light;
    private AudioSource arrestSound;

    [SerializeField] private Vector3[] patrolPoints;
    [SerializeField] private float stopTime;
    [SerializeField] private float detectionTime;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float detectionAngle;
    [SerializeField] private Vector3 m_LastKnownPosition = Vector3.zero;
    [SerializeField] private float m_DetectionTimer = 0f;
    private Vector3 m_CurrentPatrolPoint;
    private float m_WaitTime = 0;
    public float TempDetectionDistance;
    public float TempDetectionAngle;
    public static bool SpottingOverride;
    
    
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        TempDetectionDistance = detectionDistance;
        TempDetectionAngle = detectionAngle;
        StartCoroutine(GetPoint());
        m_CurrentPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        SpottingOverride = false;
        m_Light = GetComponentInChildren<Light>();
        arrestSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        m_CrouchAction = InputSystem.actions.FindAction("Crouch");
        m_SprintAction = InputSystem.actions.FindAction("Sprint");
        Patrol();
    }

    private void FixedUpdate()
    {
        if (PlayerInSight())
        {
            m_DetectionTimer += Time.deltaTime;
            m_LastKnownPosition = GameManager.Instance.GetPlayerPosition();
            LookAtPlayer();
            m_WaitTime = 3f;
        }
        else if (m_DetectionTimer > 0.125f)
        {
            m_LastKnownPosition = GameManager.Instance.GetPlayerPosition();
            m_DetectionTimer -= Time.deltaTime;
            m_WaitTime -= Time.deltaTime;
        }
        else
        {
            m_DetectionTimer -= Time.deltaTime;
            m_WaitTime -= Time.deltaTime;
        }
        if (m_LastKnownPosition != Vector3.zero && m_DetectionTimer > 0 || m_WaitTime > 0)
        {
            if (m_DetectionTimer >= detectionTime)
            {
                m_DetectionTimer = detectionTime;
                Chase();
                m_Light.color = Color.darkRed;
            }
            else if (m_DetectionTimer > 0f)
            {
                m_Light.color = Color.softYellow;
            }
        }
        else
        {
            if (m_DetectionTimer <= 0)
            {
                m_DetectionTimer = 0;
            }
            Patrol();
            m_Light.color = Color.darkGreen;
        }
    }

    private bool PlayerInSight()
    {
        TempDetectionDistance = detectionDistance;
        if (m_SprintAction.IsPressed())
        {
            TempDetectionDistance *= 1.5f;
        }
        else if (m_CrouchAction.IsPressed())
        {
            TempDetectionDistance *= 0.5f;
        }
        Vector3 directionToPlayer = GameManager.Instance.GetPlayerPosition() - (transform.position + Vector3.up);
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (directionToPlayer.magnitude < 3f || SpottingOverride)
        {
            return true;
        }   
        if (directionToPlayer.magnitude < TempDetectionDistance && angle < detectionAngle)
        {
            if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out RaycastHit hit, TempDetectionDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;
        detectionAngle = TempDetectionAngle;
        agent.SetDestination(m_CurrentPatrolPoint);
    }

    IEnumerator GetPoint()
    {
        while (true)
        {
            yield return new WaitForSeconds(stopTime);
            if (agent.remainingDistance < 0.1f)
            {
                Vector3 newPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
                while (m_CurrentPatrolPoint == newPatrolPoint)
                {
                    newPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
                }
                m_CurrentPatrolPoint = newPatrolPoint;
                m_LastKnownPosition = Vector3.zero;
            } 
            yield return null;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = m_LastKnownPosition - transform.position;
        Vector3 lookPosition = Vector3.RotateTowards(transform.forward, directionToPlayer, Time.deltaTime * 3f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPosition), 0.8f);
        agent.speed = 0f;
    }
    private void Chase()
    {
        agent.SetDestination(m_LastKnownPosition);
        agent.speed = chaseSpeed;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!arrestSound.isPlaying)
        {
            arrestSound.Play();
        }
        GameManager.Instance.EndGame(0); 
    }
}
