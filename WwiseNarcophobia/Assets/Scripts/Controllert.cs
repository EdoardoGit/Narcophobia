using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controllert : MonoBehaviour
{
    [SerializeField] private GameObject[] Menu_Obj = new GameObject[0];
    [SerializeField] private Dropdown[] DropDowns = new Dropdown[0];
    [SerializeField] private GameObject[] room = new GameObject[0];
    private int menuIndex = 0;
    private int ntap;
    private bool reverse = true;

    public AK.Wwise.RTPC tap;

    private void Start()
    {
        ntap = 0;
        tap.SetGlobalValue(ntap);
        foreach (Dropdown drop in DropDowns)
            drop.onValueChanged.AddListener(delegate { DropdownValueChanged(drop); });
        //RoomData.Instance.Start();
    }

    void DropdownValueChanged(Dropdown drop)
    {
        switch (System.Array.IndexOf(DropDowns, drop))
        {
            case 0:
                setX(drop.value);
                break;
            case 1:
                setZ(drop.value);
                break;
            case 2:
                setPos(drop.value);
                break;
            case 3:
                setPav(drop.value);
                break;
            case 4:
                setTimer(drop.value);
                break;
        }
    }

    public void openInstructions()
    {
        Menu_Obj[0].SetActive(false);
        Menu_Obj[1].SetActive(true);
        menuIndex = 7;
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
        Menu_Obj[0].SetActive(false);
        Menu_Obj[3].SetActive(true);
        menuIndex = 4;
    }

    public void back2Main()
    {
        Menu_Obj[menuIndex-1].SetActive(false);
        Menu_Obj[0].SetActive(true);
    }

    public void next()
    {
        Menu_Obj[menuIndex - 1].SetActive(false);
        Menu_Obj[menuIndex].SetActive(true);
        menuIndex++;
        if (menuIndex == 5)
            Menu_Obj[11].SetActive(true);
    }

    public void previous()
    {
        Menu_Obj[menuIndex - 1].SetActive(false);
        Menu_Obj[menuIndex - 2].SetActive(true);
        menuIndex--;
        if(menuIndex == 4)
            Menu_Obj[11].SetActive(false);
    }

    public void setX(int X)
    {
        switch (X)
        {
            case 0:
                RoomData.Instance.xSet(3);
                room[0].gameObject.transform.localScale = new Vector3(0.4f,room[0].gameObject.transform.localScale.y, room[0].gameObject.transform.localScale.z);
                room[2].gameObject.transform.localPosition = new Vector3(100f, room[2].gameObject.transform.localPosition.y, room[2].gameObject.transform.localPosition.z);
                break;
            case 1:
                RoomData.Instance.xSet(4);
                room[0].gameObject.transform.localScale = new Vector3(0.5f, room[0].gameObject.transform.localScale.y, room[0].gameObject.transform.localScale.z);
                room[2].gameObject.transform.localPosition = new Vector3(110f, room[2].gameObject.transform.localPosition.y, room[2].gameObject.transform.localPosition.z);
                break;
            case 2:
                RoomData.Instance.xSet(5);
                room[0].gameObject.transform.localScale = new Vector3(0.6f, room[0].gameObject.transform.localScale.y, room[0].gameObject.transform.localScale.z);
                room[2].gameObject.transform.localPosition = new Vector3(120f, room[2].gameObject.transform.localPosition.y, room[2].gameObject.transform.localPosition.z);
                break;
        }
    }

    public void setZ(int Z)
    {
        switch (Z)
        {
            case 0:
                RoomData.Instance.zSet(3);
                room[0].gameObject.transform.localScale = new Vector3(room[0].gameObject.transform.localScale.x, 0.4f, room[0].gameObject.transform.localScale.z);
                room[1].gameObject.transform.localPosition = new Vector3(room[1].gameObject.transform.localPosition.x, -165f, room[1].gameObject.transform.localPosition.z);
                break;
            case 1:
                RoomData.Instance.zSet(4);
                room[0].gameObject.transform.localScale = new Vector3(room[0].gameObject.transform.localScale.x, 0.5f, room[0].gameObject.transform.localScale.z);
                room[1].gameObject.transform.localPosition = new Vector3(room[1].gameObject.transform.localPosition.x, -175f, room[1].gameObject.transform.localPosition.z);
                break;
            case 2:
                RoomData.Instance.zSet(5);
                room[0].gameObject.transform.localScale = new Vector3(room[0].gameObject.transform.localScale.x, 0.6f, room[0].gameObject.transform.localScale.z);
                room[1].gameObject.transform.localPosition = new Vector3(room[1].gameObject.transform.localPosition.x, -185f, room[1].gameObject.transform.localPosition.z);
                break;
        }
    }

    public void setPos(int i)
    {
        RoomData.Instance.portaSet(i);
        Debug.Log("Blabla" + RoomData.Instance.posPorta);
    }

    public void setPav(int i)
    {
        RoomData.Instance.pavSet(i);
    }

    public void setTimer(int T)
    {
        switch (T)
        {
            case 0:
                RoomData.Instance.timeSet(60);
                break;
            case 1:
                RoomData.Instance.timeSet(90);
                break;
            case 2:
                RoomData.Instance.timeSet(120);
                break;
        }
    }

    public void nextIstr()
    {
        Menu_Obj[menuIndex].SetActive(false);
        Menu_Obj[menuIndex + 1].SetActive(true);
        menuIndex++;
        if (menuIndex == 9)
        {
            Menu_Obj[menuIndex + 1].SetActive(false);
        }
    }

    public void prevIstr()
    {
        if (menuIndex > 7)
        {
            Menu_Obj[10].SetActive(true);
            Menu_Obj[menuIndex].SetActive(false);
            Menu_Obj[menuIndex - 1].SetActive(true);
            menuIndex--;
        }
        else
        {
            closeInstructions();
        }

    }

    public void increaseTap()
    {
        if (reverse)
        {
            ntap++;
            tap.SetGlobalValue(ntap);
            if (ntap == 10)
                reverse = false;
        }
        else
        {
            ntap--;
            tap.SetGlobalValue(ntap);
            if (ntap == 0)
                reverse = true;
        }
        //Debug.Log("tap=" + ntap);
    }

    /*public void loadBuilder()
    {
        Debug.Log("Passo alla prossima scena con queste informazioni: x " + RoomData.dimX + " z " + RoomData.dimZ + " pos " + RoomData.posPorta + " pav " + RoomData.pavMat + " timer " + RoomData.timer);
    }
    */
}
