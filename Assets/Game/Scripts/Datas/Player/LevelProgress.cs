namespace ToonBlastClone.Data
{
    [System.Serializable]
    public class LevelProgress
    {
        [StringInList(typeof(PropertyDrawersHelper), "AllSceneNames")]
        public string LevelName;
    }
}