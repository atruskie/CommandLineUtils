// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace McMaster.Extensions.CommandLineUtils.Tests
{
    using Xunit;

    public class ValueParserProviderCustomTests
    {

        private class MyDateTimeOffsetParser : IValueParser
        {
            public object Parse(string argName, string value)
            {
                if (!DateTimeOffset.TryParse(value, out var result))
                {
                    throw new CommandParsingException(null, $"Invalid value specified for {argName}. '{value} is not a valid date time (with offset)");
                }

                return result;
            }
        }

        private class CustomParserProgram
        {
            [Argument(0)]
            public DateTimeOffset DateTimeOffset { get; }
        }

        [Fact]
        public void CustomParsers()
        {
            ValueParserProvider.Default.AddParser(typeof(DateTimeOffset), new MyDateTimeOffsetParser());

            var expected = new DateTimeOffset(2018, 02, 16, 21, 30, 33, 45, TimeSpan.FromHours(10));
            var parsed = CommandLineParser.ParseArgs<CustomParserProgram>(expected.ToString("O"));

            Assert.Equal(expected, parsed.DateTimeOffset);
        }

        [Fact]
        public void ThrowsIfNoType()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => ValueParserProvider.Default.AddParser(null, new MyDateTimeOffsetParser()));

            Assert.Contains("type", ex.Message);
        }

        [Fact]
        public void ThrowsIfNoParser()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => ValueParserProvider.Default.AddParser(typeof(DateTimeOffset), null));

            Assert.Contains("parser", ex.Message);
        }
    }
}
