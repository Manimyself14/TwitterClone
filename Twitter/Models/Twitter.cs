using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
	[Table("Twitter")]
	public class Twitter
	{
		public int TweetId { get; set; }
		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public string Message { get; set; }
		public DateTime Created { get; set; }
		
	}
}