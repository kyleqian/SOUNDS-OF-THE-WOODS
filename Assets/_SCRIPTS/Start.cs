using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Start : MonoBehaviour {

	bool started;
	private void Update() {
		if (OVRInput.GetDown(OVRInput.Button.One) && !started){
			started=true;
			GameManager.Instance.PressedStart();
			StartCoroutine(fade());
		}
	}
	IEnumerator fade(){
		TextMeshPro tmp = GetComponent<TextMeshPro>();
		float length=2;
		for (float i = 0; i < length; i+=Time.deltaTime)
		{
			float t=i/length;
			t=t*t;
			tmp.color=new Color(1,1,1,Mathf.Lerp(1,0,t));
			yield return null;
			
		}
		Destroy (gameObject);
	}


}
