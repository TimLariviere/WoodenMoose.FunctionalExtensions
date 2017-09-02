using System;

namespace WoodenMoose.FunctionalExtensions.Rop
{
    /// <summary>
    /// Synchronous extensions for function composition
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        /// Wraps the function into a new one to allow for railway oriented programming
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="func">Function to wrap</param>
        /// <returns>The binded function</returns>
        public static Func<Response<T>, Response<TResult1>> Bind<T, TResult1>(this Func<Response<T>, Response<TResult1>> func)
        {
            return value =>
            {
                if (value.Type == ResponseType.Success)
                {
                    return func(value);
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
        public static Func<Response<TResult1>> Then<T, TResult1>(this Func<Response<T>> function1, Func<Response<T>, Response<TResult1>> function2)
        {
            var bindedFunc2 = Bind(function2);
            return () => bindedFunc2(function1());
        }

        /// <summary>
        /// Chains another function after the current one
        /// </summary>
        /// <typeparam name="T">Input data type of the incoming response</typeparam>
        /// <typeparam name="TResult1">Output data type of the outcoming response</typeparam>
        /// <param name="function1">Current function</param>
        /// <param name="function2">Function to chain after</param>
        /// <returns>A new function representing the two chained functions</returns>
        public static Func<Response<T>, Response<TResult2>> Then<T, TResult1, TResult2>(this Func<Response<T>, Response<TResult1>> function1, Func<Response<TResult1>, Response<TResult2>> function2)
        {
            var bindedFunc2 = Bind(function2);
            return value => bindedFunc2(function1(value));
        }
    }
}
