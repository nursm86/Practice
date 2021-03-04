using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Practice3
{
    public class BinaryIntellectVirtualFile : VirtualFile
    {
        private byte[] viewContent;

        public BinaryIntellectVirtualFile(string virtualPath,
            byte[] viewContent) : base(virtualPath)
        {
            this.viewContent = viewContent;
        }

        public override Stream Open()
        {
            return new MemoryStream(viewContent);
        }
    }
}