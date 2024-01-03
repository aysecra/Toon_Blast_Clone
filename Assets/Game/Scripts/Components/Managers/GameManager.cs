using DG.Tweening;
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
            SharedLevelManager.Instance.ClearAll();
            // DOTween.KillAll();
            DOTween.Clear(true);
            LoadNextLevel();
        }

        public void SetLevelFailed()
        {
            SharedLevelManager.Instance.ClearAll();
            // DOTween.KillAll();
            DOTween.Clear(true);
            ReloadLevel();
        }

        private void LoadNextLevel()
        {
            EventManager.TriggerEvent(new LevelEvent(LevelState.Completed));
            string nextLevel = ProgressManager.Instance.GetNextLevelName();
            SceneManager.LoadScene(nextLevel);
            EventManager.TriggerEvent(new LevelEvent(LevelState.Opened));
        }

        private void ReloadLevel()
        {
            EventManager.TriggerEvent(new LevelEvent(LevelState.Failed));
            string currLevel = ProgressManager.Instance.GetCurrentLevelName();
            SceneManager.LoadScene(currLevel);
            EventManager.TriggerEvent(new LevelEvent(LevelState.Opened));
        }
    }
}