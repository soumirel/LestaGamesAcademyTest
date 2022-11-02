using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GameField : MonoBehaviour
{
    public const int WIDTH = 5;
    public const int HEIGHT = 5;

    public Cell[,] buttons;

    bool isMoveTime = false;

    bool isMoveAvailable = false;

    Cell lastClickedCell;

    public static UnityEvent OnProcessedClick = new UnityEvent();

    [SerializeField]
    private Sprite _spriteAvailable;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _pickClip;

    [SerializeField]
    private AudioClip _placeClip;


    public void PlayClip(AudioClip audioClip)
    {
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(audioClip);
    }


    public void Button—lick(Position position)
    {
        Cell clickedCell = buttons[position.x, position.y];
        if (clickedCell.CellState == CellState.CHIP)
        {
            PlayClip(_pickClip);

            if (isMoveTime)
            {
                HideAvailableCells();
            }

            if (lastClickedCell
                && lastClickedCell.position.Equals(position) == true
                && isMoveTime == true)
            {
                isMoveTime = false;
                HideAvailableCells();
            }
            else
            {
                isMoveTime = true;
                ShowAvailableCells(position);
            }

            if (isMoveAvailable == false)
            {
                isMoveTime = false;
            }
        }

        else if (clickedCell.CellState == CellState.AVAILABLE)
        {
            PlayClip(_placeClip);

            clickedCell.updateCell(
                CellState.CHIP,
                lastClickedCell.ChipColor,
                lastClickedCell.image.sprite
                );

            lastClickedCell.updateCell(CellState.EMPTY);

            HideAvailableCells();

            isMoveTime = false;
            isMoveAvailable = false;
        }

        if (clickedCell.CellState == CellState.CHIP)
        {
            lastClickedCell = clickedCell;
        }

        OnProcessedClick.Invoke();
    }


    public List<Position> getAvailableCells(Position position)
    {
        return getAvailableCells(position, new List<Position>());
    }


    public List<Position> getAvailableCells(Position position, List<Position> availableCells)
    {
        Position[] toMove =
        {
            new Position(position.x - 1, position.y),
            new Position(position.x + 1, position.y),
            new Position(position.x, position.y - 1),
            new Position(position.x, position.y + 1)
        };

        bool isOnBorder;

        foreach (var p in toMove)
        {
            isOnBorder = p.x >= 0 && p.x < WIDTH && p.y >= 0 && p.y < HEIGHT;
            if (isOnBorder && buttons[p.x, p.y].CellState != CellState.BLOCKED)
            {
                if (buttons[p.x, p.y].CellState == CellState.EMPTY
                    && !availableCells.Contains(p))
                {
                    availableCells.Add(p);

                    if (isMoveAvailable == false) isMoveAvailable = true;

                    availableCells.AddRange(getAvailableCells(p, availableCells));
                }
            }
        }

        return availableCells;
    }


    public void ShowAvailableCells(Position position)
    {
        List<Position> availableCells = getAvailableCells(position);

        foreach (var p in availableCells)
        {
            buttons[p.x, p.y].updateCell(CellState.AVAILABLE,
                ChipColor.NONE, _spriteAvailable);
        }
    }


    public void HideAvailableCells()
    {
        foreach (var cell in buttons)
        {
            if (cell && cell.CellState == CellState.AVAILABLE)
            {
                cell.updateCell(CellState.EMPTY);
            }
        }
    }
}

