using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllert : MonoBehaviour
{
    [SerializeField] private GameObject[] Menu_Obj = new GameObject[0];

    public void openInstructions()
    {
        Menu_Obj[0].SetActive(false);
        Menu_Obj[1].SetActive(true);
    }

    public void closeInstructions()
    {
        Menu_Obj[0].SetActive(true);
        Menu_Obj[1].SetActive(false);
    }

    public void openCredits()
    {
        Menu_Obj[0].SetActive(false);
        Menu_Obj[2].SetActive(true);
    }

    public void closeCredits()
    {
        Menu_Obj[0].SetActive(true);
        Menu_Obj[2].SetActive(false);
    }

    public void loadSetup()
    {
        Debug.Log("Carico scena di setup");
    }
}
