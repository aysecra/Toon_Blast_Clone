using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Duck Block")]
    public class DuckBlockSO : BlockSO
        , IExplodable
        , IFallable
    {
        [SerializeField] private FallingSO fallingSO;
        [SerializeField] private ExplosionSO explosionSO;
        [SerializeField] private float fallDurationPerCell;

        public override bool IsEqual(BlockSO block)
        {
            return false;
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
            DuckFallingSO fallingDuck = (DuckFallingSO) fallingSO;
            fallingDuck.GridGenerator = GridGenerator;
            fallingSO.Fall(fallenCell, targetCell, fallDurationPerCell);
        }
    }
}