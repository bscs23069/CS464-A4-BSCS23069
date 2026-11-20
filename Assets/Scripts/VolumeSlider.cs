using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// On the Canvas, not on the hidden pause panel: a script on a hidden panel doesn't start
// until the panel opens, so the saved volume wouldn't be applied until then.
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string parameter = "MusicVolume"; // the exposed name, and the PlayerPrefs key
    [SerializeField] Slider slider;

    // SetFloat doesn't stick in Awake, so Start
    void Start()
    {
        slider.minValue = 0.0001f; // Log10(0) is minus infinity
        slider.maxValue = 1f;
        float value = SaveData.Volume(parameter);
        slider.SetValueWithoutNotify(value);
        Apply(value);
        slider.onValueChanged.AddListener(Set);
    }

    void OnDestroy() => slider.onValueChanged.RemoveListener(Set);

    public void Set(float value)
    {
        Apply(value);
        SaveData.SetVolume(parameter, value);
    }

    void Apply(float value) => mixer.SetFloat(parameter, ToDecibels(value));

    // 1 is 0 dB, 0.5 is -6 dB, 0.0001 is -80 dB
    public static float ToDecibels(float value) => Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
}
