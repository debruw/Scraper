using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public List<GameObject> CollectedDriplets;
    public Driplet.MyColor myBoxColor;
    public Transform newParent;
    public int TargetCount;
    public bool isBoxFull;
    int wrongDripletsCount;
    public int maxWrongDripletCount;
    public Animator myAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Driplet"))
        {
            if (other.GetComponent<Driplet>().myDripletColor == myBoxColor)
            {
                CollectedDriplets.Add(other.gameObject);
                other.gameObject.transform.parent = newParent;
                if (CollectedDriplets.Count >= TargetCount)
                {
                    isBoxFull = true;
                    //Close the box
                    myAnimator.SetTrigger("CloseBox");
                    //Check game win
                    GameManager.Instance.CheckGameWin();
                }
            }
            else
            {
                wrongDripletsCount++;
                if (wrongDripletsCount >= maxWrongDripletCount)
                {
                    // GameOver
                    Debug.Log("Game Lose");
                    StartCoroutine(GameManager.Instance.WaitAndGameLose());
                    GameManager.Instance.isGameOver = true;
                }
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
