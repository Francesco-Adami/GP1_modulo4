using System;
using System.Collections;
using UnityEngine;

public enum ZoneCaptured
{
    None,
    Player,
    Enemy
}

public class Flag : MonoBehaviour
{
    [Header("Capturing Completation")]
    [SerializeField] private float captureTime = 5f;
    [SerializeField] private float captureCompletationTime = 25f;
    public ZoneCaptured ZoneCaptured = ZoneCaptured.None;

    // --- PLAYER ---
    #region PLAYER FLAG PROPERTIES
    private float _playerCaptureCompletationProgress = 0f;
    public float PlayerCaptureCompletationProgress
    {
        get { return _playerCaptureCompletationProgress; }
        set
        {
            _playerCaptureCompletationProgress = Mathf.Clamp(value, 0f, captureCompletationTime);
            FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).UpdatePlayerCaptureCompletationProgress(_playerCaptureCompletationProgress / captureCompletationTime);
            if (_playerCaptureCompletationProgress >= captureCompletationTime)
            {
                Debug.Log("Player has completed the capture!");
                ZoneCaptured = ZoneCaptured.None; // Reset the state after completion
                StartCoroutine(WinRoutine(ZoneCaptured.Player));
            }
        }
    }

    private float _playerCaptureProgress = 0f;
    public float PlayerCaptureProgress
    {
        get { return _playerCaptureProgress; }
        set
        {
            _playerCaptureProgress = Mathf.Clamp(value, 0f, captureTime);
            FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).UpdatePlayerCaptureProgress(_playerCaptureProgress / captureTime);
            if (_playerCaptureProgress >= captureTime)
            {
                ZoneCaptured = ZoneCaptured.Player;
                EnemyCaptureProgress = 0f;
            }
        }
    }

    public bool IsPlayerCapturing;
    #endregion
    // --- ------ ---

    // --- ENEMY ---
    #region ENEMY FLAG PROPERTIES
    private float _enemyCaptureCompletationProgress = 0f;
    public float EnemyCaptureCompletationProgress
    {
        get { return _enemyCaptureCompletationProgress; }
        set
        {
            _enemyCaptureCompletationProgress = Mathf.Clamp(value, 0f, captureCompletationTime);
            FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).UpdateEnemyCaptureCompletationProgress(_enemyCaptureCompletationProgress / captureCompletationTime);
            if (_enemyCaptureCompletationProgress >= captureCompletationTime)
            {
                Debug.Log("Enemy has completed the capture!");
                ZoneCaptured = ZoneCaptured.None;
                StartCoroutine(WinRoutine(ZoneCaptured.Enemy));
            }
        }
    }

    private float _enemyCaptureProgress = 0f;
    public float EnemyCaptureProgress
    {
        get { return _enemyCaptureProgress; }
        set
        {
            _enemyCaptureProgress = Mathf.Clamp(value, 0f, captureTime);
            FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).UpdateEnemyCaptureProgress(_enemyCaptureProgress / captureTime);
            if (_enemyCaptureProgress >= captureTime)
            {
                ZoneCaptured = ZoneCaptured.Enemy;
                PlayerCaptureProgress = 0f;
            }
        }
    }

    public bool IsEnemyCapturing;
    #endregion
    // --- ----- ---

    private void Start()
    {
        ResetCaptureProgress();
    }

    private void Update()
    {
        switch (ZoneCaptured)
        {
            case ZoneCaptured.None:
                break;

            case ZoneCaptured.Player:
                //Debug.Log("Enemy is capturing the zone.");
                PlayerCaptureCompletationProgress += Time.deltaTime;
                break;

            case ZoneCaptured.Enemy:
                //Debug.Log("Enemy is capturing the zone.");
                EnemyCaptureCompletationProgress += Time.deltaTime;
                break;

            default:
                Debug.LogError("Invalid zone captured state.");
                break;
        }

        if (!IsPlayerCapturing && ZoneCaptured != ZoneCaptured.Player)
        {
            PlayerCaptureProgress -= Time.deltaTime;
        }
        else if (!IsEnemyCapturing && ZoneCaptured != ZoneCaptured.Enemy)
        {
            EnemyCaptureProgress -= Time.deltaTime;
        }
    }

    private IEnumerator WinRoutine(ZoneCaptured winner)
    {
        GameManager.Instance.IsGamePaused = true;

        yield return new WaitForSecondsRealtime(2f); // Wait for 2 seconds before showing the win message

        Debug.Log($"Game Over! {winner} has won the game!");
        if (winner == ZoneCaptured.Player)
        {
            // Show player win UI
            UIManager.Instance.ShowUI(UIManager.GameUI.Win);
        }
        else if (winner == ZoneCaptured.Enemy)
        {
            // Show enemy win UI
            UIManager.Instance.ShowUI(UIManager.GameUI.Lose);
        }
    }

    private void ResetCaptureProgress()
    {
        PlayerCaptureProgress = 0f;
        PlayerCaptureCompletationProgress = 0f;
        EnemyCaptureProgress = 0f;
        EnemyCaptureCompletationProgress = 0f;
    }
}
