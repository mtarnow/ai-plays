using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public class LoggedInfo
    {
        public Object obj;
        public string msg;

        public LoggedInfo(Object obj, string msg)
        {
            this.obj = obj;
            this.msg = msg;
        }
    }

    static List<LoggedInfo> loggedInfo = new List<LoggedInfo>();
    static string logStr = "";

    public static void LogFromFirst(Object loggingObj, string msg)
    {
        bool exists = false;
        char[] charsToTrim = { ' ', '(', ')', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        foreach (LoggedInfo info in loggedInfo)
        {
            if (info.obj.name.Trim(charsToTrim) == loggingObj.name.Trim(charsToTrim) && info.msg == msg)
            {
                exists = true;
                break;
            }
        }
        if (!exists)
        {
            Debug.Log(loggingObj.name.Trim(charsToTrim) + "\n" + msg);
            loggedInfo.Add(new LoggedInfo(loggingObj, msg));
            //logStr = logStr + ", " + loggingObj.name;
            //Debug.Log(logStr);
        }
    }

    public static void LogFromNamed(bool toDebug, string msg)
    {
        if (!toDebug)
            return;
        Debug.Log(msg);
    }

    public class PastAgentStates
    {
        public float[,] lastActions = new float[20, 2];
        public float[] lastHeights = new float[20];
    }

    public static long actionCounter = 0;
    static bool crazyJump = false;
    public static PastAgentStates pastStates = new PastAgentStates();

    public static void DebugAgent(bool toDebug, RobotAgent agent, float[] action)
    {
        if (!toDebug)
            return;

        int i;
        if (crazyJump == false && agent.transform.position.y > 5.5f)
        {
            string arrayStr = "";
            crazyJump = true;
            for (i = 0; i < 20; i++)
            {
                arrayStr = arrayStr + "[" + pastStates.lastActions[i, 0].ToString() + "] [" + pastStates.lastActions[i, 1].ToString() +
                           "] " + pastStates.lastHeights[i].ToString() +"\n";
            }
            LogFromNamed(toDebug, agent.transform.position.y.ToString() + "\n" + actionCounter.ToString() + "\n" + arrayStr);
        }
        else if (crazyJump == true && agent.transform.position.y < 5.5f) {
            crazyJump = false;
        }
        //LogFromNamed(toDebug, agent.transform.position.y.ToString());

        for (i = 0; i < 19; i++)
        {
            pastStates.lastActions[i, 0] = pastStates.lastActions[i + 1, 0];
            pastStates.lastActions[i, 1] = pastStates.lastActions[i + 1, 1];
            pastStates.lastHeights[i] = pastStates.lastHeights[i + 1];
        }
        pastStates.lastActions[i, 0] = action[0];
        pastStates.lastActions[i, 1] = action[1];
        pastStates.lastHeights[i] = agent.transform.position.y;

        actionCounter++;
    }
}
