using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    public float TurnSpeed;
    public float currentRadial;
    public GameObject[] Spawns;
    int currentSpawn;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        transform.Rotate(new Vector3(-TurnSpeed * Time.deltaTime, 0, 0), Space.World);
        currentRadial += TurnSpeed * Time.deltaTime;
        if (currentRadial >= 270 && currentSpawn < Spawns.Length)
        {
            currentRadial = 0;            
            Spawns[currentSpawn].transform.parent = transform;
            Spawns[currentSpawn].SetActive(true);
            currentSpawn++;
        }
    }
}
