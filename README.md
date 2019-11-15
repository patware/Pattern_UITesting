# Pattern: UITesting

A Rich client application that is part of a nTier architecture is difficult to UI Testing - Would there be a pattern that would allow this efficiently ?

## UI Testing Client Apps

Client Apps that are not dependent of other systems: Notepad, Calc, etc.  Are easy to test: launch the app, simulate keystrokes and/or buttons, validate UI, close app.  done.

In a classic 3 Tier architecture like this

![Deployed](https://github.com/patware/Pattern_UITesting/blob/master/images/Deployed.png)

you have a client app (1) talking to a business tier (2) which leverages data from a database (3).  When doing any UI Testing in the Client App, the problem is that it expects a deployed, configured and functioning Business App, which in turn requires a database with actual data.

1. All the pieces (tiers) need to be deployed, configured and available in order for the Coded UI tests to work.
2. Running different UI tests in parallel for efficiency becomes tricky when all the Client apps under test all talk to the same backend (unless you deploy the full stack):

    * You have to make sure that the test data are not colliding: working on the same order/person/etc.
    * You can't perform an action that will break the expected status of the other UI tests, ex: putting a business tier in "maintenance mode", running UI tests actions on reference data that are used by other tests, you see the idea

3. Really difficult to simulate outages (network, locks, concurrency) unless you "unplug" the network cord, kill a process, etc.
4. Some systems require an extensive amount of seeding data just to start the app (business)
5. Some systems are way more than just 3 tier, with many Windows Services, Web Services, ETL process with other database, you name it.  A lot of complexity just testing a button don't you think ?
6. etc. etc. etc.

So what can we do ?  If we want to run the Coded UI tests during a Continuous Integration, Continuous Testing, Continuous Deployment, we need to be pragmatic and isolate the Rich Client under test.

## Mocking the Business Tier

A Client app will connect to Business Tiers in various ways, but modern methods would be:

* REST API - [Web API](https://dotnet.microsoft.com/apps/aspnet/apis)
* RPC [gRPC](https://grpc.io/)
* WCF (not modern any more, but so powerful) for [.net Framework](https://docs.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf) and now for [CoreWCF](https://github.com/CoreWCF/CoreWCF) or [dotnet/wcf](https://github.com/dotnet/wcf)
* Service Reference

All of these methods usually include a Client Proxy and/or contract of the form of an Interface.  So, what if we could [Mock](https://en.wikipedia.org/wiki/Mock_object) that business tier using that contract ?

![Mocked](https://github.com/patware/Pattern_UITesting/blob/master/images/Mock.png)

The UI Test would then:

* Start the Mock\<Business>
* Start the Client App
* Point the Client's connection to the URI of the running Mock\<Business> (instead of the real business server implementation)
* Instruct the Mock\<Business> to return the known value for the Unit Test
* Invoke proper UI Actions (keystrokes/buttons/etc) on the Client App that would at one point call the business tier via the Proxy/Interface which points to....  The Mock\<Business> which is owned by....  the UI Test !

## Appium.WebDriver

In our scenario above, we have a UnitTest project (well, a Unit Test project doing some UI Testing) that will automate UI actions.  I use Appium and WinAppDriver to achieve this UI Test automation.

Appium.WebDriver is installed via the NuGet package "Appium.WebDriver".  More details at [appium.io](http://appium.io/docs/en/drivers/windows/).

Note: Yes, we're using the "Appium.WebDriver" to launch a WPF exe, it's because the "WindowsDriver" is that Nuget package.  Confusing.

The UnitTest1's TestMethod1 does the following:

1. Runs a mockup (a fake) of the Ping GRPC Service
2. Sets a return value that the mocked Ping will return (this is the cool part 1)
3. Sets up Appium's to launch the WpfClient (using the WinAppDriver, see bellow)
4. Appium connects to WinAppDriver that launches the WpfClient
5. Invokes the Ping Button on the WpfClient, which opens a GRPC client to server call to the Unit Test's mocked up Ping GRPC service (the cool part 2)
6. Verifies that the Ping Label text is the one in steps 2.

## WinAppDriver

Appium can be used to automate calls to popular Web browsers, Windows apps and mobile apps.  Very cool.  

Appium does not launch the Windows apps.  For that, it relies on the [WinAppDriver](https://github.com/Microsoft/WinAppDriver).  The winAppDriver takes care of starting the exe and executing Appium's generic "find this control" using the proper windows technology associated to the exe (Win32 vs Wpf vs UWP).

Before running the UnitTest, you need to download, install and run the WinAppDriver.
