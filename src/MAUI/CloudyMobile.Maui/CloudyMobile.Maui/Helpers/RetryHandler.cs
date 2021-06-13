﻿using CloudyMobile.Maui.Services.Abstractions;
using CloudyMobile.Maui.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CloudyMobile.Maui.Helpers
{
    public class RetryHandler : DelegatingHandler
    {
        private IAuthService authService;

        public RetryHandler()
        {
            authService = ViewModelResolver.Resolve<IAuthService>();
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            int i = 1;

            while (true)
            {
                try
                {
                    var response = await base.SendAsync(request, cancellationToken);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        if (await HandleUnauthorized())
                        {
                            continue;
                        }
                    }

                    if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        if (i < 4)
                        {
                            await Task.Delay(1000, cancellationToken);
                            i++;
                            continue;
                        }
                    }

                    return response;
                }
                catch (Exception ex) when (IsNetworkError(ex))
                {
                    // Network error
                    // Wait a bit and try again later
                    await Task.Delay(2000, cancellationToken);
                    continue;
                }
            }
        }

        private static bool IsNetworkError(Exception ex)
        {
            // Check if it's a network error
            if (ex is SocketException)
                return true;
            if (ex.InnerException != null)
                return IsNetworkError(ex.InnerException);
            return false;
        }

        public async Task<bool> HandleUnauthorized()
        {
            return await authService.Authenticate();
        }
    }
}
