using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
        private static Dictionary<int, string> _dictDirs = new Dictionary<int, string>();
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

                var allDirectories = Directory.GetDirectories(_dictDirs[idDirectory]);

                AddDictDirs(allDirectories);

                strBuild.Append("<ul class=\"Container\">");
                foreach (var directory in allDirectories)
                {
                    var key = _dictDirs.First(p => p.Value == directory).Key;
                    strBuild.Append("<li class=\"Node ExpandClosed\" id=\'" + key + "\'>");
                    strBuild.Append("<div class=\"Expand\" path-id=\"" + key + "\"></div>");
                    strBuild.Append("<div class=\"Content\" path-id=\"" + key + "\">" + directory + "</div>");
                    strBuild.Append("</li>");
                }
                strBuild.Append("</ul>");
            }
            else if (context.Request.Form["idDirectory"] != null)
            {
                var idDirectory = int.Parse(context.Request.Form["idDirectory"]);
                var allFiles = Directory.GetFiles(_dictDirs[idDirectory]);

                strBuild.Append("<ul class=\"Files\">");
                foreach (var file in allFiles)
                {
                    strBuild.Append("<li><div>" + file + "</div></li>");
                }
                strBuild.Append("</ul>");
            }
            else
            {
                AddDictDirs(new [] {@"C:\"});
                var allDirectories = Directory.GetDirectories(@"C:\");

                AddDictDirs(allDirectories);
                strBuild.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"5\">");
                strBuild.Append("<tr>");
                strBuild.Append("<td width=\"400\" valign=\"top\">");
                strBuild.Append("<div onclick=\"tree_toggle(arguments[0])\">");
                strBuild.Append("<div>Tree</div>");
                strBuild.Append("<ul class=\"Container\">");
                foreach (var directory in _dictDirs)
                {
                    if (directory.Key == 0) continue;
                    strBuild.Append("<li class=\"Node IsRoot IsLast ExpandClosed\" id=\'" + directory.Key + "\'>");
                    strBuild.Append("<div class=\"Expand\" path-id=\"" + directory.Key + "\"></div>");
                    strBuild.Append("<div class=\"Content\" path-id=\"" + directory.Key + "\">" + directory.Value + "</div>");
                    strBuild.Append("</li>");
                }
                strBuild.Append("</ul>");
                strBuild.Append("</div>");
                strBuild.Append("</td>");
                strBuild.Append("<td id=\"ForFiles\" valign=\"top\"></td>");
                strBuild.Append("</tr>");
                strBuild.Append("</table>");
            }

            var ff = new FirstFolders {Str = strBuild.ToString()};
            context.Response.Write(js.Serialize(ff));
        }

        public void AddDictDirs(string[] directories)
        {
            foreach (var item in directories)
            {
                if (!_dictDirs.ContainsValue(item))
                    _dictDirs.Add(_dictDirs.Count, item);
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