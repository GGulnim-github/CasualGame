using System.Collections.Generic;
using UnityEngine;

public class UIManager : Manager<UIManager>
{
    [ReadOnly, SerializeField] UIScene _scene;
    [ReadOnly, SerializeField] List<UIPopup> _popupList = new();
    
    Dictionary<string, GameObject> _preloadPrefab = new();

    public override void Initialize()
    {

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            int listCount = _popupList.Count;
            bool openExitGame = true;
            for (int index = listCount - 1; index >= 0; --index)
            {
                UIPopup popup = _popupList[index];
                if (popup.gameObject.activeSelf)
                {
                    popup.Close();
                    openExitGame = false;
                    break;
                }
            }
            if (openExitGame)
            {
                OpenPopupUI<UIExitGame>();
            }
        }
    }

    public void SetSceneUI(UIScene scene)
    {
        if (_scene == null)
        {
            _scene = scene;
        }
        else
        {
            if (_scene != scene)
            {
                Destroy(_scene.gameObject);
                _scene = scene;
            }
        }
        _scene.transform.SetParent(transform);
    }

    public T OpenSceneUI<T>() where T : UIScene
    {
        T scene;
        if (_scene == null)
        {
            scene = CreateUI(UIType.Scene, typeof(T).Name).GetComponent<T>();
        }
        else
        {
            if (_scene.name == typeof(T).Name)
            {
                scene = _scene.GetComponent<T>();
            }
            else
            {
                scene = CreateUI(UIType.Scene, typeof(T).Name).GetComponent<T>();
            }
        }
        scene.gameObject.SetActive(true);

        return scene;
    }

    public void CloseSceneUI()
    {
        if (_scene != null)
        {
            Destroy(_scene.gameObject);
            _scene = null;
        }
    }

    public T OpenPopupUI<T>() where T : UIPopup
    {
        T popup = GetPopupUI<T>();
        if (popup == null)
        {
            popup = CreateUI(UIType.Popup, typeof(T).Name).GetComponent<T>();
            popup.SetCanvasSortOrder(_popupList.Count);
            popup.transform.SetParent(transform);

            _popupList.Add(popup);            
        }
        popup.gameObject.SetActive(true);

        return popup;
    }

    public UIPopup OpenPopupUI(string popupName)
    {
        UIPopup popup = GetPopupUI(popupName);
        if (popup == null)
        {
            popup = CreateUI(UIType.Popup, popupName).GetComponent<UIPopup>();
            popup.SetCanvasSortOrder(_popupList.Count);
            popup.transform.SetParent(transform);

            _popupList.Add(popup);
        }
        popup.gameObject.SetActive(true);

        return popup;
    }

    public void ClosePopupUI<T>() where T : UIPopup
    {
        T popup = GetPopupUI<T>();

        if (popup != null)
        {
            ClosePopupUI(popup);
        }
    }

    public void ClosePopupUI(UIPopup popup)
    {
        int index = popup.GetCanvasSortOrder();

        _popupList.Remove(popup);
        Destroy(popup.gameObject);

        for (int i = index; i < _popupList.Count; i++)
        {
            _popupList[i].SetCanvasSortOrder(i);
        }
    }

    public void HidePopupUI<T>() where T : UIPopup
    {
        T popup = GetPopupUI<T>();

        popup?.gameObject.SetActive(false);
    }

    public T GetPopupUI<T>() where T : UIPopup
    {
        for (int i = 0; i < _popupList.Count; i++)
        {
            if (_popupList[i].name == typeof(T).Name)
            {
                return _popupList[i].GetComponent<T>();
            }
        }
        return null;
    }

    public UIPopup GetPopupUI(string popupName)
    {
        for (int i = 0; i < _popupList.Count; i++)
        {
            if (_popupList[i].name == popupName)
            {
                return _popupList[i].GetComponent<UIPopup>();
            }
        }
        return null;
    }

    GameObject CreateUI(UIType type, string prefabName)
    {
        GameObject prefab = GetPrefab(type, prefabName);
        GameObject ui = Instantiate(prefab);
        ui.name = prefabName;
        return ui;
    }

    string GetPrefabPath(UIType type, string prefabName)
    {
        return $"UI/{type}/{prefabName}";
    }

    GameObject GetPrefab(UIType type, string prefabName)
    {
        if (_preloadPrefab.TryGetValue(prefabName, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            return Resources.Load<GameObject>(GetPrefabPath(type, prefabName)) 
                ?? throw new MissingReferenceException($"UI {type} not found for {prefabName}");
        }
    }
}
