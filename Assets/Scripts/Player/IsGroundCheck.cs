using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsGroundCheck : MonoBehaviour
{
    public bool isGround = false;
    void OnTriggerEnter2D(Collider2D target){
        if(target.gameObject.tag == "Wall"){
            isGround = true;
        }
    }

    void OnTriggerExit2D(Collider2D target){
        if(target.gameObject.tag == "Wall"){
            isGround = false;
        }
    }
}
