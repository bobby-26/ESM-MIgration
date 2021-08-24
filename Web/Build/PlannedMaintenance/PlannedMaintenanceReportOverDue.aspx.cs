using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportOverDue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbarmain = new PhoenixToolbar();

                PhoenixToolbar toolbarreport = new PhoenixToolbar();
                toolbarreport.AddButton("Show Report", "GO", ToolBarDirection.Right);
                MenuPrintReportOverdue.AccessRights = this.ViewState;
                MenuPrintReportOverdue.MenuList = toolbarreport.Show();

                drdwnFleet.DataSource = PhoenixRegistersFleet.ListFleet();
                drdwnFleet.DataBind();
                chkVessels.DataSource = PhoenixRegistersVessel.ListAssignedVesselTechFleet(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null, 1);
                chkVessels.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void drdwnFleet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drdwnFleet.SelectedValue != String.Empty)
            {
                chkVessels.DataSource = PhoenixRegistersVessel.ListAssignedVesselTechFleet(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, General.GetNullableInteger(drdwnFleet.SelectedValue), 1);
                chkVessels.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool IsValidDateRange(DateTime? FromDate, DateTime? ToDate)
    {
        if (FromDate == null || ToDate == null)
        {
            return true;
        }
        else if (FromDate > ToDate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    protected void MenuPrintReportOverdue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "GO")
            {
                if (IsValidDateRange(General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text)))
                {
                    string vessels = "";
                    foreach (ButtonListItem item in chkVessels.Items)
                    {
                        if (item.Selected)
                            vessels = vessels + item.Value + ",";
                    }

                    string parameters = "";
                    parameters += "&datefrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                    parameters += "&dateto=" + General.GetNullableDateTime(txtDateTo.Text);
                    parameters += "&fleet=" + General.GetNullableInteger(drdwnFleet.SelectedValue);
                    parameters += "&vessels=" + General.GetNullableString(vessels);
                    parameters += exceloptions();

                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=OVERDUE" + parameters);
                }
                else
                {
                    ucError.ErrorMessage = "From Date should not be greater than To Date";
                    ucError.Visible = true;
                }
            }
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

            if (IsValidDateRange(General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text)))
            {
                string vessels = "";
                foreach (ButtonListItem item in chkVessels.Items)
                {
                    if (item.Selected)
                        vessels = vessels + item.Value + ",";
                }

                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.ReportOverDue(General.GetNullableDateTime(txtDateFrom.Text),
                                                                                    General.GetNullableDateTime(txtDateTo.Text),
                                                                                    General.GetNullableInteger(drdwnFleet.SelectedValue),
                                                                                    General.GetNullableString(vessels));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        ds.Clear();
                    }

                    RadGrid1.DataSource = ds.Tables[0];
                }
                //else
                //{
                //    ds.Tables[0].Rows[0].Delete();
                //    ds.AcceptChanges();
                //    DataTable dt = ds.Tables[0];
                //}
            }
            else
            {
                ucError.ErrorMessage = "From Date should not be greater than To Date";
                ucError.Visible = true;
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
        try
        {

            DataSet ds = new DataSet();

            string vessels = "";
            foreach (ButtonListItem item in chkVessels.Items)
            {
                if (item.Selected)
                    vessels = vessels + item.Value + ",";
            }

            string[] alColumns = { "FLDVESSELNAME", "FLDNOOFDUE", "FLDNOOFWO", "PERCENTAGE", "FLDNOOFCRITIACLDUE", "FLDNOOFDUEEXCEED30DAYS" };
            string[] alCaptions = { "Vessel Name", "No of Due", "No of Workorders", "Overdue", "Overdue Critical jobs", "Overdue > 30 days" };

            ds = PhoenixPlannedMaintenanceWorkOrderReport.ReportOverDue(General.GetNullableDateTime(txtDateFrom.Text), 
                General.GetNullableDateTime(txtDateTo.Text), General.GetNullableInteger(drdwnFleet.SelectedValue), General.GetNullableString(vessels));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Columns.Add("PERCENTAGE", typeof(String));
                ds.AcceptChanges();
                int lastindex = ds.Tables[0].Columns.Count - 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr.BeginEdit();
                    dr[lastindex] = dr["FLDPERCENT"].ToString() + " %";
                    dr.EndEdit();
                }
                ds.AcceptChanges();
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=OverDue.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Overdue Report</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void drdwnFleet_DataBound(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            drdwnFleet.Items.Insert(0, new DropDownListItem("--Select--", "DUMMY"));
        }
    }
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }





    /////
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            RadGrid1.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;
            ShowExcel();
        }
    }
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;

            RadLabel lblPercent = (RadLabel)e.Item.FindControl("lblPercent");
            if (!String.IsNullOrEmpty(lblPercent.Text))
            {
                decimal per = Convert.ToDecimal(lblPercent.Text);
                if (per > 2)
                {
                    lblPercent.BackColor = System.Drawing.Color.Red;
                }

                lblPercent.Text += "%";
            }
        }
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

}
