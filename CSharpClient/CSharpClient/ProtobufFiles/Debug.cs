using System;

public class Debug
{
    public static bool enableLog = true;
    public static bool enableWarning = true;
    public static bool enableError = true;

    public static void Log(string text)
    {
        if (!enableLog) return;
        // Console.WriteLine(GetStackTraceModelName());
        Console.WriteLine(text);
    }

    public static void LogWarning(string text)
    {
        if (!enableWarning) return;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void LogError(string text)
    {
        if (!enableError) return;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    /// summary 
    /// 获取当前堆栈的上级调用方法列表,直到最终调用者,只会返回调用的各方法,而不会返回具体的出错行数，
    /// 可参考：微软真是个十足的混蛋啊！让我们跟踪Exception到行把！
    /// /summary 
    static string GetStackTraceModelName()
    {
        //当前堆栈信息
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
        System.Diagnostics.StackFrame[] sfs = st.GetFrames();

        //过虑的方法名称,以下方法将不会出现在返回的方法调用列表中
        string _filterdName = "ResponseWrite,ResponseWriteError,";
        string _fullName = string.Empty, _methodName = string.Empty;

        for (int i = 0; i < sfs.Length; ++i)
        {
            //非用户代码,系统方法及后面的都是系统调用，不获取用户代码调用结束
            if (System.Diagnostics.StackFrame.OFFSET_UNKNOWN == sfs[i].GetILOffset()) break;

            _methodName = sfs[i].GetMethod().Name; //方法名称
            //sfs[i].GetFileLineNumber();//没有PDB文件的情况下将始终返回0

            if (_filterdName.Contains(_methodName)) continue;

            _fullName = _methodName + "()- " + _fullName;
        }

        st = null;
        sfs = null;
        _filterdName = _methodName = null;
        return _fullName.TrimEnd('-', ' ');
    }
}