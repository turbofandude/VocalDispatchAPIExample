##About the Example Project
The code provided in this repository can be built and installed directly as a callout for LSPDFR. Assuming you have VocalDispatch installed, when it loads, if you accept the callout, it registers its own Code2Backup function and displays a notification when you request Code 2 backup verbally. Although this example project is a callout, the VocalDispatch API has practical uses far beyond custom dispatch during callouts.

The project references several dependencies. You can resolve these assemblies, after ensuring you have met the runtime prerequisites, by copying VocalDispatch.dll from {Grand Theft Auto V}\plugins\lspdfr, [VocalDispatch.xml](https://github.com/turbofandude/VocalDispatchAPIExample/blob/master/VocalDispatchAPIExample/Dependencies/VocalDispatch.XML), LSPD First Response.dll from {Grand Theft Auto V}\plugins, RagePluginHookSDK.dll from {Grand Theft Auto V}\plugins, and RagePluginHookSDK.xml from {Grand Theft Auto V}\plugins into the "Dependencies" folder in the project.

##API Guide
###Runtime Prequisites
You'll (obviously) need Grand Theft Auto V, [RagePluginHook](http://ragepluginhook.net/Downloads.aspx), [LSPDFR](http://www.lcpdfr.com/files/file/7792-lspd-first-response/), and [VocalDispatch](www.lcpdfr.com/files/file/10593-vocaldispatch/).

###Development Prerequisites
To get started on a project that uses the VocalDispatch API, you'll need to setup an LSPDFR plugin project. For more information on this, see [this forum topic](http://www.lcpdfr.com/forums/topic/52906-api-quick-start-guide-example-project/).

###Setting Up
These instructions assume you have the references/dependencies required for an LSPDFR plugin setup properly already.
  1. Copy VocalDispatch.dll (from {Grand Theft Auto V}\plugins\lspdfr) and [VocalDispatch.xml](https://github.com/turbofandude/VocalDispatchAPIExample/blob/master/VocalDispatchAPIExample/Dependencies/VocalDispatch.XML) into the folder you want to store dependencies in.
  2. Add VocalDispatch.dll as a reference to your project (see below). 
  3. Add [Utilities.cs](https://github.com/turbofandude/VocalDispatchAPIExample/blob/master/VocalDispatchAPIExample/Utilities.cs) and [VocalDispatchHelper.cs](https://github.com/turbofandude/VocalDispatchAPIExample/blob/master/VocalDispatchAPIExample/VocalDispatchHelper.cs) to your project.
  
###Adding a Reference to a Project
This can be accomplished by right clicking the "References" item in the project and clicking on "Add Reference", then clicking "Browse" on that dialog and looking for the DLLs.

![alt text](http://i.imgur.com/xdNVagU.png "Right click the project and click Add Reference")
![alt text](http://i.imgur.com/4dtUbxb.png "Click Browse and look for the DLLs")

###Writing the Code
  1. Setup your AssemblyResolve function like this: `AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Utilities.ResolveAssemblyEventHandler);`. This is demonstrated in the Initialize function in [Main.cs](https://github.com/turbofandude/VocalDispatchAPIExample/blob/master/VocalDispatchAPIExample/Main.cs). This tells the .NET Framework what function should be called when it tries to load an assembly (in this case, VocalDispatch). We're using a custom function here so that we can resolve assembly requests to plugins loaded by LSPDFR - something that can't be done automatically. Feel free to use the function I've supplied as is.
  2. Write your code to honor the request the user has made via VocalDispatch. This will be the main portion of your plugin.
  3. Add a VocalDispatchHelper object to your class and initialize it like this: `VocalDispatchHelper MyVocalDispatchHelper = new VocalDispatchHelper();`. We use a helper class to help prevent issues if your plugin is used and VocalDispatch isn't loaded.
  4. In your `OnCalloutAccepted()` function (for a callout) or your initialization function (for a utility), use the `VocalDispatchHelper.SetupVocalDispatchAPI` function to register your event handler. An example of this is in [MyCallout.cs](https://github.com/turbofandude/VocalDispatchAPIExample/blob/master/VocalDispatchAPIExample/MyCallout.cs). It wants the string of an event name that that corresponds to an event in the script XML file(s) and a function to call when it's heard.
  5. In your `End()` function (for a callout) or your cleanup function (for a utility), use the `VocalDispatchHelper.ReleaseVocalDispatchAPI()` function cleanup. This tells VocalDispatch not to attempt to call your code anymore and will free up any resources associated with it.
  
###Adding Custom Events to the XML File
VocalDispatch automatically loads all XML files in the {Grand Theft Auto V}\plugins\lspdfr\VocalDispatch\ folder. You can use this to your advantage to add additional events or phrases without modifying default information. This is especially useful for third party utilities that want to add a new request a user can make verbally. **This part of the guide is not reflected in the example project's code**.
  1. Add an XML file for your events. For the example, let's call it "VocalDispatchAPIExample.xml".
  2. Setup the phrase(s) and event(s) you want. For the example, we'll setup a function for when the user requests a coroner. The XML for that might look like this:
```xml
<?xml version="1.0"?>
<VocalDispatch>
    <Phrases>      
        <Phrase Event="VocalDispatchAPIExample.RequestCoroner" Priority="5">
            <Word PreferredText="requesting" AcceptedAlternates="need,send,request,role,roll" />            
            <Word PreferredText="coroner"/>
        </Phrase>       
    </Phrases>    
</VocalDispatch>
``` 
To add other phrases, you'd simply need to add a new "Phrase" element and populate it with new "Word" elements. Keep in mind, **all words (or one of their accepted alternates) must be heard for an event to trigger**. Do not include connecting words like "a" or "the".
  
###The Phrase Priority System Explained
The "Priority" attribute specifies the order that VocalDispatch should check for your phrase in. This is useful for a situation like this - the Code2Backup event is triggered by "requesting backup", and the Code3Backup event is triggered by "requesting immediate backup", if VocalDispatch checked for the first event first, it would succeed and you would receive Code 2 backup, even though you said "immediate". The priority system allows VocalDispatch to prioritize certain phrases first, only checking for others if higher priority phrases aren't satisfied.  If you're not sure what to use, 5 is a good starting point.

###Adding a Custom Event to the Code
You'll need most of the code setup as discussed above, but use your custom event name when you call the `VocalDispatchHelper.SetupVocalDispatchAPI` function. For example, that might look like this (assuming that MyVocalDispatchHelper is an object defined as a VocalDispatchHelper and SendCoroner is a function in your code):
```c#
MyVocalDispatchHelper.SetupVocalDispatchAPI("VocalDispatchAPIExample.RequestCoroner", new Utilities.VocalDispatchEventDelegate(SendCoroner));
```

##License
VocalDispatch and the VocalDispatchAPIExample are governed by a modified version of the MIT License. The license is as follows:
Modified MIT License

Copyright (c) 2016 Collin Biedenkapp

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, and distribute, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software and the Software is not used commercially or sold.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
