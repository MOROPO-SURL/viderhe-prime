namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Handlers
{
    public class SecureImageHttpClientHandler : System.Net.Http.HttpClientHandler
    {
        protected override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            foreach (var header in Managers.SessionManager.Instance.ExtraHeaders)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
