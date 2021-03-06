﻿namespace Serilog.Sinks.RabbitMQ {
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if NET_FX
    using System.Configuration;
#endif
#if NETSTANDARD2_0
    using Microsoft.Extensions.Configuration;
#endif
    using Serilog.Debugging;

    internal static class ApplySystemConfiguration {
        internal static List<string> ParseHostName(string hostname) {
            if (hostname == null) throw new ArgumentNullException(nameof(hostname));

            return hostname.Split(',').Select(m => m.Trim()).ToList();
        }

        /// <summary>
        /// Examine if supplied uri is a reference to an item in the "ConnectionStrings" section of web.config
        /// If it is, return the uri item, if not, return string as supplied.
        /// </summary>
        /// <param name="nameOrUri">The name of the ConnectionStrings key or raw uri.</param>
#if NET_FX
        internal static string GetUri(string nameOrUri) {
            // If there is an `://`, we assume this is a raw uri
            // If there are no `://`, attempt to pull the named value from config
            if (nameOrUri != null && nameOrUri.IndexOf("://") < 0) {
                var cs = ConfigurationManager.ConnectionStrings[nameOrUri];
                if (cs != null) {
                    return cs.ConnectionString;
                }
                else {
                    SelfLog.WriteLine("RabbitMQ sink configured value {0} is not found in ConnectionStrings settings and does not appear to be a raw connection string.", nameOrUri);
                }
            }

            return nameOrUri;
        }
#endif
#if NETSTANDARD2_0
        /// <summary>
        /// Examine if supplied uri is a reference to an item in the "ConnectionStrings" section of web.config
        /// If it is, return the uri item, if not, return string as supplied.
        /// </summary>
        /// <param name="nameOrUri">The name of the ConnectionStrings key or raw uri.</param>
        /// <param name="appConfiguration">Additional application-level configuration.</param>
        internal static string GetUri(string nameOrUri, IConfiguration appConfiguration) {
            // If there is an `://`, we assume this is a raw uri
            // If there are no `://`, attempt to pull the named value from config
            if (nameOrUri != null && nameOrUri.IndexOf("://") < 0) {
                string cs = appConfiguration?.GetConnectionString(nameOrUri);
                if (!string.IsNullOrEmpty(cs)) {
                    return cs;
                }
                else {
                    SelfLog.WriteLine("RabbitMQ sink configured value {0} is not found in ConnectionStrings settings and does not appear to be a raw connection string.", nameOrUri);
                }
            }

            return nameOrUri;
        }
#endif
    }
}
