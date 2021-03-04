using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using Practice3.Models;

namespace Practice3
{
    public class BinaryIntellectVirtualPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            var view = GetViewFromDatabase(virtualPath);

            if (view == null)
            {
                return base.FileExists(virtualPath);
            }
            else
            {
                return true;
            }
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var view = GetViewFromDatabase(virtualPath);

            if (view == null)
            {
                return base.GetFile(virtualPath);
            }
            else
            {
                byte[] content = ASCIIEncoding.ASCII.
                    GetBytes(view.ViewContent);
                return new BinaryIntellectVirtualFile
                    (virtualPath, content);
            }
        }

        //public CacheDependency GetCacheDependency(string virtualPath, Array virtualPathDependencies, DateTime utcStart)
        //{
        //    var view = GetViewFromDatabase(virtualPath);
        //    if (view != null)
        //    {
        //        return null;
        //    }
        //    return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        //}

        private View GetViewFromDatabase(string virtualPath)
        {
            virtualPath = virtualPath.Replace("~", "");

            Practice3DBEntities db = new Practice3DBEntities();
            var view = from v in db.Views
                where v.ViewPath == virtualPath
                select v;
            return view.SingleOrDefault();
        }
    }
}