using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public SectionsViewComponent(IProductData productData)
        {
            _productData = productData;
        }
        public IViewComponentResult Invoke()
        {
            var sections = _productData.GetSections();
            var parentSections = sections.Where(el => el.ParentId is null);
            var parentSectionsViews = parentSections
                .Select(el => new SectionViewModel
                {
                    Id = el.Id,
                    Name = el.Name,
                    Order = el.Order,
                })
                .ToList();

            foreach(var parent in parentSectionsViews)
            {
                var childs = sections.Where(q => q.ParentId == parent.Id);
                foreach (var child in childs)
                {
                    parent.Childs.Add(new SectionViewModel
                    {
                        Id = child.Id,
                        Name = child.Name,
                        Order = child.Order,
                        Parent = parent,
                    }); ;
                }
                parent.Childs.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            }
            parentSectionsViews.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));
            return View(parentSectionsViews);
        }
    }
}
