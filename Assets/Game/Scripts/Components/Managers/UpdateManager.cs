using System.Collections.Generic;
using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Interface;

namespace ToonBlastClone.Components.Manager
{
    public class UpdateManager : Singleton<UpdateManager>
    {
        private List<IUpdateListener> _updateListenersList = new List<IUpdateListener>();
        
        private void Update()
        {
            for (int i = 0; i < _updateListenersList.Count; ++i)
            {
                _updateListenersList[i].ManagedUpdate();
            }
        }
        
        public void AddListener(IUpdateListener listener)
        {
            if (!_updateListenersList.Contains(listener))
            {
                _updateListenersList.Add(listener);
            }
            
        }
            
        public void RemoveListener(IUpdateListener listener)
        {
            if (_updateListenersList.Contains(listener))
            {
                _updateListenersList.Remove(listener);
            }
        }
    }
}
