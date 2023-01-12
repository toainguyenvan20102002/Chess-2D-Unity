using Const;
using System;
using Type;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Tile[,] tilesBoard = new Tile[ConstSize.SIZE_BOARD_GAME, ConstSize.SIZE_BOARD_GAME];

    [SerializeField] private Player whitePlayer, blackPlayer;

    private Player currentPlayer;

    #region GET _ SET
    public Tile[,] GetTilesBoard()
    {
        return tilesBoard;
    }
    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }
    public Player GetOppositePlayer()
    {
        if (currentPlayer == whitePlayer) return blackPlayer;
        return whitePlayer;
    }
    #endregion

    private void Start()
    {
        if(instance == null) instance = this;
        GetBoardGameMatrix();

        whitePlayer.Init(EColor.WHITE);
        blackPlayer.Init(EColor.BLACK);

        whitePlayer.CreateChessPiece(tilesBoard);
        blackPlayer.CreateChessPiece(tilesBoard);

        currentPlayer = whitePlayer;
        
    }

    private void GetBoardGameMatrix()
    {
        for (int row = 0; row < ConstSize.SIZE_BOARD_GAME; row++)
        {
            for (int col = 0; col < ConstSize.SIZE_BOARD_GAME; col++)
            {
                tilesBoard[row, col] = transform.GetChild(row * ConstSize.SIZE_BOARD_GAME + col).GetComponent<Tile>();
            }
        }
    }

    public void ClearStateOfTile()
    {
        for (int row = 0; row < ConstSize.SIZE_BOARD_GAME; row++)
        {
            for (int col = 0; col < ConstSize.SIZE_BOARD_GAME; col++)
            {
                tilesBoard[row, col].SetStateForTile(EStateTile.NOTHING);
            }
        }
    }

    public Tile GetTileBeChosenPrev()
    {
        for (int row = 0; row < ConstSize.SIZE_BOARD_GAME; row++)
        {
            for (int col = 0; col < ConstSize.SIZE_BOARD_GAME; col++)
            {
                if(tilesBoard[row, col].GetStateOfTile() == EStateTile.BE_CHOSEN)
                {
                    return tilesBoard[row, col];
                };
            }
        }
        return null;
    }

    public void ChangeTurn()
    {
        if (currentPlayer == whitePlayer) currentPlayer = blackPlayer;
        else currentPlayer = whitePlayer;

        if (CheckMate())
        {
            ControlUI.instance.NotifyWinner((currentPlayer == whitePlayer) ? EColor.BLACK : EColor.WHITE);
        }
    }

    private EStateTile[,] CopyStateFromTilesBoard()
    {
        EStateTile[,] statesCopy = new EStateTile[ConstSize.SIZE_BOARD_GAME,ConstSize.SIZE_BOARD_GAME];
        for (int row = 0; row < ConstSize.SIZE_BOARD_GAME; row++)
        {
            for (int col = 0; col < ConstSize.SIZE_BOARD_GAME; col++)
            {
                statesCopy[row, col] = tilesBoard[row, col].GetStateOfTile();
            }
        }

        return statesCopy;
    }
    private void PasteStateToTileBoard(EStateTile[,] statesBoard)
    {
        for (int row = 0; row < ConstSize.SIZE_BOARD_GAME; row++)
        {
            for (int col = 0; col < ConstSize.SIZE_BOARD_GAME; col++)
            {
                tilesBoard[row, col].SetStateForTile(statesBoard[row, col]);
            }
        }
    }

    public bool BeInCheck()
    {
        EStateTile[,] stateCopy = CopyStateFromTilesBoard();

        ClearStateOfTile();

        // Calculate Attack tile of All posite Chessman
        for(int row = 0; row < ConstSize.SIZE_BOARD_GAME; row++)
        {
            for(int col = 0; col < ConstSize.SIZE_BOARD_GAME; col++)
            {
                ChessMan chessMan = tilesBoard[row, col].GetChessManOfTile();
                if(chessMan != null && chessMan.GetColorOfChessMan() != currentPlayer.GetColorOfPlayer())
                {
                    chessMan.StartCalculateAttack(tilesBoard);
                }
            }
        }

        bool beCheckedMate = false;
        Tuple<int, int> indexKing = currentPlayer.GetIndexOfKing();

        if (tilesBoard[indexKing.Item1, indexKing.Item2].GetStateOfTile() == EStateTile.BE_ATTACKED) beCheckedMate = true;


        PasteStateToTileBoard(stateCopy);
        return beCheckedMate;

    }

    public bool CheckMate()
    {
        List<ChessMan> chessManList = currentPlayer.GetListChessMan();

        int numberOfTileCanMoveOrAttack = 0;
        foreach (var chessMan in chessManList)
        {
            Tuple<int, int> oldIndex = chessMan.GetIndexOfChessMan();

            // Get list index of tile chessMan can move or attack
            List<Tuple<Tuple<int, int>, EStateTile>> listIndex = chessMan.StartCalculateMove(tilesBoard);
            listIndex.AddRange(chessMan.StartCalculateAttack(tilesBoard));
           
            foreach (Tuple<Tuple<int, int>, EStateTile> item in listIndex)
            {
                int row = item.Item1.Item1, col = item.Item1.Item2;

                // Bat dau tinh so o co the di hoac tan cong
                GameObject beAttackChessMan = null;
                // Neu là t?n công thì 
                if (item.Item2 == EStateTile.BE_ATTACKED)
                {
                    beAttackChessMan =  tilesBoard[row,col].GetChessManOfTile().gameObject;
                    tilesBoard[row,col].SetChessManForTile(null);
                    beAttackChessMan.SetActive(false);
                }

                // Set null cho ô mà chessMan ?ang xét ??ng trên ?ó  
                tilesBoard[oldIndex.Item1,oldIndex.Item2].SetChessManForTile(null);

                // Set currentChessMan cho ô hi?n t?i 
                tilesBoard[row, col].SetChessManForTile(chessMan);
                chessMan.SetIndex(Tuple.Create(row, col));

                if (!BeInCheck())
                {
                    numberOfTileCanMoveOrAttack++;
                }

                // Tr? l?i tr?ng thái cho các quân c? 
                if (item.Item2 == EStateTile.BE_ATTACKED)
                {
                    beAttackChessMan.SetActive(true);
                    tilesBoard[row,col].SetChessManForTile(beAttackChessMan.GetComponent<ChessMan>());
                }
                else
                {
                    tilesBoard[row, col].SetChessManForTile(null);
                }
                tilesBoard[oldIndex.Item1, oldIndex.Item2].SetChessManForTile(chessMan);
                chessMan.SetIndex(oldIndex);

            }
        }

        ClearStateOfTile();
        return (numberOfTileCanMoveOrAttack == 0);
    }
}
