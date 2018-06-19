using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;


public enum ParticleType
{
    Butterflies, Dust, Fireflies
}
public class EffectsManager : ManagerBase
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

    public Material ground;

    const float LIGHTING_TRANSITION_PROPORTION = 0.3f; // At what point in the phase should lighting be finished transitioning?
    Material copySkyboxMaterial;
    Dictionary<GamePhase, Lighting> lightingReference;
    Coroutine activeCoroutine;

    //Particle effects
    [Header("Particles")]
    [SerializeField]
    GameObject butterfly;
    [SerializeField]
    GameObject dust, fireflies;

    void Awake()
    {
        InitializeLightingReference();

        // Make in-memory copy of Material so we don't overwrite the original
        copySkyboxMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = copySkyboxMaterial;
        ground.color=Color.white;
    }

    void Start()
    {
        GameManager.Instance.DeathByEnemy += DieEffect;
    }

    void DieEffect()
    {
        Color sky = copySkyboxMaterial.GetColor("_Tint");
        Color ambient = RenderSettings.ambientSkyColor;

        StopActiveCoroutine();
        StartCoroutine(FadeGroundIntensity(ground.color, Color.black, 7));

        Lighting lighting = new Lighting(
            new Color32(0, 0, 0, 255),
            new Color(sky.r * 1.5f, sky.g, sky.b, 1),
            0.14f,
            directionalLight.color,
            0f
        );

        UpdateLightingOverTime(lighting);
    }

    void InitializeParticle(ParticleType particle)
    {
        GameObject particleObject = null;
        switch (particle)
        {
            case ParticleType.Butterflies:
                //butterflies are not a Particle System thus need a special case.
                particleObject = handleButterflies(UnityEngine.Random.Range(4, 6));
                break;
            case ParticleType.Dust:
                particleObject = GameObject.Instantiate(dust, transform, true);
                break;
            case ParticleType.Fireflies:
                particleObject = GameObject.Instantiate(fireflies, transform, true);
                break;
        }
        if (particleObject != null)
            particleObject.name = particle.ToString();

    }

    GameObject handleButterflies(int butterflyAmount)
    {
        GameObject particleObject = new GameObject();
        particleObject.transform.parent = transform;
        for (int i = 0; i < butterflyAmount; i++)
        {
            GameObject.Instantiate(butterfly,
                    new Vector3(UnityEngine.Random.Range(-4f, 4f), UnityEngine.Random.Range(0.5f, 2), UnityEngine.Random.Range(-4f, 4f)),
                    Quaternion.identity,
                    particleObject.transform);
        }
        return particleObject;
    }

    void RemoveParticle(ParticleType particle)
    {
        Transform particleObject = transform.Find(particle.ToString());
        //check if particle we want to remove exists.
        if (particleObject == null) return;
        //Destroy it because we don't expect to need it in the future.
        Destroy(particleObject.gameObject);
    }

    IEnumerator FadeMoon(float length)
    {
        Transform moon = transform.Find("moon");
        Vector3 point0 = new Vector3(-20.77f, -4.88f, 24.42f);
        Vector3 point1 = new Vector3(-3.75f, 30.38f, 24.42f);
        Vector3 point2 = new Vector3(23.367f, -6.53f, 24.42f);
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            float t = i / length;
            Vector3 m1 = Vector3.Lerp(point0, point1, t);
            Vector3 m2 = Vector3.Lerp(point1, point2, t);
            moon.position = Vector3.Lerp(m1, m2, t);
            yield return null;
        }
    }

    //fade so ground is less shiny at night
    IEnumerator FadeGroundIntensity(Color one, Color two, float length)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            ground.color = Color.Lerp(one, two, i/length);
            yield return null;
        }
    }

    void InitializeLightingReference()
    {
        Lighting afternoonLighting = new Lighting(
            new Color32(180, 231, 162, 255),
            new Color32(173, 149, 86, 255),
            0.6f,
            new Color32(255, 229, 85, 255),
            0.7f
        );
        Lighting duskLighting = new Lighting(
            new Color32(128, 88, 84, 255),
            new Color32(43, 75, 98, 255),
            0.69f,
            new Color32(255, 161, 0, 255),
            1f
        );
        Lighting nightLighting = new Lighting(
            new Color32(9, 18, 20, 255),
            new Color32(10, 20, 15, 255),
            0.69f,
            new Color32(0, 50, 60, 255),
            0.5f
        );
        Lighting latenightLighting = new Lighting(
            new Color32(1, 1, 1, 255),//ambient sky
            new Color32(3, 3, 3, 255),//skybox tint
            0.69f,
            new Color32(2, 0, 2, 255),
            0.1f
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
        activeCoroutine = null;
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
        float adjustedPhaseLength = GameManager.Instance.PhaseLengths[(int)GameManager.Instance.CurrPhase] * LIGHTING_TRANSITION_PROPORTION;

        Lighting initialLighting = new Lighting(
            RenderSettings.ambientSkyColor,
            copySkyboxMaterial.GetColor("_Tint"),
            bloomEffect.bloomThreshold,
            directionalLight.color,
            directionalLight.intensity
        );

        while (GameManager.Instance.CurrPhaseTime < adjustedPhaseLength)
        {
            float lerpFactor = GameManager.Instance.CurrPhaseTime / adjustedPhaseLength;

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
            case GamePhase.Start:
                // Initial lighting and particles
                Lighting nextLighting = lightingReference[phase + 1];
                UpdateLightingImmediate(nextLighting);
                InitializeParticle(ParticleType.Dust);
                InitializeParticle(ParticleType.Butterflies);
                break;
            case GamePhase.Afternoon:


                break;
            case GamePhase.Dusk:
                float length = GameManager.Instance.PhaseLengths[(int)phase] + GameManager.Instance.PhaseLengths[(int)GamePhase.Night]
                + GameManager.Instance.PhaseLengths[(int)GamePhase.Latenight] + GameManager.Instance.PhaseLengths[(int)GamePhase.Dawn];
                StartCoroutine(FadeMoon(length));
                UpdateLightingOverTime(lightingReference[phase]);
                InitializeParticle(ParticleType.Fireflies);
                break;
            case GamePhase.Night:
                UpdateLightingOverTime(lightingReference[phase]);
                break;
            case GamePhase.Latenight:
                UpdateLightingOverTime(lightingReference[phase]);
                break;
            case GamePhase.Dawn:
                UpdateLightingOverTime(lightingReference[phase]);
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
            case GamePhase.Start:

                break;
            case GamePhase.Afternoon:
                break;
            case GamePhase.Dusk:
                RemoveParticle(ParticleType.Butterflies);

                StartCoroutine(FadeGroundIntensity(Color.white, Color.black, GameManager.Instance.minPhaseLengthInSeconds));
                break;
            case GamePhase.Night:
                RemoveParticle(ParticleType.Fireflies);
                break;
            case GamePhase.Latenight:
                RemoveParticle(ParticleType.Dust);
                break;
            case GamePhase.Dawn:
                StartCoroutine(FadeGroundIntensity(Color.black, Color.white, GameManager.Instance.minPhaseLengthInSeconds));

                break;
            case GamePhase.End:
                break;
        }
    }
}
