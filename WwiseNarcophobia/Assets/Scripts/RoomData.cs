using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomData
{
    public static int dimX { get; set; }
    public static int dimZ { get; set; }
    public static int posPorta { get; set; }

    public static List<bool> stanzeConf = new List<bool>() {false,false,false,false};
    public static int pavMat { get; set; }

    public static void setup()
    {
        dimX = 3;
        dimZ = 4;
        posPorta = 1;
        pavMat = 2;
    }

}
