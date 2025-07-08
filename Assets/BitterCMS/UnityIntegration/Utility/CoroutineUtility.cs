using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitterCMS.Utility
{
    public class CoroutineUtility : MonoBehaviour
    {
        private readonly Queue<Coroutine> _activeCoroutines = new Queue<Coroutine>();
        private event Action OnStopAll;
        private event Action<IEnumerator> OnStopOne;
        private Coroutine RunCoroutine(IEnumerator coroutine)
        {
            var coroutineInstance = StartCoroutine(InternalRunCoroutine(coroutine));
            _activeCoroutines.Enqueue(coroutineInstance);

            return coroutineInstance;
        }

        private IEnumerator InternalRunCoroutine(IEnumerator coroutine)
        {
            yield return coroutine;
            _activeCoroutines.Dequeue();
            OnStopOne?.Invoke(coroutine);

            if (_activeCoroutines.Count == 0)
                OnStopAll?.Invoke();
        }
        
        public sealed class CoroutineRunner
        {
            private readonly CoroutineUtility _utility;

            public CoroutineRunner(CoroutineUtility utility)
            {
                _utility = utility;
            }

            public Coroutine Run(IEnumerator coroutine)
            {
               return _utility.RunCoroutine(coroutine);
            }

            public void StopAll(Action callBack)
            {
                if (!_utility._activeCoroutines.Any())
                {
                    callBack?.Invoke();
                    return;
                }
                
                _utility.OnStopAll += callBack;
            }

            public void StopOne(Action<IEnumerator> callBack)
            {
                _utility.OnStopOne += callBack;
            }
        }
    }

}
