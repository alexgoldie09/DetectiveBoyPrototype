using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        HidePanel();
    }

    [SerializeField] private GameObject loadPanel;
    [SerializeField] private RectTransform loadBar;

    private Vector3 barScale = Vector3.one;

    public void SceneLoad(string _sceneName)
    {
        StartCoroutine(AsyncLoading(_sceneName));
    }

    private IEnumerator AsyncLoading(string _sceneName)
    {
        ShowPanel();

        yield return new WaitForEndOfFrame();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress/0.9f);

            UpdateBar(progress);

            yield return null;
        }

        HidePanel();
    }

    private void UpdateBar(float _value)
    {
        barScale.x = _value;

        loadBar.localScale = barScale;
    }

    private void ShowPanel()
    {
        loadPanel.SetActive(true);
    }

    private void HidePanel()
    {
        loadPanel?.SetActive(false);
    }
}
