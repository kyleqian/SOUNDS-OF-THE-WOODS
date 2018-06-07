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

    void DetectCreatures()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, layerMask))
        {
            Debug.DrawRay(transform.position, transform.forward);
        }
    }
}
