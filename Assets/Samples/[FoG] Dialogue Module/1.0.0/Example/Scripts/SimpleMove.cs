using UnityEngine;
using UnityEngine.InputSystem;

namespace Fog.Dialogue.Samples
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SimpleMove : MonoBehaviour
    {
        [SerializeField, Range(0, 100)] private float moveSpeed;
        public bool canMove = true;
        public static SimpleMove instance = null;
        [SerializeField] InputActionReference movementAction;
        private Rigidbody2D rigid;

        void Awake() {
            rigid = GetComponent<Rigidbody2D>();
            if(instance == null) {
                instance = this;
            } else {
                Destroy(this);
            }
        }

        void OnDestroy() {
            if(instance == this) {
                instance = null;
            }
        }

        public void BlockMovement() {
            canMove = false;
            rigid.velocity = Vector2.zero;
        }

        public void AllowMovement() {
            canMove = true;
        }

        void Update() {
            if (canMove)
            {
                Vector2 speed = movementAction.action.ReadValue<Vector2>();
                speed *= moveSpeed;
                rigid.velocity = speed;
            }
            else
            {
                rigid.velocity = Vector2.zero;
            }
        }
    }
}