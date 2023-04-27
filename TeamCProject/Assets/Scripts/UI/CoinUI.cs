using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : UIBase
{
    TextMeshProUGUI coinText;

    void Start()
    {
        coinText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        player.onCoinCountChange += onCoinChange;
        coinText.text = $"X {player.CoinCount}";

    }

    void Update()
    {
        
    }

    private void onCoinChange(int playerCoin)
    { 
        coinText.text = $"X {playerCoin}";
    }
}
