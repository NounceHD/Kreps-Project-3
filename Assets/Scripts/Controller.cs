using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] squares;
    [SerializeField] Sprite x;
    [SerializeField] Sprite o;

    private int turn = 0;
    private int[] board = new int[9];

    public void SelectSquare(int index)
    {
        Debug.Log(board);
        if (turn == 0)
        {
            squares[index].gameObject.GetComponent<SpriteRenderer>().sprite = x;
            turn = 1;
        } else
        {
            squares[index].gameObject.GetComponent<SpriteRenderer>().sprite = o;
            turn = 0;
        }

    }
}
