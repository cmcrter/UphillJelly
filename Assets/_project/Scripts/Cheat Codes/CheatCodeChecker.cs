using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatCodeChecker : MonoBehaviour
{
    [SerializeField] public string buffer;
    [SerializeField] public TextMeshProUGUI cheatcodeTMP;
    [SerializeField] private float maxTimeDif = 1;
    private List<string> validPatterns = new List<string>() { "Shrimp", "123" };
    private float timeDif;

    void Start()
    {
        timeDif = maxTimeDif;
        
    }

    void Update()
    {
        timeDif -= Time.deltaTime;

        if (timeDif <= 0)
        {
            buffer = "";
        }

        //cheatcodeTMP.text = buffer;
        buffer = cheatcodeTMP.text;
        CheckPattern();

    }

    void AddToBuffer(string c)
    {
        timeDif = maxTimeDif;
        buffer += c;
    }

    void CheckPattern()
    {
        if (buffer.EndsWith(validPatterns[0]))
        {
            Debug.Log("Shrimpiessssssss");
            buffer = "";
        }
        else if (buffer.EndsWith(validPatterns[1]))
        {
            Debug.Log("NUMBERS");
            buffer = "";
        }

    }

}
