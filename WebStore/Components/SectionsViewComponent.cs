using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

// Визуальный компонент нужен для подготовки данных для дальнейшей визуализации

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;
        public SectionsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }
        public IViewComponentResult Invoke()
        {
            var section = _ProductData.GetSection(); // Получаемм секции

            var parent_sections = section.Where(s => s.ParentId is null);
            var parent_section_views = parent_sections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,

                })
                .ToList();

            foreach(var parent_section in parent_section_views)
            {
                var childs = section.Where(s => s.ParentId == parent_section.Id);

                foreach(var child_section in childs)
                {
                    parent_section.ChildSection.Add(new SectionViewModel 
                    {
                        Id= child_section.Id,
                        Name= child_section.Name,
                        Order= child_section.Order,
                        Parent = parent_section,
                    });
                }

                parent_section.ChildSection.Sort((a,b) => Comparer<int>.Default.Compare(a.Order,b.Order));  
            }
            parent_section_views.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return View(parent_section_views);
        }
    }
}
