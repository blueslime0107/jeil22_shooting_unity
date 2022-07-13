using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFirst : MonoBehaviour
{
    public GameManager gameManager;
    public Image nameInputField;
    public Image tutorial;
    public Text playerName;
    public Text ScoreTextTop;
    public Text ScoreText;



    public string named;

    public int saveMaxScore;
    public int savedScore;
    public string savedName;

    void Awake(){
        saveMaxScore = PlayerPrefs.GetInt("score");
    }

    public void nameInputed(){
        named = playerName.text;       
        nameInputField.gameObject.SetActive(false);

        PlayerPrefs.SetInt("score", gameManager.score);
        PlayerPrefs.SetString("name", named);
    }

    public void PlayerDied(){
        tutorial.gameObject.SetActive(false);
        Debug.Log(gameManager.score > saveMaxScore);
        if(gameManager.score > saveMaxScore){       
            nameInputField.gameObject.SetActive(true);
        }
        ScoreTextTop.gameObject.SetActive(true);        
        ScoreText.text = gameManager.score.ToString("D10");
    }

    

}
