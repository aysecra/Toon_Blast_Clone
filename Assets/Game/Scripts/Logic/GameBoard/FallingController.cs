using System.Collections.Generic;
using System.Linq;
using ToonBlastClone.Components;
using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone.Logic
{
    public static class FallingController
    {
        public static void Fall(List<CellData> explodedCells, GridGenerator gridGenerator)
        {
            explodedCells = explodedCells.OrderBy(cell => cell.Index.x).ToList();

            List<Vector2Int> minIndexesAllColumn = new List<Vector2Int>();
            CellData minIndexCell = explodedCells[0];

            for (int i = 1; i < explodedCells.Count; i++)
            {
                Vector2Int minIndex = minIndexCell.Index;
                Vector2Int currIndex = explodedCells[i].Index;

                if (minIndex.x == currIndex.x)
                {
                    if (minIndex.y > currIndex.y)
                    {
                        minIndexCell = explodedCells[i];
                    }
                }
                else
                {
                    minIndexesAllColumn.Add(minIndexCell.Index);
                    minIndexCell = explodedCells[i];
                }
            }

            minIndexesAllColumn.Add(minIndexCell.Index);

            ShiftBlockOrder(minIndexesAllColumn, gridGenerator);
        }

        private static void ShiftBlockOrder(List<Vector2Int> minIndexes, GridGenerator gridGenerator)
        {
            GridModel<GridGenerator> gridModel = gridGenerator.GridModel;
            GameBoardGenerator gameBoard = ((GameBoardGenerator) gridGenerator);
            float cellSizeY = gridModel.GridCellSize.y;

            foreach (var index in minIndexes)
            {
                int firstSpawnY = index.y;
                
                for (int targetY = index.y; targetY < gridModel.CellAmount.y; targetY++)
                {
                    CellData targetCell = gridModel.GetCell(new Vector2Int(index.x, targetY));
                    
                    if (targetCell.IsEmpty)
                    {
                        for (int fallenY = targetY + 1; fallenY < gridModel.CellAmount.y; fallenY++)
                        {
                            CellData fallenCell = gridModel.GetCell(new Vector2Int(index.x, fallenY));
                            
                            if (!fallenCell.IsEmpty)
                            {
                                firstSpawnY = targetY + 1;
                                (fallenCell.BlockSO as IFallable)?.Fall(fallenCell, targetCell);
                                break;
                            }
                        }
                    }
                }

                if (firstSpawnY < gridModel.CellAmount.y)
                {
                    for (int spawnY = firstSpawnY; spawnY < gridModel.CellAmount.y; spawnY++)
                    {
                        CellData targetCell = gridModel.GetCell(new Vector2Int(index.x, spawnY));

                        if (targetCell.IsEmpty)
                        {
                            gameBoard ??= (GameBoardGenerator) gridGenerator;

                            Vector3 spawnPosition = gameBoard.SpawnPoint.position;
                            spawnPosition.x = targetCell.Border.CenterPosition.x;
                            spawnPosition.y += (spawnY - firstSpawnY) * cellSizeY;

                            BlockSO newBlock = gameBoard.SpawnableBlock.GetRandomBlock();
                            SpriteRenderer newSprite = gridModel.GetSpriteRendererPoolObject();
                            newSprite.sprite = newBlock.Image;
                            newSprite.transform.position = spawnPosition;
                            newSprite.gameObject.SetActive(true);

                            ((ISpawnable) newBlock)?.Spawn(newSprite, newBlock, targetCell,
                                ((spawnPosition.y - targetCell.Border.CenterPosition.y) / cellSizeY));
                        }
                    }
                }
            }
        }
    }
}