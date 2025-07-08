using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    [Header("Player Settings")]
    public Slider healthBarSlider;

    [Header("Flag Capturing")]
    [SerializeField] private Image playerCapture;
    [SerializeField] private Image enemyCapture;

    [Header("Flag Capture Progress")]
    [SerializeField] private Slider playerCaptureProgressBar;
    [SerializeField] private Slider enemyCaptureProgressBar;

    internal void UpdatePlayerCaptureCompletationProgress(float v)
    {
        if (playerCaptureProgressBar != null)
        {
            playerCaptureProgressBar.value = v;
        }
    }

    internal void UpdateEnemyCaptureCompletationProgress(float v)
    {
        if (enemyCaptureProgressBar != null)
        {
            enemyCaptureProgressBar.value = v;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.Instance.ShowUI(UIManager.GameUI.Pause);
            GameManager.Instance.IsGamePaused = true;
        }
    }

    internal void UpdateEnemyCaptureProgress(float v)
    {
        enemyCapture.fillAmount = v;
    }

    internal void UpdatePlayerCaptureProgress(float v)
    {
        playerCapture.fillAmount = v;
    }
}
