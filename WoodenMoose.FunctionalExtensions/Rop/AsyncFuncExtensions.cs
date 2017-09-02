using System;
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
        public static Func<Response<T>, Task<Response<TResult1>>> Bind<T, TResult1>(this Func<Response<T>, Task<Response<TResult1>>> func)
        {
            return async value =>
            {
                if (value.Type == ResponseType.Success)
                {
                    return await func(value);
                }
                else
                {
                    return Response<TResult1>.Failure(value.FailureReason);
                }
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Task<Response<TResult1>>> ThenAsync<T, TResult1>(this Func<Response<T>> function1, Func<Response<T>, Task<Response<TResult1>>> function2)
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
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the intermediate outcoming response</typeparam>
        /// <typeparam name="TResult2">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Response<T>, Task<Response<TResult2>>> ThenAsync<T, TResult1, TResult2>(this Func<Response<T>, Response<TResult1>> function1, Func<Response<TResult1>, Task<Response<TResult2>>> function2)
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
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Task<Response<TResult1>>> ThenAsync<T, TResult1>(this Func<Task<Response<T>>> function1, Func<Response<T>, Task<Response<TResult1>>> function2)
        {
            return async () =>
            {
                var bindedFunc2 = Bind(function2);
                return await bindedFunc2(await function1());
            };
        }

        /// <summary>
        /// Chains another function after the current one
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Task<Response<TResult1>>> ThenAsync<T, TResult1>(this Func<Task<Response<T>>> function1, Func<Response<T>, Response<TResult1>> function2)
        {
            return async () =>
            {
                var asyncedFunc2 = MakeAsync(function2);
                var bindedFunc2 = Bind(asyncedFunc2);
                return await bindedFunc2(await function1());
            };
        }

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
        public static Func<Response<T>, Task<Response<TResult1>>> MakeAsync<T, TResult1>(this Func<Response<T>, Response<TResult1>> func)
        {
            return value => Task.FromResult(func(value));
        }
    }
}
