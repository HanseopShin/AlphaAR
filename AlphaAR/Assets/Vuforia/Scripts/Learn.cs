﻿using UnityEngine;
using System.Collections;
using Vuforia;
using System;

public class Learn : MonoBehaviour
{
    GameObject childObject = null;
    GameObject curObject = null;
    GameObject target;
    GameObject sound;
    GameObject swapObject = null;
    GameObject[] objects;
    SortedList mapped = new SortedList();
    String[,] objectList = { { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" }, { "none", "none", "none", "none", "none", "none", "none", "none", "none", "none" } };

    String curMsg = "";
    String objectStr = "";
    String middleTargetName= "";
    int objectIdx = 0;

    bool lossFlag = false;
    bool flag = false;
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Alphabet");
        objects = GameObject.FindGameObjectsWithTag(ThemeScript.theme);
        sound = GameObject.FindGameObjectWithTag("Sound");
        swapObject = GameObject.FindGameObjectWithTag("Swap");
        if (swapObject != null)
        {
            swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx,objectIdx]));
        }
        objectStr = objectList[ThemeScript.themeIdx, 0];
    }

    void OnGUI()
    {
        int height = Screen.height;
        int width = Screen.width;
        String swapStr = "<size=30>SWAP</size>";
        String soundStr = "<size=30>SOUND</size>";
        String nextStr = "<size=30>NEXT</size>";
        String returnMenu = "<size=30>RETURN TO MENU</size>";

        if (objectIdx == objects.Length)
        {
            objectIdx = 0;
        }

        //GUI버튼 생성 클릭 하면 참
        if (GUI.Button(new Rect(10, 10, 300, 80), swapStr))
        {
            if (DefaultTrackableEventHandler.trackableItems.Count != 0)
            {
                curMsg = DefaultTrackableEventHandler.ChkMsg();
                middleTargetName = DefaultTrackableEventHandler.getTargetinMiddleString();
                Debug.Log("cur msg : " + curMsg);
                Debug.Log("가운데 이름 : " + DefaultTrackableEventHandler.getTargetinMiddleString());

                if (curMsg.Equals(objectStr))
                {
                    for (int i = 0; i < target.transform.childCount; i++)
                    {
                        childObject = getChildObject(target, i);
                        childObject.GetComponent<ImageFlag>().setChildFalse();
                    }

                    for (int i = 0; i < objects.Length; i++)
                    {
                        if (objects[i].name.Equals(curMsg))
                        {
                            curObject = objects[i];
                            break;
                        }
                    }
                    Debug.Log("middle char is " + middleTargetName);
                    for (int i = 0; i < target.transform.childCount; i++)
                    {
                        childObject = getChildObject(target, i);
                        if (childObject.name.Substring(11, 1).Equals(middleTargetName.Substring(11, 1)))
                        {
                            DefaultTrackableEventHandler.setStateLoss(false);
                            Debug.Log(curObject.name + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                            Debug.Log(childObject.transform + "paaaaaaaaaaaaaaaareeeeeeeeeeeeeen");
                            curObject.transform.parent = childObject.transform;
                            curObject.transform.localPosition = new Vector3(0, 1, 0);
                            curObject.transform.localRotation = Quaternion.identity;
                            curObject.transform.localScale = new Vector3(1, 1, 1);
                            curObject.SetActive(true);
                            break;
                        }

                    }

                }
                else
                {
                    Debug.Log("invalid!");
                }
            }
        }

        if (GUI.Button(new Rect(10, 110, 300, 80), soundStr))
        {
            for (int i = 0; i < sound.transform.childCount; i++)
            {
                childObject = getChildObject(sound, i);
                if (childObject.name.Equals(objectStr))
                {
                    childObject.GetComponent<AudioSource>().Play();
                    break;
                }
            }

        }

        if (GUI.Button(new Rect(10, 210, 300, 80), nextStr))
        {
            objectIdx++;
            if (objectIdx == objects.Length)
            {
                objectIdx = 0;
            }
            objectStr = objectList[ThemeScript.themeIdx, objectIdx];
            for (int i = 0; i < objects.Length; i++)
            {
                if (objectStr.Equals(objects[i].name))
                {
                    DefaultTrackableEventHandler.setStateLoss(false);
                    swapObject.GetComponent<Swap>().setModel(objects[i]);
                    break;
                }
            }
            Debug.Log("cur num : " + objectStr);
            flag = true;
        }

        if (GUI.Button(new Rect(10, 310, 300, 80), returnMenu))
        {
            Application.LoadLevel("Intro");
        }

        if (flag)
            GUI.Label(new Rect(510, 10, 700, 210), "<size=150>" + objectStr + "</size>");
    }

    public GameObject getChildObject(GameObject obj, int idx)
    {
        return obj.transform.GetChild(idx).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        lossFlag = DefaultTrackableEventHandler.getStateLoss();
        if(lossFlag==true){
            for (int i = 0; i < target.transform.childCount;i++){
                Debug.Log("innnnnnnnnnnnnnnnnnnnnnnn");
                childObject = getChildObject(target, i);
                childObject.GetComponent<ImageFlag>().setChildTrue();
                childObject.GetComponent<ImageFlag>().setNumberChildFalse();
                swapObject.GetComponent<Swap>().setModel(GameObject.Find(objectList[ThemeScript.themeIdx, objectIdx]));
            }
        }else{
            for (int i = 0; i < objects.Length; i++){
                if(objects[i].name.Equals(objectStr)){
                    objects[i].SetActive(true);
                    break;
                }
            }
        }
    }
}