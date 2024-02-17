using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace AIAD.SL
{
    public static class SL_ScriptManager
    {
        public static string ScriptsSerializationPath;
        public static string ScriptsFilesType;
       
        public static string[] DeserializeScriptById(string scriptId)
        {
            string[] commands;
            using(StreamReader reader= new StreamReader(ScriptsSerializationPath+scriptId+ ScriptsFilesType))
            {
                string scriptSyntax=reader.ReadToEnd().FilterBy('\r');
                commands= scriptSyntax.Split('\n');
            }
            return commands;
        }
        public static void ExecuteScriptById(string scriptId)
        {
            SL_Executor.ExecuteCommandsList(DeserializeScriptById(scriptId));
        }
    }
}
