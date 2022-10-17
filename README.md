# Grpc.Demo

This is my quick shot at creating a gRPC Client & Server app in .NET 6.
The Application basically generates random weather data for a random european capital city, and it does that while using gRPC communication 
between Grpc.Client and Grpc.Server APIs.

As a disclaimer, my sole interest was to explore what gRPC has to offer, its perks and downsides. So there was no focus on clean code & architecture.

On a more technical note, the Grpc.Client API exposes 2 HTTP mehods, so this demonstration can be used in a "plug & play" fashion:

![image](https://user-images.githubusercontent.com/51697555/196144465-d4abef73-6406-4420-a2d6-2446cae488f8.png)

The first call "/api/Weather", as it can be seen in the snip above, uses a simple gRPC Unary Request. Meaning the client api fires the call, waits for the server api to respond,
and the connection is being closed. In other words it works in a similar fashion with a simple HTTP REST call.

However the interesting part is the second call: "/api/WeatherStream". This one uses a gRPC Server-Side Stream Request. 
It starts when the client api fires this call and keeps the connection open. The server api receives the request, and streams the result back to the
caller. When all data is being sent (or the request is cancelled) the connection is automatically closed.

There are some 1000ms thread sleeps so this can be clearly seen in the console logs:
(on the left side is the client api, on the right side is the server api)
![image](https://user-images.githubusercontent.com/51697555/196147263-4161729f-c81e-4d32-a676-21e4c1227ce3.png)

