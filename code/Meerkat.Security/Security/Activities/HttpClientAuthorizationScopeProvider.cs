using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Meerkat.Logging;

namespace Meerkat.Security.Activities
{
    /// <summary>
    /// Uses <see cref="HttpClient"/> to retrieve an <see cref="AuthorizationScope"/>.
    /// <para>
    /// This presumes you have set up an appropriate HttpClientHandler to handle issues such as attaching security tokens to the request.
    /// </para>
    /// <para>
    /// Also consider using Polly to provide retry and/or circuit breaker capabilities
    /// </para>
    /// </summary>
    public class HttpClientAuthorizationScopeProvider : IAuthorizationScopeProvider
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly HttpClient httpClient;
        private readonly string serviceUri;

        /// <summary>
        /// Creates a new instance of the <see cref="HttpClientAuthorizationScopeProvider"/> class.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="serviceUri"></param>
        public HttpClientAuthorizationScopeProvider(HttpClient httpClient, string serviceUri)
        {
            this.httpClient = httpClient;
            this.serviceUri = serviceUri;
        }

        /// <copydoc cref="IAuthorizationScopeProvider.AuthorizationScopeAsync" />
        public async Task<AuthorizationScope> AuthorizationScopeAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var request = new HttpRequestMessage(HttpMethod.Get, serviceUri);

            try
            {
                var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    Logger.ErrorFormat("Http Error ({0}): {1}", serviceUri, response.StatusCode);
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return result.ToAuthorizationScope();
            }
            catch (OperationCanceledException)
            {
                // Presume this is intentional so just log at info level
                Logger.Info("Cancelled: " + serviceUri);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Failed", ex);
            }

            // No valid data so let the caller decide what to do now.
            return null;
        }
    }
}