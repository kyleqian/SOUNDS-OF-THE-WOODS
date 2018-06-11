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
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        TextMeshPro tmp = GetComponent<TextMeshPro>();
        TextMeshPro tmp2 = transform.Find("subTitle").GetComponent<TextMeshPro>();
        float length = 2;

        for (float i = 0; i < length; i += Time.deltaTime)
        {
            float t = i / length;
            t = t * t;
            tmp.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
	    tmp2.color=tmp.color;
            yield return null;
        }
        yield return null;
        GameManager.Instance.PressedStart();
        Destroy(gameObject);
    }
}
