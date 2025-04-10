using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ClaerButtonSetting : MonoBehaviour
{
    public Button GotoTitle;
    // Start is called before the first frame update
    void Start()
    {
        GotoTitle.onClick.AddListener(() => Movetutorial());
    }

    // Update is called once per frame
    void Movetutorial(){
        SceneManager.LoadScene("Title");
    }
}

