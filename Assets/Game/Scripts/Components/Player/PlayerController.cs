using ToonBlastClone.Components.Patterns;
using ToonBlastClone.Data;
using ToonBlastClone.Enums;
using ToonBlastClone.Interface;
using ToonBlastClone.Logic;
using ToonBlastClone.Structs;
using ToonBlastClone.Structs.Event;
using UnityEngine;

namespace ToonBlastClone.Components
{
    public class PlayerController : Singleton<PlayerController>
        , EventListener<InputEvent>
    {
        private GridGenerator _gridGenerator;

        private void Start()
        {
            _gridGenerator = FindObjectOfType<GridGenerator>();
        }

        void ManualRaycast(Vector3 inputPosition)
        {
            ScreenToWorldPointData data = CameraController.Instance.ScreenToWorldPoint(inputPosition);
            CellData touchedCell = _gridGenerator!.GridModel.TouchedCell(data);
            if (touchedCell != null && _gridGenerator != null)
            {
                ITouchable touchable = touchedCell.BlockSO as ITouchable;
                touchable?.Touch(touchedCell, _gridGenerator);
            }
        }

        private void OnEnable()
        {
            EventManager.EventStartListening(this);
        }

        private void OnDisable()
        {
            EventManager.EventStopListening(this);
        }

        public void OnEventTrigger(InputEvent currentEvent)
        {
            if (currentEvent.State == TouchState.Touch)
            {
                ManualRaycast(currentEvent.Position);
            }
        }
    }
}