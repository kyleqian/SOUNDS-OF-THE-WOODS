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

    // TODO: Does this work as creatures Spawn and Despawn?
    int prevCreatureId;
    float prevCreatureLookDuration;

    void Awake()
    {
        green = new Color(0.51f, 1, 0.25f);
        red = new Color(0.89f, 0, 0.09f);
        layerMask = LayerMask.NameToLayer(layerMaskName);
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

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            lightSource.enabled = !lightSource.enabled;
        }
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

        //Debug.DrawRay(transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, layerMask))
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
