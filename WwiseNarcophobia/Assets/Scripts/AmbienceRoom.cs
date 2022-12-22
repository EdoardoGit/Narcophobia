using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceRoom : MonoBehaviour
{
    public AK.Wwise.Event standardAmbience;
    public AK.Wwise.Event specialAmbience;
    public AK.Wwise.Event ghostSound;

    public void StartSound(bool easter)
    {
        if (!easter)
            standardAmbience.Post(gameObject);
        else
            specialAmbience.Post(gameObject);
    }

    public void ghostStartSound()
    {
        ghostSound.Post(gameObject);
    }

    public void StopSound(bool easter)
    {
        if (!easter)
            standardAmbience.Stop(gameObject,1000);
        else
            specialAmbience.Stop(gameObject,1000);
    }
}
