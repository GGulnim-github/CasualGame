using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class UIBase : MonoBehaviour
{
    public int GetCanvasSortOrder()
    {
        return GetComponent<Canvas>().sortingOrder;
    }
    public void SetCanvasSortOrder(int order)
    {
        GetComponent<Canvas>().sortingOrder = order;
    }
}
