using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Managers.Notifiers
{
    [Serializable]
    public class Informer : IInformer
    {
        [SerializeField] private string informerId;
        [SerializeField] private bool clearAfterNotify;
        
        private List<IResponcer> _itemToNotify;

        public static InformerInitializer _informerInitializer;
        
        public string InformerId => informerId;

        public void Init()
        {
            _itemToNotify = new List<IResponcer>();
            
            if (string.IsNullOrEmpty(informerId)) return;
            
            _informerInitializer ??= InformerInitializer.Instance;
            _informerInitializer.RegistrInformer(this);
        }

        public void Unregistr()
        {
            _informerInitializer.UnregistrInformer(InformerId);
            _itemToNotify.Clear();
        }

        public void Inform(InformStatus status = InformStatus.End)
        {
            if (_itemToNotify.Count == 0) return;
            
            int length;
            for (int i = 0; i < _itemToNotify.Count; i++)
            {
                length = _itemToNotify.Count;
                _itemToNotify[i].Responce(informerId, status);
                if (length > _itemToNotify.Count) i -= 1;
            }
            
            if (clearAfterNotify) Unregistr();
        }

        public void Unsubscribe(IResponcer who)
        {
            if (_itemToNotify.Contains(who))
                _itemToNotify.Remove(who);
        }

        public void Subscribe(IResponcer who)
        {
            if (!_itemToNotify.Contains(who))
                _itemToNotify.Add(who);
        }
    }
}