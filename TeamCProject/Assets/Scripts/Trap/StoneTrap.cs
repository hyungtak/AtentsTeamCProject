using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTrap : MonoBehaviour
{
    public GameObject stone;

    Transform stonePos;


    private void Awake()
    {
        stonePos = FindObjectOfType<Transform>();
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //소환
        }
    }


}
