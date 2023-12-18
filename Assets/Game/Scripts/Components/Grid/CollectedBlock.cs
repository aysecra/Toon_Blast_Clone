using DG.Tweening;
using ToonBlastClone.Components.Manager;
using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Data;
using UnityEngine;

namespace ToonBlastClone.Components
{
    public class CollectedBlock : PoolableObject
    {
        [SerializeField] private float movementDurationPerDistance;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Move(Sprite sprite, Vector3 begining, Vector3 target, GoalBlockData key)
        {
            spriteRenderer.sprite = sprite;
            var position = transform.position;
            begining.z = position.z;
            target.z = position.z;
            transform.position = begining;

            float dist = Vector3.Distance(begining, target);

            transform.DOMove(target, movementDurationPerDistance * dist).OnComplete((() =>
            {
                gameObject.SetActive(false);
                GUIManager.Instance.DecreaseGoal(key);
            }));
        }
    }
}