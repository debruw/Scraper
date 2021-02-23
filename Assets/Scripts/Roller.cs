using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Roller : MonoBehaviour
{
    public float TurnSpeed;
    float defaultRollerSpeed;
    public float currentRadial;
    public GameObject[] Spawns;
    int currentSpawn;

    private void Start()
    {
        defaultRollerSpeed = TurnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver || !GameManager.Instance.isGameStarted)
        {
            return;
        }
        transform.Rotate(new Vector3(-TurnSpeed * Time.deltaTime, 0, 0), Space.World);
        currentRadial += TurnSpeed * Time.deltaTime;
        if (currentRadial >= 180)
        {
            if (currentSpawn < Spawns.Length)
            {
                currentRadial = 0;
                Spawns[currentSpawn].transform.parent = transform;
                Spawns[currentSpawn].SetActive(true);
                currentSpawn++;
            }
        }
    }

    public void RollerCrash()
    {
        //Stop roller and shake and get roller back
        TurnSpeed = 0;
        StartCoroutine(RotateMe(Vector3.right * -5, 1));
        GameManager.Instance.isCrushed = true;
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }

    public void RollerContinue()
    {
        TurnSpeed = defaultRollerSpeed;
    }
}
