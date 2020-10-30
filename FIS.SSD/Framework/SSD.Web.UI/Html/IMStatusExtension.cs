using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace SSD.Web.UI.Html
{
    public class Nic
    {
        public Nic(Protocol p, string n, string m)
        {
            Protocol = p;
            NicName = n;
            Msg = m;
        }
        public Protocol Protocol { get; set; }
        public string NicName { get; set; }
        public string Msg { get; set; }
    }
    public enum Protocol
    {
        yahoo,
        msn,
        aol,
        icq,
        skype
    }
    public static class IMStatusExtension
    {
        public static MvcHtmlString IMStatus(this HtmlHelper helper, List<Nic> nics, string displayName)
        {
            var html = new TagBuilder("div");
            html.MergeAttribute("id", "imstatus-title");
            var str = new StringBuilder("<div class=\"imstatus-displayname\">").Append(displayName).Append("</div>");
            foreach(var nic in nics)
            {
                str.Append("<div class=\"imstatus-icon\"><a href=\"")
                    .Append(GetLink(nic.Protocol, nic.NicName, nic.Msg))
                    .Append("\">")
                    .Append(GetIcon(nic.Protocol, nic.NicName))
                    .Append("</a>")
                    .Append("</div>");
            }
            html.InnerHtml = str.ToString();

            return new MvcHtmlString(html.ToString(TagRenderMode.Normal));
        }
        private static string GetLink(Protocol protocol, string nicName, string msg)
        {
            string link = "";
            switch (protocol)
            {
                case Protocol.yahoo:
                    link = string.Format("ymsgr:sendim?{0}&amp;m={1}",nicName,msg);
                    break;
                case Protocol.msn:
                    break;
                case Protocol.aol:
                    break;
                case Protocol.icq:
                    break;
                case Protocol.skype:
                    link = string.Format("skype:{0}?chat", nicName, msg);
                    break;
            }
            return link;
        }
        private static string GetIcon(Protocol protocol, string nicName)
        {
            string Status = "";
            switch (protocol)
            {
                case Protocol.yahoo:
                    Status = "<img src=\"http://opi.yahoo.com/online?u=" + nicName + "&m=g&t=0\" border=\"0\">";
                    break;
                case Protocol.msn:
                    Status = "<img src=\"http://www.funnyweb.dk:8080/msn/" + nicName + "/onurl=www.braintechnosys.com/images/msnonline.png/offurl=www.braintechnosys.com/images/msnoffline.png/unknownurl=www.braintechnosys.com/images/msnoffline.png\" align=\"absmiddle\">";
                    break;
                case Protocol.aol:
                    Status = "<img src=\"http://big.oscar.aol.com/" + nicName + "?on_url=http://www.aim.com/remote/gr/MNB_online.gif&off_url=http://www.aim.com/remote/gr/MNB_offline.gif\" style=\"border: none;\" alt=\"My status\" />";
                    break;
                case Protocol.icq:
                    Status = "<img src=\"http://web.icq.com/whitepages/online?icq=" + nicName + "&img=26\" />";
                    break;
                case Protocol.skype:
                    Status = "<img src=\"http://mystatus.skype.com/smallicon/" + nicName + "\" border=\"0\" />";
                    break;
            }
            return Status;
        }
    }
}
