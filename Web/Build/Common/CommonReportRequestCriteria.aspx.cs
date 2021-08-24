using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.CrewReports;
using System.Xml.Linq;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Text;


public partial class Common_CommonReportRequestCriteria : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["COMPANYID"] = "";
            ViewState["SELECTEDCOMPANYID"] = "";
            ViewState["DEFICIENCYTYPE"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            if (Request.QueryString["REPORTREQUESTID"] != null && Request.QueryString["REPORTREQUESTID"].ToString() != string.Empty)
                ViewState["REPORTREQUESTID"] = Request.QueryString["REPORTREQUESTID"].ToString();

            DataSet ds;

            ds = PhoenixXLReportRequest.ReportRequestCriteria(General.GetNullableGuid(ViewState["REPORTREQUESTID"].ToString()));

            if (ds.Tables.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataTable dt1 = ds.Tables[1];
                    foreach (DataRow dr in dt1.Rows)
                    {
                        foreach (DataColumn dc in dt1.Columns)
                        {
                            sb.AppendFormat("<table style=\"font-size:11px;width:40%;\"> ");
                            sb.AppendFormat("<tr align=\"left\">");
                            sb.AppendFormat("<td style=\"width:50%;\" align=\"left\"><b>");
                            sb.AppendFormat("\t{0}", dc.ColumnName);
                            sb.AppendFormat("</b></td>");
                            sb.AppendFormat("<td align=\"left\">");
                            sb.AppendFormat("\t{0}", dr[dc]);
                            sb.AppendFormat("</td>");
                            sb.AppendFormat("</tr>");
                        }
                    }

                    sb.AppendFormat("</table>");
                    ltGrid.Text = sb.ToString();
                }
                else
                {
                    sb.AppendFormat("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
                    sb.AppendFormat("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
                    sb.AppendFormat("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                    sb.AppendFormat("</table>");

                    ltGrid.Text = sb.ToString();
                }
            }
        }
    }
}
