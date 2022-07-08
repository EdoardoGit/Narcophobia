using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QController : MonoBehaviour
{
    [SerializeField] private GameObject[] Questions = new GameObject[0];
    
    public void setX(int X)
    {
        switch (X)
        {
            case 0:
                RoomData.dimX = 3;
                break;
            case 1:
                RoomData.dimX = 4;
                break;
            case 2:
                RoomData.dimX = 5;
                break;
            case 3:
                RoomData.dimX = 6;
                break;
        }
    }

    public void setY(int Z)
    {
        switch (Z)
        {
            case 0:
                RoomData.dimZ = 3;
                break;
            case 1:
                RoomData.dimZ = 4;
                break;
            case 2:
                RoomData.dimZ = 5;
                break;
            case 3:
                RoomData.dimZ = 6;
                break;
        }
    }

    public void setPos(int i)
    {
        RoomData.posPorta = i;
    }

    public void setRooms()
    {

    }

    public void setPav(int i)
    {
        RoomData.pavMat = i;
    }

    public void back()
    {
        Debug.Log("Torno al main menù");
    }

    public void next()
    {
        Debug.Log("Passo alla prossima scena con queste informazioni: x " + RoomData.dimX + " z " + RoomData.dimZ + " pos " + RoomData.posPorta + " pav " + RoomData.pavMat);
    }
}
