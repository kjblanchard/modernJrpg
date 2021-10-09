using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController 
{
    [System.Serializable]
    public enum GameScenesEnum
    {
        DebugRoom,
        Battle1,
    }

    private static readonly Dictionary<GameScenesEnum, string> gameSceneToSceneNameDictionary = new Dictionary<GameScenesEnum, string>
    {
        {GameScenesEnum.DebugRoom, "DebugRoom"},
        {GameScenesEnum.Battle1, "battle1"}
    };

    public static void ChangeGameScene(GameScenesEnum sceneToChangeTo)
    {
        SceneManager.LoadScene(gameSceneToSceneNameDictionary[sceneToChangeTo]);
    }


}
