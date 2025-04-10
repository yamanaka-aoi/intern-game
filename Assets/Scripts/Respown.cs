using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//これはプレイヤーに入れてください
public class Respown : MonoBehaviour
{
    Animator anim;
    public Rigidbody2D RB;
    [SerializeField] GameObject particle;
    void Start(){
        anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }
private void OnCollisionEnter2D(Collision2D target)
{
    if(target.gameObject.CompareTag("Needle") || target.gameObject.CompareTag("Death")){ //死の判定
        partialoccurence();
        Invoke("Respawn", 0.5f);
    }
}
private void OnTriggerEnter2D(Collider2D target)
{
    if(target.gameObject.CompareTag("Needle") || target.gameObject.CompareTag("Death")){ //死の判定
        partialoccurence();
        Invoke("Respawn", 0.5f);
    }
}
private void Respawn()
{

    GameManager.Instance.SetCanMovePlayer(true);
    GameManager.Instance.setDeathCount(GameManager.Instance.DeathCount+1);
    Vector2 respawnPoint = GameManager.Instance.GetRespawnPoint();
    RB.simulated = true;
    transform.position = respawnPoint;
    
}
private void partialoccurence(){
    GameManager.Instance.SetCanMovePlayer(false);
    GameObject createObject = Instantiate(particle, transform.position, Quaternion.identity);
    Bullet bulletScript = createObject.GetComponent<Bullet>();
    RB.simulated = false;
}

}
