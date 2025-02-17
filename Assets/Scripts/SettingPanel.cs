using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public TextMeshProUGUI volumeTextIndicator;
    public Slider volumeSlider;
    public string parameterName;
    void Start()
    {
        volumeSlider.value=GetCurrentVolume();
        float volume=volumeSlider.value;
        volumeTextIndicator.text=((int)(volume*100)).ToString();
    }

    public void OnVolumeChange()
    {
        float volume=volumeSlider.value;
        volumeTextIndicator.text=((int)(volume*100)).ToString();// volumeSlider.value.ToString();
        float dbValue=volume>0? Mathf.Log10(volume) * 20 : -80f;
        AudioManager.Instance.mainMixer.SetFloat(parameterName, dbValue);
    }

    public float GetCurrentVolume()
    {
        float volumeValue;
        AudioManager.Instance.mainMixer.GetFloat(parameterName, out volumeValue);
        return Mathf.Pow(10, volumeValue / 20);
    }
    // private void SetVolume(string parameterName, float value)
    // {
    //     // Convert slider value (0 to 1) to decibels (-80dB to 0dB)
    //     float dbValue = value > 0 ? Mathf.Log10(value) * 20 : -80f;
    //     audioMixer.SetFloat(parameterName, dbValue);
    // }
}
