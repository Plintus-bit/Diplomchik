using System.Collections.Generic;
using Enums;
using Interfaces.ReadOnly;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class ProofMenuUI : MonoBehaviour
    {
        [SerializeField] private List<ImageType> _drawings;
        [SerializeField] private CharacterService _charService;
        [SerializeField] private BookmarksManager _bookmarksManager;
        [SerializeField] private Transform _startPlaceTransform;
        [SerializeField] private PlayerInput _input;
        
        private IReadOnlyProofDataForUI _proofService;

        private Vector3 _leftTopLocalPos;
        
        public Canvas proofPanel;
        public List<ProofPlaceUI> _proofs;
        
        public TMP_Text author;
        public TMP_Text description;
        public TMP_Text guiltyPercentage;
        public Image guiltyBtn;

        public Image arrowLeft;
        public Image arrowRight;

        [Range(0, 100)]
        public int drawingAppearChance;

        private void Start()
        {
            _charService = FindObjectOfType<CharacterService>();
            _input = GetComponent<PlayerInput>();
            _leftTopLocalPos = _startPlaceTransform.localPosition;
            proofPanel = GetComponent<Canvas>();
            _proofService = FindObjectOfType<ProofFoundService>();
            BookmarksManager.ProofMenuUI = this;
            author.text = "";
            description.text = "";
            guiltyPercentage.text = "";
            Close();
        }

        public void SetDrawing()
        {
            foreach (var drawing in _drawings)
            {
                drawing.Hide();
            }
            
            var item = Randomizer<ImageType>
                .Randomize(_drawings, drawingAppearChance);
            if (item != null) item.Show();
        }

        public void Open()
        {
            _input.enabled = true;
            FillData();

            _charService.SetInputState(PlayerState.Pause, _charService.PlayerName);
            
            proofPanel.enabled = true;
            SetDrawing();
        }

        public void FillData()
        {
            IReadOnlyAuthor tempAuthor = _proofService.GetCurrAuthor();

            if (tempAuthor == null) return;
            List<IReadOnlyProofData> datas = 
                _proofService.GetAuthorProofs(tempAuthor.AuthorType);

            if (datas == null) return;
            Clear();
            
            SetDataInUIPlaces(datas);

            SetAuthor(tempAuthor);
            CheckArrows();
        }

        private void SetDataInUIPlaces(List<IReadOnlyProofData> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                IReadOnlyProofData data = datas[i];

                int emptyPlaceIndex = GetEmptyPlace();
                if (i < 0) return;
                _proofs[emptyPlaceIndex].Set(data);
                CheckAndMoveProofPlace(emptyPlaceIndex);
                _proofs[emptyPlaceIndex].Show();
            }
        }

        private void MovePlacesToStartPos()
        {
            int lenght = _proofs.Count;
            for (int i = 0; i < lenght; i++)
            {
                int value = i / 2;
                float deltaPosX = 333f * (i % 2);
                float deltaPosY = (145f + 15f) * value;
                
                _proofs[i].transform.localPosition = new Vector3(
                    _leftTopLocalPos.x + deltaPosX,
                    _leftTopLocalPos.y - deltaPosY,
                    _leftTopLocalPos.z
                );
            }
        }
        
        private void CheckAndMoveProofPlace(int index)
        {
            float distanceToMove = 0;
            for (int i = index - 2; i >= 0; i -= 2)
            {
                distanceToMove += _proofs[i].SizeAndBasicSizeDelta();
            }

            if (distanceToMove > 0)
            {
                _proofs[index].transform.localPosition -= new Vector3(
                    0, distanceToMove, 0
                );
            }
        }
        
        public int GetEmptyPlace() {
            for (int i = 0; i < _proofs.Count; i++)
            {
                if (_proofs[i].IsEmpty()) return i;
            }

            return -1;
        }

        public void ChangeAuthor(bool isNext)
        {
            if (!_proofService.ChangeAuthor(isNext)) return;
            FillData();
            _bookmarksManager.ChangeBookmark(_proofService.GetCurrAuthor());
            SetDrawing();
        }
        
        public void ChangeAuthor(string authorName)
        {
            if (!_proofService.ChangeAuthor(authorName)) return;
            FillData();
            _bookmarksManager.ChangeBookmark(_proofService.GetCurrAuthor());
            SetDrawing();
        }

        private void CheckArrows()
        {
            if (!_proofService
                    .IsStartOrLastAuthor(out SwitcherStates state))
            {
                SetArrows(true, true);
                return;
            }

            switch (state)
            {
                case SwitcherStates.Start:
                    SetArrows(false, false);
                    break;
                case SwitcherStates.End:
                    SetArrows(true, false);
                    break;
                case SwitcherStates.StartEnd:
                    SetArrows(false, true);
                    break;
            }
        }

        public void SetAuthor(IReadOnlyAuthor authorData)
        {
            author.text = authorData.Name;
            description.text = authorData.Description;
            int authorGuiltyPercentage = _proofService
                .GetPercentageAuthorGuilty(
                    authorData.AuthorType);
            guiltyPercentage.text = authorGuiltyPercentage + "%/100%";
        }

        public void Close()
        {
            _input.enabled = false;

            if (_charService.HasPlayer())
            {
                _charService.SetInputState(PlayerState.Movable, _charService.PlayerName);   
            }
            
            proofPanel.enabled = false;
            Clear();
            
            foreach (var drawing in _drawings)
            {
                drawing.Hide();
            }
            
            SetArrows(false, true);
        }

        private void SetArrows(bool isLeftArrowOn, bool isLeftRightEqual)
        {
            if (isLeftRightEqual)
            {
                arrowLeft.enabled = isLeftArrowOn;
                arrowRight.enabled = isLeftArrowOn;
            }
            else
            {
                arrowLeft.enabled = isLeftArrowOn;
                arrowRight.enabled = !isLeftArrowOn;
            }
        }

        public void HideProofItems()
        {
            foreach (var proof in _proofs)
            {
                proof.Hide();
            }
        }

        public void Clear()
        {
            MovePlacesToStartPos();
            foreach (var proof in _proofs)
            {
                proof.Clear();
                proof.Hide();
            }
        }
    }
}