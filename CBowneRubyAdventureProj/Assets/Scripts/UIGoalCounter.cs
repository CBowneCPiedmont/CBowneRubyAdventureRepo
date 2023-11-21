using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGoalCounter : Singleton<UIGoalCounter>
{

    protected override void OnAwake() 
    {  
        text = GetComponent<TextMeshProUGUI>();
        UpdateDisplay();
    }
    
    string str1 = "Fixed Robots : ";

    int robotsFixed;
    int robotstoFix;

    TextMeshProUGUI text;

    private void UpdateDisplay()
    {
        if(!text)text = GetComponent<TextMeshProUGUI>();
        text.text = str1 + robotsFixed + " / " + robotstoFix;
        if(robotsFixed >= robotstoFix)
        {  
            UIEnding.instance.GameEnd();
        }
    }

    public void AddRobot()
    {  
        robotstoFix++;
        UpdateDisplay();
    }

    public void FixRobot()
    {  
        robotsFixed++;
        UpdateDisplay();
    }




}
