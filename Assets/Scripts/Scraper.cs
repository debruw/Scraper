using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scraper : MonoBehaviour
{
    Touch touch;
    Vector2 firstPressPos;
    private Vector3 translation;
    public float Xspeed = 25f, limit;
    public Camera cam;
    public Roller roller;
    public Transform raycastStart;
    public GameObject Spark1, Spark2;

    private void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            if (Spark1.activeSelf)
            {
                Spark1.SetActive(false);
                Spark2.SetActive(false);
            }
            return;
        }
#if UNITY_EDITOR
        if (!Spark1.activeSelf)
        {
            Spark1.SetActive(true);
            Spark2.SetActive(true);
        }
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
        timer += Time.deltaTime;
        if (timer >= .25f)
        {
            zooming = false;
            timer = .25f;
        }
        if (!zooming)
        {
            if (cam.fieldOfView > 68)
            {
                cam.fieldOfView -= 15 * Time.deltaTime;
            }
            else if (cam.fieldOfView <= 68)
            {
                cam.fieldOfView = 68;
            }
        }
    }

    float raycastTimer;
    RaycastHit hit;
    private void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        raycastTimer += Time.fixedDeltaTime;

        if (GameManager.Instance.isCrushed && raycastTimer > .5f)
        {
            raycastTimer = 0;
            
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raycastStart.position, Vector3.up, out hit, 5))
            {
                //Debug.Log("Did Hit");
            }
            else
            {
                GameManager.Instance.isCrushed = false;
                roller.RollerContinue();
            }
        }
    }

    float timer;
    bool zooming;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Driplet"))
        {
            collision.gameObject.transform.parent = null;
            collision.gameObject.GetComponent<Rigidbody>().useGravity = true;
            collision.gameObject.GetComponent<Animator>().SetTrigger("ScaleDown");
            zooming = true;
            if (cam.fieldOfView < 75)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 75, Time.deltaTime);
            }
            else if (cam.fieldOfView >= 75)
            {
                cam.fieldOfView = 75;
            }
            timer = 0;
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
        else if (other.CompareTag("Obstacle"))
        {
            roller.RollerCrash();
            other.GetComponent<Obstacle>().ReleaseChilds();
        }
        else if (other.CompareTag("FinishLine"))
        {
            roller.RollerCrash();
            GameManager.Instance.isGameOver = true;
            StartCoroutine(GameManager.Instance.WaitAndGameLose());
        }
    }
}
