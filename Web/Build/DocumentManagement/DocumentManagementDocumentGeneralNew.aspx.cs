using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Windows.Media;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Model;

public partial class DocumentManagement_DocumentManagementDocumentGeneralNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Export PDF", "PDF", ToolBarDirection.Right);
            toolbar.AddButton("Export DOCX", "WORD", ToolBarDirection.Right);

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                MenuSave.AccessRights = this.ViewState;
                MenuSave.MenuList = toolbar.Show();
            }

            ucEditor.TrackChangesSettings.Author = PhoenixSecurityContext.CurrentSecurityContext.FirstName + " " + PhoenixSecurityContext.CurrentSecurityContext.LastName;
            ucEditor.EnableComments = true;




            string evtArg = Request["__EVENTARGUMENT"];
            switch (evtArg)
            {
                case "SaveAsDocx":
                    ucEditor.ExportToDocx();
                    break;
                case "SaveAsPDF":
                    ucEditor.ExportToPdf();
                    break;
                default:
                    break;
            }

            if (!IsPostBack)
            {
                ViewState["sectionname"] = string.Empty;
                ViewState["documentname"] = string.Empty;
                ViewState["showall"] = "";
                if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
                {
                    ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();

                    ViewState["showall"] = 0;
                }
                else
                {
                    ViewState["callfrom"] = "";
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
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
                //MenuSave.Title = dr["FLDDOCUMENTNAME"].ToString();
            }
        }
    }

    private void GetData()
    {
        try
        {
            if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() != "")
            {
                DataSet ds = PhoenixDocumentManagementDocument.NewDocumentData(new Guid(ViewState["DOCUMENTID"].ToString())
                    , General.GetNullableInteger(ViewState["showall"].ToString())
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    );

                int firstsectionyn = 1;
                String NewLine = System.Environment.NewLine;
                String divForm = "";
                string starttag = @"<tr style=""text-align:left;margin:0px;font-size:10pt;""><td style=""width:60px;text-align:left;"">";
                string endtag = "</td></tr>";
                int type = GetBrowserType();

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)     //to bind document name
                {
                    DataRow dr = ds.Tables[1].Rows[0];

                    string documentname = dr["FLDDOCUMENTNAME"].ToString();

                    divForm = @"<table style=""width:700px;""><tr style=text-align:center;margin:0px;font-size:14pt;font-weight:bold;text-decoration:underline;><td colspan=4>" + documentname + endtag;
                    divForm = divForm + @"<tr style=""text-align:left;margin:0px;font-weight:bold;font-size:10pt;""><td colspan=""4""><br/><br/>Table of Contents" + endtag;

                    ViewState["documentname"] = documentname;
                }

                if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections(index)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        divForm = divForm + ((dr["FLDLEVEL"].ToString() == "1" && firstsectionyn > 1) ? @"<tr><td colspan=""4""><br/></td></tr>" : "");

                        string onclickevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisionDetailsOfChange.aspx?FLDSECTIONID=" + dr["FLDSECTIONID"].ToString() + @"&showall=" + ViewState["showall"].ToString() + @"'); return false;""";
                        string onclickquestionevent = @"javascript:parent.openNewWindow('codehelp2','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementQuestionResponse.aspx?sectionid=" + dr["FLDSECTIONID"].ToString() + @"&revisionid=" + dr["FLDREVISONID"].ToString() + @"'); return false;""";
                        string onclickformevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementLinkedForms.aspx?sectionid=" + dr["FLDSECTIONID"].ToString() + @"&revisionid=" + dr["FLDREVISONID"].ToString() + @"','false','500px','500px'); return false;""";

                        if (dr["FLDSECTIONID"].ToString().ToLower() != ViewState["DOCUMENTID"].ToString().ToLower())
                        {
                            if (dr["FLDFORMSPOSTERCHECKLISTIDS"].ToString() != null && dr["FLDFORMSPOSTERCHECKLISTIDS"].ToString() != "")
                            {
                                if (dr["FLDREADYN"].ToString().Equals("0") && dr["FLDNOTREAD"].ToString().Equals("1"))
                                {
                                    divForm = divForm + @"<td colspan=3 style =""width:560px;text-align:left;""><a onclick=""return OnSectionClick(event);"" href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                                    + dr["FLDSECTIONFULLNAME"].ToString() + @"</a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;<b><a title=Linked_Forms onclick=" + onclickformevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-file-contract-af\"></i></span></a>" + "<a title = 'Details of Change' onclick = " + onclickquestionevent + @" href = ""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-exclamation-circle\"></i></span></a>" + @"</b></td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                                else
                                {
                                    divForm = divForm + @"<td colspan=3 style =""width:560px;text-align:left;""><a onclick=""return OnSectionClick(event);"" href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                                    + dr["FLDSECTIONFULLNAME"].ToString() + @"</a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;<b><a title=Linked_Forms onclick=" + onclickformevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-file-contract-af\" ></i></span></a>" + @"</b></td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                            }
                            else
                            {
                                if (dr["FLDREADYN"].ToString().Equals("0") && dr["FLDNOTREAD"].ToString().Equals("1"))
                                {
                                    divForm = divForm + @"<td colspan=3 style =""width:560px;text-align:left;""><a onclick=""return OnSectionClick(event);"" href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                                    + dr["FLDSECTIONFULLNAME"].ToString() + @"</a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;<b><a title = 'Details of Change' onclick = " + onclickquestionevent + @" href = ""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-exclamation-circle\"></i></span></a>" + @"</b></td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                                else
                                {
                                    divForm = divForm + @"<td colspan=3 style =""width:560px;text-align:left;""><a onclick=""return OnSectionClick(event);"" href=""#" + dr["FLDSECTIONID"].ToString() + @""">"
                                    + dr["FLDSECTIONFULLNAME"].ToString() + @"</a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;</td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                            }
                        }

                        else
                        {
                            if (General.GetNullableString(ViewState["callfrom"].ToString()) != null)
                            {
                                string clickevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentRevision.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + @"&showall=" + ViewState["showall"].ToString() + @"'); return false;""";
                                divForm = divForm + @"<td colspan=""4""><a onclick=" + clickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDSECTIONFULLNAME"].ToString() + "</a>";
                            }
                        }

                        firstsectionyn++;
                    }
                    divForm = divForm + @"<tr><td colspan=""4""><br/></td></tr>";
                }

                if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections data
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string onclickevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionRevisionDetailsOfChange.aspx?FLDSECTIONID=" + dr["FLDSECTIONID"].ToString() + @"&showall=" + ViewState["showall"].ToString() + @"'); return false;""";
                        string onclickquestionevent = @"javascript:parent.openNewWindow('codehelp2','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementQuestionResponse.aspx?sectionid=" + dr["FLDSECTIONID"].ToString() + @"&revisionid=" + dr["FLDREVISONID"].ToString() + @"'); return false;""";
                        string onclickformevent = @"javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementLinkedForms.aspx?sectionid=" + dr["FLDSECTIONID"].ToString() + @"&revisionid=" + dr["FLDREVISONID"].ToString() + @"','false','500px','500px'); return false;""";

                        if (dr["FLDSECTIONID"].ToString().ToLower() != ViewState["DOCUMENTID"].ToString().ToLower())
                        {
                            if (dr["FLDFORMSPOSTERCHECKLISTIDS"].ToString() != null && dr["FLDFORMSPOSTERCHECKLISTIDS"].ToString() != "")
                            {
                                if (dr["FLDREADYN"].ToString().Equals("0") && dr["FLDNOTREAD"].ToString().Equals("1"))
                                {
                                    divForm = divForm + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td colspan=2 style=width:475px><b>"
                                                + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;<b><a title=Linked_Forms onclick=" + onclickformevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-file-contract-af\"></i></span></a>" + "<a title = 'Details of Change' onclick = " + onclickquestionevent + @" href = ""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-exclamation-circle\"></i></span></a>" + @"</b></td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                                else
                                {
                                    divForm = divForm + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td colspan=2 style=width:475px><b>"
                                                   + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;<b><a title=Linked_Forms onclick=" + onclickformevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-file-contract-af\"></i></span></a>" + @"</b></td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                            }
                            else
                            {
                                if (dr["FLDREADYN"].ToString().Equals("0") && dr["FLDNOTREAD"].ToString().Equals("1"))
                                {
                                    divForm = divForm + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td colspan=2 style=width:475px><b>"
                                                + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;<b><a title = 'Details of Change' onclick = " + onclickquestionevent + @" href = ""#" + dr["FLDSECTIONID"].ToString() + @""">" + "" + "<span class=\"icon\"><i class=\"fa-exclamation-circle\"></i></span></a>" + @"</b></td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                                else
                                {
                                    divForm = divForm + starttag + "<b>" + dr["FLDSECTIONNUMBER"].ToString() + "</b></td><td colspan=2 style=width:475px><b>"
                                                   + dr["FLDSECTIONNAME"].ToString() + "</b>" + @"<a id=""" + dr["FLDSECTIONID"].ToString() + @""" ></a></td><td style=""width:180px;text-align:left;""><b><a onclick=" + onclickevent + @" href=""#" + dr["FLDSECTIONID"].ToString() + @""">" + dr["FLDREVISIONNO"].ToString() + "</a>" + @"</b>&nbsp;</td></tr><tr><td colspan=""4""><br/></td></tr>";
                                }
                            }
                        }
                        string strResult = HttpUtility.HtmlDecode(dr["FLDNEWTEXT"].ToString());

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

                        //strResult = Regex.Replace(strResult, "\\d+/\\d+/", "");
                        strResult = strResult.Replace(".jpg", "");
                        strResult = strResult.Replace(".png", "");
                        strResult = strResult.Replace(".tif", "");

                        divForm = divForm + starttag + @"</td><td colspan =""3"" style=""width:630px""><p>" + strResult + @"</p>" + endtag;
                        divForm = divForm + @"<tr><td colspan=""4""><br/></td></tr>";
                    }
                }
                divForm = divForm + @"<tr><td colspan=""4""><br/></td></tr></table>";
                string strnowrap = @"nowrap=""";
                divForm = divForm.Replace(strnowrap, "");
                strnowrap = @"<p style=""margin:0px;text-align:center;""><img";
                divForm = divForm.Replace(strnowrap, @"<p style=""margin:0px;text-align:left;""><img");
                strnowrap = "";
                strnowrap = @"<p style=""margin:0px;text-align:right;""><img";
                divForm = divForm.Replace(strnowrap, @"<p style=""margin:0px;text-align:left;""><img");
                strnowrap = @"<p style=""margin:0px;text-align:right;"">";
                divForm = divForm.Replace(strnowrap, @"<p style=""margin:0px;text-align:left;"">");

                strnowrap = @"<object id=""ieooui"" classid=""clsid:38481807-CA0E-42D2-BF39-B33AF135CC4D"" />";
                divForm = divForm.Replace(strnowrap, "");
                strnowrap = @"</object>";
                divForm = divForm.Replace(strnowrap, "");

                ucEditor.Content = HttpUtility.HtmlDecode(divForm);

                Regex tblRegex = new Regex("<table[^>]*>");
                MatchEvaluator tblSize = new MatchEvaluator(ProcessTable);
                string newtableContent = tblRegex.Replace(ucEditor.Content, tblSize);
                ucEditor.Content = newtableContent;
                ucEditor.ExportSettings.FileName = ViewState["documentname"].ToString();
                ViewState["sectionname"] = ViewState["documentname"].ToString();
                ucEditor.ExportSettings.FileName = ViewState["documentname"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void GetRevisionData()
    {
        if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonsEdit(new Guid(ViewState["REVISONID"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                string strResult = HttpUtility.HtmlDecode(dr["FLDTEXT"].ToString());
                ucEditor.Content = null;
                ucEditor.Content = strResult;
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString();

                // MenuSave.Title = MenuSave.Title + " /" + dr["FLDSECTIONDETAILS"].ToString();
            }
        }
    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("WORD"))
            {
                string content = ucEditor.Content;
                Regex imgRegex = new Regex("<img[^>]*>");

                // Custom MatchEvaluator to process the image's URL and replace it with base64.
                MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);
                string newContent = imgRegex.Replace(content, imgEvaluator);

                string Filename = ViewState["sectionname"].ToString().Replace(" ", "");

                Filename = Filename.Replace(",", "");

                HtmlFormatProvider provider = new HtmlFormatProvider();
                // HtmlExportSettings exportSettings = new HtmlExportSettings();

                RadFlowDocument document = provider.Import(newContent);
                WordDownload(Filename, document);
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {

                string content = ucEditor.Content;

                // Use a regex to find all images.
                Regex imgRegex = new Regex("<img[^>]*>");

                // Custom MatchEvaluator to process the image's URL and replace it with base64.
                MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);
                string newContent = imgRegex.Replace(content, imgEvaluator);
                //string htmlContent = ucEditor.Content;
                //htmlContent = htmlContent.Replace("img src", "img height=\"450\" width=\"650\" src");
                string filename = (ViewState["sectionname"].ToString().Replace(" ", "")).Replace(",", "");

                //if (filename.Contains(","))
                //    filename = filename.Replace(",", "");
                //else if (filename.Contains("."))
                //    filename = filename.Replace(".", "");

                HtmlFormatProvider provider = new HtmlFormatProvider();
                HtmlExportSettings exportSettings = new HtmlExportSettings();

                RadFlowDocument document = provider.Import(newContent);
                PdfDownload(filename, document);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void btnInsertPic_Click(object sender, EventArgs e)
    {
        try
        {
            HttpFileCollection filecolln = Request.Files;
            //if (filecolln.Count > 0 && ucEditor.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
            //{
            if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) == null)
            {
                string strContent = ucEditor.Content;
                Guid? revisonid = Guid.Empty;
                Guid? revisiondtkey = Guid.Empty;
                PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonInsert(
                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                   , strContent
                   , new Guid(ViewState["SECTIONID"].ToString())
                   , new Guid(ViewState["DOCUMENTID"].ToString())
                   , ref revisonid
                   , ref revisiondtkey
                   );


                ViewState["REVISONID"] = revisonid;
                ViewState["REVISONDTKEY"] = revisiondtkey;
            }

            Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["REVISONDTKEY"].ToString()), PhoenixModule.DOCUMENTMANAGEMENT, null, ".jpg,.png,.gif", string.Empty, "UPLOADEDDOCIMAGE");
            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["REVISONDTKEY"].ToString()));
            DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
            if (dr.Length > 0)
            {
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                if (!path.EndsWith("/"))
                    path = path + "/";

                ucEditor.Content = ucEditor.Content + "<img src=\"" + "../Common/Download.aspx?dtkey=" + dr[0]["FLDDTKEY"].ToString() + "\" />";
                //ucEditor.Content = ucEditor.Content + "<img src=\"../Common/Download.aspx?dtkey=" + dta.Rows[0]["FLDDTKEY"].ToString(); "\" />";

            }
            //}
            else
            {
                ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidRevison(string documentid, string sectionid, string content)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(documentid) == null)
            ucError.ErrorMessage = "Document is not selected.";

        if (General.GetNullableGuid(sectionid) == null)
            ucError.ErrorMessage = "Section is not selected.";

        if (General.GetNullableString(content) == null)
            ucError.ErrorMessage = "Content is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        GetData();
    }


    protected void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        try
        {
            string fileExt = e.File.GetExtension();
            switch (fileExt)
            {
                case ".doc":
                case ".docx":
                    ucEditor.LoadDocxContent(e.File.InputStream);
                    break;
                case ".rtf":
                    ucEditor.LoadRtfContent(e.File.InputStream);
                    break;
                case ".txt":
                case ".html":
                case ".htm":
                    using (StreamReader sr = new StreamReader(e.File.InputStream))
                    {
                        ucEditor.Content = sr.ReadToEnd();
                    }
                    break;
                case ".md":
                    using (StreamReader sr = new StreamReader(e.File.InputStream))
                    {
                        ucEditor.Content = sr.ReadToEnd();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "importMarkdownScript", "TelerikDemo.setMarkdownContent();", true);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void OnFileUploadEvent(EventArgs e)
    {
        //if (FileUploadEvent != null)
        //    FileUploadEvent(this, e);
        btnInsertPic_Click(this, e);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        OnFileUploadEvent(e);
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

    public string FullyQualifiedApplicationPath()
    {

        //Return variable declaration
        var appPath = string.Empty;

        //Getting the current context of HTTP request
        var context = HttpContext.Current;

        //Checking the current context content
        if (context != null)
        {
            //Formatting the fully qualified website url/name
            appPath = string.Format("{0}://{1}{2}{3}",
            context.Request.Url.Scheme,
            context.Request.Url.Host,
            context.Request.Url.Port == 80
            ? string.Empty
            : ":" + context.Request.Url.Port,
            context.Request.ApplicationPath);
        }

        if (!appPath.EndsWith("/"))
            appPath += "/";

        return appPath;
    }

    private string ProcessMatch(Match match)
    {
        Regex srcRegex = new Regex("src=(\"|')(?!data:)[^\"']*");

        if (!srcRegex.IsMatch(match.Value))
        {
            // if it is already a base64 value, return it. 
            return match.Value;
        }

        Match srcMatch = srcRegex.Match(match.Value);
        string result = String.Empty;

        try
        {

            string urlValue = srcMatch.Value.Substring(5);
            urlValue = urlValue.Replace("../", FullyQualifiedApplicationPath());
            bool isAbsolute = urlValue.StartsWith("http", StringComparison.CurrentCultureIgnoreCase);
            Uri myUri = new Uri(urlValue);

            //Create the base64 string from the file's data.

            using (MemoryStream stream = new MemoryStream())
            {
                Uri Uri = new Uri(urlValue);
                WebClient webClient = new WebClient();
                string fileExtension = Path.GetExtension(urlValue).TrimStart('.');
                var openRead = webClient.OpenRead(urlValue);
                if (openRead != null)
                {
                    openRead.CopyTo(stream);
                    FileStream fs = GetImage(Uri);
                    byte[] bites = new byte[fs.Length];
                    fs.Read(bites, 0, bites.Length);

                    string base64ImageRepresentation = Convert.ToBase64String(bites);

                    string queryString = Uri.Query;
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);
                    string dtkey = queryDictionary["dtkey"];
                    DataTable dt = PhoenixCommonFileAttachment.EditAttachment(new Guid(dtkey));
                    if (dt.Rows.Count > 0)
                    {
                        fileExtension = Path.GetExtension(dt.Rows[0]["FLDFILENAME"].ToString()).TrimStart('.');
                    }


                    result = String.Format("data:image/{0};base64,{1}", fileExtension, base64ImageRepresentation);

                }
            }

            // Build the new src value.
            string base64SrcAttr = "src=\"" + result;

            // Replace the the old src value with the new one and return the new img tag. 
            return match.Value.Replace(srcMatch.Value, base64SrcAttr);
        }
        catch (Exception)
        {
            string fileExtension = "png";
            FileStream file = new FileStream(Server.MapPath("~/css/Theme1/images/xf_close_icon.png"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(file);
            byte[] bites = new byte[file.Length];
            file.Read(bites, 0, bites.Length);
            string base64ImageRepresentation = Convert.ToBase64String(bites);
            result = String.Format("data:image/{0};base64,{1}", fileExtension, base64ImageRepresentation);
            string base64SrcAttr = "src=\"" + result;
            return match.Value.Replace(srcMatch.Value, base64SrcAttr);
        }

    }
    private FileStream GetImage(Uri url)
    {
        FileStream file = null;
        try
        {

            if (url == null)
            {
                return file;
            }
            string queryString = url.Query;
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

            string dtkey = queryDictionary["dtkey"];
            DataTable dt = PhoenixCommonFileAttachment.EditAttachment(new Guid(dtkey));
            if (dt.Rows.Count > 0)
            {
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                string archivedpath = PhoenixGeneralSettings.CurrentGeneralSetting.ArchivedAttachmentPath;
                if (!path.EndsWith("/"))
                    path = path + "/";
                if (!archivedpath.EndsWith("/"))
                    archivedpath = archivedpath + "/";

                string _fullPath = path + dt.Rows[0]["FLDFILEPATH"].ToString();

                file = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(file);
                return file;

            }
            return file;
        }

        catch (Exception)
        {

            file = new FileStream(Server.MapPath("~/css/Theme1/images/xf_close_icon.png"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(file);
            return file;
        }
    }

    protected void ucEditor_ExportContent(object sender, EditorExportingArgs e)
    {

        ExportType exportType = e.ExportType;
        string exportedOutput = e.ExportOutput;

        Byte[] output = Encoding.Default.GetBytes(exportedOutput);

        if (exportType == ExportType.Word)
        {


            DocxFormatProvider docxProvider = new DocxFormatProvider();
            RadFlowDocument document = docxProvider.Import(output);
            string headertxt = ViewState["sectionname"].ToString();

            TextWatermarkSettings settings = new TextWatermarkSettings()
            {
                Angle = -45,
                Width = 600,
                Height = 150,
                Opacity = 0.2,
                ForegroundColor = Colors.Red,
                Text = "Uncontrolled"
            };
            Watermark textWatermark = new Watermark(settings);

            Header defaultHeader = document.Sections.First().Headers.Add();
            defaultHeader.Watermarks.Add(textWatermark);

            string content = ucEditor.Content;

            // Use a regex to find all images.
            Regex imgRegex = new Regex("<img[^>]*>");

            // Custom MatchEvaluator to process the image's URL and replace it with base64.
            MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);

            string newContent = imgRegex.Replace(content, imgEvaluator);

            Byte[] modifiedOutput = docxProvider.Export(document);
            string finalOutput = Encoding.Default.GetString(modifiedOutput, 0, modifiedOutput.Length);

            e.ExportOutput = finalOutput;
        }

        if (exportType == ExportType.Pdf)
        {
            // Use a regex to find all images.
            //PdfFormatProvider pdfProvider = new PdfFormatProvider();
            //RadFlowDocument document = pdfProvider.Import(Decoder.Default.GetBytes(exportedOutput));

            //string headertxt = ViewState["sectionname"].ToString();

            //TextWatermarkSettings settings = new TextWatermarkSettings()
            //{
            //    Angle = -45,
            //    Width = 600,
            //    Height = 150,
            //    Opacity = 0.2,
            //    ForegroundColor = Colors.Red,
            //    Text = headertxt
            //};
            //Watermark textWatermark = new Watermark(settings);

            //Header defaultHeader = document.Sections.First().Headers.Add();
            //defaultHeader.Watermarks.Add(textWatermark);

            //string content = ucEditor.Content;
            //Regex imgRegex = new Regex("<img[^>]*>");

            //// Custom MatchEvaluator to process the image's URL and replace it with base64.
            //MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);

            //string newContent = imgRegex.Replace(content, imgEvaluator);

            //Byte[] modifiedOutput = pdfProvider.Export(document);
            //string finalOutput = Encoding.Default.GetString(modifiedOutput, 0, modifiedOutput.Length);

            //e.ExportOutput = finalOutput;
        }
    }
    private void PdfDownload(string filename, RadFlowDocument document)
    {
        try
        {

            Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider pdfProvider = new Telerik.Windows.Documents.Flow.FormatProviders.Pdf.PdfFormatProvider();

            TextWatermarkSettings settings = new TextWatermarkSettings()
            {
                Angle = -45,
                Width = 600,
                Height = 150,
                Opacity = 0.3,
                ForegroundColor = Colors.Red,
                Text = "Uncontrolled"
            };

            Watermark textWatermark = new Watermark(settings);
            Header header = document.Sections[0].Headers.Add();
            string newwater = textWatermark.TextSettings.Text;
            header.Watermarks.Add(textWatermark);
            //header.Blocks.AddParagraph().TextAlignment = Telerik.Windows.Documents.Flow.Model.Styles.Alignment.Right;
            //header.Blocks.AddParagraph().Inlines.AddRun(newwater);
            document.Sections.First().PageOrientation = PageOrientation.Portrait;
            document.Sections.First().PageMargins = new Telerik.Windows.Documents.Primitives.Padding(30, 5, 30, 5);
            document.Sections.First().PageSize = PaperTypeConverter.ToSize(PaperTypes.A4);
            byte[] pdfBytes = null;
            pdfProvider.ExportSettings.ImageQuality = Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ImageQuality.High;
            pdfBytes = pdfProvider.Export(document);

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(pdfBytes);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private string ProcessTable(Match match)
    {
        Regex styleRegex = new Regex("style=*");
        Match styleMatch = styleRegex.Match(match.Value);

        if (!styleRegex.IsMatch(match.Value))
        {
            // if it is already a base64 value, return it. 
            return match.Value;
        }
        if (match.Value == @"<table style=""width:700px;"">")
        {
            // if it is already a base64 value, return it. 
            return match.Value;
        }
        string newTable = @"style='border: 1px solid #000000; width: 610px; border-collapse: collapse;' ";

        // Replace the the old src value with the new one and return the new img tag. 
        return match.Value.Replace(styleMatch.Value, newTable);

    }
    private void WordDownload(string filename, RadFlowDocument document)
    {
        try
        {
            DocxFormatProvider provider = new DocxFormatProvider();

            TextWatermarkSettings settings = new TextWatermarkSettings()
            {
                Angle = -45,
                Width = 600,
                Height = 150,
                Opacity = 0.2,
                ForegroundColor = Colors.Red,
                Text = "Uncontrolled"
            };
            Watermark textWatermark = new Watermark(settings);
            Header header = document.Sections[0].Headers.Add();
            string newwater = textWatermark.TextSettings.Text;
            header.Watermarks.Add(textWatermark);
            document.Sections.First().PageOrientation = PageOrientation.Portrait;
            document.Sections.First().PageMargins = new Telerik.Windows.Documents.Primitives.Padding(30, 5, 30, 5);
            byte[] DocxBytes = null;

            DocxBytes = provider.Export(document);
            Response.Clear();
            Response.ContentType = "Application/msword";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".docx");
            Response.BinaryWrite(DocxBytes);
            Response.Flush();
            Response.Close();
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
