using System.Collections.Generic;
using Interfaces.ReadOnly;
using UI;
using UnityEngine;

public class BookmarksManager : MonoBehaviour
{
    [SerializeField] private Bookmark _prefab;
    
    private static ProofMenuUI _proofMenuUI;
    
    private List<Bookmark> _bookmarks;

    private float _verticalMargin;
    private float _margin = 6f;

    private Bookmark _currActiveBookmark;

    public static ProofMenuUI ProofMenuUI
    {
        set => _proofMenuUI = value;
        get => _proofMenuUI;
    }

    private void Awake()
    {
        _bookmarks = new List<Bookmark>();
    }

    private void Start()
    {
        _verticalMargin = _prefab.GetComponent<RectTransform>().rect.height;
        Bookmark.parent = this;
    }

    public void CreateBookmark(IReadOnlyAuthor author)
    {
        Bookmark bookmark = Instantiate(_prefab, transform);
        int lenght = _bookmarks.Count;
        float newYPos = lenght * _margin + lenght * _verticalMargin;
        bookmark.transform.localPosition = new Vector3(
            bookmark.transform.localPosition.x,
            bookmark.transform.localPosition.y - newYPos, 0);
        
        _bookmarks.Add(bookmark);
        
        bookmark.Set(author.Name,
            Resources.Load<Sprite>(
                "UI/" + author.AuthorType + "_bookmark"));

        if (_bookmarks.Count == 1)
        {
            SetActiveBookmark(bookmark);
        }

    }

    public void ChangeBookmark(IReadOnlyAuthor author)
    {
        if (_currActiveBookmark.name.text == author.Name) return;
        foreach (var bookmark in _bookmarks)
        {
            if (bookmark.name.text == author.Name)
            {
                SetActiveBookmark(bookmark);
                return;
            }
        }
    }

    public void SetActiveBookmark(Bookmark bookmark)
    {
        if (_currActiveBookmark != null) _currActiveBookmark.Close();
        _currActiveBookmark = bookmark;
        _currActiveBookmark.Open();
    }

    public void OnBookmarkClick(string authorName)
    {
        _proofMenuUI.ChangeAuthor(authorName);
    }
    
}
