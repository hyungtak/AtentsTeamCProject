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
    
    Transform creation;

    private void Awake()
    {
        creation = transform.GetChild(0);
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
            Debug.Log("파과");
            Break();
        }
    }



    void Break()
    {
            Rigidbody[] rigid = GetComponentsInChildren<Rigidbody>();

            Vector3 center = transform.position;

            foreach (Rigidbody rb in rigid)
            {
                rb.AddExplosionForce(explosionPower, center, radius, upfoward, ForceMode.Impulse);
                boroken = true;
                Debug.Log("파과ㅣ ");
                //Destroy();
            }

        GameObject obj = Instantiate(potion , creation.position, Quaternion.identity);
        //obj.transform.position = creation.position;
    }

    //private void Destroy()
    //{
      

    //}


}