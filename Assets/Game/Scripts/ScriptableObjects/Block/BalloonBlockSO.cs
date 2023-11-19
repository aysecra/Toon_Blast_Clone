using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Balloon Block")]
    public class BalloonBlockSO : BlockSO
        , IExplodable
        , IFallable
    {
        [SerializeField] private ExplosionSO explosionSO;
        [SerializeField] private FallingSO fallingSO;
        [SerializeField] private float fallDurationPerCell;

        public override bool IsEqual(BlockSO block)
        {
            return block != null && !Image.Equals(block.Image);
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
    }
}