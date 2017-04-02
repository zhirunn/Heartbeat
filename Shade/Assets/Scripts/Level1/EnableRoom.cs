﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRoom : MonoBehaviour
{
    public GameObject doors;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") { return; }

        //Disable doors
        foreach (SpriteRenderer renderer in doors.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = true;
        }

        //Turn off seeing
        foreach (Transform t in doors.GetComponentInChildren<Transform>())
        {
            t.gameObject.tag = "Untagged";
        }
    }
}