using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    CapsuleCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        Bounds bounds = _collider.bounds;
        float radius = _collider.radius;
        float halfHeight = _collider.height * 0.5f - radius;
        Vector3 point1 = bounds.center;
        point1.y += halfHeight;
        Vector3 point2 = bounds.center;
        point2.y -= halfHeight;
        RaycastHit[] _raycastHit=new RaycastHit[8];
        Physics.CapsuleCastNonAlloc(point1,point2,radius,Vector3.forward, _raycastHit,10,~0,QueryTriggerInteraction.Ignore);
        foreach (var item in _raycastHit)
        {
            if (item.transform == transform)
            {
                continue;
            }
            //判断刚体不等于空，防止与地面碰撞
            //attachedRigidbody,碰撞器附加的刚体,如果碰撞器没有附加刚体返回null。
            if (item.rigidbody != null)
            {
                Debug.Log(item.transform.name);
            }
        }
        for (int i=0; i < _raycastHit.Length; i++)
        {
            _raycastHit[i] = new RaycastHit();
        }
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + radius), Color.red);
    }
}
