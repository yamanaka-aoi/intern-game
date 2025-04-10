using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class player_wire_test : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("playerいた");
    }
}
