using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class Physickicker : NetworkBehaviour
{
   public float m_KickRadius = 3f;

    public float m_KickForce = 3f;

    public float m_Upward = 0.4f;


    private void Update()
    {
       if(!IsOwner)
       {
            return;
       }

        Keyboard keyboard = Keyboard.current;
    if (keyboard == null)
    {
            return;
    }

    if (keyboard.kKey.wasPressedThisFrame)
    {
        RequestKickRpc();
    }
    }

    [Rpc(SendTo.Server)]
    private void RequestKickRpc(RpcParams rpcParams = default)
    {
        var colliders = Physics.OverlapSphere(transform.position, m_KickRadius);
        foreach(var collider in colliders)
        {
            Rigidbody body = collider.attachedRigidbody;
            if(body == null || body.isKinematic)
            {
                continue;
            }

            Vector3 dir = transform.forward;
            dir.y = m_Upward;
            dir.Normalize();

            body.AddForce(dir * m_KickForce, ForceMode.Impulse);
        }
    }
}
