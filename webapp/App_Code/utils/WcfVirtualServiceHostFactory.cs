using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace AppCode
{
    public class WcfVirtualServiceHostFactory : ServiceHostFactory
    {
        public override System.ServiceModel.ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            var assmblyName = constructorString.Split('.')[0] + ".dll";
            var serviceName = constructorString;
            assmblyName = System.IO.Path.Combine(Constants.AbsolutePath, assmblyName);
            var assembly = Assembly.LoadFile(assmblyName);
            var serviceType = assembly.GetType(serviceName);
            var host = new ServiceHost(serviceType, baseAddresses);

            foreach (var iface in serviceType.GetInterfaces())
            {
                var attr = (ServiceContractAttribute)Attribute.GetCustomAttribute(iface, typeof(ServiceContractAttribute));
                if (attr != null)
                    host.AddServiceEndpoint(iface, new BasicHttpBinding(), "");
            }
            var metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            host.Description.Behaviors.Add(metadataBehavior);
            return host;
        }
    }
}