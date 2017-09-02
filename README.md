# WoodenMoose.FunctionalExtensions
Set of extension methods that allows functional programming in C#

Inspired by https://fsharpforfunandprofit.com/rop/

Installation
-------
A NuGet package of the library is available here : https://www.nuget.org/packages/WoodenMoose.FunctionalExtensions/

Railway Oriented Programming (ROP)
-------
*Inspired by https://fsharpforfunandprofit.com/rop/*

Compose your methods into a chain that will correctly handle error cases.
If one method fails (by returning a Failure value), it will immediately stop the execution of the following methods and instead return the reason why the method failed.

```csharp
var ropFunc = FuncHelper.AsFunc(ReadConsole).Then(ConvertToInt).Then(Times2);
var result = railwayOrientedFunc();
```

```csharp
public static Response<string> ReadConsole()
{
    var line = Console.ReadLine();
    return Response.Success(line);
}

public static Response<int> ConvertToInt(Response<string> response)
{
    if (int.TryParse(response.Data, out int i))
    {
        return Response.Success(i);
    }
    else
    {
        return Response.Failure<int>($"{response.Data} is not an integer");
    }
}

public static Response<int> Times2(Response<int> response)
{
    if (response.Data < 10)
    {
        return Response.Success(response.Data * 2);
    }
    else
    {
        return Response.Failure<int>($"{response.Data} is bigger than 9");
    }
}
```

An asynchronous version is also available

```csharp
var railwayOrientedFuncAsync = FuncHelper.AsFunc(ReadConsole).ThenAsync(ConvertToIntAsync).ThenAsync(Times2);
var result2 = railwayOrientedFuncAsync().Result;

public static async Task<Response<int>> ConvertToIntAsync(Response<string> response)
{
    if (int.TryParse(response.Data, out int i))
    {
        await Task.Delay(5000);
        return Response.Success(i);
    }
    else
    {
        return Response.Failure<int>($"{response.Data} is not an integer");
    }
}
```