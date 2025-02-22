using UnityEngine;

using UnityEngine.EventSystems;

public class UISoundHandler : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Sound Names")]
    [SerializeField] private string hoverSoundName = "ButtonHover";
    [SerializeField] private string clickSoundName = "ButtonClick";

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(hoverSoundName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(clickSoundName);
        Debug.Log("clicked");
    }
}