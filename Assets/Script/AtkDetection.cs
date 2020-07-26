using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkDetection : MonoBehaviour
{
    public float damageModifier;
    public GameObject Enemy;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Enemy.GetComponentInParent<Enemy>().health -= (collision.transform.gameObject.GetComponent<BulletScript>().atk * damageModifier);
        //if(collision.gameObject.tag=="Bullet")
        Debug.Log(collision.gameObject.name);
    }
}
