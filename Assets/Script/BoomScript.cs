using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoomScript : MonoBehaviour
{

    public float forceFactor = 10;


    private void OnCollisionEnter(Collision collision)
    {
        //生成橡胶球，检测碰撞
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);
        //剔除自己和没有加刚体的
        foreach (var item in colliders)
        {
            //剔除自己
            if (item.transform == transform)
            {
                continue;
            }
            //判断刚体不等于空，防止与地面碰撞
            //attachedRigidbody,碰撞器附加的刚体,如果碰撞器没有附加刚体返回null。
            if (item.attachedRigidbody != null)
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                float forceValue = 1 / distance * forceFactor;
                Vector3 dir = (item.transform.position - transform.position).normalized;
                Vector3 force = dir * forceValue;
                item.attachedRigidbody.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}