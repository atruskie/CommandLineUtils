// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace McMaster.Extensions.CommandLineUtils
{
    /// <summary>
    /// Defines the interface required to parse different types.
    /// </summary>
    public interface IValueParser
    {
        /// <summary>
        /// Parse an argument string into a type
        /// </summary>
        /// <param name="argName">The name of the argument to parse.</param>
        /// <param name="value">The name of the value to parse.</param>
        /// <returns>A value representing the argument that was parsed.</returns>
        object Parse(string argName, string value);
    }
}
