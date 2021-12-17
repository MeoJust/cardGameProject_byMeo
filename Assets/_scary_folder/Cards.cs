using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cards : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _cardOpen;
    [SerializeField] private GameObject _cardClosed;
    [SerializeField] private SpriteRenderer _cardValue;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Closed();
    }
    ////////////////////Attaching sprites to sprites
    public void Init(Sprite sprite)
    {
        _cardValue.sprite = sprite;
    }

    public void Open()
    {
        _cardOpen.SetActive(true);
        _cardClosed.SetActive(false);

        gameManager.TryOpenedCards(this);
    }

    public void Closed()
    {
        _cardOpen.SetActive(false);
        _cardClosed.SetActive(true);
    }

    ////////////The sprite itself attached to card
    public Sprite GetValue()
    {
        return _cardValue.sprite;
    }

    ///////////////Register click on cards
    public void OnPointerClick(PointerEventData eventData)
    {
        Open();
    }
}
