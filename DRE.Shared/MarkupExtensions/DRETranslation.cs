using DRE.Libs.Lng;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Text;

namespace DRE.MarkupExtensions
{
    [MarkupExtensionReturnType(ReturnType = typeof(String))]
    public class DRETranslation : MarkupExtension
    {
        public String _ { get; set; }

        private String T { get; set; }

        private LibLng Lng { get; set; }

        public DRETranslation() { 
        
            if (Lng == null) Lng = new LibLng();
        }
 
        //public DRETranslation(String textCodeToTranslate) => _ = textCodeToTranslate;
 
        protected override object ProvideValue(IXamlServiceProvider serviceProvider)
        {
            try
            {
                T = Lng._(_);
            }
            catch (Exception) { T = _; }

            return T;
        }

        public override string ToString() => Lng._(_);
       

    }
}
