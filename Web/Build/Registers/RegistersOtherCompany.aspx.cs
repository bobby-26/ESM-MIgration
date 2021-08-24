using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOtherCompany : PhoenixBasePage
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
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
			HookOnFocus(this.Page as Control);
            if (Request.QueryString["CompanyId"] != null)
            {
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                ViewState["CompanyId"] = Request.QueryString["CompanyId"].ToString();
               
            }
            else
            {
                //toolbar.AddButton("New", "NEW");
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuCompanyList.AccessRights = this.ViewState;
            MenuCompanyList.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["CompanyId"] != null)
            {               
                CompanyEdit(Int32.Parse(Request.QueryString["CompanyId"].ToString()));
            }
            Page.ClientScript.RegisterStartupScript(
			typeof(RegistersOtherCompany),
			"ScriptDoFocus",
			SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
			true);

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
    protected void CompanyList_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidOtherCompany())
            {
                ucError.Visible = true;
                return;
            }

            try
            {
                if (ViewState["CompanyId"] != null)
                {
                    PhoenixRegistersOtherCompany.UpdateOhterCompany(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Int32.Parse(ViewState["CompanyId"].ToString()),
                        txtCompanyName.Text.Trim(),
                        txtLicenceNo.Text.Trim(),
                        ucAddress.Address1,
                        //txtPlaceOfIncorp.Text.Trim(),
                        General.GetNullableDateTime(txtIssueDate.Text),
                        General.GetNullableDateTime(txtExpiryDate.Text),
                        txtRemarks.Text,
                        chkActive.Checked == true ? 1 : 0,
                        ucAddress.Address2,
                        ucAddress.Address3,
                        ucAddress.Address4,
                        ucAddress.City,
                        General.GetNullableInteger(ucAddress.State),
                        General.GetNullableInteger(ucAddress.Country),
                        ucAddress.PostalCode,
                        txtTelephoneNo.Text,
                        General.GetNullableInteger(ucRegisteredCompany.SelectedOtherCompany)
                        );
                }
                else
                {
                    PhoenixRegistersOtherCompany.InsertOtherCompany(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        txtCompanyName.Text.Trim(),
                        txtLicenceNo.Text.Trim(),
                        ucAddress.Address1,
                        //txtPlaceOfIncorp.Text.Trim(),
                        General.GetNullableDateTime(txtIssueDate.Text),
                        General.GetNullableDateTime(txtExpiryDate.Text),
                        txtRemarks.Text,
                        chkActive.Checked == true ? 1 : 0,
                        ucAddress.Address2,
                        ucAddress.Address3,
                        ucAddress.Address4,
                        ucAddress.City,
                        General.GetNullableInteger(ucAddress.State),
                        General.GetNullableInteger(ucAddress.Country),
                        ucAddress.PostalCode,
                        txtTelephoneNo.Text,
                        General.GetNullableInteger(ucRegisteredCompany.SelectedOtherCompany)
                        );

                    // Reset();
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }

        //if (dce.CommandName.ToUpper().Equals("NEW"))
        //    Reset();
    }

    private bool IsValidOtherCompany()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtCompanyName.Text.Equals(""))
            ucError.ErrorMessage = "Company Name is required.";

        //if (txtLicenceNo.Text.Equals(""))
        //    ucError.ErrorMessage = "Company Licence No. is required.";

        if (ucAddress.Address1.Equals(""))
            ucError.ErrorMessage = "Address1 is required.";

        //if (txtPlaceOfIncorp.Text.Equals(""))
        //    ucError.ErrorMessage = "Place of Incorporation is required.";

        if (General.GetNullableInteger(ucAddress.Country) == null)
            ucError.ErrorMessage = "Country is required.";

        if (General.GetNullableInteger(ucAddress.City) == null)
            ucError.ErrorMessage = "City is required.";

        return (!ucError.IsError);
    }

    private void CompanyEdit(int companyid)
    {
        DataSet ds = PhoenixRegistersOtherCompany.EditOtherCompany(companyid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtCompanyName.Text = dr["FLDCOMPANYNAME"].ToString();
            txtLicenceNo.Text = dr["FLDCOMPANYREGNO"].ToString();
            ucAddress.Address1 = dr["FLDCOMPANYADDRESS"].ToString();
            //txtPlaceOfIncorp.Text = dr["FLDPLACEOFINCORPORATION"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();
            chkActive.Checked = dr["FLDACTIVEYN"].ToString() == "1" ? true : false;
            ucAddress.Address2 = dr["FLDADDRESS2"].ToString();
            ucAddress.Address3 = dr["FLDADDRESS3"].ToString();
            ucAddress.Address4 = dr["FLDADDRESS4"].ToString();
            ucAddress.Country = dr["FLDCOUNTRY"].ToString();
            ucAddress.State = dr["FLDSTATE"].ToString();
            ucAddress.City = dr["FLDCITY"].ToString();
            ucAddress.PostalCode = dr["FLDPOSTALCODE"].ToString();
            txtTelephoneNo.Text = dr["FLDTELEPHONENO"].ToString();
            txtExpiryDate.Text = dr["FLDEXPIRYDATE"].ToString();
            txtIssueDate.Text = dr["FLDISSUEDATE"].ToString();
            ucRegisteredCompany.SelectedOtherCompany = dr["FLDREGISTEREDTO"].ToString();
        }
    }

    protected void ucRegisteredCompany_Changed(object sender, EventArgs e)
    {
        //if (General.GetNullableInteger(ucRegisteredCompany.SelectedOtherCompany) != null)
        //{
        //    DataSet ds = PhoenixRegistersOtherCompany.EditOtherCompany(int.Parse(ucRegisteredCompany.SelectedOtherCompany));

        //    txtCompanyName.Text = ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString();
        //}
    }
}
