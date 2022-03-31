using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using Xunit;

namespace lmc.poc.tests
{
    public class CalculatorTests
    {
        private readonly Calculator calculator;

        public CalculatorTests()
        {
            calculator = new Calculator();
        }

        public static IEnumerable<object[]> GetExcelData()
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvCalculationDataMapping csvMapper = new CsvCalculationDataMapping();
            CsvParser<CsvCalculationData> csvParser = new CsvParser<CsvCalculationData>(csvParserOptions, csvMapper);
            var result = csvParser
                .ReadFromFile(@"TestData.csv", Encoding.ASCII)
                .ToList();

            return result.Select(x => new object[]
            {
                x.Result.N1,
                x.Result.N2,
                x.Result.N3,
                x.Result.Total,

            });
        }

        [Theory]
        [MemberData(nameof(GetExcelData))]
        public void ExcelXlsTests(
            int n1, 
            int n2, 
            int n3, 
            int total)
        {
            // Act
            var result = calculator.Sum(n1, n2, n3);

            // Assert
            result.Should().Be(total);
        }

        public class CsvCalculationData
        {
            public int N1 { get; set; }
            public int N2 { get; set; }
            public int N3 { get; set; }
            public int Total { get; set; }
        }

        public class CsvCalculationDataMapping : CsvMapping<CsvCalculationData>
        {
            public CsvCalculationDataMapping():base()
            {
                MapProperty(0, x => x.N1);
                MapProperty(1, x => x.N2);
                MapProperty(2, x => x.N3);
                MapProperty(3, x => x.Total);
            }
        }
    }
}
