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
        public async Task EncurtaURL_Deve_Retornar_URL_Curta_ValidaAsync()
        {
            // Arrange
            string originalURL = "http://example.com/";

            // Act
            var (id, URL, expiracao) = await _urlShortenerService.EncurtaURL(originalURL);

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
        public async Task GetOriginalURL_Deve_Retornar_URL_Original_CorretaAsync(string originalURL)
        {
            // Arrange
            var (id, shortURL, _) = await _urlShortenerService.EncurtaURL(originalURL);

            // Act
            string returnedURL = await _urlShortenerService.GetOriginalURL(shortURL);

            // Assert
            returnedURL.Should().Be(originalURL);
        }

        [Fact]
        public async Task GetOriginalURL_Deve_Lancar_Excecao_Quando_URL_Curta_Invalida()
        {
            // Arrange
            string shortURL = "http://shortURLME.com/invalid";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await _urlShortenerService.GetOriginalURL(shortURL));
            exception.Message.Should().Be("URL curta inválida");
        }


        [Fact]
        public async Task GetOriginalURL_Deve_Lancar_Excecao_Quando_URL_Curta_ExpiradaAsync()
        {
            // Arrange
            string originalURL = "http://example.com";
            var (id, shortURL, _) = await _urlShortenerService.EncurtaURL(originalURL);
            System.Threading.Thread.Sleep(20000);

            // Act
            Func<Task> act = async () => await _urlShortenerService.GetOriginalURL(shortURL);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("URL curta expirada");
        }
    }

}
    
