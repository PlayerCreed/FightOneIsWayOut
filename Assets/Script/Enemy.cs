using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Scout Setting")]
    public Transform[] destination;
    public bool loop = false;
    public int patrolStopTime;
    public float runSpeed;
    private bool loopUp = false;
    private int desNum = 0;
    private NavMeshAgent nav;

    [Header("Audio Setting")]
    public AudioSource _audioSource;
    public AudioClip walkingSound;
    public AudioClip runningSound;

    [Header("Attack Setting")]
    public int rayLingth;
    public int attLingth;
    public float health;
    public float atk;
    private bool attack = false;
    private Transform attTarget;
    private bool death = false;

    [Header("Explosion Setting")]
    public float exPower;
    public float exRadius;
    public Transform explosionPrefab;
    public float grenadeTimer;

    private Animator animator;
    private Rigidbody _rigidbody;

    private RaycastHit[] hitInfo;
    private RaycastHit[] attInfo;

    private int layerMask; 

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        layerMask = LayerMask.GetMask(new string[] { "background" });
    }
    void Start()
    {
        _audioSource.clip = walkingSound;
        nav.destination = destination[desNum].position;       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!death)
        {
            NavMove();
            Attack();
        }      
    }

    private void Update()
    {
        if (!death)
        {
            Scout();

            //if (1.7f <= Mathf.Abs(nav.velocity.x)
            //    || 1.7f <= Mathf.Abs(nav.velocity.z))
            //    animator.SetBool("run", true);
            //else if (Mathf.Abs(nav.velocity.x) > 0.01f
            //    || Mathf.Abs(nav.velocity.z) > 0.01f)
            //{
            //    animator.SetBool("run", false);
            //    animator.SetBool("walk", true);
            //}
            //else
            //    animator.SetBool("walk", false);
            if (Mathf.Abs(nav.velocity.x) > 0.01f
                || Mathf.Abs(nav.velocity.z) > 0.01f)
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
                animator.SetBool("walk", true);
            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Pause();
                }
                animator.SetBool("walk", false);
            }

            if (health <= 0)
                Death();
        }        
    }


    private void Attack()
    {
        if (attack)
        {
            animator.Play("Standing_Run_Forward");
            _audioSource.clip = runningSound;
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            nav.destination = attTarget.position;
            Vector3 speed = nav.velocity.normalized * runSpeed;
            _rigidbody.AddForce(speed- _rigidbody.velocity,ForceMode.Force);
            attInfo = Physics.RaycastAll(origin, new Vector3(transform.forward.x, 0, transform.forward.z), attLingth, ~layerMask);
            foreach (var item in attInfo)
            {
                if (item.transform == transform)
                    break;
                StartCoroutine(ExplosionTimer());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet"&&!death)
        {
            animator.Play("Standing_React_Large_From_Front");
        }
    }

    private IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(grenadeTimer);

        RaycastHit checkGround;
        if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
        {
            //Instantiate metal explosion prefab on ground
            Instantiate(explosionPrefab, checkGround.point,
                Quaternion.FromToRotation(Vector3.forward, checkGround.normal));
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, exRadius);
        foreach (var item in colliders)
        {
            Rigidbody rd=item.GetComponent<Rigidbody>();
            if (rd != null)
                rd.AddExplosionForce(exPower * 5, transform.position, exRadius, 0.3f);
            if (item.gameObject.tag == "Player")
            {
                item.GetComponent<Player>().health -= atk;
            }
        }
        Destroy(gameObject);
    }

    private void NavMove()
    {
        if (destination != null && attack == false)
        {
            if (loop && desNum == destination.Length - 1)
            {
                loopUp = true;
            }
            if (loop && desNum == 0)
            {
                loopUp = false;
            }
            if (transform.position.x == destination[desNum].position.x && transform.position.z == destination[desNum].position.z)
            {
                if (loopUp && desNum > 0)
                    desNum--;
                else if (!loopUp && desNum < destination.Length - 1)
                    desNum++;
                StartCoroutine(Patrol());
            }
        }
    }

    private IEnumerator Patrol()
    {
        yield return new WaitForSeconds(patrolStopTime);
        
        nav.destination = destination[desNum].position;
    }

    private void Scout()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y+1f, transform.position.z);
        
        for (int i = 0; i < 3; i++)
        {
            Debug.DrawRay(origin,new Vector3( (transform.forward.x+transform.right.x)-transform.right.x*i, 0, (transform.forward.z + transform.right.z) - transform.right.z * i), Color.red,0);
            hitInfo = Physics.RaycastAll(origin, new Vector3((transform.forward.x + transform.right.x) - transform.right.x * i, 0, (transform.forward.z + transform.right.z) - transform.right.z * i), rayLingth, ~layerMask);
            foreach (var item in hitInfo)
            {
                if (item.transform == transform)
                    break;
                if (item.transform.tag == "Player")
                {
                    attack = true;
                    attTarget = item.transform;
                }
                else
                {
                    //attack = false;
                }
            }
        }
        for (int i = 0; i < hitInfo.Length; i++)
            hitInfo[i] = new RaycastHit();
    }

    private void Death()
    {
        death = true;
        nav.destination = transform.position;
        animator.Play("Standing_React_Death_Forward");
    }

    public void _Destroy()
    {
        Destroy(gameObject);
    }
}
