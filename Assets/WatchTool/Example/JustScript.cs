using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WatchTool;

public class JustScript : MonoBehaviour{

	[Watch] 
	public float angle = 7f;
	
	[Watch("AwesomeRange")] 
	public float range = 7f;
	
	[Watch("!Global")] 
	public Vector3 global = Vector3.up;

	[Watch] public string word = "hi";
	[Watch] public string noword;
	[Watch] public float nofloat;
	
	
	void Update(){
		angle += 0.08f;
		nofloat += 1;
	}
}
