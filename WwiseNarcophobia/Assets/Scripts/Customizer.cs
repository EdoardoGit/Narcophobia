using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customizer : MonoBehaviour
{
    public AK.Wwise.RTPC roomArea;

    //index: 0 = bot, 1 = top; 2 = sx; 3 = dx;
    public GameObject[] walls = new GameObject[0];
    public GameObject floor;
    public GameObject grid;
    public GameObject container;
    public GameObject door;
    public GameObject start;
    public GameObject camera;

    public int altezza = 4;

    private int x = 0;
    private int y = 0;
    private int z = 0;

    private const float scaling = 1.5f;
    private List<GameObject> grids = new List<GameObject>();
    private int nObj;

    public NavMeshSurface surface;
    public GameObject ghost;

    private void Start()
    {
        //RoomData.setup();
        Setx();
        Sety();
        Setz();
        CheckParameters();
    }

    // Update is called once per frame
    void CheckParameters()
    {
        if(x!=0 && y!=0 && z != 0)
        {
            //Debug.Log("Genero la stanza");
            GenerateRoom();
            GenerateGrid();
            SetupDoor();
            SetupStart();
            //SetupGhost();
            //surface.BuildNavMesh();
        }
    }

    void GenerateRoom()
    {
        walls[0].transform.localScale = new Vector3(3 * x, y, 1f);
        //Debug.Log("Nuovo Scaling:" + walls[0].transform.localScale);
        walls[0].transform.localPosition = new Vector3(0f, y / 2, -(z*1.5f + 0.5f));
        walls[1].transform.localScale = new Vector3(3 * x, y, 1f);
        walls[1].transform.localPosition = new Vector3(0f, y / 2, z*1.5f + 0.5f);
        walls[2].transform.localScale = new Vector3(1f, y, 3 * z);
        walls[2].transform.localPosition = new Vector3(-(x*1.5f + 0.5f), y / 2, 0);
        walls[3].transform.localScale = new Vector3(1f, y, 3 * z);
        walls[3].transform.localPosition = new Vector3(x*1.5f + 0.5f, y / 2, 0);
        floor.transform.localScale = new Vector3(3f * x / 10f, 1f, 3f * z / 10f);
        floor.transform.localPosition = new Vector3(0f, 0f, 0f);


        //Cambio materiale pavimento
        Material floorMaterial;
        switch (RoomData.Instance.pavMat)
        {
            case 0:
                floorMaterial = Resources.Load<Material>("Materials/Marmo");
                break;
            case 1:
                floorMaterial = Resources.Load<Material>("Materials/Parque");
                break;
            default :
                floorMaterial = Resources.Load<Material>("Materials/Cotto");
                break;
        }
        floor.GetComponent<MeshRenderer>().material = floorMaterial;

        //Ridimensiono il box collider per il reverb
        this.GetComponent<BoxCollider>().center = new Vector3(0, 2, 0);
        this.GetComponent<BoxCollider>().size = new Vector3(x * 3f, 4f, z * 3f);
        this.GetComponent<BoxCollider>().enabled = false;
    }

    void GenerateGrid()
    {
        int nGrid = x * z;
        //Vector3 gridPos = new Vector3();
        for(int row = 0; row < z-1; row++)
        {
            for(int col = 0; col < x-1; col++)
            {
                /*Bounds b = lastDragged.GetComponent<Collider2D>().bounds;
                Vector2 upL = new Vector2(b.min.x, b.max.y);
                Vector2 upR = b.max;
                Vector2 botL = b.min;
                Vector2 botR = new Vector2(b.max.x, b.min.y);*/
                /*Vector2 upR = new Vector2(x * scaling - col * scaling * 2, z * scaling - row * scaling * 2);
                Vector2 upL = new Vector2(x * scaling - (col + 1) * scaling * 2, z * scaling - row * scaling * 2);
                Vector2 botR = new Vector2(x * scaling - col * scaling, z * scaling - (row + 1) * scaling * 2);
                Vector2 botL = new Vector2(x * scaling - (col + 1) * scaling * 2, z * scaling - (row + 1) * scaling * 2);*/
                GameObject tmp = Instantiate(grid, container.transform);
                float scaleX = x * scaling * 2 / (x - 1);
                float scaleZ = z * scaling * 2 / (z - 1);
                tmp.transform.localPosition = new Vector3(x * scaling - scaleX*col - scaleX/2, 0f, z * scaling - scaleZ*row - scaleZ/2);
                tmp.transform.localScale = new Vector3(scaleX * 0.1f -0.05f, tmp.transform.localScale.y, scaleZ * 0.1f - 0.05f);

                //BoxCollider collider = tmp.AddComponent<BoxCollider>();
                //collider.isTrigger = true;
                //collider.size = new Vector3((x * scaling - col * scaling * 2) - (x * scaling - (col + 1) * scaling * 2),0.1f,(z * scaling - row * scaling * 2) - (z * scaling - (row + 1) * scaling * 2));
                grids.Add(tmp);
            }
        }
        Destroy(grid);
    }

    void SetupDoor()
    {
        switch (RoomData.Instance.posPorta)
        {
            case 0:
                //Alto destra
                door.transform.localPosition = new Vector3 (grids[0].transform.position.x, 0, walls[1].transform.position.z-0.55f);
                break;
            case 1:
                //Alto sinistra
                door.transform.localPosition = new Vector3 (grids[x - 2].transform.position.x, 0, walls[1].transform.position.z - 0.55f);
                break;
            case 2:
                //Basso destra
                door.transform.localPosition = new Vector3(grids[((x - 1) * (z - 2))].transform.position.x, 0, walls[0].transform.position.z + 0.55f);
                door.transform.Rotate(0, -180, 0);
                break;
            case 3:
                //Basso sinistra
                door.transform.localPosition = new Vector3(grids[(x-1)*(z-1)-1].transform.position.x, 0, walls[0].transform.position.z + 0.55f);
                door.transform.Rotate(0, -180, 0);
                break;
            case 4:
                //Destra alto
                door.transform.localPosition = new Vector3(walls[3].transform.position.x - 0.55f, 0, grids[0].transform.position.z);
                door.transform.Rotate(0, 90, 0);
                break;
            case 5:
                //Destra basso
                door.transform.localPosition = new Vector3(walls[3].transform.position.x - 0.55f, 0, grids[(x - 1) * (z - 2)].transform.position.z);
                door.transform.Rotate(0, 90, 0);
                break;
            case 6:
                //Sinistra alto
                door.transform.localPosition = new Vector3(walls[2].transform.position.x + 0.55f, 0, grids[x - 2].transform.position.z);
                door.transform.Rotate(0, -90, 0);
                break;
            case 7:
                //Sinistra basso
                door.transform.localPosition = new Vector3(walls[2].transform.position.x + 0.55f, 0, grids[(x - 1) * (z - 1) - 1].transform.position.z);
                door.transform.Rotate(0, -90, 0);
                break;
        }
    }

    void SetupStart()
    {
        switch (RoomData.Instance.posPorta)
        {
            case 0:
                //Alto destra
                start.transform.localPosition = new Vector3(grids[0].transform.position.x, 0, walls[1].transform.position.z + 1.5f);
                Destroy(grids[0].gameObject);
                break;
            case 1:
                //Alto sinistra
                start.transform.localPosition = new Vector3(grids[x - 2].transform.position.x, 0, walls[1].transform.position.z + 1.5f);
                Destroy(grids[x-2].gameObject);
                break;
            case 2:
                //Basso destra
                start.transform.localPosition = new Vector3(grids[((x - 1) * (z - 2))].transform.position.x, 0, walls[0].transform.position.z - 1.5f);
                Destroy(grids[((x - 1) * (z - 2))].gameObject);
                break;
            case 3:
                //Basso sinistra
                start.transform.localPosition = new Vector3(grids[(x - 1) * (z - 1) - 1].transform.position.x, 0, walls[0].transform.position.z - 1.5f);
                Destroy(grids[(x - 1) * (z - 1) - 1].gameObject);
                break;
            case 4:
                //Destra alto
                start.transform.localPosition = new Vector3(walls[3].transform.position.x + 1.5f, 0, grids[0].transform.position.z);
                Destroy(grids[0].gameObject);
                break;
            case 5:
                //Destra basso
                start.transform.localPosition = new Vector3(walls[3].transform.position.x + 1.5f, 0, grids[(x - 1) * (z - 2)].transform.position.z);
                Destroy(grids[(x - 1) * (z - 2)].gameObject);
                break;
            case 6:
                //Sinistra alto
                start.transform.localPosition = new Vector3(walls[2].transform.position.x - 1.5f, 0, grids[x - 2].transform.position.z);
                Destroy(grids[x - 2].gameObject);
                break;
            case 7:
                //Sinistra basso
                start.transform.localPosition = new Vector3(walls[2].transform.position.x - 1.5f, 0, grids[(x - 1) * (z - 1) - 1].transform.position.z);
                Destroy(grids[(x - 1) * (z - 1) - 1].gameObject);
                break;
        }
    }

    public void SetupGhost()
    {
        surface.BuildNavMesh();
        Instantiate(ghost, new Vector3(start.transform.position.x, 1, start.transform.position.z), Quaternion.identity);
        camera.transform.position = new Vector3(-8.5f, camera.transform.position.y, camera.transform.position.z);
        //visualDistance();
    }

    public void SetupWWise()
    {
        int roomSize = x * z;
        Debug.Log("Dimesione stamza: " + roomSize);
        roomArea.SetGlobalValue(roomSize);
    }

    void visualDistance()
    {
        foreach(Transform grid in container.transform)
        {
            if(grid.transform.childCount!=0)
                Debug.Log("Distanza tra " + grid.transform.GetChild(0).name + " e " + grid.transform.GetChild(0).name + " : " + Vector3.Distance(grid.transform.position, container.transform.GetChild(0).transform.position));
        }
    }

    public void Setx()
    {
        x = RoomData.Instance.dimX;
        Debug.Log("X settata a:" + x);
        //CheckParameters();
    }

    public void Setz()
    {
        z = RoomData.Instance.dimZ;
        Debug.Log("Z settata a:" + z);
        //CheckParameters();
    }

    public void Sety()
    {
        y = altezza;
        Debug.Log("Y settata a:" + y);
    }
}
