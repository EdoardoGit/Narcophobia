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
                RoomData.Instance.dimX = 3;
                break;
            case 1:
                RoomData.Instance.dimX = 4;
                break;
            case 2:
                RoomData.Instance.dimX = 5;
                break;
            case 3:
                RoomData.Instance.dimX = 6;
                break;
        }
    }

    public void setY(int Z)
    {
        switch (Z)
        {
            case 0:
                RoomData.Instance.dimZ = 3;
                break;
            case 1:
                RoomData.Instance.dimZ = 4;
                break;
            case 2:
                RoomData.Instance.dimZ = 5;
                break;
            case 3:
                RoomData.Instance.dimZ = 6;
                break;
        }
    }

    public void setPos(int i)
    {
        RoomData.Instance.posPorta = i;
    }

    public void setPav(int i)
    {
        RoomData.Instance.pavMat = i;
    }
}
