﻿using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Logger
{
    private static ILogger logSystem =
#if UNITY_EDITOR
    new UnityLog();
#else
    new FileLogger();
#endif

    public static void Error(string msg)
    {
        logSystem.Error(msg);
    }

    public static void Error(Exception e)
    {
        logSystem.Error(e);
    }

    public static void Info(string msg)
    {
        logSystem.Info(msg);
    }

    public static void Warning(string msg)
    {
        logSystem.Warning(msg);
    }

    public static void Comment(string msg)
    {
        logSystem.Comment(msg);
    }
    
    public static void Indent(int amount = 1)
    {
        logSystem.Indent(amount);
    }
}