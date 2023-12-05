using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationTableSO : ScriptableObject
{
    public LocalizationLanguage language;
    public SerializableDictionary<string, string> localizedStrings = new();
}
