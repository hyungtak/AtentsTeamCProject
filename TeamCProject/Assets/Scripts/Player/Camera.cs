using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Camera : MonoBehaviour
{

    public Transform player;


    private  void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, 3, -7);
    }
}
