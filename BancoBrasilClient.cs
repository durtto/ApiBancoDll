using RestSharp;
using RestSharp.Serializers.Json;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static BoletoOnlineAPI.BoletoOnline.BancoBrasil.Cobranca.ConsulteBoletoResponse.ConsulteBoletoResponse;
using static BoletoOnlineAPI.BoletoOnline.BancoBrasil.ErrorResponse;

namespace BoletoOnlineAPI.BoletoOnline.BancoBrasil
{
    public class BancoBrasilClient : ClientAbstrato
    {
        protected override RestClient ObtenhaClient()
        {
            // Implement the method to return a RestClient instance.  
            // This is required to fulfill the abstract member from ClientAbstrato.  
            return new RestClient();
        }

        protected override async Task<T> ExecuteAsync<T>(RestRequest request)
        {
            RestResponse response = await _client.ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<T>(response.Content);

                if (result == null)
                {
                    throw new ApplicationException("Failed to deserialize response content.");
                }

                return result;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ApplicationException($"Recurso não encontrado: {request.Resource}");
            }

            if (!string.IsNullOrEmpty(response.Content))
            {
                ListaErroRequisicaoInvalida requisicaoInvalida = JsonSerializer.Deserialize<ListaErroRequisicaoInvalida>(response.Content);

                if (requisicaoInvalida.erros?.Count > 0)
                {
                    throw new ErrorExceptionRequisicaoInvalida(requisicaoInvalida.erros);
                }

                ErrorNaoAutorizado erroNaoAutorizado = JsonSerializer.Deserialize<ErrorNaoAutorizado>(response.Content);

                if (!string.IsNullOrEmpty(erroNaoAutorizado?.error))
                {
                    throw new ErrorExceptionNaoAutorizado(erroNaoAutorizado);
                }

                ErrosInconsistencia errosInconsistencia = JsonSerializer.Deserialize<ErrosInconsistencia>(response.Content);

                if (errosInconsistencia.errors?.Count > 0)
                {
                    throw new ApplicationException(errosInconsistencia.errors.First()?.message);
                }
            }

            throw new ApplicationException($"Erro ao acessar o servico: {response.StatusCode}");
        }
    }
}
