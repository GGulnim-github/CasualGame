using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Manager<CameraManager>
{
    PlayerFollowCamera _playerFollowCamera;
    public PlayerFollowCamera PlayerFollowCamera
    {
        get
        {
            if (_playerFollowCamera == null)
            {
                return CreateCamera(typeof(PlayerFollowCamera).Name).GetComponent<PlayerFollowCamera>();
            }
            else
            {
                return _playerFollowCamera;
            }
        }
    }

    public override void Initialize()
    {

    }
    
    GameObject CreateCamera(string prefabName)
    {
        GameObject prefab = GetPrefab(prefabName);
        GameObject camera = Instantiate(prefab);
        camera.name = prefabName;
        return camera;
    }

    string GetPrefabPath(string prefabName)
    {
        return $"Camera/{prefabName}";
    }

    GameObject GetPrefab(string prefabName)
    {
        return Resources.Load<GameObject>(GetPrefabPath(prefabName))
            ?? throw new MissingReferenceException($"Camera not found for {prefabName}");
    }
}

