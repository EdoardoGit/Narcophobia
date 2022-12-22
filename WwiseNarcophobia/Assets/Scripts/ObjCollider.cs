using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCollider : MonoBehaviour
{
    public AK.Wwise.Event objSound;
    public int cooldown = 15;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            objSound.Post(gameObject);
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        this.gameObject.GetComponent<SphereCollider>().enabled = true;
    }
}
