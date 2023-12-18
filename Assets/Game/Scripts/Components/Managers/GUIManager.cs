using System.Collections.Generic;
using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Data;
using ToonBlastClone.Enums;
using ToonBlastClone.Interface;
using ToonBlastClone.Logic;
using ToonBlastClone.ScriptableObjects;
using ToonBlastClone.Structs.Event;
using UnityEngine;
using UnityEngine.UI;

namespace ToonBlastClone.Components.Manager
{
    public class GUIManager : PersistentSingleton<GUIManager>
        , EventListener<LevelUIDataEvent>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<GoalUIData> goalUIList = new List<GoalUIData>();
        [SerializeField] private TMPro.TextMeshProUGUI moveText;

        private Dictionary<GoalBlockData, GoalUIData> _goalDict = new Dictionary<GoalBlockData, GoalUIData>();
        private int _moveCount;
        private GameBoardGenerator _gridGenerator;
        private int _goalCount;

        public AudioSource AudioSource => audioSource;

        private void Start()
        {
            _gridGenerator = FindObjectOfType<GameBoardGenerator>();
        }

        private void SetLevelUI(List<GoalBlockData> goalList)
        {
            _goalDict.Clear();
            moveText.SetText($"{_moveCount}");

            for (int i = 0; i < goalUIList.Count; i++)
            {
                if (i > goalList.Count - 1 || goalList[i].BlockCount <= 0 || goalList[i].BlockType == null)
                {
                    if (goalUIList[i].GoalImage != null)
                        goalUIList[i].GoalImage.gameObject.SetActive(false);
                }
                else
                {
                    _goalDict.Add(goalList[i], goalUIList[i]);
                    if (goalUIList[i].GoalImage != null)
                    {
                        goalUIList[i].GoalImage.gameObject.SetActive(true);
                        goalUIList[i].GoalImage.sprite = goalList[i].BlockType.Image;
                    }

                    goalUIList[i].GoalText.text = $"{goalList[i].BlockCount}";
                }
            }

            _goalCount = _goalDict.Count;
        }

        public void DecreaseMove()
        {
            _moveCount = _moveCount - 1 > 0 ? _moveCount - 1 : 0;

            moveText.text = $"{_moveCount}";
            if (_moveCount <= 0)
                GameManager.Instance.SetLevelFailed();
        }

        public void GoalMovement(BlockSO block, Vector3 cellPos)
        {
            foreach (var goal in _goalDict)
            {
                if (goal.Key.BlockType != null && goal.Key.BlockType.name.Equals(block.name))
                {
                    CollectedBlock cellBlock = SharedLevelManager.Instance.GetCollectedBlock();
                    cellBlock.gameObject.SetActive(true);

                    if (_gridGenerator.SpawnPoint == null)
                        _gridGenerator = FindObjectOfType<GameBoardGenerator>();

                    cellBlock.Move(goal.Key.BlockType.Image
                        , cellPos, _gridGenerator.SpawnPoint.transform.position, goal.Key);
                }
            }
        }

        public void DecreaseGoal(GoalBlockData key)
        {
            if (!_goalDict.ContainsKey(key)) return;
            key.BlockCount = key.BlockCount - 1 > 0 ? key.BlockCount - 1 : 0;
            _goalDict[key].GoalText.text = $"{key.BlockCount}";

            if (key.BlockCount == 0)
            {
                _goalDict.Remove(key);
            }

            if (_goalCount > 0 && _goalDict.Count == 0)
                GameManager.Instance.SetLevelCompleted();
        }

        private void OnValidate()
        {
            EventManager.EventStartListening<LevelUIDataEvent>(this);
        }

        private void OnEnable()
        {
            EventManager.EventStartListening<LevelUIDataEvent>(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening<LevelUIDataEvent>(this);
        }

        public void OnEventTrigger(LevelUIDataEvent currentEvent)
        {
            _moveCount = currentEvent.LevelData.MoveCount > 0 ? currentEvent.LevelData.MoveCount : 25;
            SetLevelUI(currentEvent.LevelData.Goals);
        }
    }
}