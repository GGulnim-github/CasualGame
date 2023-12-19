using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ExcelDataReader;
using UnityEditor;

#if UNITY_EDITOR
public class LocalizationTableCreator : EditorWindow
{
    private static Object localizationTableXlsx;

    Dictionary<int, LocalizationLanguage> _columnMatchLanguage;
    Dictionary<LocalizationLanguage, LocalizationTableSO> _localizationTables;

    string _tableName = "Localization";
    string _tablePath = "Assets/CasualGame/Tables";
    string _savePath = "Assets/CasualGame/Resources/Localization";

    [MenuItem("Localization/Table Creator")]
    public static void OpenWindow()
    {
        LocalizationTableCreator window = GetWindow<LocalizationTableCreator>();
        window.titleContent = new GUIContent("Localization Table Creator");
        window.Show();
    }

    private void OnEnable()
    {
        _columnMatchLanguage = new();
        _localizationTables = new();

        if (localizationTableXlsx == null)
        {
            string[] sAssetGuids = AssetDatabase.FindAssets(_tableName, new[] { _tablePath });
            string[] sAssetPathList = System.Array.ConvertAll(sAssetGuids, AssetDatabase.GUIDToAssetPath);
            foreach (string path in sAssetPathList) 
            {
                if (path[^5..] == ".xlsx")
                {
                    localizationTableXlsx = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                    break;
                }
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        localizationTableXlsx = EditorGUILayout.ObjectField(localizationTableXlsx, typeof(Object), false);
        GUILayout.EndVertical();

        if (GUILayout.Button("Create Table"))
        {
            CreateTable();
        }
    }

    void CreateTable()
    {
        if (localizationTableXlsx == null)
        {
            return;
        }

        _columnMatchLanguage = new();
        _localizationTables = new();

        string path = AssetDatabase.GetAssetPath(localizationTableXlsx);
        FileStream streamer = new(path, FileMode.Open, FileAccess.Read);

        using var reader = ExcelReaderFactory.CreateReader(streamer);

        System.Data.DataTableCollection tables = reader.AsDataSet().Tables;

        System.Data.DataTable sheet = tables[0];

        for (int rowIndex = 0; rowIndex < sheet.Rows.Count; rowIndex++)
        {
            System.Data.DataRow row = sheet.Rows[rowIndex];
            for (int columnIndex = 1; columnIndex < row.ItemArray.Length; columnIndex++)
            {
                if (rowIndex == 0)
                {
                    string language = row.ItemArray[columnIndex].ToString();
                    LocalizationTableSO tableSO;
                    LocalizationLanguage localizationLanguage;

                    localizationLanguage = (LocalizationLanguage)System.Enum.Parse(typeof(LocalizationLanguage), language, true);
                    _columnMatchLanguage.Add(columnIndex, localizationLanguage);

                    tableSO = CreateInstance<LocalizationTableSO>();
                    tableSO.language = localizationLanguage;

                    _localizationTables.Add(localizationLanguage, tableSO);
                }
                else
                {
                    string key = row.ItemArray[0].ToString();
                    string localizedString = row.ItemArray[columnIndex].ToString();

                    if (_columnMatchLanguage.TryGetValue(columnIndex, out LocalizationLanguage language))
                    {
                        if (_localizationTables.TryGetValue(language, out LocalizationTableSO tableSO))
                        {
                            tableSO.localizedStrings.Add(key, localizedString);
                        }
                    }                   
                }
            }
        }

        foreach(var table in _localizationTables.Values)
        {
            AssetDatabase.CreateAsset(table, $"{_savePath}/{table.language}.asset");
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(table));
        }
        Debug.Log("Localization Create Table Finish!!");

        reader.Dispose();
        reader.Close();
    }

}
#endif