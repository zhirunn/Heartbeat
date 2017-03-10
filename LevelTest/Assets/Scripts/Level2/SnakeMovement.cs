﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour {

    public bool SnakeMode = false;
    public GameObject MainBody;
    public Transform handPos;
    public List<Transform> BodyParts = new List<Transform>();
    public float minddistance;
    public int size;
    public float speed = 0.5f;
    public float rotationspeed = 100;
    Vector2 dir = Vector2.left;
    public GameObject bodyprefab;
    private float dis;
    private Transform cur_part;
    private Transform prev_part;

    [HideInInspector]
    public Footprints footprints;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < size - 1; i++) {
            AddBodyPart();
            //set up the line renderer
        }
        //BodyParts[size-1] connects to shoulder

        footprints = GetComponent<Footprints>();
    }
	
	// Update is called once per frame
	void Update () {

        if (SnakeMode == true)
        {
            //FourDirHand();
            FreeHand();
            Move();
            if (Input.GetKeyUp(KeyCode.R))
            {
                SnakeMode = false;
                MainBody.GetComponent<Player>().PlayerMode = true;
            }
            /*else if (Input.GetKeyUp(KeyCode.Q)) {
                SnakeMode = false;              
                Leap(0.1f);
                MainBody.GetComponent<Player>().PlayerMode = true;
            }*/
        }
        else {
            Shrink(0.1f);
        }
    }
    public void AddBodyPart() {

        int length = BodyParts.Count - 1;
        Transform newpart = (Instantiate(bodyprefab, BodyParts[length].position, BodyParts[length].rotation) as GameObject).transform;

        // newpart.SetParent(transform);
        newpart.SetParent(MainBody.transform);
        BodyParts.Add(newpart);
        // Create Line Renderer for each node
    }

    public void Move()
    {
        for (int i = 1; i < BodyParts.Count; i++) {
            cur_part = BodyParts[i];
            prev_part = BodyParts[i - 1];

            dis = Vector3.Distance(prev_part.position, cur_part.position);
            Vector3 newpos = prev_part.position;
            //newpos.y = BodyParts[0].position.y;
            float T = Time.deltaTime * dis / minddistance * speed;
            if (T >0.5f) {
                T = 0.5f;
            }
            cur_part.position = Vector2.Lerp(cur_part.position, newpos, T);
            //cur_part.LookAt(prev_part);
            cur_part.rotation = Quaternion.Slerp(cur_part.rotation, prev_part.rotation, 0.1f);
        }
    }
    public void FourDirHand() {
        transform.Translate(dir * Time.smoothDeltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector2.right;
            // transform.Rotate(0, 0, -90);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector2.down;  
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector2.left;
        } 
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector2.up;
        }
    }
    public void FreeHand() {

        Quaternion rot = transform.rotation;
        float z = rot.eulerAngles.z;
        z -= Input.GetAxis("Horizontal") * rotationspeed * Time.deltaTime;
        rot = Quaternion.Euler(0, 0, z);
        transform.rotation = rot;

        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);

        pos += rot * velocity;
        transform.position = pos;
        
    }
    public void Shrink(float time) {
        // TODO: Z, the hand is special and shouldn't be in this list
        Transform hand = BodyParts[0]; 

        if(footprints.footprints.Count == 0)
        {
            hand.position = Vector2.Lerp(hand.position, handPos.position, time);
            hand.rotation = Quaternion.Slerp(hand.rotation, handPos.rotation, time);
        }
        else
        {
            Transform target = footprints.footprints[footprints.footprints.Count - 1].transform;
            hand.position = Vector2.Lerp(hand.position, target.position, time);
            hand.rotation = Quaternion.Slerp(hand.rotation, target.rotation, time);

            if ((transform.position - target.position).sqrMagnitude <= 0.1f)
            {
                footprints.footprints.Remove(footprints.footprints[footprints.footprints.Count - 1]);
            }
        }

        // TODO: Z, adjust code so that the other body parts follow along

        // FIXME: Z, if the player presses E, R, and then E again, the 
        // footprint tracing forces the arm straight back to the player (see 
        // Player.cs#HandleArm() code that clears the footprint list)!
        //
        // Therefore, only allow the player to activate this ability if it's 
        // not already activated and if it's back at the default position. One 
        // way to know if it's back at the default position is if the 
        // footprints list is empty again, perhaps?
        
        /*
        foreach (Transform node in BodyParts)
        {
            node.position = Vector2.Lerp(node.position, handPos.position, time);
            //cur_part.LookAt(prev_part);
            node.rotation = Quaternion.Slerp(node.rotation, handPos.rotation, time);


        }
        */

/*
        for (int i = BodyParts.Count - 1; i > 0 ; i--)
            {
                cur_part = BodyParts[i];
                prev_part = BodyParts[i - 1];

                dis = Vector3.Distance(prev_part.position, cur_part.position);
                Vector3 newpos = prev_part.position;
                //newpos.y = BodyParts[0].position.y;
                float T = Time.deltaTime * dis / minddistance * speed;
                if (T > 0.5f)
                {
                    T = 0.5f;
                }
                cur_part.position = Vector2.Lerp(cur_part.position, newpos, T);
                //cur_part.LookAt(prev_part);
                cur_part.rotation = Quaternion.Slerp(cur_part.rotation, prev_part.rotation, 0.1f);
            }*/
        
    }

    public void Leap(float time)
    {
        foreach (Transform node in BodyParts)
        {
            node.position = Vector2.Lerp(node.position, BodyParts[0].transform.position, time);
            //cur_part.LookAt(prev_part);
            node.rotation = Quaternion.Slerp(node.rotation, BodyParts[0].transform.rotation, time);

        }
    }

}
