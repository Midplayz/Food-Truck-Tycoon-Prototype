using UnityEngine;
using UnityEngine.EventSystems;

public class MapPanning : MonoBehaviour, IDragHandler
{
    private RectTransform mapRect;
    private Vector2 minPosition, maxPosition;

    [Header("Pan Settings")]
    public float panSpeed = 1.0f;

    void Start()
    {
        mapRect = GetComponent<RectTransform>();
        CalculateBounds();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = mapRect.anchoredPosition + (eventData.delta * panSpeed);
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);
        mapRect.anchoredPosition = newPosition;
    }

    private void CalculateBounds()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 mapSize = mapRect.sizeDelta * mapRect.lossyScale;

        float xLimit = (mapSize.x - screenSize.x) / 2;
        float yLimit = (mapSize.y - screenSize.y) / 2;

        minPosition = new Vector2(-xLimit, -yLimit);
        maxPosition = new Vector2(xLimit, yLimit);
    }
}
