// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils.ValueParsers;

namespace McMaster.Extensions.CommandLineUtils
{
    /// <summary>
    /// A store of parsers that are used to convert argument values from strings to types.
    /// </summary>
    public class ValueParserProvider
    {
        private Dictionary<Type, IValueParser> _parsers = new Dictionary<Type, IValueParser>
        {
            { typeof(string), StringValueParser.Singleton },
            { typeof(bool), BooleanValueParser.Singleton },
            { typeof(byte), ByteValueParser.Singleton },
            { typeof(short), Int16ValueParser.Singleton },
            { typeof(int), Int32ValueParser.Singleton },
            { typeof(long), Int64ValueParser.Singleton },
            { typeof(ushort), UInt16ValueParser.Singleton },
            { typeof(uint), UInt32ValueParser.Singleton },
            { typeof(ulong), UInt64ValueParser.Singleton },
            { typeof(float), FloatValueParser.Singleton },
            { typeof(double), DoubleValueParser.Singleton },
        };

        private ValueParserProvider()
        { }

        /// <summary>
        /// A singleton of the parser provider.
        /// </summary>
        public static ValueParserProvider Default { get; } = new ValueParserProvider();

        internal IValueParser GetParser(Type type)
        {
            if (_parsers.TryGetValue(type, out var parser))
            {
                return parser;
            }

            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsEnum)
            {
                return new EnumParser(type);
            }

            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var wrappedType = type.GetTypeInfo().GetGenericArguments().First();
                if (_parsers.TryGetValue(wrappedType, out parser))
                {
                    return new NullableValueParser(parser);
                }
            }

            return parser;
        }

        /// <summary>
        /// Add a new parser to the provider.
        /// </summary>
        /// <param name="type">The type for which this parser will be used.</param>
        /// <param name="parser">An instance of the parser that is used to convert an argument from a string.</param>
        public void AddParser(Type type, IValueParser parser)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (parser == null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            _parsers.Add(type, parser);
        }
    }
}
