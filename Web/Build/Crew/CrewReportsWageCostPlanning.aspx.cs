using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewReports;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewReportsWageCostPlanning : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuWageCostPlanning.AccessRights = this.ViewState;
            MenuWageCostPlanning.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReportsWageCostPlanning.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");

            toolbar.AddFontAwesomeButton("../Crew/CrewReportsWageCostPlanning.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuWCPExcel.AccessRights = this.ViewState;
            MenuWCPExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                txtCurrentWageDate.Text = DateTime.Now.ToShortDateString();
                //   BindData();
            }
            gvWageCost.PageSize = 10000;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        DataSet ds = PhoenixCrewReportsWageCostPlanning.ListCrewWageCostPlanning(General.GetNullableInteger(ucVessel.SelectedVessel),
            General.GetNullableDateTime(txtCurrentWageDate.Text), General.GetNullableDateTime(txtCrewChangeDate.Text),
            byte.Parse(chkManual.Checked == true ? "1" : "0"), General.GetNullableDecimal(txtBudgetedYTD.Text), General.GetNullableDecimal(txtActualYTD.Text));

        if (ds.Tables.Count > 0)
            gvWageCost.DataSource = ds.Tables[0];

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtActualYTD.Text = ds.Tables[0].Rows[0]["FLDACTUTALYTD"].ToString();
            txtBudgetedYTD.Text = ds.Tables[0].Rows[0]["FLDBUDGETEDYTD"].ToString();
            txtVarianceYTD.Text = ds.Tables[0].Rows[0]["FLDVARIENCEYTD"].ToString();
        }
    }

    protected void ShowExcel()
    {
        string date = DateTime.Now.ToShortDateString();
        string[] alColumns = { "FLDSRNO","FLDREFEMPLOYEENAME", "FLDRANKNAME", "FLDRANKEXP", "FLDSIGNONDATE", "FLDEOCDATE", "FLDAMOUNT", "FLDSUBTOTAL",
                                 "FLDPROPOSEDACTION", "FLDADDITIONALRANKNAME", "FLDEMPLOYEENAME", "FLDNEWRANKEXP", "FLDDOA",
                                 "FLDWAGECOST", "FLDPLANSUBTOTAL" };
        string[] alCaptions = { "Sr.No","Employee Name", "Rank", "Rank exp in Months ", "S/on date", "EOC date", "Current wages as on date", "",
                                  "proposed action", "Rank", "Name", "Rank exp in Months", "DOA", "Wages cost will be", "" };

        DataSet ds = PhoenixCrewReportsWageCostPlanning.ListCrewWageCostPlanning(General.GetNullableInteger(ucVessel.SelectedVessel),
            General.GetNullableDateTime(txtCurrentWageDate.Text), General.GetNullableDateTime(txtCrewChangeDate.Text),
            byte.Parse(chkManual.Checked == true ? "1" : "0"), General.GetNullableDecimal(txtBudgetedYTD.Text), General.GetNullableDecimal(txtActualYTD.Text));

        //DataTable dt = ds.Tables[0].Copy();
        //General.ShowExcel("Crew List For " + vesselname, dt, alColumns, alCaptions, sortdirection, sortexpression);
        Response.AddHeader("Content-Disposition", "attachment; filename=WageCostPlanning.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h3><center>Crew Wage Planning</center></h3></td></tr>");
        Response.Write("<br />");
        Response.Write("<tr><td colspan='14'><b>Vessel Name : </b>" + ucVessel.SelectedVesselName + "</td></tr>");
        Response.Write("<tr><td colspan='14'><b>Date of Report : </b>" + txtCurrentWageDate.Text + "</td></tr>");
        Response.Write("<tr></tr><tr><td>Budgeted YTD</td><td>" + ds.Tables[0].Rows[0]["FLDBUDGETEDYTD"].ToString() + "</td></tr>");
        Response.Write("<tr><td>Actual YTD</td><td>" + ds.Tables[0].Rows[0]["FLDACTUTALYTD"].ToString() + "</td></tr>");
        Response.Write("<tr><td>Variance YTD</td><td>" + ds.Tables[0].Rows[0]["FLDVARIENCEYTD"].ToString() + "</td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        //Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr><td colspan='7'><center><b>Part - A - Actual Onboard</b></center></td><td colspan='7'><center><b>Part B - Planner</b></center></td></tr>");
        Response.Write("<tr><td colspan='7'><center><b>Current Wage Scenario as on date : " + txtCurrentWageDate.Text + "</b></center></td><td colspan='7'><center><b>Crew Change Plan Date : " + txtCrewChangeDate.Text + "</b></center></td></tr>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void MenuWageCostPlanning_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (!DateTime.TryParse(txtCurrentWageDate.Text, out resultDate))
            ucError.ErrorMessage = "'Current wage scenario as on date' is required.";

        //if (!DateTime.TryParse(txtCrewChangeDate.Text, out resultDate))
        //    ucError.ErrorMessage = "'Crew Change Date' is required.";
        if (chkManual.Checked == true)
        {
            if (!General.GetNullableDecimal(txtBudgetedYTD.Text).HasValue)
                ucError.ErrorMessage = "Budgeted YTD is required.";
            if (!General.GetNullableDecimal(txtActualYTD.Text).HasValue)
                ucError.ErrorMessage = "Actual YTD is required.";
        }
        return (!ucError.IsError);
    }

    protected void MenuWCPExcel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVessel.SelectedVessel = "";
                txtCrewChangeDate.Text = "";
                txtCurrentWageDate.Text = DateTime.Now.ToShortDateString();
               // BindData();
                gvWageCost.SelectedIndexes.Clear();
                gvWageCost.EditIndexes.Clear();
                gvWageCost.DataSource = null;
                gvWageCost.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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

    protected void gvWageCost_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblRankid = (RadLabel)e.Item.FindControl("lblRankid");
                RadLabel lblRefEmployeeid = (RadLabel)e.Item.FindControl("lblRefEmployeeid");

                PhoenixCrewReportsWageCostPlanning.UpdateProposedActionWageCostPlanning(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ucVessel.SelectedVessel), Convert.ToDateTime(txtCurrentWageDate.Text), int.Parse(lblRankid.Text), null,
                    int.Parse(lblRefEmployeeid.Text), 2, string.Empty);
                gvWageCost.SelectedIndexes.Clear();
                gvWageCost.EditIndexes.Clear();
                gvWageCost.DataSource = null;
                gvWageCost.Rebind();

                ucStatus.Visible = true;
                ucStatus.Text = "Employee is removed.";
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                string id = ((RadTextBox)e.Item.FindControl("txtEmployeeIdAdd")).Text;
                string amt = ((UserControlMaskNumber)e.Item.FindControl("txtWageCostAdd")).Text;
                if (!IsValidUpdate("1", id, amt))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewReportsWageCostPlanning.UpdateProposedActionWageCostPlanning(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ucVessel.SelectedVessel), Convert.ToDateTime(txtCurrentWageDate.Text),
                    null,
                    int.Parse(id),
                    null, 0,
                    amt
                    );
                BindData();
                gvWageCost.Rebind();
                ucStatus.Visible = true;
                ucStatus.Text = "Employee is Added.";
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UserControlRank ucr = ((UserControlRank)e.Item.FindControl("ucRankEdit"));
                RadComboBox ddl = ((RadComboBox)e.Item.FindControl("ddlEmployeeEdit"));
                RadLabel lblRefEmployeeid = ((RadLabel)e.Item.FindControl("lblRefEmployeeid"));
                UserControlMaskNumber wages = ((UserControlMaskNumber)e.Item.FindControl("txtWageCostEdit"));
                if (!IsValidUpdate(ucr.SelectedRank, ddl.SelectedValue, "1"))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewReportsWageCostPlanning.UpdateProposedActionWageCostPlanning(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ucVessel.SelectedVessel), Convert.ToDateTime(txtCurrentWageDate.Text), int.Parse(ucr.SelectedRank), int.Parse(ddl.SelectedValue),
                        int.Parse(lblRefEmployeeid.Text), 1, wages.Text);
                ucStatus.Visible = true;
                ucStatus.Text = "Employee is replaced.";
                BindData();
            }

            if (e.CommandName.ToUpper().Equals("REVOKE"))
            {
                RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
                PhoenixCrewReportsWageCostPlanning.RevokeProposedActionWageCostPlanning(int.Parse(ucVessel.SelectedVessel)
                    , Convert.ToDateTime(txtCurrentWageDate.Text)
                    , new Guid(lblDTKey.Text));

                gvWageCost.SelectedIndexes.Clear();
                gvWageCost.EditIndexes.Clear();
                gvWageCost.DataSource = null;
                gvWageCost.Rebind();

                ucStatus.Visible = true;
                ucStatus.Text = "Proposed Action Revoked.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWageCost_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {   
           GridColumnGroup p= gvWageCost.MasterTableView.ColumnGroups.FindGroupByName("wage scenario");
            p.HeaderText = "Current Wage Scenario as on Date " + DateTime.Now.ToString("M/d/yyyy");

        }


        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
            RadLabel lblEmpid = (RadLabel)e.Item.FindControl("lblEmpid");
            if (lb != null && lblEmpid != null)
                lb.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpid.Text + "'); return false;");

            RadLabel lblExtraCrew = (RadLabel)e.Item.FindControl("lblExtraCrew");
            if (lblExtraCrew != null && lblExtraCrew.Text == "1") e.Item.CssClass = "redfont";

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

            LinkButton ib = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ib != null) ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);

            if (drv["FLDACTION"].ToString() == "2" || drv["FLDACTION"].ToString() == "1" || drv["FLDACTION"].ToString() == "0")
            {
                if (db != null) db.Visible = false;
                if (ib != null) ib.Visible = false;
            }
            if (drv["FLDRANKNAME"].ToString().ToUpper().Equals("<B>SUB TOTAL 1</B>") ||
                drv["FLDRANKNAME"].ToString().ToUpper().Equals("<B>SUB TOTAL 2</B>") ||
                drv["FLDRANKNAME"].ToString().ToUpper().Equals("<B>SUB TOTAL 3</B>") ||
                drv["FLDRANKNAME"].ToString().ToUpper().Equals("<B>TOTAL CURRENT CREW WAGE (T)</B>") ||
                drv["FLDRANKNAME"].ToString().ToUpper().Equals("<B>EXTRA RANKS COST (X)</B>") ||
                drv["FLDRANKNAME"].ToString().ToUpper().Equals("<B>MONTHLY BUDGETED MANNING COST (B)</B>") ||
                drv["FLDRANKNAME"].ToString().ToUpper().Equals("<B>VARIANCE(( T-X)-B))</B>")
                )
            {
                if (db != null) db.Visible = false;
                if (ib != null) ib.Visible = false;
            }

            if (drv["FLDRANKID"].ToString().Equals("0") && (decimal.Parse(drv["FLDSUBTOTAL"].ToString()) < 0 || decimal.Parse(drv["FLDPLANSUBTOTAL"].ToString()) < 0))
                e.Item.CssClass = "redfont";

            UserControlRank ucr = (UserControlRank)e.Item.FindControl("ucRankEdit");
            if (ucr != null)
            {
                ucr.RankList = PhoenixRegistersRank.ListRank();
                ucr.DataBind();
                ucr.SelectedRank = drv["FLDRANKID"].ToString();
            }

            RadComboBox ddl = (RadComboBox)(e.Item.FindControl("ddlEmployeeEdit"));
            if (ddl != null && General.GetNullableInteger(ucr.SelectedRank) != null && ucr.SelectedRank != string.Empty)
            {
                DataSet ds = PhoenixCrewReportsWageCostPlanning.ListProposedEmployees(int.Parse(ucVessel.SelectedVessel), int.Parse(ucr.SelectedRank));
                ddl.DataSource = ds;
                ddl.DataTextField = "FLDEMPLOYEENAME";
                ddl.DataValueField = "FLDEMPLOYEEID";
                ddl.DataBind();
                ddl.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddl.SelectedValue = drv["FLDEMPLOYEEID"].ToString();
            }
            if (drv["FLDDTKEY"].ToString() == string.Empty)
            {
                LinkButton rk = (LinkButton)e.Item.FindControl("cmdRevoke");
                if (rk != null) rk.Visible = false;
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ca = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ca != null)
            {
                ca.Visible = SessionUtil.CanAccess(this.ViewState, ca.CommandName);
            }
            HtmlGenericControl gc = (HtmlGenericControl)e.Item.FindControl("spnPickListEmployeeAdd");
            LinkButton emp = (LinkButton)e.Item.FindControl("btnShowEmployeeAdd");
            if (emp != null) emp.Attributes.Add("onclick", "showPickList('" + gc.ClientID + "', 'codehelp1', '', '../Common/CommonPickListEmployee.aspx', false); return false;");
        }
    }
    public bool IsValidUpdate(string rankid, string employeeid, string cost)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (string.IsNullOrEmpty(employeeid) || General.GetNullableInteger(employeeid) == null)
            ucError.ErrorMessage = "Employee is required.";

        if (!General.GetNullableInteger(cost).HasValue)
            ucError.ErrorMessage = "Wage Cost is required.";

        return (!ucError.IsError);
    }
    private void PopulatePlannedEmployee(RadComboBox ddl, int Rank)
    {
        DataSet ds = new DataSet();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            ds = PhoenixCrewReportsWageCostPlanning.ListProposedEmployees(int.Parse(ucVessel.SelectedVessel), Rank);
            ddl.DataSource = ds;
            ddl.DataTextField = "FLDEMPLOYEENAME";
            ddl.DataValueField = "FLDEMPLOYEEID";
            ddl.DataBind();
            ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
    }
    
    //      protected void RankAdd_Changed(object sender, EventArgs e)
    //      {
    //          UserControlRank ucr = (UserControlRank)(sender);
    //          RadGrid dg = (RadGrid)(ucr.Parent.Parent.Parent.Parent);
    //          DropDownList ddl = (DropDownList)(dg.FooterRow.FindControl("ddlEmployeeAdd"));
    //          if (General.GetNullableInteger(ucr.SelectedRank).HasValue)
    //              PopulatePlannedEmployee(ddl, int.Parse(ucr.SelectedRank));
    //      }

    protected void chkManual_CheckedChanged(object sender, EventArgs e)
    {
        if (chkManual.Checked == true)
        {
            txtBudgetedYTD.CssClass = "input_mandatory";
            txtBudgetedYTD.ReadOnly = false;
            txtActualYTD.CssClass = "input_mandatory";
            txtActualYTD.ReadOnly = false;
        }
        else
        {
            txtBudgetedYTD.CssClass = "readonlytextbox";
            txtBudgetedYTD.ReadOnly = true;
            txtActualYTD.CssClass = "readonlytextbox";
            txtActualYTD.ReadOnly = true;
        }
    }
    protected void gvWageCost_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
}