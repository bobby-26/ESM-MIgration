using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Configuration;
using Telerik.Web.UI;

public partial class VesselAccountsStockandVictuallingRate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState); CreateMenu();
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                {
                    ddlType.SelectedValue = Request.QueryString["type"].ToString();
                    type();
                }
                else { ddlType.SelectedValue = "1"; type(); }
                ddlYear.SelectedYear = DateTime.Today.Year;
                gvStock.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStockAndVictuallingRate(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                        , General.GetNullableInteger(ddlYear.SelectedYear.ToString() != null ? ddlYear.SelectedYear.ToString() : DateTime.Today.Year.ToString())
                                                                                        , General.GetNullableInteger(ddlMonth.SelectedMonth)
                                                                                        , int.Parse(ddlType.SelectedValue));

                ShowExcel(ddlType.SelectedItem.Text, ds.Tables[0]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void typechange(object sender, EventArgs e)
    {
        type();
    }
    protected void type()
    {
        if (ddlType.SelectedValue == "3")
        {
            ddlMonth.Enabled = false;
        }
        else if (ddlType.SelectedValue == "4")
        {
            Response.Redirect("../VesselAccounts/VesselAccountsStockandVictuallingRateDetails.aspx?type=" + ddlType.SelectedValue, false);
        }
        else
        {
            ddlMonth.Enabled = true;
        }
    }
    protected void gvStock_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["RECORDCOUNT"] = "0";
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStockAndVictuallingRate(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                        , General.GetNullableInteger(ddlYear.SelectedYear.ToString() != null && ddlYear.SelectedYear.ToString() != "" ? ddlYear.SelectedYear.ToString() : DateTime.Today.Year.ToString())
                                                                                        , General.GetNullableInteger(ddlMonth.SelectedMonth), int.Parse(ddlType.SelectedValue));

            gvStock.DataSource = ds;
            gvStock.VirtualItemCount = ds.Tables[0].Rows.Count;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) ViewState["RECORDCOUNT"] = "1";
            else ViewState["RECORDCOUNT"] = "0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsStockandVictuallingRate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsStockandVictuallingRate.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuStock.AccessRights = this.ViewState;
        MenuStock.MenuList = toolbar.Show();
    }
    public void ShowExcel(string strHeading, DataTable dt)
    {
        Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + strHeading + "</h3></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");

        Response.Write("<tr>");
        foreach (DataColumn c in dt.Columns)
        {
            Response.Write("<td><b>");
            if (c.ColumnName == "FLDVESSELNAME")
                Response.Write("Vessel Name");
            else
                Response.Write(c.ColumnName.Substring(0, 3));
            Response.Write("</b></td>");
        }
        Response.Write("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[i].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[i].ToString()) : dr[i]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.Write("<br/>");
        Response.Write("<br/>");
        Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        Response.End();
    }
    protected void gvStock_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        RadGrid gv = (RadGrid)sender;
        if (gv.DataSource.GetType().Name != "DataSet") return;
        if (ViewState["RECORDCOUNT"].ToString() != "0")
        {
            DataSet ds1 = (DataSet)gv.DataSource;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ddlType.SelectedValue == "3")
            {
                //if (e.Item is GridDataItem)
                //{
                //    GridDataItem dataItem = e.Item as GridDataItem;
                //    DataRow[] result = ds1.Tables[1].Select("FLDVESSELNAME ='" + ds1.Tables[0].Rows[e.Item.ItemIndex]["FLDVESSELNAME"].ToString() + "'");
                //    for (int i = 1; i < gvStock.MasterTableView.RenderColumns.Length-2; i++)
                //    {
                //        if (ds1.Tables[0].Columns[i].ToString().ToUpper() != "FLDVESSELNAME")   
                //        {
                //            foreach (DataRow row in result)
                //            {
                //                if (ds1.Tables[0].Columns[i].ToString().ToUpper() == row["FLDMONTH"].ToString().ToUpper())
                //                    e.Item.Cells[i].ToolTip = row["FLDREASON"].ToString();
                //          string s=       dataItem.Cells[i].Text.ToString();
                //            }
                //        }
                //    }
                // }
            }
        }
    }
    protected void gvStock_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column.UniqueName != null && e.Column.UniqueName != "ExpandColumn")
        {
            if (e.Column.UniqueName.ToString() == "FLDVESSELNAME")
            {
                e.Column.HeaderText = "Vessel Name";
                e.Column.HeaderStyle.Width = Unit.Pixel(180);
            }
            else
            {
                if (ddlType.SelectedValue != "3" && ddlType.SelectedValue != "4")
                    e.Column.HeaderTooltip = e.Column.UniqueName.ToString().Substring(0, 11);
                e.Column.HeaderText = e.Column.UniqueName.ToString().Substring(0, 3);
                e.Column.HeaderStyle.Width = Unit.Pixel(80);
            }
        }

    }
}
