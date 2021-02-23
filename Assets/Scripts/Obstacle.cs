using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject[] childs;
    bool isChildsReleased;
    public ParticleSystem[] hitParticles;

    public void ReleaseChilds()
    {
        foreach (ParticleSystem item in hitParticles)
        {
            item.Play();
        }

        if (!isChildsReleased)
        {
            foreach (GameObject item in childs)
            {
                item.transform.parent = null;
                item.GetComponent<Collider>().isTrigger = false;
                item.GetComponent<Rigidbody>().useGravity = true;
            }
            isChildsReleased = true;
        }
    }
}
