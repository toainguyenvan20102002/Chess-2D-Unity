using UnityEngine;
using Type;
using System;

public class Tile : MonoBehaviour
{
    private EColor colorTile;

    private ChessMan currentChessMan;
    private EStateTile currentState;

    [SerializeField] private GameObject highlight;
    [SerializeField] private Color whiteColor;
    [SerializeField] private Color blackColor;

    private SpriteRenderer render;

    

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // INIT FOR TILE
    public void Init(EColor color)
    {
        colorTile = color;
        SetColor();
    }
    private void SetColor()
    {
        if (colorTile == EColor.WHITE) render.color = new Color(whiteColor.r, whiteColor.g, whiteColor.b, 255);
        else render.color = new Color(blackColor.r, blackColor.g, blackColor.b, 255);
    }

    #region SET _ GET
    public void SetChessManForTile(ChessMan curChess)
    {
        currentChessMan = curChess;
    }
    public ChessMan GetChessManOfTile()
    {
        return currentChessMan;
    }

    public EStateTile GetStateOfTile()
    {
        return currentState;
    }
    public void SetStateForTile(EStateTile state)
    {
        currentState = state;
    }

    public Tuple<int, int> GetIndexOfTile()
    {
        string name = transform.name;
        return Tuple.Create(int.Parse(name[5].ToString()), int.Parse(name[7].ToString()));
    }
    #endregion



    private void OnMouseDown()
    {
        // Ô hiện tại trống rỗng hoặc đã bị chọn hoặc chứa quân cờ khác màu mà k nằm trong ô có thể bị tấn công 
        {
            if (currentChessMan == null && currentState == EStateTile.NOTHING ||
            currentState == EStateTile.BE_CHOSEN ||
            (currentChessMan != null && GameManager.instance.GetCurrentPlayer().GetColorOfPlayer() !=
            currentChessMan.GetColorOfChessMan() && currentState != EStateTile.BE_ATTACKED)) return;
        }

        //Nếu ô hiện tại có thể di chuyển hoặc tấn công 
        if (currentState==EStateTile.CAN_MOVE || currentState == EStateTile.BE_ATTACKED)
        {
            GameObject beAttackChessMan = null;
            // Neus là tấn công thì 
            if(currentState == EStateTile.BE_ATTACKED)
            {
                beAttackChessMan = currentChessMan.gameObject;
                this.currentChessMan = null;
                beAttackChessMan.SetActive(false);
            }

            // Lấy ra ô đã chọn trước đó 
            Tile prevTile = GameManager.instance.GetTileBeChosenPrev();

            // Lấy quân cờ đã chọn trước đó 
            ChessMan prevChessMan = prevTile.GetChessManOfTile();

            // Hủy quân cờ mà ô prevTile nắm giữ 
            prevTile.SetChessManForTile(null);

            // Set currentChessMan cho ô hiện tại 
            this.currentChessMan = prevChessMan;
            prevChessMan.SetIndex(GetIndexOfTile());

            // Kiểm tra nếu sau khi di chuyển hoặc ăn 
            // thì có bị chiếu tướng hay không
            if(GameManager.instance.BeInCheck())  // Nếu bị chiếu 
            {
                // Trả lại trạng thái cũ cho các quân cờ 
                if(currentState == EStateTile.BE_ATTACKED)
                {
                    beAttackChessMan.SetActive(true);
                    this.currentChessMan = beAttackChessMan.GetComponent<ChessMan>();
                }
                else
                {
                    this.currentChessMan = null;
                }
                prevTile.SetChessManForTile(prevChessMan);
                prevChessMan.SetIndex(prevTile.GetIndexOfTile());
            }
            else
            {
                if (currentState == EStateTile.BE_ATTACKED)
                {
                    Debug.Log("Destroy");
                    Debug.Log(beAttackChessMan.GetComponent<ChessMan>().GetTypeOfChessMan());
                    Debug.Log(prevChessMan.GetTypeOfChessMan());
                    Debug.Log(beAttackChessMan.GetComponent<ChessMan>().GetIndexOfChessMan());
                    GameManager.instance.GetOppositePlayer().RemoveChess(beAttackChessMan.GetComponent<ChessMan>().GetIndexOfChessMan());
                    Destroy(beAttackChessMan);
                    currentChessMan = null;
                    
                }
                // Set vị trí mới cho quân cờ 
                prevChessMan.SetPositionOfChessMan(this.transform.position, GetIndexOfTile());
                currentChessMan = prevChessMan;
                // Di chuyển thành công -> Xóa state cho tất cả các ô và đổi lượt 
                GameManager.instance.ClearStateOfTile();
                GameManager.instance.ChangeTurn();
            }
            
        }
        // Nếu ô hiện tại chứa cờ cùng màu thì -> Chọn ô đó 
        else if (currentChessMan != null)
        {
            GameManager.instance.ClearStateOfTile();
            currentState = EStateTile.BE_CHOSEN;
            currentChessMan.StartCalculateMove(GameManager.instance.GetTilesBoard());
            currentChessMan.StartCalculateAttack(GameManager.instance.GetTilesBoard());
        }
        Debug.Log(currentChessMan.GetTypeOfChessMan());
    }

    private void Update()
    {
        if (highlight == null) return;
        if(currentState == EStateTile.NOTHING)
        {
            highlight.SetActive(false);
        }
        else
        {
            highlight.SetActive(true);
        }
    }
}
