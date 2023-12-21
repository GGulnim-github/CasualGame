using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationLogSystem
{
    public void OnEnable()
    {
        Application.logMessageReceived += LogMessageAction;
    }

    public void OnDisable()
    {
        Application.logMessageReceived -= LogMessageAction;
    }

    void LogMessageAction(string condition, string stackTrace, LogType type)
    {
        if (type == LogType.Warning || type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
        {
            // TODO : 경고 메세지나 에러 메세지가 출력될때 서버로 로그를 던진다.
        }
    }
}
