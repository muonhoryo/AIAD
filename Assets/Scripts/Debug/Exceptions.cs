using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIAD.Exceptions
{
    public class AIADException : Exception
    {
        private AIADException() { }
        public AIADException(string message, string sourceName = "")
        {
            Debug.Log(message + SourceShowingMessage(sourceName));
        }

        public static string SourceShowingMessage(string sourceName)
        {
            return "Source: " + sourceName;
        }
    }
    public class AIADMissingModuleException : AIADException
    {
        public AIADMissingModuleException(string moduleName, string sourceName = "")
            : base(MissingModuleMessage(moduleName), sourceName) { }

        public static string MissingModuleMessage(string moduleName)
        {
            return $"Cannot find module by name {moduleName}.";
        }
        public static string MissingModuleMessage_Full(string moduleName, string sourceName)
        {
            return MissingModuleMessage(moduleName) + SourceShowingMessage(sourceName);
        }
    }

}
