using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TunnelToBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Canvas>().gameObject.transform.GetChild(1).gameObject.SetActive(true);
        new WaitForSeconds(1);
        SceneManager.LoadScene(3);          //보스방으로 씬 전환
        other.transform.position = Vector3.zero;

    }
}
