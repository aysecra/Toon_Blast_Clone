using System.Collections.Generic;
using UnityEngine;

namespace ToonBlastClone.Components.Patterns
{
    public class ParticleObjectPool : ObjectPool
    {
        [SerializeField] protected ParticleSystem particleObject;
        
        private List<ParticleSystem> _particleObjects;

        public ParticleSystem ParticleObject => particleObject;
        
        protected override void Start()
        {
            _particleObjects = new List<ParticleSystem>();
            _objects = new List<GameObject>();
            base.Start();
        }
        
        protected override void AddNewObject()
        {
            GameObject newObject = Instantiate(particleObject.gameObject, parentObject);
            newObject.SetActive(isAllObjectActive);
            ParticleSystem newParticleObject = newObject.GetComponent<ParticleSystem>();
            _particleObjects.Add(newParticleObject);
            _objects.Add(newObject);
        }

        public override GameObject GetObject()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if(!_particleObjects[i].gameObject.activeInHierarchy)
                {
                    return _particleObjects[i].gameObject;
                }
            }
            
            AddNewObject();
            return _particleObjects[^1].gameObject;
        }

        public ParticleSystem GetParticleObject()
        {
            for(int i = 0; i < amountToPool; i++)
            {
                if(!_particleObjects[i].isPlaying)
                {
                    return _particleObjects[i];
                }
            }
            
            AddNewObject();
            return _particleObjects[^1];
        }
    }
}
