using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameField _gameField;

    [SerializeField]
    private List<Sprite> _spriteList;

    private int _width = GameField.WIDTH;
    private int _height = GameField.HEIGHT;

    private bool isYellowComplete;
    private bool isOrangeComplete;
    private bool isRedComplete;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _showChipClip;

    [SerializeField]
    private AudioClip _columntCompleteClip;

    [SerializeField]
    private AudioClip _columnUncompleteClip;

    [SerializeField]
    private AudioClip _winClip;

    [SerializeField]
    private AudioClip _winMelodyClip;

    public static UnityEvent<bool> OnYellowComplete = new UnityEvent<bool>();
    public static UnityEvent<bool> OnOrangeComplete = new UnityEvent<bool>();
    public static UnityEvent<bool> OnRedComplete = new UnityEvent<bool>();
    public static UnityEvent OnGameOver = new UnityEvent();

    void Start()
    {
        InitButtons();
        GenerateChips();
        StirChips(500);
        StartCoroutine(ShowChips());
        GameField.OnProcessedClick.AddListener(CheckPuzzle);
    }

    public void InitButtons()
    {
        _gameField.buttons = new Cell[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Cell currentCell = _gameField.transform.GetChild(i * _height + j).GetComponent<Cell>();
                currentCell.position = new Position(i, j);
                _gameField.buttons[i, j] = currentCell;
                currentCell.onClick.AddListener(
                        () => _gameField.Button—lick(currentCell.position));
            }
        }
    }

    public void GenerateChips()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _gameField.buttons[i, j].image.enabled = false;
                switch (j)
                {
                    case 0:
                        _gameField.buttons[i, j].updateCell(CellState.CHIP,
                            ChipColor.YELLOW, _spriteList[0]);
                        break;
                    case 2:
                        _gameField.buttons[i, j].updateCell(CellState.CHIP,
                            ChipColor.ORANGE, _spriteList[1]);
                        break;
                    case 4:
                        _gameField.buttons[i, j].updateCell(CellState.CHIP,
                            ChipColor.RED, _spriteList[2]);
                        break;
                    default:
                        _gameField.buttons[i, j].updateCell(CellState.EMPTY);
                        break;
                }
            }
        }

        for (int i = 0; i < _width; i += 2)
        {
            for (int j = 1; j < _height; j += 2)
            {
                _gameField.buttons[i, j].updateCell(CellState.BLOCKED,
                    ChipColor.NONE);
                _gameField.buttons[i, j].interactable = false;
            }
        }
    }


    public void StirChips(int steps)
    {
        Position chipPosition;
        for (int i = 0; i < steps; i++)
        {
            chipPosition = getRandomChip();

            List<Position> availableCells =
                _gameField.getAvailableCells(chipPosition);

            while (availableCells.Count == 0)
            {
                chipPosition = getRandomChip();

                availableCells =
                    _gameField.getAvailableCells(chipPosition);
            }

            Position randomAvailableCell = availableCells[Random.Range(0, availableCells.Count)];


            _gameField.buttons[randomAvailableCell.x, randomAvailableCell.y].updateCell(
                CellState.CHIP, _gameField.buttons[chipPosition.x, chipPosition.y].ChipColor,
                _gameField.buttons[chipPosition.x, chipPosition.y].image.sprite);

            _gameField.buttons[chipPosition.x, chipPosition.y].updateCell(CellState.EMPTY);
        }
    }

    private Position getRandomChip()
    {
        Position position = getRandomPosition();
        while (_gameField.buttons[position.x, position.y].CellState == CellState.BLOCKED
            || _gameField.buttons[position.x, position.y].CellState != CellState.CHIP)
        {
            position = getRandomPosition();
        }
        return position;
    }

    private Position getRandomPosition()
    {
        int x = Random.Range(0, _width);
        int y = Random.Range(0, _height);
        return new Position(x, y);
    }


    private IEnumerator ShowChips()
    {
        yield return new WaitForSeconds(0.7f);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _gameField.buttons[x, y].image.enabled = true;
                if (_gameField.buttons[x, y].CellState == CellState.CHIP)
                {
                    _audioSource.pitch = Random.Range(0.8f, 1.2f);
                    _audioSource.PlayOneShot(_showChipClip);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(_winMelodyClip);
        OnGameOver.Invoke();
        yield return new WaitForSeconds(0.6f);
        _audioSource.pitch = 2f;
        _audioSource.PlayOneShot(_winMelodyClip);

    }


    private void CheckPuzzle()
    {
        bool isYellowCompletePrev = isYellowComplete;
        isYellowComplete = CheckColumn(ChipColor.YELLOW);

        bool isOrangeCompletePrev = isOrangeComplete;
        isOrangeComplete = CheckColumn(ChipColor.ORANGE);

        bool isRedCompletePrev = isRedComplete;
        isRedComplete = CheckColumn(ChipColor.RED);

        if ((!isRedCompletePrev && isRedComplete)
            || (!isYellowCompletePrev && isYellowComplete)
            || (!isOrangeCompletePrev && isOrangeComplete))
        {
            _audioSource.pitch = 1f;
            _audioSource.PlayOneShot(_columntCompleteClip);
        }

        if ((isRedCompletePrev && !isRedComplete)
            || (isYellowCompletePrev && !isYellowComplete)
            || (isOrangeCompletePrev && !isOrangeComplete))
        {
            _audioSource.pitch = 1f;
            _audioSource.PlayOneShot(_columnUncompleteClip);
        }

        if (isYellowComplete && isOrangeComplete && isRedComplete)
        {
            _audioSource.PlayOneShot(_winClip);
            StartCoroutine(EndGame());
        }
    }


    private bool CheckColumn(ChipColor color)
    {
        bool isColumnCompleted = true;
        for (var i = 0; i < _height; i++)
        {
            if (_gameField.buttons[i, (int)color].ChipColor
                != color)
            {
                isColumnCompleted = false;
                break;
            }
        }

        switch (color)
        {
            case ChipColor.YELLOW:
                OnYellowComplete.Invoke(isColumnCompleted);
                break;

            case ChipColor.ORANGE:
                OnOrangeComplete.Invoke(isColumnCompleted);
                break;

            case ChipColor.RED:
                OnRedComplete.Invoke(isColumnCompleted);
                break;
        }

        return isColumnCompleted;
    }
}
