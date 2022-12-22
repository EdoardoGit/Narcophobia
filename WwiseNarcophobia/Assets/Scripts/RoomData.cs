using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public static RoomData Instance;
    public int dimX { get; set; }
    public int dimZ { get; set; }
    public int posPorta { get; set; }
    public int pavMat { get; set; }

    public int timer { get; set; }

    public bool timerClose { get; set; }
    public bool timerOut { get; set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        dimX = 3;
        dimZ = 3;
        posPorta = 0;
        pavMat = 0;
        timer = 60;
    }

    public void setup()
    {
        dimX = Random.Range(3,5);
        dimZ = Random.Range(3, 5);
        posPorta = Random.Range(0, 7);
        pavMat = Random.Range(0,2);
    }

    public void timerReset()
    {
        timerClose = false;
        timerOut = false;
    }

    public void isClose()
    {
        timerClose = true;
    }

    public void isEnded()
    {
        timerOut = true;
    }

    public void timeSet(int time)
    {
        timer = time;
    }
    public void xSet(int x)
    {
        dimX = x;
    }
    public void zSet(int z)
    {
        dimZ = z;
    }
    public void portaSet(int porta)
    {
        posPorta = porta;
    }

    public void pavSet(int pav)
    {
        pavMat = pav;
    }
}
