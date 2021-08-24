using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogCRBTranscationHistory : PhoenixBasePage
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

    public void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("", "Search", "<i class=\"fa fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("", "Clear", "<i class=\"fa fa-eraser\"></i>", "CLEAR");
        toolbar.AddFontAwesomeButton("", "Export To Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");

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

    private DataSet HistoryData(ref int rowCount)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string txid = string.Empty;
        if (string.IsNullOrWhiteSpace(Request.QueryString["txid"]) == false)
        {
            txid = Request.QueryString["txid"];
        }

        DateTime? fromDate = General.GetNullableDateTime(txtFromDate.Text);
        DateTime? toDate = General.GetNullableDateTime(txtToDate.Text);

         DataSet ds =  PhoenixMarpolLogCRB.LogBookCRBHistorySearch(usercode, vesselId, 1, gvTranscationHistory.PageSize, ref iRowCount, ref iTotalPageCount, txid, fromDate, toDate);
         iRowCount = rowCount;
         return ds;
    }

    private string[] alColumns()
    {
        string[] columns = new string[] { "FLDDISPLAYID", "FLDTYPE", "FLDEMPLOYEECODE",  "FLDRANKSHORTCODE", "FLDCODE", "FLDITEMNO", "FLDRECORD", "FLDLINE2", "FLDLINE3", "FLDLINE4", "FLDLINE5", "FLDLINE6", "FLDLINE7", "FLDLINE8", "FLDDATE", "FLDSTATUS", "FLDDUTYENGINEERSIGN", "FLDVALIDATED", "FLDREVALIDATED", "FLDVERIFIED", "FLDREVERIFIED", "FLDMODIFYING", "FLDREASON",  };
        return columns;
    }

    private string[] alCaptions()
    {
        string[] columns = new string[] { "Entry", "Type", "User Id", "Rank", "Code", "Itemno", "Line 1", "Line 2", "Line 3", "Line 4", "Line 5", "Line 6", "Line 7", "Line 8", "Datetime", "Status", "DE Sign", "Validated", "Re-Validated", "Verified", "Re-Verified", "Modifying", "Reason" }; 
        return columns;
    }

    #region Grid Events
    protected void gvTranscationHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        gvTranscationHistory.DataSource = HistoryData(ref iRowCount);
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
                DataSet ds= HistoryData(ref rowCount);

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