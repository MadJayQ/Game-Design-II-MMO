using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Assertions;

public class ConsoleWindowConfiguration : ScriptableObject {

    protected Encoding Encoding { 
        get; set;
    } 
    public virtual void Initailize() {
        try {
            var fileStream = InitializeFileStream();
            var standardOutput = new StreamWriter(fileStream, Encoding);
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            Application.logMessageReceived += OnLogReceived;
        } catch(System.Exception ex) {
            //Assert.IsTrue(false, "Could not redirect output!");
            return;
        }
    }
    protected virtual FileStream InitializeFileStream() {
        return null;
    }

    protected virtual void OnLogReceived(
        string message, 
        string stackTrace, 
        LogType type) {
            System.Console.WriteLine(message);
    }
}
