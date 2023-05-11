using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldToTunnel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<Canvas>().gameObject.transform.GetChild(1).gameObject.SetActive(true);
            SceneManager.LoadScene(2);          //터널로 씬 전환
            other.transform.position = Vector3.zero;

        }
    }
}
