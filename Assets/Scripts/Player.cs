using System;
using System.Collections.Generic;
using UnityEngine;
using Const;
using Type;

public class Player : MonoBehaviour
{
    private List<ChessMan> chessMans = new List<ChessMan> ();

    private EColor colorPlayer;

    [Header("Prefabs")]
    [SerializeField] private GameObject chessManPrefabs;

    [Header("WHITE SPRITE")]
    [SerializeField] private Sprite[] whiteSprites = new Sprite[6];

    [Header("BLACK SPRITE")]
    [SerializeField] private Sprite[] blackSprites = new Sprite[6];

    #region SET - GET
    public EColor GetColorOfPlayer()
    {
        return colorPlayer;
    }
    public List<ChessMan> GetListChessMan()
    {
        return chessMans;
    }
    #endregion

    public void RemoveChess(Tuple<int, int> index)
    {
        for(int i=0;i<chessMans.Count; i++)
        {
            if(chessMans[i].GetIndexOfChessMan() == index)
            {
                chessMans.RemoveAt(i);
                return;
            }
        }
    }

    public void Init(EColor color)
    {
        colorPlayer = color;
    }
    public void CreateChessMan(ETypeChess type, Transform parent, EColor clPlayer)
    {
        var chess = Instantiate(chessManPrefabs, Vector2.zero, Quaternion.identity);
        chess.transform.parent = parent;

        ChessMan chessManTemp = chess.GetComponent<ChessMan>();

        Sprite sprite;
        if (clPlayer == EColor.WHITE) sprite = whiteSprites[(int)type];
        else sprite = blackSprites[(int)type];

        chessManTemp.Init(clPlayer, type, sprite);
        chessMans.Add(chessManTemp);
    }

    public void CreateChessPiece(Tile[,] tilesBoard)
    {
        // Assign Sprites for Color
        Sprite[] tempSprites = new Sprite[whiteSprites.Length];

        if(colorPlayer == EColor.WHITE) whiteSprites.CopyTo(tempSprites,0);
        else blackSprites.CopyTo(tempSprites,0);

        // SPAWN PAWN
        for(int i = 0; i < ConstSize.SIZE_BOARD_GAME; i++)
        {
            CreateChessMan(ETypeChess.PAWN, this.transform, colorPlayer);
        }

        // SPAWN ROOK
        for(int i = 0; i < 2; i++)
        {
            CreateChessMan(ETypeChess.ROOK,this.transform,colorPlayer);
        }

        // SPAWN KNIGHT
        for (int i = 0; i < 2; i++)
        {
            CreateChessMan(ETypeChess.KNIGHT, this.transform, colorPlayer);
        }

        // SPAWN BISHOP
        for (int i = 0; i < 2; i++)
        {
            CreateChessMan(ETypeChess.BISHOP, this.transform, colorPlayer);
        }

        // SPAWN QUEEN
        CreateChessMan(ETypeChess.QUEEN, this.transform, colorPlayer);

        // SPAWN KING
        CreateChessMan(ETypeChess.KING, this.transform, colorPlayer);

        SetDefaultPositionForChessMan(tilesBoard);
    }

    private void SetDefaultPositionForChessMan(Tile[,] tilesBoard)
    {
        int pawnRowIndex, rookRowIndex, knightRowIndex, bishopRowIndex, queenRowIndex, kingRowIndex;
        if(colorPlayer == EColor.WHITE)
        {
            pawnRowIndex = 6;
            rookRowIndex = knightRowIndex = bishopRowIndex = queenRowIndex = kingRowIndex = 7;
        }
        else
        {
            pawnRowIndex = 1;
            rookRowIndex = knightRowIndex = bishopRowIndex = queenRowIndex = kingRowIndex = 0;
        }

        // Set Pawn
        for(int i = 0; i < ConstSize.SIZE_BOARD_GAME; i++)
        {
            chessMans[i].SetPositionOfChessMan(tilesBoard[pawnRowIndex, i].transform.position,Tuple.Create(pawnRowIndex,i));
            tilesBoard[pawnRowIndex, i].SetChessManForTile(chessMans[i]);
        }

        // Set Rook
        chessMans[8].SetPositionOfChessMan(tilesBoard[rookRowIndex, 0].transform.position, Tuple.Create(rookRowIndex, 0));
        chessMans[9].SetPositionOfChessMan(tilesBoard[rookRowIndex, 7].transform.position, Tuple.Create(rookRowIndex, 7));
        tilesBoard[rookRowIndex, 0].SetChessManForTile(chessMans[8]);
        tilesBoard[rookRowIndex, 7].SetChessManForTile(chessMans[9]);


        // Set Knight
        chessMans[10].SetPositionOfChessMan(tilesBoard[knightRowIndex, 1].transform.position, Tuple.Create(knightRowIndex, 1));
        chessMans[11].SetPositionOfChessMan(tilesBoard[knightRowIndex, 6].transform.position,Tuple.Create(knightRowIndex, 6));
        tilesBoard[knightRowIndex, 1].SetChessManForTile(chessMans[10]);
        tilesBoard[knightRowIndex, 6].SetChessManForTile(chessMans[11]);

        // Set bishop
        chessMans[12].SetPositionOfChessMan(tilesBoard[bishopRowIndex, 2].transform.position, Tuple.Create(bishopRowIndex, 2));
        chessMans[13].SetPositionOfChessMan(tilesBoard[bishopRowIndex, 5].transform.position, Tuple.Create(bishopRowIndex, 5));
        tilesBoard[bishopRowIndex, 2].SetChessManForTile(chessMans[12]);
        tilesBoard[bishopRowIndex, 5].SetChessManForTile(chessMans[13]);

        //Set Queen
        chessMans[14].SetPositionOfChessMan(tilesBoard[queenRowIndex, 3].transform.position, Tuple.Create(queenRowIndex, 3));
        tilesBoard[queenRowIndex, 3].SetChessManForTile(chessMans[14]);

        // Set King
        chessMans[15].SetPositionOfChessMan(tilesBoard[kingRowIndex, 4].transform.position, Tuple.Create(kingRowIndex, 4));
        tilesBoard[kingRowIndex, 4].SetChessManForTile(chessMans[15]);
    }

    public Tuple<int, int> GetIndexOfKing()
    {
        for(int i=0;i<chessMans.Count;i++)
        {
            if(chessMans[i].GetTypeOfChessMan() == ETypeChess.KING)
            {
                return chessMans[i].GetIndexOfChessMan();
            }
        }
        
        return Tuple.Create(-1, -1);
    }

    
}
