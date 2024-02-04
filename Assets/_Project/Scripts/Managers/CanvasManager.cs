using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] Text scoreText;
    [SerializeField] Sprite lifeIcon;
    [SerializeField] Color lifeIconColor;
    [SerializeField] Color lifeLossColor;
    [SerializeField] Transform lifeFeedbackParent;

    [Header("StartScreen")]
    [SerializeField] Text startScreenMaxScore;
    [SerializeField] GameObject startScreenGO;

    [Header("EndScreen")]
    [SerializeField] GameObject endScreenGO;

    private readonly List<Image> lifeFeedbackList = new();
    private int maxScore = 0;


    public static CanvasManager Instance { get; private set;}

    void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start() {
        SetTotalLifeCount(GameBalanceValues.Instance.Life.MaxAmount);
        endScreenGO.SetActive(false);
    }

    public void UpdateScore(int score) {
        scoreText.text = score.ToString() + " / MAX: " + maxScore.ToString();
    }

    public void SetTotalLifeCount(int lifeCount) {
        Utils.RemoveAllChild(lifeFeedbackParent);

        while(lifeCount > 0) {
            Image instance = new GameObject().AddComponent<Image>();
            instance.sprite = lifeIcon;
            instance.SetNativeSize();
            instance.color = lifeIconColor;

            instance.transform.SetParent(lifeFeedbackParent);
            instance.transform.localScale = Vector3.one;

            lifeFeedbackList.Add(instance);

            lifeCount--;
        }
    }

    public void UpdateLifeAmount(int amount) {
        if(amount < 0 || amount > lifeFeedbackList.Count) {
            throw new System.Exception("INVALID AMOUNT OF LIFE");
        }

        for(int i = 0; i < lifeFeedbackList.Count; i++) {
            lifeFeedbackList[i].color = i < amount ? lifeIconColor : lifeLossColor;
        }
    }

    public void SetMaxScore(int score) {
        maxScore = score;
        startScreenMaxScore.text = "Max Score: " + score.ToString();
    }
    public void OnEndGame() {
        endScreenGO.SetActive(true);
    }

    public void OnStartButton() {
        startScreenGO.SetActive(false);
        GameManager.Instance.StartGame();
    }

}
