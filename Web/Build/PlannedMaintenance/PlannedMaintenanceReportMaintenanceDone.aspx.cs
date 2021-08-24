using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportMaintenanceDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuReportMaintenanceDone.AccessRights = this.ViewState;
            MenuReportMaintenanceDone.MenuList = toolbarmain.Show();

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            txtMakerId.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucJobClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.JOBCLASS)).ToString();
                ucFrequency.HardTypeCode = "7";
                ucCounterType.HardTypeCode = "111";

                cblResponsibility.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
                cblResponsibility.DataBind();

                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }
            }
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

    protected void MenuReportMaintenanceDone_TabStripCommand(object sender, EventArgs e)
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
                    string responsibility = "";
                    foreach (ButtonListItem item in cblResponsibility.Items)
                    {
                        if (item.Selected)
                            responsibility = responsibility + item.Value + ",";
                    }
                    responsibility = responsibility.TrimEnd(',');

                    prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    prams += "&compno=" + General.GetNullableString(txtComponentNumber.Text.Trim());
                    prams += "&compname=" + General.GetNullableString(txtComponentName.Text.Trim());
                    prams += "&comptype=" + General.GetNullableString(txtComponentType.Text);
                    prams += "&dtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                    prams += "&dtto=" + General.GetNullableDateTime(txtDateTo.Text);
                    prams += "&jobclass=" + General.GetNullableInteger(ucJobClass.SelectedQuick);
                    prams += "&vendid=" + General.GetNullableInteger(txtVendorId.Text);
                    prams += "&makeid=" + General.GetNullableInteger(txtMakerId.Text);
                    prams += "&mtnctype=" + General.GetNullableInteger(ucMainType.SelectedQuick);
                    prams += "&frequency=" + General.GetNullableInteger(ucFrequency.SelectedHard);
                    prams += "&frequencyvalue=" + General.GetNullableInteger(txtFrequency.Text);
                    prams += "&counter=" + General.GetNullableInteger(ucCounterType.SelectedHard);
                    prams += "&countervalue=" + General.GetNullableInteger(ucCounterFrequency.Text);
                    prams += "&unplannedwork=" + General.GetNullableInteger(chkUnplanned.Checked == true ? "1" : "null");
                    prams += "&classcode=" + General.GetNullableString(txtClassCode.Text);
                    prams += "&responsibility=" + General.GetNullableString(responsibility);
                    prams += exceloptions();

                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=MAINTENANCEDONE" + prams);
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
        DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

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
