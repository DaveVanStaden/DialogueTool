using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerClickHandler
{
    private RectTransform _panelRectTransform;
    private Vector2 _pointerOffset;
    private AddSentenceToManager _sentenceToManager;
    public DialogueManager dialogueManager;
    
    private void Start()
    {
        _sentenceToManager = GetComponent<AddSentenceToManager>();
    }

    private void Awake()
    {
        _panelRectTransform = transform.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_panelRectTransform, eventData.position, eventData.pressEventCamera, out _pointerOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
        Vector2 localPointerPosition = ClampToCanvas(eventData);
        Vector2 newPanelPosition = localPointerPosition - _pointerOffset;
        _panelRectTransform.localPosition = new Vector3(newPanelPosition.x, newPanelPosition.y, _panelRectTransform.localPosition.z);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            dialogueManager.RemoveSentence(_sentenceToManager.previousSentence);
            dialogueManager.RemoveTitle(_sentenceToManager.previousName);

            Destroy(gameObject);

            dialogueManager.RemoveFromSentenceHolders(_sentenceToManager.gameObject);
            dialogueManager.UpdateAllIndices();
            dialogueManager.UpdateNumbering();
            dialogueManager.RemoveSentenceAmount();
        }
    }

    private Vector2 ClampToCanvas(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        RectTransform transformRect = _panelRectTransform.parent.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_panelRectTransform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition);
        float halfWidth = _panelRectTransform.rect.width * 0.5f;
        float halfHeight = _panelRectTransform.rect.height * 0.5f;
        float maxX = transformRect.rect.width - halfWidth;
        float maxY = transformRect.rect.height - halfHeight;
        float x = Mathf.Clamp(localPointerPosition.x, -maxX, maxX);
        float y = Mathf.Clamp(localPointerPosition.y, -maxY, maxY);
        return new Vector2(x, y);
    }
}
