using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class RPCDemo : NetworkBehaviour
{
    public int m_ActionId = 1;

    private void Update()
    {
        if (!IsOwner)
            return;

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.fKey.wasPressedThisFrame)
        {
            RequestActionRpc(m_ActionId);
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestActionRpc(int actionId, RpcParams rpcParam = default)
    {
        ulong senderClientId = rpcParam.Receive.SenderClientId;
        if (actionId <= 0)
        {
            Debug.LogWarning($"[Server] 잘못된 actionId: {actionId} (clientId: {senderClientId})");
            return;
        }
        Debug.Log($"[Server] clientId = {senderClientId}, 액션 = {actionId}");

        AnnounceActionRpc(senderClientId, actionId);

        AckRpc(actionId, RpcTarget.Single(senderClientId, RpcTargetUse.Temp));
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void AnnounceActionRpc(ulong actorClientId, int actionId)
    {
        bool isMine = actorClientId == NetworkManager.LocalClientId;

        Debug.Log($"[Client/Host] clientId = {actorClientId}, 액션 = {actionId}, IsMine? {isMine}");
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void AckRpc(int actionId, RpcParams rpcParams)
    {
        Debug.Log($"[Client] 서버 응답 수신: action {actionId} 처리 완료");
    }
}
