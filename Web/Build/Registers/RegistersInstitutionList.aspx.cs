using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersInstitutionList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["INSTITUTIONID"] != null)
            {
                toolbar.AddButton("Save", "SAVE");
                ViewState["INSTITUTIONID"] = Request.QueryString["INSTITUTIONID"].ToString();
                InstitutionEdit(Int32.Parse(Request.QueryString["INSTITUTIONID"].ToString()));
            }
            else
            {
                toolbar.AddButton("New", "NEW");
                toolbar.AddButton("Save", "SAVE");
            }
            MenuInstitutionList.AccessRights = this.ViewState;
            MenuInstitutionList.MenuList = toolbar.Show();
            
        }

    }

    protected void MenuInstitutionList_TabStripCommand(object sender, EventArgs e)
    {
        string scriptClosePopup = "";
        scriptClosePopup += "<script language='javaScript' id='InstitutionEdit'>" + "\n";
        scriptClosePopup += "fnReloadList('InstitutionList');";
        scriptClosePopup += "</script>" + "\n";

        string scriptRefreshDontClose = "";
        scriptRefreshDontClose += "<script language='javaScript' id='InstitutionAddNew'>" + "\n";
        scriptRefreshDontClose += "fnReloadList('InstitutionList', null, 'yes');";
        scriptRefreshDontClose += "</script>" + "\n";

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {

            if (ViewState["INSTITUTIONID"] != null)
                try
                {
                    //if (!ucAddress.IsValidAddress())
                    //{
                    //    ucError.ErrorMessage = ucAddress.ErrorMessage;
                    //    ucError.Visible = true;
                    //    return;
                    //}
                    if (!ErrorMessage())
                    {
                        ucError.Visible = true;
                        return;
                    }                    
                        PhoenixRegistersInstitution.UpdateInstitution(PhoenixSecurityContext.CurrentSecurityContext.UserCode,          
                        Int32.Parse(ViewState["INSTITUTIONID"].ToString()),
                        "Institution",
                        txtName.Text,
                        ucAddress.Address1,
                        ucAddress.Address2,
                        ucAddress.Address3,
                        ucAddress.Address4,
                        ucAddress.City,
                        Convert.ToInt32(ucAddress.State),
                        Convert.ToInt32 (ucAddress.Country),
                        ucAddress.PostalCode,
                        txtPhone1.Text,
                        txtPhone2.Text,
                        txtFaxno.Text,
                        txtEmail.Text,
                        txtContact.Text);
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "InstitutionList", scriptClosePopup);

                   
                }
                catch (Exception ex)
                {
                    ucError.Visible = true;
                    ucError.ErrorMessage = ex.Message;
                }

            else
                try
                {
                    if (!ErrorMessage())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersInstitution.InsertInstitution(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                      "Institution",
                      txtName.Text,
                      ucAddress.Address1,
                      ucAddress.Address2,
                      ucAddress.Address3,
                      ucAddress.Address4,
                      ucAddress.City,
                      Convert.ToInt32(ucAddress.State),
                      Convert.ToInt32(ucAddress.Country),
                      ucAddress.PostalCode,
                      txtPhone1.Text,
                      txtPhone2.Text,
                      txtFaxno.Text,
                      txtEmail.Text,
                      txtContact.Text);
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "InstitutionListAddNew", scriptRefreshDontClose);
                    Reset();



                }
                catch (Exception ex)
                {
                    ucError.Visible = true;
                    ucError.ErrorMessage = ex.Message;
                }
        }
        if (dce.CommandName.ToUpper().Equals("NEW"))
            Reset();

    }
    public bool ErrorMessage()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;
        if (txtName.Text.Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (ucAddress.Address1.Equals(""))
            ucError.ErrorMessage = "Address1 is required.";

        if (ucAddress.City.Equals(""))
            ucError.ErrorMessage = "City is required.";

        if (Int32.TryParse(ucAddress.State, out result) == false)
            ucError.ErrorMessage = "State is required";

        if (Int32.TryParse(ucAddress.Country, out result) == false)
            ucError.ErrorMessage = "Country is required";

        if (txtPhone1.Text.Equals(""))
            ucError.ErrorMessage = "Phone no 1 is required.";

        if (txtEmail.Text.Equals(""))
            ucError.ErrorMessage = "Email is required.";
        else if (!General.IsvalidEmail(txtEmail.Text))
        {
            ucError.ErrorMessage = "Email is Not Valid.";
        }

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["INSTITUTIONID"] = null;
        txtName.Text = "";
        ucAddress.Address1="";
        ucAddress.Address2 = "";
        ucAddress.Address3 = "";
        ucAddress.Address4 = "";
        ucAddress.City = "";
        ucAddress.State = null;
        ucAddress.Country=null;
        ucAddress.PostalCode="";
        txtPhone1.Text = "";
        txtPhone2.Text = "";
        txtFaxno.Text = "";
        txtEmail.Text = "";
        txtContact.Text = "";
    }

    private void InstitutionEdit(int companyid)
    {
        try
        {
            DataSet ds = PhoenixRegistersInstitution.EditInstitution(PhoenixSecurityContext.CurrentSecurityContext.UserCode, companyid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtName.Text = dr["FLDNAME"].ToString();
                ucAddress.Address1 = dr["FLDADDRESS1"].ToString();
                ucAddress.Address2 = dr["FLDADDRESS2"].ToString();
                ucAddress.Address3 = dr["FLDADDRESS3"].ToString();
                ucAddress.Address4 = dr["FLDADDRESS4"].ToString();
                ucAddress.City = dr["FLDCITY"].ToString();
                ucAddress.Country = dr["FLDCOUNTRYID"].ToString();
                ucAddress.State = dr["FLDSTATEID"].ToString();            
                ucAddress.PostalCode = dr["FLDPIN"].ToString();
                txtPhone1.Text = dr["FLDPHONENO1"].ToString();
                txtPhone2.Text = dr["FLDPHONENO2"].ToString();
                txtFaxno.Text = dr["FLDFAXNO"].ToString();
                txtEmail.Text = dr["FLDEMAIL"].ToString();
                txtContact.Text = dr["FLDCONTACT"].ToString();

            }
        }

        catch (Exception ex)
        {
            ucError.Visible = true;
            ucError.ErrorMessage = ex.Message;
        }
    }
}
