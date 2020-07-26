using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Raytest : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hitInfo;
    // Use this for initialization
    void Start()
    {
        //层级位移运算
        int layerMask = (1 << 8);
        Debug.Log(layerMask);
        layerMask = (1 << 9);
        Debug.Log(layerMask);
        layerMask = (1 << 8) | (1 << 9);
        Debug.Log(layerMask);
        layerMask = LayerMask.GetMask(new string[] { "Lxm", "Cjy" });//获取图层名为Lxm Cjy的图层号
        Debug.Log(layerMask);
        //结果第8位，第9位
        //Ray(Vector3 origin, Vector3 direction);
        //Ray射线是结构体，有两个参数，origin原点，direction方向
        ray = new Ray(transform.position, Vector3.forward);
        //发射射线，物理系统的光线投射，有16种重载
        //Physics.Raycast(ray);
        //射线是没有网格的，是看不到，是物理系统的一部分，是数学计算得来的
        if (Physics.Raycast(ray))
        {
            Debug.Log("射线已射中物体");
        }
        //maxDistance射线的最大距离就是float的最大值，可以手动设置
        //Raycast(Ray ray, float maxDistance);
        //hitinfo:这条射线所碰撞物体的相关信息，储存碰撞物体的信息；
        //Raycast(Ray ray, out RaycastHit hitInfo);
        if (Physics.Raycast(ray, out hitInfo, 100, layerMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hitInfo.transform.name);
            Debug.DrawLine(transform.position, hitInfo.point, Color.red, 100);
        }
        //层级遮罩，设置只检查哪一个层级的游戏物体，默认全部检测
        //层级实质相当于枚举项，值是2的幂次方，序号只是层级的索引
        //Raycast(Ray ray, float maxDistance, int layerMask);
        //如果检测第八层，需要填写256
        //if (Physics.Raycast(ray, GetComponent<Collider>().gameObject.tag, 256))
        //{
        //    Debug.Log(hitInfo.transform.name);
        //    Debug.DrawLine(transform.position, hitInfo.point, Color.red, 100);
        //}
        //Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask);
        //Ray ray是射线；RaycastHit hitInfo是碰撞信息；float distance是碰撞距离；int layerMask是碰撞的层
        //RaycastAll穿过多个物体
        RaycastHit[] hitinfos = Physics.RaycastAll(ray, 100);
        foreach (var item in hitinfos)
        {
            Debug.Log(item.transform.name);
        }
        //不仅发射摄像，还可以发射形状物体
        //Physics.BoxCast();
        //Physics.CapsuleCast();
        //如何检测一个人在地面上而不是在空中（二段跳判定）
        //1、在人物半身添加一个向下的射线，射线刚好接触地面
        //2、在人物头顶添加一个投射盒
        //特点：
        //射线是一帧判定
        //射线没有运行时间
        //位运算符（二进制的加减乘除）
        //&与运算：有零即为零，全一才是一
        //1001010
        //1000111  &
        //-----------
        //1000010
        //|或运算：有一就是一，全零才是零
        //1001010
        //1110001   |
        //-----------
        //1111011
        //~非运算符：一变零，零变一
        //1101010  ~
        //-----------
        //1111011
        //^异或运算：相同即为零，不同即为一
        //1010001
        //1100010   ^
        //-----------
        //0110011
        //<<向左移：所有二进制位向高位进位，空出来的位补零
        //1010<<1
        //-----------
        //10100
        //>>向右移：所有二进制位向低位进位，移出位被丢弃，左边移出的空位或者一律补0
        //1011>>1
        //-----------
        //0101
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, hitInfo.point, Color.red, 0);
    }
}