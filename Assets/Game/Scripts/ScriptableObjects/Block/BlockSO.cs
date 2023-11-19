using ToonBlastClone.Components;
using ToonBlastClone.Editor;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [System.Serializable]
    public class BlockSO : ScriptableObject
    {
        [ScriptableObjectId] public string id;
        [SerializeField] private Sprite image;
        [SerializeField] private Color color;

        public Sprite Image => image;
        public string Id => id;

        public Color ParticleColor
        {
            get => color;
            set => color = value;
        }

        public GridGenerator GridGenerator;


        public virtual bool IsEqual(BlockSO block)
        {
            return block != null && image.Equals(block.Image);
        }
    }
}