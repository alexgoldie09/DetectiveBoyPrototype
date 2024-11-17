using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    [Header("UI panels")]
    [SerializeField] private GameObject mainPanel;

    // Update is called once per frame
    void Update()
    {
        if(Extensions.isPaused || Extensions.isTalking || Extensions.isExamining)
        {
            mainPanel.SetActive(false);
        }
        else
        {
            mainPanel.SetActive(true);
        }
    }
}
