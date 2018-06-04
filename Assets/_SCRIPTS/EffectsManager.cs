using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class EffectsManager : ManagerBase
{
    struct Lighting
    {
        public readonly Color32 ambientSkyColor;
        public readonly Color32 skyboxColor;
        public readonly float bloomThreshold;
        public readonly Color32 directionalLightColor;
        public readonly float directionalLightIntensity;

        public Lighting(Color32 ambientSkyColor, Color32 skyboxColor,
                                float bloomThreshold, Color32 directionalLightColor,
                                float directionalLightIntensity)
        {
            this.ambientSkyColor = ambientSkyColor;
            this.skyboxColor = skyboxColor;
            this.bloomThreshold = bloomThreshold;
            this.directionalLightColor = directionalLightColor;
            this.directionalLightIntensity = directionalLightIntensity;
        }
    }

    [SerializeField] Material skyboxMaterial;
    [SerializeField] Bloom bloomEffect;
    [SerializeField] Light directionalLight;

    Dictionary<GamePhase, Lighting> lightingReference;

    void Awake()
    {
        InitializeLightingConditions();
    }

    void InitializeLightingConditions()
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

    void UpdateLighting()
    {
        
    }

    protected override void OnPhaseLoad(GamePhase phase)
    {
        Lighting lightingCondition = lightingReference[phase];
        switch (phase)
        {
            case GamePhase.Afternoon:
                // Initial lighting
                RenderSettings.ambientSkyColor = lightingCondition.ambientSkyColor;
                skyboxMaterial.SetColor("_Tint", lightingCondition.skyboxColor);
                bloomEffect.bloomThreshold = lightingCondition.bloomThreshold;
                directionalLight.color = lightingCondition.directionalLightColor;
                directionalLight.intensity = lightingCondition.directionalLightIntensity;

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
