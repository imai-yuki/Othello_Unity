using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid : MonoBehaviour
{
    [SerializeField] protected GameObject Board;
    [SerializeField] private GameObject Piece_Prefab;
    [SerializeField] private GameObject Point_Light;

    private enum Piece_Type_enum
    {
        None = 0,
        White = 1,
        Black = 2
    }
    private int Piece_type;
    protected int Grid_x;
    protected int Grid_y;

    void Awake()
    {
        Board = gameObject.transform.parent.gameObject;

    }

    //子のPieceに色の状態を渡す
    public int Receive_Piece_type()
    {
        return Piece_type;
    }

    //Boardに今のGridの状態を確認
    public void Check_Grid()
    {
        var Boad_Script = Board.GetComponent<Board>();
        Piece_type = Boad_Script.Check_Board(x: Grid_x, y: Grid_y);

        //置けるなら盤面を光らせる
        if (Boad_Script.CanPutDown(x: Grid_x, y: Grid_y) && Piece_type == 0)
        {

            Point_Light.SetActive(true);
            Boad_Script.Board_Checker();
        }
        else
        {
            Point_Light.SetActive(false);
        }

    }

    //盤面の座標を取得
    public void SetPosition(int x, int y)
    {
        Grid_x = x;
        Grid_y = y;
        Piece_type = (int)Piece_Type_enum.None;

        //初期の4つを配置
        if (x == 4 && y == 4 || x == 5 && y == 5)
        {
            Place_Black_Piece();
        }
        if (x == 4 && y == 5 || x == 5 && y == 4)
        {
            Place_White_Piece();

        }
    }

    public void OnClick()
    {

        var Turn_script = Board.GetComponent<Turn>();
        var Board_script = Board.GetComponent<Board>();

        //Boardから置けるかどうかを取ってくる
        bool Place_Flag = Board_script.CanPutDown(x: Grid_x, y: Grid_y);


        //いずれかの駒が置ける時
        if (Place_Flag && Piece_type == 0)
        {
            Turn_script.Piece_Placer(Grid: this.gameObject);
            Board_script.Reverse(x: Grid_x, y: Grid_y);

            //盤面を更新

            Turn_script.Step();
            Board_script.Update_Board();


        }
        else
        {
            Debug.Log("ここにはおけません");
        }
    }

    public void Place_White_Piece()
    {
        var go = Instantiate(Piece_Prefab, new Vector3(this.transform.position.x, 0.2f, this.transform.position.z), Quaternion.identity, this.transform);
        var Piece_Script = go.GetComponent<Piece>();
        Piece_Script.Set_Rotation(Piece_type);

        Piece_type = (int)Piece_Type_enum.White;
        var Board_script = Board.GetComponent<Board>();
        Board_script.Receive_Grid(x: Grid_x, y: Grid_y, Piece_type: Piece_type);
    }

    public void Place_Black_Piece()
    {

        var go = Instantiate(Piece_Prefab, new Vector3(this.transform.position.x, 0.2f, this.transform.position.z), Quaternion.identity, this.transform);
        var Piece_Script = go.GetComponent<Piece>();
        Piece_Script.Set_Rotation(Piece_type);

        Piece_type = (int)Piece_Type_enum.Black;
        var Board_script = Board.GetComponent<Board>();
        Board_script.Receive_Grid(x: Grid_x, y: Grid_y, Piece_type: Piece_type);
    }

}