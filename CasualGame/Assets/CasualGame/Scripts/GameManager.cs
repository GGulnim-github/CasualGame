using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager> 
{
    void Start()
    {
        ApplicationManager.Instance.Initialize();
        LocalizationManager.Instance.Initialize();
        SoundManager.Instance.Initialize();
        UIManager.Instance.Initialize();
    }
}
