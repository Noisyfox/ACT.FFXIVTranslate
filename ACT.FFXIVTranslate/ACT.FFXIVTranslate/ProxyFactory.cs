using System;
using System.Net;
using System.Net.Http;
using Extreme.Net;

namespace ACT.FFXIVTranslate
{
    class ProxyFactory : PluginComponent
    {
        public const string TypeNone = "none";
        public const string TypeHttp = "http";
        public const string TypeSocks5 = "socks5";

        public static ProxyFactory Instance { get; } = new ProxyFactory();

        private ProxyFactory()
        {

        }

        private ProxySettings _currentSettings;

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            plugin.Controller.ProxyChanged += ControllerOnProxyChanged;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
        }

        private void ControllerOnProxyChanged(bool fromView, string type, string server, int port, string user,
            string password, string domain)
        {
            if (!fromView)
            {
                return;
            }

            _currentSettings = new ProxySettings
            {
                Type = type,
                Server = server,
                Port = port,
                User = user,
                Password = password,
                Domain = domain
            };
        }

        private class ProxySettings
        {
            public string Type { get; set; }
            public string Server { get; set; }
            public int Port { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string Domain { get; set; }
        }

        public HttpClient NewClient()
        {
            var settings = _currentSettings;
            if (settings == null || settings.Type == TypeNone)
            {
                return new HttpClient(new HttpClientHandler
                {
                    UseProxy = false
                });
            }
            switch (settings.Type)
            {
                case TypeHttp:
                {
                    var proxyCreds = new NetworkCredential(settings.User, settings.Password, settings.Domain);
                    var proxy = new WebProxy(new Uri($"http://{settings.Server}:{settings.Port}"), true)
                    {
                        UseDefaultCredentials = false,
                        Credentials = proxyCreds
                    };
                    var handler = new HttpClientHandler
                    {
                        Proxy = proxy,
                        UseProxy = true,
                        PreAuthenticate = true,
                        UseDefaultCredentials = false
                    };
                    return new HttpClient(handler);
                }
                case TypeSocks5:
                {
                    var proxy = new Socks5ProxyClient(settings.Server, settings.Port, settings.User, settings.Password);
                    var handler = new ProxyHandler(proxy);
                    return new HttpClient(handler);
                }
                default:
                    throw new Exception($"Proxy Error: Unknown proxy type: {settings.Type}");
            }
        }
    }
}
