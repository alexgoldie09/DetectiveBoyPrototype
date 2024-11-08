using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier = 20f; // Suggested default multiplier for volume control

    private void Awake()
    {
        // Initialize the slider if needed
        if (slider == null)
            slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Set the initial slider position based on the current mixer volume level
        float currentVolume;
        if (audioMixer.GetFloat(parameter, out currentVolume))
        {
            // Convert the decibel level back to a linear value for the slider
            slider.value = Mathf.Pow(10, currentVolume / multiplier);
        }

        // Add listener to detect when the slider value changes
        slider.onValueChanged.AddListener(SliderValue);
    }

    public void SliderValue(float _value)
    {
        // Use Logarithmic scaling with multiplier to set the volume in decibels
        audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);
    }
}
