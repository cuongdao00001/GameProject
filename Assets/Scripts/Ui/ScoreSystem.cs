using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI myScore;
    private int scoreNum;
    void Start()
    {
        // Lấy điểm số từ PlayerPrefs
        scoreNum = PlayerPrefs.GetInt("Score", 0);
        UpdateScoreUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int amount)
    {
        scoreNum += amount;
        UpdateScoreUI();

        // Lưu điểm số vào PlayerPrefs
        PlayerPrefs.SetInt("Score", scoreNum);
    }

    private void UpdateScoreUI()
    {
        myScore.text = " " + scoreNum.ToString();
    }

}
