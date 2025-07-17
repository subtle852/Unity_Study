using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original {  get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if(_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else 
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if ( _root == null )
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad( _root );
        }
    }

    public void Push(Poolable poolable)
    {

    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        return null;
    }

    public GameObject GetOriginal(string name)
    {
        return null;
    }


}
