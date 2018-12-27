using System.ComponentModel.DataAnnotations.Schema;

namespace Twitter.Models
{
    [Table("Following")]
    public class Following
    {
        public Following()
        {
        }
		       
        public string FollowingId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }

    }
}