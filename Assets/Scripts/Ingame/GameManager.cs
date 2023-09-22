using System;
using System.Collections.Generic;
using UnityEngine;
using LoveTriangle;
using Dreamteck.Splines;
public class GameManager : MonoBehaviour {
	public static Action OnRestartGame;
    public static Action<Level> OnLevelInstantiated;
    public static GameManager Instance;
    public EndSceneAnimationManager endSceneManager;
    public List<Level> levels = new List<Level>();
    public SplineComputer Spline {  private set; get;   }

    public SplineFollower splineFollower;

    public bool dontShowProgressbarTutorial;
    void OnEnable(){
        if(Instance == null){
            Instance = this;
        }
        LadiesProgressBar.updateSatisfactionLevel += UpdateSatisfactionLevel;
        //Gentleman.onDone += OnDone;
    }

    void OnDisable(){
        if(Instance == this){
            Instance = null;
        }
        LadiesProgressBar.updateSatisfactionLevel -= UpdateSatisfactionLevel;
        //Gentleman.onDone -= OnDone;
    }

    void Start(){
        PlayerData.Initialize();
        InitializeLevels();
    }
    void InitializeLevels(){
        int levelIndex = PlayerData.CurrentStage;
        if(levelIndex >= levels.Count){
            levelIndex = levelIndex % levels.Count;
        }
        GameObject goLevel = Instantiate(levels[levelIndex]).gameObject;
        Spline = goLevel.GetComponentInChildren<SplineComputer>();
        splineFollower.spline = Spline;
        splineFollower.SetPercent(0);
        splineFollower.RebuildImmediate();
        Level level = goLevel.GetComponent<Level>();
        OnLevelInstantiated?.Invoke(level);
        FogChanger.ChageFog(level.environmentType);
    }

    void UpdateSatisfactionLevel(LadiesProgressBar.LadyType p_type, LadiesProgressBar.SatisfactionLevel p_satisfactionLevel, float p_fillAmount) {
        switch(p_type){
            case LadiesProgressBar.LadyType.MISTRESS:
            PlayerData.MistressSatisfactionLevel = p_satisfactionLevel;
            break;
            case LadiesProgressBar.LadyType.WIFE:
            PlayerData.WifeSatisfactionLevel = p_satisfactionLevel;
            break;
        }
    }

    void OnDone(GameObject p_guy, GameObject p_wife, GameObject p_mistress){
        if(PlayerData.WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS && 
           PlayerData.MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS){
               endSceneManager.PlayBothCareless(p_wife, p_mistress);
        } else if(PlayerData.WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES && 
           PlayerData.MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES){
               endSceneManager.PlayBothWOmanInLikes(p_wife, p_mistress);
        } else if(PlayerData.WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE && 
           PlayerData.MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE){
               endSceneManager.PlayBothInLove(p_wife, p_mistress);
        } else if(PlayerData.WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE && 
           PlayerData.MistressSatisfactionLevel != LadiesProgressBar.SatisfactionLevel.INLOVE){
               endSceneManager.PlayOneInLoveOneLikes(p_wife, p_mistress, true);
        } else if(PlayerData.WifeSatisfactionLevel != LadiesProgressBar.SatisfactionLevel.INLOVE && 
           PlayerData.MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE){
               endSceneManager.PlayOneInLoveOneLikes(p_mistress, p_wife);
        } else if(PlayerData.WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES && 
           PlayerData.MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS){
               endSceneManager.PlayOneInLikesOneCareless(p_wife, p_mistress, true);
        } else if(PlayerData.WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS && 
           PlayerData.MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES){
               endSceneManager.PlayOneInLikesOneCareless(p_mistress, p_wife);
        }
    }
}