using System;
using ToonBlastClone.ScriptableObjects;

namespace ToonBlastClone.Structs
{
    [Serializable]
    public struct CompositeAudioEntry
    {
        public AudioEvent Event;
        public float Weight;
    }
}
