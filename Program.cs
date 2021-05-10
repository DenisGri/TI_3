using System;
using System.Collections.Generic;
using System.Linq;

namespace RSA_Check
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("List of prime numbers:\n" +
                              "2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97,\n" +
                              "101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197,\n" +
                              "199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271\n");
            Console.WriteLine("Write prime number p:");
            var p = long.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Write prime number q:");
            var q = long.Parse(Console.ReadLine() ?? string.Empty);
            Console.WriteLine("Write encrypted text:");
            var text = Console.ReadLine()?.ToUpper();
            text = text?.Replace("\\s", "");
            var r = p * q;
            var f = (p - 1) * (q - 1);
            var e = GetE(f);
            var temp = GetExtendGcd(f, e);
            var d = temp.Y;
            if (d < 0)
            {
                d += f;
            }

            var mainHash = RsaCheckHash(text, e);
            var sign = Power(mainHash, d, r);
            Console.WriteLine($"Hash: {mainHash}\nSign: {sign}");
            Console.WriteLine($"Write message and sign to check");
            var checkText = Console.ReadLine()?.ToUpper();
            checkText = checkText?.Replace("\\s", "");
            var newSign = long.Parse(Console.ReadLine() ?? string.Empty);
            var newHash = RsaCheckHash(checkText, r);
            Console.WriteLine(newHash == Power(newSign, e, r) ? $"Sign accepted" : $"Sign denied");
            Console.WriteLine($"Hash: {newHash}\nSignature hash: {Power(newSign, e, r)}");
        }

        private static long RsaCheckHash(string str, long n)
        {
            return str.Aggregate<char, long>(100, (current, t) => (current + t) * (current + t % n));
        }

        private static long GetE(long f)
        {
            var valArr = new List<long>();
            var e = f - 1;
            for (var i = 2; i < f; i++)
            {
                if (IsPrime(e) && IsMutuallySimple(e, f))
                {
                    valArr.Add(e);
                }

                e--;
            }

            Random random = new();
            var index = random.Next(valArr.Count);
            return valArr[index];
        }

        private static TempValuesGcd GetExtendGcd(long a, long b)
        {
            if (b == 0)
            {
                return new TempValuesGcd(a, 1, 0);
            }

            var tmp = GetExtendGcd(b, a % b);
            var d = tmp.D;
            var y = tmp.X - tmp.Y * (a / b);
            var x = tmp.Y;
            return new TempValuesGcd(d, x, y);
        }

        private static long Power(long x, long y, long n)
        {
            if (y == 0)
            {
                return 1;
            }

            var z = Power(x, y / 2, n);
            if (y % 2 == 0) return (z * z) % n;
            return (x * z * z) % n;
        }

        private static bool IsMutuallySimple(long a, long b)
        {
            if (a == b)
            {
                return a == 1;
            }

            if (a > b)
            {
                return IsMutuallySimple(a - b, b);
            }

            return IsMutuallySimple(b - a, a);
        }

        private static bool IsPrime(long a)
        {
            for (long i = 2; i <= Math.Sqrt(a); i++)
            {
                if (a % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private class TempValuesGcd
        {
            public TempValuesGcd(long d, long x, long y)
            {
                D = d;
                X = x;
                Y = y;
            }

            public long D { get; }
            public long X { get; }
            public long Y { get; }
        }
    }
}