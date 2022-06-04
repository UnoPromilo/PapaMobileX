using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Android.Net;

namespace PapaMobileX.App.Foundation.Concrete;

public partial class HttpClientHandlerBuilder
{
    public partial HttpClientHandler Build()
    {
        var handler = new AndroidClientHandler();
        handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidationCallback;
        return handler;
    }

    private static bool ServerCertificateCustomValidationCallback(HttpRequestMessage arg1, X509Certificate2? arg2, X509Chain? arg3, SslPolicyErrors arg4)
    {
        return true;
    }
}