using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BehaviourState { none, wander}

public class GhostController : MonoBehaviour
{
    public float timeGap;
    public BehaviourState initialState;
    [Header("Wander Settings")]
    public Bounds[] Rooms = new Bounds[0];
    public Bounds[] Objects = new Bounds[0];
    /*public Bounds Room1;
    public Bounds Room2;
    public Bounds bed;
    public Bounds mobile1;
    public Bounds mobile2;*/

    private NavMeshAgent agent;
    private BehaviourState currentState = BehaviourState.none;
    private Vector3 targetPos;
    private bool changeRoom = false;
    private int changeCount = 0;

    //Wwise
    [Header("Wwise Events")]
    public AK.Wwise.Event myFootstep;

    private bool footstepIsPlaying = false;
    private float lastStepTime = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lastStepTime = Time.time;
    }

    void Start()
    {
        SetState(initialState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentState == BehaviourState.wander)
        {
            float targetDistance = Vector3.Distance(targetPos, transform.position);
            if(targetDistance<= agent.stoppingDistance)
            {
                findNewWanderTarget();
            }
            if (agent.velocity.sqrMagnitude > 2f && !footstepIsPlaying)
            {
                myFootstep.Post(gameObject);
                lastStepTime = Time.time;
                footstepIsPlaying = true;
            }
            else if (agent.velocity.sqrMagnitude > 1f)
                if(Time.time-lastStepTime>timeGap/agent.velocity.sqrMagnitude*Time.deltaTime)
                {
                    footstepIsPlaying = false;
                }
        }
        //Debug.Log(agent.velocity.sqrMagnitude);
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
        bool contained;
        do
        {
            contained = false;
            foreach (Bounds obj in Objects)
                if (obj.Contains(targetPos))
                    contained = true;
            if (contained)
                targetPos = getRandomPoint();
        } while (contained);
        changeCount++;
        if (changeCount % 5 == 0)
            switchRoom();
        agent.SetDestination(targetPos);
        agent.isStopped = false;
    }

    Vector3 getRandomPoint()
    {
        if (!changeRoom)
        {
            float randomX = Random.Range(Rooms[0].center.x-Rooms[0].extents.x + agent.radius,Rooms[0].center.x + Rooms[0].extents.x - agent.radius);
            float randomZ = Random.Range(Rooms[0].center.z - Rooms[0].extents.z + agent.radius, Rooms[0].center.z + Rooms[0].extents.z - agent.radius);
            Debug.Log("Room1:" + randomX + "," + transform.position.y + "," + randomZ);
            return new Vector3(randomX, transform.position.y, randomZ);
        }
        else
        {
            float randomX = Random.Range(Rooms[1].center.x - Rooms[1].extents.x + agent.radius, Rooms[1].center.x + Rooms[1].extents.x - agent.radius);
            float randomZ = Random.Range(Rooms[1].center.z - Rooms[1].extents.z + agent.radius, Rooms[1].center.z + Rooms[1].extents.z - agent.radius);
            Debug.Log("Room2:" + randomX + "," + transform.position.y + "," + randomZ);
            return new Vector3(randomX, transform.position.y, randomZ);
        }
    }

    private void switchRoom()
    {
        if (changeRoom)
            changeRoom = false;
        else
            changeRoom = true;
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Room1.center, Room1.size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Room2.center, Room2.size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bed.center, bed.size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(mobile1.center, mobile1.size);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(mobile2.center, mobile2.size);
        Gizmos.color = Color.blue;*/
        foreach(Bounds room in Rooms)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(room.center, room.size);
        }
        foreach(Bounds obj in Objects)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(obj.center, obj.size);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPos, 0.2f);
    }
}
