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
    public InputField inputX;
    public InputField inputY;
    public InputField inputZ;

    private float x = 0;
    private float y = 0;
    private float z = 0;

    public NavMeshSurface surface;

    // Update is called once per frame
    void CheckParameters()
    {
        if(x!=0 && y!=0 && z != 0)
        {
            Debug.Log("Genero la stanza");
            GenerateRoom();
            surface.BuildNavMesh();
        }
    }

    void GenerateRoom()
    {
        walls[0].transform.localScale = new Vector3(2 * x, y, 1f);
        //Debug.Log("Nuovo Scaling:" + walls[0].transform.localScale);
        walls[0].transform.localPosition = new Vector3(0f, y / 2, -(z + 0.5f));
        walls[1].transform.localScale = new Vector3(2 * x, y, 1f);
        walls[1].transform.localPosition = new Vector3(0f, y / 2, z + 0.5f);
        walls[2].transform.localScale = new Vector3(1f, y, 2 * z);
        walls[2].transform.localPosition = new Vector3(-(x + 0.5f), y / 2, 0);
        walls[3].transform.localScale = new Vector3(1f, y, 2 * z);
        walls[3].transform.localPosition = new Vector3(x + 0.5f, y / 2, 0);
        floor.transform.localScale = new Vector3(2 * x / 10, 1f, 2 * z / 10);
        floor.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void Setx()
    {
        x = float.Parse(inputX.text);
        Debug.Log("X settata a:" + x);
        CheckParameters();
    }

    public void Sety()
    {
        y = float.Parse(inputY.text);
        Debug.Log("Y settata a:" + y);
        CheckParameters();
    }

    public void Setz()
    {
        z = float.Parse(inputZ.text);
        Debug.Log("Z settata a:" + z);
        CheckParameters();
    }
}
