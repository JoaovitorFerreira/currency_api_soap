using GeoPluginDeserialize;
using System;
using System.Net;
using System.Web.Services;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace currency_api_soap
{
    /// <summary>
    /// Descrição resumida de CurrencyConverter
    /// </summary>
    [WebService(Namespace = "http://soapcurrencyconverter.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    // [System.Web.Script.Services.ScriptService]
    public class CurrencyConverter : System.Web.Services.WebService
    {

        [WebMethod]
        public double ConvertCurrency(string currency, int amount)
        {
            double multiplier = GetCurrency(currency);
            return amount * multiplier;
        }

        //baseado em BRL
        private double GetCurrency(string currency)
        {
            WebRequest req = WebRequest.Create("http://www.geoplugin.net/xml.gp?base_currency=" + currency);
            WebResponse response = req.GetResponse();
            var geoPluginSerializer = new XmlSerializer(typeof(GeoPlugin));
            using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                var geoPlugin = (GeoPlugin)geoPluginSerializer.Deserialize(sr);
                return geoPlugin.Geoplugin_currencyConverter;
            }
        }

        //usa brl como intermediario
        [WebMethod]
        public double GetConversor(int value, string currencyIn, string currencyOut)
        {
           double cashIn = GetCurrency(currencyIn);
           double cashOut = GetCurrency(currencyOut);
           return value * (cashIn / cashOut);
        }
    }
}
