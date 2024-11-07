using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_Actions : Actions
{
    [SerializeField] private string sceneTarget;

    public override void Act()
    {
        DataManager.instance.SetPrevSceneName(SceneManager.GetActiveScene().name);
        GameManager.instance.SaveAllInteractableStates();
        GameManager.instance.SaveAllClueStates();
        DataManager.instance.LevelManager.SceneLoad(sceneTarget);   
    }
}
