using Microsoft.AspNetCore.Http;

namespace ManageRates.AspnetCore
{
    /// <summary>
    /// Predefined identication key extractor. You can provide you own key extraction with <see cref="KeyExtractorDelegate"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="KeyType"/> cause extract method for key. This key will be used to communicate with storage.
    /// Therefore requests with different keys will be treat as different restrictions.
    /// </remarks>
    public enum KeyType
    {
        /// <summary>
        /// No information from <see cref="HttpRequest"/> is used for rate isntances.
        /// All queries are processes as one query.
        /// </summary>
        None,

        /// <summary>
        /// For differend values of <see cref="HttpRequest.Path"/> will be applied different storages.
        /// Therefore restrictions will be applied for eash differen path separately.
        /// </summary>
        RequestPath,

        /// <summary>
        /// For differend values of <see cref="HttpContext.User"/> will be applied different storages.
        /// Therefore restrictions will be applied for eash differen user separately.
        /// </summary>
        User,

        /// <summary>
        /// For differend values of <see cref="ConnectionInfo.RemoteIpAddress"/> will be applied different storages.
        /// Therefore restrictions will be applied for eash differen IP separately.
        /// </summary>
        Ip
    }
}
