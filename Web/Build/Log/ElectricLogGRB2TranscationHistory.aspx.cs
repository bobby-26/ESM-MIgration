using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogGRB2TranscationHistory : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        ShowToolBar();


        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            gvTranscationHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("", "Search", "<i class=\"fa fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("", "Clear", "<i class=\"fa fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "EXCEL");

        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.AccessRights = this.ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvTranscationHistory.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private DataSet GetHistoryData(ref int iRowCount)
    {
        int iTotalPageCount = 0;

        string logBookId = string.Empty;
        if (string.IsNullOrWhiteSpace(Request.QueryString["txid"]) == false)
        {
            logBookId = Request.QueryString["txid"];
        }
        DateTime? fromDate = General.GetNullableDateTime(txtFromDate.Text);
        DateTime? toDate = General.GetNullableDateTime(txtToDate.Text);
        return PhoenixMarpolLogGRB2.LogBookGRB2HistorySearch(usercode, vesselId, gvTranscationHistory.CurrentPageIndex + 1, gvTranscationHistory.PageSize, ref iRowCount, ref iTotalPageCount, logBookId, fromDate, toDate);
    }

    #region Grid Events
    protected void gvTranscationHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        
        gvTranscationHistory.DataSource = GetHistoryData(ref iRowCount);
        gvTranscationHistory.VirtualItemCount = iRowCount;
    }

    protected void gvTranscationHistory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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
            LinkButton attachmentBtn = (LinkButton)e.Item.FindControl("cmdAttachment");

        }
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            LinkButton editBtn = (LinkButton)e.Item.FindControl("cmdEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (editBtn != null)
            {
                if (string.IsNullOrWhiteSpace(drv["FLDHISTORYID"].ToString()) == false)
                {
                    string script = string.Format("javascript:openNewWindow('MoreInfo', '', '{0}/Log/ElectricLogTranscationHistoryAdd.aspx?id={1}', 'true'); return false", Session["sitepath"], drv["FLDHISTORYID"].ToString());
                    editBtn.Attributes.Add("onclick", script);
                }
            }

            LinkButton lnkAttachment = (LinkButton)e.Item.FindControl("lnkAttachment");
            if (lnkAttachment != null)
            {
                Decimal? dischargeIntoReception = General.GetNullableDecimal(drv["FLDDISCHARGEINTORF"].ToString());
                string icon = Convert.ToBoolean(drv["FLDISATTACHMENTATTACHED"]) ? "fa-paperclip" : "fa-paperclip-na";
                lnkAttachment.Text = string.Format("<span class='icon'><i class='fas {0}'></i></span>", icon);
                lnkAttachment.Attributes.Add("onclick", "openNewWindow('attachment','','Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&MOD=LOG&RefreshWindowName=Log" + "');return false;");
                lnkAttachment.Visible = dischargeIntoReception == null ? false : true;
            }
        }
    }

    protected void gvTranscationHistory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
                gvTranscationHistory.Rebind();
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

    private string[] alColumns()
    {
        string[] columns = new string[] { "FLDDISPLAYID", "FLDTYPE", "FLDEMPLOYEECODE", "FLDRANKSHORTCODE", "FLDMETHOD", "FLDCATEGORY", "FLDPORTPOSITION", "FLDSTATUS", "FLDVERIFIED", "FLDREVERIFIED", "FLDMODIFYING" };
        return columns;
    }

    private string[] alCaptions()
    {
        string[] columns = new string[] { "Entry", "Type", "User Id", "Rank", "Method", "Category", "Quantity", "Facility / Vessel", "Receipt Ref", "Status", "Verified", "Re-Verified", "Modifying" };
        return columns;
    }



    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvTranscationHistory.Rebind();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFromDate.Text = string.Empty;
                txtToDate.Text = string.Empty;
                gvTranscationHistory.Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int rowCount = 0;
                DataSet ds = GetHistoryData(ref rowCount);

                string[] columns = alColumns();
                string[] captions = alCaptions();

                Response.AddHeader("Content-Disposition", "attachment; filename=AuditLog.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                Response.Write("<td><h3>Event Log</h3></td>");
                Response.Write("<td colspan='" + (columns.Length - 2).ToString() + "'>&nbsp;</td>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < captions.Length; i++)
                {
                    Response.Write("<td width='20%'>");
                    Response.Write("<b>" + captions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < columns.Length; i++)
                    {
                        Response.Write("<td>");
                        Response.Write(dr[columns[i]]);
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</TABLE>");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}