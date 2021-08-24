using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogFuelChangeTransactionHistory : PhoenixBasePage
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
        string txid = string.Empty;
        if (string.IsNullOrWhiteSpace(Request.QueryString["txid"]) == false)
        {
            txid = Request.QueryString["txid"];
        }

        DateTime? fromDate = General.GetNullableDateTime(txtFromDate.Text);
        DateTime? toDate = General.GetNullableDateTime(txtToDate.Text);

        return PhoenixLogFuelChangeOver.LogBookHistorySearch(usercode, vesselId, gvTranscationHistory.CurrentPageIndex + 1, gvTranscationHistory.PageSize, ref iRowCount, ref iTotalPageCount, txid, fromDate, toDate);
    }

    #region Grid Events
    protected void gvTranscationHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        
        AddGridColumns("ID", "FLDDISPLAYID", typeof(System.Int32), "75px");
        AddGridColumns("Date & Time", "FLDDATE", typeof(System.DateTime), "75px");
        AddGridColumns("Type", "FLDTYPE", typeof(System.String), "75px");
        AddGridColumns("UserId", "FLDEMPLOYEECODE", typeof(System.String), "75px");
        AddGridColumns("RankId", "FLDRANKSHORTCODE", typeof(System.String), "75px");
        AddGridColumns("Status Change", "FLDTRANSACTIONTYPE", typeof(System.String), "75px");

        AddGridColumns("BCO-FO Type", "FLDBEFOREFUELTYPE", typeof(System.String), "75px");
        AddGridColumns("BCO-Sulphur (%)", "FLDBEFOREFUELSULPHUR", typeof(System.Decimal), "75px");
        AddGridColumns("BCO-BDN ref", "FLDBEFOREFUELBDN", typeof(System.String), "75px");
        AddGridColumns("ACO-FO Type", "FLDAFTERFUELTYPE", typeof(System.String), "75px");
        AddGridColumns("ACO-Sulphur (%)", "FLDAFTERFUELSULPHUR", typeof(System.Decimal), "75px");
        AddGridColumns("ACO-BDN ref", "FLDAFTERFUELBDN", typeof(System.String), "75px");

        AddGridColumns("SCO-DateTime", "FLDSTARTDATE", typeof(System.DateTime), "75px");
        AddGridColumns("SCO-Position", "FLDSTARTLOCATION", typeof(System.String), "75px");

        AddGridColumns("CCO-DateTime", "FLDCOMPLETEDATE", typeof(System.DateTime), "75px");
        AddGridColumns("CCO-Position", "FLDCOMPLETELOCATION", typeof(System.String), "75px");

        AddGridColumns("SCH-DateTime", "FLDENTRYDATE", typeof(System.DateTime), "75px");
        AddGridColumns("SCH-Position", "FLDENTRYLOCATION", typeof(System.String), "75px");
        
        DataTable dt = PhoenixMarpolLogNOX.AnnexureTankDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                AddGridColumns( "Tank #" + (i + 1), dr["FLDID"].ToString() + "tank", typeof(System.Guid), "75px");
                AddGridColumns("ROB", dr["FLDID"].ToString(), typeof(System.Guid), "75px");
            }
        }

        AddGridColumns("Machinery", "FLDMACHINERY", typeof(System.Int32), "75px");
        AddGridColumns("Signed", "FLDDUTYENGINEERSIGN", typeof(System.Int32), "75px");
        AddGridColumns("Status", "FLDROWNUMBER", typeof(System.Int32), "75px");
        AddGridColumns("Verified", "FLDVERIFIED", typeof(System.Int32), "75px");
        AddGridColumns("Re-Verified", "FLDREVERIFIED", typeof(System.Int32), "75px");
        AddGridColumns("Modifying", "FLDMODIFYING", typeof(System.Int32), "75px");

        gvTranscationHistory.DataSource = GetHistoryData(ref iRowCount);
        gvTranscationHistory.VirtualItemCount = iRowCount;
    }

    private void AddGridColumns(string headerText, string dataField,Type dataType, string width)
    {
        GridBoundColumn column = new GridBoundColumn();
        gvTranscationHistory.MasterTableView.Columns.Add(column);
        column.HeaderText = headerText;
        column.UniqueName = dataField;
        column.ReadOnly = true;
        column.DataField = dataField;
        column.DataType = dataType;
        column.ItemStyle.Width = Unit.Parse(width);
        column.HeaderStyle.Width = Unit.Parse(width);
        column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
    }

    protected void gvTranscationHistory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            DataSet ds = (DataSet)((RadGrid)sender).DataSource;
            DataTable rob = ds.Tables[1];

            GridDataItem item = (GridDataItem)e.Item;

            foreach (GridColumn c in gvTranscationHistory.Columns)
            {
                if (General.GetNullableGuid(c.UniqueName) != null)
                {
                    DataRow[] dr = rob.Select("FLDTRANSACTIONID = '" + drv["FLDTRANSACTIONID"].ToString() + "' AND FLDTANKID = '" + c.UniqueName + "'");
                    if (dr.Length > 0)
                    {
                        item[c.UniqueName].Text = dr[0]["FLDROB"].ToString();
                        item[c.UniqueName + "tank"].Text = dr[0]["FLDTANKNAME"].ToString();
                    }
                    else
                        item[c.UniqueName].Text = "-";
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
                int iRowCount = 0;
                DataSet ds = GetHistoryData(ref iRowCount);

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

    private string[] alCaptions()
    {
        string[] columns = new string[] { "Entry", "Type", "User Id", "Rank", "Code", "Itemno", "Line 1", "Line 2", "Line 3", "Line 4", "Line 5", "Datetime", "Status", "DE Sign", "Validated", "Re-Validated", "Verified", "Re-Verified", "Modifying", "Reason" };
        return columns;
    }

    private string[] alColumns()
    {
        string[] columns = new string[] { "FLDDISPLAYID", "FLDTYPE", "FLDEMPLOYEECODE", "FLDRANKSHORTCODE", "FLDCODE", "FLDITEMNO", "FLDRECORD", "FLDLINE2", "FLDLINE3", "FLDLINE4", "FLDLINE5", "FLDDATE", "FLDSTATUS", "FLDDUTYENGINEERSIGN", "FLDVALIDATED", "FLDREVALIDATED", "FLDVERIFIED", "FLDREVERIFIED", "FLDMODIFYING", "FLDREASON", };
        return columns;
    }
}