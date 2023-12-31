using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToonBlastClone.Components.Patterns
{
    public abstract class ObjectPool : MonoBehaviour
    {
        [SerializeField] protected Transform parentObject;
        [SerializeField] protected uint amountToPool;
        [SerializeField] protected bool isAllObjectActive;
        
        protected List<GameObject> _objects;
        public List<GameObject> Objects => _objects;
        protected List<GameObject> _gettedObjects;

        
        public uint AmountToPool
        {
            get => amountToPool;
            set => amountToPool = value;
        }
        
        protected virtual void Start()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                AddNewObject();
            }
        }
        
        public void ClearPool()
        {
            foreach (var gettedObject in _gettedObjects)
            {
                gettedObject.SetActive(isAllObjectActive);
            }
            _gettedObjects.Clear();
        }

        protected abstract void AddNewObject();
        public abstract GameObject GetObject();
    }
}
