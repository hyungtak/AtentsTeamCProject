using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StoneTrap : MonoBehaviour
{
    public GameObject stone;

    Transform stonePos;

    public Action onTrap;
    private void Awake()
    {
        stonePos = transform.GetChild(0);
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject obj = Instantiate(stone);
            obj.transform.position = stonePos.position;
        }
    }


}
