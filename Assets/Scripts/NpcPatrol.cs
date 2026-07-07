using UnityEngine;
using Unity.Netcode;
using UnityEngine.AI;

public class NpcPatrol : NetworkBehaviour
{
    private Animator m_Animator;
    private int m_HashIsMoving = Animator.StringToHash("IsMoving");

    private NavMeshAgent m_Agent;
    private float m_PatrolRadius = 5f;

    public override void OnNetworkSpawn()
    {
        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();

        m_Agent.enabled = IsServer;
        if (IsServer)
        {
            PickNewDestination();
        }
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (m_Agent == null || !m_Agent.enabled)
            return;

        if (!m_Agent.pathPending)
        {
            PickNewDestination();
        }
    }

    private void PickNewDestination()
    {
        Vector2 random = Random.insideUnitSphere * m_PatrolRadius;
        random = transform.position  + new Vector3(random.x, 0, random.y);

        if (NavMesh.SamplePosition(random, out NavMeshHit hit, m_PatrolRadius, NavMesh.AllAreas))
        {
            m_Agent.SetDestination(hit.position);
        }
    }
}
