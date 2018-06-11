using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butterfly : MonoBehaviour
{

    float speed = 0.2f;
    Vector3 target;

    //bool movingTarget;
    void Start()
    {
        InitializeColors();
        speed = Random.Range(0.05f, 0.3f);
        GetComponent<Animator>().SetFloat("speed", speed * 2 + 0.6f);
        transform.localScale = Vector3.one * Random.Range(0.6f, 1.2f);
        target = getTarget();

    }
    Vector3 getTarget()
    {
        float x = (transform.position.x < 0) ? UnityEngine.Random.Range(-4f, -1.5f) : UnityEngine.Random.Range(1.5f, 4f);
        float z = (transform.position.z < 0) ? UnityEngine.Random.Range(-4f, -1.5f) : UnityEngine.Random.Range(1.5f, 4f);
        return new Vector3(x, UnityEngine.Random.Range(0.74f, 2), z);
    }

    void InitializeColors()
    {
        Color c = Random.ColorHSV(0, 1, 0.05f, 0.8f, 0.6f, 1);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = c;
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = c;
    }

    // IEnumerator moveTarget()
    // {
    //     movingTarget = true;
    //     float maxTime = Random.Range(5f, 10f);
    //     Vector3 newTarget = getTarget();
    //     Vector3 oldTarget = target;
    //     yield return null;
    //     for (float i = 0; i < maxTime && isActiveAndEnabled; i += Time.deltaTime)
    //     {
    //         target = Vector3.Lerp(oldTarget, newTarget, i / maxTime);
    //         yield return null;
    //     }
    //     movingTarget = false;
    // }
    // Update is called once per frame
    void Update()
    {
        // if (movingTarget)
        // {
        // 	transform.LookAt(target,transform.forward);
        //     return;
        // }

        if (transform.position == target)
        {
            target = getTarget();
            //StartCoroutine(moveTarget());
            speed = Random.Range(0.1f, 0.3f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            Quaternion wantedRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, wantedRotation, Time.time * speed * 0.005f);
        }
    }
}
