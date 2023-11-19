using System.Collections.Generic;
using ToonBlastClone.Components;
using ToonBlastClone.Components.Manager;
using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone.Logic
{
    public static class ExplodeController
    {
        public static void Explode(List<CellData> explodeList, GridGenerator gridGenerator)
        {
            int simpleBlockCount = 0;

            foreach (CellData explodeCell in explodeList)
            {
                if (explodeCell.BlockSO as SimpleBlockSO != null)
                {
                    simpleBlockCount++;
                }

                IExplodable explodableElement = (IExplodable) explodeCell.BlockSO;
                GUIManager.Instance.DecreaseGoal(explodeCell.BlockSO, explodeCell.CellObject.transform.position);
                explodableElement.Explode(explodeCell, SharedLevelManager.Instance.GetParticle(),
                    GUIManager.Instance.AudioSource);
            }

            BlockSO mergeBlock =
                ((GameBoardGenerator) gridGenerator).MergeResultElements.GetMergeResult(simpleBlockCount);
            if (mergeBlock != null)
            {
                GameObject rocket = SharedLevelManager.Instance.GetRocket();
                explodeList[0].CellObject = rocket;
                explodeList[0].IsEmpty = false;
                explodeList[0].BlockSO = mergeBlock;
                rocket.transform.position = explodeList[0].Border.CenterPosition;
                rocket.SetActive(true);
            }
            FallingController.Fall(explodeList, gridGenerator);
        }
    }
}