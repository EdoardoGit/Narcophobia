using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviourState { none, wander}

public class GhostController : MonoBehaviour
{
    public AudioClip step;
    public AudioClip knock;
    public AudioClip attack;
    public BehaviourState initialState;
    [Header("Wander Settings")]
    public Bounds boundBox;
    public Bounds bed;
    public Bounds mobile1;
    public Bounds mobile2;


    private NavMeshAgent agent;
    private BehaviourState currentState = BehaviourState.none;
    private Vector3 targetPos;
    private AudioSource noise;
    private float timeStamp;

    private bool sound = true;

    private void Awake()
    {
        noise = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        timeStamp = Time.time + 7f;
    }

    void Start()
    {
        SetState(initialState);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == BehaviourState.wander)
        {
            float targetDistance = Vector3.Distance(targetPos, transform.position);
            if(targetDistance<= agent.stoppingDistance)
            {
                /*if(timeStamp <= Time.time)
                {
                    sound = true;
                    if (Vector3.Distance(transform.position, bed.ClosestPoint(transform.position)) < 2f)
                    {
                        noise.clip = attack;
                        noise.Play();
                        sound = false;
                    }

                    else
                    {
                        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("wall"))
                        {
                            if (Vector3.Distance(transform.position, wall.transform.position) < 6f)
                            {
                                noise.clip = knock;
                                noise.Play();
                                sound = false;
                            }
                        }
                    }
                    if (sound)
                    {
                        noise.clip = step;
                        noise.Play();
                    }
                    timeStamp = Time.time + 7f;
                }*/
                findNewWanderTarget();
            }
        }
    }

    void SetState(BehaviourState s)
    {
        if (currentState != s)
        {
            currentState = s;
            if (currentState == BehaviourState.wander)
            {
                findNewWanderTarget();
            }
        }
    }

    void findNewWanderTarget()
    {
        targetPos = getRandomPoint();
        while (bed.Contains(targetPos) || mobile1.Contains(targetPos) || mobile2.Contains(targetPos))
            targetPos = getRandomPoint();
        agent.SetDestination(targetPos);
        agent.isStopped = false;
    }

    Vector3 getRandomPoint()
    {
        float randomX = Random.Range(-boundBox.extents.x + agent.radius, boundBox.extents.x - agent.radius);
        float randomZ = Random.Range(-boundBox.extents.z + agent.radius, boundBox.extents.z - agent.radius);
        return new Vector3(randomX, transform.position.y, randomZ);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boundBox.center, boundBox.size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bed.center, bed.size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(mobile1.center, mobile1.size);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(mobile2.center, mobile2.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPos, 0.2f);
    }
}
