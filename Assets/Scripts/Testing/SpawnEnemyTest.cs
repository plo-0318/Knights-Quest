using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemyTest : MonoBehaviour
{
    public Button startGameButton;
    public Button killAllEnemiesButton;
    public Button toggleSpawnButton;

    private void Start()
    {
        var gs = GameManager.GameSession();

        startGameButton.onClick.AddListener(gs.StartGame);
        killAllEnemiesButton.onClick.AddListener(gs.KillAllEnemies);
        toggleSpawnButton.onClick.AddListener(gs.TEST_ToggleSpawn);
    }
}
