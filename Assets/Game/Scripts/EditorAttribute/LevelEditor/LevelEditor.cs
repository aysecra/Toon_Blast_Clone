using System.Collections.Generic;
using System.Linq;
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
        private int[] _selectedGoal = new int[3];
        private int[] _goalAmount = new int[3];
        private GoalBlockData[] goalArray = new GoalBlockData[3];
        private int _moveCount;

        [MenuItem("Window/Level Editor")]
        private static void Init()
        {
            LevelEditor window = (LevelEditor) EditorWindow.GetWindow(typeof(LevelEditor));
            window.name = "Level Editor";
            window.Show();
        }

        private void Awake()
        {
            for (int i = 0; i < _selectedGoal.Length; i++)
            {
                _selectedGoal[i] = 0;
            }

            _gameBoard = GameObject.FindObjectOfType<GameBoardGenerator>();
            GridSO gridSO = _gameBoard.GridSo;
            _placeableBlockList = _gameBoard.PlaceableBlock.BlockList;
            gridSO = _gameBoard.GridSo;
            List<BlockIndexData> indexDatas = gridSO.GetGridData();
            _cellAmount = gridSO.CellAmount;
            _moveCount = gridSO.MoveCount;

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

        private void OnGUI()
        {
            if (_gameBoard == null || _gridBlockArray == null) return;

            GoalView();

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
                _gameBoard.Generate(_indexDatas, _cellAmount, goalArray.ToList(), _moveCount);
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
            Vector2 windowSize = new Vector2(_cellAmount.x * _cellSize.x + 50, _cellAmount.y * _cellSize.y + 360);
            GUILayout.Window(0, new Rect(50, 350, windowSize.x, windowSize.y), LevelView, "Level View");
            EndWindows();
        }

        private void PlaceableCells()
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

        private void LevelView(int winID)
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

        private void GoalView()
        {
            _moveCount = EditorGUILayout.IntField("Move Count: ", _moveCount);
            
            string[] options = new string[_placeableBlockList.Count + 1];
            options[0] = "None";

            for (int i = 0; i < _placeableBlockList.Count; i++)
            {
                options[i + 1] = _placeableBlockList[i].name;
            }

            for (int i = 0; i < _selectedGoal.Length; i++)
            {
                _selectedGoal[i] = EditorGUILayout.Popup($"{i + 1} - Goal: ", _selectedGoal[i], options);

                EditorGUILayout.BeginHorizontal();
                Texture2D img = _selectedGoal[i] - 1 >= 0
                    ? _placeableBlockList[_selectedGoal[i] - 1].Image.texture
                    : default;
                GUILayout.Label(img, GUILayout.Width(_cellSize.x), GUILayout.Height(_cellSize.y));
                _goalAmount[i] = EditorGUILayout.IntField("Amount: ", _goalAmount[i]);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void SetIndexDatas()
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

        private void ClearArray()
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

            _gameBoard.Generate(_indexDatas, _cellAmount, null);
        }
    }
#endif
}