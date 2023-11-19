using ToonBlastClone.Components;
using ToonBlastClone.Data;

namespace ToonBlastClone.Interface
{
    public interface ITouchable
    {
        public void Touch(CellData touchedCell, GridGenerator gridGenerator);
    }
}