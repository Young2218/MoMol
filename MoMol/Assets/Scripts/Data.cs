using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Data : ScriptableObject
{
    public Cocktail[] cocktail;

    public Dictionary<string, ARGB> dic = new Dictionary<string, ARGB>();

    public void Color_read()
    {
        // FileStream cfs = new FileStream("Assets/Files/result.txt", FileMode.Open, FileAccess.Read);
        // FileStream cfs = new FileStream("Application.persistentDataPath"+"/" + "result.txt", FileMode.Open, FileAccess.Read);
        TextAsset cfs = (TextAsset)Resources.Load("TextAsset/result", typeof(TextAsset)) as TextAsset;
        // 파일을 열기위해 파일 스트림 객체 생성
        StringReader csr = new StringReader(cfs.text);
        // 스트림 리더 생성
        string c_str;// 한줄읽기

        c_str = csr.ReadLine();
        // color = new Color();
        while ((c_str = csr.ReadLine()) != null)
        {
            ARGB color = new ARGB();
            string[] c_temp = c_str.Split(' ');
            name = c_temp[0];
            name = name.Replace('_', ' ');

            color.A = int.Parse(c_temp[1]);
            color.R = int.Parse(c_temp[2]);
            color.G = int.Parse(c_temp[3]);
            color.B = int.Parse(c_temp[4]);

            dic.Add(name, color);
            Debug.Log(name + " : " + color.A + ", " + color.R + ", " + color.G + ", " + color.B);
        }
        csr.Dispose();
    }
    public void readData()
    {
        //LCS lcs = new LCS();
        string[] c_base = { "Wine", "Tequila", "Rum", "Gin", "Champagne", "Liqueur", "Vodka", "Whiskey", "Brandy", "Beer" };
        string[] c_type = { "Sparkling", "Classic", "Creamy", "Frozen", "Hot", "Longdrink", "Martini", "Nonalcoholic", "Shooter", "Short", "Smoothie", "Tropical" };
        string[] c_strength = { "Nonalcoholic", "Weak", "Light", "Medium", "Strong", "Extremely_Strong" };
        string[] c_country = { "Brazil", "Canada", "Cuba", "UK", "France", "Ireland", "Italy", "Mexico", "Russia", "Spain", "Tiki_Culture", "USA" };

        HashSet<string> cock_base = new HashSet<string>();
        HashSet<string> cock_type = new HashSet<string>();
        HashSet<string> cock_strength = new HashSet<string>();
        HashSet<string> cock_country = new HashSet<string>();

        for (int i = 0; i < c_base.Length; i++)
        {
            cock_base.Add(c_base[i]);
        }
        for (int i = 0; i < c_strength.Length; i++)
        {
            cock_strength.Add(c_strength[i]);
        }
        for (int i = 0; i < c_type.Length; i++)
        {
            cock_type.Add(c_type[i]);
        }
        for (int i = 0; i < c_country.Length; i++)
        {
            cock_country.Add(c_country[i]);
        }

        // 파일 읽기
        //FileStream fs = new FileStream("Application.persistentDataPath" + "/" + "cocktail.txt", FileMode.Open, FileAccess.Read);
        TextAsset fs = (TextAsset)Resources.Load("TextAsset/cocktail", typeof(TextAsset)) as TextAsset;
        // 파일을 열기위해 파일 스트림 객체 생성
        StringReader sr = new StringReader(fs.text);
        // 스트림 리더 생성
        string str;// 한줄읽기
        int index;
        int cnt = 0;
        str = sr.ReadLine();
        cocktail = new Cocktail[int.Parse(str)];
        while ((str = sr.ReadLine()) != null)
        {
            //Debug.Log("<start>" + str);
            int length = str.Length;
            index = str.IndexOf('/'); // '/' 기준으로 - cocktail name
            cocktail[cnt] = new Cocktail();
            cocktail[cnt].Name = str.Substring(0, index);
            string[] C = new string[10];
            str = str.Substring(index + 1, length - index - 1); // cocktail name 제외 문장
            string[] temp = str.Split(' ');

            cocktail[cnt].num = temp.Length;

            for (int i = 0; i < temp.Length; i++)
            {
                if (cock_base.Contains(temp[i]))
                {
                    cocktail[cnt].Base = temp[i];
                }
                else if (cock_strength.Contains(temp[i]))
                {
                    cocktail[cnt].Strength = temp[i];
                }
                else if (cock_type.Contains(temp[i]))
                {
                    cocktail[cnt].Type.Add(temp[i]);
                }
                else if (cock_country.Contains(temp[i]))
                {
                    cocktail[cnt].Country.Add(temp[i]);
                }
            }
            Debug.Log(cocktail[cnt].ToString());
            cnt++;
        } // end while
        Debug.Log("End Read File");
        sr.Dispose();
    }
    public void Longest(string[] User)
    {
        int[,] matrix;


        for (int iter = 0; iter < cocktail.Length; iter++)
        {
            string[] c = cocktail[iter].GetBST();
            int n = User.Length;
            int m = c.Length;
            matrix = new int[m + 1, n + 1];


            for (int i = 0; i < n + 1; i++)
            {
                matrix[0, i] = 0;
            }

            for (int j = 0; j < m + 1; j++)
            {
                matrix[j, 0] = 0;
            }

            for (int i = 1; i < m + 1; i++)
            {
                for (int j = 1; j < n + 1; j++)
                {
                    if (string.Equals(c[i - 1], User[j - 1]))
                    {
                        matrix[i, j] = matrix[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        int one = matrix[i, j - 1];
                        int two = matrix[i - 1, j];

                        matrix[i, j] = Math.Max(one, two);
                    }
                }
            }

            cocktail[iter].recommendation = (float)matrix[m, n] / cocktail[iter].num;


            //Debug.Log(cocktail[iter].Name + " " + cocktail[iter].recommendation + " " + matrix[m, n] + "-" + cocktail[iter].num);

        }
    }
    public Cocktail[] getRecommendation()
    {
        Cocktail[] newList = new Cocktail[4];
        int idx = 0, end_idx;
        Cocktail temp;
        ARGB rgb1, rgb2;

        // 소팅을 합시다
        for (int i = 0; i < cocktail.Length; i++)
        {
            idx = i;
            for (int j = i; j < cocktail.Length; j++)
            {
                if (cocktail[idx].recommendation < cocktail[j].recommendation)
                    idx = j;
            }
            temp = cocktail[i];
            cocktail[i] = cocktail[idx];
            cocktail[idx] = temp;

            Debug.Log("sort: " + cocktail[i].Name + " " + cocktail[i].recommendation);
        }




        //happy case
        if (cocktail[3].recommendation != cocktail[4].recommendation)
        {
            for (int i = 0; i < 4; i++)
            {
                newList[i] = cocktail[i];
            }
            return newList;
        }
        Debug.Log("2");

        // bad case
        idx = 0;
        while (cocktail[idx].recommendation != cocktail[3].recommendation) idx++;
        Debug.Log("3");
        end_idx = 5;
        while (cocktail[3].recommendation == cocktail[end_idx].recommendation) end_idx++;

        Debug.Log("4");
        // cf
        for (int i = 0; i < UserEx.commonCocktail.Length; i++)
        {
            if (UserEx.userScore[i] < 0) continue;
            dic.TryGetValue(UserEx.commonCocktail[i].ToLower(), out rgb1);
            if (rgb1 == null) Debug.Log("rgb1 is null " + UserEx.commonCocktail[i].ToLower() + ".");
            for (int j = idx; j < end_idx; j++)
            {
                dic.TryGetValue(cocktail[j].Name.ToLower(), out rgb2);
                if (rgb2 == null) Debug.Log("rgb2 is null " + cocktail[j].Name.ToLower() + ".");
                else cocktail[j].cf += (UserEx.userScore[i] - 3) * CF.UserCF(rgb1, rgb2);
            }
        }
        Debug.Log("5");
        // sorting
        int aaa;
        for (int i = idx; i < end_idx; i++)
        {
            aaa = i;
            for (int j = i; j < end_idx; j++)
            {
                if (cocktail[aaa].cf < cocktail[j].cf)
                    aaa = j;
            }
            temp = cocktail[aaa];
            cocktail[aaa] = cocktail[i];
            cocktail[i] = temp;
        }
        Debug.Log("6");
        for (int i = 0; i < 4; i++)
        {
            newList[i] = cocktail[i];
        }
        return newList;
    }



}