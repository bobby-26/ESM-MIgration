using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogRevisionView : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid AttachmentId = new Guid();
    bool isAttached = false;
    int revisionNo;
    DateTime revisionDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (string.IsNullOrWhiteSpace(Request.QueryString["date"]) == false)
        {
            revisionDate = DateTime.Parse(Request.QueryString["date"]);
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["revisionNo"]) == false)
        {
            revisionNo = Convert.ToInt32(Request.QueryString["revisionNo"]);
        }


        ShowToolBar();

        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            gvRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    public void ShowToolBar()
    {
        GetAttachmentId();
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        if (isAttached)
        {
            toolbarmain.AddFontAwesomeButton("javascript: openNewWindow('revisionAttachment', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + AttachmentId + "&MOD=LOG'); return false;", "Attachment", "<img src='../css/Theme1/images/attachment.png' title='Attachment'>", "ATTACHMENT");
        }
        else
        {
            toolbarmain.AddFontAwesomeButton("javascript: openNewWindow('revisionAttachment', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + AttachmentId + "&MOD=LOG'); return false;", "Attachment", "<img src='../css/Theme1/images/no-attachment.png' title='Attachment'>", "ATTACHMENT");
        }

        gvTabStrip.MenuList = toolbarmain.Show();
        gvTabStrip.AccessRights = this.ViewState;

    }

    public void GetAttachmentId()
    {
        DataSet ds = PhoenixElog.LogLocationGetAttachmentFromRevision(usercode, vesselId, revisionNo);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            if (row["FLDATTACHMENTID"] != DBNull.Value)
            {
                AttachmentId = (Guid)row["FLDATTACHMENTID"];
            }

            if (row["FLDATTACHMENTCODE"] != DBNull.Value)
            {
                isAttached = String.IsNullOrWhiteSpace(row["FLDATTACHMENTCODE"].ToString()) == true ? false : true;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvRevision.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

     #region Grid Events
    protected void gvRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRevision.CurrentPageIndex + 1;
        gvRevision.DataSource =  PhoenixElog.PhoenixLogLocationByRevisionNumber(usercode, vesselId, revisionNo, revisionDate);
        //gvRevision.VirtualItemCount = iRowCount;
    }

    protected void gvRevision_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.IsInEditMode)
        {
           
        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            GridDataItem dataItem = (GridDataItem)e.Item;
            int isAdded = drv["FLDISADDED"] == DBNull.Value ? 0 : Convert.ToInt32(drv["FLDISADDED"]);
            ChangeRowColor(isAdded, dataItem);
        }

        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (editBtn != null)
            {
                if (string.IsNullOrWhiteSpace(drv["FLDLOCATIONID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/Log/ElectricLogRevisionViewAdd.aspx?id={1}', 'true'); return false", Session["sitepath"], drv["FLDLOCATIONID"].ToString());
                    editBtn.Attributes.Add("onclick", script);
                }
            }

        }
    }

    private void ChangeRowColor(int isAdded, GridDataItem dataItem)
    {

        if (revisionNo == 0)
        {
            return;
        }

        if (isAdded == 1)
        {
            dataItem.Style.Add("background-color", "green !important");
            dataItem.Style.Add("color", "white !important");
        }
        else if (isAdded == 2)
        {
            dataItem.Style.Add("background-color", "red !important");
            dataItem.Style.Add("color", "white !important");
        }
    }

    protected void gvRevision_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdSave");
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                LinkButton db1 = (LinkButton)e.Item.FindControl("cmdDelete");
                Guid Id = new Guid(e.CommandArgument.ToString());
                gvRevision.Rebind();
            }
            if (e.CommandName == "VIEW")
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    #endregion


    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
	try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}