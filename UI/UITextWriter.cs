using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITextWriter : MonoBehaviour
{
    public class UIWriteText
    {
        public TMP_Text _textPlace;
        public string _textToWrite;
        public int _characterIndex;
        public float _timePerChar;
        public float _timer;
        
        private static Dictionary<char, float> _timeScalerPerChar =
            new Dictionary<char, float>()
            {
                { '.', 15f },
                { '?', 10f },
                { '!', 10f },
                { '-', 3f },
                { ',', 6f },
            };

        private static char _startIgnoreChars = '<';
        private static char _endIgnoreChars = '>';

        private bool _isIgnoring = false;

        public bool Update()
        {
            if (_textPlace == null) return false;

            if (IsIgnore())
            {
                _characterIndex += 1;
                return true;
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                Write();
                if (IsEndWrite())
                {
                    EndWrite();
                    return false;
                }
            }

            return true;
        }

        public UIWriteText()
        {
            _textPlace = null;
            _isIgnoring = false;
            _characterIndex = -1;
        }
        
        public UIWriteText(
            TMP_Text textPlace, string text, float timePerChar)
        {
            SetWritter(textPlace, text, timePerChar);
        }

        public void SetWritter(
            TMP_Text textPlace, string text, float timePerChar)
        {
            _textPlace = textPlace;
            _textToWrite = text;
            _timePerChar = timePerChar;
            _characterIndex = 0;

            _timer = 0;
        }

        public void Write()
        {
            _timer += _timePerChar;
            _timer += _timePerChar * GetCharTimeScaler();
            _characterIndex += 1;
            _textPlace.text = _textToWrite.Substring(0, _characterIndex);
            _textPlace.text += "<color=#00000000>"
                               + _textToWrite.Substring(_characterIndex)
                               + "</color>";
        }

        public bool IsEndWrite()
        {
            if (_textPlace == null) return true;
            return _characterIndex >= _textToWrite.Length;
        }

        public void EndWrite()
        {
            if (_textPlace == null) return;
            _textPlace.text = _textToWrite;
            _isIgnoring = false;
            _textPlace = null;
        }

        public float GetCharTimeScaler()
        {
            char charForTimeScale = _textToWrite[_characterIndex];
            if (_timeScalerPerChar.ContainsKey(charForTimeScale))
            {
                return _timeScalerPerChar[charForTimeScale];
            }

            return 0;
        }

        public bool IsIgnore()
        {
            if (_characterIndex >= _textToWrite.Length) return false;
            
            char tempChar = _textToWrite[_characterIndex];
            if (!_isIgnoring && _startIgnoreChars == tempChar)
            {
                _isIgnoring = true;
                return true;
            }

            if (_isIgnoring && _endIgnoreChars != tempChar)
            {
                _isIgnoring = true;
                return true;
            }

            _isIgnoring = false;
            return false;
        }
    }

    // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
    // public List<UIWriteText> _writers;

    public UIWriteText _writer;

    private void Start()
    {
        // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
        // _writers = new List<UIWriteText>();
        
        _writer = new UIWriteText();
    }

    private void Update()
    {
        // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
        // for (int i = 0; i < _writers.Count; i++)
        // {
        //     if (!_writers[i].Update())
        //     {
        //         _writers.RemoveAt(i);
        //         i -= 1;
        //     }
        // }

        _writer.Update();
    }

    public void AddToWriter(TMP_Text textPlace, string text, float time)
    {
        // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
        // if (HasWriter(textPlace)) return;
        //
        // _writers.Add(new UIWriteText(textPlace, text, time));
        _writer.SetWritter(textPlace, text, time);
    }

    public bool HasWriter(TMP_Text textPlace)
    {
        // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
        // if (_writers.Count == 0) return false;
        // foreach (var item in _writers)
        // {
        //     if (item._textPlace == textPlace) return true;
        // }
        
        return false;
    }

    public bool TryGetItem(TMP_Text textPlace, out UIWriteText writeItem)
    {
        // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
        // if (_writers.Count == 0)
        // {
        //     writeItem = null;
        //     return false;
        // }
        // foreach (var item in _writers)
        // {
        //     if (item._textPlace == textPlace)
        //     {
        //         writeItem = item;
        //         return true;
        //     }
        // }
        
        writeItem = null;
        return false;
    }

    public bool IsEndWrite(TMP_Text textPlace)
    {
        // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
        // if (TryGetItem(textPlace, out UIWriteText item))
        //     return item.IsEndWrite();
        // return true;

        return _writer.IsEndWrite();
    }
    
    public void EndWrite(TMP_Text textPlace)
    {
        // UNCOMMENT IF WILL NEED MULTIPLE WRITERS
        // if (TryGetItem(textPlace, out UIWriteText item)) item.EndWrite();
        
        _writer.EndWrite();
    }
    
}
