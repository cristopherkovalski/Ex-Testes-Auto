namespace EncurtadorDeUrl.Services
{
     
        public interface IEncurtadorUrlService
        {
            Task<(string id, string URL, int expiracao)> EncurtaURL(string originalURL);
            Task<string> GetOriginalURL(string shortURL);
        }

            public class EncurtadorUrlService : IEncurtadorUrlService
            {
                    private static readonly Dictionary<string, (string url, DateTime expiration)> urlStorage = new Dictionary<string, (string, DateTime)>();
                    private static readonly string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    private static readonly Random random = new Random();
                    private const int tamanhoId = 7;
                    private const int tempoExpiracaoDefault = 20;

            public async Task<(string id, string URL, int expiracao)> EncurtaURL(string originalURL)
            {
                string id = GerarId();
                string URL = $"http://shortURLME.com/{id}";
                DateTime expiration = DateTime.UtcNow.AddSeconds(tempoExpiracaoDefault);
                urlStorage[id] = (originalURL, expiration);
                await Task.Delay(0); 
                return (id, URL, tempoExpiracaoDefault);
            }

            public async Task<string> GetOriginalURL(string shortURL)
            {
                await Task.Delay(0); 
                string id = shortURL.Split('/').Last();
                if (!urlStorage.ContainsKey(id))
                {
                    throw new ArgumentException("URL curta inválida");
                }

                if (DateTime.UtcNow > urlStorage[id].expiration)
                {
                    urlStorage.Remove(id);
                    throw new ArgumentException("URL curta expirada");
                }

                return urlStorage[id].url;
            }

            private string GerarId()
            {
                char[] id = new char[tamanhoId];
                for (int i = 0; i < tamanhoId; i++)
                {
                    id[i] = caracteresPermitidos[random.Next(caracteresPermitidos.Length)];
                }
                return new string(id);
            }



    }
}

