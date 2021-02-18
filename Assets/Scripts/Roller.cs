using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    public float TurnSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(-TurnSpeed * Time.deltaTime, 0, 0), Space.World);
    }
}
