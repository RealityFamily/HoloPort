using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows.Speech;
using NativeKeywordRecognizer = UnityEngine.Windows.Speech.KeywordRecognizer;

namespace HoloGroup.UserInteraction.Input.Voice
{
    /// <summary>
    /// keywordRecognizer is wrapper for UnityEngine.Windows.Speech.KeywordRecognizer.
    /// This class can dynamicly add, remove keywords, change emulation keys for keywords
    /// and add or remove listeners. You must call Restart whenewer you change recognizable keywords.
    /// </summary>
    public class KeywordRecognizer : MonoBehaviour
    {
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        [Serializable]
        public struct KeywordAndAction
        {
            public string keyword;
            public KeyCode emulationKey;
            public UnityEvent action;
        }
        #endregion

        #region Classes
        #endregion

        #region Fields
        [SerializeField]
        private bool _isAutoStart;

        private NativeKeywordRecognizer _keywordRecognizer;

        [SerializeField]
        private List<KeywordAndAction> _keywordsAndActions;
        #endregion

        #region Events
        #endregion

        #region Properties
        public string[] Keywords { get { return _keywordsAndActions.Select(k => k.keyword).ToArray(); } }

        public bool IsRunning { get { return _keywordRecognizer == null ? false : _keywordRecognizer.IsRunning; } }
        #endregion

        #region Methods
        private void Start()
        {
            if (_isAutoStart)
            {
                AcceptKeywordsToNewRecognizer();
            }
        }

        private void OnDisable()
        {
            if (_keywordRecognizer != null && _keywordRecognizer.IsRunning)
            {
                _keywordRecognizer.Stop();
            }
        }

        private void OnEnable()
        {
            if (_keywordRecognizer != null && !_keywordRecognizer.IsRunning)
            {
                _keywordRecognizer.Start();
            }
        }

        private void OnDestroy()
        {
            KillRecognizer();
        }

        private void Update()
        {
            KeyCode[] keys = (from k in _keywordsAndActions select k.emulationKey).ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                if (UnityEngine.Input.GetKeyDown(keys[i]) && _keywordsAndActions[i].action != null && _keywordRecognizer != null && _keywordRecognizer.IsRunning)
                {
                    CallActionsByKeyword(_keywordsAndActions[i].keyword);
                }
            }
        }

        public void AcceptKeywordsToNewRecognizer()
        {
            if (_keywordsAndActions.Count == 0)
            {
                Debug.LogError("Keywords list is empty");
                return;
            }

            if (_keywordRecognizer != null)
            {
                KillRecognizer();
            }

            string[] keywords = (from k in _keywordsAndActions select k.keyword).ToArray();

            _keywordRecognizer = new NativeKeywordRecognizer(keywords);
            _keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
            _keywordRecognizer.Start();
        }

        private void KillRecognizer()
        {
            if (_keywordRecognizer == null)
            {
                return;
            }

            if (_keywordRecognizer.IsRunning)
            {
                _keywordRecognizer.Stop();
            }

            _keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
            _keywordRecognizer.Dispose();

            _keywordRecognizer = null;
        }

        public void ContinueRecognizer()
        {
            if (_keywordRecognizer == null)
            {
                Debug.LogError("KeywordRecognizer not created");
                return;
            }

            if (_keywordRecognizer.IsRunning)
            {
                return;
            }

            _keywordRecognizer.Start();
        }

        public void StopRecognizer()
        {
            if (_keywordRecognizer == null)
            {
                Debug.LogError("KeywordRecognizer not created");
                return;
            }

            if (!_keywordRecognizer.IsRunning)
            {
                return;
            }

            _keywordRecognizer.Stop();
        }

        public void AddKeyword(string keyword)
        {
            AddKeyword(keyword, KeyCode.None, null);
        }

        public void AddKeyword(string keyword, KeyCode emulationkey)
        {
            AddKeyword(keyword, emulationkey, null);
        }

        public void AddKeyword(string keyword, Action action)
        {
            AddKeyword(keyword, KeyCode.None, action);
        }

        public void AddKeyword(string keyword, KeyCode emulationkey, Action action)
        {
            List<string> keywords = (from k in _keywordsAndActions select k.keyword).ToList();

            if (keywords.Contains(keyword))
            {
                Debug.LogErrorFormat("KeywordRecognizer already have keyword \"{0}\"", keyword);
                return;
            }

            KeywordAndAction keywordAndAction = new KeywordAndAction() { keyword = keyword, emulationKey = emulationkey, action = new UnityEvent() };

            if (action != null)
            {
                keywordAndAction.action.AddListener(new UnityAction(action));
            }

            _keywordsAndActions.Add(keywordAndAction);
        }

        public void RemoveKeyword(string keyword)
        {
            int index = _keywordsAndActions.FindIndex(x => x.keyword == keyword);

            if (index == -1)
            {
                Debug.LogErrorFormat("Keyword \"{0}\" not exist in recognizable keywords list", keyword);
                return;
            }

            _keywordsAndActions.RemoveAt(index);
        }

        public void RemoveAllKeywords()
        {
            _keywordsAndActions.Clear();
        }

        public void ChangeKeywordEmulationKey(string keyword, KeyCode newEmulationKey)
        {
            int index = _keywordsAndActions.FindIndex(x => x.keyword == keyword);

            if (index == -1)
            {
                Debug.LogErrorFormat("Keyword \"{0}\"not exist in recognizable keywords list", keyword);
                return;
            }

            KeywordAndAction keywordAndActions = _keywordsAndActions[index];
            keywordAndActions.emulationKey = newEmulationKey;
            _keywordsAndActions[index] = keywordAndActions;
        }

        public void AddKeywordAction(string keyword, Action action)
        {
            if (action == null)
            {
                Debug.LogErrorFormat("Action can not be null", keyword);
                return;
            }

            int index = _keywordsAndActions.FindIndex(x => x.keyword == keyword);

            if (index == -1)
            {
                Debug.LogErrorFormat("Keyword \"{0}\"not exist in recognizable keywords list", keyword);
                return;
            }

            KeywordAndAction keywordAndActions = _keywordsAndActions[index];
            keywordAndActions.action.AddListener(new UnityAction(action));

            // We not need write _keywordsAndActions[index] = keywordAndActions;
            // because keywordAndActions.action is reference type.
        }

        public void RemoveKeywordAction(string keyword, Action action)
        {
            if (action == null)
            {
                Debug.LogErrorFormat("Action can not be null", keyword);
                return;
            }

            int index = _keywordsAndActions.FindIndex(x => x.keyword == keyword);

            if (index == -1)
            {
                Debug.LogErrorFormat("Keyword \"{0}\" not exist in recognizable keywords list", keyword);
                return;
            }

            KeywordAndAction keywordAndActions = _keywordsAndActions[index];
            keywordAndActions.action.RemoveListener(new UnityAction(action));

            // We not need write _keywordsAndActions[index] = keywordAndActions;
            // because keywordAndActions.action is reference type.
        }

        private void CallActionsByKeyword(string keyword)
        {
            List<string> keywords = (from k in _keywordsAndActions select k.keyword).ToList();

            if (!keywords.Contains(keyword))
            {
                Debug.LogErrorFormat("Keyword \"{0}\" recogized, but not exist in recognizable keywords list", keyword);
                return;
            }

            int index = keywords.IndexOf(keyword);

            if (_keywordsAndActions[index].action != null)
            {
                _keywordsAndActions[index].action.Invoke();
            }
        }
        #endregion

        #region Event handlers
        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            CallActionsByKeyword(args.text);
        }
        #endregion
    }
}