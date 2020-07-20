using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [Tooltip("Count of List elements is the count of Waves. Value of element is the quantity of enemies in one spawnPoint.")]
    [SerializeField] private List<int> _waves;
    [Space(20)]
    [SerializeField] private GameObject _zombiPrefab;

    private int _currentWaveIndex = 0;
    private int _waveSize;

    public Transform Player;
    public event UnityAction<int> NextWaveSpawned;
    public event UnityAction WaveEnded;
    public event UnityAction GameWon;

    public void SpawnNextWave() 
    {
        if (_currentWaveIndex >= _waves.Count)
            return;

        foreach (var sp in _spawnPoints)
        {
            for (int i = 0; i < _waves[_currentWaveIndex]; i++)
            {
                Zombi z = Instantiate(_zombiPrefab, sp).GetComponent<Zombi>();
                z.Died += OnZombiDied;
                z.GetComponent<ZombiController>().Target = Player;
            }
        }


        _waveSize = _waves[_currentWaveIndex] * _spawnPoints.Count;
        NextWaveSpawned?.Invoke(_currentWaveIndex);

        _currentWaveIndex++;
    }

    private void OnZombiDied(Zombi zombi)
    {
        zombi.Died -= OnZombiDied;

        _waveSize--;

        if (_waveSize == 0 && _currentWaveIndex >= _waves.Count)
            GameWon?.Invoke();
        else if (_waveSize == 0)
            WaveEnded?.Invoke();
    }
}
