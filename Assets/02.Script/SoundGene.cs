using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGene : MonoBehaviour
{
    float sampleRate;
    AudioSource audioSource;
    public Dictionary<int, List<float>> frequencies;
    List<float> phase, increment;
    public float volume = 0.1f;
    private void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
        phase = new List<float> { 0, 0 };
        increment = new List<float> { 0, 0 };
        frequencies = new Dictionary<int, List<float>>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = true;
    }
    public void OnKey(int keyNumber)
    {
        float freq = 440 * Mathf.Pow(2, ((float)keyNumber - 69f) / 12f);
        frequencies[keyNumber] = new List<float> { freq, 0 };
    }
    public void OnKeyOff(int keyNumber)
    {
        frequencies.Remove(keyNumber);
    }
    public void ChangePitch(int keyNumber, float pitch)
    {
        frequencies[keyNumber][1] = pitch;
    }
    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
                OnKey(80+i);
            if (Input.GetKeyUp(KeyCode.Keypad0 + i))
                OnKeyOff(80 + i);
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        int counter = 0;
        foreach (var item in frequencies.Keys)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                float freq = frequencies[item][0];

                float incrementAmount = (freq) * 2f * Mathf.PI / sampleRate;
                phase[counter] += incrementAmount;
                data[i] += (float)(volume*Mathf.Sin(phase[counter]));

                if (channels == 2)
                {
                    data[i + 1] = data[i];
                }
                if (phase[counter] > (Mathf.PI * 2f))
                {
                    phase[counter] = 0f;
                }
            }
            counter++;
        }
    }
}
