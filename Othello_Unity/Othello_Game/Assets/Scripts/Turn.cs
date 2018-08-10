using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{

    private bool Is_White_Turn;
    private bool Is_Piece_Grid;
    private int Turn_Count = 1;
    public GameObject White_Turn_Obj;
    public GameObject Black_Turn_Obj;

    public void Piece_checker(bool Is_Piece)
    {
        Is_Piece_Grid = Is_Piece;
    }

    public void Piece_Placer(GameObject Grid)
    {
        var Grid_Script = Grid.GetComponent<Grid>();
        if (Is_Piece_Grid == false && Is_White_Turn == false)
        {
            Grid_Script.Place_Black_Piece();
        }
        else if (Is_Piece_Grid == false && Is_White_Turn == true)
        {
            Grid_Script.Place_White_Piece();
        }
    }

    //ターンを増やす
    public void Step()
    {
        Turn_Count++;

        if (Turn_Count % 2 == 0)
        {
            White_Turn_Obj.SetActive(true);
            Black_Turn_Obj.SetActive(false);
            Is_White_Turn = true;
        }
        else
        {
            White_Turn_Obj.SetActive(false);
            Black_Turn_Obj.SetActive(true);
            Is_White_Turn = false;
        }
    }

    //現在のターン数を返す
    public int Get_Turn_Count()
    {
        return Turn_Count;
    }

}
