using System.Collections.Generic;
using System.Linq;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Grid Element")]
    public class GridSO : ScriptableObject
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private Vector2 gridCellSize = Vector2.one;
        [SerializeField] private GameObject cellPrefab;
        
        private Vector2Int _cellAmount = Vector2Int.one;
        private LevelData _levelData;
        private GameData _gameData;
        private int _moveCount;
        private List<GoalBlockData> _goalList;

        public Vector2Int CellAmount => _cellAmount;

        public GameObject CellPrefab => cellPrefab;

        public Vector2 GridCellSize => gridCellSize;

        public int MoveCount => _moveCount;

        public List<GoalBlockData> GoalList => _goalList;

        public void SetGridData(Vector2Int amount, List<BlockIndexData> gridData, List<GoalBlockData> goalList, int moveCount)
        {
            _cellAmount = amount;
            _goalList = goalList;
            if (_gameData == null || _levelData == null)
            {
                LoadData();
            }

            bool isContain = _gameData is {LevelDataList: not null} && _levelData != null;
            if (isContain)
            {
                _levelData.GridData = gridData;
                _levelData.CellAmount = amount;
                _levelData.Goals = goalList;
                _levelData.MoveCount = moveCount;
                
                foreach (var levelData in _gameData!.LevelDataList!.Where(t => t.GridSoName.Equals(this.name)))
                {
                    levelData.GridData = gridData;
                    levelData.CellAmount = amount;
                    levelData.Goals = goalList;
                    levelData.MoveCount = moveCount;
                }
            }

            if (!isContain)
            {
                _levelData = new LevelData()
                {
                    CellAmount = amount,
                    GridData = gridData,
                    Goals = goalList,
                    MoveCount = moveCount,
                    GridSoName = this.name
                };

                _gameData!.LevelDataList ??= new List<LevelData> {_levelData};
            }

            SaveData();
            LoadData();
        }

        public List<BlockIndexData> GetGridData()
        {
            LoadData();
            _cellAmount = _levelData!.CellAmount;
            _goalList = _levelData!.Goals;
            _moveCount = _levelData!.MoveCount;
            return _levelData!.GridData;
        }
        
        public LevelData GetLevelData()
        {
            LoadData();
            return _levelData;
        }

        private void SaveData()
        {
            for (int i = 0; i < _gameData.LevelDataList.Count; i++)
            {
                if (!_gameData.LevelDataList[i].GridSoName.Equals(this.name)) continue;
                _gameData.LevelDataList[i] = _levelData;
                break;
            }

            GameSettings.SaveToJSON(_gameData);
        }

        private void LoadData()
        {
            GameData gameData = GameSettings.LoadFromJSON<GameData>();

            // null gameData condition
            if (gameData == null)
            {
                _gameData = new GameData()
                {
                    LevelDataList = new List<LevelData>()
                };
                _levelData = new LevelData()
                {
                    CellAmount = _cellAmount,
                    GridData = new List<BlockIndexData>(),
                    GridSoName = this.name
                };
            }


            // control level data is contain
            bool isContain = false;

            if (gameData is {LevelDataList: not null})
                foreach (var levelData in gameData!.LevelDataList.Where(levelData =>
                             levelData.GridSoName.Equals(this.name)))
                {
                    _levelData = levelData;
                    _cellAmount = levelData.CellAmount;
                    _goalList = levelData.Goals;
                    isContain = true;
                    break;
                }

            if (isContain) return;

            // level data not contain
            _levelData = new LevelData()
            {
                CellAmount = _cellAmount,
                GridData = new List<BlockIndexData>(),
                GridSoName = this.name
            };

            _gameData.LevelDataList ??= new List<LevelData>();

            _gameData.LevelDataList.Add(_levelData);
        }

        private void SaveSO()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}