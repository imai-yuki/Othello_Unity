using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] public Transform Posision_Root;
    [SerializeField] private Animator Position_Root_Animator;
    [SerializeField] private Animator Rotation_Root_Animator;

    private int Piece_type;
    private int Cash_Piece_type = 0;
    public void Check_Color()
    {
        var Grid_Script = gameObject.transform.parent.GetComponent<Grid>();
        Piece_type = Grid_Script.Receive_Piece_type();
        //変化があったときのみアニメーションさせる
        if (Cash_Piece_type != Piece_type)
        {
            Position_Root_Animator.SetTrigger("Roll_Piece_Trigger");
            Rotation_Root_Animator.SetTrigger("Roll_Piece_Trigger");
        }
        if (Piece_type == 2)
        {
            Posision_Root.transform.rotation = Quaternion.Euler(0, 0, 180);
            Cash_Piece_type = 2;
        }
        else if (Piece_type == 1)
        {
            Posision_Root.transform.rotation = Quaternion.Euler(0, 0, 0);
            Cash_Piece_type = 1;
        }
    }
    public void Set_Rotation(int Piece_type)
    {
        if (Piece_type == 2)
        {
            Posision_Root.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Piece_type == 1)
        {
            Posision_Root.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
