namespace Chistes.Api.Services
{
	public class MathOperationService
	{
		public static int CalculateLCM(List<int> numeros)
		{
			// Verificar si la lista de números es nula o vacía
			if (numeros == null || numeros.Count == 0)
			{
				throw new ArgumentException("La lista de números está vacía o es nula.");
			}

			// Calcular el MCM de todos los números en la lista
			int mcm = numeros[0];
			for (int i = 1; i < numeros.Count; i++)
			{
				mcm = CalculateLCM(mcm, numeros[i]);
			}

			return mcm;
		}

		// Método privado para calcular el MCM de dos números
		private static int CalculateLCM(int a, int b)
		{
			return (a * b) / CalculateGCD(a, b);
		}

		// Método privado para calcular el Máximo Común Divisor (MCD) de dos números
		private static int CalculateGCD(int a, int b)
		{
			while (b != 0)
			{
				int temp = b;
				b = a % b;
				a = temp;
			}

			return a;
		}

		public static int CalculateNextNumber(int number)
		{
			return ++number;
		}
	}
}
