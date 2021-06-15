using ILVisualizer.Application.Common.Exceptions.Parser;
using ILVisualizer.Application.Common.Services;
using ILVisualizer.Domain.Enums;
using ILVisualizer.Domain.Models;
using System.Linq;
using Xunit;

namespace ILVisualizer.UnitTests.Parsing
{
    public class ILParserServiceTests
    {
        //[Theory]
        //[InlineData("ldc_i4_0")]
        [Fact]
        public void Parse_OneInstruction_NoArgument()
        {
            var parser = new ILParserService();
            var lst = parser.Parse("ldc.i4.1");

            var expected = new ILInstruction[] 
            { 
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_1
                }
            };

            Assert.True(expected.SequenceEqual(lst));
        }

        [Fact]
        public void Parse_OneInstruction_Int8Argument()
        {
            var parser = new ILParserService();
            var lst = parser.Parse("ldc.i4.s 4");

            var expected = new ILInstruction[]
            {
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_S,
                    IntArg = 4
                }
            };

            Assert.True(expected.SequenceEqual(lst));
        }

        [Fact]
        public void Parse_OneInstruction_Int8Argument_InvalidRange()
        {
            var parser = new ILParserService();
            Assert.Throws<ParseFailedException>(() => parser.Parse("ldc.i4.s 478"));
        }

        [Fact]
        public void Parse_OneInstruction_Int8Argument_InvalidContents()
        {
            var parser = new ILParserService();
            Assert.Throws<ParseFailedException>(() => parser.Parse("ldc.i4.s 478a"));
        }

        [Fact]
        public void Parse_OneInstruction_Int32Argument()
        {
            var parser = new ILParserService();
            var lst = parser.Parse("ldc.i4 572");

            var expected = new ILInstruction[]
            {
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4,
                    IntArg = 572
                }
            };

            Assert.True(expected.SequenceEqual(lst));
        }


        [Fact]
        public void Parse_OneInstruction_Int64Argument()
        {
            var parser = new ILParserService();
            var lst = parser.Parse("ldc.i8 80000000000000");

            var expected = new ILInstruction[]
            {
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I8,
                    LongArg = 80000000000000
                }
            };

            Assert.True(expected.SequenceEqual(lst));
        }

        [Fact]
        public void Parse_OneInstruction_Int32Argument_InvalidContents()
        {
            var parser = new ILParserService();
            Assert.Throws<ParseFailedException>(() => parser.Parse("ldc.i4 478a"));
        }

        [Fact]
        public void Parse_MultipleInstructions_Variety()
        {
            var parser = new ILParserService();
            var lst = parser.Parse(@"ldc.i4.2
ldc.i4.s 7
ldc.i4 567
ldc.i4.m1");

            var expected = new ILInstruction[]
            {
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_2
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_S,
                    IntArg = 7
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4,
                    IntArg = 567
                },
                new ILInstruction()
                {
                    Type = ILInstructionType.Ldc_I4_M1
                }
            };

            Assert.True(expected.SequenceEqual(lst));
        }

    }
}
