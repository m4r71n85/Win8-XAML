using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncPrimes
{
	public static class PrimesCalculator
	{
		public static Task<List<int>> GetPrimesInRangeAsync(int rangeFirst, int rangeLast)
		{
			return Task.Run(
				() => GetPrimesInRange(rangeFirst, rangeLast)
			);
		}

		public static List<int> GetPrimesInRange(int rangeFirst, int rangeLast, bool firstOnly = false)
		{
			List<int> primes = new List<int>();

			for (int number = rangeFirst; number < rangeLast; number++)
			{
				if (CheckIsPrime(number))
				{
					primes.Add(number);
					if (firstOnly)
					{
						return primes;
					}
				}
			}

			return primes;
		}

		public static bool CheckIsPrime(int number)
		{
			bool isPrime = true;
			for (int divider = 2; divider < number; divider++)
			{
				if (number % divider == 0)
				{
					isPrime = false;
					break;
				}
			}
			return isPrime;
		}

		public static Task<List<string>> FindPrimesPartnersAsync(List<int> primes, bool getPrimesOnly)
		{
			return Task.Run(
				() => FindPrimesPartners(primes, getPrimesOnly)
			);
		}

		private static List<string> FindPrimesPartners(List<int> primes, bool getPrimesOnly)
		{
			List<string> primesWithPartners = new List<string>();
			foreach (int prime in primes)
			{
				int lastDigit = prime % 10;

				if (lastDigit != prime && CheckIsPrime(lastDigit))
				{
					primesWithPartners.Add(prime + "_" + lastDigit);
					continue;
				}

				int rangeFrom = lastDigit;

				rangeFrom *= 10;
				int rangeTo = rangeFrom + 9;

				bool partnerFound = false;
				while (!partnerFound)
				{
					List<int> partners = GetPrimesInRange(rangeFrom, rangeTo, true); //true stands for first only!
					if (partners.Count > 0)
					{
						int newNumber = int.Parse(prime+""+partners[0]);
						if (CheckIsPrime(newNumber) == getPrimesOnly)
						{
							primesWithPartners.Add(prime + "_" + partners[0]);
						}
						partnerFound = true;
					}
					rangeFrom *= 10;
					rangeTo += 9;
				}
			}

			return primesWithPartners;
		}
	}
}