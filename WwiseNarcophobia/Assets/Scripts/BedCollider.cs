using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedCollider : MonoBehaviour
{
    public AK.Wwise.Event objSound;
    public int cooldown = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ghost"))
        {
            float dist0G = Vector3.Distance(this.gameObject.transform.GetChild(1).GetChild(0).position, other.gameObject.transform.position);
            float dist1G = Vector3.Distance(this.gameObject.transform.GetChild(1).GetChild(1).position, other.gameObject.transform.position);

            if (dist0G > 1.2f * dist1G)
                this.gameObject.transform.GetChild(1).GetChild(1).GetComponent<AmbienceRoom>().ghostStartSound();
            else if (dist1G > 1.2f * dist0G)
                this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<AmbienceRoom>().ghostStartSound();
            else
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
