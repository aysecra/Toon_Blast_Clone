using System.Collections.Generic;
using DG.Tweening;
using ToonBlastClone.Components;
using ToonBlastClone.Components.Manager;
using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using ToonBlastClone.Logic;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Simple Block Explosion")]
    public class SimpleBlockExplosionSO : ExplosionSO
    {
        [SerializeField] protected SimpleAudioEvent explodeAudio;
        [SerializeField] protected float explodeDelay;

        public override void ExplodeSearch(CellData cell, GridGenerator gridGenerator)
        {
            List<CellData> prevTrailCell = new List<CellData>();
            List<CellData> newTrailCell = new List<CellData>();

            List<CellData> result = new List<CellData>();
            IExplodable firstElement = (IExplodable) cell.BlockSO;

            if (firstElement == null) return;

            prevTrailCell.Add(cell);
            result.Add(cell);

            while (true)
            {
                if (prevTrailCell.Count == 0)
                    break;

                foreach (var lastCell in prevTrailCell)
                {
                    foreach (var neighbors in lastCell.Neighbors)
                    {
                        if (neighbors.BlockSO.IsEqual(lastCell.BlockSO))
                        {
                            if (!result.Contains(neighbors))
                            {
                                newTrailCell.Add(neighbors);
                                result.Add(neighbors);
                            }
                        }
                    }
                }

                prevTrailCell.Clear();
                foreach (var newCell in newTrailCell)
                {
                    prevTrailCell.Add(newCell);
                }

                newTrailCell.Clear();
            }

            if (result.Count > 1)
            {
                ExplodeController.Explode(result, gridGenerator);
                InputManager.Instance.SetInputDelay();
            }
        }

        public override void Explode(CellData explodedCell, ParticleSystem particleObject = null,
            AudioSource audioSource = default)
        {
            Tween tween = null;
            GameObject blockObject = explodedCell.CellObject;

            explodedCell.IsEmpty = true;
            explodedCell.BlockSO = null;
            explodedCell.CellObject = null;
            
            tween = blockObject.transform.DOScale(.1f, explodeDelay)
                .OnStart((() =>
                {
                    if (particleObject != null)
                    {
                        particleObject.gameObject.SetActive(true);
                        particleObject.transform.position = blockObject.transform.position;
                    }
                }))
                .OnComplete((() =>
                {
                    blockObject.SetActive(false);
                    if (audioSource != null)
                        explodeAudio.Play(audioSource);
                    blockObject.transform.localScale = Vector3.one;
                    tween.Kill();
                }));
        }
    }
}