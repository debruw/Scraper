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
    public ParticleSystem particle;

    private void Start()
    {
        defaultColor = GetComponent<MeshRenderer>().materials[1].color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Driplet"))
        {
            if (other.GetComponent<Driplet>().myDripletColor == myBoxColor)
            {
                CollectedDriplets.Add(other.gameObject);
                other.gameObject.transform.parent = newParent;
                if (CollectedDriplets.Count >= TargetCount && !isBoxFull)
                {
                    isBoxFull = true;
                    //Close the box
                    myAnimator.SetTrigger("CloseBox");
                    particle.Play();
                    //Check game win
                    GameManager.Instance.CheckGameWin();
                }
            }
            else
            {
                SoundManager.Instance.playSound(SoundManager.GameSounds.WrongDriplet);
                wrongDripletsCount++;
                StartCoroutine(BlinkColor());
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

    public Color defaultColor;
    IEnumerator BlinkColor()
    {
        GetComponent<MeshRenderer>().materials[1].color = Color.red;
        yield return new WaitForSeconds(.1f);
        GetComponent<MeshRenderer>().materials[1].color = defaultColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Driplet") && CollectedDriplets.Contains(other.gameObject))
        {
            CollectedDriplets.Remove(other.gameObject);
        }
    }
}
