﻿//using System;

//namespace GameEstate
//{
//    /// <summary>
//    /// Debug
//    /// </summary>
//    public class Debug
//    {
//        static Debug() => Estate.Bootstrap();

//        public static Action<bool> AssertFunc;
//        public static Action<string> LogFunc;
//        public static Action<string, object[]> LogFormatFunc;
//        public static void Assert(bool condition) => AssertFunc(condition);
//        public static void Log() { }
//        public static void Log(string format) => LogFunc(format);
//        public static void LogFormat(string format, params object[] args) => LogFormatFunc(format, args);
//    }
//}