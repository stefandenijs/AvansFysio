using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Space
{
    public class Room
    {
        public int Id { get; set; }
        [Required] 
        public int RoomNumber { get; set; }
        [Required]
        public string RoomType { get; set; }
    }
}
