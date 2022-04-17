using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
	public class Base
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
	}
}
