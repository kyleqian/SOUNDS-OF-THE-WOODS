using System.Collections;
using TMPro;
using UnityEngine;

public class Start : MonoBehaviour
{
    bool started;

    void Update()
    {
#if UNITY_EDITOR
        if (!started && Input.GetKeyDown(KeyCode.Space))
#else
        if (!started && OVRInput.GetDown(OVRInput.Button.One))
#endif
        {
			started = true;
			GameManager.Instance.PressedStart();
			StartCoroutine(Fade());
		}
	}

	IEnumerator Fade()
    {
		TextMeshPro tmp = GetComponent<TextMeshPro>();
		float length = 2;

		for (float i = 0; i < length; i += Time.deltaTime)
		{
			float t = i / length;
			t = t * t;
			tmp.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
			yield return null;
		}

		Destroy(gameObject);
	}
}
