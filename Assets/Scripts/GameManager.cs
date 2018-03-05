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
    public int EnemyIncreasePerWave = 1;
    public int RemainingBackupShips = 2;
    public int Score = 0;

    public List<ShipController> Ships;

    private int m_remainingEnemies;

	void Awake()
    {
        Instance = this;
        UI = FindObjectOfType<UserInterface>();
        Level = GameObject.Find("Level").transform;
	}

    private void spawnPlayer()
    {
        PlayerShip = spawnShip(PlayerPrefab, new Vector2(0, 0), null).transform.Find("Ship");
    }

    private void spawnEnemies(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            // Get a random sector (excluding the middle sector that the player spawns in) to spawn the enemy in
            int _randomSectorIndex = Random.Range(0, Level.childCount);
            if (_randomSectorIndex == Mathf.CeilToInt(GameManager.Instance.Level.childCount / 2.0f) - 1) ++_randomSectorIndex;
            Transform _sectorToSpawnIn = Level.GetChild(_randomSectorIndex);
            Transform _enemy = spawnShip(EnemyPrefab, _sectorToSpawnIn.position, _sectorToSpawnIn).transform;

            // Get a random position near the center of the sector and set that as the enemy's position
            _enemy.position = ProjectConstants.PickRandomPositionNearby(_sectorToSpawnIn.position, 0.0f, ProjectConstants.SECTOR_WIDTH / 3);
        }
    }

    private ShipController spawnShip(GameObject prefab, Vector2 position, Transform parent)
    {
        ShipController _ship = GameObject.Instantiate(prefab, position, new Quaternion(0, 0, 0, 0)).GetComponent<ShipController>();
        if (parent) _ship.transform.parent = parent;
        _ship.name = _ship.name.Replace("(Clone)", "");
        _ship.Initialize();
        Ships.Add(_ship);

        return _ship;
    }

    private void despawnShipsAndProjectiles()
    {
        // Despawn all ships
        for (int i = 0; i < Ships.Count; ++i)
        {
            Destroy(Ships[i].gameObject);
        }

        Ships = new List<ShipController>();
        PlayerShip = null;

        // Despawn all projectiles
        foreach (Projectile projectile in FindObjectsOfType<Projectile>())
        {
            Destroy(projectile.gameObject);
        }
    }

    private void resetSectorPositions()
    {
        int _middleSectorIndex = Mathf.CeilToInt(Level.childCount / 2.0f) - 1;

        for (int i = 0; i < Level.childCount; ++i)
        {
            float _distanceFromMiddleSector = (_middleSectorIndex - i) * -ProjectConstants.SECTOR_WIDTH;
            Level.GetChild(i).localPosition = new Vector2(_distanceFromMiddleSector, Level.GetChild(i).localPosition.y);
        }
    }

    public void StartWave(int enemyAmount)
    {
        WaveNumber++;
        m_remainingEnemies = enemyAmount;
        Ships = new List<ShipController>();
        spawnPlayer();
        spawnEnemies(enemyAmount);
    }

    public void RestartWave()
    {
        // Despawn and the player and the remaining enemies,
        // then also reset the position of the sectors
        despawnShipsAndProjectiles();
        resetSectorPositions();

        // If the player still has backup ships remaining
        if (RemainingBackupShips > 0)
        {
            RemainingBackupShips--;

            // Respawn the player and the remaining enemies
            spawnPlayer();
            spawnEnemies(m_remainingEnemies);
        }
        // Otherwise activate the game over screen
        else UI.GameOverScreen.gameObject.SetActive(true);
    }

    public void EnemyDestroyed(int destroyPoints)
    {
        m_remainingEnemies--;
        Score += destroyPoints;

        // Check if enemies still remain, if not then start the next wave
        if (m_remainingEnemies < 1)
        {
            despawnShipsAndProjectiles();
            resetSectorPositions();
            WaveEnemyAmount += EnemyIncreasePerWave;
            UI.Overlay.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
