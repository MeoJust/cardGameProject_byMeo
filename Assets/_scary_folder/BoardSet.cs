using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardSet : MonoBehaviour
{
    [SerializeField] private List<Board> _boards;

    private GameObject _currentLayout;

    ////////////loads choosen card deck
    public void LoadDeck(CardsDeck cardsDeck)
    {
        Board board = _boards.Find(x => x.CardsDeck == cardsDeck);
        _currentLayout = Instantiate(board.Prefab);
    }

    [Serializable]
    private class Board
    {
        public CardsDeck CardsDeck;
        public GameObject Prefab;
    }
}

///////////Variants of decks with 4, 6 and 9 cards
public enum CardsDeck
{
    NONE = 0,

    board4 = 4,
    board6 = 6,
    board9 = 9
}
