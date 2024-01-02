using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum eScene
{
    Non = -1,
    Initialize,
    Entry,
    Loby,
    Interface,
    Game
}

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject _objLoadSceneOriginal = null;

    public void Initialize()
    {

    }

    public void SceneLoad(string title, string description, eScene eLoadSceneIndex, Action onLoadDoneCallback, Image backgroundImage = null)
    {
        GameObject objLoadScene = Instantiate(_objLoadSceneOriginal);
        CanvasGroup cg = objLoadScene.GetComponent<CanvasGroup>();

        LoadScene loadScene = objLoadScene.GetComponent<LoadScene>();
        loadScene.Intialize(title, description, backgroundImage);
        loadScene.SetLoadDoneCallback(() =>
        {
            onLoadDoneCallback?.Invoke();

            Debug.Log(eLoadSceneIndex + "  LOAD DONE");
        });

        loadScene.LoadStart(eLoadSceneIndex);
    }

    public void SceneLoadDone()
    {

    }
}
