using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    private GameObject gameCtr;
    public float detectDistance;
    private int layerMask;
    private RaycastHit[] raycastHits;

    // Start is called before the first frame update
    void Start()
    {
        gameCtr = GameObject.Find("SystemCanvas");
    }

    // Update is called once per frame
    void Update()
    {        
        if (gameCtr.GetComponent<GameCtr>().healthInfiniteFlag)
        {
            health = 100;
        }
        if (health <= 0)
        {
            Death();
            health = 0;
        }
    }

    //void ItemDetect()
    //{
    //    Vector3 origin = transform.position;
    //    raycastHits = Physics.RaycastAll(origin, transform.forward, detectDistance, ~layerMask, QueryTriggerInteraction.Ignore);
    //    Debug.DrawRay(origin, transform.forward * detectDistance, Color.red);
    //    foreach (var hit in raycastHits)
    //    {
    //        if (hit.collider!=null&&hit.transform!=transform)
    //        {
    //            if (hit.transform.tag == "Caisson")
    //                gameCtr.GetComponent<GameCtr>().caissonFlag = true;
    //            else
    //                gameCtr.GetComponent<GameCtr>().caissonFlag = false;
    //        }
    //    }
    //}

    private void Death()
    {
        gameCtr.GetComponent<GameCtr>().deathFlag = true;
    }
}
