using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _helpMenu;
    [SerializeField] private TMP_Text _playerHealthLabel;
    [SerializeField] private TMP_Text _gameStateLabel;
    [SerializeField] private Button _nextWaveButton;
    [SerializeField] private Button _restartGameButton;
    [Space(20)]
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawner _enemySpawner;

    private void OnEnable()
    {
        _player.HealthChanged += OnPlayerHealthChanged;
        _enemySpawner.NextWaveSpawned += OnNextWaveSpawned;
        _enemySpawner.WaveEnded += OnWaveEnded;
        _enemySpawner.GameWon += OnGameWon;
        _player.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnPlayerHealthChanged;
        _enemySpawner.NextWaveSpawned -= OnNextWaveSpawned;
        _enemySpawner.WaveEnded -= OnWaveEnded;
        _enemySpawner.GameWon -= OnGameWon;
        _player.Died += OnPlayerDied;
    }

    public void OnNextWaveButtonClicked()
    {
        _enemySpawner.SpawnNextWave();
        _nextWaveButton.interactable = false;
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
    
    public void OpenHelpMenu() 
    {
        _helpMenu.SetActive(!_helpMenu.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0; 
    }

    private void OnPlayerDied()
    {
        _gameStateLabel.text = $"Game Over";
        Time.timeScale = 0;
        _restartGameButton.gameObject.SetActive(true);
    }

    private void OnGameWon() 
    {
        _gameStateLabel.text = $"Happy End";
        _restartGameButton.gameObject.SetActive(true);
    }

    private void OnWaveEnded()
    {
        _nextWaveButton.interactable = true;
    }

    private void OnNextWaveSpawned(int currentWaveIndex)
    {
        _gameStateLabel.text = $"Wave {currentWaveIndex + 1}";
    }

    private void OnPlayerHealthChanged(int health)
    {
        _playerHealthLabel.text = $"Health: {health}";
    }
}
