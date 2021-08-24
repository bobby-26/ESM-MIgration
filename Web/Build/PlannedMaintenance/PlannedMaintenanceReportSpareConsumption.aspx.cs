using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportSpareConsumption : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuReportSpareConsumption.AccessRights = this.ViewState;
            MenuReportSpareConsumption.MenuList = toolbarmain.Show();

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            txtMakerId.Attributes.Add("style", "visibility:hidden");

            imgShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");
            cmdShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");

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

    protected void MenuReportSpareConsumption_TabStripCommand(object sender, EventArgs e)
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
                    prams += "&comptype=" + General.GetNullableString(txtComponentType.Text);
                    prams += "&spareno=" + General.GetNullableString(txtSpareItemNumber.Text.Trim());
                    prams += "&sparename=" + General.GetNullableString(txtSpareItemName.Text.Trim());
                    prams += "&dtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                    prams += "&dtto=" + General.GetNullableDateTime(txtDateTo.Text);
                    prams += "&vendid=" + General.GetNullableInteger(txtVendorId.Text);
                    prams += "&makeid=" + General.GetNullableInteger(txtMakerId.Text);
                    prams += exceloptions();

                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=SPARECONSUME" + prams);
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

    protected void cmdMakerClear_Click(object sender, ImageClickEventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }

    protected void cmdVendorClear_Click(object sender, ImageClickEventArgs e)
    {
        txtVendorNumber.Text = "";
        txtVenderName.Text = "";
        txtVendorId.Text = "";
    }
}
