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
        float length = 1.3f;
        float i = 0;
        for (; i < length - 0.4f; i += Time.deltaTime)
        {
            float t = i / length;
            t = t * t;
            tmp.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            tmp2.color = tmp.color;
            yield return null;
        }
        startGame();

        for (; i < length; i += Time.deltaTime)
        {
            float t = i / length;
            t = t * t;
            tmp.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            tmp2.color = tmp.color;
            yield return null;
        }

    }

    void startGame()
    {
        Eye e = Camera.main.gameObject.GetComponent<Eye>();
        e.EyeClose(0.3f, false, () =>
        {
            GameManager.Instance.PressedStart();
            e.EyeOpen(0.8f, true, null, 0.1f);

            Destroy(gameObject);
        });
    }
}
