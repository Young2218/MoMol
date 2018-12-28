using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Button : MonoBehaviour
{

    public GameObject InitialPanel = null;
    public GameObject ResultPanel = null;
    public Transform Origin;
    public Transform Out;

    public Slider StarSlider = null;
    public Text UserAge = null;
    public Text UserGender = null;
    public string[] userChoices;
    public Data data;

    Quest[] questArr;
    ShowQuestion sq;
    UserEx ux;
    int stage;
    int maxStage;


    // ===========================================================================================
    void Start()
    {
        InitialPanel = GameObject.Find("InitialPanel");
        ResultPanel = GameObject.Find("ResultPanel");

        Origin = GameObject.Find("Main Camera").GetComponent<Transform>();
        Out = GameObject.Find("Out").GetComponent<Transform>();


        StarSlider = GameObject.Find("StarSlider").GetComponent<Slider>();
        data = new Data();
        ux = new UserEx();

        //=======================================================================================
        InitialPanel.transform.position = new Vector2(Origin.position.x, Origin.position.y);
        data.readData();
        Debug.Log("1");
        data.Color_read();
        Debug.Log("2");
        ReadQuest();


        //=============== 사진과 이름이 다른 칵테일을 모두 찾아볼거에요==============================
        
        //string src = "Sprites/Cocktails/";
        //for (int i = 0; i < data.cocktail.Length; i++)
        //{
        //    string[] temp = data.cocktail[i].Name.Split(' ');
        //    string underName = "cocktail";
        //    foreach (string str in temp)
        //    {
        //        if (!str.Equals(""))
        //            underName += "_" + str.ToLower();
        //    }
        //    // 이미지 가져오기
            
        //    if (!System.IO.File.Exists("Assets/Resources/"+src + underName + "-1.png"))
        //        Debug.Log("not exist: " + data.cocktail[i].Name);


        //}

        for(int i = 0; i < data.cocktail.Length; i++)
        {
            ARGB argb;
            if(!data.dic.TryGetValue(data.cocktail[i].Name.ToLower(),out argb))
            {
                Debug.Log(data.cocktail[i].Name);
            }
        }
    }
    //=============================================================================================
    public void StartBtnOnClick()
    {
        Debug.Log("Start Button On click");
        GoOut(InitialPanel);

        ux.Next();

    }
    public void Choice1()
    {
        userChoices[stage] = sq.Q.option[0];
        NextQuestion();
    }
    public void Choice2()
    {
        userChoices[stage] = sq.Q.option[1];
        NextQuestion();
    }
    public void Choice3()
    {
        userChoices[stage] = sq.Q.option[2];
        NextQuestion();
    }
    public void Choice4()
    {
        userChoices[stage] = sq.Q.option[3];
        NextQuestion();
    }
    public void Yes()
    {
        // 평점 가져오기
        StarSlider.value = 3f;
        ux.Star();
    }
    public void No()
    {
        // 다음 문제 가져오기
        if (!ux.Next())
        {
            StartQuestion();
            GoOut(ux.ExPanel1);
            GoOut(ux.ExPanel2);
        }
    }
    public void UserConfirm()
    {

        UserEx.userScore[ux.stage] = (int)StarSlider.value;

        // 점수를 입력해야함
        if (!ux.Next())
        {
            StartQuestion();
            GoOut(ux.ExPanel1);
            GoOut(ux.ExPanel2);
        }
    }
    public void UserScore()
    {

        string temp = "";
        if ((int)StarSlider.value == 5)
        {
            temp = "최고";
        }
        else if ((int)StarSlider.value == 4)
        {
            temp = "좋았어";
        }
        else if ((int)StarSlider.value == 3)
        {
            temp = "보통";
        }
        else if ((int)StarSlider.value == 2)
        {
            temp = "좀 별로";
        }
        else if ((int)StarSlider.value == 1)
        {
            temp = "최악";
        }
        StarSlider.transform.GetChild(0).GetComponent<Text>().text = temp;
    }
    
    //=============================================================================================
    public void StartQuestion()
    {
       stage = 0;
        sq = new ShowQuestion(questArr[stage], Origin);
    }
    public void NextQuestion()
    {
        GoOut(sq.panel);
        Debug.Log(stage +" "+maxStage);
       
        if (stage == maxStage - 1)
        {
            Debug.Log("please point 2");
            foreach (string tp in userChoices)
            {
                Debug.Log("User choice: " + tp);
            }
            Debug.Log("please point 1");
            data.Longest(userChoices); // LCS
            Debug.Log("please point 3");
            Cocktail[] result = data.getRecommendation(); // 추천도가 높은 상위 4개 뽑아오기
            foreach (Cocktail tp in result)
            {
                Debug.Log("Recommadation: " + tp.Name + " " + tp.recommendation + " " + tp.cf);
            }
            // 결과창을 띄어줘야함.
            ShowResult(result);
            Debug.Log("please point end");
        }
        else
        {
            sq = new ShowQuestion(questArr[++stage], Origin);
        }
       
    }
    public void ReadQuest()
    {
        //FileStream fs = new FileStream("Application.persistentDataPath" + "/" + "question.txt", FileMode.Open, FileAccess.Read);
        TextAsset fs = (TextAsset)Resources.Load("TextAsset/question", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(fs.text);

        string str;
        
        string temp;
        int numOfQuest = int.Parse(sr.ReadLine());
        maxStage = numOfQuest;
        questArr = new Quest[numOfQuest];
        userChoices = new string[numOfQuest];

        for (int i = 0; i < numOfQuest; i++)
        {
            temp = sr.ReadLine();
            string[] strArr = temp.Split('/');

            questArr[i] = new Quest(strArr[0], int.Parse(strArr[1]), strArr[2].Split(','), strArr[3].Split(','));
            Debug.Log(i + ": " + questArr[i].PrintQuest());
        }
        sr.Dispose();

    }
    public void GoOut(GameObject ob)
    {
        ob.transform.position = Out.transform.position;
    }
    public void GoOrigin(GameObject ob)
    {
        ob.transform.position = new Vector3(Origin.transform.position.x, Origin.transform.position.y, 1f);

    }
    public void ShowResult(Cocktail[] resultCocktail)
    {
        Image[] resultImg = new Image[4];
        Text[] resultName = new Text[4];
        string src = "Sprites/Cocktails/";
        GoOrigin(ResultPanel);
        for (int i = 0; i < 4; i++)
        {
            string[] temp = resultCocktail[i].Name.Split(' ');
            string underName = "cocktail";
            foreach (string str in temp)
            {
                if (!str.Equals(""))
                    underName += "_" + str.ToLower();
            }


            resultImg[i] = ResultPanel.transform.GetChild(i + 1).transform.GetChild(0).GetComponent<Image>();
            resultName[i] = ResultPanel.transform.GetChild(i + 1).transform.GetChild(1).GetComponent<Text>();
            resultImg[i].sprite = Resources.Load<Sprite>(src + underName + "-1");
            resultName[i].text = resultCocktail[i].Name;
            Debug.Log(src + underName + "-1");
        }

    }
}
