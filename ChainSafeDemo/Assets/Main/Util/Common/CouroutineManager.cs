using System;
using System.Collections;

namespace Main.Utility
{
    using System.Collections.Generic;
    using UnityEngine;

    public class CouroutineManager : MonoBehaviour
    {
        

        private static CouroutineManager _instance;
        private Coroutine coroutine;

        private void Start()
        {
        }

        public void StartNewCoroutine(IEnumerator action)
        {
            StartCoroutine(action);
        }
        public void StopCoroutine()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        public static CouroutineManager Instance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CouroutineManager>();
                if (_instance == null)
                {
                    var go = new GameObject("CouroutineManager");
                    _instance = go.AddComponent<CouroutineManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

}