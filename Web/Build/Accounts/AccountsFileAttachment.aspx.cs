using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Web;
using Telerik.Web.UI;

public partial class AccountsFileAttachments : PhoenixBasePage
{
    //const int TimedOutExceptionCode = -2147467259;
    string attachmentcode = string.Empty;
    string purchaseinvoiceattachmentcode = string.Empty;
    string AlloweMimeType = string.Empty;
    int isbilled = 0;

    PhoenixModule module;
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvAttachment.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    base.Render(writer);
    //}
    string cmdname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);
        cmdname = (Request.QueryString["cmdname"] == null ? "UPLOAD" : Request.QueryString["cmdname"].ToUpper());

        if (Request.QueryString["pvdtkey"] != null)
        {
            if (General.GetNullableGuid(Request.QueryString["pvdtkey"].ToString()) != null)
                purchaseinvoiceattachmentcode = Request.QueryString["pvdtkey"];
        }

        if (Request.QueryString["voucherid"] != null)
        {
            PhoenixAccountsVoucher.AllowVoucherAttachmentUpdate(General.GetNullableGuid(Request.QueryString["dtkey"].ToString()), ref isbilled);
        }

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

        if (Request.QueryString["mimetype"] != null && (Request.QueryString["type"] == "Invoice" || Request.QueryString["type"] == "MST Acknowledgement" || Request.QueryString["type"] == "Owner Approval"))
        {
            AlloweMimeType = Request.QueryString["mimetype"].ToString();
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["VESSELID"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (!string.IsNullOrEmpty(Request.QueryString["maxreqlen"]))
            {
                ucError.ErrorMessage = "File is too large to upload. Maximum upload Size is " + Math.Round(int.Parse(Request.QueryString["maxreqlen"].ToString()) / 1048576.00 * 100000.00) / 100000 + " MB";
                ucError.Visible = true;
            }
        }

        if (string.IsNullOrEmpty(Request.QueryString["U"]) && isbilled == 0)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Upload", cmdname, ToolBarDirection.Right);
            AttachmentList.AccessRights = this.ViewState;
            AttachmentList.Title = "Attachment";
            AttachmentList.MenuList = toolbarmain.Show();
        }

        if (module.ToString() == "CREW" && !String.IsNullOrEmpty(attachmentcode))
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageLink("javascript:Openpopup('download','','../Common/CommonViewAllAttachments.aspx?dtkey=" + attachmentcode + "&mod=" + module + "&type=" + type + "')", "View All Images", "view_gallery.png", "VIEWALL");
            //SubMenuAttachment.AccessRights = this.ViewState;
            //SubMenuAttachment.MenuList = toolbargrid.Show();
        }

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
                    //if (Request.QueryString["mod"].ToString() == "CREW" && (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") || type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "AOD")))
                    //{
                    //    PhoenixCommonCrew.CrewApprovedPDInsert(
                    //        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    //        attachmentcode,
                    //        type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? 0 : 1);

                    //    string atttype = "";
                    //    atttype = (type.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 154, "APD") ? "APPROVEDPD" : "APPROVEDOWNERPD");
                    //    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(atttype.ToString()), VesselId());

                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    //             "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

                    //    BindData();
                    //    SetPageNavigator();

                    //    return;
                    //}
                    if (Request.QueryString["mod"].ToString().ToUpper() == "ACCOUNTS")
                    {
                        vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();

                        if (!string.IsNullOrEmpty(purchaseinvoiceattachmentcode))
                        {
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(purchaseinvoiceattachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                        }
                        else
                            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, AlloweMimeType, string.Empty, General.GetNullableString(type), vesselid);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                        PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
                        PhoenixAccountsAirfareInvoiceAdmin.AirfareInvoiceAdminStatusUpdate(new Guid(attachmentcode));
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                        // BindData();
                        // gvAttachment.Rebind();
                        Rebind();
                        return;
                    }

                    vesselid = ViewState["VESSELID"] != null ? int.Parse(ViewState["VESSELID"].ToString()) : VesselId();
                    PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(attachmentcode), module, null, string.Empty, string.Empty, General.GetNullableString(type), vesselid);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
                //Rebind();
            }
            else
            {
                string msg = "Select a record to add attachment";
                msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
                ucError.ErrorMessage = msg;
                ucError.Visible = true;
            }

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
            ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                       gvAttachment.PageSize,
                                                                        ref iRowCount, ref iTotalPageCount);
        }


        gvAttachment.DataSource = ds;
        gvAttachment.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }



    protected void gvAttachment_RowDataBound(object sender, GridItemEventArgs e)
    {
        // GridView _gridView = (GridView)sender;
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            // Get the LinkButton control in the first cell
            //LinkButton _doubleClickButton = (LinkButton)e.Item.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            // string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            // e.Item.Attributes["ondblclick"] = _jsDouble;
        }
        if (e.Item is GridEditableItem)
        {
            //  if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                if (Request.QueryString["u"] != null)
                {
                    db.Visible = false;
                    ed.Visible = false;
                    e.Item.Attributes["ondblclick"] = string.Empty;
                }

                if (isbilled == 1)
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
                    lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + drv["FLDDTKEY"].ToString(); //Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                    //lnk.Style.Add("text-decoration", "underline");
                    //lnk.Style.Add("cursor", "pointer");
                    //lnk.Style.Add("color", "blue");
                    //lnk.Attributes.Add("onclick", "javascript:Openpopup('download','','" + Session["sitepath"] + "/attachments/" + lblFilePath.Text + "')");
                }

                if (Request.QueryString["mod"].ToString().ToUpper() == "ACCOUNTS")
                {
                    RadLabel lblAttachmenttype = (RadLabel)e.Item.FindControl("lblAttachmenttype");
                    lblAttachmenttype.Visible = true;
                    imgtype.Visible = false;
                }
            }
        }
        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
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

    protected void gvAttachment_RowDeleting(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (Request.QueryString["adjustmentamount"] != null && Request.QueryString["adjustmentamount"] != string.Empty)
            {
                if (Convert.ToDouble(ViewState["adjustmentamount"]) == 0.00)
                {
                    string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                    PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                    //Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                    //String path = MapPath("~/attachments/" + lblFilePath.Text);
                    //System.IO.File.Delete(path);
                }
                else
                {
                    ucError.ErrorMessage = "Not possible to delete the attachment after adjustment amount as modified";
                    ucError.Visible = true;
                }
            }

            if (Request.QueryString["MOD"].ToString().ToUpper() == "ACCOUNTS")
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                //Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                //String path = MapPath("~/attachments/" + lblFilePath.Text);
                //System.IO.File.Delete(path);
                PhoenixCommonAcount.UpdateDirectPOInvoiceStatus(new Guid(attachmentcode));
            }
            else
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                //Label lblFilePath = (Label)_gridView.Rows[nCurrentRow].FindControl("lblFilePath");
                //String path = MapPath("~/attachments/" + lblFilePath.Text);
                //System.IO.File.Delete(path);
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvAttachment_Sorting(object sender, GridSortCommandEventArgs e)
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
    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAttachment_RowUpdating(object sender, GridCommandEventArgs e)
    {

        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text;
            bool chk = ((RadCheckBox)e.Item.FindControl("chkSynch")).Checked == true;
            PhoenixCommonFileAttachment.FileSyncUpdate(new Guid(dtkey), (chk ? byte.Parse("1") : byte.Parse("0")));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        // BindData();


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
                imagepath += "word.png";
                break;
            case ".xls":
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
