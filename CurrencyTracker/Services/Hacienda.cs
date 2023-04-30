namespace CurrencyTracker.Services
{
    public class Hacienda
    {

        public static (decimal, decimal) ApiHacienda()
        {
            HttpClient client = new HttpClient();
            string apiUrl = "https://api.hacienda.go.cr/indicadores/tc/dolar?fbclid=IwAR0zWDoToLXrNWsH0d7kG184c-hGT9FwpgWwzI0F22bpJbKzLSJ7KyqRu0c";
            var response = client.GetAsync(apiUrl).Result;
            var jsonData = response.Content.ReadAsStringAsync().Result;

            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData);
            var exchangeRateVenta = (decimal)jsonObject.venta.valor;
            var exchangeRateCompra = (decimal)jsonObject.compra.valor;

            return (exchangeRateVenta, exchangeRateCompra);
        }



    }
}
