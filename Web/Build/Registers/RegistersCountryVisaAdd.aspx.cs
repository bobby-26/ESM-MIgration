using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegistersCountryVisaAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuRegistersCountryAdd.AccessRights = this.ViewState;
            MenuRegistersCountryAdd.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                PopulateData();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    private void PopulateData()
    {
        ddlCountry.SelectedCountry = Request.QueryString["countryid"] != null ? Request.QueryString["countryid"] : "";
        ddlVisaTypeAdd.SelectedHard = Request.QueryString["VisaType"] != null ? Request.QueryString["VisaType"] : "";
        txtTimeTakenAdd.Text = Request.QueryString["TimeTaken"] != null ? Request.QueryString["TimeTaken"] : "";
        chkOnArrivalAdd.Checked = Convert.ToString(Request.QueryString["OnArrival"].Trim()) == "1" ? true : false;
        txtDaysRequiredAdd.Text = Request.QueryString["DaysRequired"] != null ? Request.QueryString["DaysRequired"].Trim() : "";
        chkPhysicalPresenceYNAdd.Checked = Convert.ToString(Request.QueryString["PhysPres"].Trim()) == "1" ? true : false;
        txtPhysicalPresenceSpecificationAdd.Text = Request.QueryString["PhyPreSpec"] != null ? Request.QueryString["PhyPreSpec"].Trim() : "";
        txtUrgentProcedureAdd.Text = Request.QueryString["UrgentProcedure"] != null ? Request.QueryString["UrgentProcedure"].Trim() : "";
        chkPassportYNAdd.Checked = Convert.ToString(Request.QueryString["Passport"].Trim()) == "1" ? true : false;
        txtRemarksAdd.Text = Request.QueryString["Remarks"] != null ? Request.QueryString["Remarks"] : "";
        txtOrdinaryAmountAdd.Text = Request.QueryString["OrdinaryAmount"] != null ? Request.QueryString["OrdinaryAmount"] : "";
        txtUrgentAmonutAdd.Text = Request.QueryString["UrgentAmount"] != null ? Request.QueryString["UrgentAmount"] : "";
    }
    private void UpdateCountryVisa(string visaid, string countrycode, string visatypeid, string timetaken, string daysrequired, int physicalpresence
      , string physicalpresencespecification, string urgentprocedure, string remarks, string ordinaryamount, string urgentamount, int notvalidonoldpp
        ,string Onarrival)
    {
        if (!IsValidCountryVisa(countrycode, visatypeid,
            timetaken, daysrequired,
            physicalpresence, physicalpresencespecification, urgentprocedure))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersCountryVisa.UpdateCountryVisa(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(visaid),
            int.Parse(countrycode),
            int.Parse(visatypeid),
            timetaken, daysrequired, physicalpresence,
            physicalpresencespecification, urgentprocedure, remarks,
            General.GetNullableDecimal(ordinaryamount), General.GetNullableDecimal(urgentamount), notvalidonoldpp,General.GetNullableInteger(Onarrival));
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
    }
    private void InsertCountryVisa(string countrycode, string visatype, string timetaken, string daysrequired
        , int physicalpresence, string physicalpresencespecification, string urgentprocedure, string remarks,
        string ordinaryamount, string urgentamount, int notvalidonoldpp,string Onarrival)
    {
        if (!IsValidCountryVisa(countrycode, visatype,
            timetaken, daysrequired,
            physicalpresence, physicalpresencespecification, urgentprocedure))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersCountryVisa.InsertCountryVisa(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            int.Parse(countrycode),
            int.Parse(visatype),
            timetaken, daysrequired, physicalpresence, physicalpresencespecification, urgentprocedure, remarks,
            General.GetNullableDecimal(ordinaryamount), General.GetNullableDecimal(urgentamount), notvalidonoldpp
            , General.GetNullableInteger(Onarrival));
        String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", scriptpopupclose, true);
    }
    private bool IsValidCountryVisa(string countryname, string visatype, string timetaken, string daysrequired
       , int physicalpresence, string physicalpresencespecification, string urgentprocedure)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(countryname) == null)
            ucError.ErrorMessage = "Country Name is required.";

        if (General.GetNullableInteger(visatype) == null)
            ucError.ErrorMessage = "Visa type is required.";

        if (timetaken.Trim().Equals(""))
            ucError.ErrorMessage = "Time taken is required.";

        if (daysrequired.Trim().Equals(""))
            ucError.ErrorMessage = "Days required is required.";

        if (physicalpresence == 1)
            if (physicalpresencespecification == string.Empty)
                ucError.ErrorMessage = "Physical presence specification is required";

        if (urgentprocedure.Trim().Equals(""))
            ucError.ErrorMessage = "Urgent procedure is required.";

        return (!ucError.IsError);
    }
    protected void MenuRegistersCountryAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                txtPhysicalPresenceSpecificationAdd.Text = chkPhysicalPresenceYNAdd.Checked == false ? "" : txtPhysicalPresenceSpecificationAdd.Text;
                if (Request.QueryString["VisaID"] != null && Request.QueryString["VisaID"].Trim() != string.Empty)
                {
                    UpdateCountryVisa(Request.QueryString["VisaID"], ddlCountry.SelectedCountry, ddlVisaTypeAdd.SelectedHard, txtTimeTakenAdd.Text,
                      txtDaysRequiredAdd.Text, chkPhysicalPresenceYNAdd.Checked ? 1 : 0, txtPhysicalPresenceSpecificationAdd.Text,
                      txtUrgentProcedureAdd.Text, txtRemarksAdd.Text, txtOrdinaryAmountAdd.Text, txtUrgentAmonutAdd.Text, chkPassportYNAdd.Checked ? 1 : 0
                      ,chkOnArrivalAdd.Checked==true?"1":"0");
                }
                else
                {
                    InsertCountryVisa( ddlCountry.SelectedCountry, ddlVisaTypeAdd.SelectedHard, txtTimeTakenAdd.Text, txtDaysRequiredAdd.Text,
                    chkPhysicalPresenceYNAdd.Checked ? 1 : 0, txtPhysicalPresenceSpecificationAdd.Text, txtUrgentProcedureAdd.Text,
                    txtRemarksAdd.Text, txtOrdinaryAmountAdd.Text, txtUrgentAmonutAdd.Text, chkPassportYNAdd.Checked ? 1 : 0, chkOnArrivalAdd.Checked == true ? "1" : "0");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkPhysicalPresenceYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        txtPhysicalPresenceSpecificationAdd.Visible = chkPhysicalPresenceYNAdd.Checked;
    }
}
