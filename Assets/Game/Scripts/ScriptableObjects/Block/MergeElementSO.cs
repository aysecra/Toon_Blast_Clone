using ToonBlastClone.Components;
using ToonBlastClone.Components.Manager;
using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Merge Element")]
    public class MergeElementSO : BlockSO
        , ITouchable
        , IFallable
    {
        [SerializeField] ExplosionSO explosionSO;
        [SerializeField] FallingSO fallingSO;
        [SerializeField] private float fallDurationPerCell  =.15f;
        [SerializeField] private int minAmount;
        [SerializeField] private int maxAmount;

        public int MinAmount => minAmount;

        public int MaxAmount => maxAmount;

        public void Touch(CellData touchedCell, GridGenerator gridGenerator)
        {
            GUIManager.Instance.DecreaseMove();
            explosionSO.ExplodeSearch(touchedCell, gridGenerator);
        }

        public void Fall(CellData fallenCell, CellData targetCell)
        {
            fallingSO.Fall(fallenCell, targetCell, fallDurationPerCell);
        }
    }
}