using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private PlayerJumping jump;
        [SerializeField] private Transform player;
        
        private Rigidbody _rb;

        private bool _isRight;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            jump = GetComponentInChildren<PlayerJumping>();
            jump.SetRigidBody(_rb);
            _isRight = true;
        }

        public void Move(float moveDir)
        {
            if (moveDir is < 0 and >= -1f && !_isRight)
            {
                Rotate(1);
                _isRight = true;
            }
            else if (moveDir is > 0 and <= 1f && _isRight)
            {
                Rotate(-1);
                _isRight = false;
            }
            
            Vector3 moveVector = new Vector3(moveDir, 0.0f, 0.0f);
            _rb.MovePosition(transform.position + moveVector * speed);
        }

        public void Jump()
        {
            jump.Jump();
        }

        public void Rotate(int dir)
        {
            player
                .LeanRotateY(dir * 85, 0.24f)
                .setEaseOutSine();
        }
    }
}
