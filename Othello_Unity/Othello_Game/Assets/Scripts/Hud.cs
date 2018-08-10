using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{

    [SerializeField] private Text GameOver_text;
    [SerializeField] private Text White_Win_Text;
    [SerializeField] private Text Black_Win_Text;
    [SerializeField] private GameObject Reset_Button;

    public void GameOver(int White_Piece_Num, int Black_Piece_Num)
    {
        GameOver_text.text = "GAME OVER!";
        Reset_Button.SetActive(true);

        if (White_Piece_Num > Black_Piece_Num)
        {
            White_Win_Text.text = "White Win!" + White_Piece_Num;

        }
        else if (White_Piece_Num < Black_Piece_Num)
        {
            Black_Win_Text.text = "Black Win!" + Black_Piece_Num;

        }
        else if (White_Piece_Num == Black_Piece_Num)
        {
            White_Win_Text.text = "Draw!";

        }


    }

    public void Onclick()
    {
        //リセットボタンを押した処理
        SceneManager.LoadScene(0);
    }

}