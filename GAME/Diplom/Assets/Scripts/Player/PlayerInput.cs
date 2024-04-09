using Enums;
using Interfaces;
using Player;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerInteraction playerInteraction;
        [SerializeField] private PlayerMovement playerMovement;
        private IInteractManager _interactManager;
        
        private PlayerState _state;

        public PlayerState State
        {
            get => _state;
            set => _state = value;
        }

        void Start()
        {
            playerInteraction = GetComponent<PlayerInteraction>();
            playerMovement = GetComponent<PlayerMovement>();
            _interactManager = GetComponentInChildren<IInteractManager>();
            _state = PlayerState.Movable;
        }

        private void FixedUpdate()
        {
            if (_state == PlayerState.Transition) return;
            Move();
        }

        private void Update()
        {
            if (_state == PlayerState.Transition) return;
            Interact();
            Jump();
        }

        public void Move()
        {
            if (_state == PlayerState.Movable)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                if (moveHorizontal != 0)
                {
                    playerMovement.Move(moveHorizontal);
                }
            }
        }

        public void Jump()
        {
            if (_state == PlayerState.Movable)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerMovement.Jump();
                }
            }
        }

        public void Interact()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerInteraction.Interact();
            }

            if (_state != PlayerState.InDialog)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    _interactManager.ChangeSelected(true);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    _interactManager.ChangeSelected(false);
                }
            }
        }
    }
}
