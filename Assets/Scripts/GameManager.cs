using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    [HideInInspector] public UserInterface UI;
    [HideInInspector] public Transform Level;
    [HideInInspector] public Transform PlayerShip;

    public int WaveNumber = 0;
    public int WaveEnemyAmount = 3;
    public int RemainingBackupShips = 2;
    public int Score = 0;

    public List<CharacterController> Characters;

    private int m_remainingEnemies;

	void Awake()
    {
        Instance = this;
        UI = FindObjectOfType<UserInterface>();
        Level = GameObject.Find("Level").transform;
	}

    private void spawnPlayer()
    {
        PlayerShip = spawnCharacter(PlayerPrefab, new Vector2(0, 0), null).transform.Find("Ship");
    }

    private void spawnEnemies(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            // Get a random sector (excluding the middle sector that the player spawns in) to spawn the enemy in
            int _randomSectorIndex = Random.Range(0, Level.childCount);
            if (_randomSectorIndex == Mathf.CeilToInt(GameManager.Instance.Level.childCount / 2.0f) - 1) ++_randomSectorIndex;
            Transform _sectorToSpawnIn = Level.GetChild(_randomSectorIndex);
            Transform _enemy = spawnCharacter(EnemyPrefab, _sectorToSpawnIn.position, _sectorToSpawnIn).transform;

            _enemy.position = ProjectConstants.PickRandomPositionNearby(_sectorToSpawnIn.position, 0.0f, ProjectConstants.SECTOR_WIDTH / 3);
        }
    }

    private CharacterController spawnCharacter(GameObject prefab, Vector2 position, Transform parent)
    {
        CharacterController _character = GameObject.Instantiate(prefab, position, new Quaternion(0, 0, 0, 0)).GetComponent<CharacterController>();
        if (parent) _character.transform.parent = parent;
        _character.name = _character.name.Replace("(Clone)", "");
        _character.Initialize();

        Characters.Add(_character);
        return _character;
    }

    private void despawnCharactersAndProjectiles()
    {
        // Despawn all characters
        for (int i = 0; i < Characters.Count; ++i)
        {
            Destroy(Characters[i].gameObject);
        }

        Characters = new List<CharacterController>();
        PlayerShip = null;

        // Despawn all projectiles
        foreach (Projectile projectile in FindObjectsOfType<Projectile>())
        {
            Destroy(projectile.gameObject);
        }
    }

    private void resetSectorPositions()
    {
        for (int i = 0; i < Level.childCount; ++i)
        {
            int _middleSectorIndex = Mathf.CeilToInt(Level.childCount / 2.0f) - 1;
            float _distanceFromMiddleSector = (_middleSectorIndex - i) * -ProjectConstants.SECTOR_WIDTH;
            Level.GetChild(i).localPosition = new Vector2(_distanceFromMiddleSector, Level.GetChild(i).localPosition.y);
        }
    }

    public void StartWave(int enemyAmount)
    {
        WaveNumber++;
        m_remainingEnemies = enemyAmount;
        Characters = new List<CharacterController>();
        spawnPlayer();
        spawnEnemies(enemyAmount);
    }

    public void RestartWave()
    {
        // Despawn and the player and the remaining enemies,
        // then also reset the position of the sectors
        despawnCharactersAndProjectiles();
        resetSectorPositions();

        if (RemainingBackupShips > 0)
        {
            RemainingBackupShips--;

            // Respawn the player and the remaining enemies
            spawnPlayer();
            spawnEnemies(m_remainingEnemies);
        }
        else
        {
            UI.GameOverScreen.gameObject.SetActive(true);
        }
    }

    public void EnemyDestroyed(int destroyPoints)
    {
        // Update score
        m_remainingEnemies--;
        Score += destroyPoints;

        // Check if enemies still remain, if not then start the next wave
        if (m_remainingEnemies < 1)
        {
            despawnCharactersAndProjectiles();
            resetSectorPositions();
            WaveEnemyAmount++;
            UI.Overlay.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
