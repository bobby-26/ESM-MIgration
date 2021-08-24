using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewDMSDocumentSectionView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Convert to Word", "PDF");

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                MenuClose.AccessRights = this.ViewState;
                MenuClose.MenuList = toolbar.Show();
            }

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

            //if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
            //{
            //    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
            //    GetDocumentName();
            //}
            //else
            //    ViewState["DOCUMENTID"] = "";

            //if (Request.QueryString["FLDSECTIONID"] != null && Request.QueryString["FLDSECTIONID"].ToString() != "")
            //    ViewState["SECTIONID"] = Request.QueryString["FLDSECTIONID"].ToString();
            //else
            //    ViewState["SECTIONID"] = "";
            GetData();
        }

    }

    private void GetData()
    {
        string documentId = null,sectionId=null;
        if (Request.QueryString["FLDSECTIONID"] != null)
            sectionId = Request.QueryString["FLDSECTIONID"].ToString();

        DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionData(
                                                    General.GetNullableGuid(documentId)
                                                    , General.GetNullableGuid(sectionId)
                                                    , 0
                                                    );

        String NewLine = System.Environment.NewLine;
        string starttag = @"<tr style=""text-align: left; margin: 0px; font-size: 10pt; ""><td align=""left"">";
        string endtag = "</td></tr>";

        int type = GetBrowserType();
        string tdwidth = (type == 1) ? "300px" : "700px";

        int subsectioncount = (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0) ? ds.Tables[1].Rows.Count : 0;
        string innerHtml = null;
        if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document name
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ttlContent.Text = ds.Tables[0].Rows[0]["FLDDOCUMENTNAME"].ToString();
            string sectionname = dr["FLDSECTIONNAME"].ToString();
           
            innerHtml = innerHtml + @"<table><tr style=""text-align: center; margin: 0px; font-size: 14pt; font-weight: bold; text-decoration: underline;""><td colspan=""4"">" + sectionname + endtag;
            if (subsectioncount > 1)                    //to show table of contents when it has sub sections
                innerHtml = innerHtml + @"<tr style=""text-align: left; margin: 0px; font-weight: bold; font-size: 10pt;""><td></td><td colspan=""3""><br/><br/>Table of Contents" + endtag;
        }

        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            if (subsectioncount > 1)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)                   //to bind document sections(index)
                {
                    innerHtml = innerHtml + starttag + @"</td><td style=""width:" + tdwidth + @";text-align: left;""><a href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                               + dr["FLDSECTIONFULLNAME"].ToString() + @"</a></td><td style=""width:120px;text-align: left;"">" + dr["FLDREVISIONDETAILS"].ToString() + @"</td><td>" + endtag;
                }

                innerHtml = innerHtml + @"<tr><td colspan=""4""><br/><br/></td></tr>";

                foreach (DataRow dr in ds.Tables[1].Rows)                   //to bind document sections(data)
                {
                    string onclickevent = @"javascript:parent.Openpopup('codehelp1','','../DocumentManagement/DocumentManagementDocumentSectionRevisionDetailsOfChange.aspx?FLDSECTIONID=" + dr["FLDSECTIONID"].ToString() + @"'); return false;""";
                    innerHtml = innerHtml + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td><b>"
                        + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a><td style=""width:" + ((type == 1) ? "120px" : "150px") + @";text-align: left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONDETAILS"].ToString() + "</a>" + @"</b></td><td></td></tr><tr><td colspan=""4""><br/></td></tr>";

                    innerHtml = innerHtml + starttag + @"</td><td colspan=""3"">" + HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString()) + endtag;
                    innerHtml = innerHtml + @"<tr><td colspan=""4""><br/></td></tr>";
                }
            }
            else
            {
                innerHtml = innerHtml + @"<tr><td colspan=""4""><br/><br/></td></tr>";
                foreach (DataRow dr in ds.Tables[1].Rows)                   //to bind document sections(data)
                {
                    string onclickevent = @"javascript:parent.Openpopup('codehelp1','','../DocumentManagement/DocumentManagementDocumentSectionRevisionDetailsOfChange.aspx?FLDSECTIONID=" + dr["FLDSECTIONID"].ToString() + @"'); return false;""";
                    innerHtml = innerHtml + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td style=width:150px><b>"
                                + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a><td style=""width:" + ((type == 1) ? "120px" : "150px") + @";text-align: right;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONDETAILS"].ToString() + "</a>" + @"</b></td><td></td></tr><tr><td colspan=""4""><br/></td></tr>";

                    innerHtml = innerHtml + starttag + @"</td><td colspan=""3""><p style=line-height: 50%>" + HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString()) + @"</p>" + endtag;
                    innerHtml = innerHtml + @"<tr><td colspan=""4""><br/></td></tr>";
                }
            }
        }
        divForm.InnerHtml = innerHtml + "</table>";
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

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("PDF"))
            {
                string strHtml = divForm.InnerHtml;

                PhoenixDocumentManagementDocumentPDF.ConvertToPDF(strHtml);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
    }
}