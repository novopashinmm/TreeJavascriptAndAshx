using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace TreeView
{
    /// <summary>
    /// Сводное описание для Handler1
    /// </summary>
    public class Handler1 : IHttpHandler
    {
        private static Dictionary<int, string> _dict = new Dictionary<int, string>();
        class FirstFolders
        {
            public string Str { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "json/application";
            JavaScriptSerializer js = new JavaScriptSerializer();

            var strBuild = new StringBuilder();
            if (context.Request.Form["id"] != null)
            {
                var idDirectory = int.Parse(context.Request.Form["id"]);

                var allDirectories = Directory.GetDirectories(_dict[idDirectory]);

                AddDict(allDirectories);

                strBuild.Append("<ul class=\"Container\">");
                foreach (var directory in allDirectories)
                {
                    var key = _dict.First(p => p.Value == directory).Key;
                    strBuild.Append("<li class=\"Node ExpandClosed\" id=\'" + key + "\'>");
                    strBuild.Append("<div class=\"Expand\" path-id=\"" + key + "\"></div>");
                    strBuild.Append("<div class=\"Content\">" + directory + "</div>");
                    strBuild.Append("</li>");
                }
                strBuild.Append("</ul>");
            }
            else
            {
                AddDict(new [] {@"C:\"});
                var allDirectories = Directory.GetDirectories(@"C:\");

                AddDict(allDirectories);

                strBuild.Append("<div onclick=\"tree_toggle(arguments[0])\">");
                strBuild.Append("<div>Tree</div>");
                strBuild.Append("<ul class=\"Container\">");
                foreach (var directory in _dict)
                {
                    if (directory.Key == 0) continue;
                    strBuild.Append("<li class=\"Node IsRoot IsLast ExpandClosed\" id=\'" + directory.Key + "\'>");
                    strBuild.Append("<div class=\"Expand\" path-id=\"" + directory.Key + "\"></div>");
                    strBuild.Append("<div class=\"Content\">" + directory.Value + "</div>");
                    strBuild.Append("</li>");
                }
                strBuild.Append("</ul>");
                strBuild.Append("</div>");
            }

            var ff = new FirstFolders {Str = strBuild.ToString()};
            context.Response.Write(js.Serialize(ff));
        }

        public void AddDict(string[] directories)
        {
            foreach (var item in directories)
            {
                if (!_dict.ContainsValue(item))
                    _dict.Add(_dict.Count, item);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}