using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.IO;
using System.Globalization;
using System.Web;

namespace AppCode
{
    public class WcfVirtualFile : VirtualFile
    {
        private string _service;
        private string _factory;

        public WcfVirtualFile(string vp, string service, string factory)
            : base(vp)
        {
            _service = service;
            _factory = factory;
        }

        public override Stream Open()
        {
            var ms = new MemoryStream();
            var tw = new StreamWriter(ms);
            tw.Write(string.Format(CultureInfo.InvariantCulture,
                "<%@ServiceHost language=c# Debug=\"true\" Service=\"{0}\" Factory=\"{1}\"%>",
                HttpUtility.HtmlEncode(_service), HttpUtility.HtmlEncode(_factory)));

            tw.Flush();
            ms.Position = 0;
            return ms;
        }
    }
}