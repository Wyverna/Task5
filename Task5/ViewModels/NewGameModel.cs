using System.ComponentModel.DataAnnotations;

namespace Task4.ViewModels
{
    public class NewGameModel
    {
        [Required(ErrorMessage = "Game name is required")]
        public string Name { get; set; }

        public string Tags { get; set; }
    }
}
