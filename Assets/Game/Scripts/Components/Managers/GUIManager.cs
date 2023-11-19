using System.Collections.Generic;
using System.Linq;
using ToonBlastClone.Structs;
using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Data;
using ToonBlastClone.ScriptableObjects;
using UnityEngine;

namespace ToonBlastClone.Components.Manager
{
    public class GUIManager : Singleton<GUIManager>
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<GoalUIData> goalUIList = new List<GoalUIData>();
        [SerializeField] private TMPro.TextMeshProUGUI moveText;
        [SerializeField] private List<GoalBlockData> goalList = new List<GoalBlockData>();
        [SerializeField] private uint moveCount;

        private GameBoardGenerator _gridGenerator;
        
        public AudioSource AudioSource => audioSource;

        private void Start()
        {
            _gridGenerator = FindObjectOfType<GameBoardGenerator>();
            
            moveText.SetText($"{moveCount}");

            for (int i = 0; i < goalUIList.Count; i++)
            {
                if (i > goalList.Count - 1)
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(false);
                }
                else
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(true);
                    goalUIList[i].GoalImage.sprite = goalList[i].BlockType.Image;
                    goalUIList[i].GoalText.text = $"{goalList[i].BlockCount}";
                }
            }
        }

        public void DecreaseMove()
        {
            moveCount--;
            moveCount = moveCount - 1 > 0 ? moveCount - 1 : 0;

            moveText.text = $"{moveCount}";
            if (moveCount <= 0)
                GameManager.Instance.SetLevelFailed();
        }

        public void DecreaseGoal(BlockSO block, Vector3 cellPos)
        {
            int completedGoalCount = 0;

            foreach (var goal in goalList)
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
                if (i > goalList.Count - 1)
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(false);
                }
                else
                {
                    goalUIList[i].GoalImage.gameObject.SetActive(true);
                    goalUIList[i].GoalImage.sprite = goalList[i].BlockType.Image;
                    goalUIList[i].GoalText.text = $"{goalList[i].BlockCount}";
                }
            }

            if (completedGoalCount == goalList.Count)
                GameManager.Instance.SetLevelCompleted();
        }
    }
}