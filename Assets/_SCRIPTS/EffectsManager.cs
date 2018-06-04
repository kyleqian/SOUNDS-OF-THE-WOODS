using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class EffectsManager : ManagerBase
{
    struct LightingCondition
    {
        public readonly Color32 ambientSkyColor;
        public readonly Color32 skyboxColor;
        public readonly float bloomThreshold;
        public readonly Color32 directionalLightColor;
        public readonly float directionalLightIntensity;

        public LightingCondition(Color32 ambientSkyColor, Color32 skyboxColor,
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

    Dictionary<GamePhase, LightingCondition> lighting;

    void Awake()
    {
        InitializeLightingConditions();
    }

    void InitializeLightingConditions()
    {
        LightingCondition afternoonLighting = new LightingCondition(
            new Color32(180, 231, 162, 255),
            new Color32(173, 149, 86, 255),
            0.6f,
            new Color32(255, 229, 85, 255),
            1.5f
        );
        LightingCondition duskLighting = new LightingCondition(
            new Color32(180, 123, 117, 255),
            new Color32(96, 146, 166, 255),
            0.69f,
            new Color32(255, 161, 0, 255),
            1.5f
        );
        LightingCondition nightLighting = new LightingCondition(
            new Color32(23, 37, 48, 255),
            new Color32(15, 31, 27, 255),
            0.69f,
            new Color32(0, 241, 255, 255),
            1.2f
        );
        LightingCondition latenightLighting = new LightingCondition(
            new Color32(14, 14, 14, 255),
            new Color32(14, 14, 14, 255),
            0.69f,
            new Color32(2, 0, 255, 255),
            0.5f
        );
        LightingCondition dawnLighting = new LightingCondition(
            new Color32(187, 198, 255, 255),
            new Color32(96, 146, 166, 255),
            0.69f,
            new Color32(2, 0, 255, 255),
            0.5f
        );

        lighting = new Dictionary<GamePhase, LightingCondition>();
        lighting.Add(GamePhase.Afternoon, afternoonLighting);
        lighting.Add(GamePhase.Dusk, duskLighting);
        lighting.Add(GamePhase.Night, nightLighting);
        lighting.Add(GamePhase.Latenight, latenightLighting);
        lighting.Add(GamePhase.Dawn, dawnLighting);
    }

    protected override void OnPhaseLoad(GamePhase phase)
    {
        LightingCondition lightingCondition = lighting[phase];

        switch (phase)
        {
            case GamePhase.Afternoon:
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
