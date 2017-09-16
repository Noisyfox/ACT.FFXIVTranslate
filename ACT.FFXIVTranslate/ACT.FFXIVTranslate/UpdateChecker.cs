using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ACT.FFXIVTranslate
{
    internal class UpdateChecker : PluginComponent
    {
        public const string ReleasePage = "https://github.com/Noisyfox/ACT.FFXIVTranslate/releases";

        private readonly UpdateCheckerThread _workingThread = new UpdateCheckerThread();
        private MainController _controller;

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _controller = plugin.Controller;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
        }

        public void CheckUpdate(bool forceNotify)
        {
            _workingThread.StartWorkingThread(new UpdateContext
            {
                Controller = _controller,
                ForceNotify = forceNotify
            });
        }

        public void Stop()
        {
            _workingThread.StopWorkingThread();
        }


        internal class VersionInfo
        {
            public PublishVersion LatestStableVersion { get; set; }
            public PublishVersion LatestVersion { get; set; }
        }

        private class UpdateContext
        {
            public MainController Controller { get; set; }
            public bool ForceNotify { get; set; }
        }

        private class UpdateCheckerThread : BaseThreading<UpdateContext>
        {
            private const string UpdateURL = "https://api.github.com/repos/Noisyfox/ACT.FFXIVTranslate/releases";

            private const string UserAgent =
                    "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.3319.102 Safari/537.36"
                ;

            private const string NameMatcher =
                @"^ACT\.FFXIVTranslate(?:-|\.)(?<version>\d+(?:\.\d+)*)(?:|-Release)\.7z$";

            protected override void DoWork(UpdateContext context)
            {
                context.Controller.NotifyUpdateCheckingStarted(false);

                try
                {
                    string responseBody;
                    using (var client = ProxyFactory.Instance.NewClient())
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Get;
                        request.RequestUri = new Uri(UpdateURL);
                        request.Headers.Add("User-Agent", UserAgent);

                        var response = client.SendAsync(request).Result;
                        response.EnsureSuccessStatusCode();
                        responseBody = response.Content.ReadAsStringAsync().Result;
                    }
                    var result = JArray.Parse(responseBody);
                    var versions = new List<PublishVersion>();

                    foreach (var release in result.Cast<JObject>())
                    {
                        var isPrerelease = (bool) release["prerelease"];
                        var message = (string) release["body"];
                        var page = (string) release["html_url"];
                        foreach (var asset in ((JArray) release["assets"]).Cast<JObject>())
                        {
                            var name = (string) asset["name"];

                            // Parse name
                            var match = Regex.Match(name, NameMatcher);
                            if (match.Success)
                            {
                                var version = match.Groups["version"].Value;
                                if (Version.TryParse(version, out var pv))
                                {
                                    versions.Add(new PublishVersion
                                    {
                                        IsPreRelease = isPrerelease,
                                        RawVersion = version,
                                        ParsedVersion = pv,
                                        ReleaseMessage = message,
                                        PublishPage = page,
                                    });
                                }
                            }
                        }
                    }

                    // Sort
                    versions.Sort((l, r) =>
                    {
                        var c = r.ParsedVersion.CompareTo(l.ParsedVersion);
                        return c != 0 ? c : r.IsPreRelease.CompareTo(l.IsPreRelease);
                    });

                    var latest = versions.FirstOrDefault();
                    var latestStable = versions.FirstOrDefault(it => !it.IsPreRelease);
                    context.Controller.NotifyVersionChecked(false, new VersionInfo
                    {
                        LatestVersion = latest,
                        LatestStableVersion = latestStable
                    }, context.ForceNotify);
                }
                catch (Exception ex)
                {
                    context.Controller.NotifyVersionChecked(false, null, context.ForceNotify);
                    context.Controller.NotifyLogMessageAppend(false, ex + "\n");
                }
            }
        }

        internal class PublishVersion
        {
            public string RawVersion { get; set; }
            public Version ParsedVersion { get; set; }
            public bool IsPreRelease { get; set; }
            public string ReleaseMessage { get; set; }
            public string PublishPage { get; set; }
        }
    }
}
