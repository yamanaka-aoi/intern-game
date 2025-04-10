using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager
{
    private Vector2 respawnPoint;
    private static GameManager _instance;
    public static GameManager Instance
{
    get
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }
}
    public void SetRespawnPoint(Vector2 point)
    {
        respawnPoint = point;
    }

    public Vector2 GetRespawnPoint()
    {
        return respawnPoint;
    }

    public bool CanMovePlayer {get; private set;} = false;
    public int DeathCount{get; private set;} = 0;
    public void setDeathCount(int _DeathCount){
        DeathCount = _DeathCount;
    }
    public void SetCanMovePlayer(bool _CanMovePlayer){CanMovePlayer = _CanMovePlayer;}

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("RespawnX", respawnPoint.x);
        PlayerPrefs.SetFloat("RespawnY", respawnPoint.y);
        PlayerPrefs.Save();
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("RespawnX") && PlayerPrefs.HasKey("RespawnY"))
        {
            float x = PlayerPrefs.GetFloat("RespawnX");
            float y = PlayerPrefs.GetFloat("RespawnY");
            respawnPoint = new Vector2(x, y);
        }
        else
        {
            respawnPoint = Vector2.zero; // デフォルトのリスポーンポイント
        }
    }

}
