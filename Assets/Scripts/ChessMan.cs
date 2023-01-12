using UnityEngine;
using Type;
using System;
using System.Collections.Generic;
using System.Linq;

public class ChessMan : MonoBehaviour
{
    private EColor colorChess;
    private ETypeChess typeChess;
    private Tuple<int, int> indexPosition;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    #region SET - GET
    public ETypeChess GetTypeOfChessMan()
    {
        return typeChess;
    }
    public EColor GetColorOfChessMan()
    {
        return colorChess;
    }
    public Tuple<int, int> GetIndexOfChessMan()
    {
        return indexPosition;
    }
    public void SetPositionOfChessMan(Vector2 position, Tuple<int, int> index)
    {
        transform.position = position;
        indexPosition = index;
    }
    public void SetIndex(Tuple<int, int> index)
    {
        indexPosition = index;
    }
    #endregion




    public void Init(EColor color,ETypeChess type, Sprite render)
    {
        colorChess = color;
        typeChess = type;
        spriteRenderer.sprite = render;
    }



    private bool IsHadMove()
    {
        if (colorChess == EColor.BLACK) return !(indexPosition.Item1 == 1);
        return !(indexPosition.Item1 == 6);
    }

    public List<Tuple<Tuple<int, int>, EStateTile>> StartCalculateMove(Tile[,] tileBoards)
    {
        List<Tuple<Tuple<int, int>, EStateTile>> listIndexAndStateMove = new List<Tuple<Tuple<int, int>, EStateTile>>();
        switch (typeChess)
        {
            case ETypeChess.PAWN:
                {
                    int moveUp = 0;
                    {
                        if (colorChess == EColor.BLACK) moveUp = 1;
                        else moveUp = -1;
                    }

                    if (IsHadMove()) listIndexAndStateMove.AddRange(Move.ChessMove(1, moveUp, 0, indexPosition, tileBoards));
                    else listIndexAndStateMove.AddRange( Move.ChessMove(2, moveUp, 0, indexPosition, tileBoards));
                    break;
                }
                
            case ETypeChess.ROOK:
                {
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 1, 0, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 0, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, -1, 0, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 0, -1, indexPosition, tileBoards));
                    break;
                }
            case ETypeChess.KNIGHT:
                listIndexAndStateMove.AddRange(Move.KnightMove(indexPosition, tileBoards));
                break;
            case ETypeChess.BISHOP:
                {

                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 1, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 1, -1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, -1, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, -1, -1, indexPosition, tileBoards));

                    break;
                }
            case ETypeChess.QUEEN:
                {
                    // T?n công, di chuy?n ngang và d?c 
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 1, 0, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 0, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, -1, 0, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 0, -1, indexPosition, tileBoards));

                    // T?n công, di chuy?n chéo
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 1, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, 1, -1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, -1, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(7, -1, -1, indexPosition, tileBoards));
                    break;
                }
            case ETypeChess.KING:
                {

                    // T?n công, di chuy?n ngang và d?c 
                    listIndexAndStateMove.AddRange(Move.ChessMove(1, 1, 0, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(1, 0, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(1, -1, 0, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(1, 0, -1, indexPosition, tileBoards));


                    listIndexAndStateMove.AddRange(Move.ChessMove(1, 1, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(1, 1, -1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(1, -1, 1, indexPosition, tileBoards));
                    listIndexAndStateMove.AddRange(Move.ChessMove(1, -1, -1, indexPosition, tileBoards));
                    break;
                }
            default:
                break;
        }
        return listIndexAndStateMove;
    }
    public List<Tuple<Tuple<int, int>, EStateTile>> StartCalculateAttack(Tile[,] tileBoards)
    {
        List<Tuple<Tuple<int, int>, EStateTile>> listIndexAndStateAttack = new List<Tuple<Tuple<int, int>, EStateTile>>();

        switch (typeChess)
        {
            case ETypeChess.PAWN:
                {
                    int moveUp = 0, moveRight = 0;
                    {
                        if (colorChess == EColor.BLACK)
                        {
                            moveUp = 1;
                            moveRight = -1;
                        }
                        else
                        {
                            moveUp = -1;
                            moveRight = 1;
                        }
                    }

                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, moveUp, moveRight, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, moveUp, -1 * moveRight, indexPosition, tileBoards));

                    break;
                }

            case ETypeChess.ROOK:
                {
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 1, 0, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 0, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, -1, 0, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 0, -1, indexPosition, tileBoards));
                    break;
                }
            case ETypeChess.KNIGHT:
                listIndexAndStateAttack.AddRange(Move.KnightAttack(colorChess, indexPosition, tileBoards));
                break;
            case ETypeChess.BISHOP:
                {
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 1, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 1, -1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, -1, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, -1, -1, indexPosition, tileBoards));
                    break;
                }
            case ETypeChess.QUEEN:
                {
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 1, 0, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 0, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, -1, 0, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 0, -1, indexPosition, tileBoards));

                    // T?n công, di chuy?n chéo

                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 1, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, 1, -1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, -1, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 7, -1, -1, indexPosition, tileBoards));
                    break;
                }
            case ETypeChess.KING:
                {
                    // T?n công, di chuy?n ngang và d?c 

                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, 1, 0, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, 0, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, -1, 0, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, 0, -1, indexPosition, tileBoards));


                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, 1, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, 1, -1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, -1, 1, indexPosition, tileBoards));
                    listIndexAndStateAttack.AddRange(Move.ChessAttack(colorChess, 1, -1, -1, indexPosition, tileBoards));
                    break;
                }
            default:
                break;
        }
        return listIndexAndStateAttack;
    }
}
