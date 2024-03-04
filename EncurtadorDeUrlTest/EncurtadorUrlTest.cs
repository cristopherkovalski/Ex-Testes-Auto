using EncurtadorDeUrl.Services;
using FluentAssertions;

namespace EncurtadorDeUrlTest
{
    public class EncurtadorUrlTest
    {

        private readonly EncurtadorUrlService _urlShortenerService;

        public EncurtadorUrlTest()
        {
            _urlShortenerService = new EncurtadorUrlService();
        }

        [Fact]
        public void EncurtaURL_Deve_Retornar_URL_Curta_Valida()
        {
            // Arrange
            string originalURL = "http://example.com/";

            // Act
            var (id, URL, expiracao) = _urlShortenerService.EncurtaURL(originalURL);

            // Assert
            URL.Should().NotBeNullOrEmpty();
            URL.Should().StartWith("http://shortURLME.com/");
            id.Should().NotBeNullOrEmpty();
            expiracao.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("http://example1.com/adobe/acrobat/aWedwewAWdedf")]
        [InlineData("http://example2.com/pesquisar/cadastros")]
        [InlineData("http://example3.com")]
        public void GetOriginalURL_Deve_Retornar_URL_Original_Correta(string originalURL)
        {
            // Arrange
            var (id, shortURL, _) = _urlShortenerService.EncurtaURL(originalURL);

            // Act
            string returnedURL = _urlShortenerService.GetOriginalURL(shortURL);

            // Assert
            returnedURL.Should().Be(originalURL);
        }

        [Fact]
        public void GetOriginalURL_Deve_Lancar_Excecao_Quando_URL_Curta_Invalida()
        {
            // Arrange
            string shortURL = "http://shortURLME.com/invalid";

            // Act
            Action act = () => _urlShortenerService.GetOriginalURL(shortURL);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("URL curta inválida");
        }

        [Fact]
        public void GetOriginalURL_Deve_Lancar_Excecao_Quando_URL_Curta_Expirada()
        {
            // Arrange
            string originalURL = "http://example.com";
            var (id, shortURL, _) = _urlShortenerService.EncurtaURL(originalURL);
            System.Threading.Thread.Sleep(20000); 

            // Act
            Action act = () => _urlShortenerService.GetOriginalURL(shortURL);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("URL curta expirada");
        }
    }

}
    
