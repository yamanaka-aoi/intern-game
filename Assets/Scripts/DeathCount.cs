using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathCount : MonoBehaviour
{
    public TextMeshProUGUI tmpUI;
    void Start(){
        DisplayDeathCount();
        tmpUI = GetComponent<TextMeshProUGUI>();
    }
    private void DisplayDeathCount(){
        tmpUI.text = "Death "+GameManager.Instance.DeathCount.ToString();
    }
}
