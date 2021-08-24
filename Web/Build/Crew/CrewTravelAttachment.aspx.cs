using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;

public partial class Crew_CrewTravelAttachment : PhoenixBasePage
{
    string attachmentcode = string.Empty;
    PhoenixModule module;

    string cmdname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Filter.CurrentMenuCodeSelection != null)
            SessionUtil.PageAccessRights(this.ViewState);

        cmdname = (Request.QueryString["cmdname"] == null ? "UPLOAD" : Request.QueryString["cmdname"].ToUpper());
        if (!string.IsNullOrEmpty(Request.QueryString["dtkey"]) && !string.IsNullOrEmpty(Request.QueryString["mod"]) && !string.IsNullOrEmpty(Request.QueryString["Travelid"]))
        {
            attachmentcode = Request.QueryString["dtkey"];
            ViewState["TRAVELDTKEY"] = attachmentcode;
            ViewState["TRAVELID"] = Request.QueryString["Travelid"];

            ViewState["AGENTID"] = null;

            if (!string.IsNullOrEmpty(Request.QueryString["Agentid"]))
            {
                ViewState["AGENTID"] = Request.QueryString["Agentid"];
            }

            module = (PhoenixModule)(Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]));
        }
        else
        {
            string msg = string.IsNullOrEmpty(Request.QueryString["dtkey"]) ? "Select a record to add attachment" : string.Empty;
            msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
            ucError.ErrorMessage = msg;
            ucError.Visible = true;
        }

        if (string.IsNullOrEmpty(Request.QueryString["U"]) || Request.QueryString["ratingyn"] == "1")
        {
            txtFileUpload1.Enabled = true;
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["VESSELID"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }


            if (Request.QueryString["mimetype"] != null)
            {
                ViewState["AllowedMIMEType"] = Request.QueryString["mimetype"].ToString();
            }
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

            ViewState["RPAGENUMBER"] = 1;

            gvPlan1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

        if (module.ToString() == "CREW" && !String.IsNullOrEmpty(attachmentcode))
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('download','','" + Session["sitepath"] + "/Common/CommonViewAllAttachments.aspx?dtkey=" + attachmentcode + "&mod=" + module + "&type=" + type + "')", "View All Images", "<i class=\"fas fa-images\"></i>", "VIEWALL");
            SubMenuAttachment.AccessRights = this.ViewState;
            SubMenuAttachment.MenuList = toolbargrid.Show();
        }
    }

    
    protected void gvPlan1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["RPAGENUMBER"] = ViewState["RPAGENUMBER"] != null ? ViewState["RPAGENUMBER"] : gvPlan1.CurrentPageIndex + 1;
        BindTicket();
    }

    protected void BindTicket()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string Attachmetdtkey = (ViewState["ATTACHMENTDTKEY"] == null) ? null : (ViewState["ATTACHMENTDTKEY"].ToString());

            string agent = (ViewState["AGENTID"] == null) ? null : (ViewState["AGENTID"].ToString());

            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelhopLineItemSearchTicket(new Guid(ViewState["TRAVELID"].ToString()),
                General.GetNullableInteger(agent),
                (int)ViewState["RPAGENUMBER"],
                gvPlan1.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvPlan1.DataSource = ds;
            gvPlan1.VirtualItemCount = iRowCount;


            ViewState["RROWCOUNT"] = iRowCount;
            ViewState["RTOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPlan1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["RPAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlan1_UpdateCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            string ticketNo = ((RadTextBox)e.Item.FindControl("txtTicketNoEdit")).Text;
            string hoplineitemid = ((RadLabel)e.Item.FindControl("lblHoplineItemId")).Text;
            string AttachmentCode = ViewState["TRAVELDTKEY"].ToString();
            string MappingDTKey = ((RadLabel)e.Item.FindControl("lblMappingDTKey")).Text;

            if (ViewState["RowIndex"] != null)
            {
                GridDataItem item = gvAttachment.Items[int.Parse(ViewState["RowIndex"].ToString()) - 2];

                string AttachmentDTKey = ((RadLabel)item.FindControl("lblDTKey")).Text;
                if (General.GetNullableGuid(AttachmentDTKey) != null)
                {
                    PhoenixCrewTravelQuoteLine.InsertCrewTravelAttachmentMapping(ticketNo.Trim()
                                                                                , new Guid(AttachmentCode)
                                                                                , new Guid(AttachmentDTKey)
                                                                                , new Guid(hoplineitemid)
                                                                                , General.GetNullableGuid(MappingDTKey));

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);

                }
            }
            else
            {
                ucError.ErrorMessage = "Please Select the Attachment";
                ucError.Visible = true;
            }

            BindTicket();
            gvPlan1.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPlan1_DeleteCommand(object sender, GridCommandEventArgs e)
    {

        string AttachmentCode = ((RadLabel)e.Item.FindControl("lblAttachmentCode")).Text;
        string MappingDTKey = ((RadLabel)e.Item.FindControl("lblMappingDTKey")).Text;
        string hoplineitemid = ((RadLabel)e.Item.FindControl("lblHoplineItemId")).Text;

        if (General.GetNullableGuid(MappingDTKey) != null)
        {
            PhoenixCrewTravelQuoteLine.CrewTravelAttachmentMappingDelete(new Guid(MappingDTKey), new Guid(hoplineitemid));

            BindTicket();
            gvPlan1.Rebind();
        }

    }

    protected void gvPlan1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure want to delete ticket mapping?'); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            string agent = (ViewState["AGENTID"] == null) ? null : (ViewState["AGENTID"].ToString());

            if (attachmentcode != string.Empty)
            {
                string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;

                if (ViewState["BYVESSEL"].ToString() == string.Empty)
                {
                    ds = PhoenixCrewTravelQuoteLine.AttachmentSearch(new Guid(attachmentcode), null, type, sortexpression, sortdirection,
                                                                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                gvAttachment.PageSize,
                                                                                ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(agent));
                }
                else
                {
                    ds = PhoenixCrewTravelQuoteLine.AttachmentSearch(new Guid(attachmentcode), null, type
                                                                              , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                              , sortexpression, sortdirection,
                                                                              Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                              gvAttachment.PageSize,
                                                                              ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(agent));
                }
            }


            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }

            if (Request.QueryString["u"] != null)
            {
                db.Visible = false;
                ed.Visible = false;
            }

            LinkButton lblFileName = ((LinkButton)e.Item.FindControl("lblFileName"));
            LinkButton imgtype = (LinkButton)e.Item.FindControl("imgfiletype");
            if (lblFileName.Text != string.Empty)
            {
                string imgclass = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                if (imgclass != null)
                {
                    imgtype.Visible = true;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\">" + imgclass + "</span>";
                    imgtype.Controls.Add(html);

                }

                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");

                lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();

                //if (ViewState["ATTACHMENT"] != null && ViewState["ATTACHMENT"].ToString() != "")
                //{
                //    lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + General.GetNullableGuid(ViewState["ATTACHMENT"].ToString());
                //}
            }

        }
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                LinkButton lblFileName = ((LinkButton)e.Item.FindControl("lblFileName"));

                ViewState["RowIndex"] = e.Item.RowIndex;

                lblTicket.Text = "Ticket List - " + lblFileName.Text;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;

            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelAttachmentMappingSearch(General.GetNullableGuid(dtkey));
            if (ds.Tables[0].Rows.Count <= 0)
            {
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");

                //delete the file from the configured path uncomment if this is required

                //String directoryPath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + lblFilePath.Text;                
                //System.IO.File.Delete(directoryPath);

                ViewState["RowIndex"] = null;

                lblTicket.Text = "Ticket List";

            }
            else
            {
                ucError.ErrorMessage = "Delete Mapped Ticket First";
                ucError.Visible = true;
                return;
            }

            BindData();
            gvAttachment.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvAttachment_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text;
            bool chk = ((RadCheckBox)e.Item.FindControl("chkSynch")).Checked == true;
            string filename = ((RadLabel)e.Item.FindControl("lblFileNameEdit")).Text;
            PhoenixCommonFileAttachment.FileSyncUpdate(new Guid(dtkey), (chk ? byte.Parse("1") : byte.Parse("0")), filename);

            BindData();
            gvAttachment.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        string imagepath = "";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "<i class=\"fas fa-file-image\"></i>";
                break;
            case ".doc":
            case ".docx":
                imagepath += "<i class=\"fas fa-file-word\"></i>";
                break;
            case ".xls":
            case ".xlsx":
            case ".xlsm":
                imagepath += "<i class=\"fas fa-file-excel\"></i>";
                break;
            case ".pdf":
                imagepath += "<i class=\"fas fa-file-pdf\"></i>";
                break;
            case ".rar":
            case ".zip":
                imagepath += "<i class=\"fas fa-file-archive\"></i>";
                break;
            case ".txt":
                imagepath += "<i class=\"fas fa-clipboard\"></i>";
                break;
            default:
                imagepath += "<i class=\"fas fa-file\"></i>";
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




    protected void txtFileUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
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

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            BindData();
            gvAttachment.Rebind();
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