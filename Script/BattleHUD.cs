using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.CodeDom;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    // variables for the name text and the hp slider
    public TextMeshProUGUI nameText;
    public Slider hpSlider;

    // sets the hud for the start of the game
    public void setHUD(Unit unit) 
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }

    // sets the hp for when peoples hp changes
    public void SetHP(int hp) 
    {
        hpSlider.value = hp;
    }    
}
