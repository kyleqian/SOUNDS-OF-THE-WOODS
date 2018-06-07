using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class    EffectsManager : ManagerBase
{
    struct Lighting
    {
        public readonly Color32 ambientSkyColor;
        public readonly Color32 skyboxTintColor;
        public readonly float bloomThreshold;
        public readonly Color32 directionalLightColor;
        public readonly float directionalLightIntensity;

        public Lighting(Color32 ambientSkyColor, Color32 skyboxTintColor,
                        float bloomThreshold, Color32 directionalLightColor,
                        float directionalLightIntensity)
        {
            this.ambientSkyColor = ambientSkyColor;
            this.skyboxTintColor = skyboxTintColor;
            this.bloomThreshold = bloomThreshold;
            this.directionalLightColor = directionalLightColor;
            this.directionalLightIntensity = directionalLightIntensity;
        }
    }

    [SerializeField] Bloom bloomEffect;
    [SerializeField] Light directionalLight;

    Material copySkyboxMaterial;
    Dictionary<GamePhase, Lighting> lightingReference;
    Coroutine activeCoroutine;

    void Awake()
    {
        InitializeLightingReference();

        // Make in-memory copy of Material so we don't overwrite the original
        copySkyboxMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = copySkyboxMaterial;
    }

    void InitializeLightingReference()
    {
        Lighting afternoonLighting = new Lighting(
            new Color32(180, 231, 162, 255),
            new Color32(173, 149, 86, 255),
            0.6f,
            new Color32(255, 229, 85, 255),
            1.5f
        );
        Lighting duskLighting = new Lighting(
            new Color32(180, 123, 117, 255),
            new Color32(96, 146, 166, 255),
            0.69f,
            new Color32(255, 161, 0, 255),
            1.5f
        );
        Lighting nightLighting = new Lighting(
            new Color32(23, 37, 48, 255),
            new Color32(15, 31, 27, 255),
            0.69f,
            new Color32(0, 241, 255, 255),
            1.2f
        );
        Lighting latenightLighting = new Lighting(
            new Color32(14, 14, 14, 255),
            new Color32(14, 14, 14, 255),
            0.69f,
            new Color32(2, 0, 255, 255),
            0.5f
        );
        Lighting dawnLighting = new Lighting(
            new Color32(187, 198, 255, 255),
            new Color32(96, 146, 166, 255),
            0.69f,
            new Color32(2, 0, 255, 255),
            0.5f
        );

        lightingReference = new Dictionary<GamePhase, Lighting>();
        lightingReference.Add(GamePhase.Afternoon, afternoonLighting);
        lightingReference.Add(GamePhase.Dusk, duskLighting);
        lightingReference.Add(GamePhase.Night, nightLighting);
        lightingReference.Add(GamePhase.Latenight, latenightLighting);
        lightingReference.Add(GamePhase.Dawn, dawnLighting);
    }

    void StopActiveCoroutine()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
    }

    void UpdateLightingImmediate(Lighting lighting)
    {
        RenderSettings.ambientSkyColor = lighting.ambientSkyColor;
        copySkyboxMaterial.SetColor("_Tint", lighting.skyboxTintColor);
        bloomEffect.bloomThreshold = lighting.bloomThreshold;
        directionalLight.color = lighting.directionalLightColor;
        directionalLight.intensity = lighting.directionalLightIntensity;
    }

    void UpdateLightingOverTime(Lighting lighting)
    {
        StopActiveCoroutine();
        activeCoroutine = StartCoroutine(_UpdateLightingOverTime(lighting));
    }

    IEnumerator _UpdateLightingOverTime(Lighting lighting)
    {
        float phaseLength = GameManager.Instance.PhaseLengths[(int)GameManager.Instance.CurrPhase];
        Lighting initialLighting = new Lighting(
            RenderSettings.ambientSkyColor,
            copySkyboxMaterial.GetColor("_Tint"),
            bloomEffect.bloomThreshold,
            directionalLight.color,
            directionalLight.intensity
        );

        while (GameManager.Instance.CurrPhaseTime < phaseLength)
        {
            float lerpFactor = GameManager.Instance.CurrPhaseTime / phaseLength;
            Lighting lerpedLighting = new Lighting(
                Color32.Lerp(initialLighting.ambientSkyColor, lighting.ambientSkyColor, lerpFactor),
                Color32.Lerp(initialLighting.skyboxTintColor, lighting.skyboxTintColor, lerpFactor),
                Mathf.Lerp(initialLighting.bloomThreshold, lighting.bloomThreshold, lerpFactor),
                Color32.Lerp(initialLighting.directionalLightColor, lighting.directionalLightColor, lerpFactor),
                Mathf.Lerp(initialLighting.directionalLightIntensity, lighting.directionalLightIntensity, lerpFactor)
            );
            UpdateLightingImmediate(lerpedLighting);
            yield return null;
        }
    }

    protected override void OnPhaseLoad(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Afternoon:
                // Initial lighting
                Lighting lighting = lightingReference[phase];
                Lighting nextLighting = lightingReference[phase + 1];

                UpdateLightingImmediate(lighting);
                UpdateLightingOverTime(nextLighting);
                break;
            case GamePhase.Dusk:
                nextLighting = lightingReference[phase + 1];
                UpdateLightingOverTime(nextLighting);
                break;
            case GamePhase.Night:
                nextLighting = lightingReference[phase + 1];
                UpdateLightingOverTime(nextLighting);
                break;
            case GamePhase.Latenight:
                nextLighting = lightingReference[phase + 1];
                UpdateLightingOverTime(nextLighting);
                break;
            case GamePhase.Dawn:
                StopActiveCoroutine();
                break;
            case GamePhase.End:
                StopActiveCoroutine();
                break;
        }
    }

    protected override void OnPhaseUnload(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Afternoon:
                break;
            case GamePhase.Dusk:
                break;
            case GamePhase.Night:
                break;
            case GamePhase.Latenight:
                break;
            case GamePhase.Dawn:
                break;
            case GamePhase.End:
                break;
        }
    }
}
