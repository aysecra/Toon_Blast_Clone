using System.Collections.Generic;
using UnityEngine;

namespace ToonBlastClone.Components.Patterns
{
    public class PoolableObjectPool : ObjectPool
    {
        [SerializeField] protected PoolableObject poolableObject;

        private List<PoolableObject> _pooledObjects;

        public PoolableObject PoolableObject => poolableObject;

        protected override void Start()
        {
            _pooledObjects = new List<PoolableObject>();
            _gettedObjects = new List<GameObject>();
            _objects = new List<GameObject>();
            base.Start();
        }

        protected override void AddNewObject()
        {
            GameObject newObject = Instantiate(poolableObject.gameObject, parentObject);
            newObject.SetActive(isAllObjectActive);
            PoolableObject newPoolableObject = newObject.GetComponent<PoolableObject>();
            _pooledObjects.Add(newPoolableObject);
            _objects.Add(newObject);
        }

        public override GameObject GetObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    _gettedObjects.Add(_pooledObjects[i].gameObject);
                    return _pooledObjects[i].gameObject;
                }
            }

            AddNewObject();
            _gettedObjects.Add(_pooledObjects[^1].gameObject);
            return _pooledObjects[^1].gameObject;
        }

        public PoolableObject GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!_pooledObjects[i].gameObject.activeInHierarchy)
                {
                    _gettedObjects.Add(_pooledObjects[i].gameObject);
                    return _pooledObjects[i];
                }
            }

            AddNewObject();
            _gettedObjects.Add(_pooledObjects[^1].gameObject);
            return _pooledObjects[^1];
        }
    }
}