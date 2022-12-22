using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperlink : MonoBehaviour
{
    public void OpenGoogleForm()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSfzN6j2pRAe_HSHVE3d6XiuPqcMaV-e6Pc7VbYU6l4jZfQ8-w/viewform?usp=sf_link");
    }
}
