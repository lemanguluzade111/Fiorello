using Fiorello.Models;
using System.Collections.Generic;

namespace Fiorello.ViewModels
{
    public class HomeVM
    {
        public List<Product>Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Slider> Sliders { get; set; }

        public SliderInfo SliderInfo { get; set; }
    }
}
