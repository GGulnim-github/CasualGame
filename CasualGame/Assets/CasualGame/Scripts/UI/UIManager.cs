using System.Collections.Generic;
using UnityEngine;

public class UIManager : PersistentSingleton<UIManager>
{
    List<UIPopup> _uiPopupList = new();
    
    Dictionary<string, GameObject> _preloadPrefab = new();

    public void Initialize()
    {

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            int listCount = _uiPopupList.Count;
            bool openExitGame = true;
            for (int index = listCount - 1; index >= 0; --index)
            {
                UIPopup popup = _uiPopupList[index];
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

    public T OpenPopupUI<T>() where T : UIPopup
    {
        T popup = GetPopupUI<T>();
        if (popup == null)
        {
            popup = CreateUI(UIType.Popup, typeof(T).Name).GetComponent<T>();
            popup.SetCanvasSortOrder(_uiPopupList.Count);
            _uiPopupList.Add(popup);
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

        _uiPopupList.Remove(popup);
        Destroy(popup.gameObject);

        for (int i = index; i < _uiPopupList.Count; i++)
        {
            _uiPopupList[i].SetCanvasSortOrder(i);
        }
    }

    public void HidePopupUI<T>() where T : UIPopup
    {
        T popup = GetPopupUI<T>();

        popup?.gameObject.SetActive(false);
    }

    public T GetPopupUI<T>() where T : UIPopup
    {
        for (int i = 0; i < _uiPopupList.Count; i++)
        {
            if (_uiPopupList[i] is T)
            {
                return _uiPopupList[i].GetComponent<T>();
            }
        }
        return null;
    }

    GameObject CreateUI(UIType type, string prefabName)
    {
        GameObject prefab = GetPrefab(type, prefabName);
        return Instantiate(prefab);
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
