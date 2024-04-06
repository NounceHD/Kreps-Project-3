using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject[] squares;
    [SerializeField] Sprite[] shapes;
    [SerializeField] Canvas settingsCanvas;
    [SerializeField] Canvas endCanvas;
    [SerializeField] TMP_Text endStatus;

    private int playerTurn = 0;
    private int aiTurn = 1;
    private int turn = 0;
    private int[] board = {-1,-1,-1, -1, -1, -1, -1, -1, -1};
    private bool inGame = false;
    private int depth = 2;

    public void changeDifficulty(float difficulty)
    {
        if (difficulty == 0) depth = 2;
        if (difficulty == 1) depth = 4;
        if (difficulty == 2) depth = 6;
    }

    public void StartGame()
    {
        for (int index = 0; index < 9; index++)
        {
            squares[index].gameObject.GetComponent<SpriteRenderer>().sprite = null;
            board[index] = -1;
        }
        turn = 0;
        inGame = true;
        settingsCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);
    }

    public void PlayerSelect(int index)
    {
        if (board[index] == -1 && turn == playerTurn && inGame) StartCoroutine(PutSquare(index, playerTurn));
    }

    private IEnumerator AISelect()
    {
        yield return new WaitForSeconds(1);
        int bestMove = Minimax(board, turn, depth)[1];
        StartCoroutine(PutSquare(bestMove, aiTurn));
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

    private IEnumerator PutSquare(int index, int shape)
    {
        board[index] = shape;
        squares[index].gameObject.GetComponent<SpriteRenderer>().sprite = shapes[shape];
        turn++;
        turn %= 2;

        int endCondition = DetectEnd(board);
;       if (endCondition != -1)
        {
            inGame = false;
            if (endCondition == 0) endStatus.text = "X wins!";
            if (endCondition == 1) endStatus.text = "O wins!";
            if (endCondition == 2) endStatus.text = "Draw!";
            yield return new WaitForSeconds(1);
            endCanvas.gameObject.SetActive(true);
        }

        if (inGame && turn == aiTurn) StartCoroutine(AISelect());
    }
}
