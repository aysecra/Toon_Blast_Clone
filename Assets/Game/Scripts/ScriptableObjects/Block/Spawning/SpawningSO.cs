using DG.Tweening;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Spawning Attribute")]
    public class SpawningSO : ScriptableObject
    {
        public void Spawn(SpriteRenderer spawnedSprite, BlockSO spawnedBlock, CellData targetCell, float cellDistance, float fallDurationPerCell)
        {
            Vector3 targetPosition = targetCell.Border.CenterPosition;
            targetCell.IsEmpty = false;
            targetCell.CellObject = spawnedSprite.gameObject;
            targetCell.BlockSO = spawnedBlock;

            Tween tween = null;

            tween = spawnedSprite.transform
                .DOMove(targetPosition, cellDistance * fallDurationPerCell)
                .OnComplete((() => { tween.Kill(); }));
        }
    }
}
