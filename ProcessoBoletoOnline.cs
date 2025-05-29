using BoletoOnlineAPI.BoletoOnline.BancoBrasil.Cobranca.ConsulteBoletoResponse;
using RestSharp;
using System;
using System.Threading.Tasks;
using static BoletoOnlineAPI.BoletoOnline.BancoBrasil.Cobranca.ConsulteBoletoResponse.ConsulteBoletoResponse;

namespace BoletoOnlineAPI.BoletoOnline.BancoBrasil
{
    public class ProcessoBoletoOnline
    {
        private const string _developerApplicationKey = "c2bf835ff33e4e4697d2566ebb454f86";

        public async Task ConsulteBoletos() =>
            await Consulte<ListaBoletosResponse>();

        public async Task ConsulteBoletos(string idNumeroTituloCliente, string numeroConvenio) =>
            await Consulte<DetalhaBoletoBancarioResponse>(idNumeroTituloCliente, numeroConvenio);

        // public async Task RegistreBoleto(RequisicaoRegistroBoletos boleto) =>
        //   await Registre<RegistraBoletoCobrancaReponse>(boleto);
         
        public async Task<RegistraBoletoCobrancaReponse> RegistreBoleto(RequisicaoRegistroBoletos boleto) =>
         await Registre<RegistraBoletoCobrancaReponse>(boleto);





        public async Task AltereBoleto(AlteracaoBoleto boleto, string idTituloCliente) =>
            await Altere<ConsulteBoletoResponse>(boleto, idTituloCliente);

        public async Task BaixeBoleto(string idNumeroTituloCliente, string numeroConvenio) =>
            await BaixeBoleto<BaixaBoletoResponse>(idNumeroTituloCliente, numeroConvenio);

        public async Task GerePixBoleto(string idNumeroTituloCliente, string numeroConvenio) =>
           await GerePixBoleto<GerarPixBoletoResponse>(idNumeroTituloCliente, numeroConvenio);

        public async Task ConsultePixBoleto(string idNumeroTituloCliente, string numeroConvenio) =>
          await ConsultePixBoleto<ConsultaPixResponse>(idNumeroTituloCliente, numeroConvenio);

        public async Task CancelePix(string idNumeroTituloCliente, string numeroConvenio) =>
          await CancelePix<GerarPixBoletoResponse>(idNumeroTituloCliente, numeroConvenio);

        private async Task GerePixBoleto<T>(string idNumeroTituloCliente, string numero)
        {
            try
            {
                var restRequest = ObtenhaRequest("/boletos/{id}/gerar-pix", Method.Post)
                                 .AddUrlSegment("id", idNumeroTituloCliente)
                                 .AddJsonBody(new { numeroConvenio = numero });

                await ExecuteResquisicao<T>(restRequest);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        private async Task Altere<T>(AlteracaoBoleto boleto, string idTituloCliente)
        {
            try
            {
                var restRequest = ObtenhaRequest("/boletos/{id}", Method.Patch)
                                 .AddUrlSegment("id", idTituloCliente)
                                 .AddJsonBody(boleto);

                await ExecuteResquisicao<T>(restRequest);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        private async Task BaixeBoleto<T>(string idNumeroTituloCliente, string convenio)
        {
            try
            {
               var restRequest = ObtenhaRequest("boletos/{id}/baixar", Method.Post)
                                .AddUrlSegment("id", idNumeroTituloCliente)
                                .AddJsonBody(new { numeroConvenio = convenio });

                await ExecuteResquisicao<T>(restRequest);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        private async Task CancelePix<T>(string idNumeroTituloCliente, string convenio)
        {
            try
            {
                var restRequest = ObtenhaRequest("/boletos/{id}/cancelar-pix", Method.Post)
                                 .AddUrlSegment("id", idNumeroTituloCliente)
                                 .AddJsonBody(new { numeroConvenio = convenio });

                await ExecuteResquisicao<T>(restRequest);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        private async Task<T> Registre<T>(RequisicaoRegistroBoletos boleto)
        {
            try
            {
                var restRequest = ObtenhaRequest("boletos", Method.Post)
                                 .AddBody(boleto);

                return await ExecuteResquisicao<T>(restRequest);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        private async Task Consulte<T>(string idNumeroTituloCliente, string numeroConvenio)
        {
            try
            {
                var restRequest = ObtenhaRequest("boletos/{id}", Method.Get)
                                 .AddQueryParameter("numeroConvenio", numeroConvenio)
                                 .AddUrlSegment("id", idNumeroTituloCliente);

               await ExecuteResquisicao<T>(restRequest);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        private async Task ConsultePixBoleto<T>(string idNumeroTituloCliente, string numeroConvenio)
        {
            try
            {
                // Converter o número do convênio para int
                if (!int.TryParse(numeroConvenio, out int convenioInt))
                    throw new ArgumentException("O número do convênio deve ser um valor numérico válido.");


                var restRequest = ObtenhaRequest("/boletos/{id}/pix", Method.Get)
                                 .AddQueryParameter("numeroConvenio", convenioInt)
                                 .AddUrlSegment("id", idNumeroTituloCliente);

                await ExecuteResquisicao<T>(restRequest);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        private async Task Consulte<T>()
        {
            try
            {
                RestRequest request = ObtenhaRequest("boletos", Method.Get)
                                     .AddQueryParameter("indicadorSituacao", "A")
                                     .AddQueryParameter("agenciaBeneficiario", "452")
                                     .AddQueryParameter("contaBeneficiario", "123873");

                await ExecuteResquisicao<T>(request);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }

        public async Task ConsultePixAuthBoleto(string txId)
        {
            RestRequest request = ObtenhaRequest("/cob/{txid}", Method.Get)
                                 .AddUrlSegment("txid", txId);

            var restultado = await ExecuteResquisicao<DTOREsponsePixAuth>(request);
        }

        private RestRequest ObtenhaRequest(string requisicao, Method metodo) =>
            new RestRequest(requisicao, metodo)
                .AddQueryParameter("gw-dev-app-key", _developerApplicationKey);

        private async Task<T> ExecuteResquisicao<T>(RestRequest restRequest)
        {
            try
            {
                // Replace 'using declaration' with explicit 'using statement'  
                using (var cliente = new BancoBrasilClient())
                {
                    return await cliente.Execute<T>(restRequest);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                throw new ApplicationException("Erro ao consultar!", ex);
            }
        }
    }
}
