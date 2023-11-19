using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        private static GameSettings _instance;

        public static GameSettings Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<GameSettings>().FirstOrDefault();
#if UNITY_EDITOR
                if (!_instance)
                    InitializeFromDefault(
                        UnityEditor.AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/DefaultGameSettings.asset"));
#endif
                return _instance;
            }
        }

        private string SavedSettingsPath => System.IO.Path.Combine(Application.persistentDataPath, "GameSettings.json");


        public T LoadFromJSON<T>()
        {
            if (File.Exists(SavedSettingsPath))
            {
                string loadPlayerData = File.ReadAllText(SavedSettingsPath);
                T result =  JsonUtility.FromJson<T>(loadPlayerData);
                return result;
            }

            return default;
        }

        public void SaveToJSON<T>(T saveData)
        {
            var folderPath = Application.persistentDataPath;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            
            string saveContent = JsonUtility.ToJson(saveData);
            System.IO.File.WriteAllText(SavedSettingsPath, saveContent);
        }

        public static void InitializeFromDefault(GameSettings settings)
        {
            if (_instance) DestroyImmediate(_instance);
            _instance = Instantiate(settings);
            _instance.hideFlags = HideFlags.HideAndDontSave;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Window/Game Settings")]
        public static void ShowGameSettings()
        {
            UnityEditor.Selection.activeObject = Instance;
        }
#endif
    }
}