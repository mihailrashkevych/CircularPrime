﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CircularPrime
{
    class Program
    {

        public static void Main(string[] args)
        {
            new Program().GetCircularPrime();
            Console.ReadKey();
        }

        SortedSet<int> primes;

        public void GetCircularPrime()
        {
            Stopwatch clock = Stopwatch.StartNew();

            int noCircularPrimes = 2;
            primes = new SortedSet<int>(ESieve(1000000));
 
            while (primes.Count > 0)
            {
                noCircularPrimes += CheckCircularPrimes(primes.Min);
            }

            clock.Stop();
            Console.WriteLine("The number of ciruclar primes below 1.000.000 is {0}", noCircularPrimes);
            Console.WriteLine("Solution took {0} ms", clock.ElapsedMilliseconds);
        }

        public int CheckCircularPrimes(int prime)
        {
            int multiplier = 1;
            int number = prime;
            int count = 0;
            int d;

            //Count the digits and check for even numbers
            while (number > 0)
            {
                d = number % 10;
                if (d % 2 == 0 || d == 5)
                {
                    primes.Remove(prime);
                    return 0;
                }
                number /= 10;
                multiplier *= 10;
                count++;
            }
            multiplier /= 10;

            //Rotate the number and check if they are prime
            number = prime;
            List<int> foundCircularPrimes = new List<int>();

            for (int i = 0; i < count; i++)
            {
                if (primes.Contains(number))
                {
                    foundCircularPrimes.Add(number);
                    primes.Remove(number);
                }
                else if (!foundCircularPrimes.Contains(number))
                {
                    return 0;
                }

                d = number % 10;
                number = d * multiplier + number / 10;
            }

            return foundCircularPrimes.Count;
        }


        public int[] ESieve(int upperLimit)
        {

            int sieveBound = (int)(upperLimit - 1) / 2;
            int upperSqrt = ((int)Math.Sqrt(upperLimit) - 1) / 2;

            BitArray PrimeBits = new BitArray(sieveBound + 1, true);

            for (int i = 1; i <= upperSqrt; i++)
            {
                if (PrimeBits.Get(i))
                {
                    for (int j = i * 2 * (i + 1); j <= sieveBound; j += 2 * i + 1)
                    {
                        PrimeBits.Set(j, false);
                    }
                }
            }

            List<int> numbers = new List<int>((int)(upperLimit / (Math.Log(upperLimit) - 1.08366)));
            numbers.Add(2);
            for (int i = 1; i <= sieveBound; i++)
            {
                if (PrimeBits.Get(i))
                {
                    numbers.Add(2 * i + 1);
                }
            }

            return numbers.ToArray();
        }

    }
}
