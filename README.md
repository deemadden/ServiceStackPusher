ServiceStackPusher
==================

A command shell ServiceStack-based REST Service Example that saves a POST body to ORMLite.

This is a Visual Studio 2013 Solution.

##ServiceStackPusher REST API
It has a single POST endpoint:
http://[localhost, 127.0.0.1 or ip]:1337/register/:devicetype?format=json

Headers:
Content-Type: application/json

Request body:
{
  "deviceToken" : "smartphonedevicepushtoken"
}

*devicetype parameter:* Expects "iphone" or "android" as a device type parameter

###How to run
1. Open Visual Studio as Administrator, then open solution
2. Update all of the NuGet packages
3. Clean, build, and Start
4. From the REST client of your choice, execute a POST following the API spec above

The command shell output should look something like (assuming POST was done using "iphone" as parameter):
```
DEBUG: Registering OneWay service 'DeviceTokenRegistrationService' with request 'DeviceTokenRequest'
INFO: Initializing Application took 640.5708ms
AppHost Created at 4/19/2014 4:44:49 PM, listening on http://*:1337/
INFO: [your ip]:1337 Request : /register/iphone?format=json
INFO: Received Request from Device iphone
INFO: Token saved!
INFO: Name: iphone PushToken: iphonetoken DateSaved: 04-19-14 04:54:32
```

###How to verify token was saved
Using your SQLite client of choice, open the db.sqlite database located at ServiceStackPusher2\bin\Debug or ServiceStackPusher2\bin\x86\Debug.  It should have a table named DeviceTokenEntity, with a record for the POST you performed. the url parameter will be stored in the Name column, and the deviceToken key value in your request body will have been saved to the PushToken column.
