using UnityEngine;


public enum CellState
{
    EMPTY,
    BLOCKED,
    AVAILABLE,
    CHIP
}


//Ќаименование цветов фишек в коде решено оставить
//в соответсвии с изначальной стилистикой задани€
//и не прив€зывать их к цветам спрайтов
public enum ChipColor
{
    NONE = -1,
    YELLOW = 0,
    ORANGE = 2,
    RED = 4,
}


public struct Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }


    public bool Equals(Position position)
    {
        return x == position.x && y == position.y;
    }
}

public class Cell : UnityEngine.UI.Button
{
    public CellState CellState;

    public ChipColor ChipColor;

    public Position position;

    public void updateCell(CellState state, ChipColor color = ChipColor.NONE,
        Sprite sprite = null)
    {
        if (state != CellState.EMPTY)
        {
            image.color = Color.white;
        }
        image.sprite = sprite;
        ChipColor = color;
        CellState = state;
        RedrawCell();
    }

    public void RedrawCell()
    {
        switch (CellState)
        {
            case CellState.EMPTY:
                image.color = new Color(0, 0, 0, 0);
                break;

            case CellState.BLOCKED:
                image.color = new Color(0, 0, 0, 0);
                break;
        }
    }
}





