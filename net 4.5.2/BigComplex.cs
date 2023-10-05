namespace System.Numerics
{
    public struct BigComplex
    {
        private BigDecimal _value;

        private static readonly byte imaginary = 0b_1000_0000;

        public BigComplex(BigDecimal value, bool isImaginary = false)
        {
            _value = value;
            if (isImaginary)
                _value._flags |= imaginary;
        }
    }
}