using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBroken : MonoBehaviour
{
    bool boroken = false;

    public GameObject potion;
    public float explosionPower = 1.0f;
    public float radius = 1.0f;
    public float upfoward = -1.0f;
    public float timer = 3.0f;
    Rigidbody []rigid;

    Transform creation;

    private void Awake()
    {
        creation = transform.GetChild(0);
        rigid = GetComponentsInChildren<Rigidbody>();
    }
    private void Start()
    {
        foreach(Rigidbody rb in rigid)
        {
            rb.useGravity= false;

        }
    }


    void Update()
    {
        if (boroken)
        {
            Destroy(gameObject, timer);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Break();
            
            foreach (Rigidbody rb in rigid)
            {
                rb.useGravity = true;
            }

                
        }
    }



    void Break()
    {

        // Rigidbody[] rigid = GetComponentsInChildren<Rigidbody>();
        Vector3 center = transform.position;
        foreach (Rigidbody rb in rigid)
        {
            rb.AddExplosionForce(explosionPower, center, radius, upfoward, ForceMode.Impulse);
        }
        GameObject obj = Instantiate(potion, creation.position, Quaternion.identity);
            boroken = true;
            
       
    }




}
