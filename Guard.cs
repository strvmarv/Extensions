// <copyright file="Guard.cs" company="LiteGuard contributors">
//  Copyright (c) LiteGuard contributors. All rights reserved.
// </copyright>

// <license>
//  The MIT License (MIT)
//  Copyright (c) LiteGuard contributors. (liteguard.ch@gmail.com)
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </license>

// <source>
//  ADAPTED FROM
//  https://github.com/liteguard/liteguard
// </source>

// <modifications>
//  - Slight renaming of methods for better consistency/readability
// </modifications>

namespace Extensions
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// Provides guard clauses.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Guards against a null argument.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="argument">The argument.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="argument" /> is <c>null</c>.</exception>
        /// <remarks><typeparamref name="TArgument"/> is restricted to reference types to avoid boxing of value type objects.</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Distributed as a source code package.")]
        [DebuggerStepThrough]
        public static void AgainstArgumentNull<TArgument>(string parameterName, [ValidatedNotNull]TArgument argument) where TArgument : class
        {
            if (typeof(TArgument) == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(argument as string))
                {
                    throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, "{0} is null.", parameterName));
                }
            }
            else
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, "{0} is null.", parameterName));
                }
            }
        }

        /// <summary>
        /// Guards against a null argument if <typeparamref name="TArgument" /> can be <c>null</c>.
        /// </summary>
        /// <typeparam name="TArgument">The type of the argument.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="argument">The argument.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="argument" /> is <c>null</c>.</exception>
        /// <remarks>
        /// Performs a type check to avoid boxing of value type objects.
        /// </remarks>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Distributed as a source code package.")]
        [DebuggerStepThrough]
        public static void AgainstArgumentNullIfNullable<TArgument>(string parameterName, [ValidatedNotNull]TArgument argument)
        {
            if (typeof(TArgument) == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(argument as string))
                {
                    throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, "{0} is null.", parameterName));
                }
            }
            else
            {
                if (typeof(TArgument).IsNullableType() && argument == null)
                {
                    throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, "{0} is null.", parameterName));
                }
            }
        }

        /// <summary>
        /// Guards against a null argument property value.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="argumentProperty">The argument property.</param>
        /// <exception cref="System.ArgumentException"><paramref name="argumentProperty" /> is <c>null</c>.</exception>
        /// <remarks><typeparamref name="TProperty"/> is restricted to reference types to avoid boxing of value type objects.</remarks>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Distributed as a source code package.")]
        [DebuggerStepThrough]
        public static void AgainstArgumentPropertyNull<TProperty>(string parameterName, string propertyName, [ValidatedNotNull]TProperty argumentProperty)
            where TProperty : class
        {
            if (typeof(TProperty) == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(argumentProperty as string))
                {
                    throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, "{0} is null.", propertyName));
                }
            }
            else
            {
                if (argumentProperty == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0}.{1} is null.", parameterName, propertyName), propertyName);
                }
            }
        }

        /// <summary>
        /// Guards against a null argument property value if <typeparamref name="TProperty"/> can be <c>null</c>.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="argumentProperty">The argument property.</param>
        /// <exception cref="System.ArgumentException"><paramref name="argumentProperty" /> is <c>null</c>.</exception>
        /// <remarks>
        /// Performs a type check to avoid boxing of value type objects.
        /// </remarks>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Distributed as a source code package.")]
        [DebuggerStepThrough]
        public static void AgainstArgumentPropertyNullIfNullable<TProperty>(string parameterName, string propertyName, [ValidatedNotNull]TProperty argumentProperty)
        {
            if (typeof(TProperty) == typeof(string))
            {
                if (string.IsNullOrWhiteSpace(argumentProperty as string))
                {
                    throw new ArgumentNullException(parameterName, string.Format(CultureInfo.InvariantCulture, "{0} is null.", propertyName));
                }
            }
            else
            {
                if (typeof(TProperty).IsNullableType() && argumentProperty == null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0}.{1} is null.", parameterName, propertyName), propertyName);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified type is a nullable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is a nullable type; otherwise, <c>false</oc>.
        /// </returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Distributed as a source code package.")]
        private static bool IsNullableType(this Type type)
        {
            return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// When applied to a parameter, this attribute provides an indication to code analysis that the argument has been null checked.
        /// </summary>
        private sealed class ValidatedNotNullAttribute : Attribute
        {
        }
    }
}
