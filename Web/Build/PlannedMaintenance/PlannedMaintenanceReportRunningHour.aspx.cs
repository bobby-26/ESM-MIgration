using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportRunningHour : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuReportRunningHour.AccessRights = this.ViewState;
            MenuReportRunningHour.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                drpdwnCounterType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 111, 0, "HRS");
                drpdwnCounterType.DataBind();

                ViewState["ISTREENODECLICK"] = false;
                ViewState["CURRENTINDEX"] = 1;
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void drpdwnCounterType_DataBound(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            drpdwnCounterType.Items.Insert(0, new DropDownListItem("--Select--", "DUMMY"));
        }
    }
    protected void MenuReportRunningHour_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        try
        {

            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                string prams = "";

                prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                prams += "&compno=" + General.GetNullableString(txtComponentNumber.Text.Trim());
                prams += "&compname=" + General.GetNullableString(txtComponentName.Text.Trim());
                prams += "&dtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                prams += "&dtto=" + General.GetNullableDateTime(txtDateTo.Text);
                prams += "&countertype=" + General.GetNullableString(drpdwnCounterType.SelectedValue);
                prams += exceloptions();

                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=RUNNINGHOUR" + prams);

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
