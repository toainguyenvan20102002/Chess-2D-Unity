using UnityEngine;
using Const;
using Type;

public class CreateBoardGame : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;


    public void CreateBoard()
    {
        for (int row = 0; row < ConstSize.SIZE_BOARD_GAME; row++)
        {
            for(int col = 0; col < ConstSize.SIZE_BOARD_GAME; col++)
            {
                bool isWhite = ((row + col) % 2 == 0);
                Vector2 tilePosition = new Vector2(col * 2, -2 * row);

                var tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);

                tile.name = $"Tile {row} {col}";

                tile.transform.parent = transform;

                tile.GetComponent<Tile>().Init((isWhite == true) ? EColor.WHITE : EColor.BLACK);
            }
        }

        this.transform.position = new Vector2(-7, 7);
    }

    private void Start()
    {
        CreateBoard();
    }
}
