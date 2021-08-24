using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Text;
using System.IO;
using Telerik.Web.UI;
using System.Text.RegularExpressions;

public partial class DocumentManagementDocumentSectionContentView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Convert to Word", "PDF", ToolBarDirection.Right);

            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
            {
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
                divForm.Attributes.Add("style", "width: 900px;");
                divForm.Attributes.Add("style", "padding-right:1%;");
            }
            else
            {
                ViewState["callfrom"] = "";
                divForm.Attributes.Add("style", "width: 850px;");
                divForm.Attributes.Add("style", "padding-right:8%;");
            }

            if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
            {
                ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                GetDocumentName();
            }
            else
                ViewState["DOCUMENTID"] = "";

            if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
            else
                ViewState["SECTIONID"] = "";
            GetData();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                MenuClose.AccessRights = this.ViewState;
                MenuClose.MenuList = toolbar.Show();
            }

        }

    }

    private void GetDocumentName()
    {
        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.DocumentEdit(new Guid(ViewState["DOCUMENTID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                MenuClose.Title = dr["FLDDOCUMENTNAME"].ToString();
            }
        }
    }

    private void GetData()
    {
        DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionData(
                                                    General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                                                    , General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                                                    , 0
                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                    );

        String NewLine = System.Environment.NewLine;

        string starttag = @"<tr style=""text-align: left; margin: 0px; font-size: 10pt; ""><td align=""left"">";
        string endtag = "</td></tr>";

        int type = GetBrowserType();
        string tdwidth = (type == 1) ? "300px" : "700px";

        int subsectioncount = (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows.Count : 0;

        if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document name
        {
            DataRow dr = ds.Tables[0].Rows[0];
            MenuClose.Title = ds.Tables[0].Rows[0]["FLDDOCUMENTNAME"].ToString();
            string sectionname = dr["FLDSECTIONNAME"].ToString();
            ViewState["sectionname"] = dr["FLDSECTIONNAME"].ToString();

            divForm.InnerHtml = divForm.InnerHtml + @"<table><tr style=""text-align: center; margin: 0px; font-size: 14pt; font-weight: bold; text-decoration: underline;""><td colspan=""4"">" + sectionname + endtag;

            if (subsectioncount > 1)                    //to show table of contents when it has sub sections
                divForm.InnerHtml = divForm.InnerHtml + @"<tr style=""text-align: left; margin: 0px; font-weight: bold; font-size: 10pt;""><td></td><td colspan=""3""><br/><br/>Table of Contents" + endtag;

            //var sb = new StringBuilder();
            //divForm.RenderControl(new HtmlTextWriter(new StringWriter(sb)));

            //sb.Append(@"<table><tr style=""text-align: center; margin: 0px; font-size: 14pt; font-weight: bold; text-decoration: underline;""><td colspan=""4"">" + sectionname + endtag);

            //if (subsectioncount > 1)                    //to show table of contents when it has sub sections
            //    sb.Append(@"<tr style=""text-align: left; margin: 0px; font-weight: bold; font-size: 10pt;""><td></td><td colspan=""3""><br/><br/>Table of Contents" + endtag);

            //string s = sb.ToString();

            //divForm.InnerHtml = s;
        }

        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            if (subsectioncount > 1)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)                   //to bind document sections(index)
                {
                    divForm.InnerHtml = divForm.InnerHtml + starttag + @"</td><td style=""width:" + tdwidth + @";text-align: left;""><a href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                               + dr["FLDSECTIONFULLNAME"].ToString() + @"</a></td><td style=""width:120px;text-align: left;"">" + dr["FLDREVISIONDETAILS"].ToString() + @"</td><td>" + endtag;
                }

                divForm.InnerHtml = divForm.InnerHtml + @"<tr><td colspan=""4""><br/><br/></td></tr>";

                foreach (DataRow dr in ds.Tables[1].Rows)                   //to bind document sections(data)
                {
                    string onclickevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisionDetailsOfChange.aspx?FLDSECTIONID=" + dr["FLDSECTIONID"].ToString() + @"'); return false;""";
                    divForm.InnerHtml = divForm.InnerHtml + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td><b>"
                        + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a><td style=""width:" + ((type == 1) ? "120px" : "150px") + @";text-align: left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONDETAILS"].ToString() + "</a>" + @"</b></td><td></td></tr><tr><td colspan=""4""><br/></td></tr>";

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
            else
            {
                divForm.InnerHtml = divForm.InnerHtml + @"<tr><td colspan=""4""><br/><br/></td></tr>";
                foreach (DataRow dr in ds.Tables[1].Rows)                   //to bind document sections(data)
                {
                    string onclickevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisionDetailsOfChange.aspx?FLDSECTIONID=" + dr["FLDSECTIONID"].ToString() + @"'); return false;""";
                    divForm.InnerHtml = divForm.InnerHtml + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td style=width:150px><b>"
                                + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a><td style=""width:" + ((type == 1) ? "120px" : "150px") + @";text-align: right;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONDETAILS"].ToString() + "</a>" + @"</b></td><td></td></tr><tr><td colspan=""4""><br/></td></tr>";

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

                    divForm.InnerHtml = divForm.InnerHtml + starttag + @"</td><td colspan=""3""><p style=line-height: 50%>" + strResult + @"</p>" + endtag;
                    divForm.InnerHtml = divForm.InnerHtml + @"<tr><td colspan=""4""><br/></td></tr>";
                }
            }
        }
        divForm.InnerHtml = divForm.InnerHtml + "</table>";
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

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("PDF"))
            {
                string strHtml = divForm.InnerHtml;

                // PhoenixDocumentManagementDocumentPDF.ConvertToPDF(strHtml);
                Response.AddHeader("content-disposition", "attachment; filename=" + ViewState["sectionname"].ToString().Replace(" ", "") + ".doc");
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

