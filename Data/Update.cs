using System.ComponentModel.DataAnnotations;

namespace GodSimulator.Data
{
    public class Update
    {
        [Key]
        public int Id { get; set; }
        public string Version { get; set; } = "";
        public string Description { get; set; } = "";

        public int SocietyOpinion { get; set; }

        public Update()
        {
            SocietyOpinion = Random.Shared.Next(1, 6);
        }
    }
}
