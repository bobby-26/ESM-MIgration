using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportSurveyDue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuReportSurveyDue.AccessRights = this.ViewState;
            MenuReportSurveyDue.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ucJobClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.JOBCLASS)).ToString();
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

    protected void MenuReportSurveyDue_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                string prams = "";

                if (IsValidDateRange(General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text)))
                {

                    prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    prams += "&compno=" + General.GetNullableString(txtComponentNumber.Text.Trim());
                    prams += "&compname=" + General.GetNullableString(txtComponentName.Text.Trim());
                    prams += "&jobclass=" + General.GetNullableInteger(ucJobClass.SelectedQuick);
                    prams += "&jobcode=" + General.GetNullableString(txtJobCode.Text);
                    prams += "&classcode=" + General.GetNullableString(txtClassCode.Text);
                    prams += "&lastdonedtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                    prams += "&lastdonedtto=" + General.GetNullableDateTime(txtDateTo.Text);
                    prams += exceloptions();

                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=SURVEYDUE" + prams);
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
