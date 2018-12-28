using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CF : ScriptableObject{

	public static float UserCF(ARGB rgb1, ARGB rgb2)
    {
        float result = 0f;
            int top = 0;
        int[] user1 = new int[] { rgb1.A, rgb1.R, rgb1.G, rgb1.B };
        int[] user2 = new int[] { rgb2.A, rgb2.R, rgb2.G, rgb2.B };


        for (int i = 0; i < user1.Length; i++)
        {
            top += user1[i] * user2[i];
            Debug.Log(user1[i] + "vs " + user2[i]);
        }
        

        result = (float)top / (VectorSize(user1) * VectorSize(user2));
        Debug.Log("cf"+result);
        return result;
    }
    
    public static float VectorSize(int[] vec)
    {
        int result = 0;
        for (int i = 0; i < vec.Length; i++)
            result += vec[i] * vec[i];

        return Mathf.Sqrt(result);

    }
    
}
