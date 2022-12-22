using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropValid"))
        {
            this.gameObject.transform.position = new Vector3(other.transform.position.x,this.gameObject.transform.position.y,other.transform.position.z);
        }
    }
}
