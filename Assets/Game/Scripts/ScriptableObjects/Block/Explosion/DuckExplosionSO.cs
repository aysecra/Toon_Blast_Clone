using System.Collections.Generic;
using DG.Tweening;
using ToonBlastClone.Components;
using ToonBlastClone.Data;
using ToonBlastClone.Logic;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Block/Duck Explosion")]
    public class DuckExplosionSO : SimpleBlockExplosionSO
    {
        [SerializeField] private float explodeMovementDuration = .15f;
        private Vector2 gridSize;

        public override void ExplodeSearch(CellData cell, GridGenerator gridGenerator)
        {
            if (cell.Index.y == 0)
            {
                gridSize = gridGenerator.GridModel.GridCellSize;
                List<CellData> explodeList = new List<CellData>();
                explodeList.Add(cell);
                ExplodeController.Explode(explodeList, gridGenerator);
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

            tween = blockObject.transform
                .DOMoveY(blockObject.transform.position.y - gridSize.y, explodeMovementDuration)
                .OnComplete(() =>
                {
                    blockObject.transform.DOScale(.1f, explodeDelay).OnComplete((() =>
                    {
                        if (particleObject != null)
                        {
                            particleObject.gameObject.SetActive(true);
                            particleObject.transform.position = blockObject.transform.position;
                        }
                        blockObject.SetActive(false);
                        if (audioSource != null)
                            explodeAudio.Play(audioSource);
                        blockObject.transform.localScale = Vector3.one;
                    }));
                    tween.Kill();
                });
        }
    }
}