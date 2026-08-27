using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace BffAuth0.Server;

public static class OidcEventHandlers
{
    public static OpenIdConnectEvents OidcEvents(IConfiguration configuration)
    {
        return new OpenIdConnectEvents
        {
            OnAuthorizationCodeReceived = context => OnAuthorizationCodeReceivedHandler(context, configuration),

            // use OAuth PAR
            OnPushAuthorization = context => OnPushAuthorizationHandler(context, configuration),

            OnRedirectToIdentityProviderForSignOut = context => OnRedirectToIdentityProviderForSignOutHandler(context, configuration),

            // standard OIDC flow handlers using JAR and client assertions - not using OAuth PAR
            //OnRedirectToIdentityProvider = context => OnRedirectToIdentityProviderHandler(context, configuration),
        };
    }

    private static Task OnRedirectToIdentityProviderForSignOutHandler(RedirectContext context, IConfiguration configuration)
    {
        var logoutUri = $"https://{configuration.GetValue<string>("Auth0:Domain")}/v2/logout?client_id={configuration.GetValue<string>("Auth0:ClientId")}";

        var postLogoutUri = context.Properties.RedirectUri;
        if (!string.IsNullOrEmpty(postLogoutUri))
        {
            if (postLogoutUri.StartsWith("/"))
            {
                // transform to absolute
                var request = context.Request;
                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
            }
            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
        }

        context.Response.Redirect(logoutUri);
        context.HandleResponse();
        return Task.CompletedTask;
    }

    private static Task OnAuthorizationCodeReceivedHandler(AuthorizationCodeReceivedContext context, IConfiguration configuration)
    {
        // https://openid.net/specs/openid-connect-eap-acr-values-1_0-final.html
        if (context.Properties != null && context.Properties.Items.ContainsKey("acr_values"))
        {
            context.ProtocolMessage.AcrValues = context.Properties.Items["acr_values"];
        }

        if (context.TokenEndpointRequest != null)
        {
            context.TokenEndpointRequest.ClientAssertionType = OidcConstants.ClientAssertionTypes.JwtBearer;
            context.TokenEndpointRequest.ClientAssertion = AssertionService.CreateClientToken(configuration);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Not using OAuth PAR
    /// </summary>
    //private static Task OnRedirectToIdentityProviderHandler(RedirectContext context, IConfiguration configuration)
    //{
    //    var request = AssertionService.SignAuthorizationRequest(context.ProtocolMessage, configuration);
    //    var clientId = context.ProtocolMessage.ClientId;
    //    var redirectUri = context.ProtocolMessage.RedirectUri;
    //
    //    context.ProtocolMessage.Parameters.Clear();
    //    context.ProtocolMessage.ClientId = clientId;
    //    context.ProtocolMessage.RedirectUri = redirectUri;
    //    context.ProtocolMessage.SetParameter("request", request);
    //    return Task.CompletedTask;
    //}

    private static Task OnPushAuthorizationHandler(PushedAuthorizationContext context, IConfiguration configuration)
    {
        context.ProtocolMessage.Parameters.Add("client_assertion", AssertionService.CreateClientToken(configuration));
        context.ProtocolMessage.Parameters.Add("client_assertion_type", OidcConstants.ClientAssertionTypes.JwtBearer);

        context.ProtocolMessage.Parameters.Add("audience", configuration.GetValue<string>("Auth0:Audience"));

        context.HandleClientAuthentication();

        // https://openid.net/specs/openid-connect-eap-acr-values-1_0-final.html
        if (context.Properties.Items.ContainsKey("acr_values"))
        {
            context.ProtocolMessage.AcrValues = context.Properties.Items["acr_values"];
        }

        return Task.CompletedTask;
    }
}
