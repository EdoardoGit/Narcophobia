using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public AK.Wwise.RTPC objCount;

    public GameObject container;
    public GameObject menuAdd;
    public GameObject menuRemove;
    public GameObject startButton;
    public GameObject eventManager;

    public GameObject bed;
    public GameObject bookcase;
    public GameObject closet;
    public GameObject laptop;
    public GameObject desktop;
    public GameObject rug;

    //public GameObject floor;
    //public GameObject plane;

    private GameObject selected;
    private Vector3 reset;
    private GameObject rifBed;
    private bool bedAdded = false;
    private int objCounter = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayCast();

            if (hit.collider != null)
            {
                if (!hit.collider.CompareTag("Grid") && !hit.collider.CompareTag("Obj"))
                {
                    return;
                }

                selected = hit.collider.gameObject;
                reset = selected.transform.position;

                if (selected.transform.childCount == 0 && !selected.CompareTag("Obj"))
                {
                    startButton.SetActive(false);
                    if (menuRemove.activeSelf)
                        menuRemove.SetActive(false);
                    if (!menuAdd.activeSelf)
                        menuAdd.SetActive(true);
                    //Controlla se il menù principale o rimozione è aperto, chiudi se aperti, sennò apri il principale
                }
                else if (selected.transform.GetChild(0).CompareTag("Porta"))
                    return;
                else
                {
                    startButton.SetActive(false);
                    if (menuAdd.activeSelf)
                        menuAdd.SetActive(false);
                    if (!menuRemove.activeSelf)
                        menuRemove.SetActive(true);
                    //Controlla se il menù principale è aperto, se lo è chiudilo. Poi apri il menu rimozione se non è già aperto 
                }
            }
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

    public void placeBed()
    {
        GameObject cloneBed = Instantiate(bed);
        cloneBed.transform.position = reset;
        cloneBed.transform.parent = selected.transform;
        rifBed = cloneBed;
        selected.transform.SetAsFirstSibling();
        menuAdd.SetActive(false);
        menuAdd.transform.GetChild(0).gameObject.SetActive(false);
        startButton.SetActive(true);
        bedAdded = true;
        objCounter++;
    }

    public void placeDesktop()
    {
        GameObject cloneDesktop = Instantiate(desktop);
        cloneDesktop.transform.position = reset;
        cloneDesktop.transform.parent = selected.transform;
        menuAdd.SetActive(false);
        if (bedAdded)
            startButton.SetActive(true);
        objCounter++;
    }

    public void placeLaptop()
    {
        GameObject cloneLaptop = Instantiate(laptop);
        cloneLaptop.transform.position = reset;
        cloneLaptop.transform.parent = selected.transform;
        menuAdd.SetActive(false);
        if (bedAdded)
            startButton.SetActive(true);
        objCounter++;
    }

    public void placeRug()
    {
        GameObject cloneRug = Instantiate(rug);
        cloneRug.transform.position = reset;
        cloneRug.transform.parent = selected.transform;
        menuAdd.SetActive(false);
        if (bedAdded)
            startButton.SetActive(true);
        objCounter++;
    }

    public void placeCloset()
    {
        GameObject cloneCloset = Instantiate(closet);
        cloneCloset.transform.position = reset;
        cloneCloset.transform.parent = selected.transform;
        menuAdd.SetActive(false);
        if (bedAdded)
            startButton.SetActive(true);
        objCounter++;
    }

    public void placeBookcase()
    {
        GameObject cloneBookcase = Instantiate(bookcase);
        cloneBookcase.transform.position = reset;
        cloneBookcase.transform.parent = selected.transform;
        menuAdd.SetActive(false);
        if (bedAdded)
            startButton.SetActive(true);
        objCounter++;
    }

    public void removeObj()
    {
        GameObject tmp;
        if (selected.CompareTag("Obj"))
        {
            if(selected.gameObject.Equals(rifBed))
            {
                menuAdd.transform.GetChild(0).gameObject.SetActive(true);
                bedAdded = false;
            }
            tmp = selected;
            Destroy(tmp);
        }
        else if (selected.CompareTag("Grid"))
        {
            if (selected.transform.GetChild(0).gameObject.Equals(rifBed))
            {
                menuAdd.transform.GetChild(0).gameObject.SetActive(true);
                bedAdded = false;
            }
            tmp = selected.transform.GetChild(0).gameObject;
            Destroy(tmp);
        }
        menuRemove.SetActive(false);
        if (bedAdded)
            startButton.SetActive(true);
        objCounter--;
    }
    
    public void rotateDx()
    {
        if (selected.CompareTag("Obj"))
            selected.transform.Rotate(0, 90, 0);
        else if (selected.CompareTag("Grid"))
            selected.transform.GetChild(0).gameObject.transform.Rotate(0, 90, 0);
    }

    public void rotateSx()
    {
        if (selected.CompareTag("Obj"))
            selected.transform.Rotate(0, -90, 0);
        else if (selected.CompareTag("Grid"))
            selected.transform.GetChild(0).gameObject.transform.Rotate(0, -90, 0);
    }

    public void hideGrid()
    {
        foreach (Transform grid in container.transform)
        {
            grid.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        startButton.SetActive(false);
    }

    public void back()
    {
        menuAdd.SetActive(false);
        menuRemove.SetActive(false);
        if (bedAdded)
            startButton.SetActive(true);
    }

    public void disableInput()
    {
        eventManager.SetActive(false);
    }

    public void enableInput()
    {
        eventManager.SetActive(true);
    }

    public void disableSelector()
    {
        this.gameObject.SetActive(false);
    }

    public void SetupWWise()
    {
        Debug.Log("Numero oggetti: "+objCounter);
        objCount.SetGlobalValue(objCounter);
    }

    /*private void CheckGrid(Vector3 worldPosition)
    {
        List<GridDistance> grids = new List<GridDistance>();

        foreach (Transform child in container.transform)
        {
            GridDistance tmp = new GridDistance(Vector3.Distance(child.position, selected.transform.position), child);
            grids.Add(tmp);
        }

        grids.Sort((g1, g2) => g1.distance.CompareTo(g2.distance));

        Reposition(grids);
    }

    private void Reposition(List<GridDistance> grids)
    {
        foreach (GridDistance grid in grids)
        {
            if (grid.obj.childCount == 0)
            {
                selected.transform.position = new Vector3(grid.obj.position.x, reset.y, grid.obj.position.z);
                selected.transform.parent = grid.obj;
                return;
            }
        }
        Debug.Log("Nessuna Posizione valida");
        selected.transform.position = reset;

    }*/
}
