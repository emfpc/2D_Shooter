using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("TextMeshProUGUI field for Score")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Player lives display section")]
    [SerializeField] private Image _liveImageHolder;
    [SerializeField] private List<Sprite> _livesSpritesList = new List<Sprite>();

    [Header("Game Over Panel section")]
    [SerializeField] private GameObject _gameOverPanelObj;
    [SerializeField] private GameObject _gameOverTextObj;
    private WaitForSeconds _waitForSecondsGameOverText;

    private void OnEnable()
    {
        Player.OnGetIsPlayerDead += GameOverPanel;
    }
    private void Start()
    {
        _waitForSecondsGameOverText = new WaitForSeconds(0.50f);
    }

    public void UpdatePlayerScore(int updatedScore)
    {
        _scoreText.text = "Score: " + updatedScore.ToString();
    }

    public void PlayerLivesDisplay(int livesId)
    {
        _liveImageHolder.sprite = _livesSpritesList[livesId];
    }

    public void GameOverPanel(bool gameOverPanelStatus)
    {
        _gameOverPanelObj.SetActive(gameOverPanelStatus);
        StartCoroutine(GameOverFlickingAnimation());
    }

    IEnumerator GameOverFlickingAnimation()
    {
        while (true)
        {
            yield return _waitForSecondsGameOverText;
            _gameOverTextObj.SetActive(true);
            yield return _waitForSecondsGameOverText;
            _gameOverTextObj.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Player.OnGetIsPlayerDead -= GameOverPanel;
    }
}
