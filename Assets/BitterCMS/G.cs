using BitterCMS.Utility;
using BitterCMS.Utility.Interfaces;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class G
{
    [NotNull] private static IRoot _root;
    [NotNull] private static CoroutineUtility.CoroutineRunner _coroutine;

    public static CoroutineUtility.CoroutineRunner Coroutine {
        get {
            if (_coroutine != null)
                return _coroutine;
            
            var coroutine = new GameObject("[Coroutine]").AddComponent<CoroutineUtility>();
            Object.DontDestroyOnLoad(coroutine.gameObject);
            _coroutine = new CoroutineUtility.CoroutineRunner(coroutine);

            return _coroutine;
        }
    }


    public static void SetRoot(IRoot root)
    {
        if (_root == null || _root != root)
            _root = root;
    }
    public static T GetRoot<T>() where T : class, IRoot
    {
        return _root as T;
    }
}
