using System.Globalization;
using ChatDomain.Models;
using CsvHelper;

namespace ChatInfrastruncture.Service;

public class StockService
{
    private static HttpClient _httpClient;
    public StockService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Message> GetStockData(string stockCode)
    {
        try
        {
            Stock? stock = null;
            Message msg = new Message() { message = "Stock not Found", createdAt = DateTime.Now };

            using var resp = await _httpClient.GetAsync($"https://stooq.com/q/l/?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            using var reader = new StreamReader(await resp.Content.ReadAsStreamAsync());
            {
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                stock = csv.GetRecords<Stock>().FirstOrDefault();
            }
            if (stock != null && stock?.Open!="N/D")
                msg.message =
                    $"{stock?.Symbol} quote is ${stock?.Open} per share";
            return  msg;
        }
        catch (Exception ex)
        {
            throw new Exception("Request error", ex);
        }
    }
}