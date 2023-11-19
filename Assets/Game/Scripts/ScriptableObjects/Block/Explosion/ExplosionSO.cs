using ToonBlastClone.Components;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    public abstract class ExplosionSO : ScriptableObject
    {
        public abstract void ExplodeSearch(CellData cell, GridGenerator gridGenerator);

        public abstract void Explode(CellData explodedCell,
            ParticleSystem particleObject = null, AudioSource audioSource = default);
    }
}