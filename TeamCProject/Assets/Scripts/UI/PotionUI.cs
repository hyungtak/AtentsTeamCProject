using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PotionUI : UIBase
{
    TextMeshProUGUI potionText;

    void Start()
    {
        potionText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        player.onPotionCountChange += onPotionChange;
        potionText.text = $"X {player.PotionCount}";
    }

    void Update()
    {

    }

    private void onPotionChange(int playerPotionCount)
    {
        potionText.text = $"X {playerPotionCount}";
    }
}
