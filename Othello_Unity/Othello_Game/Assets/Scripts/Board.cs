using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject Grid_Prefab;
    [SerializeField] private Canvas Hud;
    [SerializeField] protected int Stone_Type;
    [SerializeField] protected int Turn_Num;
    [SerializeField] private float Delay_time = 0.1f;


    protected int[,] Grids = new int[10, 10];
    private bool Place_Flag;
    int Can_Put_Grid_Num;

    public void Awake()
    {
        Generate_Board();
        Update_Board();
    }

    //Gridの状態を受け取る
    public void Receive_Grid(int x, int y, int Piece_type)
    {
        Grids[x, y] = Piece_type;
    }

    //Gridに色を返す
    public int Check_Board(int x, int y)
    {
        return Grids[x, y];
    }



    //置けるかどうかを確認する
    public bool CanPutDown(int x, int y)
    {
        //オーバーロードで8方向探索
        if (CanPutDown(x, y, 1, 0))
            return true; // 右
        if (CanPutDown(x, y, 0, 1))
            return true; // 下
        if (CanPutDown(x, y, -1, 0))
            return true; // 左
        if (CanPutDown(x, y, 0, -1))
            return true; // 上
        if (CanPutDown(x, y, 1, 1))
            return true; // 右下
        if (CanPutDown(x, y, -1, -1))
            return true; // 左上
        if (CanPutDown(x, y, 1, -1))
            return true; // 右上
        if (CanPutDown(x, y, -1, 1))
            return true; // 左下

        // どの方向もだめな場合
        return false;

    }

    private bool CanPutDown(int x, int y, int VecX, int VecY)
    {
        var Turn_script = this.GetComponent<Turn>();
        Turn_Num = Turn_script.Get_Turn_Count();

        if (Turn_Num % 2 == 0)
        {
            Stone_Type = 1;
        }
        else
        {
            Stone_Type = 2;
        }


        x += VecX;
        y += VecY;


        //隣が同じ色か何もないとき
        if (Grids[x, y] == Stone_Type || Grids[x, y] == 0)
            return false;


        x += VecX;
        y += VecY;


        // となりに石がある間ループがまわる
        while (x > 0 && x < 10 && y > 0 && y < 10)
        {
            // 空白があったとき
            if (Grids[x, y] == 0)
                return false;
            // 打つ石と同じ石があるとき
            if (Grids[x, y] == Stone_Type)
                return true;

            x += VecX;
            y += VecY;
        }
        //いずれにも当てはまらないとき
        return false;
    }

    //ひっくり返す
    public void Reverse(int x, int y)
    {
        // ひっくり返せる石がある方向はすべてひっくり返す
        if (CanPutDown(x, y, 1, 0)) Reverse(x, y, 1, 0);
        if (CanPutDown(x, y, 0, 1)) Reverse(x, y, 0, 1);
        if (CanPutDown(x, y, -1, 0)) Reverse(x, y, -1, 0);
        if (CanPutDown(x, y, 0, -1)) Reverse(x, y, 0, -1);
        if (CanPutDown(x, y, 1, 1)) Reverse(x, y, 1, 1);
        if (CanPutDown(x, y, -1, -1)) Reverse(x, y, -1, -1);
        if (CanPutDown(x, y, 1, -1)) Reverse(x, y, 1, -1);
        if (CanPutDown(x, y, -1, 1)) Reverse(x, y, -1, 1);
    }

    private void Reverse(int x, int y, int VecX, int VecY)
    {
        int putStone;
        var Turn_script = this.GetComponent<Turn>();
        Turn_Num = Turn_script.Get_Turn_Count();
        if (Turn_Num % 2 == 0)
        {
            putStone = 1;
        }
        else
        {
            putStone = 2;
        }

        x += VecX;
        y += VecY;

        while (Grids[x, y] != putStone)
        {
            Grids[x, y] = putStone;

            x += VecX;
            y += VecY;
        }
    }


    //盤面の生成
    void Generate_Board()
    {
        var parent = this.transform;

        for (int x =1; x < 9; x++)
        {
            for (int y = 1; y < 9; y++)
            {
                var go = Instantiate(Grid_Prefab, new Vector3(x, 0, y), Quaternion.identity, parent);
                var grid = go.GetComponent<Grid>();
                grid.SetPosition(x: x, y: y);
            }
        }
    }

    //全てのGridが置けるかどうかを調べたものを貰う
    public void Board_Checker()
    {
        Can_Put_Grid_Num++;
    }



    //盤面の更新,ゲーム終了か確認
    public void Update_Board()
    {
        Can_Put_Grid_Num = 0;
        GameObject[] Pieces = GameObject.FindGameObjectsWithTag("Piece");
        GameObject[] Grids = GameObject.FindGameObjectsWithTag("Grid");

        int Step_Piece = 0;
        foreach (var Grid in Grids)
        {
            var Grid_Script = Grid.GetComponent<Grid>();
            Grid_Script.Check_Grid();
        }
        foreach (var Piece in Pieces)
        {
            var Piece_Script = Piece.GetComponent<Piece>();
            Piece_Script.Check_Color();
            Step_Piece++;
        }
        if (Can_Put_Grid_Num == 0 && Step_Piece<64)
        {
            Debug.Log("置けるところがありません！相手にターンが渡ります。");
            var Turn_Script = this.GetComponent<Turn>();
            Turn_Script.Step();
            Update_Board();
        }

        if (Step_Piece >= 64)
        {

            Game_Over();
        }
    }

    //ゲーム終了・駒の取得
    private void Game_Over()
    {
        int White_Piece_Num = 0;
        int Black_Piece_Num = 0;
        for (int x = 1; x < 9; x++)
        {
            for (int y = 1; y < 9; y++)
            {
                if (Grids[x, y] == 1)
                {
                    White_Piece_Num++;
                }
                else if (Grids[x, y] == 2)
                {
                    Black_Piece_Num++;
                }

            }
        }

        var Hud_Scrip = Hud.GetComponent<Hud>();
        Hud_Scrip.GameOver(White_Piece_Num, Black_Piece_Num);
    }
}