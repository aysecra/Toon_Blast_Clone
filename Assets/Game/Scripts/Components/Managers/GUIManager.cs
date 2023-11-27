using System;
using System.Collections.Generic;
using System.Linq;
using ToonBlastClone.Structs;
using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Data;
using ToonBlastClone.Interface;
using ToonBlastClone.Logic;
using ToonBlastClone.ScriptableObjects;
using ToonBlastClone.Structs.Event;
using UnityEngine;

namespace ToonBlastClone.Components.Manager
{
    public class GUIManager : Singleton<GUIManager>, EventListener<LevelUIDataEvent>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<GoalUIData> goalUIList = new List<GoalUIData>();
        [SerializeField] private TMPro.TextMeshProUGUI moveText;
        
         private List<GoalBlockData> _goalList = new List<GoalBlockData>();
        private int _moveCount;

        private GameBoardGenerator _gridGenerator;

        public AudioSource AudioSource => audioSource;

        private void Start()
        {
            _gridGenerator = FindObjectOfType<GameBoardGenerator>();
        }

        private void SetLevelUI()
        {
            moveText.SetText($"{_moveCount}");

            for (int i = 0; i < goalUIList.Count; i++)
            {
                if (i > _goalList.Count - 1)
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(false);
                }
                else
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(true);
                    goalUIList[i].GoalImage.sprite = _goalList[i].BlockType.Image;
                    goalUIList[i].GoalText.text = $"{_goalList[i].BlockCount}";
                }
            }
        }

        public void DecreaseMove()
        {
            _moveCount--;
            _moveCount = _moveCount - 1 > 0 ? _moveCount - 1 : 0;

            moveText.text = $"{_moveCount}";
            if (_moveCount <= 0)
                GameManager.Instance.SetLevelFailed();
        }

        public void DecreaseGoal(BlockSO block, Vector3 cellPos)
        {
            int completedGoalCount = 0;

            foreach (var goal in _goalList)
            {
                if (goal.BlockType.name.Equals(block.name))
                {
                    goal.BlockCount = goal.BlockCount - 1 > 0 ? goal.BlockCount - 1 : 0;
                    CollectedBlock cellBlock = SharedLevelManager.Instance.GetCollectedBlock();
                    cellBlock.gameObject.SetActive(true);
                    cellBlock.Move(goal.BlockType.Image, cellPos, _gridGenerator.SpawnPoint.transform.position);
                }

                if (goal.BlockCount == 0) completedGoalCount++;
            }

            for (int i = 0; i < goalUIList.Count; i++)
            {
                if (i > _goalList.Count - 1)
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(false);
                }
                else
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(true);
                    goalUIList[i].GoalImage.sprite = _goalList[i].BlockType.Image;
                    goalUIList[i].GoalText.text = $"{_goalList[i].BlockCount}";
                }
            }

            if (completedGoalCount == _goalList.Count)
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
            _goalList = currentEvent.LevelData.Goals;
            _moveCount = currentEvent.LevelData.MoveCount > 0 ? currentEvent.LevelData.MoveCount : 25;
            SetLevelUI();
        }
    }
}