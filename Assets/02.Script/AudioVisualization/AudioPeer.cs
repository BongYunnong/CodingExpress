using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource audioSource;
    public static float[] samplesLeft = new float[512];
    public static float[] samplesRight = new float[512];

    float[] freqBand = new float[8];
    float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    float[] freqBandHighest = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    public static float amplitude, amplitudeBuffer;

    public float audioProfile;

    float amplitudeHighest;
    public enum Channel {
        Stereo,
        Left,
        Right
    }
    public Channel channel;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioProfile(audioProfile);
    }
    void AudioProfile(float _audioProfile)
    {
        for(int i = 0; i < 8; i++)
        {
            freqBandHighest[i] = _audioProfile;
        }
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for(int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if(currentAmplitude> amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }

        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    void CreateAudioBands()
    {
        for(int i = 0; i < 8; i++)
        {
            if(freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }
            audioBand[i] = (freqBand[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }
    void GetSpectrumAudioSource()
    {
        // AudioSource의 sample 값을 가져올 수 있다.
        audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        // Stereo를 위함
        audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for(int i = 0; i < 8; i++)
        {
            if(freqBand[i] > bandBuffer[i])
            {
                // buffer에 저장된 값보다 현재 값이 크면 buffer 업데이트
                bandBuffer[i] = freqBand[i];
                // buffer가 업데이트 된다면 decrease값도 초기화
                bufferDecrease[i] = 0.5f;
            }
            if(freqBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i]*Time.deltaTime;
                // 가속도를 붙여서 줄어들 것임
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        // 512개의 sample에서 8개의 Band를 만들어서 나눌 것임
        /*
         * 22050 / 512 = 43hertz per sample
         * 
         * 20-60 hertz
         * 60-250 hertz
         * 500-2000 hertz
         * 2000-4000 hertz
         * 4000-6000 hertz
         * 6000-20000 hertz
         * 
         * 0-2 = 86 hertz
         * 1-4 = 172 hertz - 87-258
         * 2-8 = 344 hertz - 259-602
         * 3-16 = 688 hertz - 603-1290
         * 4-32 = 1376 hertz - 1291-2666
         * 5-64 = 2752 hertz - 2667-5418
         * 6-128 = 5504hertz - 5419-10922
         * 7-256 = 11008hertz - 10923-21930
         * 
         */

        int count = 0;
        for(int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;


            if (i == 7)
            {
                sampleCount += 2;
            }

            for(int j = 0; j < sampleCount; j++)
            {
                switch (channel)
                {
                    case Channel.Stereo:
                        average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                        break;
                    case Channel.Left:
                        average += (samplesLeft[count]) * (count + 1);
                        break;
                    case Channel.Right:
                        average += (samplesRight[count]) * (count + 1);
                        break;
                }
                
                count++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }
    }
}
