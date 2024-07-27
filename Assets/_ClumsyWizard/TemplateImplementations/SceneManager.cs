using ClumsyWizard.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : CW_SceneManagement
{
    private Animator animator;
    [SerializeField] private Image loadingBar;
    public override bool IsGameLevel => CurrentLevel.Contains("Level");

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        loadingBar.fillAmount = 0.0f;
    }

    protected override void OnLoadTriggered()
    {
        animator.SetBool("Fade", true);
    }
    protected override void LoadingProgress(float progress)
    {
        loadingBar.fillAmount = progress;
    }
    protected override void OnFinishLoadingScene()
    {
        animator.SetBool("Fade", false);
        loadingBar.fillAmount = 0.0f;
    }
}
