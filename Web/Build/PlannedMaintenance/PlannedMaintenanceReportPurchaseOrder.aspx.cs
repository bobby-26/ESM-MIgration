using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportPurchaseOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO", ToolBarDirection.Right);
            MenuReportCommittedCost.AccessRights = this.ViewState;
            MenuReportCommittedCost.MenuList = toolbarmain.Show();
            txtDateTo.Text = DateTime.Now.ToShortDateString();
            ucVessel.VesselsOnly = true;
            if (!IsPostBack)
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public bool IsValidFilter(string vessellist, string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessellist.Equals("") || vessellist.Equals("Dummy") || vessellist.Equals(","))
        {
            ucError.ErrorMessage = "Select Vessel";
        }

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }

    protected void MenuReportPurchaseOrder_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        try
        {

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                string prams = "";

                if (!IsValidFilter(ucVessel.SelectedVessel.ToString(), txtDateFrom.Text, txtDateTo.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    prams += "&vesselid=" + ucVessel.SelectedVessel.ToString();
                    prams += "&fromdate=" + General.GetNullableDateTime(txtDateFrom.Text);
                    prams += "&todate=" + General.GetNullableDateTime(txtDateTo.Text);
                    prams += exceloptions();

                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=3&reportcode=PURCHASEFORM" + prams);
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
