using MauiApp1.Entities;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDataTemplateSelector
{
    class ImageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImagePathTemplate { get; set; }

        public DataTemplate ImageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var product = (Product)item;

            return product.TitlePath == null ? ImageTemplate : ImagePathTemplate;
        }
    }
}
