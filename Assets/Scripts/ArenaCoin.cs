using Unity.Netcode;
using UnityEngine;

namespace NetworkStudy.Gameplay
{
    public class ArenaCoin : NetworkBehaviour
    {
        [SerializeField]
        private float m_PickupRadius = 1.5f;

        [SerializeField]
        private int m_ScoreValue = 1;

        private bool m_Collected;

        [Rpc(SendTo.Server)]
        public void RequestPickupRpc(RpcParams rpcParams = default)
        {
            if(m_Collected || !IsSpawned)
            {
                return;
            }
            
            ulong senderClientId = rpcParams.Receive.SenderClientId;
            NetworkObject playerObject = NetworkManager.SpawnManager.GetPlayerNetworkObject(senderClientId);
            if (playerObject == null)
            {
                Debug.LogWarning($"플레이어 오브젝트 탐색 실패 {senderClientId}");
                return;
            }

            float distance = Vector3.Distance(playerObject.transform.position, transform.position);
            if (distance > m_PickupRadius)
            {
                Debug.LogWarning($"줍기 거리 실패 {distance}");
                return;
            }

            m_Collected = true;
            CoinArenaManager manager = FindFirstObjectByType<CoinArenaManager>();
            manager.ServerAwardPoint(senderClientId, m_ScoreValue);

            NetworkObject.Despawn();
        }
    }
}
