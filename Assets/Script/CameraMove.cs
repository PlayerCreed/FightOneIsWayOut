using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;//获取旋转目标
    public float speed;

    private void Update()
    {
        camerarotate();
    }


    private void camerarotate() //摄像机围绕目标旋转操作
    {
        transform.RotateAround(target.transform.position, Vector3.up, speed); //摄像机围绕目标旋转
        var mouse_x = Input.GetAxis("Mouse X");//获取鼠标X轴移动
        var mouse_y = -Input.GetAxis("Mouse Y");//获取鼠标Y轴移动
     
        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.RotateAround(target.transform.position, Vector3.up, mouse_x * 5);
            transform.RotateAround(target.transform.position, transform.right, mouse_y * 5);
        }
    }

}
