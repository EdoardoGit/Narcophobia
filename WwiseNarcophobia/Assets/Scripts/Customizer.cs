using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customizer : MonoBehaviour
{
    //index: 0 = bot, 1 = top; 2 = sx; 3 = dx;
    public GameObject[] walls = new GameObject[0];
    public GameObject floor;
    public GameObject grid;
    public GameObject container;

    public int altezza = 4;

    private int x = 0;
    private int y = 0;
    private int z = 0;

    private const float scaling = 1.5f;
    private List<GameObject> grids = new List<GameObject>();

    public NavMeshSurface surface;

    private void Start()
    {
        RoomData.setup();
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
            Debug.Log("Genero la stanza");
            GenerateRoom();
            GenerateGrid();
            surface.BuildNavMesh();
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

    public void Setx()
    {
        x = RoomData.dimX;
        Debug.Log("X settata a:" + x);
        //CheckParameters();
    }

    public void Setz()
    {
        z = RoomData.dimZ;
        Debug.Log("Z settata a:" + z);
        //CheckParameters();
    }

    public void Sety()
    {
        y = altezza;
        Debug.Log("Y settata a:" + y);
    }
}
