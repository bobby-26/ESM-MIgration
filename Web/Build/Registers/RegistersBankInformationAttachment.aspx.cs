using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using System.IO;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using Telerik.Web.UI;

public partial class Registers_RegistersBankInformationAttachment : PhoenixBasePage
{
    string attachmentcode = string.Empty;
    PhoenixModule module;
    string cmdname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Filter.CurrentMenuCodeSelection != null)
            SessionUtil.PageAccessRights(this.ViewState);

        cmdname = (Request.QueryString["cmdname"] == null ? "UPLOAD" : Request.QueryString["cmdname"].ToUpper());
        if (!string.IsNullOrEmpty(Request.QueryString["dtkey"]) && !string.IsNullOrEmpty(Request.QueryString["mod"]))
        {
            attachmentcode = Request.QueryString["dtkey"];
            module = (PhoenixModule)(Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]));
        }
        else
        {
            string msg = string.IsNullOrEmpty(Request.QueryString["dtkey"]) ? "Select a record to add attachment" : string.Empty;
            msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
            ucError.ErrorMessage = msg;
            ucError.Visible = true;
        }
        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(Request.QueryString["U"]) || Request.QueryString["ratingyn"] == "1")
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Export to Pdf", "E2PDF", ToolBarDirection.Right);
                toolbarmain.AddButton("Upload", cmdname, ToolBarDirection.Right);
                AttachmentList.AccessRights = this.ViewState;
                AttachmentList.MenuList = toolbarmain.Show();
            }
            if (Request.QueryString["maxnoofattachments"] != null && Request.QueryString["maxnoofattachments"].ToString() != "")
                ViewState["maxnoofattachments"] = Request.QueryString["maxnoofattachments"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            //gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["BYVESSEL"] = (string.IsNullOrEmpty(Request.QueryString["BYVESSEL"]) ? string.Empty : Request.QueryString["BYVESSEL"]);
            if (!string.IsNullOrEmpty(Request.QueryString["maxreqlen"]))
            {
                ucError.ErrorMessage = "File is too large to Upload. Maximum Upload Size is " + Math.Round(int.Parse(Request.QueryString["maxreqlen"].ToString()) / 1048576.00 * 100000.00) / 100000 + " MB";
                ucError.Visible = true;
            }
        }
        // Rebind();
    }
    protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToString().ToUpper() == cmdname)
        {
            if (!string.IsNullOrEmpty(attachmentcode))
            {

                try
                {
                    int? vesselid = null;
                    string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;

                    if (ViewState["maxnoofattachments"] != null && ViewState["maxnoofattachments"].ToString() != "")
                    {
                        if (ViewState["ROWCOUNT"].ToString().Equals(ViewState["maxnoofattachments"]))
                        {
                            ucError.ErrorMessage = "You cannot upload more than " + ViewState["maxnoofattachments"] + " attachments.";
                            ucError.Visible = true;
                            return;
                        }
                    }
                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);

                    //RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    //             "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                    RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                         "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                    Rebind();

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }

            }
            else
            {
                string msg = "Select a record to add attachment";
                msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
                ucError.ErrorMessage = msg;
                ucError.Visible = true;
            }
        }
        else if (CommandName.ToString().ToUpper() == "E2PDF")
        {
            try
            {
                if (gvAttachment.Items.Count > 0)
                {
                    List<String> FilePath = new List<string>();

                    for (int i = 0; i < gvAttachment.Items.Count; i++)
                    {
                        GridDataItem row = gvAttachment.Items[i];
                        bool isChecked = ((CheckBox)row.FindControl("chkExport")).Checked;

                        if (isChecked)
                        {
                            RadLabel path = ((RadLabel)row.FindControl("lblFilePath"));

                            string pathName = path.Text.ToLower();
                            string ext = Path.GetExtension(pathName).ToLower();
                            if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp" || ext == ".pdf")
                            {
                                // string file  = Session["sitepath"] + "/Attachments/" + pathName;
                                string filename = Server.MapPath("~/Attachments/" + pathName);
                                bool exists = File.Exists(filename);
                                if (exists == true)
                                {
                                    FilePath.Add(filename);
                                }
                            }
                        }
                    }
                    if (FilePath.Count > 0)
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment;Filename=Attachment.pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);

                        iTextSharp.text.Document doc = new iTextSharp.text.Document();
                        iTextSharp.text.Rectangle pageSize = null;
                        var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
                        doc.Open();

                        foreach (string filename in FilePath)
                        {
                            if (Path.GetExtension(filename).ToLower() == ".pdf")
                            {
                                var cb = writer.DirectContent;
                                var reader = new PdfReader(filename);
                                int i = 0;
                                while (i < reader.NumberOfPages)
                                {
                                    i++;
                                    doc.SetPageSize(reader.GetPageSizeWithRotation(1));
                                    doc.NewPage();
                                    var page = writer.GetImportedPage(reader, i);
                                    var rotation = reader.GetPageRotation(i);

                                    if (rotation == 90 || rotation == 270)
                                    {
                                        cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                                    }
                                    else
                                    {
                                        cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                                    }
                                }
                            }
                            else
                            {
                                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(filename);
                                pageSize = new iTextSharp.text.Rectangle(0, 0, jpg.Width, jpg.Height);
                                doc.SetPageSize(pageSize);
                                float pageWidth = doc.PageSize.Width;
                                float pageHeight = doc.PageSize.Height;
                                jpg.SetAbsolutePosition(0, 0);
                                jpg.ScaleToFit(pageWidth, pageHeight);
                                //writer.Add(jpg);
                                doc.NewPage();
                                doc.Add(jpg);

                            }
                        }

                        doc.Close();
                        Response.Write(doc);
                        Response.Flush();
                        //Response.End();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Select atleast one attachment to Export";
                        ucError.Visible = true;
                    }

                }
                RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
            "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            Rebind();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (attachmentcode != string.Empty)
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") || type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "AOD")))
            {
                type = (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? "APPROVEDPD" : "APPROVEDOWNERPD");
            }

            if (ViewState["BYVESSEL"].ToString() == string.Empty)
            {
                ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type, sortexpression, sortdirection,
                                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                            gvAttachment.PageSize,
                                                                            ref iRowCount, ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type
                                                                          , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                          , sortexpression, sortdirection,
                                                                          Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                          gvAttachment.PageSize,
                                                                          ref iRowCount, ref iTotalPageCount);
            }
        }
        gvAttachment.DataSource = ds;
        gvAttachment.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
    protected void gvAttachment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = e.NewEditIndex;
            Rebind();
            ((CheckBox)_gridView.Rows[e.NewEditIndex].FindControl("chkSynch")).Focus();
            string txtFileName = ((RadTextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtFileNameEdit")).Text;
            RadLabel lblFileName = ((RadLabel)_gridView.Rows[e.NewEditIndex].FindControl("lblFileName"));
            Image imgtype = (Image)_gridView.Rows[e.NewEditIndex].FindControl("imgfiletype");
            if (txtFileName != string.Empty)
            {
                imgtype.ImageUrl = ResolveImageType(txtFileName.Substring(txtFileName.LastIndexOf('.')));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //string dtkey = string.Empty;

        if (e.CommandName.ToString().ToUpper() == "SORT") return;
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToString().ToUpper() == "CONVERTTOPDF")
            {
                RadLabel path = ((RadLabel)e.Item.FindControl("lblFilePath"));
                string filename = ((RadLabel)e.Item.FindControl("lblFileName")).Text;
                string filedtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                string pathName = path.Text.ToLower();
                string ext = Path.GetExtension(pathName).ToLower();

                if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp")
                {
                    string file = Server.MapPath("~/Attachments/" + pathName);
                    string name = filename.Replace(ext, ".pdf");
                    string newpath = pathName.Replace(ext, ".pdf");

                    FileStream filestream = new FileStream(Server.MapPath("~/Attachments/") + newpath, FileMode.Create, System.IO.FileAccess.Write);

                    iTextSharp.text.Rectangle pageSize = null;
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(file);

                    pageSize = new iTextSharp.text.Rectangle(0, 0, jpg.Width, jpg.Height);

                    iTextSharp.text.Document doc = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
                    PdfWriter.GetInstance(doc, filestream);
                    doc.Open();

                    float pageWidth = doc.PageSize.Width;
                    float pageHeight = doc.PageSize.Height;
                    jpg.SetAbsolutePosition(0, 0);
                    jpg.ScaleToFit(pageWidth, pageHeight);

                    doc.Add(jpg);
                    doc.Close();
                    filestream.Close();

                    PhoenixCommonFileAttachment.UpdatePDFFileAttachment(new Guid(filedtkey), name, newpath);
                }
                RadScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "PDFDOWNLOAD")
            {
                RadLabel path = ((RadLabel)e.Item.FindControl("lblFilePath"));
                string filename = ((RadLabel)e.Item.FindControl("lblFileName")).Text;
                string filedtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                string pathName = path.Text.ToLower();
                string ext = Path.GetExtension(pathName).ToLower();

                if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp")
                {
                    string file = Server.MapPath("~/Attachments/" + pathName);
                    //filename = Server.MapPath("~/Attachments/" + pathName);
                    //bool exists = File.Exists(filename);
                    string name = filename.Replace(ext, ".pdf");
                    // filename = filename.Replace(ext, ".pdf");

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;Filename=" + name);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    iTextSharp.text.Rectangle pageSize = null;
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(file);

                    pageSize = new iTextSharp.text.Rectangle(0, 0, jpg.Width, jpg.Height);

                    iTextSharp.text.Document doc = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
                    doc.Open();

                    float pageWidth = doc.PageSize.Width;
                    float pageHeight = doc.PageSize.Height;
                    jpg.SetAbsolutePosition(0, 0);
                    jpg.ScaleToFit(pageWidth, pageHeight);

                    doc.Add(jpg);
                    doc.Close();
                    Response.Write(doc);
                    Response.End();

                }
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                try
                {
                    string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                    //RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                    //String path = MapPath("~/attachments/" + lblFilePath.Text);
                    //System.IO.File.Delete(path);
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
                Rebind();

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text;
                bool chk = ((CheckBox)e.Item.FindControl("chkSynch")).Checked;
                string filename = ((RadTextBox)e.Item.FindControl("txtFileNameEdit")).Text.Trim();
                string prvfilename = ((RadLabel)e.Item.FindControl("lblFileNameEdit")).Text;
                string ext = strAfter(prvfilename, ".");
                string newext = strAfter(filename, ".");
                //if (ext.ToUpper().ToString() != newext.ToUpper().ToString())
                //{
                //    ucError.ErrorMessage = " file extension modification is not allowed.";
                //    ucError.Visible = true;
                //    return;
                //}
                if (newext.ToString() != string.Empty)
                {
                    filename = filename.Substring(0, filename.IndexOf("."));
                }
                filename = filename + "." + ext;
                PhoenixCommonFileAttachment.FileSyncUpdate(new Guid(dtkey), (chk ? byte.Parse("1") : byte.Parse("0")), filename);
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    // Get the LinkButton control in the first cell
        //    LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
        //    // Get the javascript which is assigned to this LinkButton
        //    string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //    // Add this javascript to the onclick Attribute of the row
        //    e.Row.Attributes["ondblclick"] = _jsDouble;
        //}
        if (e.Item is GridEditableItem)
        {
            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        }
        if (e.Item is GridDataItem && !e.Item.IsInEditMode)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

            ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
            ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            ImageButton cp = (ImageButton)e.Item.FindControl("cmdConvert");
            if (cp != null)
                cp.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

            ImageButton cd = (ImageButton)e.Item.FindControl("cmdDownload");
            if (cd != null)
                cd.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

            CheckBox cb = (CheckBox)e.Item.FindControl("chkExport");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

            List<string> fileExtentions = new List<string>();

            fileExtentions.Add(".png");
            fileExtentions.Add(".jpeg");
            fileExtentions.Add(".jpg");
            fileExtentions.Add(".gif");
            fileExtentions.Add(".bmp");
            fileExtentions.Add(".pdf");

            if (!fileExtentions.Contains(Path.GetExtension(drv["FLDFILENAME"].ToString()).ToLower()))
            {
                cp.Visible = false;
                cd.Visible = false;
                cb.Visible = false;
            }
            else if (Path.GetExtension(drv["FLDFILENAME"].ToString()).ToLower() == ".pdf")
            {
                cp.Visible = false;
                cd.Visible = false;
            }

            if (Request.QueryString["u"] != null)
            {
                db.Visible = false;
                ed.Visible = false;
                e.Item.Attributes["ondblclick"] = string.Empty;
            }

            RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
            Image imgtype = (Image)e.Item.FindControl("imgfiletype");
            if (lblFileName.Text != string.Empty)
            {
                imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + drv["FLDDTKEY"].ToString(); // Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                //lnk.Style.Add("text-decoration", "underline");
                //lnk.Style.Add("cursor", "pointer");
                //lnk.Style.Add("color", "blue");
                //lnk.Attributes.Add("onclick", "javascript:Openpopup('download','','" + Session["sitepath"] + "/attachments/" + lblFilePath.Text + "')");
            }
        }
    }
    private string strAfter(string value, string a)
    {
        int posA = value.LastIndexOf(a);
        if (posA == -1)
        {
            return "";
        }
        int adjustedPosA = posA + a.Length;
        if (adjustedPosA >= value.Length)
        {
            return "";
        }
        return value.Substring(adjustedPosA);
    }
    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
            case ".docx":
                imagepath += "word.png";
                break;
            case ".xls":
            case ".xlsx":
            case ".xlsm":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }
    private int? VesselId()
    {
        int? VesselId = 0;
        string menucode = Filter.CurrentMenuCodeSelection;
        if (menucode == "REG-GEN-VLM" || menucode.StartsWith("INV") || menucode.StartsWith("PMS")
            || menucode.StartsWith("PUR") || menucode.StartsWith("CRW-PBL") || menucode.StartsWith("VAC"))
        {
            if (menucode == "REG-GEN-VLM")
                VesselId = General.GetNullableInteger(Filter.CurrentVesselMasterFilter != null ? Filter.CurrentVesselMasterFilter : "0");
            else
                VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        return VesselId;
    }
    protected void gvAttachment_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
