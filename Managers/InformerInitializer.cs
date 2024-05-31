using System.Collections.Generic;
using Interfaces;

namespace Managers
{
    public class InformerInitializer
    {
        private static InformerInitializer _instance;

        private Dictionary<string, IInformer> _informers;
        private Dictionary<string, List<IResponcer>> _needToInformAddQueue;

        public static InformerInitializer Instance
        {
            get
            {
                _instance ??= new InformerInitializer();
                return _instance;
            }
        }

        public InformerInitializer()
        {
            if (_instance != null) return;
            _informers = new Dictionary<string, IInformer>();
            _needToInformAddQueue = new Dictionary<string, List<IResponcer>>();
            _instance = this;
        }
        
        private void CheckAndAdd(string informerId)
        {
            if (!_needToInformAddQueue.ContainsKey(informerId)) return;
            List<IResponcer> needAdd = _needToInformAddQueue[informerId];
            IInformer informer = _informers[informerId];

            foreach (var responcer in needAdd)
            {
                informer.Subscribe(responcer);
            }
            _needToInformAddQueue[informerId].Clear();
            _needToInformAddQueue.Remove(informerId);
        }

        public void RegistrInformer(IInformer informer)
        {
            string id = informer.InformerId;
            if (string.IsNullOrEmpty(id)) return;
            
            if (_informers.ContainsKey(id)) return;
            _informers.Add(id, informer);
            CheckAndAdd(id);
        }

        public void UnregistrInformer(string id)
        {
            if (_informers.ContainsKey(id))
                _informers.Remove(id);
        }

        public void RegistrResponcer(
            string informerId, IResponcer responcer)
        {
            if (string.IsNullOrEmpty(informerId)) return;

            if (!_informers.ContainsKey(informerId))
            {
                if (!_needToInformAddQueue.ContainsKey(informerId))
                {
                    _needToInformAddQueue
                        .Add(informerId, new List<IResponcer>());
                }
                _needToInformAddQueue[informerId].Add(responcer);
                return;
            }
            
            _informers[informerId].Subscribe(responcer);
        }

        public void UnregistrResponcer(
            string informerId, IResponcer responcer)
        {
            if (!_informers.ContainsKey(informerId)) return;
            _informers[informerId].Unsubscribe(responcer);
        }
        
    }
}