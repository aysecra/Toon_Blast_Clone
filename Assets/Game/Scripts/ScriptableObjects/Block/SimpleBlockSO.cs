using ToonBlastClone.Components;
using ToonBlastClone.Components.Manager;
using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Simple Block")]
    public class SimpleBlockSO : BlockSO
        , ITouchable
        , IExplodable
        , IFallable
        , ISpawnable
    {
        [SerializeField] private ExplosionSO explosionSO;
        [SerializeField] private FallingSO fallingSO;
        [SerializeField] private SpawningSO spawningSO;
        [SerializeField] private float fallDurationPerCell;

        [SerializeField] private SimpleAudioEvent collectAudio;
        public SimpleAudioEvent CollectAudio => collectAudio;

        public void Touch(CellData touchedCell, GridGenerator gridGenerator)
        {
            GUIManager.Instance.DecreaseMove();
            explosionSO.ExplodeSearch(touchedCell, gridGenerator);
        }

        public void Explode(CellData explodedCell, ParticleSystem particleObject, AudioSource audioSource = null)
        {
            if (particleObject != null)
            {
                var mainParticle = particleObject.main;
                mainParticle.startColor = new ParticleSystem.MinMaxGradient( ParticleColor );
            }
            
            explosionSO.Explode(explodedCell, particleObject, audioSource);
        }

        public void Fall(CellData fallenCell, CellData targetCell)
        {
            fallingSO.Fall(fallenCell, targetCell, fallDurationPerCell);
        }

        public void Spawn(SpriteRenderer spawnedSprite, BlockSO spawnedBlock, CellData targetCell, float cellDistance)
        {
            spawningSO.Spawn(spawnedSprite, spawnedBlock, targetCell, cellDistance, fallDurationPerCell);
        }
    }
}