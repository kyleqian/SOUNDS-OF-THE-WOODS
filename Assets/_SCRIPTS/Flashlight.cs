using UnityEngine;

public class Flashlight : MonoBehaviour
{
	[SerializeField] Light light;

	public float currBattery;

	void Update()
	{
		if (currBattery <= 0)
		{
			return;
		}

		if (OVRInput.GetDown(OVRInput.Button.One))
		{
			light.enabled = !light.enabled;
		}

		if (light.enabled)
		{
			currBattery--;
			if (currBattery <= 0)
			{
				light.enabled = false;
			}
		}
	}
}
