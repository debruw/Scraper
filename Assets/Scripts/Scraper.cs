﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scraper : MonoBehaviour
{
    Touch touch;
    Vector2 firstPressPos;
    private Vector3 translation;
    public float Xspeed = 25f, limit;

    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButton(0))
        {
            translation = new Vector3(Input.GetAxis("Mouse X"), 0, 0) * Time.deltaTime * Xspeed;

            transform.Translate(translation, Space.World);
            transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -limit, limit), transform.localPosition.y, transform.localPosition.z);
        }

#elif UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x + touch.deltaPosition.x * 0.01f, -1.5f, 1.5f), transform.localPosition.y, transform.localPosition.z);
            }
            else if (touch.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }

#endif
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Driplet"))
        {
            collision.gameObject.transform.parent = null;
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
            collision.gameObject.GetComponent<Animator>().SetTrigger("ScaleDown");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Driplet"))
        {
            other.transform.parent = null;
            other.GetComponent<Collider>().isTrigger = false;
            other.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}