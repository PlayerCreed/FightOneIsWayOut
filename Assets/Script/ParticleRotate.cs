using UnityEngine;
using System.Collections;

public class ParticleRotate : MonoBehaviour
{
    public class particleClass
    {
        public float radiu = 0.0f;
        public float angle = 0.0f;
        public particleClass(float radiu_, float angle_)
        {
            radiu = radiu_;
            angle = angle_;
        }
    }

    //创建粒子系统，粒子数组，粒子数目，声明粒子环的半径
    public ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particlesArray;
    private particleClass[] particleAttr; //粒子属性数组
    public int particleNum = 10000;
    public float minRadius = 5.0f;
    public float maxRadius = 10.0f;
    public int Part = 2;
    public float speed = 0.1f;

    void Start()
    {
        particleAttr = new particleClass[particleNum];
        particlesArray = new ParticleSystem.Particle[particleNum];
        particleSystem.maxParticles = particleNum;
        particleSystem.Emit(particleNum);
        particleSystem.GetParticles(particlesArray);
        for (int i = 0; i < particleNum; i++)
        {   //相应初始化操作，为每个粒子设置半径，角度
            //产生一个随机角度
            float randomAngle = Random.Range(0.0f, 360.0f);

            // 随机产生每个粒子距离中心的半径，同时粒子要集中在平均半径附近  
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float randomRadius = Random.Range(minRadius * minRate, maxRadius * maxRate);

            ////一种待完善的产生随机粒子的办法 box-muller
            //float sita = 2 * Mathf.PI * Random.Range(0, 1);
            //float R = Mathf.Sqrt(-2 * Mathf.Log(0.7f));
            //float Z = R * Mathf.Cos(sita);
            //float randomRadius = (minRadius + maxRadius) / 2 + Z * 2.5f;
            //print(randomRadius);
            //Debug.Log("r = " + R);
            //Debug.Log(randomRadius);

            //粒子属性设置
            particleAttr[i] = new particleClass(randomRadius, randomAngle);
            particlesArray[i].position = new Vector3(randomRadius * Mathf.Cos(randomAngle), randomRadius * Mathf.Sin(randomAngle), 0.0f);
        }
        //设置粒子
        particleSystem.SetParticles(particlesArray, particleNum);
    }


    void Update()
    {
        //设置为两部分的粒子，一部分顺时针，一部分逆时针。
        for (int i = 0; i < particleNum; i++)
        {
            if (i % 2 == 0) particleAttr[i].angle += (i % Part + 1) * speed;
            else particleAttr[i].angle -= (i % Part + 1) * speed;

            //根据新的角度重新设置位置
            particleAttr[i].angle = particleAttr[i].angle % 360;
            float rad = particleAttr[i].angle / 180 * Mathf.PI;
            particlesArray[i].position = new Vector3(particleAttr[i].radiu * Mathf.Cos(rad), particleAttr[i].radiu * Mathf.Sin(rad), 0f);
        }
        particleSystem.SetParticles(particlesArray, particleNum);
    }
}
