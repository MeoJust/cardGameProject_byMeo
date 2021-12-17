using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ScenesSwitcher : MonoBehaviour
{
    [SerializeField] private ScenesNames sceneName;

    [SerializeField] private Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(GoToScene);

        _playButton.transform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Restart);
    }

    private void GoToScene()
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
}

public enum ScenesNames
{
    Level01,
    Menu
}
