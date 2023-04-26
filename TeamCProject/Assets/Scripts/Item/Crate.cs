using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject createBroken;
    Transform creation;

    private void Start()
    {
        creation = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Destroy(this.gameObject);
            float delay = 0.2f;
            StartCoroutine(create(delay));
            
            
        }
    }

    IEnumerator create(float delay)
    { 
        yield return new WaitForSeconds(delay);
        GameObject obj = Instantiate(createBroken, creation.position, Quaternion.identity); 
    }
}
