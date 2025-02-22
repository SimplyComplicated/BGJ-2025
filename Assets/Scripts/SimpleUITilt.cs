using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleUITilt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float tiltAngle = 15f;
    
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.rotation = Quaternion.Euler(0, 0, tiltAngle);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }
}