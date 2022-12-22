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
    private Bounds Room = new Bounds();
    public float range;
    private GameObject bed;

    private NavMeshAgent agent;
    private BehaviourState currentState = BehaviourState.none;
    private Vector3 targetPos;
    private Vector3 navPosition;
    private Vector3 startingPosition;
    private GameObject container;
    private bool endSim = false;
    private GameObject presence;
    private float presenceMaxDistanceDx;
    private float presenceMaxDistanceSx;
    private Vector3 presenceOGPosDx;
    private Vector3 presenceOGPosSx;
    private bool easterEgg = false;
    private float rngTimeGap = 0f;

    //Wwise
    [Header("Wwise Events")]
    public AK.Wwise.Event myFootstep;

    private bool footstepIsPlaying = false;
    private float lastStepTime = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        container = GameObject.Find("Container");
        presence = container.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        lastStepTime = Time.time;
        
    }

    void Start()
    {
        SetupEasterEgg();
        GetGhostStart();
        SetupRoomBound();
        SelectFootstep();
        presence.transform.GetChild(0).GetComponent<AmbienceRoom>().StartSound(easterEgg);
        presence.transform.GetChild(1).GetComponent<AmbienceRoom>().StartSound(easterEgg);
        SetState(initialState);
    }

    void SelectFootstep()
    {
        switch (RoomData.Instance.pavMat)
        {
            case 1:
                AkSoundEngine.SetSwitch("Footsteps", "Parque", this.gameObject);
                break;
            default:
                AkSoundEngine.SetSwitch("Footsteps", "Tile", this.gameObject);
                break;
        }
    }

    void SetupEasterEgg()
    {
        int k = 0;
        foreach (Transform grid in container.transform)
        {
            if (grid.transform.childCount != 0)
            {
                k++;
            }
        }
        if (k == 1)
            easterEgg = true;
    }

    public void SetupRoomBound()
    {
        Room.center = new Vector3(-8.5f, 1, 0);
        Room.extents = new Vector3(RoomData.Instance.dimX*1.5f-0.5f, 1f, RoomData.Instance.dimZ*1.5f-0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "rug")
        {
            AkSoundEngine.SetSwitch("Footsteps", "Carpet", this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "rug")
        {
            SelectFootstep();
        }
    }

    void FixedUpdate()
    {
        if(currentState == BehaviourState.wander)
        {
            float targetDistance = Vector3.Distance(targetPos, transform.position);
            if(targetDistance<= agent.stoppingDistance && !RoomData.Instance.timerOut && !endSim)
            {
                findNewWanderTarget();
            }

            float distanceDx = Vector3.Distance(this.gameObject.transform.position, presenceOGPosDx);
            float distanceSx = Vector3.Distance(this.gameObject.transform.position, presenceOGPosSx);

            distanceDx = distanceDx / presenceMaxDistanceDx;
            distanceSx = distanceSx / presenceMaxDistanceSx;

            presence.transform.GetChild(0).position = new Vector3(presenceOGPosDx.x - (1f-1f* distanceDx),presenceOGPosDx.y,presenceOGPosDx.z);
            presence.transform.GetChild(1).position = new Vector3(presenceOGPosSx.x + (1f-1f * distanceSx), presenceOGPosSx.y, presenceOGPosSx.z);
            if (agent.velocity.sqrMagnitude > 0.8f && !footstepIsPlaying)
            {
                myFootstep.Post(gameObject);
                lastStepTime = Time.time;
                footstepIsPlaying = true;
            }
            else if (agent.velocity.sqrMagnitude > 0.4f)
                if(Time.time-lastStepTime>(timeGap+rngTimeGap)/agent.velocity.sqrMagnitude*Time.deltaTime)
                {
                    rngTimeGap = Random.Range(-200, 50);
                    footstepIsPlaying = false;
                }
        }

        if (agent.velocity.sqrMagnitude == 0 && !endSim && !RoomData.Instance.timerOut)
        {
            findNewWanderTarget();
        }
    }

    void GetGhostStart()
    {
        startingPosition = gameObject.transform.position;
        presenceOGPosDx = presence.transform.GetChild(0).position;
        presenceOGPosSx = presence.transform.GetChild(1).position;
        presenceMaxDistanceDx = Vector3.Distance(startingPosition, presenceOGPosDx);
        presenceMaxDistanceSx = Vector3.Distance(startingPosition, presenceOGPosSx);
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
        if (!RoomData.Instance.timerClose)
        {
            int i = 0;
            bool valid = false;
            do
            {
                targetPos = getRandomPoint();
                if (checkRandomPoint(new Vector3(targetPos.x,0,targetPos.z), range, out navPosition))
                {
                    valid = canReach(navPosition);
                }
                targetPos.Set(navPosition.x, targetPos.y, navPosition.z);
                i++;
            } while (!valid && i < 30);
        }
        else
        {
            targetPos = startingPosition;
            endSim = true;
            presence.transform.GetChild(0).GetComponent<AmbienceRoom>().StopSound(easterEgg);
            presence.transform.GetChild(1).GetComponent<AmbienceRoom>().StopSound(easterEgg);
        }

        agent.SetDestination(targetPos);
        agent.isStopped = false;
    }

    bool checkRandomPoint(Vector3 randomPoint, float range, out Vector3 navPoint)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            navPoint = hit.position;
            return true;
        }
        else
        {
            navPoint = Vector3.zero;
            return false;
        }
    }

    bool canReach(Vector3 navPoint)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(navPoint, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    Vector3 getRandomPoint()
    {
            float randomX = Random.Range(Room.center.x-Room.extents.x + agent.radius,Room.center.x + Room.extents.x - agent.radius);
            float randomZ = Random.Range(Room.center.z - Room.extents.z + agent.radius, Room.center.z + Room.extents.z - agent.radius);
            return new Vector3(randomX, 0.6f, randomZ);
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Room.center, Room.size);
        /*foreach(Bounds obj in Obj)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(obj.center, obj.size);
        }*/
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPos, 0.2f);
    }
}
