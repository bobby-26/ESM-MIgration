using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsOtherCrewGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuOtherCrew.AccessRights = this.ViewState;
            MenuOtherCrew.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                BindFields();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["DTKEY"] != null) && (Request.QueryString["DTKEY"] != ""))
            {
                DataSet ds = PhoenixVesselAccountsOtherCrew.ListOtherCrew(new Guid(Request.QueryString["DTKEY"].ToString())
                                                              , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                txtFirstName.Text = ds.Tables[0].Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = ds.Tables[0].Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = ds.Tables[0].Rows[0]["FLDLASTNAME"].ToString();
                txtSignonDate.Text = ds.Tables[0].Rows[0]["FLDSIGNONDATE"].ToString();

                ddlSignonport.SelectedValue = ds.Tables[0].Rows[0]["FLDSIGNONSEAPORTID"].ToString();//int.Parse();
                ddlSignonport.Text = ds.Tables[0].Rows[0]["FLDSIGNONPORTNAME"].ToString();
                txtSignonRemarks.Text = ds.Tables[0].Rows[0]["FLDSIGNONREMARKS"].ToString();
                txtSignoffDate.Text = ds.Tables[0].Rows[0]["FLDSIGNOFFDATE"].ToString();
                if (!String.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDSIGNOFFSEAPORTID"].ToString()))
                {
                    ddlSignoffport.SelectedValue = ds.Tables[0].Rows[0]["FLDSIGNOFFSEAPORTID"].ToString();
                    ddlSignoffport.Text = ds.Tables[0].Rows[0]["FLDSIGNOFFPORTNAME"].ToString();
                }
                txtSignoffRemarks.Text = ds.Tables[0].Rows[0]["FLDSIGNOFFREMARKS"].ToString();
                if (ds.Tables[0].Rows[0]["FLDREFERENCEID"].ToString() == "-1" || ds.Tables[0].Rows[0]["FLDREFERENCEID"].ToString() == "-2")
                {
                    ddlAccountType.SelectedValue = ds.Tables[0].Rows[0]["FLDREFERENCEID"].ToString();
                    ddlAccountType.Enabled = true;
                }
                else
                {
                    ddlAccountType.SelectedValue = "1";
                    ddlAccountType.Enabled = false;
                }
                ViewState["OPERATIONMODE"] = "EDIT";
            }
            else
            {
                ViewState["OPERATIONMODE"] = "ADD";
                txtSignonDate.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOtherCrew_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOtherCrew(txtFirstName.Text, txtSignonDate.Text, ddlSignonport.SelectedValue, ddlAccountType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                if ((String)ViewState["OPERATIONMODE"] == "EDIT")
                {
                    UpdateOtherCrew(txtFirstName.Text, txtLastName.Text, txtMiddleName.Text, Convert.ToDateTime(txtSignonDate.Text)
                       , int.Parse(ddlSignonport.SelectedValue), txtSignonRemarks.Text, General.GetNullableDateTime(txtSignoffDate.Text)
                        , General.GetNullableInteger(ddlSignoffport.SelectedValue.ToString()), txtSignoffRemarks.Text
                        , new Guid(Request.QueryString["DTKEY"].ToString()), int.Parse(ddlAccountType.SelectedValue));
                }
                else if ((String)ViewState["OPERATIONMODE"] == "ADD")
                {
                    if (ddlAccountType.SelectedValue.ToString() == "1")
                    {
                        ucError.ErrorMessage = "You cannot add Staff account here.";
                        ucError.Visible = true;
                        return;
                    }
                    InsertOtherCrew(txtFirstName.Text, txtLastName.Text, txtMiddleName.Text, Convert.ToDateTime(txtSignonDate.Text)
                        , int.Parse(ddlAccountType.SelectedValue), int.Parse(ddlSignonport.SelectedValue), txtSignonRemarks.Text);
                }
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:fnReloadList('filter');", false);
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidOtherCrew(string FirstName, string Signondate, string Signonport, string Accounttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (FirstName.Trim().Equals(""))
            ucError.ErrorMessage = "First name is required.";

        if (!General.GetNullableDateTime(Signondate).HasValue)
        {
            ucError.ErrorMessage = "Sign On Date is required.";
        }
        else if (DateTime.TryParse(Signondate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign On Date should be earlier than current date";
        }
        else if (DateTime.TryParse(txtSignoffDate.Text, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign Off Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(Signonport).HasValue)
            ucError.ErrorMessage = "Sign On Port is required";

        if (!General.GetNullableInteger(Accounttype).HasValue)
            ucError.ErrorMessage = "Account Type is required";
        return (!ucError.IsError);
    }
    private void InsertOtherCrew(string Firstname, string Lastname, string Middlename, DateTime Signondate, int Accounttype, int Signonport, string Signonremark)
    {
        PhoenixVesselAccountsOtherCrew.InsertOtherCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , Firstname, Lastname, Middlename, Accounttype, Signondate, Signonport
                                        , General.GetNullableString(Signonremark), 1);
    }
    private void UpdateOtherCrew(string Firstname, string Lastname, string Middlename, DateTime Signondate, int Signonport, string Signonremark, DateTime? Signoffdate, int? Signoffport, string Signoffremark, Guid dtkey, int AccountType)
    {
        PhoenixVesselAccountsOtherCrew.UpdateOtherCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , Firstname, Lastname, Middlename, Signondate, Signonport, Signonremark
                                        , Signoffdate, Signoffport, Signoffremark, 1, dtkey, AccountType);
    }

}
