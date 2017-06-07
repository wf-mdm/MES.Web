using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MES.Web.Controllers
{
    public static class MESHtml
    {
        public static IEnumerable<SelectListItem> YesOrNo
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem(){Text="Y", Value="Y"},
                    new SelectListItem(){Text="N", Value="N"}
                };
            }
        }

        public static MvcHtmlString RadioButtonEx(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string selValue)
        {
            StringBuilder sb = new StringBuilder();
            foreach (SelectListItem item in selectList)
            {
                sb.Append("<label>");
                sb.Append("<input type=\"radio\" name=\"");
                sb.Append(name);
                sb.Append("\" value=\"");
                sb.Append(item.Value);
                sb.Append("\"");
                if (item.Value.Equals(selValue))
                    sb.Append(" checked");
                sb.Append("/>");
                sb.Append(item.Text);
                sb.Append("</label>");
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}