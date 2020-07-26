using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FledArea : MonoBehaviour
{
    public GameObject gameCtr;
    // Start is called before the first frame update
    void Start()
    {
        gameCtr = GameObject.Find("SystemCanvas");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag=="Player")
        {
            gameCtr.GetComponent<GameCtr>().winFalg = true;
        }
    }
}
