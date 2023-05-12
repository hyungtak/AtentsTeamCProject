using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldToTunnel : MonoBehaviour
{
    private int a;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            other.transform.position = Vector3.zero;
            SceneManager.LoadScene(2);          //터널로 씬 전환
        }
    }

}
