using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportCommittedCost : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuReportCommittedCost.AccessRights = this.ViewState;
            MenuReportCommittedCost.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["f"] != null)
                {
                    if (Request.QueryString["f"].ToString() == "1")
                    {
                        rblFormatList.SelectedIndex = 0;
                    }
                    else if (Request.QueryString["f"].ToString() == "2")
                    {
                        rblFormatList.SelectedIndex = 1;
                    }
                }
                else
                    rblFormatList.SelectedIndex = 0;
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

    protected void MenuReportCommittedCost_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        try
        {

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                if (IsValidDateRange(General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text)))
                {
                    string prams = "";

                    prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    prams += "&dtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                    prams += "&dtto=" + General.GetNullableDateTime(txtDateTo.Text);
                    prams += exceloptions();

                    if (rblFormatList.SelectedIndex == 0)
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=6&reportcode=COMMITTEDCOST" + prams);
                    if (rblFormatList.SelectedIndex == 1)
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=6&reportcode=COMMITTEDCOSTFORMAT1" + prams);
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
}
