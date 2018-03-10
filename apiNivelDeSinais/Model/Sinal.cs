using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiNivelDeSinais.Model
{
	/// <summary>
	/// Sinal do equipamento
	/// </summary>
    public class Sinal
    {
		private static decimal GetFator1(string mac)
		{
			var decValue = Int64.Parse(mac, System.Globalization.NumberStyles.HexNumber);
			return (decValue % 12345M);
		}

		private static decimal GetFator2(DateTime data)
		{
			return Convert.ToDecimal(data.Day + data.Month);
		}

		/// <summary>
		/// Calcula o nível do sinal 1
		/// </summary>
		/// <param name="macId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static double CalcularSinal1(string macId, DateTime data)
		{
			var mac = macId.Replace("-", "").Replace(":", "");
			var fator1 = GetFator1(mac);
			var fator2 = GetFator2(data);

			var value = ((double)(fator1 * fator2) % Math.PI);

			return Math.Sin(value);

		}

		/// <summary>
		/// Calcula o nivel do sinal 2
		/// </summary>
		/// <param name="macId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static double CalcularSinal2(string macId, DateTime data)
		{
			var mac = macId.Replace("-", "").Replace(":", "");
			var fator1 = GetFator1(mac);
			var fator2 = GetFator2(data);

			var value = ((double)(fator1 * fator2) % Math.PI);

			return Math.Cos(value);
		}
	}
}
