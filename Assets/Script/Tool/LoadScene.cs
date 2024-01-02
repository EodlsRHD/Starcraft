using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textTitle = null;

    [SerializeField]
    private TMP_Text _textDescription = null;

    [SerializeField]
    private Slider _loadProgress = null;

    [SerializeField]
    private Image _backgroundImage = null;

    private Action _onLoadDoneCallback = null;

    private CancellationTokenSource disableCancellation = new CancellationTokenSource();

    public void Intialize(string title, string description, Image backgroundImage = null)
    {
        if (backgroundImage != null)
        {
            _backgroundImage = backgroundImage;
        }

        _loadProgress.value = 0;

        _textTitle.text = title;
        _textDescription.text = description;
    }

    public void SetLoadDoneCallback(Action onLoadDoneCallback)
    {
        if (onLoadDoneCallback != null)
        {
            _onLoadDoneCallback = onLoadDoneCallback;
        }
    }

    public void LoadStart(eScene eLoadScene)
    {
        if (GameManager.instance.TEST_MODE == true)
        {
            StartCoroutine(CoLoading(eLoadScene));
        }
        else
        {
            Loading(eLoadScene).Forget();
        }
    }

    private void DoneLoad()
    {
        _onLoadDoneCallback?.Invoke();
    }

    IEnumerator CoLoading(eScene eLoadScene)
    {
        Debug.Log("IEnumerator LoadScene   " + eLoadScene);

        yield return 1f;

        AsyncOperation op = SceneManager.LoadSceneAsync((int)eLoadScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                _loadProgress.value = Mathf.Lerp(_loadProgress.value, op.progress, timer);
                if (_loadProgress.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                _loadProgress.value = Mathf.Lerp(_loadProgress.value, 1f, timer);
                if (_loadProgress.value == 1.0f)
                {
                    op.allowSceneActivation = true;

                    if (eLoadScene == eScene.Loby || eLoadScene == eScene.Game)
                    {
                        SceneManager.LoadScene((int)eScene.Interface, LoadSceneMode.Additive);
                    }

                    DoneLoad();

                    yield break;
                }
            }

            InfiniteLoopDetector.Run();
        }
    }

    private async UniTaskVoid Loading(eScene eLoadScene)
    {
        Debug.Log("UniTaskVoid LoadScene   " + eLoadScene);

        await UniTask.Delay(TimeSpan.FromSeconds(1f), false, PlayerLoopTiming.Update, disableCancellation.Token);

        AsyncOperation op = SceneManager.LoadSceneAsync((int)eLoadScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                _loadProgress.value = Mathf.Lerp(_loadProgress.value, op.progress, timer);
                if (_loadProgress.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                _loadProgress.value = Mathf.Lerp(_loadProgress.value, 1f, timer);
                if (_loadProgress.value == 1.0f)
                {
                    op.allowSceneActivation = true;

                    if (eLoadScene == eScene.Loby || eLoadScene == eScene.Game)
                    {
                        SceneManager.LoadScene((int)eScene.Interface, LoadSceneMode.Additive);
                    }

                    DoneLoad();

                    break;
                }
            }

            InfiniteLoopDetector.Run();
        }
    }
}
