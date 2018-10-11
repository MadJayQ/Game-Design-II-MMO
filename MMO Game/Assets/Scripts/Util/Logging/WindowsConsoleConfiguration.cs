using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Assertions;

namespace Windows {
    using Microsoft.Win32.SafeHandles;

    [CreateAssetMenu(menuName = "MMO Utilities/Logging/Windows Console")]
    public class WindowsConsoleConfiguration : ConsoleWindowConfiguration {
        protected override FileStream InitializeFileStream() {
            IntPtr standardOutHandlePtr = GetStdHandle(STD_OUTPUT_HANDLE);
            //We're going to safely acquire a file handle to our output stream
            SafeFileHandle standardOutHandle = new SafeFileHandle( 
                standardOutHandlePtr,
                true
            ); 

            return new FileStream(standardOutHandle, FileAccess.Write);
        }

        public override void Initailize() {
            AllocConsole();
            base.Initailize();
        }
        private const int STD_OUTPUT_HANDLE = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleTitle(string lpConsoleTitle);
    }
}