using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogEventLogHistory : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddFontAwesomeButton("", "Search", "<i class=\"fa fa-search\"></i>", "FIND");
        toolbarmain.AddFontAwesomeButton("../Log/ElectricLogEventLogHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        //toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvTranscationHistory')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        gvTabStrip.AccessRights = this.ViewState;
        gvTabStrip.MenuList = toolbarmain.Show();

        if (IsPostBack == false)
        {
            ViewState["PAGENUMBER"] = 1;
            gvTranscationHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

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

    #region Grid Events
    protected void gvTranscationHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string txid = string.Empty;

        DataSet ds = new DataSet();

        ds = PhoenixElogCommon.MarpolEventLogSearch(usercode,
                                                        vesselId,
                                                        General.GetNullableDateTime(txtFromDate.Text),
                                                        General.GetNullableDateTime(txtToDate.Text),
                                                        null, gvTranscationHistory.CurrentPageIndex + 1, gvTranscationHistory.PageSize, ref iRowCount, ref iTotalPageCount);
        gvTranscationHistory.DataSource = ds;
        gvTranscationHistory.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtFromDate.Text = ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString();
            txtToDate.Text = ds.Tables[0].Rows[0]["FLDTODATE"].ToString();
        }
        ViewState["ROWCOUNT"] = iRowCount;
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
                gvTranscationHistory.CurrentPageIndex = 0;
                gvTranscationHistory.Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

        string[] alColumns = { "FLDUSERID", "FLDUSERNAME", "FLDDATE", "FLDVESSELNAME", "FLDEVENTNAME", "FLDLOGBOOK" };
        string[] alCaptions = { "User Id", "User Name", "Datetime", "Vessel", "Event", "Log Book" };

        DataSet ds = new DataSet();

        ds = PhoenixElogCommon.MarpolEventLogSearch(usercode,
                                                           vesselId,
                                                           General.GetNullableDateTime(txtFromDate.Text),
                                                           General.GetNullableDateTime(txtToDate.Text),
                                                           null, gvTranscationHistory.CurrentPageIndex + 1, int.Parse(ViewState["ROWCOUNT"].ToString()), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=EventLog.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Event Log</h3></td>");
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

}