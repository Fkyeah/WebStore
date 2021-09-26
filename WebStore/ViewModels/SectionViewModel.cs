using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class SectionViewModel
    {
        public SectionViewModel()
        {
            Childs = new List<SectionViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public SectionViewModel Parent { get; set; }
        public List<SectionViewModel> Childs { get; set; }
    }
}
