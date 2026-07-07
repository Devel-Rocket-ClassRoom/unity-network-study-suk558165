using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : NetworkBehaviour
{
   private Animator m_Animator;

   private NetworkAnimator m_NetworkAnimator;

   private static readonly int m_HashIsMoving = Animator.StringToHash("IsMoving");

   private static readonly int m_HashTaunt = Animator.StringToHash("Taunt");

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_NetworkAnimator = GetComponent<NetworkAnimator>();
    }

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

       float move = 0f;
       float turn = 0f;

       if (keyboard.wKey.isPressed)
            move += 1f;
       if (keyboard.sKey.isPressed)
            move += -1f;
       if (keyboard.dKey.isPressed)
            turn += 1f;
       if (keyboard.aKey.isPressed)
            turn += -1f;

       m_Animator.SetBool(m_HashIsMoving, move != 0f);

         if (keyboard.tKey.wasPressedThisFrame)
         {
            m_NetworkAnimator.SetTrigger(m_HashTaunt);
         }
    }

   
}
