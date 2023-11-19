using ToonBlastClone.Data;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone.Interface
{
    public interface ISpawnable
    {
        public void Spawn(SpriteRenderer spawnedSprite, BlockSO spawnedBlock, CellData targetCell, float cellDistance);
    }
}
