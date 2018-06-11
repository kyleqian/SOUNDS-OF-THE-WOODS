using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimator : MonoBehaviour {

	public CreatureBase cb;
	public void Footstep(){
		cb.Footstep();
	}
	public void Bugle(){
		((Crow)cb).Bugle();
	}
	public void StopSong(){
		((Decoy)cb).StopSong();
	}

	public void BadTeleport(){
		((Shapeshifter)cb).BadTeleport();
	}


	
}
