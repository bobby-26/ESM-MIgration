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
using System.Net;
using System.Security.Authentication;
using Telerik.Web.UI;
using System.Configuration;
using System.Web.Configuration;

public partial class CommonFileAttachment : PhoenixBasePage
{
    string attachmentcode = string.Empty;
    PhoenixModule module;
    string cmdname = string.Empty;
    int MaxTotalBytes = 1048576; // 1 MB
    long totalBytes;
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
        if (Request.QueryString["adjustmentamount"] != null && Request.QueryString["adjustmentamount"] != string.Empty)
            ViewState["adjustmentamount"] = Request.QueryString["adjustmentamount"];
        if (Request.QueryString["VESSELID"] != null)
        {
            ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
        }
        if (Request.QueryString["pdapprovalyn"] != null)
        {
            ddlStatus.Visible = true;
            lblStatus.Visible = true;
        }
        if (string.IsNullOrEmpty(Request.QueryString["U"]) || Request.QueryString["ratingyn"] == "1")
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Export to Pdf", "E2PDF", ToolBarDirection.Right);
            //toolbarmain.AddButton("Upload", cmdname,ToolBarDirection.Right);
            AttachmentList.AccessRights = this.ViewState;
            AttachmentList.MenuList = toolbarmain.Show();
        }
        else
        {
            tblFiles.Visible = false;
            //AttachmentList.Visible = false;
        }
        spnAllowedFileType.Text = "All";
        if (Request.QueryString["mimetype"] != null)
        {
            ViewState["AllowedMIMEType"] = Request.QueryString["mimetype"];
            txtFileUpload1.AllowedFileExtensions = ViewState["AllowedMIMEType"].ToString().Replace(".", "").Split(',');
            spnAllowedFileType.Text = ViewState["AllowedMIMEType"].ToString().Replace(".", "");
        }
        if (Request.QueryString["SOAStatus"] != null)
        {
            ViewState["SOAStatus"] = Request.QueryString["SOAStatus"];
        }

        if (Request.QueryString["DocSource"] != null)
        {
            ViewState["DocSource"] = Request.QueryString["DocSource"];
        }

        if (Request.QueryString["VOUCHERLINEITEMNO"] != null)
        {
            ViewState["VOUCHERLINEITEMNO"] = Request.QueryString["VOUCHERLINEITEMNO"];
        }

        if (Request.QueryString["RefreshWindowName"] != null)
        {
            ViewState["RefreshWindowName"] = Request.QueryString["RefreshWindowName"];
        }

        if (Request.QueryString["LineYN"] != null)
            ViewState["LineYN"] = Request.QueryString["LineYN"].ToString();

        if (Request.QueryString["maxnoofattachments"] != null && Request.QueryString["maxnoofattachments"].ToString() != "")
            ViewState["maxnoofattachments"] = Request.QueryString["maxnoofattachments"].ToString();

        ViewState["PAGENUMBER"] = 1;
        ViewState["SORTEXPRESSION"] = null;
        ViewState["SORTDIRECTION"] = null;
        ViewState["CURRENTINDEX"] = 1;
        ViewState["BYVESSEL"] = (string.IsNullOrEmpty(Request.QueryString["BYVESSEL"]) ? string.Empty : Request.QueryString["BYVESSEL"]);
        if (!string.IsNullOrEmpty(Request.QueryString["maxreqlen"]))
        {
            ucError.ErrorMessage = "File is too large to Upload. Maximum Upload Size is " + Math.Round(int.Parse(Request.QueryString["maxreqlen"].ToString()) / 1048576.00 * 100000.00) / 100000 + " MB";
            ucError.Visible = true;
        }
        if (module.ToString() == "LOG")
        {
            AttachmentList.Visible = false;
        }

        if (module.ToString() == "OWNERS")
        {
            AttachmentList.Visible = false;
        }

        if (module.ToString() == "ACCOUNTS" && Request.QueryString["Status"] == "Published")
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Upload", cmdname);
            AttachmentList.AccessRights = this.ViewState;
            AttachmentList.Visible = false;
        }

        if (module.ToString() == "CREW" && !String.IsNullOrEmpty(attachmentcode))
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageLink("javascript:openNewWindow('download','','" + Session["sitepath"] + "/Common/CommonViewAllAttachments.aspx?dtkey=" + attachmentcode + "&mod=" + module + "&type=" + type + "')", "View All Images", "view_gallery.png", "VIEWALL");
            SubMenuAttachment.AccessRights = this.ViewState;
            SubMenuAttachment.MenuList = toolbargrid.Show();
        }

        if (module.ToString() == "LOG" && string.IsNullOrWhiteSpace(Request.QueryString["Status"]) == false)
        {
            tblFiles.Visible = (Request.QueryString["Status"].ToLower() == "verified") || (Request.QueryString["Status"].ToLower() == "re-verified") ? false : true;
        }

        if (string.IsNullOrEmpty(Request.QueryString["U"]) && ViewState["SOAStatus"] != null)
        {
            if (ViewState["SOAStatus"].ToString() != "Pending")
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Upload", cmdname);
                AttachmentList.AccessRights = this.ViewState;
                //AttachmentList.MenuList = toolbarmain.Show();
                AttachmentList.Visible = false;
            }
        }
        if (!IsPostBack)
        {
            ViewState["AFO"] = PhoenixCommonRegisters.GetHardCode(1, 52, "ON");

            if (ViewState["AFO"] != null)
            {
                gvAttachment.Columns[9].Visible = true;
                gvAttachment.Columns[10].Visible = true;
            }
            var section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;

            // Value in KBs. 4096 means 4Mb.
            spnUploadSize.Text = (section.MaxRequestLength / 1024).ToString() + " MB";
            MaxTotalBytes = section.MaxRequestLength;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

        //BindData();
    }
    protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == cmdname)
        {

            if (!string.IsNullOrEmpty(attachmentcode))
            {
                UploadedFileCollection fileUpload = txtFileUpload1.UploadedFiles;
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

                    if (Request.QueryString["pdapprovalyn"] != null)
                    {
                        if (General.GetNullableInteger(ddlStatus.SelectedHard) != null)
                        {
                            if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") || type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "AOD")))
                            {
                                PhoenixCommonCrew.CrewApprovedPDInsert(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    attachmentcode,
                                    type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? 0 : 1,
                                    General.GetNullableInteger(ddlStatus.SelectedHard));

                                string atttype = "";
                                atttype = (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? "APPROVEDPD" : "APPROVEDOWNERPD");
                                PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(atttype.ToString()), VesselId());

                                RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                         "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

                                Rebind();

                                return;
                            }
                        }
                        else
                        {
                            string msg = "Select a status for the attachment";
                            ucError.ErrorMessage = msg;
                            ucError.Visible = true;
                        }
                    }

                    if (Request.QueryString["mod"].ToString().ToUpper() == "ACCOUNTS")
                    {
                        vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                        if (ViewState["AllowedMIMEType"] != null)
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module,
                                null, ViewState["AllowedMIMEType"].ToString(), string.Empty, General.GetNullableString(type), vesselid);
                        }
                        else
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        }

                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                        PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                        Rebind();

                        return;
                    }

                    if (Request.QueryString["mod"].ToString() == "QUALITY" && (type.ToUpper() == "INSPECTIONREPORT" || type.ToUpper() == "OPERATORCOMMENTS" || type.ToUpper() == "APPROVALLETTER"))
                    {
                        vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                         "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());

                        Rebind();

                        return;
                    }

                    if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == "APPRAISAL"))
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                        PhoenixCommonCrew.CrewAppraisalFinaliseByAttachment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(attachmentcode));
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);

                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());

                        PhoenixCommonCrewIsAttachment.CrewAppraisalIsAttachmentUpdate(General.GetNullableGuid(attachmentcode));
                        Rebind();

                        return;
                    }
                    if (Request.QueryString["mod"].ToString() == "PRESEA")
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);


                        Rebind();

                        return;
                    }
                    if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == "CONTRACT"))
                    {

                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                        PhoenixCommonCrew.CrewOffshoreContractAttachment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(attachmentcode));

                        Rebind();

                        return;
                    }

                    if (Request.QueryString["mod"].ToString() == "LOG")
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        string RefreshWindowName = string.IsNullOrWhiteSpace((string)ViewState["RefreshWindowName"]) ? "ifMoreInfo" : (string)ViewState["RefreshWindowName"];
                        string script = string.Format("closeTelerikWindow(null,'{0}');", RefreshWindowName);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);

                        Rebind();

                        return;
                    }

                    vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();
                    if (ViewState["AllowedMIMEType"] != null)
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module,
                            null, ViewState["AllowedMIMEType"].ToString(), string.Empty, General.GetNullableString(type), vesselid);
                    }
                    else
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                    }

                    if (ViewState["DocSource"] != null)
                        PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString(), vesselid);

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
                        if (row.Cells.Count > 1)
                        {
                            bool isChecked = ((CheckBox)row.FindControl("chkExport")).Checked;

                            if (isChecked)
                            {
                                RadLabel path = ((RadLabel)row.FindControl("lblFilePath"));

                                string pathName = path.Text.ToLower();
                                string ext = Path.GetExtension(pathName).ToLower();
                                if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp" || ext == ".pdf")
                                {
                                    // string file  = Session["sitepath"] + "/Attachments/" + pathName;
                                    string filename = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + pathName;
                                    bool exists = File.Exists(filename);
                                    if (exists == true)
                                    {
                                        FilePath.Add(filename);
                                    }
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
    protected void gvAttachment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = e.NewEditIndex;
            BindData();
            ((CheckBox)_gridView.Rows[e.NewEditIndex].FindControl("chkSynch")).Focus();
            string txtFileName = ((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtFileNameEdit")).Text;
            Label lblFileName = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblFileName"));
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

    protected void gvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string dtkey = string.Empty;
        //GridView _gridView = (GridView)sender;

        //if (e.CommandName.ToString().ToUpper() == "SORT") return;

        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        //try
        //{
        //    if (e.CommandName.ToString().ToUpper() == "CONVERTTOPDF")
        //    {
        //        Label path = ((Label)item.FindControl("lblFilePath"));
        //        string filename = ((Label)item.FindControl("lblFileName")).Text;
        //        string filedtkey = ((Label)item.FindControl("lblDTKey")).Text;
        //        string pathName = path.Text;
        //        string ext = Path.GetExtension(filename);

        //        if (ext.ToLower() == ".png" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpg" || ext.ToLower() == ".gif" || ext.ToLower() == ".bmp")
        //        {
        //            string file = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + pathName;
        //            string name = filename.Replace(ext, ".pdf");
        //            string newpath = pathName.Replace(ext, ".pdf");

        //            FileStream filestream = new FileStream(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + newpath, FileMode.Create, System.IO.FileAccess.Write);

        //            iTextSharp.text.Rectangle pageSize = null;
        //            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(file);

        //            pageSize = new iTextSharp.text.Rectangle(0, 0, jpg.Width, jpg.Height);

        //            iTextSharp.text.Document doc = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
        //            PdfWriter.GetInstance(doc, filestream);
        //            doc.Open();

        //            float pageWidth = doc.PageSize.Width;
        //            float pageHeight = doc.PageSize.Height;
        //            jpg.SetAbsolutePosition(0, 0);
        //            jpg.ScaleToFit(pageWidth, pageHeight);

        //            doc.Add(jpg);
        //            doc.Close();
        //            filestream.Close();

        //            PhoenixCommonFileAttachment.UpdatePDFFileAttachment(new Guid(filedtkey), name, newpath);
        //        }
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

        //        BindData();
        //    }
        //    else if (e.CommandName.ToString().ToUpper() == "PDFDOWNLOAD")
        //    {
        //        Label path = ((Label)item.FindControl("lblFilePath"));
        //        string filename = ((Label)item.FindControl("lblFileName")).Text;
        //        string filedtkey = ((Label)item.FindControl("lblDTKey")).Text;
        //        string pathName = path.Text.ToLower();
        //        string ext = Path.GetExtension(pathName).ToLower();

        //        if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp")
        //        {
        //            string file = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + pathName;
        //            //filename = Server.MapPath("~/Attachments/" + pathName);
        //            //bool exists = File.Exists(filename);
        //            string name = filename.Replace(ext, ".pdf");
        //            // filename = filename.Replace(ext, ".pdf");

        //            Response.Clear();
        //            Response.ContentType = "application/pdf";
        //            Response.AddHeader("Content-Disposition", "attachment;Filename=" + HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8));
        //            Response.Cache.SetCacheability(HttpCacheability.NoCache);

        //            iTextSharp.text.Rectangle pageSize = null;
        //            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(file);

        //            pageSize = new iTextSharp.text.Rectangle(0, 0, jpg.Width, jpg.Height);

        //            iTextSharp.text.Document doc = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
        //            iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
        //            doc.Open();

        //            float pageWidth = doc.PageSize.Width;
        //            float pageHeight = doc.PageSize.Height;
        //            jpg.SetAbsolutePosition(0, 0);
        //            jpg.ScaleToFit(pageWidth, pageHeight);

        //            doc.Add(jpg);
        //            doc.Close();
        //            Response.Write(doc);
        //            Response.End();

        //        }
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

        //        BindData();
        //    }

        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }
    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        DataRowView drv = (DataRowView)e.Row.DataItem;

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    // Get the LinkButton control in the first cell
        //    LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
        //    // Get the javascript which is assigned to this LinkButton
        //    string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        //    // Add this javascript to the onclick Attribute of the row
        //    e.Row.Attributes["ondblclick"] = _jsDouble;
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (ViewState["SOAStatus"] != null)
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Please note that removing attachment from voucher level might affect other voucher lineitems in this voucher.Are sure want to proceed?')");
                else
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                ImageButton cp = (ImageButton)e.Row.FindControl("cmdConvert");
                ed.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

                ImageButton cd = (ImageButton)e.Row.FindControl("cmdDownload");
                cd.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

                CheckBox cb = (CheckBox)e.Row.FindControl("chkExport");
                cb.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

                List<string> fileExtentions = new List<string>();

                fileExtentions.Add(".png");
                fileExtentions.Add(".jpeg");
                fileExtentions.Add(".jpg");
                fileExtentions.Add(".gif");
                fileExtentions.Add(".bmp");
                fileExtentions.Add(".pdf");

                string filename = GetSafeFilename(drv["FLDFILENAME"].ToString());

                if (!fileExtentions.Contains(Path.GetExtension(filename).ToLower()))
                {
                    cp.Visible = false;
                    cd.Visible = false;
                    cb.Visible = false;
                }
                else if (Path.GetExtension(filename).ToLower() == ".pdf")
                {
                    cp.Visible = false;
                    cd.Visible = false;
                }

                if (Request.QueryString["u"] != null)
                {
                    db.Visible = false;
                    ed.Visible = false;
                    e.Row.Attributes["ondblclick"] = string.Empty;
                }
                if (module.ToString() == "ACCOUNTS" && Request.QueryString["Status"] == "Published")
                {
                    db.Visible = false;
                    ed.Visible = false;
                    e.Row.Attributes["ondblclick"] = string.Empty;
                }

                Label lblFileName = ((Label)e.Row.FindControl("lblFileName"));
                Image imgtype = (Image)e.Row.FindControl("imgfiletype");
                if (lblFileName.Text != string.Empty)
                {
                    imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                    Label lblFilePath = (Label)e.Row.FindControl("lblFilePath");
                    HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
                    lnk.NavigateUrl = "download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();
                    //lnk.Style.Add("text-decoration", "underline");
                    //lnk.Style.Add("cursor", "pointer");
                    //lnk.Style.Add("color", "blue");
                    //lnk.Attributes.Add("onclick", "javascript:Openpopup('download','','" + Session["sitepath"] + "/attachments/" + lblFilePath.Text + "')");
                }

                if (ViewState["SOAStatus"] != null)
                {
                    if (ViewState["SOAStatus"].ToString() != "Pending")
                    {
                        db.Visible = false;
                    }
                }

            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }
    public string GetSafeFilename(string filename)
    {
        return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
    }
    protected void gvAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            if (Request.QueryString["adjustmentamount"] != null && Request.QueryString["adjustmentamount"] != string.Empty)
            {
                if (Convert.ToDouble(ViewState["adjustmentamount"]) == 0.00)
                {
                    string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                    //Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                    //String path = MapPath("~/attachments/" + lblFilePath.Text);
                    //System.IO.File.Delete(path);
                }
                else
                {
                    ucError.ErrorMessage = "Not possible to delete the Attachment after adjustment amount as modified";
                    ucError.Visible = true;
                }
            }

            if (Request.QueryString["MOD"].ToString().ToUpper() == "LOG")
            {
                string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                int? vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                if (ViewState["DocSource"] != null)
                    PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString(), vesselid);

                string RefreshWindowName = string.IsNullOrWhiteSpace((string)ViewState["RefreshWindowName"]) ? "ifMoreInfo" : (string)ViewState["RefreshWindowName"];
                string script = string.Format("closeTelerikWindow(null,'{0}');", RefreshWindowName);
                RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
                BindData();
                return;
            }


            if (Request.QueryString["MOD"].ToString().ToUpper() == "ACCOUNTS")
            {
                string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
                Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                if (ViewState["SOAStatus"] != null && ViewState["LineYN"] == null)
                {
                    ViewState["DTKeyVL"] = dtkey;
                    ViewState["FilePathVL"] = lblFilePath.Text;
                    ucConfirmMsg.Visible = true;
                    return;
                }
                else
                {

                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                    //String path = MapPath("~/attachments/" + lblFilePath.Text);
                    //System.IO.File.Delete(path);
                    PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
                    if (ViewState["DocSource"] != null)
                        PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());
                }
            }
            else
            {
                string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                //Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                //String path = MapPath("~/attachments/" + lblFilePath.Text);
                //System.IO.File.Delete(path);
                int? vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                if (ViewState["DocSource"] != null)
                    PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString(), vesselid);
                if (Request.QueryString["MOD"].ToString().ToUpper() == "OFFSHORE")
                {
                    PhoenixCommonFileAttachment.CBTCrewAuditAttachmentDelete(new Guid(dtkey));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                 "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
        BindData();
    }
    protected void gvAttachment_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvAttachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string dtkey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKeyEdit")).Text;
            bool chk = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkSynch")).Checked;
            string filename = ((RadTextBox)_gridView.Rows[nCurrentRow].FindControl("txtFileNameEdit")).Text.Trim();
            string prvfilename = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblFileNameEdit")).Text;
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
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
        if (menucode != null && (menucode == "REG-GEN-VLM" || menucode.StartsWith("INV") || menucode.StartsWith("PMS")
            || menucode.StartsWith("PUR") || menucode.StartsWith("CRW-PBL") || menucode.StartsWith("VAC")))
        {
            if (menucode == "REG-GEN-VLM")
                VesselId = General.GetNullableInteger(Filter.CurrentVesselMasterFilter != null ? Filter.CurrentVesselMasterFilter : "0");
            else
                VesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        return VesselId;
    }

    protected void CheckMapping_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessageVoucherAttachment ucCM = sender as UserControlConfirmMessageVoucherAttachment;
            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["DTKeyVL"] != null && ViewState["FilePathVL"] != null)
                {
                    string dtkey = ViewState["DTKeyVL"].ToString();
                    int VoclineNo = 0;
                    int flag;
                    if (ViewState["VOUCHERLINEITEMNO"] != null)
                        VoclineNo = int.Parse(ViewState["VOUCHERLINEITEMNO"].ToString());
                    if (ucCM.RequestList == "Line")
                        flag = 1;
                    else flag = 0;

                    DataTable dt = new DataTable();

                    if (flag == 1)
                    {
                        PhoenixCommonAcount.SOAGenerationAttachmentList(new Guid(attachmentcode), flag, VoclineNo);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        PhoenixCommonFileAttachment.InsertAttachment(new Guid(dr["FLDDTKEY"].ToString()));

                        //    }
                        //}
                    }


                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(ViewState["DTKeyVL"].ToString()));
                    //string lblFilePath = ViewState["FilePathVL"].ToString();
                    //String path = MapPath("~/attachments/" + lblFilePath);
                    //System.IO.File.Delete(path);

                    BindData();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void txtFileUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        var section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;

        // Value in KBs. 4096 means 4Mb.        
        MaxTotalBytes = section.MaxRequestLength;
        var liItem = new HtmlGenericControl("li");
        liItem.InnerText = e.File.FileName;
        if (!string.IsNullOrEmpty(attachmentcode))
        {
            //UploadedFile fileUpload = txtFileUpload1.UploadedFiles[0];
            UploadedFileCollection fileUpload = txtFileUpload1.UploadedFiles;
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
                totalBytes += e.File.ContentLength / 1024;
                if (totalBytes < MaxTotalBytes)
                {
                    // Total bytes limit has not been reached, accept the file
                    e.IsValid = true;
                }
                else
                {
                    // Limit reached, discard the file
                    e.IsValid = false;
                }
                if (e.IsValid)
                {

                    if (Request.QueryString["pdapprovalyn"] != null)
                    {
                        if (General.GetNullableInteger(ddlStatus.SelectedHard) != null)
                        {
                            if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") || type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "AOD")))
                            {
                                PhoenixCommonCrew.CrewApprovedPDInsert(
                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    attachmentcode,
                                    type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? 0 : 1,
                                    General.GetNullableInteger(ddlStatus.SelectedHard));

                                string atttype = "";
                                atttype = (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? "APPROVEDPD" : "APPROVEDOWNERPD");
                                PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(atttype.ToString()), VesselId());

                                RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                         "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

                                Rebind();
                                return;
                            }
                        }
                        else
                        {
                            string msg = "Select a status for the attachment";
                            ucError.ErrorMessage = msg;
                            ucError.Visible = true;
                        }
                    }

                    if (Request.QueryString["mod"].ToString().ToUpper() == "ACCOUNTS")
                    {
                        vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                        if (ViewState["AllowedMIMEType"] != null)
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module,
                                null, ViewState["AllowedMIMEType"].ToString(), string.Empty, General.GetNullableString(type), vesselid);
                        }
                        else
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        }

                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                        PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                        Rebind();

                        return;
                    }

                    if (Request.QueryString["mod"].ToString() == "QUALITY" && (type.ToUpper() == "INSPECTIONREPORT" || type.ToUpper() == "OPERATORCOMMENTS" || type.ToUpper() == "APPROVALLETTER"))
                    {
                        vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                         "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());

                        Rebind();
                        return;
                    }

                    if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == "APPRAISAL"))
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                        PhoenixCommonCrew.CrewAppraisalFinaliseByAttachment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(attachmentcode));
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);

                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());

                        PhoenixCommonCrewIsAttachment.CrewAppraisalIsAttachmentUpdate(General.GetNullableGuid(attachmentcode));
                        Rebind();
                        return;
                    }
                    if (Request.QueryString["mod"].ToString() == "PRESEA")
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);


                        Rebind();

                        return;
                    }
                    if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == "CONTRACT"))
                    {

                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                        PhoenixCommonCrew.CrewOffshoreContractAttachment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(attachmentcode));

                        Rebind();

                        return;
                    }

                    if (Request.QueryString["mod"].ToString() == "LOG")
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        string RefreshWindowName = string.IsNullOrWhiteSpace((string)ViewState["RefreshWindowName"]) ? "ifMoreInfo" : (string)ViewState["RefreshWindowName"];
                        string script = string.Format("closeTelerikWindow(null,'{0}');", RefreshWindowName);
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);

                        Rebind();

                        return;
                    }

                    vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();
                    if (ViewState["AllowedMIMEType"] != null)
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module,
                            null, ViewState["AllowedMIMEType"].ToString(), string.Empty, General.GetNullableString(type), vesselid);
                    }
                    else
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(fileUpload, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                    }

                    if (ViewState["DocSource"] != null)
                        PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString(), vesselid);

                    if (Request.QueryString["modalwindow"] != null && Request.QueryString["modalwindow"] != string.Empty)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "refreshParent();", true);
                    }
                    else
                    {
                        RadScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
                    }
                }
                else
                {
                    InvalidFiles.Visible = true;
                    InValidFilesList.Controls.AddAt(0, liItem);
                }

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

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string dtkey = string.Empty;
        //GridView _gridView = (GridView)sender;

        if (e.CommandName.ToString().ToUpper() == "SORT") return;
        try
        {
            if (e.CommandName.ToString().ToUpper() == "CONVERTTOPDF")
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel path = ((RadLabel)item.FindControl("lblFilePath"));
                string filename = ((RadLabel)item.FindControl("lblFileName")).Text;
                string filedtkey = ((RadLabel)item.FindControl("lblDTKey")).Text;
                string pathName = path.Text;
                string ext = Path.GetExtension(filename);

                if (ext.ToLower() == ".png" || ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpg" || ext.ToLower() == ".gif" || ext.ToLower() == ".bmp")
                {
                    string file = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + pathName;
                    string name = filename.Replace(ext, ".pdf");
                    string newpath = pathName.Replace(ext, ".pdf");

                    FileStream filestream = new FileStream(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + newpath, FileMode.Create, System.IO.FileAccess.Write);

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
                //RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                Rebind();
            }
            else if (e.CommandName.ToString().ToUpper() == "PDFDOWNLOAD")
            {
                GridDataItem item = (GridDataItem)e.Item;

                RadLabel path = ((RadLabel)item.FindControl("lblFilePath"));
                string filename = ((RadLabel)item.FindControl("lblFileName")).Text;
                string filedtkey = ((RadLabel)item.FindControl("lblDTKey")).Text;
                string pathName = path.Text.ToLower();
                string ext = Path.GetExtension(pathName).ToLower();

                if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp")
                {
                    string file = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + pathName;
                    //filename = Server.MapPath("~/Attachments/" + pathName);
                    //bool exists = File.Exists(filename);
                    string name = filename.Replace(ext, ".pdf");
                    // filename = filename.Replace(ext, ".pdf");

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;Filename=" + HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8));
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
                //RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                GridEditableItem ei = (GridEditableItem)e.Item;
                dtkey = ((RadLabel)ei.FindControl("lblDTKeyEdit")).Text;
                bool chk = ((CheckBox)ei.FindControl("chkSynch")).Checked;
                string filename = ((RadTextBox)ei.FindControl("txtFileNameEdit")).Text.Trim();
                string prvfilename = ((RadLabel)ei.FindControl("lblFileNameEdit")).Text;
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
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                GridDataItem item = (GridDataItem)e.Item;

                if (Request.QueryString["adjustmentamount"] != null && Request.QueryString["adjustmentamount"] != string.Empty)
                {
                    if (Convert.ToDouble(ViewState["adjustmentamount"]) == 0.00)
                    {
                        dtkey = ((RadLabel)item.FindControl("lblDTKey")).Text;
                        PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                        //Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                        //String path = MapPath("~/attachments/" + lblFilePath.Text);
                        //System.IO.File.Delete(path);
                    }
                    else
                    {
                        ucError.ErrorMessage = "Not possible to delete the Attachment after adjustment amount as modified";
                        ucError.Visible = true;
                    }
                }

                if (Request.QueryString["MOD"].ToString().ToUpper() == "LOG")
                {
                    dtkey = ((RadLabel)item.FindControl("lblDTKey")).Text;
                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                    int? vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                    if (ViewState["DocSource"] != null)
                        PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString(), vesselid);

                    string RefreshWindowName = string.IsNullOrWhiteSpace((string)ViewState["RefreshWindowName"]) ? "ifMoreInfo" : (string)ViewState["RefreshWindowName"];
                    string script = string.Format("closeTelerikWindow(null,'{0}');", RefreshWindowName);
                    RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
                    Rebind();
                    return;
                }

                if (Request.QueryString["MOD"].ToString().ToUpper() == "ACCOUNTS")
                {

                    dtkey = ((RadLabel)item.FindControl("lblDTKey")).Text;
                    RadLabel lblFilePath = (RadLabel)item.FindControl("lblFilePath");
                    if (ViewState["SOAStatus"] != null && ViewState["LineYN"] == null)
                    {
                        ViewState["DTKeyVL"] = dtkey;
                        ViewState["FilePathVL"] = lblFilePath.Text;
                        ucConfirmMsg.Visible = true;
                        return;
                    }
                    else
                    {

                        PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                        //String path = MapPath("~/attachments/" + lblFilePath.Text);
                        //System.IO.File.Delete(path);
                        PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
                        if (ViewState["DocSource"] != null)
                            PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString());
                    }
                }
                else
                {
                    dtkey = ((RadLabel)item.FindControl("lblDTKey")).Text;
                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                    //Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                    //String path = MapPath("~/attachments/" + lblFilePath.Text);
                    //System.IO.File.Delete(path);
                    int? vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                    if (ViewState["DocSource"] != null)
                        PhoenixCommonFileAttachment.AttachmentExist(module, new Guid(attachmentcode), ViewState["DocSource"].ToString(), vesselid);
                    if (Request.QueryString["MOD"].ToString().ToUpper() == "OFFSHORE")
                    {
                        PhoenixCommonFileAttachment.CBTCrewAuditAttachmentDelete(new Guid(dtkey));
                    }
                }
                if (ViewState["RefreshWindowName"] != null)
                {
                    string script = "top.closeTelerikWindow('cmnattachment','" + ViewState["RefreshWindowName"].ToString() + "');";
                    RadScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
                }
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
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
        if (e.Item is GridDataItem && !e.Item.IsInEditMode)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                if (ViewState["SOAStatus"] != null)
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Please note that removing attachment from voucher level might affect other voucher lineitems in this voucher.Are sure want to proceed?')");
                else
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton cp = (LinkButton)e.Item.FindControl("cmdConvert");
            if (cp != null) cp.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

            LinkButton cd = (LinkButton)e.Item.FindControl("cmdDownload");
            if (cd != null) cd.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

            CheckBox cb = (CheckBox)e.Item.FindControl("chkExport");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

            List<string> fileExtentions = new List<string>();

            fileExtentions.Add(".png");
            fileExtentions.Add(".jpeg");
            fileExtentions.Add(".jpg");
            fileExtentions.Add(".gif");
            fileExtentions.Add(".bmp");
            fileExtentions.Add(".pdf");

            string filename = GetSafeFilename(drv["FLDFILENAME"].ToString());

            if (!fileExtentions.Contains(Path.GetExtension(filename).ToLower()))
            {
                cp.Visible = false;
                cd.Visible = false;
                cb.Visible = false;
            }
            else if (Path.GetExtension(filename).ToLower() == ".pdf")
            {
                if (cp != null)
                    cp.Visible = false;
                if (cd != null)
                    cd.Visible = false;
            }

            if (Request.QueryString["u"] != null)
            {
                db.Visible = false;
                ed.Visible = false;
                e.Item.Attributes["ondblclick"] = string.Empty;
            }
            if (module.ToString() == "ACCOUNTS" && Request.QueryString["Status"] == "Published")
            {
                db.Visible = false;
                ed.Visible = false;
                e.Item.Attributes["ondblclick"] = string.Empty;
            }

            if (module.ToString() == "LOG" && string.IsNullOrWhiteSpace(Request.QueryString["Status"]) == false)
            {
                db.Visible = (Request.QueryString["Status"].ToLower() == "verified") || (Request.QueryString["Status"].ToLower() == "re-verified") ? false : true;
                ed.Visible = (Request.QueryString["Status"].ToLower() == "verified") || (Request.QueryString["Status"].ToLower() == "re-verified") ? false : true;
                e.Item.Attributes["ondblclick"] = string.Empty;
            }

            RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
            Image imgtype = (Image)e.Item.FindControl("imgfiletype");
            if (!string.IsNullOrEmpty(lblFileName.Text))
            {
                imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                lnk.NavigateUrl = "download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();
                //lnk.Style.Add("text-decoration", "underline");
                //lnk.Style.Add("cursor", "pointer");
                //lnk.Style.Add("color", "blue");
                //lnk.Attributes.Add("onclick", "javascript:Openpopup('download','','" + Session["sitepath"] + "/attachments/" + lblFilePath.Text + "')");
            }

            if (ViewState["SOAStatus"] != null)
            {
                if (ViewState["SOAStatus"].ToString() != "Pending")
                {
                    db.Visible = false;
                }
            }

        }
    }

    protected void gvAttachment_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }

    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
        BindData();
    }
    private void Rebind()
    {
        gvAttachment.EditIndexes.Clear();
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
}