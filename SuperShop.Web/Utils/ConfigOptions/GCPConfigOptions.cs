using System.Collections.Generic;

namespace SuperShop.Web.Utils.ConfigOptions;

public class GCPConfigOptions
{
    internal readonly string GCPStorageAuthFile_Jorge;
    internal readonly string GCPStorageAuthFile_Nuno;
    internal readonly string GCPStorageBucketName_Jorge;
    internal readonly string GCPStorageBucketName_Nuno;


    internal readonly IEnumerable<string> Scopes;
}