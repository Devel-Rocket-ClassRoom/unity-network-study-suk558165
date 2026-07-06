using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NetworkStudy.Student
{
    public class MyPlayerMovement : NetworkBehaviour
    {
        [Tooltip("초당 이동 속도(월드 유닛).")]
        [SerializeField]
        private float m_MoveSpeed = 5f;

        [Tooltip("초당 회전 속도(도).")]
        [SerializeField]
        private float m_RotateSpeed = 120f;

        private bool m_IsJumping = false;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Debug.Log($"[MyPlayerMovement] 내 플레이어 스폰 {OwnerClientId}");
            }
        }

        private void Update()
        {
            if (!IsOwner)
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
            float currentSpeed = keyboard.nKey.isPressed ? m_MoveSpeed * 2f : m_MoveSpeed;

            if (keyboard.wKey.isPressed)
            {
                move += 1f;
            }
            if (keyboard.sKey.isPressed)
            {
                move -= 1f;
            }
            if (keyboard.dKey.isPressed)
            {
                turn += 1f;
            }
            if (keyboard.aKey.isPressed)
            {
                turn -= 1f;
            }

            if (keyboard.bKey.wasPressedThisFrame && !m_IsJumping)
            {
                StartCoroutine(JumpRoutine());
            }

            transform.Rotate(0f, turn * m_RotateSpeed * Time.deltaTime, 0f);
            transform.Translate(0f, 0f, move * currentSpeed * Time.deltaTime);
        }

        private IEnumerator JumpRoutine()
        {
            m_IsJumping = true;

            float elapsed = 0f;
            float duration = 3f; // 올라갔다 내려오는 총 시간
            Vector3 startPos = transform.position;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                // 0→1→0 포물선 (sin 곡선)
                float height = Mathf.Sin(t * Mathf.PI) * 3f;
                transform.position = new Vector3(
                    transform.position.x,
                    startPos.y + height,
                    transform.position.z
                );
                yield return null;
            }

            // 착지 시 y 정확히 원위치
            transform.position = new Vector3(
                transform.position.x,
                startPos.y,
                transform.position.z
            );
            m_IsJumping = false;
        }
    }
}
