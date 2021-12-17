using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    ///////////!!!!!!SINGLETON!!!!!!
    public static GameData instance;

    private CardsDeck _cardsDeck = CardsDeck.NONE;
    [SerializeField] private CardsDeck _defaultDeck = CardsDeck.board9;

    private int minEnemyAttack = 10;
    private int maxEnemyAttack = 20;

    private int enemyDamageRate;

    private void Awake()
    {      
        ///////////!!!!!!SINGLETON!!!!!!
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public CardsDeck GetCardsDeck()
    {
        /////////////sets deck9 if player don't choose anithing
        if (_cardsDeck == CardsDeck.NONE)
            return _defaultDeck;
        else
            return _cardsDeck;
    }

    public void SetCardsDeck(CardsDeck cardsDeck)
    {
        _cardsDeck = cardsDeck;
    }

    public int GetEnemyDamageRate()
    {
        return enemyDamageRate = Random.Range(minEnemyAttack, maxEnemyAttack);
    }

    public void SetMinEnemyAttack(int value)
    {
        minEnemyAttack = value;
    }

    public void SetMaxEnemyAttack(int value)
    {
        maxEnemyAttack = value;
    }
}
