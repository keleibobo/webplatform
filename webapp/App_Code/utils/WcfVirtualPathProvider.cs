using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Caching;
using System.Collections;

namespace AppCode
{
    public class WcfVirtualPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            var appRelativeVirtualPath = ToAppRelativeVirtualPath(virtualPath);

            if (IsVirtualFile(appRelativeVirtualPath))
            {
                return true;
            }
            else
            {
                return Previous.FileExists(virtualPath);
            }
        }

        public override System.Web.Hosting.VirtualFile GetFile(string virtualPath)
        {
            var appRelativeVirtualPath = ToAppRelativeVirtualPath(virtualPath);
            if (IsVirtualFile(appRelativeVirtualPath))
            {
                var servicePath = VirtualPathUtility.MakeRelative(Constants.VirtualWcfDirectoryName + "/", virtualPath);
                if (servicePath.EndsWith(".svc"))
                    servicePath = servicePath.Substring(0, servicePath.IndexOf(".svc"));
                // check
                if (!servicePath.Contains("."))
                    return Previous.GetFile(virtualPath);

                var assemblyLocation = System.IO.Path.Combine(Constants.AbsolutePath, servicePath.Split('.')[0] + ".dll");
                if (!System.IO.File.Exists(assemblyLocation))
                    return Previous.GetFile(virtualPath);

                return new WcfVirtualFile(virtualPath, servicePath, typeof(WcfVirtualServiceHostFactory).FullName);
            }
            else
            {
                return Previous.GetFile(virtualPath);
            }

        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            var appRelativeVirtualPath = ToAppRelativeVirtualPath(virtualPath);

            if (IsVirtualFile(appRelativeVirtualPath) || IsVirtualDirectory(appRelativeVirtualPath))
            {
                return null;
            }
            else
            {
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
        }


        private bool IsVirtualFile(string appRelativeVirtualPath)
        {
            if (appRelativeVirtualPath.StartsWith(Constants.VirtualWcfDirectoryName + "/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private bool IsVirtualDirectory(string appRelativeVirtualPath)
        {
            return appRelativeVirtualPath.Equals(Constants.VirtualWcfDirectoryName, StringComparison.OrdinalIgnoreCase);
        }

        private string ToAppRelativeVirtualPath(string virtualPath)
        {
            var appRelativeVirtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            if (!appRelativeVirtualPath.StartsWith("~/"))
            {
                throw new HttpException("Unexpectedly does not start with ~.");
            }
            return appRelativeVirtualPath;
        }
    }

    public class Constants
    {
        public static readonly string VirtualWcfDirectoryName = "~/WcfLibs";
        public static readonly string AbsolutePath = HttpContext.Current.Server.MapPath("~/WcfLibs");
    }
}