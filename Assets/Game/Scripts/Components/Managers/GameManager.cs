using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Enums;
using ToonBlastClone.Logic;
using ToonBlastClone.Structs.Event;
using UnityEngine.SceneManagement;

namespace ToonBlastClone.Components.Manager
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public void SetLevelPaused()
        {
            // todo: level pause element will added
        }

        public void SetLevelCompleted()
        {
            LoadNextLevel();
        }
        public void SetLevelFailed()
        {
            ReloadLevel();
        }
        
        private void LoadNextLevel()
        {
            EventManager.TriggerEvent(new LevelEvent(LevelState.Completed));
            string nextLevel = ProgressManager.Instance.GetNextLevelName();
            SceneManager.LoadScene(nextLevel);
        }

        private void ReloadLevel()
        {
            EventManager.TriggerEvent(new LevelEvent(LevelState.Failed));
            string currLevel = ProgressManager.Instance.GetCurrentLevelName();
            SceneManager.LoadScene(currLevel);
        }
    }
}
