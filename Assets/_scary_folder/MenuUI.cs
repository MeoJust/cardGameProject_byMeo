using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleTXT;
    [SerializeField] TextMeshProUGUI meoTXT;

    [SerializeField] private Button _deck4BTN;
    [SerializeField] private Button _deck6BTN;
    [SerializeField] private Button _deck9BTN;

    [SerializeField] Button easyBTN;
    [SerializeField] Button normalBTN;
    [SerializeField] Button hardBTN;

    [SerializeField] Button exitBTN;

    float normScale = 1.0f;
    float choseScale = 1.2f;

    private void Start()
    {
        titleTXT.transform.position -= Vector3.right * 50;

        _deck4BTN.onClick.AddListener(SetBoard4);
        _deck6BTN.onClick.AddListener(SetBoard6);
        _deck9BTN.onClick.AddListener(SetBoard9);

        easyBTN.onClick.AddListener(EasyBTNPressed);
        normalBTN.onClick.AddListener(NormalBTNPressed);
        hardBTN.onClick.AddListener(HardBTNPressed);

        exitBTN.onClick.AddListener(ExitGame);

        titleTXT.transform.DOMoveX(transform.position.x + 50, 2f).SetLoops(-1, LoopType.Yoyo);
        meoTXT.transform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void SetBoard4()
    {
        GameData.instance.SetCardsDeck(CardsDeck.board4);

        ButtonPressed(_deck4BTN, choseScale, Color.white);
        ButtonPressed(_deck6BTN, normScale, Color.yellow);
        ButtonPressed(_deck9BTN, normScale, Color.yellow);
    }

    private void SetBoard6()
    {
        GameData.instance.SetCardsDeck(CardsDeck.board6);

        ButtonPressed(_deck4BTN, normScale, Color.yellow);
        ButtonPressed(_deck6BTN, choseScale, Color.white);
        ButtonPressed(_deck9BTN, normScale, Color.yellow);
    }

    private void SetBoard9()
    {
        GameData.instance.SetCardsDeck(CardsDeck.board9);

        ButtonPressed(_deck4BTN, normScale, Color.yellow);
        ButtonPressed(_deck6BTN, normScale, Color.yellow);
        ButtonPressed(_deck9BTN, choseScale, Color.white);
    }

    private void SetDifficulty(int value1, int value2)
    {
        GameData.instance.SetMinEnemyAttack(value1);
        GameData.instance.SetMaxEnemyAttack(value2);
    }

    private void EasyBTNPressed()
    {
        SetDifficulty(1, 10);

        ButtonPressed(easyBTN, choseScale, Color.white);
        ButtonPressed(normalBTN, normScale, Color.black);
        ButtonPressed(hardBTN, normScale, Color.black);
    }

    private void NormalBTNPressed()
    {
        SetDifficulty(10, 20);

        ButtonPressed(easyBTN, normScale, Color.black);
        ButtonPressed(normalBTN, choseScale, Color.white);
        ButtonPressed(hardBTN, normScale, Color.black);
    }

    private void HardBTNPressed()
    {
        SetDifficulty(20, 30);

        ButtonPressed(easyBTN, normScale, Color.black);
        ButtonPressed(normalBTN, normScale, Color.black);
        ButtonPressed(hardBTN, choseScale, Color.white);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ButtonPressed(Button button, float scale, Color color)
    {
        button.transform.DOScale(scale, 0.2f).SetLoops(1);
        button.GetComponentInChildren<TextMeshProUGUI>().color = color;
    }
}
