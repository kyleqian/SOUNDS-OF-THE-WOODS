using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Light lightSource;

    public float CurrBattery { get; private set; }
    [SerializeField] float maxBattery;
    [SerializeField] SpriteRenderer batteryBar;
    Color green, red;

    [SerializeField] string layerMaskName;
    int layerMask;
    const float RAYCAST_DISTANCE = 15f;

    // TODO: Does this work as creatures Spawn and Despawn?
    int prevCreatureId;
    float prevCreatureLookDuration;

    void Awake()
    {
        green = new Color(0.51f, 1, 0.25f);
        red = new Color(0.89f, 0, 0.09f);
        layerMask = 1 << LayerMask.NameToLayer(layerMaskName);
        CurrBattery = maxBattery;
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
        if (OVRInput.GetDown(OVRInput.Button.One))
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
        batteryBar.size = new Vector2(0.1f, Mathf.Lerp(0, 0.3f, CurrBattery / maxBattery));
        batteryBar.color = Color.Lerp(red, green, CurrBattery / maxBattery);
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
