using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.Interface
{
    public interface IExplodable
    {
        public void Explode(CellData explodedCell, ParticleSystem particleObject = null, AudioSource audioSource = null);
    }
}