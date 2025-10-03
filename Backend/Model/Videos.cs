using System.ComponentModel.DataAnnotations;

namespace Backend.Model
{
    public class Videos
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoURL { get; set; }
        public int NumberOfLikes { get; set; }
    }
}

