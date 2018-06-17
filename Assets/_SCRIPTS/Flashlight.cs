using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Light lightSource;

    public float CurrBattery { get; private set; }
    [SerializeField] float maxBattery;
    [SerializeField] MeshRenderer battery;

    [SerializeField] string layerMaskName;
    int layerMask;
    const float RAYCAST_DISTANCE = 15f;

    // TODO: Does this work as creatures Spawn and Despawn?
    int prevCreatureId;
    float prevCreatureLookDuration;

    void Awake()
    {
        layerMask = 1 << LayerMask.NameToLayer(layerMaskName);
        CurrBattery = maxBattery;
    }

    void Start()
    {
        GameManager.Instance.GameOverEvent += GameOverHandler;
    }

    void GameOverHandler()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        HandleInputs();
        DrainBattery();
        DetectCreatures();
    }

    void HandleInputs()
    {
        if (CurrBattery <= 0)
        {
            return;
        }

#if UNITY_EDITOR
        float pitchDelta = Input.GetAxis("Vertical") * -2f;
        float yawDelta = Input.GetAxis("Horizontal") * 2f;
        Vector3 angles = transform.eulerAngles;
        angles.x += pitchDelta;
        angles.y += yawDelta;
        transform.eulerAngles = angles;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lightSource.enabled = !lightSource.enabled;
        }
#else
        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            lightSource.enabled = !lightSource.enabled;
        }
#endif
    }

    void DrainBattery()
    {
        if (lightSource.enabled)
        {
            CurrBattery--;
            if (CurrBattery <= 0)
            {
                lightSource.enabled = false;
            }
            else
            {
                SetBatterySprite();
            }
        }
    }

    void SetBatterySprite()
    {
        //battery.transform.localPosition = new Vector3(0, -0.008f, Mathf.Lerp(0.04f, 0.05f, CurrBattery / maxBattery));
        //battery.transform.localScale = new Vector3(0.006f, 0.005f, Mathf.Lerp(0, 0.02f, CurrBattery / maxBattery));
        battery.material.color = Color.Lerp(Color.red, Color.green, CurrBattery / maxBattery);
        battery.material.SetColor("_EmissionColor", battery.material.color);
    }

    // TODO: Efficient to call this every frame?
    void DetectCreatures()
    {
        if (!lightSource.enabled)
        {
            return;
        }

#if UNITY_EDITOR
        Debug.DrawRay(transform.position, transform.forward * RAYCAST_DISTANCE, Color.red);
#endif

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, RAYCAST_DISTANCE, layerMask))
        {
            GameObject creature = hit.collider.gameObject;
            int creatureId = creature.GetInstanceID();

            // How long you've been continuously looking at this creature
            if (creatureId == prevCreatureId)
            {
                prevCreatureLookDuration += Time.deltaTime;
            }
            else
            {
                prevCreatureLookDuration = 0;
            }

            // Inform creature how long you've been looking at it
            creature.GetComponentInParent<CreatureBase>().ISeeYou(prevCreatureLookDuration);

            prevCreatureId = creatureId;
        }
        else
        {
            prevCreatureId = 0;
            prevCreatureLookDuration = 0;
        }
    }
}
