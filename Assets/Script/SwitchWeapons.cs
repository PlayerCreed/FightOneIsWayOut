using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapons : MonoBehaviour
{
    public GameObject deputyWeapon;
    public float closeAtkScope;
    public float gentlyCloseAtk;
    public float forcedCloseAtk;
    private int layerMask;
    

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask(new string[] { "background" });
    }



    public void ForcedCloseAttack()
    {
        RaycastHit[] hitInfo;
        hitInfo = Physics.RaycastAll(transform.position, transform.forward, closeAtkScope, ~layerMask);
        foreach (var item in hitInfo)
        {
            if (item.transform == transform)
                break;
            if (item.transform.tag == "Enemy")
            {
                item.transform.GetComponent<Enemy>().health -= forcedCloseAtk;
            }
        }
    }

    public void GentlyCloseAttack()
    {
        RaycastHit[] hitInfo;
        hitInfo = Physics.RaycastAll(transform.position, transform.forward, closeAtkScope, ~layerMask);
        foreach (var item in hitInfo)
        {
            if (item.transform == transform)
                break;
            if (item.transform.tag == "Enemy")
            {
                item.transform.GetComponent<Enemy>().health -= gentlyCloseAtk;
            }
        }
    }

    public void Switch()
    {
        GameObject ob= Instantiate(deputyWeapon,transform);
        ob.transform.parent = transform.parent.gameObject.transform.parent;
        Destroy(transform.parent.gameObject);     
    }
}
