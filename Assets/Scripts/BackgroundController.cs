using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject columnPrefab;
    [SerializeField] private List<GameObject> columns = new List<GameObject>();
    [SerializeField] private int padding = 2;
    private int cameraHeight, cameraWidth, columnWidth;

    float[] samples = new float[512];
    public float[] frequencyBand = new float[8];
    public float[] bandBuffer = new float[8];
    public float[] bufferDecrease = new float[8];
    private AudioSource audioSource;

    int count, sampleCount;
    float average;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        MakeFrequancyBands();
        BandBuffer();
        UpdateColumns();
    }

    private void BandBuffer()
    {
        for (int i = 0; i < bandBuffer.Length; i++)
        {
            if (frequencyBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBand[i];
                bufferDecrease[i] = 0.005f;
            }
            else
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    private void MakeFrequancyBands()
    {
        count = 0;

        for (int i = 0; i < frequencyBand.Length; i++)
        {
            sampleCount = (int)Mathf.Pow(2, i) * 2;
            sampleCount += (i == 7) ? 2 : 0;
            average = 0f;

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            frequencyBand[i] = average * 10;
        }
    }

    private void UpdateColumns()
    {
        for (int i = 0; i < columns.Count; i++)
        {
            columns[i].transform.localScale = new Vector3(2, bandBuffer[i] * 10);
        }
    }
}
