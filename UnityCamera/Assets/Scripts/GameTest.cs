using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTest : MonoBehaviour
{
    // Start is called before the first frame update
    public List<string> tempList = new List<string>() { "a", "c", "b", "a", "b", "b", "b", "c", "b", "c" };
    void Start()
    {
        //for (int i = 0; i < tempList.Count; i++)
        //{
        //    if (tempList[i] == "b")
        //    {
        //        tempList.Remove(tempList[i]);
        //        i--;
        //    }
        ////}
        //foreach (var item in tempList)
        //{
        //    if (item == "b")
        //    {
        //        tempList.Remove(item);
        //    }
        //}
        //tempList.ForEach(p =>
        //{
        //    Debug.Log(p + ",");
        //});
        TEST(delegate (string name)
        {
            Debug.Log(21);
            Debug.Log(name);
            Debug.Log(31);
        });
    }
    void TEST(Action<string> callBack)
    {
        Debug.Log(0);
        callBack.Invoke("hhhh");
        Debug.Log(1);

    }

}
