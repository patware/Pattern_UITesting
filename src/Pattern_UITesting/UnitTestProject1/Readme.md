# Setting up this UI Test

This UnitTest Project is actually a UI Test.  Some setup is required.

## WinAppDriver

The UnitTest1 uses Appium to call WinAppDriver that takes care of launching the WpfClient and finding its UI Controls.

* Appium: installed using the Nuget package: Appium.WebDriver
* WinAppDriver: separate install: https://github.com/microsoft/WinAppDriver
	* Download the latest (I used the .msi)
	* Run the .msi installer
	* Launch the WinAppDriver.  On my machine it was installed on "C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe"
	* Note the port.  It should indicate something like 4723.  You'll need it in the launch options, UnitTest1 line 45

Note that the WinAppDriver docs on github have obsolete samples.  https://github.com/microsoft/WinAppDriver/blob/master/Docs/AuthoringTestScripts.md
