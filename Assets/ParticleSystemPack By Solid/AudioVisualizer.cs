using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    public enum FrequencyBand { Bass, Mids, Highs }
    public enum ScaleMode { Volume, FrequencyWeighted }

    [System.Serializable]
    public class ParticleEffectConfig
    {
        public ParticleSystem particleSystem;
        public float minRate = 0;
        public float maxRate = 100;
        public ScaleMode mode = ScaleMode.Volume;
    }

    [System.Serializable]
    public class GameObjectEffectConfig
    {
        public GameObject target;
        public bool modifyScale;
        public bool modifyRotation;
        public Vector3 minScale = Vector3.one;
        public Vector3 maxScale = Vector3.one * 2;
        public Vector3 rotationAxis = Vector3.up;
        public float minRotationSpeed = 0f;
        public float maxRotationSpeed = 360f;
        public ScaleMode mode = ScaleMode.Volume;
    }

    [System.Serializable]
    public class LightEffectConfig
    {
        public Light light;
        public float minIntensity = 0f;
        public float maxIntensity = 1f;
        public float minRange = 1f;
        public float maxRange = 10f;
        public ScaleMode mode = ScaleMode.Volume;
    }

    [System.Serializable]
    public class BandConfig
    {
        public FrequencyBand band;
        public List<ParticleEffectConfig> particleEffects = new List<ParticleEffectConfig>();
        public List<GameObjectEffectConfig> objectEffects = new List<GameObjectEffectConfig>();
        public List<LightEffectConfig> lightEffects = new List<LightEffectConfig>();
    }

    [Header("General Settings")]
    public int sampleSize = 512;
    public FFTWindow fftWindow = FFTWindow.Blackman;
    public int sampleInterval = 4;
    [Range(0,1)] public float smoothing = 0.8f;

    [Header("Bands Configuration")]
    public List<BandConfig> bandConfigs = new List<BandConfig>();

    [Header("Debug (read-only)")]
    [Tooltip("Detected peak frequency in Hz")] public float debugPeakFrequency;
    [Tooltip("Band corresponding to peak frequency")] public FrequencyBand debugPeakBand;

    private AudioSource audioSource;
    private float[] spectrumData;
    private float[] lastVolume;
    private float[] currentVolume;
    private float[] lastWeighted;
    private float[] currentWeighted;
    private int frameCounter;
    private float binFrequency;
    private int bandCount;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spectrumData = new float[sampleSize];
        bandCount = bandConfigs.Count;
        lastVolume = new float[bandCount];
        currentVolume = new float[bandCount];
        lastWeighted = new float[bandCount];
        currentWeighted = new float[bandCount];
        binFrequency = AudioSettings.outputSampleRate / 2f / sampleSize;
    }

    void Update()
    {
        frameCounter++;
        if (frameCounter >= sampleInterval)
        {
            frameCounter = 0;
            audioSource.GetSpectrumData(spectrumData, 0, fftWindow);

            // Debug peak frequency/band
            int peakIdx = 0;
            float peakVal = 0f;
            for (int i = 0; i < spectrumData.Length; i++)
            {
                if (spectrumData[i] > peakVal)
                {
                    peakVal = spectrumData[i];
                    peakIdx = i;
                }
            }
            debugPeakFrequency = peakIdx * binFrequency;
            debugPeakBand = ClassifyBand(debugPeakFrequency);

            // Compute and smooth values for each band
            for (int i = 0; i < bandCount; i++)
            {
                float vol = GetBandVolume(bandConfigs[i].band);
                float wt = GetWeightedBandValue(bandConfigs[i].band);
                currentVolume[i] = Mathf.Lerp(lastVolume[i], vol, 1 - smoothing);
                currentWeighted[i] = Mathf.Lerp(lastWeighted[i], wt, 1 - smoothing);
                lastVolume[i] = currentVolume[i];
                lastWeighted[i] = currentWeighted[i];
            }
        }

        float t = Mathf.Clamp01((float)frameCounter / sampleInterval);
        for (int i = 0; i < bandCount; i++)
        {
            ApplyBandEffects(i, t);
        }
    }

    void ApplyBandEffects(int index, float t)
    {
        BandConfig config = bandConfigs[index];
        float volValue = Mathf.Lerp(lastVolume[index], currentVolume[index], t);
        float wtValue = Mathf.Lerp(lastWeighted[index], currentWeighted[index], t);

        if (currentVolume[index] != 0f && currentWeighted[index] != 0f)
        {
            // Particle effects
            foreach (var pe in config.particleEffects)
            {
                float val = (pe.mode == ScaleMode.Volume) ? volValue : wtValue;
                var emission = pe.particleSystem.emission;
                float rate = Mathf.Lerp(pe.minRate, pe.maxRate, val);
                var ro = emission.rateOverTime;
                ro.constant = rate;
                emission.rateOverTime = ro;
            }

            // GameObject effects
            foreach (var go in config.objectEffects)
            {
                float val = (go.mode == ScaleMode.Volume) ? volValue : wtValue;
                if (go.modifyScale)
                    go.target.transform.localScale = Vector3.Lerp(go.minScale, go.maxScale, val);
                if (go.modifyRotation)
                {
                    float speed = Mathf.Lerp(go.minRotationSpeed, go.maxRotationSpeed, val);
                    go.target.transform.Rotate(go.rotationAxis.normalized * speed * Time.deltaTime, Space.Self);
                }
            }

            // Light effects
            foreach (var le in config.lightEffects)
            {
                float val = (le.mode == ScaleMode.Volume) ? volValue : wtValue;
                le.light.intensity = Mathf.Lerp(le.minIntensity, le.maxIntensity, val);
                le.light.range = Mathf.Lerp(le.minRange, le.maxRange, val);
            }
        }
    }

    float GetBandVolume(FrequencyBand band)
    {
        int start = IndexStart(band), end = IndexEnd(band);
        float sum = 0f;
        for (int i = start; i < end; i++) sum += spectrumData[i];
        return sum / Mathf.Max(1, end - start);
    }

    float GetWeightedBandValue(FrequencyBand band)
    {
        int start = IndexStart(band), end = IndexEnd(band);
        float wSum = 0f, totalW = 0f;
        for (int i = start; i < end; i++)
        {
            float freq = i * binFrequency;
            wSum += spectrumData[i] * freq;
            totalW += freq;
        }
        return totalW > 0f ? wSum / totalW : 0f;
    }

    int IndexStart(FrequencyBand band)
    {
        switch (band)
        {
            case FrequencyBand.Bass: return 0;
            case FrequencyBand.Mids: return sampleSize / 3;
            default: return 2 * sampleSize / 3;
        }
    }

    int IndexEnd(FrequencyBand band)
    {
        switch (band)
        {
            case FrequencyBand.Bass: return sampleSize / 3;
            case FrequencyBand.Mids: return 2 * sampleSize / 3;
            default: return sampleSize;
        }
    }

    FrequencyBand ClassifyBand(float freqHz)
    {
        float nyquist = AudioSettings.outputSampleRate / 2f;
        if (freqHz < nyquist / 3f) return FrequencyBand.Bass;
        if (freqHz < 2f * nyquist / 3f) return FrequencyBand.Mids;
        return FrequencyBand.Highs;
    }
}
