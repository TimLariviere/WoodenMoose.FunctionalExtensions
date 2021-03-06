﻿using System;
using System.Threading.Tasks;

namespace WoodenMoose.FunctionalExtensions.Rop
{
    /// <summary>
    /// Asynchronous extensions for function composition
    /// </summary>
    public static class AsyncFuncExtensions
    {
        /// <summary>
        /// Wraps the function into a new one to allow for railway oriented programming
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="func">Function to wrap</param>
        /// <returns>The binded function</returns>
        public static Func<Response<T>, Task<Response<TResult1>>> Bind<T, TResult1>(this Func<T, Task<Response<TResult1>>> func)
        {
            return async value =>
            {
                if (value.Type == ResponseType.Success)
                {
                    return await func(value.Data);
                }
                else
                {
                    return Response<TResult1>.Failure(value.FailureReason);
                }
            };
        }

        #region ThenAsync

        /// <summary>
        /// Chains another function after the current one
        /// void -> T; T -> TResult1
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Task<Response<TResult1>>> ThenAsync<T, TResult1>(this Func<Response<T>> function1, Func<T, Response<TResult1>> function2)
        {
            return async () =>
            {
                var asyncFunc1 = MakeAsync(function1);
                var asyncFunc2 = MakeAsync(function2);
                var bindedFunc2 = Bind(asyncFunc2);
                return await bindedFunc2(await asyncFunc1());
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// void -> T; T -> Task<TResult1>
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the intermediate outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Task<Response<TResult1>>> ThenAsync<T, TResult1>(this Func<Response<T>> function1, Func<T, Task<Response<TResult1>>> function2)
        {
            return async () =>
            {
                var asyncFunc1 = MakeAsync(function1);
                var bindedFunc2 = Bind(function2);
                return await bindedFunc2(await asyncFunc1());
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// void -> Task<T>; T -> TResult1
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Task<Response<TResult1>>> ThenAsync<T, TResult1>(this Func<Task<Response<T>>> function1, Func<T, Response<TResult1>> function2)
        {
            return async () =>
            {
                var asyncFunc2 = MakeAsync(function2);
                var bindedFunc2 = Bind(asyncFunc2);
                return await bindedFunc2(await function1());
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// void -> Task<T>; T -> Task<TResult1>
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Task<Response<TResult1>>> ThenAsync<T, TResult1>(this Func<Task<Response<T>>> function1, Func<T, Task<Response<TResult1>>> function2)
        {
            return async () =>
            {
                var bindedFunc2 = Bind(function2);
                return await bindedFunc2(await function1());
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// T -> TResult1; TResult1 -> TResult2
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<T, Task<Response<TResult2>>> ThenAsync<T, TResult1, TResult2>(this Func<T, Response<TResult1>> function1, Func<TResult1, Response<TResult2>> function2)
        {
            return async value =>
            {
                var asyncFunc1 = MakeAsync(function1);
                var asyncFunc2 = MakeAsync(function2);
                var bindedFunc2 = Bind(asyncFunc2);
                return await bindedFunc2(await asyncFunc1(value));
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// T -> TResult1; TResult1 -> Task<TResult2>
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the intermediate outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<T, Task<Response<TResult2>>> ThenAsync<T, TResult1, TResult2>(this Func<T, Response<TResult1>> function1, Func<TResult1, Task<Response<TResult2>>> function2)
        {
            return async value =>
            {
                var asyncFunc1 = MakeAsync(function1);
                var bindedFunc2 = Bind(function2);
                return await bindedFunc2(await asyncFunc1(value));
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// T -> Task<TResult1>; TResult1 -> TResult2
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<T, Task<Response<TResult2>>> ThenAsync<T, TResult1, TResult2>(this Func<T, Task<Response<TResult1>>> function1, Func<TResult1, Response<TResult2>> function2)
        {
            return async value =>
            {
                var asyncFunc2 = MakeAsync(function2);
                var bindedFunc2 = Bind(asyncFunc2);
                return await bindedFunc2(await function1(value));
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// T -> Task<TResult1>; TResult1 -> Task<TResult2>
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<T, Task<Response<TResult2>>> ThenAsync<T, TResult1, TResult2>(this Func<T, Task<Response<TResult1>>> function1, Func<TResult1, Task<Response<TResult2>>> function2)
        {
            return async value =>
            {
                var bindedFunc2 = Bind(function2);
                return await bindedFunc2(await function1(value));
            };
        }

        #endregion

        /// <summary>
        /// Transforms a synchronous Func into an asynchronous one
        /// </summary>
        /// <typeparam name="T">Output data type of the func</typeparam>
        /// <param name="func">Function to transform</param>
        /// <returns>The asynchronous function</returns>
        public static Func<Task<Response<T>>> MakeAsync<T>(this Func<Response<T>> func)
        {
            return () => Task.FromResult(func());
        }

        /// <summary>
        /// Transforms a synchronous Func into an asynchronous one
        /// </summary>
        /// <typeparam name="T">Input data type of the func</typeparam>
        /// <typeparam name="T">Output data type of the func</typeparam>
        /// <param name="func">Function to transform</param>
        /// <returns>The asynchronous function</returns>
        public static Func<T, Task<Response<TResult1>>> MakeAsync<T, TResult1>(this Func<T, Response<TResult1>> func)
        {
            return value => Task.FromResult(func(value));
        }
    }
}
