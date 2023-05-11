using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TunnelToBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.GetChild(0).GetChild(0).gameObject.gameObject.SetActive(true);
        SceneManager.LoadScene(3);          //보스방으로 씬 전환
        other.transform.position = Vector3.zero;

    }
}
