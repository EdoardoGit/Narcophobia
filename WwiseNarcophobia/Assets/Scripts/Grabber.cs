using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct GridDistance
{
    public float distance;
    public Transform obj;

    public GridDistance (float distance, Transform obj)
    {
        this.distance = distance;
        this.obj = obj;
    }
}

public class Grabber : MonoBehaviour
{
    public GameObject container;
    public GameObject floor;
    public GameObject plane;

    private GameObject selected;
    private Vector3 reset;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(selected == null)
            {
                RaycastHit hit = RayCast();

                if(hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Drag"))
                    {
                        return;
                    }

                    selected = hit.collider.gameObject;
                    Cursor.visible = false;
                    //Debug.Log(selected.transform.position.y);
                    reset = selected.transform.position;
                    
                }
            }
            else
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selected.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                //selected.transform.position = new Vector3(worldPosition.x, reset.y, worldPosition.z);
                Bounds boundsFloor = floor.GetComponent<MeshCollider>().bounds;
                Bounds boundsPlane = plane.GetComponent<MeshCollider>().bounds;
                Debug.Log("boundsFloor: " + boundsFloor);
                Debug.Log("boundsPlane: " + boundsPlane);
                Debug.Log("MousePosition " + worldPosition);
                /* Vector2 upL = new Vector2(b.min.x, b.max.y);
                Vector2 upR = b.max;
                Vector2 botL = b.min;
                Vector2 botR = new Vector2(b.max.x, b.min.y);
                */
                if (worldPosition.x > boundsFloor.min.x && worldPosition.x < boundsFloor.max.x && worldPosition.z > boundsFloor.min.z && worldPosition.z < boundsFloor.max.z)
                    CheckGrid(worldPosition);
                else if (worldPosition.x > boundsPlane.min.x && worldPosition.x < boundsPlane.max.x && worldPosition.z > boundsPlane.min.z && worldPosition.z < boundsPlane.max.z)
                    selected.transform.position = new Vector3(worldPosition.x, reset.y, worldPosition.z);
                else
                    selected.transform.position = reset;
                selected = null;
                Cursor.visible = true;
            }
        }

        if (selected != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selected.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selected.transform.position = new Vector3(worldPosition.x, reset.y + 0.25f, worldPosition.z);
        }
    }

    private RaycastHit RayCast()
    {
        Vector3 ScreenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 ScreenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(ScreenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(ScreenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        if (hit.collider != null) 
            Debug.Log("Ho colpito " + hit.collider.tag);
        return hit;
    }

    private void CheckGrid(Vector3 worldPosition)
    {
        List<GridDistance> grids = new List<GridDistance>();

        foreach(Transform child in container.transform)
        {
            GridDistance tmp = new GridDistance(Vector3.Distance(child.position, selected.transform.position), child);
            grids.Add(tmp);
        }

        grids.Sort((g1, g2) => g1.distance.CompareTo(g2.distance));

        Reposition(grids);
    }

    private void Reposition(List<GridDistance> grids)
    {
        foreach(GridDistance grid in grids)
        {
            if(grid.obj.childCount == 0)
            {
                selected.transform.position = new Vector3(grid.obj.position.x,reset.y,grid.obj.position.z);
                selected.transform.parent = grid.obj;
                return;
            }
        }
        Debug.Log("Nessuna Posizione valida");
        selected.transform.position = reset;

    }
}
