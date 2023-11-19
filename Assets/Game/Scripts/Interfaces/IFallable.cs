using ToonBlastClone.ScriptableObjects;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.Interface
{
    public interface IFallable
    {
        public void Fall(CellData fallenCell, CellData targetCell);
    }
}
