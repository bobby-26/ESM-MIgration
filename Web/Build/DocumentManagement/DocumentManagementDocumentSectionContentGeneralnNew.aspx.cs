using System;
using System.Data;
using System.Web;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.Model;
using System.Net;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using System.Windows.Media;
using System.Linq;

public partial class DocumentManagement_DocumentManagementDocumentSectionContentGeneralnNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        ucEditor.RibbonBar.SelectedTabIndex = 1;

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("Export PDF", "PDF", ToolBarDirection.Right);
        toolbar.AddButton("Export DOCX", "WORD", ToolBarDirection.Right);


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
            ucEditor.SpellCheckSettings.AllowAddCustom = true;
            ucEditor.SpellCheckSettings.SpellCheckProvider = SpellCheckProvider.PhoneticProvider;
            
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

            if (Request.QueryString["REVISONID"] != null && Request.QueryString["REVISONID"].ToString() != "")
            {
                ViewState["REVISONID"] = Request.QueryString["REVISONID"].ToString();
                //GetRevisionData();
            }
            else
            {
                ViewState["REVISONID"] = "";
                //GetData(); 
            }
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbar.Show();

            GetData();
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() != "")
                ViewState["callfrom"] = Request.QueryString["callfrom"].ToString();
            else
                ViewState["callfrom"] = "";
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
                MenuSave.Title = dr["FLDDOCUMENTNAME"].ToString();
            }
        }
    }

    private void GetData()
    {
        if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionEdit(new Guid(ViewState["SECTIONID"].ToString()));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                string strResult = HttpUtility.HtmlDecode(dr["FLDCURRENTCONTENT"].ToString());

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

                ucEditor.Content = strResult;
                ucEditor.ExportSettings.FileName = dr["FLDSECTIONDETAILS"].ToString();
                ViewState["sectionname"] = ucEditor.ExportSettings.FileName = dr["FLDSECTIONDETAILS"].ToString();
                ViewState["dtkey"] = dr["FLDDTKEY"].ToString();

                MenuSave.Title = MenuSave.Title + " /" + dr["FLDSECTIONDETAILS"].ToString();
            }
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

                MenuSave.Title = MenuSave.Title + " /" + dr["FLDSECTIONDETAILS"].ToString();
            }
        }
    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string strContent = ucEditor.Content;

                if (!IsValidRevison(
                      ViewState["DOCUMENTID"].ToString()
                    , ViewState["SECTIONID"].ToString()
                    , strContent)
                    )
                {
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableGuid(ViewState["REVISONID"].ToString()) == null)
                {
                    Guid? revisonid = Guid.Empty;
                    Guid? revisiondtkey = Guid.Empty;
                    Guid? sectionid = Guid.Empty;


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

                    ucStatus.Text = "Changes are updated.";

                    string scriptRefreshDontClose = "";
                    scriptRefreshDontClose += "<script language='javaScript' id='View'>" + "\n";
                    scriptRefreshDontClose += "fnReloadList('VesselOtherCompany', 'refreshiframe', 'keepOpen');";
                    scriptRefreshDontClose += "</script>" + "\n";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddressAddNew", scriptRefreshDontClose, false);
                }
                else
                {
                    PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonUpdate(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , strContent
                            , new Guid(ViewState["SECTIONID"].ToString())
                            , new Guid(ViewState["DOCUMENTID"].ToString())
                            , new Guid(ViewState["REVISONID"].ToString())
                            );

                    ucStatus.Text = "Changes are updated.";

                    string scriptRefreshDontClose = "";
                    scriptRefreshDontClose += "<script language='javaScript' id='View'>" + "\n";
                    scriptRefreshDontClose += "fnReloadList('VesselOtherCompany', 'refreshiframe', 'keepOpen');";
                    scriptRefreshDontClose += "</script>" + "\n";

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "AddressAddNew", scriptRefreshDontClose, false);
                }
                GetData();
            }
            if (CommandName.ToUpper().Equals("WORD"))
            {
                string content = ucEditor.Content;

                // Use a regex to find all images.
                Regex imgRegex = new Regex("<img[^>]*>");

                // Custom MatchEvaluator to process the image's URL and replace it with base64.
                MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);
                string newContent = imgRegex.Replace(content, imgEvaluator);
                ucEditor.Content = newContent;
                ucEditor.ExportToDocx();
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {
                string content = ucEditor.Content;


                Regex imgRegex = new Regex("<img[^>]*>");

                // Custom MatchEvaluator to process the image's URL and replace it with base64.
                MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);

                string newContent = imgRegex.Replace(content, imgEvaluator);
                ucEditor.Content = newContent;
                ucEditor.ExportSettings.Pdf.PageHeader.MiddleCell.Text = ViewState["sectionname"].ToString();
                ucEditor.ExportToPdf();
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

                //string url = string.Format("{0}{1}", FullyQualifiedApplicationPath(), "/Common/Download.aspx?dtkey=" + dr[0]["FLDDTKEY"].ToString());
                //ucEditor.Content = ucEditor.Content + "<img src=\"" + url + "\" ></img>";
                ucEditor.Content = ucEditor.Content + "<img src=\"" + "../Common/Download.aspx?dtkey=" + dr[0]["FLDDTKEY"].ToString() + "\" />";

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

                default:
                    RadNotification1.Show("The selected file is invalid. Please upload an MS Word document with an extension .doc, .docx !");
                    break;
            }
        }
        catch (Exception ex)
        {
            RadNotification1.Show("There was an error during the import operation. Try simplifying the content.");
            ucError.ErrorMessage = ex.Message;
        }
    }
    protected void OnFileUploadEvent(EventArgs e)
    {
        //if (FileUploadEvent != null)
        //    FileUploadEvent(this, e);
        btnInsertPic_Click(this, e);
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

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        OnFileUploadEvent(e);
    }
    protected void RadEditor1_ExportContent(object sender, EditorExportingArgs e)
    {

        ExportType exportType = e.ExportType;
        string exportedOutput = e.ExportOutput;

        Byte[] output = Encoding.Default.GetBytes(exportedOutput);

        if (exportType == ExportType.Word)
        {


            DocxFormatProvider docxProvider = new DocxFormatProvider();
            RadFlowDocument document = docxProvider.Import(output);
            //string headertxt = ViewState["sectionname"].ToString();

            //TextWatermarkSettings settings = new TextWatermarkSettings()
            //{
            //    Angle = -45,
            //    Width = 600,
            //    Height = 150,
            //    Opacity = 0.2,
            //    ForegroundColor = Colors.Red,
            //    Text = "Uncontrolled"
            //};
            //Watermark textWatermark = new Watermark(settings);

            //Header defaultHeader = document.Sections.First().Headers.Add();
            //defaultHeader.Watermarks.Add(textWatermark);

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
}