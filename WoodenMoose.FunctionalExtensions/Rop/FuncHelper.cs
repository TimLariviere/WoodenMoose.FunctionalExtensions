using System;
using System.Threading.Tasks;

namespace WoodenMoose.FunctionalExtensions.Rop
{
    public static class FuncHelper
    {
        /// <summary>
        /// Converts a function to its Func representation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The method to convert</param>
        /// <returns>Func representation of the function</returns>
        public static Func<Response<T>> AsFunc<T>(Func<Response<T>> func) => func;

        /// <summary>
        /// Converts an asynchronous function to its Func representation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The method to convert</param>
        /// <returns>Func representation of the function</returns>
        public static Func<Task<Response<T>>> AsFunc<T>(Func<Task<Response<T>>> func) => func;

        /// <summary>
        /// Converts a function to its Func representation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The method to convert</param>
        /// <returns>Func representation of the function</returns>
        public static Func<T, Response<TResult>> AsFunc<T, TResult>(Func<T, Response<TResult>> func) => func;

        /// <summary>
        /// Converts an asynchronous function to its Func representation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The method to convert</param>
        /// <returns>Func representation of the function</returns>
        public static Func<T, Task<Response<TResult>>> AsFunc<T, TResult>(Func<T, Task<Response<TResult>>> func) => func;
    }
}
