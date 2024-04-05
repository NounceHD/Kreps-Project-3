using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] squares;
    [SerializeField] Sprite[] shapes;

    private int playerTurn = 0;
    private int aiTurn = 1;
    private int turn = 0;
    private int[] board = {-1,-1,-1, -1, -1, -1, -1, -1, -1};
    private bool inGame = true;

    public void PlayerSelect(int index)
    {
        if (board[index] == -1 && turn == playerTurn && inGame) PutSquare(index, playerTurn);
    }

    private void AISelect()
    {
        int bestMove = Minimax(board, turn, 6)[1];
        Debug.Log(bestMove);
        PutSquare(bestMove, aiTurn);
    }

    private int[] Minimax(int[] checkBoard, int checkTurn, int depth)
    {
        int endCondition = DetectEnd(checkBoard);
        if (endCondition == 2)
        {
            int[] score = { 0, -1 };
            return score;
        }
        if (endCondition == 1)
        {
            int[] score = { 1, -1 };
            return score;
        }
        if (endCondition == 0)
        {
            int[] score = { -1, -1 };
            return score;
        }
        if (depth == 0)
        {
            int[] score = { 0, -1 };
            return score;
        };

        int bestScore = (checkTurn == 0) ? 1 : -1;
        int bestMove = -1;

        int[] moves = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        moves = Shuffle(moves);

        foreach (int move in moves)
        {
            if (checkBoard[move] == -1)
            {
                if (bestMove == -1) bestMove = move;

                checkBoard[move] = checkTurn;
                int[] simulatedScore = Minimax(board, (checkTurn == 0) ? 1 : 0, depth - 1);
                checkBoard[move] = -1;

                if (checkTurn == 0)
                {
                    if (simulatedScore[0] < bestScore) bestMove = move;
                    bestScore = Mathf.Min(bestScore, simulatedScore[0]);
                } else
                {
                    if (simulatedScore[0] > bestScore) bestMove = move;
                    bestScore = Mathf.Max(bestScore, simulatedScore[0]);
                }
            }
        }

        int[] result = { bestScore, bestMove };
        return result;
    }

    private int[] Shuffle(int[] array)
    {
        int index = array.Length;
        while (index > 1)
        {
            int random = Random.Range(0,index--);
            (array[index], array[random]) = (array[random], array[index]);
        }

        return array;
    }

    private int DetectEnd(int[] checkBoard)
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
            if (checkBoard[condition[0]] == checkBoard[condition[1]] &&
                checkBoard[condition[1]] == checkBoard[condition[2]] &&
                checkBoard[condition[0]] != -1)
            {
                return checkBoard[condition[0]];
            }
        }

        if (!checkBoard.Any(square => square == -1)) return 2;

        return -1;
    }

    private void PutSquare(int index, int shape)
    {
        board[index] = shape;
        squares[index].gameObject.GetComponent<SpriteRenderer>().sprite = shapes[shape];
        turn++;
        turn %= 2;

        int endCondition = DetectEnd(board);
;       if (endCondition != -1)
        {
            inGame = false;
            Debug.Log(endCondition + " wins!");
            ResetGame();
        }

        if (inGame && turn == aiTurn) AISelect();
    }

    private void ResetGame()
    {
        Debug.Log("ResetGame");
        for (int index = 0; index < 8; index++)
        {
            squares[index].gameObject.GetComponent<SpriteRenderer>().sprite = null;
            board[index] = -1;
        }
        turn = 0;
        inGame = true;
    }
}
