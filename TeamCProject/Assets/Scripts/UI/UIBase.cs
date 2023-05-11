using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    protected Player player;

    protected virtual void Awake()
    {
        player = FindObjectOfType<Player>();    
    }
}
