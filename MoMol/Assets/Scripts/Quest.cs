using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : ScriptableObject
{

    public string title;
    public int numOfOption;
    public string[] option;
    public string[] imgSrc;


    public Quest(string title, int nQ, string[] opt, string[] iS)
    {
        this.title = title;
        numOfOption = nQ;
        option = opt;
        imgSrc = iS;
    }

    public string PrintQuest()
    {
        string result = title + " " + numOfOption.ToString() + " ";

        foreach (string temp in option)
        {
            result += (temp + " ");
        }
        foreach (string temp in imgSrc)
        {
            result += (temp + " ");
        }

        return result;
    }
}
