using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocktail : ScriptableObject
{


    public string Name;
    public string Base;
    public string Strength;
    public List<string> Type = new List<string>();
    public List<string> Country = new List<string>();

    public float recommendation;
    public float cf = 0;
    public int num;


    public override string ToString()
    {
        string result = this.Name + " " + this.Base + " " + this.Strength + " ";
        foreach (string temp in Type)
        {
            result += temp + " ";
        }
        foreach (string temp in Country)
        {
            result += temp + " ";
        }
        return result;
    }

    public string[] GetBST()
    {

        num = 2 + Type.Count;
        int i;

        string[] result = new string[num];
        for (i = 0; i < num - 2; i++)
        {
            result[i] = Type[i];
        }
        result[i] = Base;
        result[i + 1] = Strength;

        return result;
    }

}