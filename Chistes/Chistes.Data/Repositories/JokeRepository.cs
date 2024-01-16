using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Chistes.Domain.Models;


namespace Chistes.Data.Repositories
{
	public class JokeRepository
	{
		private readonly HttpClient _httpClient;
		private readonly string _connectionString;

		public JokeRepository(string connectionString)
		{
			_httpClient = new HttpClient();
			_connectionString = connectionString;
		}

		public async Task<string> ReturnRandomJokeAsync()
		{
			try
			{
				// Generate a random number between 0 and 1
				Random random = new Random();
				int randomNumber = random.Next(2);

				// Use the random number to decide which method to call
				if (randomNumber == 0)
				{
					return await ReturnRandomDadJoke();
				}
				else
				{
					return await ReturnRandomChuckJoke();
				}
			}
			catch (Exception ex)
			{
				return "An error occurred while fetching a random joke.";
			}
		}

		public async Task<string> ReturnRandomChuckJoke()
		{
			try
			{
				var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random");
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				// Assuming the response JSON has a field named 'value' containing the joke
				dynamic joke = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
				return joke.value;
			}
			catch (Exception ex)
			{
				return "Error al obtener un chiste de Chuck";
			}
		}

		public async Task<string> ReturnRandomDadJoke()
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Get, "https://icanhazdadjoke.com/");
				request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var response = await _httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var content = await response.Content.ReadAsStringAsync();
				var joke = JsonConvert.DeserializeObject<JokeResponse>(content);
				return joke.JokeText;
			}
			catch (Exception ex)
			{
				return "Error al obtener un chiste de Dad";
			}
		}

		private class JokeResponse
		{
			[JsonProperty("joke")]
			public string? JokeText { get; set; }
		}

		public async Task SaveJoke(string joke)
		{
			try
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					string query = "INSERT INTO Joke (Text) VALUES (@joke)";
					using (var command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@joke", joke);
						await command.ExecuteNonQueryAsync();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task UpdateJoke(int id, string joke)
		{
			try
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					string query = "UPDATE Joke SET Text = @joke WHERE Id = @id";
					using (var command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@joke", joke);
						command.Parameters.AddWithValue("@id", id);

						await command.ExecuteNonQueryAsync();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task DeleteJoke(int id)
		{
			try
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					string query = "DELETE FROM Joke WHERE Id = @id";
					using (var command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@id", id);

						await command.ExecuteNonQueryAsync();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<List<Joke>> GetAllJokesAsync()
		{
			var jokes = new List<Joke>();

			try
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();

					string query = "SELECT Id, Text FROM Joke";
					using (var command = new SqlCommand(query, connection))
					{
						using (var reader = await command.ExecuteReaderAsync())
						{
							while (await reader.ReadAsync())
							{
								var joke = new Joke
								{
									Id = reader.GetInt32(reader.GetOrdinal("Id")),
									Text = reader.GetString(reader.GetOrdinal("Text"))
								};
								jokes.Add(joke);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			return jokes;
		}
	}
}
