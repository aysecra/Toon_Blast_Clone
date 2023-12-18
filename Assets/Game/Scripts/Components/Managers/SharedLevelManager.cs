using ToonBlastClone.Components.Patterns;
using UnityEngine;

namespace ToonBlastClone.Components.Manager
{
    public class SharedLevelManager : PersistentSingleton<SharedLevelManager>
    {
        [SerializeField] private ObjectPool _explodeParticlePool;
        [SerializeField] private ObjectPool _rocketPool;
        [SerializeField] private ObjectPool _collectedPool;

        public ParticleSystem GetParticle()
        {
            return ((ParticleObjectPool) _explodeParticlePool).GetParticleObject();
        }

        public CollectedBlock GetCollectedBlock()
        {
            return (CollectedBlock) ((PoolableObjectPool) _collectedPool).GetPooledObject();
        }

        public void ClearAll()
        {
            _collectedPool.ClearPool();
            _rocketPool.ClearPool();
        }

        public GameObject GetRocket()
        {
            GameObject result = _rocketPool.GetObject();
            return result;
        }
    }
}