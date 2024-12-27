namespace Day7
{
    internal class Equation(long expectedValue, List<long> numbers)
    {
        public long ExpectedResult { get; set; } = expectedValue;

        public List<long> Numbers { get; set; } = numbers;
    }
}
