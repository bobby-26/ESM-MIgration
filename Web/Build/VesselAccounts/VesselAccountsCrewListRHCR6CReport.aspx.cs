using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.Globalization;

public partial class VesselAccountsCrewListRHCR6CReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.bind();
                ddlVessel.Enabled = false;

                if (Request.QueryString["EmployeeId"] != null && Request.QueryString["EmployeeId"].ToString() != null)
                    ViewState["EmployeeId"] = Request.QueryString["EmployeeId"];
                else
                    ViewState["EmployeeId"] = null;

                if (Request.QueryString["RestHourStartId"] != null && Request.QueryString["RestHourStartId"].ToString() != null)
                    ViewState["RestHourStartId"] = Request.QueryString["RestHourStartId"];
                else
                    ViewState["RestHourStartId"] = null;

                if (Request.QueryString["RESTHOUREMPLOYEEID"] != null)
                    ViewState["RESTHOUREMPLOYEEID"] = Request.QueryString["RESTHOUREMPLOYEEID"].ToString();
                else
                    ViewState["RESTHOUREMPLOYEEID"] = null;

                BindMonth();
                ViewState["MONTH"] = DateTime.Today.Month.ToString();
                ViewState["YEAR"] = DateTime.Today.Year.ToString();
                ddlMonth.SelectedValue = ViewState["MONTH"].ToString() + "-" + ViewState["YEAR"].ToString();
            }

            string Year = ddlMonth.SelectedItem.Text;
            string[] SplitYear = Year.Split(new Char[] { '-' });
            ViewState["YEAR"] = SplitYear[1];

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindMonth()
    {
        DataSet dsMonth = PhoenixVesselAccountsRestHourReports.RestHourReportMonth(int.Parse(ViewState["EmployeeId"].ToString()), General.GetNullableGuid(ViewState["RESTHOUREMPLOYEEID"].ToString()));
        ddlMonth.DataValueField = "FLDMONTHYEARID";
        ddlMonth.DataTextField = "FLDMONTHNAME";
        ddlMonth.DataSource = dsMonth;
        ddlMonth.DataBind();
    }
    protected void BindData()
    {
        ifMoreInfo.Attributes["src"] = "../Reports/ReportsView.aspx?applicationcode=7&reportcode=RESTHOURSNC&showmenu=false&showexcel=no&showword=no&VESSELID="
                     + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()
                     + "&month=" + int.Parse(ViewState["MONTH"].ToString())
                     + "&year=" + int.Parse(ViewState["YEAR"].ToString())
                     + "&employeeid=" + int.Parse(ViewState["EmployeeId"].ToString())
                     + "&rhstartid=" + ViewState["RestHourStartId"].ToString();
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        string Month = ddlMonth.SelectedValue.ToString();
        string[] SplitMonth = Month.Split(new Char[] { '-' });
        ViewState["MONTH"] = SplitMonth[0];
        ViewState["YEAR"] = SplitMonth[1];

        BindData();
    }
}