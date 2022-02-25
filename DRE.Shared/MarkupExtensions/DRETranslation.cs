using Dapper;
using DRE.Libs.Lng;
using Microsoft.UI.Xaml.Data;
using System;
using System.Linq;

namespace DRE.MarkupExtensions
{
    public class DRETranslation : IValueConverter
    {
        public String _ { get; set; }

        private LibLng Lng { get; set; }

        private String dir = $"{AppDomain.CurrentDomain.BaseDirectory}db/locale";

        private String L { get; set; }

        public DRETranslation()
        {
            //if (SetupSvc == null) SetupSvc = new LibSetup();

            if (Lng == null) Lng = new LibLng();

        }


        // public override string ToString() => 
        //    Lng._(_, DRE.Libs.Setup.LibSetup.db.Query<String>("SELECT v FROM DRE WHERE n='defaultLanguage'").AsList().First());

        public object Convert(object value, Type targetType, object p, string language)
        {

            if (L == null) L = DRE.Libs.Setup.LibSetup.db.Query<String>("SELECT v FROM DRE WHERE n='defaultLanguage'").AsList().First();

            return Lng._(value.ToString(), L.ToUpper());

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
