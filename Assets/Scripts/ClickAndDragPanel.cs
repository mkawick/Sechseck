using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ClickAndDragPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameController gameController;
   // [SerializeField]
    Vector2 startPoint;

   // [SerializeField]
    Vector2 endPoint;

   // [SerializeField]
    bool drag;

    //float

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.pressPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        drag = true;
        endPoint = eventData.position;
        Vector2 delta = eventData.delta;
        gameController.DragCamera(delta.x);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
    }
}
