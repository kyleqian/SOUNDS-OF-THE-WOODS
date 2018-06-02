using UnityEngine;

public class Flashlight : MonoBehaviour
{
	[SerializeField] Light lightSource;

	public float currBattery;

	void Update()
	{
		if (currBattery <= 0)
		{
			return;
		}

		if (OVRInput.GetDown(OVRInput.Button.One))
		{
            lightSource.enabled = !lightSource.enabled;
		}

        if (lightSource.enabled)
		{
			currBattery--;
			if (currBattery <= 0)
			{
                lightSource.enabled = false;
			}
		}
	}
}
