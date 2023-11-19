using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Data;

namespace ToonBlastClone.Components
{
    public abstract class MergeElement : PoolableObject
    {
        public abstract void SearchForExplosion(CellData cell, GridGenerator gridGenerator);
    }
}
