using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float jumpForce = 100f;
        
        private Rigidbody _rb;
    
        private bool _isGround;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _isGround = true;
        }

        public void Move(float moveDir)
        {
            Vector3 moveVector = new Vector3(moveDir, 0.0f, 0.0f);
            _rb.MovePosition(transform.position + moveVector * speed);
        }

        public void Jump()
        {
            if (_isGround)
            {
                _rb.AddForce(Vector3.up * jumpForce);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            GroundUpdate(collision, true);
        }
    
        private void OnCollisionExit(Collision other)
        {
            GroundUpdate(other, false);
        }

        private void GroundUpdate(Collision collision, bool value)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isGround = value;
            }
        }
    }
}
