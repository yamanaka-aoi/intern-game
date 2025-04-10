using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//これはセーブポイントのところに入れてください
public class SavePointsSetting : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector2 savePoint = transform.position;
            GameManager.Instance.SetRespawnPoint(savePoint);
            GameManager.Instance.SaveGame();
            Debug.Log("セーブポイントに到達しました。");
        }
    }
}
