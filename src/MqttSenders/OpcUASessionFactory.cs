using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System.Linq;

namespace MqttSenders
{
    public class OpcUASessionFactory
    {
        private Uri opcServerUri;

        public OpcUASessionFactory(string opcServerAddress)
        {
            opcServerUri = new Uri(opcServerAddress);
        }

        /// <summary>
        /// Erzeugt eine Session mit dem Opc Server
        /// </summary>
        public async Task<Session> CreateAsync(CancellationToken token)
        {
            var telemetry = DefaultTelemetry.Create(_ => { });

            // Konfiguration der OPC-UA-Anwendungsinstant
            var application = new ApplicationInstance(telemetry)
            {
                ApplicationName = "Client", // Anwendungsname
                ApplicationType = ApplicationType.Client, // Anwendungstyp
                ConfigSectionName = "Quickstarts.ReferenceClient", // Konfigurationsbereichsname
            };

            var configFilePath = Path.Combine(
                AppContext.BaseDirectory,
                "Quickstarts.ReferenceClient.Config.xml"
            );

            var applicationConfiguration = await application.LoadApplicationConfigurationAsync(
                configFilePath,
                false,
                token
            );

            await application.CheckApplicationInstanceCertificatesAsync(true, ct: token);

            // Endpoint vom Server abfragen, damit UserTokenPolicies korrekt gesetzt sind.
            var endpointDescription = await CoreClientUtils.SelectEndpointAsync(
                applicationConfiguration,
                opcServerUri.ToString(),
                useSecurity: false,
                discoverTimeout: 15000,
                telemetry,
                ct: token
            );

            var anonymousTokenPolicy = endpointDescription
                .UserIdentityTokens?
                .FirstOrDefault(tokenPolicy => tokenPolicy.TokenType == UserTokenType.Anonymous);

            if (anonymousTokenPolicy == null)
            {
                throw new ServiceResultException(
                    StatusCodes.BadIdentityTokenInvalid,
                    $"No anonymous user token policy found for endpoint '{endpointDescription.EndpointUrl}'."
                );
            }

            var configuredEndpoint = new ConfiguredEndpoint(
                null,
                endpointDescription,
                EndpointConfiguration.Create(applicationConfiguration)
            );

            ISessionFactory sessionFactory = new DefaultSessionFactory(telemetry);

            var session = (Session)
                await sessionFactory.CreateAsync(
                    configuration: applicationConfiguration,
                    endpoint: configuredEndpoint,
                    updateBeforeConnect: true,
                    sessionName: "session1",
                    sessionTimeout: 60000,
                    identity: new UserIdentity(
                        new AnonymousIdentityToken { PolicyId = anonymousTokenPolicy.PolicyId }
                    ),
                    preferredLocales: null,
                    ct: token
                );

            return session;
        }
    }
}
