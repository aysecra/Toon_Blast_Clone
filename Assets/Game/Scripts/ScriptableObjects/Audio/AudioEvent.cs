using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play(AudioSource source);
    }
}