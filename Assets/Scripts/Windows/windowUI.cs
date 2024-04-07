using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class windowUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    Vector3 MouseDragStartPos;
    public PointerEventData.InputButton dragMouseButton;
    RectTransform rectTransform;
    public float borderSnapSize = 10;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == dragMouseButton)
        {
            transform.localPosition = Input.mousePosition - MouseDragStartPos;
            ScreenLimit();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == dragMouseButton)
            MouseDragStartPos = Input.mousePosition - transform.localPosition;
    }

    private void ScreenLimit()
    {
        Vector3 diffMin = rectTransform.position + (Vector3)rectTransform.rect.position;
        Vector3 diffMax = (Vector3)Camera.main.pixelRect.size - rectTransform.position + (Vector3)rectTransform.rect.position;

        if (diffMin.x < borderSnapSize)
            rectTransform.position -= new Vector3(diffMin.x, 0f, 0f);
        if (diffMin.y < borderSnapSize)
            rectTransform.position -= new Vector3(0f, diffMin.y, 0f);
        if (diffMax.x < borderSnapSize)
            rectTransform.position += new Vector3(diffMax.x, 0f, 0f);
        if (diffMax.y < borderSnapSize)
            rectTransform.position += new Vector3(0f, diffMax.y, 0f);
    }

}
