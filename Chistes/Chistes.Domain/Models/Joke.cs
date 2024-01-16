namespace Chistes.Domain.Models
{
	public class Joke
	{
		public int Id { get; set; }
		public string Text { get; set; }


		public Joke()
		{
			Text = string.Empty;
		}
	}
}
