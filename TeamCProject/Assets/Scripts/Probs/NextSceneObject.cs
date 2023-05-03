using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("LoadingUI").SetActive(true);

    }
}
