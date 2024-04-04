using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] squares;
    [SerializeField] Sprite[] shapes;

    private int turn = 0;
    private int[] board = {-1,-1,-1, -1, -1, -1, -1, -1, -1};

    public void PlayerSelect(int index)
    {
        if (board[index] == -1)
        {
            putSquare(index, turn);
            turn++;
            turn %= 2;
        }
    }
    private int detectEnd()
    {
        int[][] winningConditions = new int[][] {
        new int[] { 0, 1, 2 },
        new int[] { 3, 4, 5 },
        new int[] { 6, 7, 8 },
        new int[] { 0, 3, 6 },
        new int[] { 1, 4, 7 },
        new int[] { 2, 5, 8 },
        new int[] { 0, 4, 8 },
        new int[] { 2, 4, 6 }
        };

        foreach (int[] condition in winningConditions)
        {
            if (board[condition[0]] == board[condition[1]] &&
                board[condition[1]] == board[condition[2]])
            {
                return board[condition[0]];
            }
        }

        if (!board.Any(square => square == -1))
        {
            return 2;
        }

        return -1;
    }

    private void putSquare(int index, int shape)
    {
        board[index] = shape;
        squares[index].gameObject.GetComponent<SpriteRenderer>().sprite = shapes[shape];

        int endCondition = detectEnd();
;       if (endCondition != -1)
        {
            Debug.Log(endCondition + " wins!");
        }
    }
}
