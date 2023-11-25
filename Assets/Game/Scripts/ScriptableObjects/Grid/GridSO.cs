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
        [SerializeField] private Vector2Int cellAmount = Vector2Int.one;
        [SerializeField] private Vector2 gridCellSize = Vector2.one;
        [SerializeField] private GameObject cellPrefab;
        private LevelData _levelData;
        private GameData _gameData;

        public Vector2Int CellAmount => cellAmount;

        public GameObject CellPrefab => cellPrefab;

        public Vector2 GridCellSize => gridCellSize;

        public void SetGridData(Vector2Int amount, List<BlockIndexData> gridData)
        {
            cellAmount = amount;
            if (_gameData == null || _levelData == null)
            {
                LoadData();
            }

            bool isContain = _gameData is {LevelDataList: not null} && _levelData != null;
            if (isContain)
            {
                _levelData.GridData = gridData;
                _levelData.CellAmount = amount;
                
                foreach (var levelData in _gameData!.LevelDataList!.Where(t => t.GridSoName.Equals(this.name)))
                {
                    levelData.GridData = gridData;
                    levelData.CellAmount = amount;
                }
            }

            if (!isContain)
            {
                _levelData = new LevelData()
                {
                    CellAmount = amount,
                    GridData = gridData,
                    GridSoName = this.name
                };

                _gameData.LevelDataList ??= new List<LevelData> {_levelData};
            }

            SaveData();
            LoadData();
        }

        public List<BlockIndexData> GetGridData()
        {
            LoadData();
            return _levelData!.GridData;
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
                    CellAmount = cellAmount,
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
                    cellAmount = levelData.CellAmount;
                    isContain = true;
                    break;
                }

            if (isContain) return;

            // level data not contain
            _levelData = new LevelData()
            {
                CellAmount = cellAmount,
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