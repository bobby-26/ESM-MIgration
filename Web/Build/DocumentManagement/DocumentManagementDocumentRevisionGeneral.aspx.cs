using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.IO;
using System.Text;

public partial class DocumentManagementDocumentRevisionGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
            {
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
                divForm.Attributes.Add("style", "width: 980px;");
                divForm.Attributes.Add("style", "padding-right:1%;");
                ViewState["showall"] = 0;
            }
            else
            {
                ViewState["callfrom"] = "";
                divForm.Attributes.Add("style", "padding-right:8%;");
                ViewState["showall"] = 1;
            }

            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
            else
                ViewState["DOCUMENTID"] = "";

            if (Request.QueryString["DOCUMENTREVISIONID"] != null && Request.QueryString["DOCUMENTREVISIONID"].ToString() != "")
                ViewState["DOCUMENTREVISIONID"] = Request.QueryString["DOCUMENTREVISIONID"].ToString();
            else
                ViewState["DOCUMENTREVISIONID"] = "";
            GetData();
        }
    }

    private void GetData()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentRevisionData(new Guid(ViewState["DOCUMENTID"].ToString())
                , General.GetNullableGuid(ViewState["DOCUMENTREVISIONID"].ToString()));

            int firstsectionyn = 1;
            String NewLine = System.Environment.NewLine;
            string starttag = @"<tr style=""text-align: left; margin: 0px; font-size: 10pt; ""><td align=""left"">";
            string endtag = "</td></tr>";

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)     //to bind document name
            {
                DataRow dr = ds.Tables[1].Rows[0];

                string documentname = dr["FLDDOCUMENTNAME"].ToString();
                ViewState["documentname"] = documentname;

                span1.InnerHtml = span1.InnerHtml + @"<table><tr style=""text-align: center; margin: 0px; font-size: 14pt; font-weight: bold; text-decoration: underline""><td colspan=""4"">" + documentname + endtag;
                span1.InnerHtml = span1.InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-weight: bold; font-size: 10pt;""><td></td><td><br/><br/>Table of Contents" + endtag;
            }

            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections(index)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    span1.InnerHtml = span1.InnerHtml + ((dr["FLDLEVEL"].ToString() == "1" && firstsectionyn > 1) ? "<tr><td><br/></td></tr>" : "");

                    if (dr["FLDSECTIONID"].ToString() != ViewState["DOCUMENTID"].ToString())
                    {
                        span1.InnerHtml = span1.InnerHtml + starttag + @"</td><td><a onclick=""return OnSectionClick(event);"" href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                            + dr["FLDSECTIONFULLNAME"].ToString() + "</a>" + endtag;
                    }
                    else
                    {
                        if (General.GetNullableString(ViewState["callfrom"].ToString()) != null)
                        {
                            string clickevent = @"javascript:parent.Openpopup('codehelp1','','../DocumentManagement/DocumentManagementDocumentRevision.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + @"&showall=" + ViewState["showall"].ToString() + @"'); return false;""";
                            span1.InnerHtml = span1.InnerHtml + starttag + @"</td><td><a onclick=" + clickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDSECTIONFULLNAME"].ToString() + "</a>" + endtag;
                        }
                    }

                    firstsectionyn++;
                }
                span1.InnerHtml = span1.InnerHtml + "<tr><td><br/></td></tr>";
            }

            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections data
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FLDSECTIONID"].ToString() != ViewState["DOCUMENTID"].ToString())
                        span1.InnerHtml = span1.InnerHtml + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td><b>"
                                    + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a></td>";

                    span1.InnerHtml = span1.InnerHtml + starttag + "</td><td>" + HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString()) + endtag;
                    span1.InnerHtml = span1.InnerHtml + "<tr><td><br/></td></tr>";
                }
            }
        }
    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("HTML"))
            {
                string htmldocument = "";
                String NewLine = System.Environment.NewLine;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
                {
                    DataSet ds = PhoenixDocumentManagementDocument.DocumentData(new Guid(ViewState["DOCUMENTID"].ToString())
                        , General.GetNullableInteger(ViewState["showall"].ToString()));

                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)     //to bind document name
                    {
                        DataRow dr = ds.Tables[1].Rows[0];

                        string documentname = dr["FLDDOCUMENTNAME"].ToString();
                        ViewState["documentname"] = documentname;
                        htmldocument = htmldocument + @"<p style=""text-align: center; margin: 0px; font-size: 16pt; font-weight: bold; text-decoration: underline"">" + documentname + "</p>";
                        htmldocument = htmldocument + "</br>" + @"<p style=""text-align: left; margin: 0px; font-weight: bold; font-size: 12pt;"">" + "Table of Contents" + "</p>";
                    }

                    if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections
                    {
                        htmldocument = htmldocument + "<br/>";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            htmldocument = htmldocument + @"<p style=""text-align: left; margin: 0px; font-weight: bold; font-size: 13pt;"">" + HttpUtility.HtmlDecode(dr["FLDSECTION"].ToString()) + "</p>";
                    }
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)     //to bind document sections data
                    {
                        DataRow dr = ds.Tables[1].Rows[0];
                        htmldocument = htmldocument + "<br/><br/>";
                        htmldocument = htmldocument + @"<font size=""4"">" + HttpUtility.HtmlDecode(dr["FLDDATA"].ToString()) + "</font>";
                        htmldocument = htmldocument + "<br/>";
                    }
                }

                StringBuilder htmldoc = new StringBuilder();

                htmldoc.AppendLine("<html>");
                htmldoc.AppendLine("<head>");
                htmldoc.AppendLine("<title></title>");
                htmldoc.AppendLine("</head>");
                htmldoc.Append(@"<body style=""margin-top:0;margin-left:50px;margin-right:50px;""><br/><br/>");
                htmldoc.AppendLine(htmldocument + "<br/>");
                htmldoc.AppendLine("</body>");
                htmldoc.AppendLine("</html>");

                FileStream fs = File.OpenWrite(path + "\\" + ViewState["documentname"] + "_" + Guid.NewGuid() + ".html");
                StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                writer.Write(htmldoc);
                writer.Close();
                ucStatus.Text = "Document is saved on the desktop.";

            }
            else if (dce.CommandName.ToUpper().Equals("PDF"))
            {
                //Response.Redirect("../DocumentManagement/DocumentManagementDocumentSectionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&SECTIONID=" + ViewState["SECTIONID"].ToString());               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuFind_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
    }
}
