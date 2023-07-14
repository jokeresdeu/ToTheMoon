using System;
using System.Collections;
using UnityEngine;

namespace Services
{
    public static class CoroutineRunner
    {
        private static CoroutineRunnerMono _runnerMono;

        private static CoroutineRunnerMono RunnerMono
        {
            get
            {
                if (_runnerMono != null)
                    return _runnerMono;
                _runnerMono = new GameObject{name = "CoroutineRunner"}.AddComponent<CoroutineRunnerMono>();
                return _runnerMono;
            }
        } 
        
        public static Coroutine StartCoroutine(this IEnumerator coroutine)
            => RunnerMono.StartCoroutine(coroutine);

        public static void StopCoroutine(this Coroutine coroutine) =>
            RunnerMono.StopCoroutine(coroutine);

        private class CoroutineRunnerMono : MonoBehaviour{}
    }
}