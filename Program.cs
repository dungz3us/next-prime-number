// Nguyễn Quang Dũng - 20206277
using System;
using System.Numerics;

namespace next_prime_number
{
    class Program
    {
        public static BigInteger createBigInteger(int bitLength)
        {
            if (bitLength < 1) return BigInteger.Zero;

            int bytes = bitLength / 8;
            int bits = bitLength % 8;

            // Generates enough random bytes to cover our bits.
            Random rnd = new Random();
            byte[] bs = new byte[bytes + 1];
            rnd.NextBytes(bs);

            // Mask out the unnecessary bits.
            byte mask = (byte)(0xFF >> (8 - bits));
            bs[bs.Length - 1] &= mask;

            return new BigInteger(bs);
        }                       

        // Random Integer Generator within the given range
        public static BigInteger RandomBigInteger(BigInteger start, BigInteger end)
        {
            if (start == end) return start;

            BigInteger res = end;

            // Swap start and end if given in reverse order.
            if (start > end)
            {
                end = start;
                start = res;
                res = end - start;
            }
            else
               
                res -= start;

            byte[] bs = res.ToByteArray();

            int bits = 8;
            byte mask = 0x7F;
            while ((bs[bs.Length - 1] & mask) == bs[bs.Length - 1])
            {
                bits--;
                mask >>= 1;
            }
            bits += 8 * bs.Length;

            return ((createBigInteger(bits + 1) * res) / BigInteger.Pow(2, bits + 1)) + start;
        }
        public static bool IsProbablePrime(BigInteger source, int certainty)
        {
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source % 2 == 0)
                return false;

            BigInteger d = source - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }
            Random rnd = new Random();
            BigInteger a;
            for (int i = 0; i < certainty; i++)
            {
                do
                {
                    a = RandomBigInteger(2, source - 2);
                }
                while (a < 2 || a >= source - 2);

                BigInteger x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }

                if (x != source - 1)
                    return false;
            }

            return true;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a number: ");
            BigInteger number = BigInteger.Parse(Console.ReadLine());
            for(BigInteger i = number; ; i++)
            {
                if(IsProbablePrime(i, 100))
                    {
                        Console.WriteLine("The next prime number is: ");
                        Console.WriteLine(i);
                        break;
                    }
            }
            Console.ReadLine();
        }        
    }
}
