using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionChange : MonoBehaviour
{

    Animator anim;
    private void Awake()
    {
        anim= GetComponent<Animator>();
    }

    public void PortalColorChange()
    {
        anim.SetTrigger("M_Change");
    } 
}
