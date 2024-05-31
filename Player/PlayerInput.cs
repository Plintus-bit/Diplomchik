using System.Collections.Generic;
using Enums;
using Interfaces;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private GameSceneManager _gameSceneManager;
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private PlayerMovement _playerMovement;
        private IInteractManager _interactManager;
        
        private PlayerState _state;
        private Stack<PlayerState> _statesStack;

        public DialogSystem dialogSystem;
        
        public PlayerState State
        {
            get => _state;
            set
            {
                if (_statesStack == null) return;
                
                if (value == PlayerState.Movable)
                {
                    if (_statesStack.TryPeek(out PlayerState lastState))
                    {
                        _state = lastState;
                        _statesStack.Pop();
                    }
                    else
                    {
                        _state = value;
                        _statesStack.Clear();
                    }
                }
                else
                {
                    _statesStack.Push(_state);
                    _state = value;
                }
            }
        }

        private void Awake()
        {
            _statesStack = new Stack<PlayerState>();
            _state = PlayerState.Movable;
        }

        void Start()
        {
            _statesStack = new Stack<PlayerState>();
            _playerInteraction = GetComponent<PlayerInteraction>();
            _playerMovement = GetComponent<PlayerMovement>();
            _interactManager = GetComponentInChildren<IInteractManager>();
            dialogSystem = FindObjectOfType<DialogSystem>();
        }

        private void FixedUpdate()
        {
            if (_state == PlayerState.Transition) return;
            if (_state == PlayerState.Movable)
            {
                Move();
            }
        }

        private void Update()
        {
            if (_state == PlayerState.Transition
                || _state == PlayerState.Pause) return;
            
            PauseGame();
            if (_state == PlayerState.Movable) Jump();
            Interact();
        }

        public void PauseGame()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _gameSceneManager.Pause();
            }
        }
        
        public void Move()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            if (moveHorizontal != 0)
            {
                _playerMovement.Move(moveHorizontal);
            }
        }

        public void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerMovement.Jump();
            }
        }

        public void Interact()
        {
            if (_state == PlayerState.InDialog)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (dialogSystem != null) dialogSystem.Play();
                }
                return;
            }
            
            if (_state != PlayerState.Movable) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                _playerInteraction.Interact();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _interactManager.ChangeSelected(false);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                _interactManager.ChangeSelected(true);
            }
        }
    }
}
