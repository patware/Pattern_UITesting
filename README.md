# Pattern: UITesting

NTier application is difficult to UI Testing - Would there be a pattern that would allow this efficiently ?

## UI Testing Client Apps

Client Apps that are not dependent of other systems: Notepad, Calc, etc.  Are easy to test: launch the app, simulate keystrokes and/or buttons, validate UI, close app.  done.

In a classic 3 Tier architecture

![Deployed](.\images\deployed.png)

you have a client app (1) talking to a business tier (2) which leverages data from a database (3).  When doing any UI Testing in the Client App, the problem is that it expects a functioning Business App, which in turn requires a database.

1. All the pieces (tiers) need to be deployed, configured and available in order for the Coded UI tests to work.
2. Running different UI tests in parallel for efficiency becomes tricky when all the Client apps under test all talk to the same backend (unless you deploy the full stack):

    * You have to make sure that the test data are not colliding: working on the same order/person/etc.
    * You can't perform an action that will break the expected status of the other UI tests, ex: putting a business tier in "maintenance mode", running UI tests actions on reference data that are used by other tests, you see the idea

3. Really difficult to simulate outages (network, locks, concurrency) unless you "unplug" the network cord, kill a process, etc.
4. Some systems require an extensive amount of seeding data just to start the app (business)
5. Some systems are way more than just 3 tier, with many Windows Services, Web Services, ETL process with other database, you name it.  A lot of complexity just testing a button don't you think ?
6. etc. etc. etc.

So what can we do ?  If we want to run the Coded UI tests during a Continuous Integration, Continuous Testing, Continuous Deployment, we need to be pragmatic.

## Mocking the Business Tier

A Client app will connect to Business Tiers in various ways, but modern methods would be:

* REST API - [Web API](https://dotnet.microsoft.com/apps/aspnet/apis)
* RPC [gRPC](https://grpc.io/)
* WCF (not modern any more, but so powerful) for [.net Framework](https://docs.microsoft.com/en-us/dotnet/framework/wcf/whats-wcf) and now for [CoreWCF](https://github.com/CoreWCF/CoreWCF) or [dotnet/wcf](https://github.com/dotnet/wcf)
* Service Reference

All of these methods usually include a Client Proxy and/or contract of the form of an Interface.  So, what if we could [Mock](https://en.wikipedia.org/wiki/Mock_object) that business tier using that contract ?

![Mocked](.\images\mock.png)

The UI Test would then:

* Start the Mock\<Business>
* Start the Client App
* Point the Client's connection to the URI of the running Mock\<Business>
* Instruct the Mock\<Business> to return a known value whenever the client requires it
* Invoke proper UI Actions (keystrokes/buttons/etc) on the Client App that would at one point call the business tier via the Proxy/Interface which points to....  The Mock\<Business> which is owned by....  the UI Test !
