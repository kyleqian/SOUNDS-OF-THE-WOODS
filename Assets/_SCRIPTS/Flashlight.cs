using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float CurrBattery { get; private set; }
    [SerializeField] Light lightSource;
    [SerializeField] string layerMaskName;
    int layerMask;

    void Awake()
    {
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
        }
    }

    void DetectCreatures()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.position, out hit, ))
    }
}
