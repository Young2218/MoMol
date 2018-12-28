using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowQuestion : ScriptableObject {

    public Quest Q;
    public GameObject panel;
    public Transform title;
    public SpriteRenderer[] btnImg;
    public Transform Origin;
    readonly string src = "Sprites/Options/";

    public ShowQuestion(Quest qu, Transform or)
    {
        Origin = or;
        Debug.Log("Enter Constructor");
        Q = qu;
        if (Q.numOfOption == 2)
        {
            panel = GameObject.Find("QuestionPanel2");
        }
        else if (Q.numOfOption == 3)
        {
            panel = GameObject.Find("QuestionPanel3");
        }
        else if (Q.numOfOption == 4)
        {
            panel = GameObject.Find("QuestionPanel4");
        }
        //Debug.Log("point0");

        //Debug.Log("point7");
        title = panel.transform.GetChild(0);
        //Debug.Log("point8");
        title.GetComponent<Text>().text = Q.title;
        //Debug.Log("point1");
        for (int i = 0; i < Q.numOfOption; i++)
        {
            panel.transform.GetChild(i + 1).GetComponent<Image>().sprite = Resources.Load<Sprite>(src + Q.imgSrc[i]);
            //Debug.Log("Img: " + src + Q.imgSrc[i]);
            //Debug.Log("point3");
        }
        //Debug.Log("point2");
        panel.transform.position = new Vector2(Origin.position.x, Origin.position.y);

    }
}
