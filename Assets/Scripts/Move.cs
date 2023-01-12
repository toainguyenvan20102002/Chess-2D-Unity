using System;
using Type;
using System.Collections.Generic;
using UnityEngine;

class Move
{
    public static List<Tuple<Tuple<int,int>,EStateTile>> ChessMove(int maxDistance, int directRow, int directCol,
                            Tuple<int, int> indexes, Tile[,] tilesBoard)
    {
        List<Tuple<Tuple<int,int>,EStateTile>> listIndexAndStateMove = new List<Tuple<Tuple<int,int>,EStateTile>>();
        for (int offset = 1; offset <= maxDistance; offset++)
        {
            int row = indexes.Item1 + offset * directRow;
            int col = indexes.Item2 + offset * directCol;

            if (!CheckValidIndex(row, col)) return listIndexAndStateMove;

            if (tilesBoard[row, col].GetChessManOfTile() != null) return listIndexAndStateMove;
            tilesBoard[row, col].SetStateForTile(EStateTile.CAN_MOVE);
            listIndexAndStateMove.Add(Tuple.Create(Tuple.Create(row, col), EStateTile.CAN_MOVE));
        }
        return listIndexAndStateMove;
    }

    public static List<Tuple<Tuple<int, int>, EStateTile>> ChessAttack(EColor colorPlayer, int maxDistance, int directRow, int directCol,
                            Tuple<int, int> indexes, Tile[,] tilesBoard)
    {
        List<Tuple<Tuple<int, int>, EStateTile>> listIndexAndStateAttack = new List<Tuple<Tuple<int, int>, EStateTile>>();

        for (int offset = 1; offset <= maxDistance; offset++)
        {
            int row = indexes.Item1 + offset * directRow;
            int col = indexes.Item2 + offset * directCol;

            if (!CheckValidIndex(row, col)) return listIndexAndStateAttack;

            if (tilesBoard[row, col].GetChessManOfTile())
            {
                if (tilesBoard[row, col].GetChessManOfTile().GetColorOfChessMan() != colorPlayer)
                {
                    tilesBoard[row, col].SetStateForTile(EStateTile.BE_ATTACKED);
                    listIndexAndStateAttack.Add(Tuple.Create(Tuple.Create(row, col), EStateTile.BE_ATTACKED));
                }
                return listIndexAndStateAttack;
            }
        }
        return listIndexAndStateAttack;
    }

    public static List<Tuple<Tuple<int, int>, EStateTile>> KnightMove(Tuple<int, int> indexes, Tile[,] tilesBoard)
    {
        List<Tuple<Tuple<int, int>, EStateTile>> listIndexAndStateMove = new List<Tuple<Tuple<int, int>, EStateTile>>();
        int[] rowOffset = { -1, -2, -2, -1, 1, 2, 2, 1 };
        int[] colOffset = { -2, -1, 1, 2, 2, 1, -1, -2 };

        for (int i = 0; i < 8; i++)
        {
            int row = indexes.Item1 + rowOffset[i];
            int col = indexes.Item2 + colOffset[i];

            if (!CheckValidIndex(row, col)) continue;

            if (tilesBoard[row, col].GetChessManOfTile() == null)
            {
                tilesBoard[row, col].SetStateForTile(EStateTile.CAN_MOVE) ;
                listIndexAndStateMove.Add(Tuple.Create(Tuple.Create(row, col), EStateTile.CAN_MOVE));

            }
        }

        return listIndexAndStateMove;
    }

    public static List<Tuple<Tuple<int, int>, EStateTile>> KnightAttack(EColor colorPlayer, Tuple<int, int> indexes, Tile[,] tilesBoard)
    {
        List<Tuple<Tuple<int, int>, EStateTile>> listIndexAndStateAttack = new List<Tuple<Tuple<int, int>, EStateTile>>();

        int[] rowOffset = { -1, -2, -2, -1, 1, 2, 2, 1 };
        int[] colOffset = { -2, -1, 1, 2, 2, 1, -1, -2 };

        for (int i = 0; i < 8; i++)
        {
            int row = indexes.Item1 + rowOffset[i];
            int col = indexes.Item2 + colOffset[i];

            if (!CheckValidIndex(row, col)) continue;

            if (tilesBoard[row, col].GetChessManOfTile() != null)
            {
                if (tilesBoard[row, col].GetChessManOfTile().GetColorOfChessMan() != colorPlayer)
                {
                    tilesBoard[row, col].SetStateForTile(EStateTile.BE_ATTACKED);
                    listIndexAndStateAttack.Add(Tuple.Create(Tuple.Create(row, col), EStateTile.BE_ATTACKED));

                }
            }
        }

        return listIndexAndStateAttack;
    }

    private static bool CheckValidIndex(int row, int col)
    {
        return (row < Const.ConstSize.SIZE_BOARD_GAME &&
                col < Const.ConstSize.SIZE_BOARD_GAME &&
                row >= 0 && col >= 0);
    }


}
