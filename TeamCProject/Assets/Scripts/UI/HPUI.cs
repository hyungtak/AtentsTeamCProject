using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPUI : UIBase
{
    TextMeshProUGUI playerHpText;
    Slider playerHpSlider;

    int playerMaxHp;

    void Start()
    {
        playerHpSlider = transform.GetChild(0).GetComponent<Slider>();
        playerHpText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        playerMaxHp = player.maxHealth;
        player.OnHpChange += onHpChange;
        playerHpSlider.value = (float)player.CurrentHealth / playerMaxHp;
        playerHpText.text = $"{player.CurrentHealth} / {playerMaxHp}";
    }

    void Update()
    {

    }

    private void onHpChange(int playerHp)
    {
        playerHpText.text = $"{playerHp} / {playerMaxHp}";

        //채력 Int가 아닌 float 로 변경 필요
        playerHpSlider.value = (float)playerHp / playerMaxHp;

    }
}
