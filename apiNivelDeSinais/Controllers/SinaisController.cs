using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using apiNivelDeSinais.Model;
using Microsoft.AspNetCore.Mvc;

namespace apiNivelDeSinais.Controllers
{
	/// <summary>
	/// API de sinais de equipamento
	/// </summary>
	public class SinaisController : Controller
	{

		// GET api/sinais/{macId}
		/// <summary>
		/// Retorna o nível de sinal atual de um equipamento
		/// </summary>
		/// <remarks>
		/// 	Essa função retorna os níveis de sinais de um determinado equipamento para o período informado. 
		/// 	O MAC address do equipamento deve estar no formato hexadecimal: HH-HH-HH-HH-HH-HH
		///		Ex.:
		///		http://{server}/api/sinais/ED-2A-11-34-90-FB
		/// </remarks>
		/// <param name="macId">MAC do Equipamento, ex.: ED-2A-11-34-90-FB</param>
		/// <returns>Valor do Sinal</returns>
		[HttpGet()]
		[Route("api/sinais/{macId}")]
		[ProducesResponseType(typeof(string), 400)]
		[ProducesResponseType(typeof(Resultado), 200)]
		public IActionResult Get(string macId)
		{
			if (!ValidateMACAddress(macId))
				return BadRequest("ID do equipamento é inválido");

			var data = DateTime.Now.Date;

			return Ok(new Resultado
			{
				Data = data,
				Sinal1 = Sinal.CalcularSinal1(macId, data),
				Sinal2 = Sinal.CalcularSinal2(macId, data)
			});
		}

		// GET api/sinais/{macId}/filter
		/// <summary>
		/// Retorna os níveis de sinais de um equipamento por data
		/// </summary>
		/// <remarks>
		/// 	Essa função retorna os níveis de sinais de um determinado equipamento para o período informado. 
		/// 	O MAC address do equipamento deve estar no formato hexadecimal: HH-HH-HH-HH-HH-HH
		/// 	A data inicial e final devem ser informadas no formato: YYYY-MM-DD
		///		Ex.: http://{server}/api/sinais/ED-2A-11-34-90-FB/filter?dataIni=2017-01-01&amp;dataFim=2017-01-30
		/// </remarks>
		/// <param name="macId">MAC do Equipamento, ex.: ED-2A-11-34-90-FB</param>
		/// <param name="dataIni">Data Inicial</param>
		/// <param name="dataFim">Data Final</param>	
		/// <returns>Valores dos sinais</returns>
		[HttpGet]
		[Route("api/sinais/{macId}/filter")]
		[ProducesResponseType(typeof(string), 400)]
		[ProducesResponseType(typeof(Resultado[]), 200)]
		public IActionResult Get(string macId, [FromQuery]DateTime dataIni, [FromQuery]DateTime dataFim)
		{
			if (!ValidateMACAddress(macId))
				return BadRequest("ID do equipamento é inválido");

			if (!ValidateFilter(dataIni, dataFim))
				return BadRequest("Filtro de datas inválidos");

			var response = new List<Resultado>();
			var data = dataIni;
			while (data <= dataFim)
			{

				response.Add(new Resultado
				{
					Data = data,
					Sinal1 = Sinal.CalcularSinal1(macId, data),
					Sinal2 = Sinal.CalcularSinal2(macId, data)
				});

				data = data.AddDays(1);
			}

			return Ok(response.ToArray());
		}

		//Validação de mac address
		private bool ValidateMACAddress(string macAddress)
		{
			var pattern = "^([0-9A-Fa-f]{2}[-:]){5}([0-9A-Fa-f]{2})$";
			return Regex.IsMatch(macAddress, pattern);
		}

		//Validação de datas
		private bool ValidateFilter(DateTime dataIni, DateTime dataFim)
		{
			if (dataIni == DateTime.MinValue || dataFim == DateTime.MinValue)
				return false;

			if (dataIni >= dataFim)
				return false;

			return true;
		}

		/// <summary>
		/// Classe de resultado da api
		/// </summary>
		public class Resultado
		{
			/// <summary>
			/// Data da leitura
			/// </summary>
			public DateTime Data { get; set; }

			/// <summary>
			/// Sinal 1 do equipamento por data
			/// </summary>
			public double Sinal1 { get; set; }

			/// <summary>
			/// Sinal 2 do equipamento por data
			/// </summary>
			public double Sinal2 { get; set; }
		}
	}
}
