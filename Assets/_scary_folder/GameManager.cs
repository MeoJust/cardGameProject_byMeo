using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Enemy enemy;
    Player player;

    bool isEnemyDead;

    BoardSet boardSet;

    private List<Cards> _cards;

    [SerializeField] GameObject hitPanel;

    [SerializeField] List<ParticleSystem> enemyHitPart;
    [SerializeField] ParticleSystem enemyDeathPart;

    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private List<Sprite> _bonusSprites;

    [SerializeField] TextMeshProUGUI playerHealthTXT;
    [SerializeField] TextMeshProUGUI enemyHealthTXT;

    [SerializeField] TextMeshProUGUI winTXT;
    [SerializeField] TextMeshProUGUI loseTXT;

    [SerializeField] AudioSource cameraSourseSFX;

    [SerializeField] AudioClip enemyHitSFX;
    [SerializeField] AudioClip enemyAttackSFX;
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] AudioClip enemyLaughtSFX;

    //////////Kostil for re-instantiate card board
    private int playerMoves;
    private int playerMovesStart;

    private Cards _card01, _card02, _bonusCard;

    bool isFeather;
    bool isSkull;
    bool isShield;

    private void Awake()
    {
        isEnemyDead = false;

        boardSet = FindObjectOfType<BoardSet>();
        PrepSpriteList();
        InstantiateNewBoard();

        enemy = FindObjectOfType<Enemy>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        PlayerHealthTXTUpdate();
        EnemyHealthTXTUpdate();
        winTXT.enabled = false;
        loseTXT.enabled = false;
    }

    private void PlayerHealthTXTUpdate()
    {
        playerHealthTXT.text = "player health: " + player.GetHealth().ToString();
        LoseDaGame();
    }

    ////////////////Set the animation of cards apperance
    private void CardsInitAnim()
    {

            //////////////Disales gameObjects for all cards
            foreach (var card in _cards)
            {
                card.gameObject.SetActive(false);
            }
            //////////////Enables gameObjects for each card with delay
            StartCoroutine(cardsInitAnim());
            IEnumerator cardsInitAnim()
            {
                foreach (var card in _cards)
                {
                    yield return new WaitForSeconds(0.1f);
                    card.gameObject.SetActive(true);
                }
            }

    }

    private void InstantiateNewBoard()
    {
        if (!isEnemyDead)
        {
            boardSet.LoadDeck(GameData.instance.GetCardsDeck());

            _cards = new List<Cards>(FindObjectsOfType<Cards>());
            CardInit();
            CardsInitAnim();
        }
    }

    ///////////Remove two random sprites from _sorites List
    private void PrepSpriteList()
    {
        int matchCount = 0;
        switch (GameData.instance.GetCardsDeck())
        {
            case CardsDeck.board4:
                matchCount = 2;
                break;
            case CardsDeck.board6:
                matchCount = 3;
                break;
            case CardsDeck.board9:
                matchCount = 4;
                break;
        }

        playerMovesStart = matchCount;
        playerMoves = playerMovesStart;

        while (_sprites.Count > matchCount)
        {
            _sprites.RemoveAt(Random.Range(0, _sprites.Count));
        }
    }

    private void CardInit()
    {   ////////////Create copy of _cards Array
        List<Cards> cardsEmpty = new List<Cards>(_cards);

        ////////Accept each sprite to two random cards
        foreach (var sprite in _sprites)
        {
            for (int i = 0; i < 2; i++)
            {
                Cards cardRandom = cardsEmpty[Random.Range(0, cardsEmpty.Count)];
                cardRandom.Init(sprite);
                cardsEmpty.Remove(cardRandom);
            }
        }
        /////////Assign the value of bonus card
        if (cardsEmpty.Count > 0)
        {
            _bonusCard = cardsEmpty[0];
            int randomizrer = Random.Range(0, 2);
            _bonusCard.Init(_bonusSprites[randomizrer]);
        }
    }

    private void UpdateBoard()
    {
        ///////////if two cards have same sprite, they both destroyed
        if (CompareCards(_card01, _card02))
        {
            DestroySameCards();
            playerMoves--;
            if (playerMoves <= 0)
            {
                /////////////Reload board if all cards destroyed
                playerMoves = playerMovesStart;
                /////////////Delays re-initialiation of new board for 0.6f
                Invoke("InstantiateNewBoard", 0.6f);
            }
        }
    }

    ///////////Checks if two cards are the same. And if they do, destroy them
    public void TryOpenedCards(Cards card)
    {      
        if (card == _bonusCard)
            OpenBonus();
        else
            SaveOpenCard(card);        
        UpdateBoard();
    }

    private void OpenBonus()
    {
        Destroy(_bonusCard.gameObject, 0.5f);

        ///////////////Get names of bonus card's sprites
        string spriteBonusName = _bonusCard.GetValue().name;
        if (spriteBonusName == "_card_bonus_icons_2")
        {
            isFeather = true;
        }
        else if (spriteBonusName == "_card_bonus_icons_0")
        {
            isSkull = true;
        }
        else 
            print("!!CHETA_GLUCHIT!!");
    }

    private void SaveOpenCard(Cards card)
    {
        if (_card01 == null)
        {
            _card01 = card;
            _card01.enabled = false;
        }
        else if (_card02 == null)
        {
            _card02 = card;
            _card02.enabled = false;
        }
        else
        {
            _card01.Closed();
            _card01.enabled = true;
            _card01 = card;
            _card01.enabled = false;

            _card02.Closed();
            _card02.enabled = true;
            _card02 = null;
        }
    }

    private bool CompareCards(Cards card1, Cards card2)
    {
        if (_card01 == null || _card02 == null)
            return false;
        /////////Checks if two cards have same sprite
        if (card1.GetValue() != card2.GetValue())
        {
            StartCoroutine(clickOff());
            StartCoroutine(EnemyAttack());
        }

        return card1.GetValue() == card2.GetValue();
    }

    ///////////////Didn't allow player to clik cards while enemy turn
    IEnumerator clickOff()
    {
        CardClickAlow(false);
        yield return new WaitForSeconds(1f);
        CardClickAlow(true);
    }

    //////////////////Disables/Enables player's abillity to click on cards
    private void CardClickAlow(bool statement)
    {
        foreach (var card in _cards)
        {
            if (card != null)
                card.GetComponent<BoxCollider2D>().enabled = statement;
        }
    }

    private void DestroySameCards()
    {
        SpritesAttacks();
        Destroy(_card01.gameObject, 0.5f);
        Destroy(_card02.gameObject, 0.5f);
    }

    private void EnemyHealthTXTUpdate()
    {
        enemyHealthTXT.text = "enemy health: " + enemy.GetHealth().ToString();
        WinDaGame();
    }

    IEnumerator EnemyHitAnim()
    {
        enemy.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        enemy.gameObject.transform.DOScale(0.8f, 0.2f);
        enemyHitPart[Random.Range(0, enemyHitPart.Capacity)].Play();
        yield return new WaitForSeconds(0.2f);
        enemy.gameObject.transform.DOScale(1f, 0.2f);
        enemy.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    private void SpritesAttacks()
    {
        ///////////////Get the names of each card's sprite
        string spriteName = _card01.GetValue().name;
        if (spriteName == "_cards_icons_0")
        {
            if (isFeather)
            {
                enemy.SehHealth(enemy.GetHealth() - 30);
                isFeather = false;
            }
            else
                enemy.SehHealth(enemy.GetHealth() - 20);
            cameraSourseSFX.PlayOneShot(enemyHitSFX);
            StartCoroutine(EnemyHitAnim());
            EnemyHealthTXTUpdate();
        }
        else if (spriteName == "_cards_icons_1")
        {
            if (isFeather)
            {
                enemy.SehHealth(enemy.GetHealth() - 40);
                isFeather = false;
            }
            else
                enemy.SehHealth(enemy.GetHealth() - 30);
            cameraSourseSFX.PlayOneShot(enemyHitSFX);
            StartCoroutine(EnemyHitAnim());
            EnemyHealthTXTUpdate();
        }
        else if (spriteName == "_cards_icons_2")
        {
            if (isFeather)
            {
                enemy.SehHealth(enemy.GetHealth() - 50);
                isFeather = false;
            }
            else
                enemy.SehHealth(enemy.GetHealth() - 40);
            cameraSourseSFX.PlayOneShot(enemyHitSFX);
            StartCoroutine(EnemyHitAnim());
            EnemyHealthTXTUpdate();
        }
        else if (spriteName == "_cards_icons_3")
        {
            isShield = true;
        }
        else if (spriteName == "_cards_icons_4")
        {
            player.SetHealth(player.GetHealth() + 20);
            PlayerHealthTXTUpdate();
        }
        else if (spriteName == "_cards_icons_5")
        {
            player.SetHealth(player.GetHealth() + 10);
            PlayerHealthTXTUpdate();
        }
        else
            print("!!!!!!!GLUK!!!!!!!");

    }

    private void WinDaGame()
    {
        if (enemy.GetHealth() <= 0)
        {
            isEnemyDead = true;

            winTXT.enabled = true;

            cameraSourseSFX.PlayOneShot(enemyDeathSFX);

            enemy.gameObject.transform.DOScaleX(0f, 1.2f);
            enemy.gameObject.transform.DOScaleY(0f, 0.8f);

            foreach (var card in _cards)
            {
                Destroy(card);
            }

            enemyDeathPart.Play();
        }
    }

    private void LoseDaGame()
    {
        if(player.GetHealth() <= 0)
        {
            loseTXT.enabled = true;

            cameraSourseSFX.PlayOneShot(enemyLaughtSFX);

            foreach (var card in _cards)
            {
                Destroy(card);
            }
        }
    }

    IEnumerator EnemyAttack()
    {
        enemy.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        enemy.gameObject.transform.DOScale(1.2f, 0.2f);
        if (isSkull && !isShield)
        {
            player.SetHealth(player.GetHealth() - GameData.instance.GetEnemyDamageRate() * 2);
            isSkull = false;
        }
        else if (!isSkull && isShield)
        {
            player.SetHealth(player.GetHealth() - GameData.instance.GetEnemyDamageRate() / 2);
            isShield = false;
        }
        else if (isSkull && isShield)
        {
            player.SetHealth(player.GetHealth() - GameData.instance.GetEnemyDamageRate());
            isShield = false;
            isSkull = false;
        }
        else
            player.SetHealth(player.GetHealth() - GameData.instance.GetEnemyDamageRate());
        PlayerHealthTXTUpdate();
        cameraSourseSFX.PlayOneShot(enemyAttackSFX);
        hitPanel.GetComponent<Image>().DOFade(1f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        hitPanel.GetComponent<Image>().DOFade(0f, 0.2f);
        enemy.gameObject.transform.DOScale(1f, 0.2f);
        enemy.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}
