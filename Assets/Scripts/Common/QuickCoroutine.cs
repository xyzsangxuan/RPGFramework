using System;
using System.Collections;
using UnityEngine;

 public class QuickCoroutine:Singleton<QuickCoroutine>
{
    GameObject _coroutineRoot;
    MonoBehaviour _coroutineMono;//用来跑协程
    public void Init()
    {
        _coroutineRoot = new GameObject("QuickCoroutine");
        GameObject.DontDestroyOnLoad(_coroutineRoot);
        _coroutineMono = _coroutineRoot.AddComponent<UtilsMonoBehaviour>();
    }

    public Coroutine StartCorontine(IEnumerator routine)
    {
        return _coroutineMono.StartCoroutine(routine);
    }
    //封装Stop
}

