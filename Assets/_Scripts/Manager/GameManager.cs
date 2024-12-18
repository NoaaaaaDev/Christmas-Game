using System;
using System.Collections.Generic;
using GestureRecognizer;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    private int currentLevel;
    private LevelData currentLevelData;

    private int currentPoints;
    public int CurrentPoints => currentPoints;

    private bool isGameEnded = false;
    public bool IsGameEnded => isGameEnded;

    private float difficultyMultiplier = 1;
    private float speedDifficulty = 1;
    private float globalSpeedMultiplier = 1;
    public float GlobalSpeedMultiplier => globalSpeedMultiplier;

    // UI
    [SerializeField] private PointsUI pointsUI;
    [SerializeField] private GameObject drawUI;
    [SerializeField] private TextMeshProUGUI levelXText;

    private void OnEnable() {
        EventDispatcher.Add<EventDefine.OnPointsAdded>(OnPointsAdded);
        EventDispatcher.Add<EventDefine.OnLoseGame>(OnLoseGame);
        EventDispatcher.Add<EventDefine.OnWinGame>(OnWinGame);
    }


    private void OnDisable() {
        EventDispatcher.Remove<EventDefine.OnPointsAdded>(OnPointsAdded);
        EventDispatcher.Remove<EventDefine.OnLoseGame>(OnLoseGame);
        EventDispatcher.Remove<EventDefine.OnWinGame>(OnWinGame);
    }

    private void Start() {
        currentLevel = PlayerData.CurrentLevel;
        currentLevelData = GameData.levels[currentLevel - 1];
        Time.timeScale = 1;
        levelXText.text = "Level " + currentLevel;
        EnemyManager.Instance.InitiateLevel(currentLevelData);
    }
    
    private void Update() {
        HandleDifficulty();
    }

    private float difficultyTimer = 0f;

    private void HandleDifficulty() {
        difficultyTimer += Time.deltaTime;
        
        if (difficultyTimer >= 5f) {
            EnemyData.enemyDataProbabilityDict[EnemyData.single] -= 1;
            EnemyData.enemyDataProbabilityDict[EnemyData.triple] -= 1;
            EnemyData.enemyDataProbabilityDict[EnemyData.triple] += 4;
            EnemyData.enemyDataProbabilityDict[EnemyData.triple] += 2;
            EnemyData.enemyDataProbabilityDict[EnemyData.quintuple] += 1;
            
            difficultyMultiplier += 0.2f;
            globalSpeedMultiplier += 0.005f;
            difficultyTimer = 0f; 
        }
    }

    private void OnPointsAdded(IEventParam param) {
        EventDefine.OnPointsAdded _param = (EventDefine.OnPointsAdded)param;

        currentPoints += _param.points;
        pointsUI.SetText(currentPoints.ToString());
    }

    private void OnLoseGame(IEventParam param) {
        Time.timeScale = 0;
        drawUI.SetActive(false);
        Debug.Log("YOU LOST");
    }

    private void OnWinGame(IEventParam param) {
        drawUI.SetActive(false);
        isGameEnded = true;
    }

    [ContextMenu("Reload Data")]
    public void ReloadData() {
        PlayerData.ResetData();
    }
}
