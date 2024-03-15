using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FTLListPart : MonoBehaviour
{
    public string name;
    public int score;
    public Image im;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void FixedUpdate()
    {
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
