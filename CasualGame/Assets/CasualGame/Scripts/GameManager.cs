using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager> 
{
    public override void Initialize()
    {
        UIManager.Instance.OpenSceneUI<UITitle>();
    }
}
