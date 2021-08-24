using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class DocumentFMSDrawingAttachmentList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            ViewState["DRAWINGID"] = "";
            ViewState["VESSELID"] = "";
            if (Request.QueryString["drawingid"] != null)
            {
                ViewState["DRAWINGID"] = Request.QueryString["drawingid"].ToString();
            }
            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();
            }
            BindMapping();
            gvFMSFileNoattachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            //gvFMSFileNoattachment.DataSource = PhoenixRegisterFMSMail.FMSFILEATTACHMENTLIST();
            //gvFMSFileNoattachment.DataBind();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddLinkButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFMSFileNoAttachmentUpload.aspx?DRAWINGID=" + ViewState["DRAWINGID"].ToString() + "');", "Upload File", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");
        toolbar.AddLinkButton("javascript:openNewWindow('codehelp1','','DocumentManagement/DocumentManagementFMSDrawingAttachmentUpload.aspx?VESSELID=" + ViewState["VESSELID"] + "&DRAWINGID=" + Request.QueryString["drawingid"].ToString() + "')", "Upload File", "UPLOAD", ToolBarDirection.Right);

        MenuRegistersFMSFileNo.AccessRights = this.ViewState;
        MenuRegistersFMSFileNo.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
    }

    protected void BindMapping()
    {
        DataSet ds = PhoenixRegisterFMSMail.EditFMSDrawingAttachments(new Guid(ViewState["DRAWINGID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            MenuRegistersFMSFileNo.Title = ds.Tables[0].Rows[0]["FLDSUBCATEGORYNAME"].ToString();
        }
    }

    protected void RegistersFMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFMSFileNoattachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixRegisterFMSMail.FMSDRAWINGUploadList(
                                                        ViewState["DRAWINGID"].ToString(),
                                                        int.Parse(ViewState["VESSELID"].ToString()),
                                                        sortexpression,
                                                        sortdirection,
                                                        gvFMSFileNoattachment.CurrentPageIndex + 1,
                                                        int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount.ToString()),
                                                        ref iRowCount,
                                                        ref iTotalPageCount
                                                        );
            //General.SetPrintOptions("gvFMSFileNoattachment", "File No Upload List", alCaptions, alColumns, ds);

            gvFMSFileNoattachment.DataSource = ds;
            gvFMSFileNoattachment.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvFMSFileNoattachment.Rebind();
    }


    protected void gvFMSFileNoattachment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)item.DataItem;
            HyperLink hlnkfilename = (HyperLink)e.Item.FindControl("lnkfilename");
            RadLabel lblattachmentcode = (RadLabel)e.Item.FindControl("lblattachmentcode");
            HyperLink lnkfile = (HyperLink)e.Item.FindControl("lnkfile");

            LinkButton del = (LinkButton)item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            if (lnkfile != null)
            {
                lnkfile.NavigateUrl = "../Common/download.aspx?dtkey=" + lblattachmentcode.Text;
                //hlnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
            }

            if (!e.Item.IsInEditMode)
            {
                LinkButton db = (LinkButton)item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmTelerik(event); return false;");
                }
            }

            DataRowView dr = (DataRowView)e.Item.DataItem;

            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null && !SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName))
                cmdDelete.Visible = false;
        }
    }


    protected void gvFMSFileNoattachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DOWNLOAD"))
            {
                //HyperLink hlnkfilename = (HyperLink)e.Item.FindControl("lnkfilename");
                //RadLabel lblattachmentcode = (RadLabel)e.Item.FindControl("lblattachmentcode");
                //if (hlnkfilename != null)
                //{
                //    hlnkfilename.NavigateUrl = "../Common/download.aspx?dtkey=" + lblattachmentcode.Text;
                //    //hlnkfilename.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();
                //}

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblattachmentcode = (RadLabel)e.Item.FindControl("lblattachmentcode");
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(lblattachmentcode.Text));
                PhoenixRegisterFMSMail.DeleteFMSFileNoupload(new Guid(lblattachmentcode.Text));
                ucStatus.Text = "Attachment is deleted.";
                gvFMSFileNoattachment.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFMSFileNoattachment_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        gvFMSFileNoattachment.Rebind();
    }
}

