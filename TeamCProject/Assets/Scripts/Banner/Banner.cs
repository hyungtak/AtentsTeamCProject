using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banner : MonoBehaviour
{
    Transform infoGameObject;

    private void Awake()
    {
        infoGameObject = transform.GetChild(0).GetChild(0);   
    }

    // Start is called before the first frame update
    void Start()
    {
        infoGameObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            infoGameObject.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
           infoGameObject.gameObject.SetActive(false);
    }
}
