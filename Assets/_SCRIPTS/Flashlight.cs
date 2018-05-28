using UnityEngine;

public class Flashlight : MonoBehaviour
{
	[SerializeField] Light light;

	void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.One))
		{
			light.enabled = !light.enabled;
		}
	}
}
