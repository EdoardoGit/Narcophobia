using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour
{
    public AK.Wwise.Event menuSound;

    void Start()
    {
        menuSound.Post(gameObject);
    }

    public void onSceneChange()
    {
        menuSound.Stop(gameObject, 1000);
    }
}
