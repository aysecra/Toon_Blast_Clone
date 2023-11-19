using System.Collections.Generic;
using DG.Tweening;
using ToonBlastClone.Data;
using ToonBlastClone.Enums;
using ToonBlastClone.Interface;
using ToonBlastClone.Logic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ToonBlastClone.Components
{
    public class Rocket : MergeElement
    {
        [SerializeField] private Transform negativeTransform;
        [SerializeField] private Transform positiveTransform;
        [SerializeField] private float _perCellDuration = .1f;
        [SerializeField] private Vector3 horizontalDirectionLocalPos;
        [SerializeField] private Vector3 verticalDirectionLocalPos;
        
        [SerializeField] private Vector3 horizontalDirectionLocalNegativeRot;
        [SerializeField] private Vector3 horizontalDirectionLocalPositiveRot;
        [SerializeField] private Vector3 verticalDirectionLocalNegativeRot;
        [SerializeField] private Vector3 verticalDirectionLocalPositiveRot;

        private RocketDirection _direction;
        public RocketDirection Direction => _direction;

        public override void SearchForExplosion(CellData cell, GridGenerator gridGenerator)
        {
            GridModel<GridGenerator> model = gridGenerator.GridModel;
            Vector2Int index = cell.Index;

            switch (_direction)
            {
                case RocketDirection.Horizontal:
                    RocketMovement(model.GetCell(new Vector2Int(0, index.y)).CellObject.transform.position,
                        model.GetCell(new Vector2Int(model.CellAmount.x - 1, index.y)).CellObject.transform.position,
                        model, gridGenerator, cell);
                    break;
                case RocketDirection.Vertical:
                    RocketMovement(model.GetCell(new Vector2Int(index.x, 0)).CellObject.transform.position,
                        model.GetCell(new Vector2Int(index.x, model.CellAmount.y - 1)).CellObject.transform.position,
                        model, gridGenerator, cell);
                    break;
            }
        }

        private void RocketMovement(Vector3 negativeTarget, Vector3 positiveTarget,
            GridModel<GridGenerator> model, GridGenerator gridGenerator, CellData cell)
        {
            List<CellData> detectedCell = new List<CellData>();

            float detectZ = negativeTarget.z;

            negativeTarget.z = -1.2f;
            positiveTarget.z = -1.2f;
            
            var position = transform.position;
            float distNegative = Vector3.Distance(position, negativeTarget);
            float distPositive = Vector3.Distance(position, positiveTarget);

            Tween tween1 = null;
            Tween tween2 = null;

            bool tween1Killed = false;
            bool tween2Killed = false;

            tween1 = negativeTransform.DOMove(negativeTarget, _perCellDuration * distNegative)
                .OnUpdate((() =>
                {
                    var position1 = negativeTransform.position;
                    CellData cell = model.GetCellFromPosition(position1);
                    if (cell != null)
                    {
                        IExplodable explodable = cell.BlockSO as IExplodable;
                    
                        if (explodable != null && !detectedCell.Contains(cell))
                        {
                            explodable.Explode(cell);
                            detectedCell.Add(cell);
                        }
                    }
                }))
                .OnComplete((() =>
                {
                    negativeTransform.gameObject.SetActive(false);
                }));

            tween2 = positiveTransform.DOMove(positiveTarget, _perCellDuration * distPositive)
                .OnUpdate((() =>
                {
                    var position1 = positiveTransform.position;
                    CellData cell = model.GetCellFromPosition(position1);
                    if (cell != null)
                    {
                        IExplodable explodable = cell.BlockSO as IExplodable;
                    
                        if (explodable != null && !detectedCell.Contains(cell))
                        {
                            explodable.Explode(cell);
                            detectedCell.Add(cell);
                        }
                    }
                }))
                .OnComplete((() =>
                {
                    positiveTransform.gameObject.SetActive(false);
                }));

            tween1.OnKill(() =>
            {
                tween1Killed = true;
                if (tween2Killed)
                {
                    cell.IsEmpty = true;
                    cell.CellObject = null;
                    cell.BlockSO = null;
                    detectedCell.Add(cell);
                    
                    gameObject.SetActive(false);

                    FallingController.Fall(detectedCell, gridGenerator);
                }
            });
            tween2.OnKill(() =>
            {
                tween2Killed = true;
                if (tween1Killed)
                {
                    cell.IsEmpty = true;
                    cell.CellObject = null;
                    cell.BlockSO = null;
                    detectedCell.Add(cell);
                    
                    gameObject.SetActive(false);
                    
                    FallingController.Fall(detectedCell, gridGenerator);
                }
            });
        }

        private void SetDirection()
        {
            int rand = Random.Range(0, 2);
            
            negativeTransform.gameObject.SetActive(true);
            positiveTransform.gameObject.SetActive(true);

            if (rand % 2 == 0)
            {
                _direction = RocketDirection.Horizontal;
                negativeTransform.localPosition = new Vector3(-horizontalDirectionLocalPos.x,
                    horizontalDirectionLocalPos.y, horizontalDirectionLocalPos.z);
                positiveTransform.localPosition = horizontalDirectionLocalPos;
                
                negativeTransform.localEulerAngles = horizontalDirectionLocalNegativeRot;
                positiveTransform.localEulerAngles = horizontalDirectionLocalPositiveRot;
            }
            else
            {
                _direction = RocketDirection.Vertical;
                negativeTransform.localPosition = new Vector3(verticalDirectionLocalPos.x,
                    -verticalDirectionLocalPos.y, verticalDirectionLocalPos.z);
                positiveTransform.localPosition = verticalDirectionLocalPos;
                
                negativeTransform.localEulerAngles = verticalDirectionLocalNegativeRot;
                positiveTransform.localEulerAngles = verticalDirectionLocalPositiveRot;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirection();
        }
    }
}