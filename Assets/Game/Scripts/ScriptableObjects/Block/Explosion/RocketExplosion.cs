using ToonBlastClone.Components;
using ToonBlastClone.Data;
using ToonBlastClone.Enums;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone
{
    [CreateAssetMenu(menuName = "Block/Rocket Explosion")]
    public class RocketExplosion : ExplosionSO
    {
        public RocketDirection Direction;
        public override void ExplodeSearch(CellData cell, GridGenerator gridGenerator)
        {
            if (cell.CellObject.TryGetComponent(out MergeElement mergeElement))
            {
                mergeElement.SearchForExplosion(cell, gridGenerator);
            }
        }

        public override void Explode(CellData explodedCell, ParticleSystem particleObject = null,
            AudioSource audioSource = default)
        {
            explodedCell.IsEmpty = true;
            explodedCell.BlockSO = null;
            explodedCell.CellObject = null;
        }
    }
}