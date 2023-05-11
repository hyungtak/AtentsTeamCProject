using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameOver : MonoBehaviour
{
    /// <summary>
    /// 알파값 변경 속도
    /// </summary>
    public float alphaChangeSpeed = 1.0f;

    CanvasGroup canvasGroup;

    TextMeshProUGUI coinPoint;

    Button restart;

    Player player;

    private void Awake()
    {
        // 컴포넌트 찾기
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(2);
        coinPoint = child.GetComponent<TextMeshProUGUI>();     
        restart = GetComponentInChildren<Button>();
        player = GameObject.Find("Player").GetComponent<Player>();

        // 버튼에 함수 등록
        restart.onClick.AddListener(OnRestartClick);
    }


    private void Start()
    {
        StopAllCoroutines();
        player.OnDie += playerDie;    // 플레이어 사망시 실행할 함수 등록
    }

    void playerDie(int coin)
    {
        coinPoint.text = $"{coin}";
        StartCoroutine(AlphaChange());
    }

    IEnumerator AlphaChange()
    {
        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed; 
            yield return null;         
        }
    }

    private void OnRestartClick()
    {
        StartCoroutine(SceneChange());
    }

    //LoadScene 변경
    IEnumerator SceneChange()
    {
        SceneManager.LoadScene("FieldMap");
        yield return null;

    }




}
