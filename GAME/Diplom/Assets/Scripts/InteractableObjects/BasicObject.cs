using Interfaces;
using Managers;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace InteractableObjects
{
    public class BasicObject : MonoBehaviour, IInteractable
    {
        private bool _isSelected;
        private bool _isInteractable;
        
        protected static ICharacterService _charService;

        public bool IsInteractable
        {
            get => _isInteractable;
            set => _isInteractable = value;
        }

        [SerializeField] private Canvas canvas;
        [SerializeField] private Image UIIcon;
        protected string IconName;
        private Sprite _UIActive;
        private Sprite _UIInactive;

        private void Awake()
        {
            SetIconNames();
        }

        private void Start()
        {
            canvas = GetComponentInChildren<Canvas>();
            UIIcon = GetComponentInChildren<Image>();
            UIIcon.color = Color.white;
            InitializeUIResources();
            canvas.enabled = false;
            IsInteractable = true;
            Initialize();
        }

        public void InitializeUIResources()
        {
            _UIActive = Resources.Load<Sprite>("UI/Active" + IconName);
            _UIInactive = Resources.Load<Sprite>("UI/Inactive" + IconName);
        }

        public void SetCharacterService()
        {
            _charService = FindObjectOfType<CharacterService>();
        }
        
        public virtual void SetIconNames()
        {
            IconName = "ObjIcon";
        }

        protected virtual void Initialize()
        {
            if (_charService == null)
            {
                SetCharacterService();
            }
        }

        public virtual bool Interact(string who)
        {
            IsInteractable = false;
            return IsInteractable;
        }

        public void SetActive(bool isActive)
        {
            canvas.enabled = isActive;
            ChangeUI();
        }
        
        public void SetSelected(bool isSelect)
        {
            _isSelected = isSelect;
            ChangeUI();
        }

        public void ChangeUI()
        {
            if (_isSelected)
            {
                UIIcon.sprite = _UIActive;
            }
            else
            {
                UIIcon.sprite = _UIInactive;
            }
        }
    }
}