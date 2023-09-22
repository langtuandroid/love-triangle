using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlayerData {
    public enum StatusLevel { Hobo = 0, Poor, Average, Rich, Millionaire }
    public static Action onLoadDone;
    public static int CurrentStage {set; get;}
    public static string LevelText => (CurrentStage + 1).ToString();
    public static int CurrentMoney { private set; get; }
    public static int BankMoney { private set; get; }
    public static int CurrentLevelMoney {set; get;}
    public static bool IsTutorialDone => m_tutorialDone == 1;
    private static int m_tutorialDone = 0;
    public static StatusLevel CurrentStatusLevel {set; get;}
    public static LadiesProgressBar.SatisfactionLevel WifeSatisfactionLevel{set; get;}
    public static LadiesProgressBar.SatisfactionLevel MistressSatisfactionLevel{set; get;}  
    public static LadiesProgressBar.LadyType EnteredHouse { set; get; }

    public static bool IsWifeSpeechDone => m_wifeSpeechDone == 1;
    private static int m_wifeSpeechDone = 0;
    public static bool IsMistressSpeechDone => m_mistressSpeechDone == 1;
    private static int m_mistressSpeechDone = 0;

    public static void Initialize(){
        CurrentLevelMoney = 0;
        Load();
    }
    public static void FinishWifeSpeech() {
        m_wifeSpeechDone = 1;
        Save();
    }
    public static void FinishMistressSpeech() {
        m_mistressSpeechDone = 1;
        Save();
    }
    public static void IncreaseCurrentLevelMoney(int p_amount, bool p_saveRightAway = false) {
        //CurrentMoney += p_amount;
        CurrentLevelMoney += p_amount;
        if (p_saveRightAway) {
            Save();
        }
    }
    public static void IncreaseMoney(int p_amount, bool p_saveRightAway = false, int p_maxClampAmount = 1000000) {
        CurrentMoney += p_amount;
        CurrentMoney = Mathf.Clamp(CurrentMoney, 0, p_maxClampAmount);
        if (p_saveRightAway) {
            Save();
        }
    }
    public static void DecreaseMoney(int p_amount, bool p_saveRightAway = false, int p_minClampAmount = 0){
        CurrentMoney -= p_amount;
        CurrentMoney = Mathf.Clamp(CurrentMoney, p_minClampAmount, 1000000);
        if (p_saveRightAway) {
            Save();
        }
    }
    public static void IncreaseBankMoney(int p_amount, bool p_saveRightAway = false, int p_maxClampAmount = 1000000) {
        BankMoney += p_amount;
        BankMoney = Mathf.Clamp(BankMoney, 0, p_maxClampAmount);
        if (p_saveRightAway) {
            Save();
        }
    }
    public static void DecreaseBankMoney(int p_amount, bool p_saveRightAway = false, int p_minClampAmount = 0) {
        BankMoney -= p_amount;
        BankMoney = Mathf.Clamp(BankMoney, p_minClampAmount, 1000000);
        if (p_saveRightAway) {
            Save();
        }
    }
    public static void Save(){
        PlayerPrefs.SetInt("CurrentMoney", CurrentMoney);
        PlayerPrefs.SetInt("CurrentStage", CurrentStage);
        PlayerPrefs.SetInt("BankMoney", BankMoney);
        PlayerPrefs.SetInt("m_tutorialDone", m_tutorialDone);
        PlayerPrefs.SetInt("m_wifeSpeechDone", m_wifeSpeechDone);
        PlayerPrefs.SetInt("m_mistressSpeechDone", m_mistressSpeechDone);
    }
    public static void Load(){
        CurrentMoney = PlayerPrefs.GetInt("CurrentMoney", 0);
        CurrentStage = PlayerPrefs.GetInt("CurrentStage", 0);
        BankMoney = PlayerPrefs.GetInt("BankMoney", 0);
        m_tutorialDone = PlayerPrefs.GetInt("m_tutorialDone", 0);
        m_wifeSpeechDone = PlayerPrefs.GetInt("m_wifeSpeechDone", 0);
        m_mistressSpeechDone = PlayerPrefs.GetInt("m_mistressSpeechDone", 0);
        onLoadDone?.Invoke();
    }

    public static void GoToNextLevel(int p_maxLevelCount){
        
        CurrentStage++;
        m_tutorialDone = 1;
        Save();
    }

    public static int GetStarCount(){
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS){
            return 1;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES){
            return 3;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE){
            return 5;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES){
            return 2;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS){
            return 2;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE){
            return 2;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.CARELESS){
            return 2;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE){
            return 4;
        }
        if(WifeSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.INLOVE && 
        MistressSatisfactionLevel == LadiesProgressBar.SatisfactionLevel.LIKES){
            return 4;
        }
        return 0;
    }

    public static string GetRating(){
        float starRating = GetStarCount();
        if(starRating < 2f){
            return "Loser";
        }
        if(starRating < 3f){
            return "Cheater";
        }
        return "Master";
    }
}
