using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;

public partial class PreSeaExamVenueGeneral : PhoenixBasePage
{
    private const string SCRIPT_DOFOCUS = @"window.setTimeout('DoFocus()', 1);
            function DoFocus()
            {
                try {
                    document.getElementById('REQUEST_LASTFOCUS').focus();
                } catch (ex) {}
            }";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                HookOnFocus(this.Page as Control);
                txtVenueName.Focus();               

                if (Request.QueryString["venueid"] != null)
                    ViewState["EXAMVENUEID"] = Request.QueryString["venueid"];
                else
                {
                    ViewState["EXAMVENUEID"] = String.Empty;
                    ClearFields();
                }
                if (!String.IsNullOrEmpty(ViewState["EXAMVENUEID"].ToString()))
                    EditExamVenue(int.Parse(ViewState["EXAMVENUEID"].ToString()));

                ucCountry_TextChanged(null, null);
                txtVenueName.Focus();

               Page.ClientScript.RegisterStartupScript(
               typeof(PreSeaExamVenueGeneral),
               "ScriptDoFocus",
               SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
               true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
            Script += "</script>" + "\n";

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidExamVenue(txtVenueName.Text.Trim(), txtAddress.Text.Trim(), txtPhone1.Text.Trim(), txtContactName.Text.Trim(), ddlSIMSZone.SelectedZone))
                {
                    ucError.Visible = true;
                    return;
                }
                if (String.IsNullOrEmpty(ViewState["EXAMVENUEID"].ToString()))
                {
                    PhoenixPreSeaExamVenue.InsertExamVenue(txtVenueName.Text.Trim()
                        , txtAddress.Text.Trim()
                        , txtAddress2.Text.Trim()
                        , txtAddress3.Text.Trim()
                        , txtAddress4.Text.Trim()
                        , General.GetNullableInteger(ucCountry.SelectedCountry)
                        , General.GetNullableInteger(ucState.SelectedState)
                        , General.GetNullableInteger(ddlCity.SelectedCity)
                        , txtPhone1.Text.Trim()
                        , txtFax1.Text.Trim()
                        , txtEmail1.Text.Trim()
                        , General.GetNullableInteger(ddlZone.SelectedZone)
                        , txtContactName.Text.Trim()
                        , txtContactPhone.Text.Trim()
                        , txtContactMobile.Text.Trim()
                        , txtContactEmail.Text.Trim()
                        , General.GetNullableInteger(ddlSIMSZone.SelectedZone)
                        );
                                                      
                }
                else
                {
                    PhoenixPreSeaExamVenue.UpdateExamVenue(int.Parse(ViewState["EXAMVENUEID"].ToString())
                        , txtVenueName.Text.Trim()
                        , txtAddress.Text.Trim()
                        , txtAddress2.Text.Trim()
                        , txtAddress3.Text.Trim()
                        , txtAddress4.Text.Trim()
                        , General.GetNullableInteger(ucCountry.SelectedCountry)
                        , General.GetNullableInteger(ucState.SelectedState)
                        , General.GetNullableInteger(ddlCity.SelectedCity)
                        , txtPhone1.Text.Trim()
                        , txtFax1.Text.Trim()
                        , txtEmail1.Text.Trim()
                        , General.GetNullableInteger(ddlZone.SelectedZone)
                        , txtContactName.Text.Trim()
                        , txtContactPhone.Text.Trim()
                        , txtContactMobile.Text.Trim()
                        , txtContactEmail.Text.Trim()
                        , General.GetNullableInteger(ddlSIMSZone.SelectedZone)
                        );                    
                }
                ucStatus.Text = "Venue details saved Successfully.";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void HookOnFocus(Control CurrentControl)
    {
        if ((CurrentControl is TextBox) ||
            (CurrentControl is DropDownList))

            (CurrentControl as WebControl).Attributes.Add(
               "onfocus", "try{document.getElementById('__LASTFOCUS').value=this.id} catch(e) {}");
        if (CurrentControl.HasControls())

            foreach (Control CurrentChildControl in CurrentControl.Controls)
                HookOnFocus(CurrentChildControl);
    }

    private void ClearFields()
    {
        string empty = String.Empty;
        ViewState["EXAMVENUEID"] = empty;
        txtVenueName.Text = empty;
        txtAddress.Text = empty;
        txtAddress2.Text = empty;
        txtAddress3.Text = empty;
        txtAddress4.Text = empty;
        ddlCity.SelectedCity = empty;
        ucState.SelectedState = empty;
        ucCountry.SelectedCountry = empty;
        txtPhone1.Text = empty;
        txtFax1.Text = empty;
        txtEmail1.Text = empty;
        txtContactName.Text = empty;
        ddlZone.SelectedZone = empty;
        txtContactPhone.Text = empty;
        txtContactMobile.Text = empty;
        txtContactEmail.Text = empty;
        ddlSIMSZone.SelectedZone = empty;
    }

    private void EditExamVenue(int VenueId)
    {
        DataTable dt = PhoenixPreSeaExamVenue.EditExamVenue(VenueId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtVenueName.Text = dr["FLDEXAMVENUENAME"].ToString();
            txtAddress.Text = dr["FLDADDRESS1"].ToString();
            txtAddress2.Text = dr["FLDADDRESS2"].ToString();
            txtAddress3.Text = dr["FLDADDRESS3"].ToString();
            txtAddress4.Text = dr["FLDADDRESS4"].ToString();
            ddlCity.SelectedCity = dr["FLDCITY"].ToString();
            ucState.SelectedState = dr["FLDSTATE"].ToString();            
            ucCountry.SelectedCountry = dr["FLDCOUNTRY"].ToString();

            if (General.GetNullableInteger(dr["FLDCOUNTRY"].ToString()) != null && General.GetNullableInteger(dr["FLDSTATE"].ToString()) ==  null)
                ucCountry_TextChanged(null, null);

            txtPhone1.Text = dr["FLDPHONE1"].ToString();
            txtFax1.Text = dr["FLDFAX1"].ToString();
            txtEmail1.Text = dr["FLDEMAIL1"].ToString();
            txtContactName.Text = dr["FLDCONTACTPERSONNAME"].ToString();
            ddlZone.SelectedZone = dr["FLDZONEID"].ToString();
            txtContactPhone.Text = dr["FLDCONTACTPERSONPHONE"].ToString();
            txtContactMobile.Text = dr["FLDCONTACTPERSONMOBILE"].ToString();
            txtContactEmail.Text = dr["FLDCONTACTPERSONMAIL"].ToString();
            ddlSIMSZone.SelectedZone = dr["FLDSIMSZONE"].ToString();
        }
    }

    private bool IsValidExamVenue(string VenueName, string Address, string Phone, string ContactName, string simszone)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (String.IsNullOrEmpty(VenueName))
        {
            ucError.ErrorMessage = "Exam Venue is required.";
        }
        if (String.IsNullOrEmpty(Address))
        {
            ucError.ErrorMessage = "Venue Address is required.";
        }
        if (String.IsNullOrEmpty(Phone))
        {
            ucError.ErrorMessage = "Phone is required.";
        }
        if (String.IsNullOrEmpty(ContactName))
        {
            ucError.ErrorMessage = "Name of the Contact Person is required.";
        }
        if (General.GetNullableInteger(simszone)== null)
        {
            ucError.ErrorMessage = "Sims Zone is required";
        }
        return (!ucError.IsError);
    }

    protected void ucCountry_TextChanged(object sender, EventArgs e)
    {
        ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry));
    }

    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ucState.SelectedState));
    }

}
