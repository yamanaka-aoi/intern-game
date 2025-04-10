using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TitleButtonSetting : MonoBehaviour
{
    public Button tutorial;
    // Start is called before the first frame update
    void Start()
    {
        tutorial.onClick.AddListener(() => Movetutorial());
    }

    // Update is called once per frame
    void Movetutorial(){
        SceneManager.LoadScene("Tutorial");
    }
}
