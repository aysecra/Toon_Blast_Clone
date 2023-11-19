using DG.Tweening;
using ToonBlastClone.Components;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Duck Falling")]
    public class DuckFallingSO : FallingSO
    {
        [SerializeField] private ExplosionSO _explosion;
        public GridGenerator GridGenerator;

        public override void Fall(CellData fallenCell, CellData targetCell, float fallDurationPerCell)
        {
            Vector3 targetPosition = targetCell.Border.CenterPosition;
            GameObject fallenObject = fallenCell.CellObject;
            BlockSO fallenBlock = fallenCell.BlockSO;

            targetCell.IsEmpty = false;
            targetCell.CellObject = fallenObject;
            targetCell.BlockSO = fallenBlock;
            
            DOTween.Kill(fallenObject.transform);

            fallenCell.IsEmpty = true;
            fallenCell.CellObject = null;
            fallenCell.BlockSO = null;

            Tween tween = null;

            tween = fallenObject.transform
                .DOMove(targetPosition, (fallenCell.Index.y - targetCell.Index.y) * fallDurationPerCell)
                .OnComplete((() =>
                {
                    _explosion.ExplodeSearch(targetCell, GridGenerator);
                    tween.Kill();
                }));
        }
    }
}