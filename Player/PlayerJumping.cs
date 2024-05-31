using System;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class PlayerJumping : MonoBehaviour
    {
        [SerializeField] private float _jumpForce = 100f;
        [SerializeField] private int _maxJumps = 1;
        [SerializeField] private long _maxTicksDelay = 15;

        private Rigidbody _rb;

        private bool _isGround;
        private int _jumpBuffer;
        
        private int _lastExitGroundTime;
        private int _lastJumpTime;
        private int _jumpQueue;

        private void Awake()
        {
            _jumpBuffer = _maxJumps;
            _jumpQueue = 0;
            _isGround = true;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag("Ground"))
            {
                _isGround = true;
                _jumpBuffer = _maxJumps;
                int delta = UsefullDataGetter
                    .GetDeltaTicks(_lastJumpTime, 7, 4);
                if (delta > 0 && delta < _maxTicksDelay
                    && _jumpQueue > 0)
                {
                    Jump();
                }
                _jumpQueue = 0;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.CompareTag("Ground"))
            {
                _isGround = false;
                _lastExitGroundTime = UsefullDataGetter.GetUsefulTicksData(7, 4);
            }
        }

        public void Jump()
        {
            if (_rb.velocity.y == 0) _jumpBuffer = _maxJumps;
            
            if (_jumpBuffer == _maxJumps)
            {
                int delta = UsefullDataGetter
                    .GetDeltaTicks(_lastExitGroundTime, 7, 4);
                bool checkAdditionPart = delta > 0 && delta < _maxTicksDelay;
                checkAdditionPart = checkAdditionPart || _rb.velocity.y == 0;
                if ((!_isGround &&
                     checkAdditionPart)
                    || _isGround)
                {
                    _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
                    _jumpBuffer -= 1;
                }
            }
            else if (_jumpBuffer > 0 && _jumpBuffer < _maxJumps)
            {
                _rb.AddForce(Vector3.up * _jumpForce / 1.5f, ForceMode.VelocityChange);
                _jumpBuffer -= 1;
            }
            else
            {
                _jumpQueue = 1;
            }
            _lastJumpTime = UsefullDataGetter.GetUsefulTicksData(7, 4);
        }
        
        public void SetRigidBody(Rigidbody rb)
        {
            _rb = rb;
        }
    }
}