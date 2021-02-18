using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<GameObject> CollectedDriplets;
    public Driplet.MyColor myBoxColor;
    public Transform newParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Driplet"))
        {
            if (other.GetComponent<Driplet>().myDripletColor == myBoxColor)
            {
                CollectedDriplets.Add(other.gameObject);
                other.gameObject.transform.parent = newParent;
            }
            else
            {

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Driplet") && CollectedDriplets.Contains(other.gameObject))
        {
            CollectedDriplets.Remove(other.gameObject);
        }
    }
}
