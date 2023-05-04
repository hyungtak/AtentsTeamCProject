using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StoneDown : MonoBehaviour
{
    public GameObject stone;

    Transform stonePos1;
    Transform stonePos2;
    Transform stonePos3;

    WaitForSeconds stoneDelay = new WaitForSeconds(1);

    public Action onTrap;
    private void Awake()
    {
        Transform child = transform.GetChild(0);
        stonePos1 = child.GetComponent<Transform>();
        child = transform.GetChild(1);
        stonePos2 = child.GetComponent<Transform>();
        child = transform.GetChild(2);
        stonePos3 = child.GetComponent<Transform>();
    }



    public void StoneStart()
    {
        StartCoroutine(StoneActivate());
    }


    IEnumerator StoneActivate()
    {   
        GameObject obj1 = Instantiate(stone);
        obj1.transform.position = stonePos1.position;
        yield return stoneDelay;

        GameObject obj2 = Instantiate(stone);
        obj2.transform.position = stonePos2.position;
        yield return stoneDelay;
        GameObject obj3 = Instantiate(stone);
        obj3.transform.position = stonePos3.position;
        yield return stoneDelay;
        StopCoroutine(StoneActivate());
        

    }

}
