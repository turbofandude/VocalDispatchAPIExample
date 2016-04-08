using LSPD_First_Response.Mod.Callouts;
using System;

namespace VocalDispatchAPIExample
{
    [CalloutInfo("MyCallout", CalloutProbability.Medium)]
    class MyCallout : LSPD_First_Response.Mod.Callouts.Callout
    {
        Rage.Vector3 spawnPoint;
        //You can add more of these helpers to add additional functions.
        VocalDispatchHelper VDRequestingCode2Backup = new VocalDispatchHelper();

        /// <summary>
        /// Called when VocalDispatch hears the phrase that triggers the event we wanted to be notified of.
        /// </summary>
        /// <returns></returns>        
        public bool VocalDispatchSaysPlayerIsRequestingCode2Backup() //You do not need to call this function. VocalDispatch calls it for you once you've set it up properly.
        {
            //Do your desired logic here. Returning false back to VocalDispatch will tell it to continue handling the request.
            //Sending true back tells it we've handled it.
            //For example, you could spawn some officers on foot and have them walk over, instead of having them drive up, then return true to tell VocalDispatch we handled it.
            Rage.Game.DisplayNotification("MyCallout handled the request for Code 2 Backup.");
            return true;
        }

        /// <summary>
        /// Called before the callout is displayed. Do all spawning here, so that if spawning isn't successful, the player won't notice, as the callout won't be shown.
        /// </summary>
        /// <returns></returns>
        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = Rage.World.GetNextPositionOnStreet(Rage.Game.LocalPlayer.Character.Position);

            this.CalloutMessage = "MyCallout";
            this.CalloutPosition = spawnPoint;

            this.ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 15f);
            this.AddMinimumDistanceCheck(5f, spawnPoint);

            return base.OnBeforeCalloutDisplayed();
        }

        /// <summary>
        /// Called when the player accepts the callout
        /// </summary>
        /// <returns></returns>
        public override bool OnCalloutAccepted()
        {            
            //Check that VocalDispatch 1.5.0.0 or greater is running first to avoid any conflict
            if (Utilities.IsLSPDFRPluginRunning("VocalDispatch", new System.Version(1,5,0,0)) == true)
            {       
                try
                {
                    //Tell our VocalDispatchHelper class to setup the "VocalDispatchSaysPlayerIsRequestingCode2Backup" function as the event handler function.
                    VDRequestingCode2Backup.SetupVocalDispatchAPI("VocalDispatch.RequestCode2Backup", new Utilities.VocalDispatchEventDelegate(VocalDispatchSaysPlayerIsRequestingCode2Backup));
                }
                catch(Exception e)
                {
                    Rage.Game.Console.Print("Error: " + e.ToString());
                }                
            }
           //Do your regular work here
            return base.OnCalloutAccepted();
        }

        /// <summary>
        /// Called when the callout ends
        /// </summary>
        public override void End()
        {
            //Check that VocalDispatch 1.5.0.0 or greater is running first to avoid any conflict
            if (Utilities.IsLSPDFRPluginRunning("VocalDispatch", new System.Version(1, 5, 0, 0)) == true)
            {
                try
                {                    
                    VDRequestingCode2Backup.ReleaseVocalDispatchAPI();
                }             
                catch (Exception e)
                {
                    Rage.Game.Console.Print("Error: " + e.ToString());
                }            
            }
            //Handle any other cleanup for your callout here.
            base.End();
        }

        /// <summary>
        /// All callout logic should be done here.
        /// </summary>
        public override void Process()
        {
            //Do regular callout stuff here.
            base.Process();
        }
    }
}
