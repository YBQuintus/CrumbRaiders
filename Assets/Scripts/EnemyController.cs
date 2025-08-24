using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Vector3[] patrolPoints;
    [SerializeField] private float stopTime;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float detectionAngle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(GameManager.Instance.GetPlayerPosition());
    }
}

