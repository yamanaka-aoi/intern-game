using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D PlayerRb;
    [SerializeField]private float MovementForce = 0.01f;
    [SerializeField]private float JumpForce = 0.01f;
    private bool _isGround = false; 
    // Start is called before the first frame update
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody2D>(); 
        _isGround = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)){
            PlayerRb.velocity += new Vector2(-MovementForce, 0);
            this.transform.rotation = Quaternion.Euler(0,180f,0);
        }
        else if(Input.GetKey(KeyCode.D)){
            PlayerRb.velocity += new Vector2(+MovementForce, 0);
            this.transform.rotation = Quaternion.Euler(0,0f,0);
        }

        if(Input.GetKey(KeyCode.Space) && _isGround == true){
            PlayerRb.velocity += new Vector2(0, JumpForce);
        }
    }
    
    void OnCollisionEnter2D(Collision2D target){
        if(target.gameObject.CompareTag("Ground")){
            _isGround = true;
        }
    }
    void OnCollisionExit2D(Collision2D target){
        if(target.gameObject.CompareTag("Ground")){
            _isGround = false;
        }
    }
}
