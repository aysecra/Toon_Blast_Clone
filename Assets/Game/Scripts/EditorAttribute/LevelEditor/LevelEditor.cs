using System.Collections.Generic;
using ToonBlastClone.Components;
using ToonBlastClone.Data;
using ToonBlastClone.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ToonBlastClone.Editor
{
#if UNITY_EDITOR

    public class LevelEditor : EditorWindow
    {
        private GameBoardGenerator _gameBoard;
        private BlockSO[,] _gridBlockArray;
        private int[,] _indexArray;
        private List<BlockSO> _placeableBlockList = new List<BlockSO>();
        private BlockSO selectedBlock;
        private int selectedIndex;
        private List<BlockIndexData> _indexDatas = new List<BlockIndexData>();

        Vector2 _panner = Vector2.zero;
        readonly Vector2 _scrollSize = new Vector2(2000, 2000);
        private Vector2 _cellSize = new Vector2(35, 35);
        private Vector2Int _cellAmount;

        [MenuItem("Window/Level Editor")]
        static void Init()
        {
            LevelEditor window = (LevelEditor) EditorWindow.GetWindow(typeof(LevelEditor));
            window.name = "Level Editor";
            window.Show();
        }

        private void Awake()
        {
            _gameBoard = GameObject.FindObjectOfType<GameBoardGenerator>();
            _cellAmount = _gameBoard.GridSo.CellAmount;
            _placeableBlockList = _gameBoard.PlaceableBlock.BlockList;
            List<BlockIndexData> indexDatas = _gameBoard.GridSo.GetGridData();
            
            if (indexDatas is {Count: > 0})
            {
                _indexDatas = indexDatas;

                _gridBlockArray = new BlockSO[_cellAmount.x, _cellAmount.y];
                _indexArray = new int[_cellAmount.x, _cellAmount.y];

                foreach (var indexData in _indexDatas)
                {
                    _gridBlockArray[indexData.GridIndex.x, indexData.GridIndex.y] =
                        _placeableBlockList[indexData.PlaceableIndex];
                    _indexArray[indexData.GridIndex.x, indexData.GridIndex.y] =
                        indexData.PlaceableIndex;
                }
            }
            else
            {
                ClearArray();
            }
        }

        void OnGUI()
        {
            if (_gameBoard == null || _gridBlockArray == null) return;
            
            Vector2Int prevCellAmount = _cellAmount;
            _cellAmount = EditorGUILayout.Vector2IntField("Cell  amount:", _cellAmount);

            if (prevCellAmount != _cellAmount)
            {
                ClearArray();
            }

            PlaceableCells();


            GUIStyle guiStyle = EditorStyles.miniButtonMid;
            guiStyle.margin = new RectOffset(0, 0, 10, 0);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate", guiStyle))
            {
                SetIndexDatas();
                _gameBoard.Generate(_indexDatas, _cellAmount);
            }

            if (GUILayout.Button("Random Generate", guiStyle))
            {
                _gameBoard.SpawnableBlock.ChangeRandomSeed();
                _indexDatas = _gameBoard.RandomGenerate();

                foreach (var indexData in _indexDatas)
                {
                    _gridBlockArray[indexData.GridIndex.x, indexData.GridIndex.y] =
                        _placeableBlockList[indexData.PlaceableIndex];
                    _indexArray[indexData.GridIndex.x, indexData.GridIndex.y] =
                        indexData.PlaceableIndex;
                }
            }

            if (GUILayout.Button("Clear", guiStyle))
            {
                _gameBoard.ClearCell();
                ClearArray();
            }

            EditorGUILayout.EndHorizontal();

            BeginWindows();
            Vector2 windowSize = new Vector2(_cellAmount.x * _cellSize.x + 30, _cellAmount.y * _cellSize.y + 50);
            GUILayout.Window(0, new Rect(15, 200, windowSize.x, windowSize.y), LevelView, "Level View");
            EndWindows();
        }

        void PlaceableCells()
        {
            GUILayout.Label("Placeable Block Types");
            GUILayout.BeginHorizontal();

            for (int i = 0; i < _placeableBlockList.Count; i++)
            {
                if (GUILayout.Button(
                        _placeableBlockList[i].Image.texture, GUILayout.Width(_cellSize.x + 10),
                        GUILayout.Height(_cellSize.y + 10)))
                {
                    selectedBlock = _placeableBlockList[i];
                    selectedIndex = i;
                }
            }

            GUILayout.EndHorizontal();
            string selectedText = selectedBlock != null ? selectedBlock.name : "";
            GUILayout.Label($"Selected Block: {selectedText}");
        }

        void LevelView(int winID)
        {
            GUI.BeginGroup(new Rect(_panner, _scrollSize));

            for (int y = _cellAmount.y - 1; y >= 0; y--)
            {
                GUILayout.BeginHorizontal();
                for (int x = 0; x < _cellAmount.x; x++)
                {
                    if (GUILayout.Button(_gridBlockArray[x, y].Image.texture,
                            GUILayout.Width(_cellSize.x), GUILayout.Height(_cellSize.y)))
                    {
                        if (selectedBlock == null) continue;
                        _gridBlockArray[x, y] = selectedBlock;
                        _indexArray[x, y] = selectedIndex;
                    }
                }

                GUILayout.EndHorizontal();
            }

            GUI.EndGroup();
        }

        void SetIndexDatas()
        {
            _indexDatas.Clear();

            for (int x = 0; x < _cellAmount.x; x++)
            {
                for (int y = 0; y < _cellAmount.y; y++)
                {
                    _indexDatas.Add(new BlockIndexData()
                    {
                        PlaceableIndex = _indexArray[x, y],
                        GridIndex = new Vector2Int(x, y)
                    });
                }
            }
        }

        void ClearArray()
        {
            _gridBlockArray = new BlockSO[_cellAmount.x, _cellAmount.y];
            _indexArray = new int[_cellAmount.x, _cellAmount.y];
            _indexDatas.Clear();

            for (int x = 0; x < _cellAmount.x; x++)
            {
                for (int y = 0; y < _cellAmount.y; y++)
                {
                    _gridBlockArray[x, y] = _placeableBlockList[0];
                    _indexArray[x, y] = 0;
                    _indexDatas.Add(new BlockIndexData()
                    {
                        PlaceableIndex = 0,
                        GridIndex = new Vector2Int(x, y)
                    });
                }
            }

            _gameBoard.Generate(_indexDatas, _cellAmount);
        }
    }
#endif
}