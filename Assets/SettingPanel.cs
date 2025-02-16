using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public TextMeshProUGUI volumeTextIndicator;
    public Slider volumeSlider;
    public void OnVolumeChange()
    {
        volumeTextIndicator.text=((int)((volumeSlider.value)*100)).ToString();// volumeSlider.value.ToString();
    }
}
