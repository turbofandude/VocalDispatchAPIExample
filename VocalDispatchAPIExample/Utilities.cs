using System;
using System.Reflection;
using LSPD_First_Response.Mod.API;

namespace VocalDispatchAPIExample
{
    public class Utilities
    {        
        //This is what we'll send to SetupVocalDispatchAPI to safely connect unsafe events in VocalDispatchAPI.cs from safe events in MyCallout.cs
        public delegate bool VocalDispatchEventDelegate();
        //Use this function to check if an LSPDFR plugin is installed and running, as well as possibly check for a minimum version
        public static bool IsLSPDFRPluginRunning(string Plugin, Version minversion = null)
        {
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {
                AssemblyName an = assembly.GetName();
                if (an.Name.ToLower() == Plugin.ToLower())
                {
                    if (minversion == null || an.Version.CompareTo(minversion) >= 0)
                        return true;
                }
            }
            return false;
        }        
        public static Assembly ResolveAssemblyEventHandler(object sender, ResolveEventArgs e)
        {            
            foreach (Assembly assembly in Functions.GetAllUserPlugins())
            {   
                if (assembly.GetName().FullName.ToLower() == e.Name.ToLower() || assembly.GetName().Name.ToLower() == e.Name.ToLower())                 
                    return assembly;                
            }            
            return null;
        }
    }
}
