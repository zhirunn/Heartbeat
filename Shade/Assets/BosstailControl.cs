﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosstailControl : MonoBehaviour {

    public GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () { 
        transform.position = new Vector3 (target.transform.position.x, transform.position.y, transform.position.z);
	}
}
