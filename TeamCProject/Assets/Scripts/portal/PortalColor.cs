using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalColor : MonoBehaviour
{

    Animator anim;


    private void Awake()
    {
        anim= GetComponent<Animator>();
    }

    public void ColorChange()
    {
        anim.SetTrigger("M_Change");
    } 
}
