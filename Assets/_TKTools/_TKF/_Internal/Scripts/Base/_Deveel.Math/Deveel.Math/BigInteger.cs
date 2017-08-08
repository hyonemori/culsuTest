// 
//  Copyright 2009-2014  Deveel
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

//namespace Deveel.Math
namespace Deveel.Math
{
    /// <summary>
    /// This class represents immutable integer numbers of arbitrary length.
    /// </summary>
    /// <remarks>
    /// Large numbers are typically used in security applications and therefore 
    /// BigIntegers offer dedicated functionality like the generation of large 
    /// prime numbers or the computation of modular inverse.
    /// <para>
    /// Since the class was modeled to offer all the functionality as the 
    /// {@link Integer} class does, it provides even methods that operate bitwise 
    /// on a two's complement representation of large integers. Note however that 
    /// the implementations favors an internal representation where magnitude and 
    /// sign are treated separately. Hence such operations are inefficient and 
    /// should be discouraged. In simple words: Do NOT implement any bit fields 
    /// based on BigInteger.
    /// </para>
    /// </remarks>
    #if !PORTABLE
	[Serializable]
    #endif
	public sealed class BigInteger : IComparable<BigInteger>, IEquatable<BigInteger>
#if !PORTABLE
		, ISerializable, IConvertible
#endif
	{

        /* Fields used for the internal representation. */

        /**
		 * The magnitude of this big integer. This array holds unsigned little
		 * endian digits. For example:
		 *   {@code 13} is represented as [ 13 ]
		 *   {@code -13} is represented as [ 13 ]
		 *   {@code 2^32 + 13} is represented as [ 13, 1 ]
		 *   {@code 2^64 + 13} is represented as [ 13, 0, 1 ]
		 *   {@code 2^31} is represented as [ Integer.MIN_VALUE ]
		 * The magnitude array may be longer than strictly necessary, which results
		 * in additional trailing zeros.
		 */
        #if !PORTABLE
		[NonSerialized] 
        #endif
		private int[] digits;

        /** The length of this in measured in ints. Can be less than digits.length(). */
        #if !PORTABLE
		[NonSerialized]
        #endif
		internal int numberLength;

        /** The sign of this. */
        #if !PORTABLE
		[NonSerialized] 
        #endif
		private int sign;


        /// <summary>
        /// The constant value <c>0</c> as <see cref="BigInteger"/>
        /// </summary>
        public static readonly BigInteger Zero = new BigInteger(0, 0);

        /// <summary>
        /// The constant value <c>1</c> as <see cref="BigInteger"/>
        /// </summary>
        public static readonly BigInteger One = new BigInteger(1, 1);

        /// <summary>
        /// The constant value <c>10</c> as <see cref="BigInteger"/>
        /// </summary>
        public static readonly BigInteger Ten = new BigInteger(1, 10);

        /// <summary>
        /// The constant value <c>-1</c> as <see cref="BigInteger"/>
        /// </summary>
        internal static readonly BigInteger MinusOne = new BigInteger(-1, 1);

        /** The {@code BigInteger} constant 0 used for comparison. */
        internal static readonly int EQUALS = 0;

        /** The {@code BigInteger} constant 1 used for comparison. */
        internal static readonly int GREATER = 1;

        /** The {@code BigInteger} constant -1 used for comparison. */
        internal static readonly int LESS = -1;

        /** All the {@code BigInteger} numbers in the range [0,10] are cached. */
        static readonly BigInteger[] SmallValues = { Zero, One, new BigInteger(1, 2),
            new BigInteger(1, 3), new BigInteger(1, 4), new BigInteger(1, 5),
            new BigInteger(1, 6), new BigInteger(1, 7), new BigInteger(1, 8),
            new BigInteger(1, 9), Ten
        };

        static readonly BigInteger[] TwoPows;

        static BigInteger()
        {
            TwoPows = new BigInteger[32];
            for (int i = 0; i < TwoPows.Length; i++)
            {
                TwoPows[i] = BigInteger.ValueOf(1L << i);
            }
        }

        private BigInteger()
        {
        }

#if !PORTABLE
        [NonSerialized]
        #endif
		private int firstNonzeroDigit = -2;

        /** Cache for the hash code. */
        #if !PORTABLE
		[NonSerialized]
        #endif
		private int hashCode = 0;

#if !PORTABLE
#region Serializable

        private BigInteger(SerializationInfo info, StreamingContext context)
        {
            sign = info.GetInt32("sign");
            byte[] magn = (byte[])info.GetValue("magnitude", typeof(byte[]));
            PutBytesPositiveToIntegers(magn);
            CutOffLeadingZeroes();
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("sign", sign);
            byte[] magn = Abs().ToByteArray();
            info.AddValue("magnitude", magn, typeof(byte[]));
        }

#endregion

#endif

#region .ctor

        /// <summary>
        /// Constructs a random non-negative big integer instance in the range [0, 2^(numBits)-1]
        /// </summary>
        /// <param name="numBits">The maximum length of the new <see cref="BigInteger"/> in bits.</param>
        /// <param name="rnd">An optional random number generator to be used.</param>
        /// <exception cref="ArgumentException">
        /// If the given <paramref name="numBits"/> value is less than 0.
        /// </exception>
        public BigInteger(int numBits, Random rnd)
        {
            if (numBits < 0)
            {
                // math.1B=numBits must be non-negative
                throw new ArgumentException(Messages.math1B); //$NON-NLS-1$
            }
            if (numBits == 0)
            {
                sign = 0;
                numberLength = 1;
                digits = new int[] { 0 };
            }
            else
            {
                sign = 1;
                numberLength = (numBits + 31) >> 5;
                digits = new int[numberLength];
                for (int i = 0; i < numberLength; i++)
                {
                    digits[i] = rnd.Next();
                }
                // Using only the necessary bits
                digits[numberLength - 1] = Utils.URShift(digits[numberLength - 1], (-numBits) & 31);
                CutOffLeadingZeroes();
            }
        }

        /**
		 * Constructs a random {@code BigInteger} instance in the range [0,
		 * 2^(bitLength)-1] which is probably prime. The probability that the
		 * returned {@code BigInteger} is prime is beyond (1-1/2^certainty).
		 *
		 * @param bitLength
		 *            length of the new {@code BigInteger} in bits.
		 * @param certainty
		 *            tolerated primality uncertainty.
		 * @param rnd
		 *            is an optional random generator to be used.
		 * @throws ArithmeticException
		 *             if {@code bitLength} < 2.
		 */
        public BigInteger(int bitLength, int certainty, Random rnd)
        {
            if (bitLength < 2)
            {
                // math.1C=bitLength < 2
                throw new ArithmeticException(Messages.math1C); //$NON-NLS-1$
            }
            BigInteger me = Primality.ConsBigInteger(bitLength, certainty, rnd);
            sign = me.sign;
            numberLength = me.numberLength;
            digits = me.digits;
        }

        /// <summary>
        /// Constructs a new {@code BigInteger} instance with the given sign and the given magnitude. 
        /// </summary>
        /// <param name="signum">The sign as an integer (-1 for negative, 0 for zero, 1 for positive).</param>
        /// <param name="magnitude">The byte array that describes the magnitude, where the most significant
        /// byte is the first.</param>
        /// <exception cref="ArgumentNullException">
        /// If the provided <paramref name="magnitude"/> provided is <c>null</c>.
        /// </exception>
        /// <exception cref="FormatException">
        /// If the provided <paramref name="signum"/> is different from -1, 0 or 1 or
        /// if the sign is 0 and the magnitude contains non-zero entries.
        /// </exception>
        public BigInteger(int signum, byte[] magnitude)
        {
            if (magnitude == null)
                throw new ArgumentNullException("magnitude");

            if ((signum < -1) || (signum > 1))
				// math.13=Invalid signum value
				throw new FormatException(Messages.math13); //$NON-NLS-1$

            if (signum == 0)
            {
                foreach (byte element in magnitude)
                {

                    if (element != 0)
                    {
                        // math.14=signum-magnitude mismatch
                        throw new FormatException(Messages.math14); //$NON-NLS-1$
                    }
                }
            }

            if (magnitude.Length == 0)
            {
                sign = 0;
                numberLength = 1;
                digits = new int[] { 0 };
            }
            else
            {
                sign = signum;
                PutBytesPositiveToIntegers(magnitude);
                CutOffLeadingZeroes();
            }
        }

        /**
		 * Constructs a new {@code BigInteger} from the given two's complement
		 * representation. The most significant byte is the entry at index 0. The
		 * most significant bit of this entry determines the sign of the new {@code
		 * BigInteger} instance. The given array must not be empty.
		 *
		 * @param val
		 *            two's complement representation of the new {@code BigInteger}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 * @throws NumberFormatException
		 *             if the length of {@code val} is zero.
		 */
        public BigInteger(byte[] val)
        {
            if (val.Length == 0)
            {
                // math.12=Zero length BigInteger
                throw new FormatException(Messages.math12); //$NON-NLS-1$
            }
            if (val[0] > sbyte.MaxValue)
            {
                sign = -1;
                PutBytesNegativeToIntegers(val);
            }
            else
            {
                sign = 1;
                PutBytesPositiveToIntegers(val);
            }
            CutOffLeadingZeroes();
        }

        /**
		 * Constructs a number which array is of size 1.
		 * 
		 * @param sign
		 *            the sign of the number
		 * @param value
		 *            the only one digit of array
		 */
        internal BigInteger(int sign, int value)
        {
            this.sign = sign;
            numberLength = 1;
            digits = new int[] { value };
        }

        /**
		 * Constructs a number without to create new space. This construct should be
		 * used only if the three fields of representation are known.
		 * 
		 * @param sign
		 *            the sign of the number
		 * @param numberLength
		 *            the length of the internal array
		 * @param digits
		 *            a reference of some array created before
		 */
        internal BigInteger(int sign, int numberLength, int[] digits)
        {
            this.sign = sign;
            this.numberLength = numberLength;
            this.digits = digits;
        }

        /**
		 * Creates a new {@code BigInteger} whose value is equal to the specified
		 * {@code long}.
		 * 
		 * @param sign
		 *            the sign of the number
		 * @param val
		 *            the value of the new {@code BigInteger}.
		 */
        internal BigInteger(int sign, long val)
        {
            // PRE: (val >= 0) && (sign >= -1) && (sign <= 1)
            this.sign = sign;
            if (((ulong)val & 0xFFFFFFFF00000000L) == 0)
            {
                // It fits in one 'int'
                numberLength = 1;
                digits = new int[] { (int)val };
            }
            else
            {
                numberLength = 2;
                digits = new int[] { (int)val, (int)(val >> 32) };
            }
        }

        /**
		 * Creates a new {@code BigInteger} with the given sign and magnitude. This
		 * constructor does not create a copy, so any changes to the reference will
		 * affect the new number.
		 * 
		 * @param signum
		 *            The sign of the number represented by {@code digits}
		 * @param digits
		 *            The magnitude of the number
		 */
        internal BigInteger(int signum, int[] digits)
        {
            if (digits.Length == 0)
            {
                sign = 0;
                numberLength = 1;
                this.digits = new int[] { 0 };
            }
            else
            {
                sign = signum;
                numberLength = digits.Length;
                this.digits = digits;
                CutOffLeadingZeroes();
            }
        }

#endregion

        public int Sign
        {
            get { return sign; }
            internal set { sign = value; }
        }

        public int BitLength
        {
            get { return BitLevel.BitLength(this); }
        }

        public int LowestSetBit
        {
            get
            {
                if (sign == 0)
                {
                    return -1;
                }
                // (sign != 0) implies that exists some non zero digit
                int i = FirstNonzeroDigit;
                return ((i << 5) + Utils.NumberOfTrailingZeros(digits[i]));
            }
        }

        /**
		 * Use {@code bitLength(0)} if you want to know the length of the binary
		 * value in bits.
		 * <p>
		 * Returns the number of bits in the binary representation of {@code this}
		 * which differ from the sign bit. If {@code this} is positive the result is
		 * equivalent to the number of bits set in the binary representation of
		 * {@code this}. If {@code this} is negative the result is equivalent to the
		 * number of bits set in the binary representation of {@code -this-1}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @return number of bits in the binary representation of {@code this} which
		 *         differ from the sign bit
		 */
        public int BitCount
        {
            get { return BitLevel.BitCount(this); }
        }

        internal int FirstNonzeroDigit
        {
            get
            {
                if (firstNonzeroDigit == -2)
                {
                    int i;
                    if (this.sign == 0)
                    {
                        i = -1;
                    }
                    else
                    {
                        for (i = 0; digits[i] == 0; i++)
                        {
                            // Empty
                        }
                    }
                    firstNonzeroDigit = i;
                }
                return firstNonzeroDigit;
            }
        }

        internal int[] Digits
        {
            get { return digits; }
        }


#region Operations

        /// <summary>
        /// Computes the absolute value of this <see cref="BigInteger"/>
        /// </summary>
        /// <returns>
        /// Returns an instance of <see cref="BigInteger"/> that represents the
        /// absolute value of this instance.
        /// </returns>
        public BigInteger Abs()
        {
            return ((sign < 0) ? new BigInteger(1, numberLength, digits) : this);
        }

        /// <summary>
        /// Computes the negation of this <see cref="BigInteger"/>.
        /// </summary>
        /// <returns>
        /// Returns an instance of <see cref="BigInteger"/> that is the negated value
        /// of this instance.
        /// </returns>
        public BigInteger Negate()
        {
            return ((sign == 0) ? this : new BigInteger(-sign, numberLength, digits));
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this + val}.
		 *
		 * @param val
		 *            value to be added to {@code this}.
		 * @return {@code this + val}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */
        public BigInteger Add(BigInteger val)
        {
            return Elementary.add(this, val);
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this - val}.
		 *
		 * @param val
		 *            value to be subtracted from {@code this}.
		 * @return {@code this - val}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */
        public BigInteger Subtract(BigInteger val)
        {
            return Elementary.subtract(this, val);
        }

        /**
		 * Returns the sign of this {@code BigInteger}.
		 *
		 * @return {@code -1} if {@code this < 0},
		 *         {@code 0} if {@code this == 0},
		 *         {@code 1} if {@code this > 0}.
		 */

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this >> n}. For
		 * negative arguments, the result is also negative. The shift distance may
		 * be negative which means that {@code this} is shifted left.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method on negative values is
		 * not recommended as the current implementation is not efficient.
		 *
		 * @param n
		 *            shift distance
		 * @return {@code this >> n} if {@code n >= 0}; {@code this << (-n)}
		 *         otherwise
		 */
        public BigInteger ShiftRight(int n)
        {
            if ((n == 0) || (sign == 0))
            {
                return this;
            }
            return ((n > 0) ? BitLevel.ShiftRight(this, n) : BitLevel.ShiftLeft(
                this, -n));
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this << n}. The
		 * result is equivalent to {@code this * 2^n} if n >= 0. The shift distance
		 * may be negative which means that {@code this} is shifted right. The
		 * result then corresponds to {@code floor(this / 2^(-n))}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method on negative values is
		 * not recommended as the current implementation is not efficient.
		 *
		 * @param n
		 *            shift distance.
		 * @return {@code this << n} if {@code n >= 0}; {@code this >> (-n)}.
		 *         otherwise
		 */
        public BigInteger ShiftLeft(int n)
        {
            if ((n == 0) || (sign == 0))
            {
                return this;
            }
            return ((n > 0) ? BitLevel.ShiftLeft(this, n) : BitLevel.ShiftRight(this, -n));
        }

        internal BigInteger ShiftLeftOneBit()
        {
            return (sign == 0) ? this : BitLevel.ShiftLeftOneBit(this);
        }

        /**
		 * Returns the length of the value's two's complement representation without
		 * leading zeros for positive numbers / without leading ones for negative
		 * values.
		 * <p>
		 * The two's complement representation of {@code this} will be at least
		 * {@code bitLength() + 1} bits long.
		 * <p>
		 * The value will fit into an {@code int} if {@code bitLength() < 32} or
		 * into a {@code long} if {@code bitLength() < 64}.
		 *
		 * @return the length of the minimal two's complement representation for
		 *         {@code this} without the sign bit.
		 */

        /**
		 * Tests whether the bit at position n in {@code this} is set. The result is
		 * equivalent to {@code this & (2^n) != 0}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @param n
		 *            position where the bit in {@code this} has to be inspected.
		 * @return {@code this & (2^n) != 0}.
		 * @throws ArithmeticException
		 *             if {@code n < 0}.
		 */
        public bool TestBit(int n)
        {
            if (n == 0)
            {
                return ((digits[0] & 1) != 0);
            }
            if (n < 0)
            {
                // math.15=Negative bit address
                throw new ArithmeticException(Messages.math15); //$NON-NLS-1$
            }
            int intCount = n >> 5;
            if (intCount >= numberLength)
            {
                return (sign < 0);
            }
            int digit = digits[intCount];
            n = (1 << (n & 31)); // int with 1 set to the needed position
            if (sign < 0)
            {
                int firstNonZeroDigit = FirstNonzeroDigit;
                if (intCount < firstNonZeroDigit)
                {
                    return false;
                }
                else if (firstNonZeroDigit == intCount)
                {
                    digit = -digit;
                }
                else
                {
                    digit = ~digit;
                }
            }
            return ((digit & n) != 0);
        }

        /**
		 * Returns a new {@code BigInteger} which has the same binary representation
		 * as {@code this} but with the bit at position n set. The result is
		 * equivalent to {@code this | 2^n}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @param n
		 *            position where the bit in {@code this} has to be set.
		 * @return {@code this | 2^n}.
		 * @throws ArithmeticException
		 *             if {@code n < 0}.
		 */
        public BigInteger SetBit(int n)
        {
            if (!TestBit(n))
            {
                return BitLevel.FlipBit(this, n);
            }
            return this;
        }

        /**
		 * Returns a new {@code BigInteger} which has the same binary representation
		 * as {@code this} but with the bit at position n cleared. The result is
		 * equivalent to {@code this & ~(2^n)}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @param n
		 *            position where the bit in {@code this} has to be cleared.
		 * @return {@code this & ~(2^n)}.
		 * @throws ArithmeticException
		 *             if {@code n < 0}.
		 */
        public BigInteger ClearBit(int n)
        {
            if (TestBit(n))
            {
                return BitLevel.FlipBit(this, n);
            }
            return this;
        }

        /**
		 * Returns a new {@code BigInteger} which has the same binary representation
		 * as {@code this} but with the bit at position n flipped. The result is
		 * equivalent to {@code this ^ 2^n}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @param n
		 *            position where the bit in {@code this} has to be flipped.
		 * @return {@code this ^ 2^n}.
		 * @throws ArithmeticException
		 *             if {@code n < 0}.
		 */
        public BigInteger FlipBit(int n)
        {
            if (n < 0)
            {
                // math.15=Negative bit address
                throw new ArithmeticException(Messages.math15); //$NON-NLS-1$
            }
            return BitLevel.FlipBit(this, n);
        }

        /**
		 * Returns the position of the lowest set bit in the two's complement
		 * representation of this {@code BigInteger}. If all bits are zero (this=0)
		 * then -1 is returned as result.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @return position of lowest bit if {@code this != 0}, {@code -1} otherwise
		 */

        /**
		 * Returns a new {@code BigInteger} whose value is {@code ~this}. The result
		 * of this operation is {@code -this-1}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @return {@code ~this}.
		 */
        public BigInteger Not()
        {
            return Logical.Not(this);
        }

        /// <summary>
        /// Computes the bit per bit operator between this number
        /// and the given one.
        /// </summary>
        /// <param name="val">The value to be and'ed with the current.</param>
        /// <remarks>
        /// <b>Implementation Note:</b> Usage of this method is not recommended as 
        /// the current implementation is not efficient.
        /// </remarks>
        /// <returns>
        /// Returns a new <see cref="BigInteger"/> whose value is <c>this & <paramref name="val"/></c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// If <paramref name="val"/> is <c>null</c>.
        /// </exception>
        public BigInteger And(BigInteger val)
        {
            return Logical.And(this, val);
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this | val}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @param val
		 *            value to be or'ed with {@code this}.
		 * @return {@code this | val}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */
        public BigInteger Or(BigInteger val)
        {
            return Logical.Or(this, val);
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this ^ val}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @param val
		 *            value to be xor'ed with {@code this}
		 * @return {@code this ^ val}
		 * @throws NullPointerException
		 *             if {@code val == null}
		 */
        public BigInteger XOr(BigInteger val)
        {
            return Logical.Xor(this, val);
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this & ~val}.
		 * Evaluating {@code x.andNot(val)} returns the same result as {@code
		 * x.and(val.not())}.
		 * <p>
		 * <b>Implementation Note:</b> Usage of this method is not recommended as
		 * the current implementation is not efficient.
		 *
		 * @param val
		 *            value to be not'ed and then and'ed with {@code this}.
		 * @return {@code this & ~val}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */
        public BigInteger AndNot(BigInteger val)
        {
            return Logical.AndNot(this, val);
        }

        public BigInteger Min(BigInteger val)
        {
            return ((this.CompareTo(val) == LESS) ? this : val);
        }

        /**
		 * Returns the maximum of this {@code BigInteger} and {@code val}.
		 *
		 * @param val
		 *            value to be used to compute the maximum with {@code this}
		 * @return {@code max(this, val)}
		 * @throws NullPointerException
		 *             if {@code val == null}
		 */
        public BigInteger Max(BigInteger val)
        {
            return ((this.CompareTo(val) == GREATER) ? this : val);
        }

        public BigInteger Gcd(BigInteger val)
        {
            BigInteger val1 = Abs();
            BigInteger val2 = val.Abs();
            // To avoid a possible division by zero
            if (val1.Sign == 0)
            {
                return val2;
            }
            else if (val2.Sign == 0)
            {
                return val1;
            }

            // Optimization for small operands
            // (op2.bitLength() < 64) and (op1.bitLength() < 64)
            if (((val1.numberLength == 1) || ((val1.numberLength == 2) && (val1.digits[1] > 0)))
                && (val2.numberLength == 1 || (val2.numberLength == 2 && val2.digits[1] > 0)))
            {
                return BigInteger.ValueOf(Division.GcdBinary(val1.ToInt64(), val2.ToInt64()));
            }

            return Division.GcdBinary(val1.Copy(), val2.Copy());

        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this * val}.
		 *
		 * @param val
		 *            value to be multiplied with {@code this}.
		 * @return {@code this * val}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */
        public BigInteger Multiply(BigInteger val)
        {
            // This let us to throw NullPointerException when val == null
            if (val.sign == 0)
            {
                return Zero;
            }
            if (sign == 0)
            {
                return Zero;
            }
            return Multiplication.Multiply(this, val);
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this ^ exp}.
		 *
		 * @param exp
		 *            exponent to which {@code this} is raised.
		 * @return {@code this ^ exp}.
		 * @throws ArithmeticException
		 *             if {@code exp < 0}.
		 */
        public BigInteger Pow(int exp)
        {
            if (exp < 0)
            {
                // math.16=Negative exponent
                throw new ArithmeticException(Messages.math16); //$NON-NLS-1$
            }
            if (exp == 0)
            {
                return One;
            }
            else if (exp == 1 || Equals(One) || Equals(Zero))
            {
                return this;
            }

            // if even take out 2^x factor which we can
            // calculate by shifting.
            if (!TestBit(0))
            {
                int x = 1;
                while (!TestBit(x))
                {
                    x++;
                }
                return GetPowerOfTwo(x * exp).Multiply(this.ShiftRight(x).Pow(exp));
            }
            return Multiplication.Pow(this, exp);
        }

        /// <summary>
        /// Log10 the specified huge.
        /// </summary>
        /// <param name="huge">Huge.</param>
        public static int Log10(BigInteger huge)
        {
            int digits = 0;
            int bits = huge.BitLength;
            // Serious reductions.
            while (bits > 4)
            {
                // 4 > log[2](10) so we should not reduce it too far.
                int reduce = bits / 4;
                // Divide by 10^reduce
                huge = huge.Divide(BigInteger.Ten.Pow(reduce));
                // Removed that many decimal digits.
                digits += reduce;
                // Recalculate bitLength
                bits = huge.BitLength;
            }
            // Now 4 bits or less - add 1 if necessary.
            if (huge.ToInt32() > 9)
            {
                digits += 1;
            }
            return digits;
        }

        /**
		 * Returns a {@code BigInteger} array which contains {@code this / divisor}
		 * at index 0 and {@code this % divisor} at index 1.
		 *
		 * @param divisor
		 *            value by which {@code this} is divided.
		 * @return {@code [this / divisor, this % divisor]}.
		 * @throws NullPointerException
		 *             if {@code divisor == null}.
		 * @throws ArithmeticException
		 *             if {@code divisor == 0}.
		 * @see #divide
		 * @see #remainder
		 */
        public BigInteger DivideAndRemainder(BigInteger divisor, out BigInteger remainder)
        {
            int divisorSign = divisor.sign;
            if (divisorSign == 0)
            {
                // math.17=BigInteger divide by zero
                throw new ArithmeticException(Messages.math17); //$NON-NLS-1$
            }
            int divisorLen = divisor.numberLength;
            int[] divisorDigits = divisor.digits;
            if (divisorLen == 1)
            {
                var values = Division.DivideAndRemainderByInteger(this, divisorDigits[0], divisorSign);
                remainder = values[1];
                return values[0];
            }

            int[] thisDigits = digits;
            int thisLen = numberLength;
            int cmp = (thisLen != divisorLen) ? ((thisLen > divisorLen) ? 1 : -1)
				: Elementary.compareArrays(thisDigits, divisorDigits, thisLen);
            if (cmp < 0)
            {
                remainder = this;
                return Zero;
            }
            int thisSign = sign;
            int quotientLength = thisLen - divisorLen + 1;
            int remainderLength = divisorLen;
            int quotientSign = ((thisSign == divisorSign) ? 1 : -1);
            int[] quotientDigits = new int[quotientLength];
            int[] remainderDigits = Division.Divide(quotientDigits, quotientLength,
                                        thisDigits, thisLen, divisorDigits, divisorLen);

            var quotient = new BigInteger(quotientSign, quotientLength, quotientDigits);
            remainder = new BigInteger(thisSign, remainderLength, remainderDigits);
            quotient.CutOffLeadingZeroes();
            remainder.CutOffLeadingZeroes();

            return quotient;
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this / divisor}.
		 *
		 * @param divisor
		 *            value by which {@code this} is divided.
		 * @return {@code this / divisor}.
		 * @throws NullPointerException
		 *             if {@code divisor == null}.
		 * @throws ArithmeticException
		 *             if {@code divisor == 0}.
		 */
        public BigInteger Divide(BigInteger divisor)
        {
            if (divisor.sign == 0)
            {
                // math.17=BigInteger divide by zero
                throw new ArithmeticException(Messages.math17); //$NON-NLS-1$
            }
            int divisorSign = divisor.sign;
            if (divisor.IsOne)
            {
                return ((divisor.sign > 0) ? this : this.Negate());
            }
            int thisSign = sign;
            int thisLen = numberLength;
            int divisorLen = divisor.numberLength;
            if (thisLen + divisorLen == 2)
            {
                long val = (digits[0] & 0xFFFFFFFFL)
                           / (divisor.digits[0] & 0xFFFFFFFFL);
                if (thisSign != divisorSign)
                {
                    val = -val;
                }
                return ValueOf(val);
            }
            int cmp = ((thisLen != divisorLen) ? ((thisLen > divisorLen) ? 1 : -1)
				: Elementary.compareArrays(digits, divisor.digits, thisLen));
            if (cmp == EQUALS)
            {
                return ((thisSign == divisorSign) ? One : MinusOne);
            }
            if (cmp == LESS)
            {
                return Zero;
            }
            int resLength = thisLen - divisorLen + 1;
            int[] resDigits = new int[resLength];
            int resSign = ((thisSign == divisorSign) ? 1 : -1);
            if (divisorLen == 1)
            {
                Division.DivideArrayByInt(resDigits, digits, thisLen,
                    divisor.digits[0]);
            }
            else
            {
                Division.Divide(resDigits, resLength, digits, thisLen,
                    divisor.digits, divisorLen);
            }
            BigInteger result = new BigInteger(resSign, resLength, resDigits);
            result.CutOffLeadingZeroes();
            return result;
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this % divisor}.
		 * Regarding signs this methods has the same behavior as the % operator on
		 * int's, i.e. the sign of the remainder is the same as the sign of this.
		 *
		 * @param divisor
		 *            value by which {@code this} is divided.
		 * @return {@code this % divisor}.
		 * @throws NullPointerException
		 *             if {@code divisor == null}.
		 * @throws ArithmeticException
		 *             if {@code divisor == 0}.
		 */
        public BigInteger Remainder(BigInteger divisor)
        {
            if (divisor.sign == 0)
            {
                // math.17=BigInteger divide by zero
                throw new ArithmeticException(Messages.math17); //$NON-NLS-1$
            }
            int thisLen = numberLength;
            int divisorLen = divisor.numberLength;
            if (((thisLen != divisorLen) ? ((thisLen > divisorLen) ? 1 : -1)
				: Elementary.compareArrays(digits, divisor.digits, thisLen)) == LESS)
            {
                return this;
            }
            int resLength = divisorLen;
            int[] resDigits = new int[resLength];
            if (resLength == 1)
            {
                resDigits[0] = Division.RemainderArrayByInt(digits, thisLen,
                    divisor.digits[0]);
            }
            else
            {
                int qLen = thisLen - divisorLen + 1;
                resDigits = Division.Divide(null, qLen, digits, thisLen,
                    divisor.digits, divisorLen);
            }
            BigInteger result = new BigInteger(sign, resLength, resDigits);
            result.CutOffLeadingZeroes();
            return result;
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code 1/this mod m}. The
		 * modulus {@code m} must be positive. The result is guaranteed to be in the
		 * interval {@code [0, m)} (0 inclusive, m exclusive). If {@code this} is
		 * not relatively prime to m, then an exception is thrown.
		 *
		 * @param m
		 *            the modulus.
		 * @return {@code 1/this mod m}.
		 * @throws NullPointerException
		 *             if {@code m == null}
		 * @throws ArithmeticException
		 *             if {@code m < 0 or} if {@code this} is not relatively prime
		 *             to {@code m}
		 */
        public BigInteger ModInverse(BigInteger m)
        {
            if (m.sign <= 0)
            {
                // math.18=BigInteger: modulus not positive
                throw new ArithmeticException(Messages.math18); //$NON-NLS-1$
            }
            // If both are even, no inverse exists
            if (!(TestBit(0) || m.TestBit(0)))
            {
                // math.19=BigInteger not invertible.
                throw new ArithmeticException(Messages.math19); //$NON-NLS-1$
            }
            if (m.IsOne)
            {
                return Zero;
            }

            // From now on: (m > 1)
            BigInteger res = Division.ModInverseMontgomery(Abs().Mod(m), m);
            if (res.sign == 0)
            {
                // math.19=BigInteger not invertible.
                throw new ArithmeticException(Messages.math19); //$NON-NLS-1$
            }

            res = ((sign < 0) ? m.Subtract(res) : res);
            return res;

        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this^exponent mod
		 * m}. The modulus {@code m} must be positive. The result is guaranteed to
		 * be in the interval {@code [0, m)} (0 inclusive, m exclusive). If the
		 * exponent is negative, then {@code this.modInverse(m)^(-exponent) mod m)}
		 * is computed. The inverse of this only exists if {@code this} is
		 * relatively prime to m, otherwise an exception is thrown.
		 *
		 * @param exponent
		 *            the exponent.
		 * @param m
		 *            the modulus.
		 * @return {@code this^exponent mod val}.
		 * @throws NullPointerException
		 *             if {@code m == null} or {@code exponent == null}.
		 * @throws ArithmeticException
		 *             if {@code m < 0} or if {@code exponent<0} and this is not
		 *             relatively prime to {@code m}.
		 */
        public BigInteger ModPow(BigInteger exponent, BigInteger m)
        {
            if (m.sign <= 0)
            {
                // math.18=BigInteger: modulus not positive
                throw new ArithmeticException(Messages.math18); //$NON-NLS-1$
            }
            BigInteger b = this;

            if (m.IsOne | (exponent.sign > 0 & b.sign == 0))
            {
                return BigInteger.Zero;
            }
            if (b.sign == 0 && exponent.sign == 0)
            {
                return BigInteger.One;
            }
            if (exponent.sign < 0)
            {
                b = ModInverse(m);
                exponent = exponent.Negate();
            }
            // From now on: (m > 0) and (exponent >= 0)
            BigInteger res = (m.TestBit(0)) ? Division.OddModPow(b.Abs(),
                                 exponent, m) : Division.EvenModPow(b.Abs(), exponent, m);
            if ((b.sign < 0) && exponent.TestBit(0))
            {
                // -b^e mod m == ((-1 mod m) * (b^e mod m)) mod m
                res = m.Subtract(BigInteger.One).Multiply(res).Mod(m);
            }
            // else exponent is even, so base^exp is positive
            return res;
        }

        /**
		 * Returns a new {@code BigInteger} whose value is {@code this mod m}. The
		 * modulus {@code m} must be positive. The result is guaranteed to be in the
		 * interval {@code [0, m)} (0 inclusive, m exclusive). The behavior of this
		 * function is not equivalent to the behavior of the % operator defined for
		 * the built-in {@code int}'s.
		 *
		 * @param m
		 *            the modulus.
		 * @return {@code this mod m}.
		 * @throws NullPointerException
		 *             if {@code m == null}.
		 * @throws ArithmeticException
		 *             if {@code m < 0}.
		 */
        public BigInteger Mod(BigInteger m)
        {
            if (m.sign <= 0)
            {
                // math.18=BigInteger: modulus not positive
                throw new ArithmeticException(Messages.math18); //$NON-NLS-1$
            }
            BigInteger rem = Remainder(m);
            return ((rem.sign < 0) ? rem.Add(m) : rem);
        }

        /**
		 * Tests whether this {@code BigInteger} is probably prime. If {@code true}
		 * is returned, then this is prime with a probability beyond
		 * (1-1/2^certainty). If {@code false} is returned, then this is definitely
		 * composite. If the argument {@code certainty} <= 0, then this method
		 * returns true.
		 *
		 * @param certainty
		 *            tolerated primality uncertainty.
		 * @return {@code true}, if {@code this} is probably prime, {@code false}
		 *         otherwise.
		 */
        public bool IsProbablePrime(int certainty)
        {
            return Primality.IsProbablePrime(Abs(), certainty);
        }

        /**
		 * Returns the smallest integer x > {@code this} which is probably prime as
		 * a {@code BigInteger} instance. The probability that the returned {@code
		 * BigInteger} is prime is beyond (1-1/2^80).
		 *
		 * @return smallest integer > {@code this} which is robably prime.
		 * @throws ArithmeticException
		 *             if {@code this < 0}.
		 */
        public BigInteger NextProbablePrime()
        {
            if (sign < 0)
            {
                // math.1A=start < 0: {0}
                throw new ArithmeticException(String.Format(Messages.math1A, this)); //$NON-NLS-1$
            }
            return Primality.NextProbablePrime(this);
        }

        /**
		 * Returns a random positive {@code BigInteger} instance in the range [0,
		 * 2^(bitLength)-1] which is probably prime. The probability that the
		 * returned {@code BigInteger} is prime is beyond (1-1/2^80).
		 * <p>
		 * <b>Implementation Note:</b> Currently {@code rnd} is ignored.
		 *
		 * @param bitLength
		 *            length of the new {@code BigInteger} in bits.
		 * @param rnd
		 *            random generator used to generate the new {@code BigInteger}.
		 * @return probably prime random {@code BigInteger} instance.
		 * @throws IllegalArgumentException
		 *             if {@code bitLength < 2}.
		 */
        public static BigInteger ProbablePrime(int bitLength, Random rnd)
        {
            return new BigInteger(bitLength, 100, rnd);
        }

#endregion

        /**
		 * Returns this {@code BigInteger} as an int value. If {@code this} is too
		 * big to be represented as an int, then {@code this} % 2^32 is returned.
		 *
		 * @return this {@code BigInteger} as an int value.
		 */

        /**
		 * Compares this {@code BigInteger} with {@code val}. Returns one of the
		 * three values 1, 0, or -1.
		 *
		 * @param val
		 *            value to be compared with {@code this}.
		 * @return {@code 1} if {@code this > val}, {@code -1} if {@code this < val}
		 *         , {@code 0} if {@code this == val}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */
        public int CompareTo(BigInteger val)
        {
            if (sign > val.sign)
            {
                return GREATER;
            }
            if (sign < val.sign)
            {
                return LESS;
            }
            if (numberLength > val.numberLength)
            {
                return sign;
            }
            if (numberLength < val.numberLength)
            {
                return -val.sign;
            }
            // Equal sign and equal numberLength
            return (sign * Elementary.compareArrays(digits, val.digits,
                numberLength));
        }

        /**
		 * Returns the minimum of this {@code BigInteger} and {@code val}.
		 *
		 * @param val
		 *            value to be used to compute the minimum with {@code this}.
		 * @return {@code min(this, val)}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */

        /**
		 * Returns a hash code for this {@code BigInteger}.
		 *
		 * @return hash code for {@code this}.
		 */
        public override int GetHashCode()
        {
            if (hashCode != 0)
            {
                return hashCode;
            }
            for (int i = 0; i < digits.Length; i++)
            {
                hashCode = (int)(hashCode * 33 + (digits[i] & 0xffffffff));
            }
            hashCode = hashCode * sign;
            return hashCode;
        }

        /**
		 * Returns {@code true} if {@code x} is a BigInteger instance and if this
		 * instance is equal to this {@code BigInteger}.
		 *
		 * @param x
		 *            object to be compared with {@code this}.
		 * @return true if {@code x} is a BigInteger and {@code this == x},
		 *          {@code false} otherwise.
		 */
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (!(obj is BigInteger))
                return false;
            return Equals((BigInteger)obj);
        }

        public bool Equals(BigInteger other)
        {
            if (other == null)
                return false;

            return sign == other.sign &&
            numberLength == other.numberLength &&
            EqualsArrays(other.digits);
        }

        bool EqualsArrays(int[] b)
        {
            int i;
            for (i = numberLength - 1; (i >= 0) && (digits[i] == b[i]); i--)
            {
                // Empty
            }
            return i < 0;
        }

        /**
		 * Returns a new {@code BigInteger} whose value is greatest common divisor
		 * of {@code this} and {@code val}. If {@code this==0} and {@code val==0}
		 * then zero is returned, otherwise the result is positive.
		 *
		 * @param val
		 *            value with which the greatest common divisor is computed.
		 * @return {@code gcd(this, val)}.
		 * @throws NullPointerException
		 *             if {@code val == null}.
		 */

        /* Private Methods */

        /** Decreases {@code numberLength} if there are zero high elements. */

        internal void CutOffLeadingZeroes()
        {
            while ((numberLength > 0) && (digits[--numberLength] == 0))
            {
                // Empty
            }
            if (digits[numberLength++] == 0)
            {
                sign = 0;
            }
        }

        /// <summary>
        /// Indicates if this instance absolute value is equivalent to <c>1</c>.
        /// </summary>
        internal bool IsOne
        {
            get { return ((numberLength == 1) && (digits[0] == 1)); }
        }

        /**
		 * Puts a big-endian byte array into a little-endian int array.
		 */
        private void PutBytesPositiveToIntegers(byte[] byteValues)
        {
            int bytesLen = byteValues.Length;
            int highBytes = bytesLen & 3;
            numberLength = (bytesLen >> 2) + ((highBytes == 0) ? 0 : 1);
            digits = new int[numberLength];
            int i = 0;
            // Put bytes to the int array starting from the end of the byte array
            while (bytesLen > highBytes)
            {
                digits[i++] = (byteValues[--bytesLen] & 0xFF)
                | (byteValues[--bytesLen] & 0xFF) << 8
                | (byteValues[--bytesLen] & 0xFF) << 16
                | (byteValues[--bytesLen] & 0xFF) << 24;
            }
            // Put the first bytes in the highest element of the int array
            for (int j = 0; j < bytesLen; j++)
            {
                digits[i] = (digits[i] << 8) | (byteValues[j] & 0xFF);
            }
        }

        /**
		 * Puts a big-endian byte array into a little-endian applying two
		 * complement.
		 */
        private void PutBytesNegativeToIntegers(byte[] byteValues)
        {
            int bytesLen = byteValues.Length;
            int highBytes = bytesLen & 3;
            numberLength = (bytesLen >> 2) + ((highBytes == 0) ? 0 : 1);
            digits = new int[numberLength];
            int i = 0;
            // Setting the sign
            digits[numberLength - 1] = -1;
            // Put bytes to the int array starting from the end of the byte array
            while (bytesLen > highBytes)
            {
                digits[i] = (byteValues[--bytesLen] & 0xFF)
                | (byteValues[--bytesLen] & 0xFF) << 8
                | (byteValues[--bytesLen] & 0xFF) << 16
                | (byteValues[--bytesLen] & 0xFF) << 24;
                if (digits[i] != 0)
                {
                    digits[i] = -digits[i];
                    firstNonzeroDigit = i;
                    i++;
                    while (bytesLen > highBytes)
                    {
                        digits[i] = (byteValues[--bytesLen] & 0xFF)
                        | (byteValues[--bytesLen] & 0xFF) << 8
                        | (byteValues[--bytesLen] & 0xFF) << 16
                        | (byteValues[--bytesLen] & 0xFF) << 24;
                        digits[i] = ~digits[i];
                        i++;
                    }
                    break;
                }
                i++;
            }
            if (highBytes != 0)
            {
                // Put the first bytes in the highest element of the int array
                if (firstNonzeroDigit != -2)
                {
                    for (int j = 0; j < bytesLen; j++)
                    {
                        digits[i] = (digits[i] << 8) | (byteValues[j] & 0xFF);
                    }
                    digits[i] = ~digits[i];
                }
                else
                {
                    for (int j = 0; j < bytesLen; j++)
                    {
                        digits[i] = (digits[i] << 8) | (byteValues[j] & 0xFF);
                    }
                    digits[i] = -digits[i];
                }
            }
        }

        /*
		 * Returns a copy of the current instance to achieve immutability
		 */

        internal BigInteger Copy()
        {
            int[] copyDigits = new int[numberLength];
            Array.Copy(digits, 0, copyDigits, 0, numberLength);
            return new BigInteger(sign, numberLength, copyDigits);
        }

        internal void UnCache()
        {
            firstNonzeroDigit = -2;
        }

        internal static BigInteger GetPowerOfTwo(int exp)
        {
            if (exp < TwoPows.Length)
            {
                return TwoPows[exp];
            }
            int intCount = exp >> 5;
            int bitN = exp & 31;
            int[] resDigits = new int[intCount + 1];
            resDigits[intCount] = 1 << bitN;
            return new BigInteger(1, intCount + 1, resDigits);
        }

#region Conversions

#if !PORTABLE
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            int value = ToInt32();
            if (value > Byte.MaxValue || value < Byte.MinValue)
                throw new InvalidCastException();
            return (byte)value;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            int value = ToInt32();
            if (value > Int16.MaxValue || value < Int16.MinValue)
                throw new InvalidCastException();
            return (short)value;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return ToInt32();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return ToInt64();
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return ToSingle();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ToDouble();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(byte))
                return (this as IConvertible).ToByte(provider);
            if (conversionType == typeof(short))
                return (this as IConvertible).ToInt16(provider);
            if (conversionType == typeof(int))
                return ToInt32();
            if (conversionType == typeof(long))
                return ToInt64();
            if (conversionType == typeof(float))
                return ToSingle();
            if (conversionType == typeof(double))
                return ToDouble();
            if (conversionType == typeof(string))
                return ToString();
            if (conversionType == typeof(byte[]))
                return ToByteArray();

            throw new NotSupportedException();
        }
#endif

        public static BigInteger ValueOf(long val)
        {
            if (val < 0)
            {
                if (val != -1)
                {
                    return new BigInteger(-1, -val);
                }
                return MinusOne;
            }
            else if (val <= 10)
            {
                return SmallValues[(int)val];
            }
            else
            {// (val > 10)
                return new BigInteger(1, val);
            }
        }

        /**
		 * Returns the two's complement representation of this BigInteger in a byte
		 * array.
		 *
		 * @return two's complement representation of {@code this}.
		 */
        public byte[] ToByteArray()
        {
            if (sign == 0)
            {
                return new byte[] { 0 };
            }
            BigInteger temp = this;
            int bitLen = BitLength;
            int iThis = FirstNonzeroDigit;
            int bytesLen = (bitLen >> 3) + 1;
            /*
			 * Puts the little-endian int array representing the magnitude of this
			 * BigInteger into the big-endian byte array.
			 */
            byte[] bytes = new byte[bytesLen];
            int firstByteNumber = 0;
            int highBytes;
            int digitIndex = 0;
            int bytesInInteger = 4;
            int digit;
            int hB;

            if (bytesLen - (numberLength << 2) == 1)
            {
                bytes[0] = (byte)((sign < 0) ? -1 : 0);
                highBytes = 4;
                firstByteNumber++;
            }
            else
            {
                hB = bytesLen & 3;
                highBytes = (hB == 0) ? 4 : hB;
            }

            digitIndex = iThis;
            bytesLen -= iThis << 2;

            if (sign < 0)
            {
                digit = -temp.digits[digitIndex];
                digitIndex++;
                if (digitIndex == numberLength)
                {
                    bytesInInteger = highBytes;
                }
                for (int i = 0; i < bytesInInteger; i++, digit >>= 8)
                {
                    bytes[--bytesLen] = (byte)digit;
                }
                while (bytesLen > firstByteNumber)
                {
                    digit = ~temp.digits[digitIndex];
                    digitIndex++;
                    if (digitIndex == numberLength)
                    {
                        bytesInInteger = highBytes;
                    }
                    for (int i = 0; i < bytesInInteger; i++, digit >>= 8)
                    {
                        bytes[--bytesLen] = (byte)digit;
                    }
                }
            }
            else
            {
                while (bytesLen > firstByteNumber)
                {
                    digit = temp.digits[digitIndex];
                    digitIndex++;
                    if (digitIndex == numberLength)
                    {
                        bytesInInteger = highBytes;
                    }
                    for (int i = 0; i < bytesInInteger; i++, digit >>= 8)
                    {
                        bytes[--bytesLen] = (byte)digit;
                    }
                }
            }
            return bytes;
        }

        public int ToInt32()
        {
            return (sign * digits[0]);
        }

        /**
		 * Returns this {@code BigInteger} as an long value. If {@code this} is too
		 * big to be represented as an long, then {@code this} % 2^64 is returned.
		 *
		 * @return this {@code BigInteger} as a long value.
		 */
        public long ToInt64()
        {
            long value = (numberLength > 1) ? 
				(((long)digits[1]) << 32) | (digits[0] & 0xFFFFFFFFL) : 
				(digits[0] & 0xFFFFFFFFL);
            return (sign * value);
        }

        /**
		 * Returns this {@code BigInteger} as an float value. If {@code this} is too
		 * big to be represented as an float, then {@code Float.POSITIVE_INFINITY}
		 * or {@code Float.NEGATIVE_INFINITY} is returned. Note, that not all
		 * integers x in the range [-Float.MAX_VALUE, Float.MAX_VALUE] can be
		 * represented as a float. The float representation has a mantissa of length
		 * 24. For example, 2^24+1 = 16777217 is returned as float 16777216.0.
		 *
		 * @return this {@code BigInteger} as a float value.
		 */
        public float ToSingle()
        {
            return (float)ToDouble();
        }

        /**
		 * Returns this {@code BigInteger} as an double value. If {@code this} is
		 * too big to be represented as an double, then {@code
		 * Double.POSITIVE_INFINITY} or {@code Double.NEGATIVE_INFINITY} is
		 * returned. Note, that not all integers x in the range [-Double.MAX_VALUE,
		 * Double.MAX_VALUE] can be represented as a double. The double
		 * representation has a mantissa of length 53. For example, 2^53+1 =
		 * 9007199254740993 is returned as double 9007199254740992.0.
		 *
		 * @return this {@code BigInteger} as a double value
		 */
        public double ToDouble()
        {
            return Conversion.BigInteger2Double(this);
        }

        public override String ToString()
        {
            return Conversion.ToDecimalScaledString(this, 0);
        }

        /**
		 * Returns a string containing a string representation of this {@code
		 * BigInteger} with base radix. If {@code radix < CharHelper.MIN_RADIX} or
		 * {@code radix > CharHelper.MAX_RADIX} then a decimal representation is
		 * returned. The CharHelpers of the string representation are generated with
		 * method {@code CharHelper.forDigit}.
		 *
		 * @param radix
		 *            base to be used for the string representation.
		 * @return a string representation of this with radix 10.
		 */
        public String ToString(int radix)
        {
            return Conversion.BigInteger2String(this, radix);
        }

        private static bool TryParse(string s, int radix, out BigInteger value, out Exception exception)
        {
            if (String.IsNullOrEmpty(s))
            {
                exception = new FormatException(Messages.math11);
                value = null;
                return false;
            }
            if ((radix < CharHelper.MIN_RADIX) || (radix > CharHelper.MAX_RADIX))
            {
                // math.11=Radix out of range
                exception = new FormatException(Messages.math12);
                value = null;
                return false;
            }

            int sign;
            int[] digits;
            int numberLength;
            int stringLength = s.Length;
            int startChar;
            int endChar = stringLength;

            if (s[0] == '-')
            {
                sign = -1;
                startChar = 1;
                stringLength--;
            }
            else
            {
                sign = 1;
                startChar = 0;
            }
            /*
			 * We use the following algorithm: split a string into portions of n
			 * char and convert each portion to an integer according to the
			 * radix. Then convert an exp(radix, n) based number to binary using the
			 * multiplication method. See D. Knuth, The Art of Computer Programming,
			 * vol. 2.
			 */

            try
            {
                int charsPerInt = Conversion.digitFitInInt[radix];
                int bigRadixDigitsLength = stringLength / charsPerInt;
                int topChars = stringLength % charsPerInt;

                if (topChars != 0)
                {
                    bigRadixDigitsLength++;
                }
                digits = new int[bigRadixDigitsLength];
                // Get the maximal power of radix that fits in int
                int bigRadix = Conversion.bigRadices[radix - 2];
                // Parse an input string and accumulate the BigInteger's magnitude
                int digitIndex = 0; // index of digits array
                int substrEnd = startChar + ((topChars == 0) ? charsPerInt : topChars);
                int newDigit;

                for (int substrStart = startChar; substrStart < endChar; substrStart = substrEnd, substrEnd = substrStart
                + charsPerInt)
                {
                    int bigRadixDigit = Convert.ToInt32(s.Substring(substrStart, substrEnd - substrStart), radix);
                    newDigit = Multiplication.MultiplyByInt(digits, digitIndex, bigRadix);
                    newDigit += Elementary.inplaceAdd(digits, digitIndex, bigRadixDigit);
                    digits[digitIndex++] = newDigit;
                }

                numberLength = digitIndex;
            }
            catch (Exception ex)
            {
                exception = ex;
                value = null;
                return false;
            }

            value = new BigInteger();
            value.sign = sign;
            value.numberLength = numberLength;
            value.digits = digits;
            value.CutOffLeadingZeroes();
            exception = null;
            return true;
        }

        public static BigInteger Parse(string s)
        {
            return Parse(s, 10);
        }

        public static bool TryParse(string s, out BigInteger value)
        {
            return TryParse(s, 10, out value);
        }

        public static bool TryParse(string s, int radix, out BigInteger value)
        {
            Exception error;
            return TryParse(s, radix, out value, out error);
        }

        public static BigInteger Parse(string s, int radix)
        {
            Exception error;
            BigInteger i;
            if (!TryParse(s, radix, out i, out error))
                throw error;

            return i;
        }

#endregion

#region Operators

        public static BigInteger operator +(BigInteger a, BigInteger b)
        {
            if (a == null)
                throw new InvalidOperationException();
            return a.Add(b);
        }

        public static BigInteger operator -(BigInteger a, BigInteger b)
        {
            if (a == null)
                throw new InvalidOperationException();
            return a.Subtract(b);
        }

        public static BigInteger operator *(BigInteger a, BigInteger b)
        {
            return a.Multiply(b);
        }

        public static BigInteger operator /(BigInteger a, BigInteger b)
        {
            return a.Divide(b);
        }

        public static BigInteger operator %(BigInteger a, BigInteger b)
        {
            return a.Mod(b);
        }

        public static BigInteger operator &(BigInteger a, BigInteger b)
        {
            return a.And(b);
        }

        public static BigInteger operator |(BigInteger a, BigInteger b)
        {
            return a.Or(b);
        }

        public static BigInteger operator ^(BigInteger a, BigInteger b)
        {
            return a.XOr(b);
        }

        public static BigInteger operator ~(BigInteger a)
        {
            return a.Not();
        }

        public static BigInteger operator -(BigInteger a)
        {
            return a.Negate();
        }

        public static BigInteger operator >>(BigInteger a, int b)
        {
            return a.ShiftRight(b);
        }

        public static BigInteger operator <<(BigInteger a, int b)
        {
            return a.ShiftLeft(b);
        }

        public static bool operator >(BigInteger a, BigInteger b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(BigInteger a, BigInteger b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator ==(BigInteger a, BigInteger b)
        {
            if ((object)a == null && (object)b == null)
                return true;
            if ((object)a == null)
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(BigInteger a, BigInteger b)
        {
            return !(a == b);
        }

        public static bool operator >=(BigInteger a, BigInteger b)
        {
            return a == b || a > b;
        }

        public static bool operator <=(BigInteger a, BigInteger b)
        {
            return a == b || a < b;
        }

#endregion

#region Implicit Operators

        public static implicit operator Int32(BigInteger i)
        {
            return i.ToInt32();
        }

        public static implicit operator Int64(BigInteger i)
        {
            return i.ToInt64();
        }

        public static implicit operator Single(BigInteger i)
        {
            return i.ToSingle();
        }

        public static implicit operator Double(BigInteger i)
        {
            return i.ToDouble();
        }

        public static implicit operator String(BigInteger i)
        {
            return i.ToString();
        }

        public static implicit operator BigInteger(int value)
        {
            return ValueOf(value);
        }

        public static implicit operator BigInteger(long value)
        {
            return ValueOf(value);
        }

#endregion
    }
}