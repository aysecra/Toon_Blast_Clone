using DG.Tweening;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Falling Attribute")]
    public class FallingSO : ScriptableObject
    {
        public virtual void Fall(CellData fallenCell, CellData targetCell, float fallDurationPerCell)
        {
            Vector3 targetPosition = targetCell.Border.CenterPosition;
            GameObject fallenObject = fallenCell.CellObject;
            BlockSO fallenBlock = fallenCell.BlockSO;

            DOTween.Kill(fallenObject.transform);

            targetCell.IsEmpty = false;
            targetCell.CellObject = fallenObject;
            targetCell.BlockSO = fallenBlock;

            fallenCell.IsEmpty = true;
            fallenCell.CellObject = null;
            fallenCell.BlockSO = null;

            Tween tween = null;

            tween = fallenObject.transform
                .DOMove(targetPosition, (fallenCell.Index.y - targetCell.Index.y) * fallDurationPerCell)
                .OnComplete((() => { tween.Kill(); }));
        }
    }
}