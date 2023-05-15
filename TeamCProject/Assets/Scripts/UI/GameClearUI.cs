using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    Player player;
    TextMeshProUGUI coinText;

    Button goToTitleSceneButton;

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.transform.position = new Vector3(-700, -700, 0);
        coinText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        coinText.text = $"X {player.CoinCount}";

        goToTitleSceneButton = transform.GetChild(2).GetComponent<Button>();
        goToTitleSceneButton.onClick.AddListener(GoToTitleScene);
        Destroy(player.gameObject);
    }

    private void GoToTitleScene()
    {
        SceneManager.LoadScene(0);
    }
}
