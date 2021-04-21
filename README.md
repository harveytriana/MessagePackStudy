# How to implement MessagePack ™ serialization from ASP.NET Core REST services

*It is possible to implement a compact protocol in Web Api services to achieve higher performance in data transfer*

One .NET way to send data in MessagePack serialization is through a SignalR server configured for that. On the other hand, if we want to program REST services for this format, the implementation is different. In this post I explain how to do it, and give an example for a Console client and for a Blazor client.

In an ASP.NET Core Web Api application, we install the `MessagePack.AspNetCoreMvcFormatter` package, then modify `ConfigureServices` to support `MessagePack` serialization.

*Startup*

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().AddMvcOptions(option => {
        option.OutputFormatters.Clear();
        option.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
        option.InputFormatters.Clear();
        option.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
    });

    services.AddControllers();
}
```

Any C# clients: Consoles, Xamarin, ASP.NET, Blazor, Windows Forms, can consume the services serialized in MessagePack according to certain rules. We must install the nuget package `MessagePack` (see reference 1).

### GET method example

```csharp
async Task GetSample()
{
    using var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri(_apiRoot);
    var result = await httpClient.GetAsync("api/Something");
    var bytes = await result.Content.ReadAsByteArrayAsync();
    var data = MessagePackSerializer.Deserialize<List<Model>>(bytes, ContractlessStandardResolver.Options);

    // do something with data ...
}
```

### Example of a POST method

```csharp
async Task PostSample()
{
    var item = new Model { ... };

    using var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri(_apiRoot);
    var bytes = MessagePackSerializer.Serialize(item, ContractlessStandardResolver.Options);
    var byteContent = new ByteArrayContent(bytes);
    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-msgpack");
    var result = await httpClient.PostAsync("Something", byteContent);

    Console.WriteLine("\nResult: {0}", result.StatusCode);
}
```

In the previous examples, `_apiRoot` is the address of the REST service, `Something` is the name of the Api Controller, and finally, `Model` is the data type that the API will deliver.

> We don't necessarily have to use a shared library for the data models, since REST is not a contract. On the client side we can use an abstraction, even in type `record` (as I show in the repository example in the Console example).

Methods in the controller are written normally, except we don't use [FromBody] (the object is a bytes sequence).

> Of course, there is a counterpart to consuming these services in JavaScript or another language, since MessagePack is open source.

### Blazor WebAssembly Client

A Blazor client needs to know the path of the ASP.NET server in `Program`,  and modify the way the API services are read. In the published GIT example you can check how to do it.

The ASP.NET Core server must have the Blazor client address in its CORS allowed list.

*The Blazor client and the console client testing the implementation. To run, use Multiple startup projects as a Solution property.*

![](https://github.com/harveytriana/MessagePackStudy/blob/master/an_mp.png)

### Flats

We gain high transmission efficiency, but we go out of the traditional way of doing things. For example, tools like Swagger or Postman lose their usefulness, since the data is binary. All sending or receiving from ASP.NET server is binary, so clients need to use the rules in REST methods as I showed in this post. We should write an ASP.NET server with MessagePack support only in situations where transfer efficiency is mission critical. However, we have a powerful and not so rigid alternative, such as SignalR.

---

**References**

1. [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp)

2. [https://msgpack.org](https://msgpack.org/)

---

`Licencia MIT. Autor: Harvey Triana. Contacto: admin@blazorspread.net`

*[Blazor Spread Blog](https://www.BlazorSpread.net)*
