using System;
using LSPD_First_Response.Mod.API;

namespace VocalDispatchAPIExample
{
    /// <summary>
    /// Do not rename! Attributes or inheritance based plugins will follow when the API is more in depth.
    /// </summary>

    public class Main : Plugin
    {
        /// <summary>
        /// Constructor for the main class, same as the class, do not rename.
        /// </summary>
        public Main() { }

        /// <summary>
        /// Called when the plugin ends or is terminated to cleanup
        /// </summary>    
        public override void Finally() { }              

        /// <summary>
        /// Called when the plugin is first loaded by LSPDFR
        /// </summary>
        public override void Initialize()
        {
            //Set the AssemblyResolve function (see documentation)
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Utilities.ResolveAssemblyEventHandler);
            
            //Subscribe to the OnOnDutyStateChanged event, so we don't register our callouts unless the player is on duty.
            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        /// <summary>
        /// The event handler mentioned above,
        /// </summary>
        private void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            //If the player is going on duty, register the callout.
            if (onDuty)
            {
                Functions.RegisterCallout(typeof(MyCallout));
            }
        }
    }
}
