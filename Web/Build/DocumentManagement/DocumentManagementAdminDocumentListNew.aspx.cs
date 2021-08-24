using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

public partial class DocumentManagement_DocumentManagementAdminDocumentListNew : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        //SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentListNew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocument')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementAdminDocumentListNewAdd.aspx?pageno=" + gvDocument.CurrentPageIndex, "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDFORM");

            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementAdminDocumentList.aspx", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementAdminDocumentList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuDocumentList.AccessRights = this.ViewState;
            MenuDocumentList.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Section", "SECTION", ToolBarDirection.Right);
            toolbar.AddButton("Document", "DOCUMENT", ToolBarDirection.Right);
            //toolbar.AddButton("Content", "CONTENT");                

            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                Filter.CurrentDMSDocumentSectionFilter = null;
                // gvDocument.PageSize = General.ShowRecords(gvDocument.PageSize);
                ViewState["pageno"] = string.Empty;
                ViewState["pagesize"] = string.Empty;

                if (Request.QueryString["pageno"] != null && General.GetNullableInteger(Request.QueryString["pageno"].ToString()) != null)
                    gvDocument.CurrentPageIndex = int.Parse(Request.QueryString["pageno"].ToString());
                //Page = null;                

                if (Request.QueryString["pagesize"] != null && Request.QueryString["pagesize"].ToString() != "")
                {
                    ViewState["pagesize"] = Request.QueryString["pagesize"].ToString();
                    gvDocument.PageSize = Int32.Parse(ViewState["pagesize"].ToString());
                }
                else
                {
                    gvDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
                else
                    ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDDOCUMENTNAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDCOMPANYSHORTCODE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDPUBLISHEDYN" };
        string[] alCaptions = { "S.No", "Name", "Category", "Active Y/N", "Company", "Added Date", "Added By", "Revision", "PublishedDate", "PublishedYN" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentDMSDocumentFilter == null || Filter.CurrentDMSDocumentFilter.ToString() != General.GetNullableString(txtDocumentName.Text.Trim()))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Add("txtDocumentName", General.GetNullableString(txtDocumentName.Text.Trim()));
            Filter.CurrentDMSDocumentFilter = criteria;
        }

        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;
        string strDocumentDetails = (nvc != null && nvc.Get("txtDocumentName") != null) ? nvc.Get("txtDocumentName").ToString() : "";

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementDocument.DocumentSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(null)
                                                                , General.GetNullableString(strDocumentDetails)
                                                                , null
                                                                , null
                                                                , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=Documents.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Documents</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuDocumentList_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            gvDocument.CurrentPageIndex = 0;
            gvDocument.Rebind();
        }
        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentDMSDocumentFilter = null;
            ViewState["DOCUMENTID"] = "";
            gvDocument.Rebind();
            txtDocumentName.Text = "";
        }
    }

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SECTION"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionListNew.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&pageno=" + gvDocument.CurrentPageIndex.ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvDocument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDDOCUMENTNAME", "FLDCATEGORYNAME", "FLDACTIVESTATUS", "FLDCOMPANYSHORTCODE", "FLDDATE", "FLDAUTHORNAME", "FLDREVISIONDETAILS", "FLDAPPROVEDDATE", "FLDPUBLISHEDYN" };
        string[] alCaptions = { "S.No", "Name", "Category", "Active Y/N", "Company", "Added Date", "Added By", "Revision", "PublishedDate", "PublishedYN" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        if (Filter.CurrentDMSDocumentFilter == null || General.GetNullableString(txtDocumentName.Text.Trim()) != null)
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Add("txtDocumentName", General.GetNullableString(txtDocumentName.Text.Trim()));
            Filter.CurrentDMSDocumentFilter = criteria;
        }

        NameValueCollection nvc = Filter.CurrentDMSDocumentFilter;
        string strDocumentDetails = (nvc != null && nvc.Get("txtDocumentName") != null) ? nvc.Get("txtDocumentName").ToString() : "";

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementDocument.DocumentSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(null)
                                                                , General.GetNullableString(strDocumentDetails)
                                                                , null
                                                                , null
                                                                , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvDocument.CurrentPageIndex + 1
                                                                , gvDocument.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , companyid
                                                                );

        General.SetPrintOptions("gvDocument", "Documents", alCaptions, alColumns, ds);


        gvDocument.DataSource = ds;
        gvDocument.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()) == null)
            {
                ViewState["DOCUMENTID"] = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
            }
        }
        SetRowSelection();
    }

    protected void gvDocument_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("CHANGEPAGESIZE"))
                return;

            int nCurrentRow = e.Item.ItemIndex;

            //gvDocument.SelectedIndexes.Clear();            
            if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Guid documentid = new Guid(item.GetDataKeyValue("FLDDOCUMENTID").ToString());
                gvDocument.SelectedIndexes.Add(e.Item.ItemIndex);
                BindPageURL(e.Item.ItemIndex);
            }
            //if (e.CommandName.ToUpper().Equals("EDIT"))
            //{
            //    BindPageURL(nCurrentRow);
            //}
            else if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                BindPageURL(nCurrentRow);
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentSectionListNew.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"] + "&pageno=" + gvDocument.CurrentPageIndex.ToString() + "&pagesize=" + gvDocument.PageSize+ "&Wholemanualread=" + ((RadLabel)item.FindControl("lblWholemanualreadyn")).Text);
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidDocument(
                    ((UserControlMaskNumber)item.FindControl("txtSequenceNumberAdd")).Text
                     , ((RadTextBox)item.FindControl("txtDocumentNameAdd")).Text
                     , ((RadTextBox)item.FindControl("txtCategoryidAdd")).Text
                     , ((UserControlCompany)item.FindControl("ucCompanyAdd")).SelectedCompany
                     ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertDocument(
                    ((RadTextBox)item.FindControl("txtDocumentNameAdd")).Text
                    , ((RadTextBox)item.FindControl("txtCategoryidAdd")).Text
                    , (((RadCheckBox)item.FindControl("chkActiveYNAdd")).Checked == true) ? 1 : 0
                    , 0
                    , int.Parse(((UserControlMaskNumber)item.FindControl("txtSequenceNumberAdd")).Text)
                    , General.GetNullableInteger(((UserControlCompany)item.FindControl("ucCompanyAdd")).SelectedCompany)
                );
                ucStatus.Text = "Document is added.";
                ViewState["DOCUMENTID"] = "";
                gvDocument.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(nCurrentRow);

                RadLabel lblPublishedDate = ((RadLabel)eeditedItem.FindControl("lblPublishedDate"));
                UserControlDate ucPublishedDateEdit = ((UserControlDate)eeditedItem.FindControl("ucPublishedDateEdit"));
                RadLabel lblDocumentId = ((RadLabel)eeditedItem.FindControl("lblDocumentId"));

                PhoenixDocumentManagementDocument.DocumentRevisionApprove(
                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , General.GetNullableGuid(lblDocumentId.Text)
                                         , General.GetNullableDateTime(lblPublishedDate.Text)
                                         , General.GetNullableGuid(((RadLabel)gvDocument.Items[nCurrentRow].FindControl("lblRevisionId")).Text)
                                         );

                ucStatus.Text = "Document is published.";
                gvDocument.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteDocument((((RadLabel)eeditedItem.FindControl("lblDocumentId")).Text));
                ViewState["DOCUMENTID"] = "";
                ucStatus.Text = "Document is deleted.";
                gvDocument.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
                LinkButton cmdedit = (LinkButton)e.Item.FindControl("cmdEdit");
                //cmdedit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFormListDetailsUpdate.aspx?FORMID=" + ViewState["FORMID"].ToString() + "'); return false;");
                Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentListNewUpdate.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"].ToString() + "&pageno=" + gvDocument.CurrentPageIndex.ToString() + "&Revisionid=" + ((RadLabel)gvDocument.Items[nCurrentRow].FindControl("lblRevisionId")).Text + "&pagesize=" + gvDocument.PageSize);
            }

            if (e.CommandName.ToUpper().Equals("BASE64"))
            {
                BindPageURL(nCurrentRow);
                UbdateBase64Data((((RadLabel)eeditedItem.FindControl("lblDocumentId")).Text));
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UbdateBase64Data(String Documentid)
    {
        if (Documentid != null && Documentid.ToString() != "")
        {
            DataSet ds = PhoenixDocumentManagementDocument.NewDocumentData(new Guid(Documentid.ToString())
                , General.GetNullableInteger(null)
                 , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                 , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                );

            if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)     //to bind document sections(index)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FLDSECTIONID"].ToString().ToLower() != ViewState["DOCUMENTID"].ToString().ToLower())
                    {
                        string orignaldata = HttpUtility.HtmlDecode(dr["FLDNEWTEXT"].ToString());
                        Regex imgRegex = new Regex("<img[^>]*>");

                        MatchEvaluator imgEvaluator = new MatchEvaluator(ProcessMatch);
                        string newContent = imgRegex.Replace(orignaldata, imgEvaluator);

                        MatchEvaluator imgSize = new MatchEvaluator(ProcessImage);
                        string newImageContent = imgRegex.Replace(newContent, imgSize);

                        Regex tblRegex = new Regex("<table[^>]*>");
                        MatchEvaluator tblSize = new MatchEvaluator(ProcessTable);
                        string newtableContent = tblRegex.Replace(newImageContent, tblSize);
                        newtableContent = HttpUtility.HtmlEncode(newtableContent);

                        try
                        {
                            PhoenixDocumentManagementDocument.DocumentSectionImagebase64Update(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                , new Guid(Documentid.ToString())
                                                                                                , new Guid(dr["FLDSECTIONID"].ToString())
                                                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                                , General.GetNullableString(newtableContent));
                        }
                        catch (Exception ex)
                        {
                            ucError.ErrorMessage = ex.Message;
                            ucError.Visible = true;
                        }

                    }
                }
            }
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
        //MatchEvaluator imgSize = new MatchEvaluator(ProcessImage);
        
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
                string _targetPath = HttpContext.Current.Server.MapPath("..\\Attachments\\TEMP\\")+ dt.Rows[0]["FLDDTKEY"].ToString()+'.'+ Path.GetExtension(dt.Rows[0]["FLDFILENAME"].ToString()).TrimStart('.');

                ReduceImageSize(_fullPath, _targetPath);

                file = new FileStream(_targetPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);                
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

    private static void ReduceImageSize(string sourcePath, string targetPath)
    {
        using (var image = System.Drawing.Image.FromFile(sourcePath))
        {
            var Width = (int)(image.Width);
            var Height = (int)(image.Height);

            var newWidth = 560;
            var newHeight = 600;

            if (Width > newWidth)
                Width = newWidth;

            if (Height > newHeight)
                Height = newHeight;

            var thumbnailImg = new Bitmap(Width, Height);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            thumbnailImg.Save(targetPath, image.RawFormat);
            image.Dispose();


        }
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

    protected void gvDocument_RowDeleting(object sender, GridDeletedEventArgs de)
    {
        gvDocument.Rebind();
    }

    protected void gvDocument_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;

            if (!IsValidDocument(
                     ((UserControlMaskNumber)item.FindControl("txtSequenceNumberEdit")).Text,
                     ((RadTextBox)item.FindControl("txtDocumentNameEdit")).Text,
                     ((RadTextBox)item.FindControl("txtCategoryidEdit")).Text,
                     ((UserControlCompany)item.FindControl("ucCompanyEdit")).SelectedCompany
                     ))
            {
                ucError.Visible = true;
                return;
            }
            UpdateDocument(
                  ((RadTextBox)item.FindControl("txtDocumentNameEdit")).Text
                , ((RadTextBox)item.FindControl("txtCategoryidEdit")).Text
                , (((RadCheckBox)item.FindControl("chkActiveYNEdit")).Checked == true) ? 1 : 0
                , ((RadLabel)item.FindControl("lblDocumentIdEdit")).Text
                , 0
                , int.Parse(((UserControlMaskNumber)item.FindControl("txtSequenceNumberEdit")).Text)
                , General.GetNullableInteger(((UserControlCompany)item.FindControl("ucCompanyEdit")).SelectedCompany)
                , General.GetNullableDateTime(((UserControlDate)item.FindControl("ucPublishedDateEdit")).Text)
                , ((RadLabel)item.FindControl("lblRevisionId")).Text
             );

            ucStatus.Text = "Document details updated.";
            //_gridView.EditIndex = -1;
            gvDocument.Rebind();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDocument_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;

            LinkButton cmdDelete = (LinkButton)item.FindControl("cmdDelete");
            LinkButton cmdApprove = (LinkButton)item.FindControl("cmdApprove");

            LinkButton cmdbase64 = (LinkButton)item.FindControl("cmdbase64");
            if (cmdbase64 != null && !SessionUtil.CanAccess(this.ViewState, cmdbase64.CommandName))
                cmdbase64.Visible = false;

            if (!e.Item.IsInEditMode)
            {
                if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton cmdEdit = (LinkButton)item.FindControl("cmdEdit");
            LinkButton cmdSave = (LinkButton)item.FindControl("cmdSave");
            LinkButton cmdCancel = (LinkButton)item.FindControl("cmdCancel");

            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            if (cmdApprove != null)
            {
                cmdApprove.Attributes.Add("onclick", "return fnConfirmTelerik(event,'Are you sure you want to Publish this document? Y/N'); return false;");
            }

            RadDropDownList ddlDocumentCategoryEdit = (RadDropDownList)e.Item.FindControl("ddlDocumentCategoryEdit");
            DataRowView dv = (DataRowView)e.Item.DataItem;
            LinkButton btnShowCategoryEdit = (LinkButton)e.Item.FindControl("btnShowCategoryEdit");
            if (btnShowCategoryEdit != null)
            {
                btnShowCategoryEdit.Attributes.Add("onclick", "return showPickList('spnPickListCategoryedit', 'codehelp1', '', '../Common/CommonPickListDocumentCategory.aspx', true); ");
            }
            RadTextBox txtCategoryEdit = (RadTextBox)e.Item.FindControl("txtCategoryEdit");
            if (txtCategoryEdit != null)
                txtCategoryEdit.Text = dv["FLDCATEGORYNAME"].ToString();
            RadTextBox txtCategoryidEdit = (RadTextBox)e.Item.FindControl("txtCategoryidEdit");
            if (txtCategoryidEdit != null)
                txtCategoryidEdit.Text = dv["FLDCATEGORYID"].ToString();

            if (ddlDocumentCategoryEdit != null)
            {
                ddlDocumentCategoryEdit.Items.Clear();
                ddlDocumentCategoryEdit.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
                ddlDocumentCategoryEdit.DataTextField = "FLDCATEGORYNAME";
                ddlDocumentCategoryEdit.DataValueField = "FLDCATEGORYID";
                ddlDocumentCategoryEdit.DataBind();

                if (dv != null)
                    ddlDocumentCategoryEdit.SelectedValue = dv["FLDCATEGORYID"].ToString();
            }

            RadLabel lblDocumentId = (RadLabel)e.Item.FindControl("lblDocumentId");
            RadLabel lblRevisionId = (RadLabel)e.Item.FindControl("lblRevisionId");

            LinkButton cmgViewContent = (LinkButton)e.Item.FindControl("cmgViewContent");
            LinkButton cmdUpload = (LinkButton)e.Item.FindControl("cmdUpload");


            //if (cmdUpload != null)
            //{
            //    if (dv["FLDDOCUMENTTYPE"] != null && dv["FLDDOCUMENTTYPE"].ToString() == "1")
            //        cmdUpload.Visible = true;
            //    //cmdUpload.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../DocumentManagement/DocumentManagementFormUpload.aspx?formid=" + dv["FLDDOCUMENTID"].ToString() + "&formrevisionid=" + dv["FLDDRAFTREVISIONID"].ToString() + "&formrevisiondtkey=" + dv["FLDDRAFTREVISIONDTKEY"].ToString() + "','medium'); return false;");
            //}

            if (cmgViewContent != null && lblDocumentId != null)
                cmgViewContent.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp','','DocumentManagement/DocumentManagementDocumentGeneralNew.aspx?DOCUMENTID=" + lblDocumentId.Text + "'); return false;");

            //if (cmdApprove != null && lblDocumentId != null)
            //    cmdApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','../DocumentManagement/DocumentManagementDocumentPublishedDate.aspx?DOCUMENTID=" + lblDocumentId.Text + "&REVISIONID=" + lblRevisionId.Text + "','medium'); return false;");


            if (cmgViewContent != null && !SessionUtil.CanAccess(this.ViewState, cmgViewContent.CommandName))
                cmgViewContent.Visible = false;
            if (cmdApprove != null && !SessionUtil.CanAccess(this.ViewState, cmdApprove.CommandName))
                cmdApprove.Visible = false;
            if (cmdEdit != null && !SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName))
                cmdEdit.Visible = false;
            if (cmdDelete != null && !SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName))
                cmdDelete.Visible = false;

            RadioButtonList rListEdit = (RadioButtonList)e.Item.FindControl("rListEdit");
            if (rListEdit != null)
            {
                if (dv != null && dv["FLDDOCUMENTTYPE"].ToString() == "0")
                    rListEdit.Items[0].Selected = true;
                if (dv != null && dv["FLDDOCUMENTTYPE"].ToString() == "1")
                    rListEdit.Items[1].Selected = true;
            }

            LinkButton cmdRevision = (LinkButton)e.Item.FindControl("cmdRevision");
            if (cmdRevision != null)
            {
                cmdRevision.Attributes.Add("onclick", "javascript:openNewWindow('Filter','Document Revisions','DocumentManagement/DocumentManagementAdminDocumentRevisionList.aspx?DOCUMENTID=" + dv["FLDDOCUMENTID"] + "'); return false;");
            }
            if (cmdRevision != null && !SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName))
                cmdRevision.Visible = false;

            UserControlToolTip ucDocumentName = (UserControlToolTip)e.Item.FindControl("ucDocumentName");
            LinkButton lnkDocumentName = (LinkButton)e.Item.FindControl("lnkDocumentName");
            if (lnkDocumentName != null)
            {
                ucDocumentName.Position = ToolTipPosition.BottomCenter;
                ucDocumentName.TargetControlId = lnkDocumentName.ClientID;
                //lnkDocumentName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDocumentName.ToolTip + "', 'visible');");
                //lnkDocumentName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDocumentName.ToolTip + "', 'hidden');");
            }

            UserControlCompany ucCompanyEdit = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            if (ucCompanyEdit != null)
            {
                ucCompanyEdit.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompanyEdit.SelectedCompany = dv["FLDCOMPANYID"].ToString();
            }

            LinkButton cmdVesselList = (LinkButton)item.FindControl("cmdVesselList");
            if (cmdVesselList != null)
            {
                cmdVesselList.Attributes.Add("onclick", "javascript:return openNewWindow('Document', '', 'DocumentManagement/DocumentManagementVesselListByDocument.aspx?DOCUMENTID=" + dv["FLDDOCUMENTID"].ToString() + "&type=Document'); return false;");
            }

            LinkButton cmdForms = (LinkButton)e.Item.FindControl("cmdForms");
            if (cmdForms != null)
            {
                cmdForms.Attributes.Add("onclick", "openNewWindow('Forms', 'Forms and Checklist', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentLinkedForms.aspx?DOCUMENTID=" + lblDocumentId.Text + "',false,500,300);return false;");
            }

            LinkButton lnkReadUnread = (LinkButton)e.Item.FindControl("lnkReadUnread");
            if (lnkReadUnread != null)
            {
                SessionUtil.CanAccess(this.ViewState, lnkReadUnread.CommandName);
                lnkReadUnread.Attributes.Add("onclick", "openNewWindow('ReadUnread', '" + lnkDocumentName.Text + " - Read/Unread User List', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionReadUnreadList.aspx?DOCUMENTID=" + dv["FLDDOCUMENTID"].ToString() + "');return false;");
            }

            LinkButton cmdConfig = (LinkButton)e.Item.FindControl("cmdConfig");
            if (dv != null && dv["FLDWHOLEMANUALREADYN"].ToString() == "1")
            {
                cmdConfig.Visible = true;
            }
            else
            {
                cmdConfig.Visible = false;
            }
            if (cmdConfig != null)
            {
                SessionUtil.CanAccess(this.ViewState, cmdConfig.CommandName);
                cmdConfig.Attributes.Add("onclick", "openNewWindow('Configuration', 'Configuration (" + lnkDocumentName.Text + ")', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementConfigurationNew.aspx?DOCUMENTID=" + lblDocumentId.Text + "');return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton btnShowCategoryAdd = (LinkButton)e.Item.FindControl("btnShowCategoryAdd");
            if (btnShowCategoryAdd != null)
            {
                btnShowCategoryAdd.Attributes.Add("onclick", "return showPickList('spnPickListCategoryAdd', 'codehelp1', '', '../Common/CommonPickListDocumentCategory.aspx', true); ");
            }

            RadDropDownList ddlDocumentCategoryAdd = (RadDropDownList)item.FindControl("ddlDocumentCategoryAdd");
            if (ddlDocumentCategoryAdd != null)
            {
                ddlDocumentCategoryAdd.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
                ddlDocumentCategoryAdd.DataTextField = "FLDCATEGORYNAME";
                ddlDocumentCategoryAdd.DataValueField = "FLDCATEGORYID";
                ddlDocumentCategoryAdd.DataBind();
            }
            UserControlCompany ucCompanyAdd = (UserControlCompany)item.FindControl("ucCompanyAdd");
            if (ucCompanyAdd != null)
            {
                ucCompanyAdd.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompanyAdd.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            }
        }

        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            item.DataCell.Text = groupDataRow["Category"].ToString();
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["DOCUMENTID"] = ((RadLabel)gvDocument.Items[rowindex].FindControl("lblDocumentId")).Text;
            //Filter.CurrentSelectedBulkOrderId = ((Label)gvDocument.Rows[rowindex].FindControl("lblOrderId")).Text;       
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
    }

    private void InsertDocument(string documentname, string categorid, int? activyn, int? documenttype, int serialnumber, int? companyid)
    {
        PhoenixDocumentManagementDocument.DocumentInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentname
            , new Guid(categorid)
            , activyn
            , documenttype
            , serialnumber
            , companyid
            );
    }

    private void UpdateDocument(string documentname, string categorid, int? activyn, string documentid, int? documenttype, int serialnumber, int? companyid, DateTime? publisheddate, string revisionid)
    {

        PhoenixDocumentManagementDocument.DocumentUpdate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentname
            , new Guid(categorid)
            , activyn
            , new Guid(documentid)
            , documenttype
            , serialnumber
            , companyid
            , publisheddate
            , General.GetNullableGuid(revisionid.ToString())
            );
    }

    private void DeleteDocument(string documentid)
    {
        PhoenixDocumentManagementDocument.DocumentDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(documentid));
    }

    private bool IsValidDocument(string serialnumber, string documentname, string categoryid, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(serialnumber) == null)
            ucError.ErrorMessage = "Serial Number is required.";

        if (General.GetNullableString(documentname) == null)
            ucError.ErrorMessage = "Document name is required.";

        if (General.GetNullableGuid(categoryid) == null)
            ucError.ErrorMessage = "Document category is required.";

        if (General.GetNullableInteger(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetRowSelection()
    {
        foreach (GridDataItem item in gvDocument.Items)
        {
            if (item.GetDataKeyValue("FLDDOCUMENTID").ToString().Equals(ViewState["DOCUMENTID"].ToString()))
            {
                gvDocument.SelectedIndexes.Add(item.ItemIndex);
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //gvDocument.CurrentPageIndex = 0;
        gvDocument.Rebind();
    }
    private string ProcessImage(Match match)
    {
        Regex srcRegex = new Regex("src=*");
        Regex srcBase64Regex = new Regex("src=(\"|')(?!data:)[^\"']*");
        Match srcMatch = srcRegex.Match(match.Value);
        Regex styleRegex = new Regex("style=*");
        Match styleMatch = styleRegex.Match(match.Value);
        string oldwidthstring = string.Empty;
        string newwidthstring = string.Empty;
        string oldheightstring = string.Empty;
        string newheightstring = string.Empty;
        string Finalresult = string.Empty;

        string newstyle = string.Empty;
        int width, height;

        if (!styleRegex.IsMatch(match.Value))
        {
            string res = Regex.Replace(match.Value, "data:image\\/\\w+\\;base64\\,", "");
            //Match the img tag and get the base64  string value
            string matchString = Regex.Match(res, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

            byte[] image = Convert.FromBase64String(matchString);
            using (var ms = new MemoryStream(image))
            {
                System.Drawing.Image imgs = System.Drawing.Image.FromStream(ms);
                width = imgs.Width -1;
                height = imgs.Height -1;
            }

            if (width > 560)
                width = 560;

            if (height > 600)
                height = 600;

            newstyle = @"style='width:" + width + "px;height:" + height + "px;' " + srcMatch.Value;
            Finalresult =  match.Value.Replace(srcMatch.Value, newstyle);
        }
        if (!srcBase64Regex.IsMatch(match.Value) && styleRegex.IsMatch(match.Value))
        {
            string res = Regex.Replace(match.Value, "data:image\\/\\w+\\;base64\\,", "");
            //Match the img tag and get the base64  string value
            string matchString = Regex.Match(res, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

            byte[] image = Convert.FromBase64String(matchString);
            using (var ms = new MemoryStream(image))
            {
                System.Drawing.Image imgs = System.Drawing.Image.FromStream(ms);
                width = imgs.Width - 1;
                height = imgs.Height - 1;
            }

            oldwidthstring = width.ToString() + "px";
            oldheightstring = height.ToString() + "px";

            if (width > 560)
                width = 560;

            if (height > 600)
                height = 600;

            newwidthstring = width.ToString() + "px";
            newheightstring = height.ToString() + "px";

            //newstyle = @"style='width:" + width + "px;height:" + height + "px;'";
            string newimage = match.Value.Replace(oldwidthstring, newwidthstring);
            newimage = match.Value.Replace(oldheightstring, newheightstring);
            // return match.Value.Replace(styleMatch.Value, newstyle);
            Finalresult = newimage;

        }
        return Finalresult;
    }
    public static Bitmap ResizeImage(System.Drawing.Image image)
    {
        int width = image.Width - 1;
        int height = image.Height - 1;

        if (width > 560)
            width = 560;

        if (height > 600)
            height = 600;

        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(destImage, destRect, 0, 0, width, height, GraphicsUnit.Pixel, wrapMode);
                graphics.DrawImage(destImage, 0, 0, width, height);
            }
        }

        return destImage;
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
}
