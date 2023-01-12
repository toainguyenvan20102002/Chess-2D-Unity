namespace Type
{
    public enum EColor
    {
        WHITE,
        BLACK
    }

    public enum ETypeChess
    {
        PAWN = 0,
        ROOK = 1,
        KNIGHT = 2,
        BISHOP = 3,
        QUEEN = 4,
        KING = 5
    }

    public enum EStateTile
    {
        NOTHING,
        BE_CHOSEN,
        BE_ATTACKED,
        CAN_MOVE
    }
}
