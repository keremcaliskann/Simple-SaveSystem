using System;
using System.Collections.Generic;
using UnityEngine;

namespace Simple
{
    [DefaultExecutionOrder(-2)]
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }
        public GameObject PoolParent { get; private set; }

        [SerializeField] private List<Pool> _pools = new List<Pool>();


        [Serializable]
        public class Pool
        {
            public int AvailableCount => _poolList.Count;
            public string ID;
            public GameObject PoolObject;
            public int Count;
            public bool DeactivateOnCreate = true;
            public bool ActivateOnGet = true;
            public bool DeactivateOnPut = true;
            public bool AddOnRunOut = true;
            private List<GameObject> _poolList = new List<GameObject>();
            public void CreateOne()
            {
                GameObject ins = Instantiate(PoolObject, Instance.PoolParent.transform);
                if (DeactivateOnCreate)
                    ins.SetActive(false);
                _poolList.Add(ins);
            }
            public void Create()
            {
                for (int i = 0; i < Count; i++)
                {
                    GameObject ins = Instantiate(PoolObject, Instance.PoolParent.transform);
                    if (DeactivateOnCreate)
                        ins.SetActive(false);
                    _poolList.Add(ins);
                }
            }
            public void Create(GameObject item, int count, string id)
            {
                PoolObject = item;
                Count = count;
                ID = id;

                for (int i = 0; i < count; i++)
                {
                    GameObject ins = Instantiate(PoolObject, Instance.PoolParent.transform);
                    if (DeactivateOnCreate)
                        ins.SetActive(false);
                    _poolList.Add(ins);
                }
            }
            public void Add(GameObject item)
            {
                item.transform.parent = Instance.PoolParent.transform;
                _poolList.Add(item);
                if (DeactivateOnPut)
                    item.SetActive(false);
            }

            public GameObject Get()
            {
                GameObject obj = _poolList[0];
                _poolList.RemoveAt(0);
                if (ActivateOnGet)
                    obj.SetActive(true);
                return obj;
            }
        }
        private void Awake()
        {
            Instance = this;
            PoolParent = new GameObject("OBJECT POOL");
            PoolParent.transform.position = new Vector3(100, 0, 100);

            for (int i = 0; i < _pools.Count; i++)
            {
                _pools[i].Create();
            }
        }

        public void CreatePool(GameObject item, int count, bool deactivateOnCreate = true, bool activateOnGet = true, bool deactivateOnPut = true, bool addOnRunOut = true)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].ID == item.name)
                {
                    Debug.LogWarning("Pool cant created. There is pool with this object!");
                    return;
                }
            }
            Pool pool = new Pool();
            pool.DeactivateOnCreate = deactivateOnCreate;
            pool.ActivateOnGet = activateOnGet;
            pool.DeactivateOnPut = deactivateOnPut;
            pool.AddOnRunOut = addOnRunOut;
            pool.Create(item, count, item.name);
            _pools.Add(pool);
        }
        public void CreatePool(GameObject item, int count, string id, bool deactivateOnCreate = true, bool activateOnGet = true, bool deactivateOnPut = true, bool addOnRunOut = true)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].ID == id)
                {
                    Debug.LogWarning("Pool cant created. There is pool with this object!");
                    return;
                }
            }
            Pool pool = new Pool();
            pool.DeactivateOnCreate = deactivateOnCreate;
            pool.ActivateOnGet = activateOnGet;
            pool.DeactivateOnPut = deactivateOnPut;
            pool.AddOnRunOut = addOnRunOut;
            pool.Create(item, count, id);
            _pools.Add(pool);
        }

        public GameObject GetPoolObject(string id)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].ID == id)
                {
                    if (_pools[i].AvailableCount > 0)
                        return _pools[i].Get();
                    else
                    {
                        if (_pools[i].AddOnRunOut)
                        {
                            _pools[i].CreateOne();
                            return _pools[i].Get();
                        }
                        Debug.LogWarning("There is no object left in pool with id : " + id);
                        return null;
                    }
                }
            }
            Debug.LogWarning("There is no pool with id : " + id);
            return null;
        }
        public T GetPoolObject<T>(string id) where T : UnityEngine.Object
        {
            return GetPoolObject(id).GetComponent<T>();
        }
        public void PutPoolObject(GameObject item, string id)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].ID == id)
                {
                    _pools[i].Add(item);
                    return;
                }
            }
            Debug.LogWarning("There is no pool with id : " + id);
        }
    }
}
