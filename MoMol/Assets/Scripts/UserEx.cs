using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserEx : ScriptableObject
{

    public GameObject ExPanel1 = null;
    public GameObject ExPanel2 = null;
    public Transform Origin;
    public Transform Out;
    

    public static string[] commonCocktail = { "Gin Tonic", "Mojito", "Screwdriver", "Blue Hawaiian",
        "Margarita", "Midori Sour", "Long Island Ice Tea", "Pina Colada",
        "Sex on the Beach", "Tequila Sunrise" };
    public static int[] userScore = new int[10];
    public int stage = 0;

    //=======================================================================================
    public UserEx()
    {
        Origin = GameObject.Find("Main Camera").GetComponent<Transform>();
        Out = GameObject.Find("Out").GetComponent<Transform>();
        ExPanel1 = GameObject.Find("ExperPanel1");
        ExPanel2 = GameObject.Find("ExperPanel2");
        stage = -1;

        for (int i = 0; i < 10; i++) userScore[i] = -1;

    }



    /* 함수 구조화
     * 
     * 만약에 사용자가 yes라고 답하면 star를 콜
     * 만약에 사용자가 no라고 답하면 next를 콜
     * 만약에 사용자가 평가를 했으면 평가를 저장하고 next를 콜
     */

    public bool Next()
    {
        GoOut(ExPanel2);
        stage++;
        if (stage < userScore.Length)
        {

            GoOrigin(ExPanel1);
            // 주소 만들기
            string src = "Sprites/Cocktails/";
            string[] temp = commonCocktail[stage].Split(' ');
            string underName = "cocktail";
            foreach (string str in temp)
            {
                if (!str.Equals(""))
                    underName += "_" + str.ToLower();
            }
            // 이미지 가져오기
            ExPanel1.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(src + underName + "-1");
            // 이름 바꾸기
            ExPanel1.transform.GetChild(1).GetComponent<Text>().text = commonCocktail[stage];


            return true;

        }
        else
        {
            return false;
        }
    }

    public void Star()
    {
        
        GoOut(ExPanel1);
        GoOrigin(ExPanel2);
        // 주소 만들기
        string src = "Sprites/Cocktails/";
        string[] temp = commonCocktail[stage].Split(' ');
        string underName = "cocktail";
        foreach (string str in temp)
        {
            if (!str.Equals(""))
                underName += "_" + str.ToLower();
        }
        // 이미지 가져오기
        ExPanel2.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(src + underName + "-1");
        // 이름 바꾸기
        ExPanel2.transform.GetChild(1).GetComponent<Text>().text = commonCocktail[stage];

    }

    public void GoOut(GameObject ob)
    {
        ob.transform.position = Out.transform.position;
    }
    public void GoOrigin(GameObject ob)
    {
        ob.transform.position = new Vector3(Origin.transform.position.x, Origin.transform.position.y, 1f);

    }
}
