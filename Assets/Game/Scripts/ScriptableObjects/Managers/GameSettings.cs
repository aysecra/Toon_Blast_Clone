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

        private static string SavedSettingsPath => Path.Combine(Application.dataPath, "Data/GameSettings.json");


        public static T LoadFromJSON<T>()
        {
            string dirPath = $"{Application.dataPath}/Data/";

            if (Directory.Exists(dirPath))
            {
                string loadPlayerData = File.ReadAllText(SavedSettingsPath);
                T result = JsonUtility.FromJson<T>(loadPlayerData);
                return result;
            }

            Directory.CreateDirectory(dirPath);
            return default;
        }

        public static void SaveToJSON<T>(T saveData)
        {
            string dirPath = $"{Application.dataPath}/Data/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            
            string saveContent = JsonUtility.ToJson(saveData);
            File.WriteAllText(SavedSettingsPath, saveContent);
        }

        private static void InitializeFromDefault(GameSettings settings)
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