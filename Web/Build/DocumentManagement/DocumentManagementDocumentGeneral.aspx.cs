using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.IO;
using System.Text;
using Telerik.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;

public partial class DocumentManagementDocumentGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Convert to Word", "PDF", ToolBarDirection.Right);

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            MenuClose.AccessRights = this.ViewState;
            MenuClose.MenuList = toolbar.Show();
        }

        if (!IsPostBack)
        {
            ViewState["showall"] = "";
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

            //To see the preview from the show changes/RA screen.
            if (Request.QueryString["showall"] != null && Request.QueryString["showall"].ToString() != "" && Request.QueryString["showall"].ToString() == "0")
                ViewState["showall"] = Request.QueryString["showall"].ToString();

            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
            else
                ViewState["DOCUMENTID"] = "";

            if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
            else
                ViewState["SECTIONID"] = "";
            GetData();
        }
    }

    private void GetData()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentData(new Guid(ViewState["DOCUMENTID"].ToString())
                , General.GetNullableInteger(ViewState["showall"].ToString())
                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                );

            int firstsectionyn = 1;
            String NewLine = System.Environment.NewLine;
            string starttag = @"<tr style=""text-align: left; margin: 0px; font-size: 10pt; ""><td align=""left"">";
            string endtag = "</td></tr>";
            int type = GetBrowserType();

            string tdwidth = (type == 1) ? "300px" : "700px";

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)     //to bind document name
            {
                DataRow dr = ds.Tables[1].Rows[0];

                string documentname = dr["FLDDOCUMENTNAME"].ToString();

                divForm.InnerHtml = divForm.InnerHtml + @"<table><tr style=""text-align: center; margin: 0px; font-size: 14pt; font-weight: bold; text-decoration: underline;""><td colspan=""4"">" + documentname + endtag;
                divForm.InnerHtml = divForm.InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-weight: bold; font-size: 10pt;""><td></td><td colspan=""3""><br/><br/>Table of Contents" + endtag;


                ViewState["documentname"] = documentname;

                //var sb = new StringBuilder();
                //divForm.RenderControl(new HtmlTextWriter(new StringWriter(sb)));

                //sb.Append(@"<table><tr style=""text-align: center; margin: 0px; font-size: 14pt; font-weight: bold; text-decoration: underline;""><td colspan=""4"">" + documentname + endtag);
                //sb.Append(@"<tr style=""text-align: left; margin: 0px; font-weight: bold; font-size: 10pt;""><td></td><td colspan=""3""><br/><br/>Table of Contents" + endtag);
                //string s = sb.ToString();

                //divForm.InnerHtml = s;
            }

            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections(index)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    divForm.InnerHtml = divForm.InnerHtml + ((dr["FLDLEVEL"].ToString() == "1" && firstsectionyn > 1) ? @"<tr><td colspan=""4""><br/></td></tr>" : "");

                    if (dr["FLDSECTIONID"].ToString().ToLower() != ViewState["DOCUMENTID"].ToString().ToLower())
                    {
                        divForm.InnerHtml = divForm.InnerHtml + starttag + @"</td><td style=""width:" + tdwidth + @";text-align: left;""><a onclick=""return OnSectionClick(event);"" href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                            + dr["FLDSECTIONFULLNAME"].ToString() + @"</a></td><td style=""width:120px;text-align: left;"">" + dr["FLDREVISIONNO"].ToString() + @"</td><td>" + endtag;
                    }
                    else
                    {
                        if (General.GetNullableString(ViewState["callfrom"].ToString()) != null)
                        {
                            string clickevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentRevision.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + @"&showall=" + ViewState["showall"].ToString() + @"'); return false;""";
                            divForm.InnerHtml = divForm.InnerHtml + starttag + @"</td><td colspan=""3""><a onclick=" + clickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDSECTIONFULLNAME"].ToString() + "</a>" + endtag;
                        }
                    }

                    firstsectionyn++;
                }
                divForm.InnerHtml = divForm.InnerHtml + @"<tr><td colspan=""4""><br/></td></tr>";
            }

            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections data
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string onclickevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisionDetailsOfChange.aspx?FLDSECTIONID=" + dr["FLDSECTIONID"].ToString() + @"&showall=" + ViewState["showall"].ToString() + @"'); return false;""";
                    if (dr["FLDSECTIONID"].ToString().ToLower() != ViewState["DOCUMENTID"].ToString().ToLower())
                        divForm.InnerHtml = divForm.InnerHtml + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td><b>"
                                    + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a><td style=""width:140px;text-align: left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b></td><td></td></tr><tr><td colspan=""4""><br/></td></tr>";

                    string strResult = HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString());

                    string repl = "../Common/Download.aspx?dtkey=";

                    string find2 = "../attachments/DOCUMENTMANAGEMENT/";

                    string findOffshore = "https://apps.southnests.com/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                    string findOffshore2 = "http://119.81.76.123:80/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                    string findOffshore3 = "http://119.81.76.123/PhoenixOffshore/attachments/DOCUMENTMANAGEMENT/";

                    string find = "https://apps.southnests.com/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                    string find3 = "http://119.81.76.123:80/Phoenix/attachments/DOCUMENTMANAGEMENT/";

                    string find4 = "http://119.81.76.123/Phoenix/attachments/DOCUMENTMANAGEMENT/";
                   

                    if (strResult.Contains(find2))
                    {
                        strResult = strResult.Replace(find2, repl);
                    }


                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    {

                        if (strResult.Contains(findOffshore))
                        {
                            strResult = strResult.Replace(findOffshore, repl);
                        }

                        if (strResult.Contains(findOffshore2))
                        {
                            strResult = strResult.Replace(findOffshore, repl);
                        }

                        if (strResult.Contains(findOffshore3))
                        {
                            strResult = strResult.Replace(findOffshore3, repl);
                        }
                    }
                    else
                    {
                        if (strResult.Contains(find))
                        {
                            strResult = strResult.Replace(find, repl);
                        }

                        if (strResult.Contains(find3))
                        {
                            strResult = strResult.Replace(find3, repl);
                        }

                        if (strResult.Contains(find4))
                        {
                            strResult = strResult.Replace(find4, repl);
                        }

                    }

                    strResult = Regex.Replace(strResult, "\\d+/\\d+/", "");

                    strResult = strResult.Replace(".jpg", "");
                    strResult = strResult.Replace(".png", "");
                    strResult = strResult.Replace(".tif", "");

                    divForm.InnerHtml = divForm.InnerHtml + starttag + @"</td><td colspan=""3"">" + strResult + endtag;
                    divForm.InnerHtml = divForm.InnerHtml + @"<tr><td colspan=""4""><br/></td></tr>";
                }
            }
            divForm.InnerHtml = divForm.InnerHtml + @"<tr><td colspan=""4""><br/></td></tr></table>";
            string strnowrap = @"nowrap=""";
            divForm.InnerHtml = divForm.InnerHtml.Replace(strnowrap, "");
            strnowrap = @"<p style=""margin: 0px; text-align: center;""><img";
            divForm.InnerHtml = divForm.InnerHtml.Replace(strnowrap, @"<p style=""margin: 0px; text-align: left;""><img");
            strnowrap = "";
            strnowrap = @"<p style=""margin: 0px; text-align: right;""><img";
            divForm.InnerHtml = divForm.InnerHtml.Replace(strnowrap, @"<p style=""margin: 0px; text-align: left;""><img");
            strnowrap = @"<p style=""margin: 0px; text-align: right;"">";
            divForm.InnerHtml = divForm.InnerHtml.Replace(strnowrap, @"<p style=""margin: 0px; text-align: left;"">");

            strnowrap = @"<object id=""ieooui"" classid=""clsid:38481807-CA0E-42D2-BF39-B33AF135CC4D"" />";
            divForm.InnerHtml = divForm.InnerHtml.Replace(strnowrap, "");
            strnowrap = @"</object>";
            divForm.InnerHtml = divForm.InnerHtml.Replace(strnowrap, "");
        }
    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("HTML"))
            {
                string htmldocument = "";
                String NewLine = System.Environment.NewLine;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
                {
                    DataSet ds = PhoenixDocumentManagementDocument.DocumentData(new Guid(ViewState["DOCUMENTID"].ToString())
                        , General.GetNullableInteger(ViewState["showall"].ToString())
                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                        );

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
            else if (CommandName.ToUpper().Equals("PDF"))
            {
                string strHtml = divForm.InnerHtml;

                //PhoenixDocumentManagementDocumentPDF.ConvertToPDF(strHtml);

                //ConvertToPDF(strHtml);

                //string strHtml = divForm.InnerHtml;

                Response.AddHeader("content-disposition", "attachment; filename=" + ViewState["documentname"].ToString().Replace(" ", "") + ".doc");
                Response.ContentType = "application/ms-word";

                Response.Write("<html>");
                Response.Write("<head>");
                Response.Write("<title></title>");
                Response.Write("</head>");
                Response.Write(@"<body style=""margin-top:0;margin-left:50px;margin-right:50px;""><br/><br/>");
                Response.Write(strHtml + "<br/>");
                Response.Write("</body>");
                Response.Write("</html>");
                Response.End();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public static void ConvertToPDF(string htmltext)
    {
        Sgml.SgmlReader reader = new Sgml.SgmlReader();
        reader.DocType = string.Empty;
        reader.InputStream = new StringReader(htmltext);
        StringWriter output = new StringWriter();
        XmlTextWriter writer = new XmlTextWriter(output);
        reader.Read();
        while (!reader.EOF)
        {
            writer.WriteNode(reader, true);
        }
        writer.Close();
        string xmlcode = output.ToString();


        xmlcode = xmlcode.Replace("&amp;", "&");
        xmlcode = xmlcode.Replace("&nbsp;", "&#160;");

        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", DateTime.Today.ToShortDateString().Replace("/", "").Replace("-", "") + "_" + DateTime.Now.ToShortDateString() + ".doc"));
        HttpContext.Current.Response.ContentType = "application/ms-word";

        xmlcode = "<meta charset=\"utf-7\"/>" + xmlcode;
        byte[] bytes = System.Text.Encoding.UTF7.GetBytes(xmlcode);

        HttpContext.Current.Response.OutputStream.Write(bytes, 0, bytes.Length);
        HttpContext.Current.Response.End();

        //HttpContext.Current.Response.Clear();
        //pck.SaveAs(HttpContext.Current.Response.OutputStream);
        //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12 .xltm";
        ////HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=PhoenixDryDock.xlsm");
        //HttpContext.Current.Response.End();


        //string strPath = HttpContext.Current.Request.PhysicalApplicationPath + "Test.doc";
        //StreamWriter sWriter = new StreamWriter(strPath);
        //sWriter.Write(xmlcode);
        //sWriter.Close();

    }

    protected void MenuFind_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
    }

    protected int GetBrowserType()
    {
        string name = "";
        System.Web.HttpBrowserCapabilities browserDetection = Request.Browser;

        name = browserDetection.Browser;
        name = name.ToUpper();        //lblBrowserVersion.Text = browserDetection.Version;         

        if (name == "IE")
            return 1;
        else
            return 2;
    }
}
