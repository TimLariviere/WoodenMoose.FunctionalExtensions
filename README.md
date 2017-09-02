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
var result = ropFunc();
```

```csharp
public static Response<string> ReadConsole()
{
    var line = Console.ReadLine();
    return Response.Success(line);
}

public static Response<int> ConvertToInt(string str)
{
    if (int.TryParse(str, out int i))
    {
        return Response.Success(i);
    }
    else
    {
        return Response.Failure<int>($"{str} is not an integer");
    }
}

public static Response<int> Times2(int number)
{
    if (number < 10)
    {
        return Response.Success(number * 2);
    }
    else
    {
        return Response.Failure<int>($"{number} is bigger than 9");
    }
}
```

An asynchronous version is also available

```csharp
var ropFuncAsync = FuncHelper.AsFunc(ReadConsole).ThenAsync(ConvertToIntAsync).ThenAsync(Times2);
var result2 = await ropFuncAsync();

public static async Task<Response<int>> ConvertToIntAsync(string str)
{
    if (int.TryParse(str, out int i))
    {
        await Task.Delay(5000);
        return Response.Success(i);
    }
    else
    {
        return Response.Failure<int>($"{str} is not an integer");
    }
}
```