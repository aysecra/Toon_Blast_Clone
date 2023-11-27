using System.Collections.Generic;
using System.Linq;
using ToonBlastClone.Data;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone.Components
{
    public class GameBoardGenerator : GridGenerator
    {
        [SerializeField] private PlaceableAndSpawnableBlockSO placeableBlock;
        [SerializeField] private PlaceableAndSpawnableBlockSO spawnableBlock;
        [SerializeField] private MergeResultElementsSO mergeResultElements;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private SpriteRenderer _borderSpriteRenderer;
        [SerializeField] private Vector2 _borderThickness;

        public PlaceableAndSpawnableBlockSO SpawnableBlock => spawnableBlock;
        public Transform SpawnPoint => spawnPoint;

        public PlaceableAndSpawnableBlockSO PlaceableBlock => placeableBlock;

        public MergeResultElementsSO MergeResultElements => mergeResultElements;

        protected override void Start()
        {
            placeableBlock.SetGridGeneratorForAllGenerator(this);
            spawnableBlock.SetGridGeneratorForAllGenerator(this);
            base.Start();
        }

        protected override void SetCell()
        {
            SetBorder();
            List<BlockIndexData> indexDatas = gridSO.GetGridData();
            indexDatas = indexDatas.OrderBy(indexData => indexData.GridIndex.x).ToList();
            indexDatas = indexDatas.OrderBy(indexData => indexData.GridIndex.y).ToList();

            Vector3 currPos = cellDownLeftPoint.position;
            int i = 0;

            if (indexDatas.Count <= 0) return;
            for (int y = 0; y < GridModel!.CellAmount.y; y++)
            {
                for (int x = 0; x < GridModel!.CellAmount.x; x++)
                {
                    Vector3 cellPosition =
                        GetWorldToCellsWorldPosition(currPos);
                    currPos = cellPosition;
                    cellPosition.z = y * -.01f;
                    Vector3 minCellPosition = cellPosition;
                    cellPosition += (Vector3) (GridModel!.GridCellSize * .5f);
                    Vector3 maxCellPosition = minCellPosition + (Vector3) GridModel!.GridCellSize;
                    BlockSO currBlock = placeableBlock.BlockList[indexDatas[i].PlaceableIndex];

                    SpriteRenderer cellSprite = _gridModel.GetSpriteRendererPoolObject();
                    cellSprite.gameObject.SetActive(true);
                    cellSprite.transform.position = cellPosition;
                    cellSprite.sprite = currBlock.Image;

                    GridModel?.SetCellArrayData(this, new Vector2Int(x, y), new CellData()
                    {
                        BlockSO = currBlock,
                        Index = new Vector2Int(x, y),
                        CellObject = cellSprite.gameObject,
                        Border = new PlaneBorderData(cellPosition, minCellPosition, maxCellPosition),
                    });

                    currPos.x += GridModel!.GridCellSize.x;
                    i++;
                }

                currPos.x = cellDownLeftPoint.position.x;
                currPos.y += GridModel!.GridCellSize.y;
            }
        }

        public override List<BlockIndexData> SetRandomCell(List<GoalBlockData> goalList,
            out List<GoalBlockData> resultGoalList)
        {
            SetBorder();
            Vector3 currPos = cellDownLeftPoint.position;
            resultGoalList = null;

            if (goalList == null)
            {
                if (placeableBlock.BlockList.Count > 2)
                {
                    resultGoalList = new List<GoalBlockData>();
                    for (int i = 0; i < 2; i++)
                    {
                        resultGoalList.Add(new GoalBlockData()
                        {
                            BlockType = placeableBlock.BlockList[i],
                            BlockCount = 25
                        });
                    }
                }
            }
            else
            {
                resultGoalList = goalList;
            }

            spawnableBlock.InitRandom();

            List<BlockIndexData> result = new List<BlockIndexData>();

            for (int y = 0; y < GridModel!.CellAmount.y; y++)
            {
                for (int x = 0; x < GridModel!.CellAmount.x; x++)
                {
                    Vector3 cellPosition =
                        GetWorldToCellsWorldPosition(currPos);
                    currPos = cellPosition;
                    cellPosition.z = y * -.01f;
                    Vector3 minCellPosition = cellPosition;
                    cellPosition += (Vector3) (GridModel!.GridCellSize * .5f);
                    Vector3 maxCellPosition = minCellPosition + (Vector3) GridModel!.GridCellSize;
                    BlockSO currBlock = spawnableBlock.GetRandomBlock();

                    SpriteRenderer cellSprite = _gridModel.GetSpriteRendererPoolObject();
                    cellSprite.gameObject.SetActive(true);
                    cellSprite.transform.position = cellPosition;
                    cellSprite.sprite = currBlock.Image;

                    result.Add(new BlockIndexData()
                    {
                        GridIndex = new Vector2Int(x, y),
                        PlaceableIndex = placeableBlock.GetListIndex(currBlock)
                    });

                    GridModel?.SetCellArrayData(this, new Vector2Int(x, y), new CellData()
                    {
                        BlockSO = currBlock,
                        Index = new Vector2Int(x, y),
                        CellObject = cellSprite.gameObject,
                        Border = new PlaneBorderData(cellPosition, minCellPosition, maxCellPosition),
                    });

                    currPos.x += GridModel!.GridCellSize.x;
                }

                currPos.x = cellDownLeftPoint.position.x;
                currPos.y += GridModel!.GridCellSize.y;
            }

            return result;
        }

        protected override void SetNeigbors()
        {
            foreach (var cell in _gridModel!.CellArray)
            {
                if (cell == null) continue;

                Vector2Int index = cell.Index;

                // get down neighbor
                if (index.y > 0)
                {
                    cell.AddNeighbor(_gridModel.GetCell(index + new Vector2Int(0, -1)));
                }

                // get up neighbor
                if (index.y < _gridModel!.CellAmount.y - 1)
                {
                    cell.AddNeighbor(_gridModel.GetCell(index + new Vector2Int(0, 1)));
                }

                // get left neighbor
                if (index.x > 0)
                {
                    cell.AddNeighbor(_gridModel.GetCell(index + new Vector2Int(-1, 0)));
                }

                // get right neighbor
                if (index.x < _gridModel!.CellAmount.x - 1)
                {
                    cell.AddNeighbor(_gridModel.GetCell(index + new Vector2Int(1, 0)));
                }
            }
        }

        public override List<CellData> GetSpawnArea(Vector2Int size, out Vector3 position)
        {
            position = default;
            return new List<CellData>();
        }

        private void SetBorder()
        {
            Vector2 cellAmount = GridSo.CellAmount;
            Vector2 gridSize = GridSo.GridCellSize;
            Vector2 borderSize = new Vector2(cellAmount.x * gridSize.x, cellAmount.y * gridSize.y) + _borderThickness;
            _borderSpriteRenderer.size = borderSize;

            Vector3 downLeft = GetWorldToCellsWorldPosition(cellDownLeftPoint.position);
            ;
            var borderTransform = _borderSpriteRenderer.transform;
            borderTransform.position = new Vector3(downLeft.x + borderSize.x * .5f - _borderThickness.x * .5f,
                downLeft.y + borderSize.y * .5f - _borderThickness.y * .5f, borderTransform.position.z);
        }
    }
}