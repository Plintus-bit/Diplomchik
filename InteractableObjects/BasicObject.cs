using System;
using Enums;
using Interfaces;
using Managers;
using Managers.Notifiers;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace InteractableObjects
{
    public abstract class BasicObject : MonoBehaviour, IInteractable, ILockable,
        IInteractNotifier, IInteractListener, IResponcer
    {
        private bool _isSelected;
        private bool _isInteractable = true;

        protected static ICharacterService _charService;

        [SerializeField] private Canvas canvas;
        [SerializeField] private Image UIIcon;
        [SerializeField] protected string IconName = String.Empty;
        private Sprite _UIActive;
        private Sprite _UIInactive;
        private Collider _collider;

        protected static LockerInitializer _lockerInitializer;
        private static Canvas _canvasPrefab;

        protected static InformerInitializer _informerInitializer;

        public string lockerId;
        public string informerId;

        public string InformerId => informerId;

        public GameObject visualItem;
        public bool needHideVisual = false;

        public bool IsInteractable
        {
            get => _isInteractable;
            set => _isInteractable = value;
        }
        
        private void Awake()
        {
            if (_canvasPrefab == null) LoadCanvasPrefab();
            if (IconName == string.Empty)
            {
                SetIconNames();   
            }
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            SetCanvas();
            
            UIIcon.color = Color.white;
            InitializeUIResources();
            canvas.enabled = false;
            if (_charService == null)
            {
                SetCharacterService();
            }

            _lockerInitializer ??= LockerInitializer.GetInstance();
            _informerInitializer ??= InformerInitializer.Instance;
            
            if (!string.IsNullOrEmpty(lockerId))
            {
                _lockerInitializer.AddItemToLocker(lockerId, this);
            }
            
            Initialize();
        }

        private void SetCanvas()
        {
            canvas = GetComponentInChildren<Canvas>();
            if (canvas == null)
            {
                canvas = Instantiate(_canvasPrefab, transform);
            }
            UIIcon = canvas.GetComponentInChildren<Image>();
        }

        private void LoadCanvasPrefab()
        {
            _canvasPrefab = Resources.Load<GameObject>("UI/ItemCanvas")
                .GetComponent<Canvas>();
            RectTransform canvasRect = _canvasPrefab
                .gameObject.GetComponent<RectTransform>();
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
            IconName = "ExploreIcon";
        }

        protected virtual void Initialize()
        {
            // do nothing
        }

        public virtual bool Interact(string who)
        {
            if (who == null) Interact();
            return IsInteractable;
        }

        public virtual bool Interact()
        {
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

        public float XPos => gameObject.transform.position.x;

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

        public virtual void Lock()
        {
            IsInteractable = false;
            _collider.enabled = false;

            if (visualItem != null && needHideVisual)
            {
                visualItem.SetActive(false);
            }
        }

        public virtual void Unlock()
        {
            IsInteractable = true;
            _collider.enabled = true;
            
            if (visualItem != null && needHideVisual)
            {
                visualItem.SetActive(true);
            }
        }
        
        public virtual void DestroyObject(bool needNotify = true)
        {
            IsInteractable = false;
            if (needNotify) Notify();
            Destroy(gameObject);
        }

        public void Notify()
        {
            InteractablesNotifier
                .Notify(this, IsInteractable);
        }

        public void Notify(bool status)
        {
            InteractablesNotifier
                .Notify(this, status);
        }
        
        public virtual void ReactOnNotify(IInteractable obj, bool isInteractable)
        {
            // pass
        }

        public void RegistrListener()
        {
            InteractablesNotifier.RegistrListener(this);
        }

        public void UnregistrListener()
        {
            InteractablesNotifier.UnregistrListener(this);
        }
        public virtual void Responce(string whoInformer,
            InformStatus status = InformStatus.End) {}
    }
}