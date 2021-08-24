using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewPersonalAddress : PhoenixBasePage
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
            if (!IsPostBack)
            {
                HookOnFocus(this.Page as Control);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["EMPLOYEEPERMANENTADDRESSID"] = null;
                ViewState["EMPLOYEELOCALADDRESSID"] = null;
                ViewState["EMPLOYEEID"]=null;
                if (Request.QueryString["empid"] != null)
                {
                    ViewState["EMPLOYEEID"] = Request.QueryString["empid"].ToString();
                    SetEmployeePrimaryDetails(ViewState["EMPLOYEEID"].ToString());
                    SetEmployeeAddress(ViewState["EMPLOYEEID"].ToString());
                }

                else
                {
                    ucError.ErrorMessage = " Please Select a Employee ";
                    ucError.Visible = true;
                    return;

                }
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
  
                LocalAddress.Country = "97";
                PermanentAddress.Country = "97";                
                ddlCountry_TextChanged(null, null);

                Page.ClientScript.RegisterStartupScript(
                typeof(CrewPersonalAddress),
                "ScriptDoFocus",
                SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),
                true);
            }
            UserControlCountry cntry = (UserControlCountry)PermanentAddress.FindControl("ddlCountry");
            DropDownList ddl = (DropDownList)cntry.FindControl("ddlCountry");
            ddl.TextChanged += new EventHandler(ddlCountry_TextChanged);
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
    protected void CrewAddressMain_TabStripCommand(object sender, EventArgs e)
    {
    }
    protected void chkCopyAddress_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        UserControlCommonAddress suc = (chk.ID == "chkLocalCopyAddress" ? LocalAddress : PermanentAddress);
        UserControlCommonAddress tuc = (chk.ID == "chkLocalCopyAddress" ? PermanentAddress : LocalAddress);
        int i = chk.ID == "chkLocalCopyAddress" ? 0 : 1;
        try
        {
            if (chk.Checked == true)
            {

                tuc.Address1 = suc.Address1;
                tuc.Address2 = suc.Address2;
                tuc.Address3 = suc.Address3;
                tuc.Address4 = suc.Address4;
                tuc.Country = suc.Country;
                tuc.State = suc.State;
                tuc.City = suc.City;
                tuc.PostalCode = suc.PostalCode;
            }
            else
            {
                tuc.Address1 = "";
                tuc.Address2 = "";
                tuc.Address3 = "";
                tuc.Address4 = "";
                tuc.Country = "";
                tuc.State = "";
                tuc.City = "";
                tuc.PostalCode = "";
                DataTable dt = new DataTable();
                if (ViewState["EMPLOYEEID"] != null)
                {
                    dt = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(Int16.Parse(ViewState["EMPLOYEEID"].ToString())));
                }
                if (dt.Rows.Count > 0)
                {
                    tuc.Address1 = dt.Rows[i]["FLDADDRESS1"].ToString();
                    tuc.Address2 = dt.Rows[i]["FLDADDRESS2"].ToString();
                    tuc.Address3 = dt.Rows[i]["FLDADDRESS3"].ToString();
                    tuc.Address4 = dt.Rows[i]["FLDADDRESS4"].ToString();
                    tuc.Country = dt.Rows[i]["FLDCOUNTRY"].ToString();
                    tuc.State = dt.Rows[i]["FLDSTATE"].ToString();
                    tuc.City = dt.Rows[i]["FLDCITY"].ToString();
                    tuc.PostalCode = dt.Rows[i]["FLDPOSTALCODE"].ToString();
                    if (chk.ID == "chkLocalCopyAddress")
                    {
                        //txtStdCode.Text = dt.Rows[i]["FLDSTDCODE"].ToString();
                        txtPhoneNumber.Text = dt.Rows[i]["FLDPHONENUMBER"].ToString();
                    }
                    else
                    {
                        //txtlocalStdCode.Text = dt.Rows[i]["FLDSTDCODE"].ToString();
                        txtlocalPhoneNumber.Text = dt.Rows[i]["FLDPHONENUMBER"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public void SetEmployeePrimaryDetails(string empid)
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(Int16.Parse(empid));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void SetEmployeeAddress(string empid)
    {
        try
        {
            DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(empid));
            if (dt.Rows.Count > 0)
            {

                PermanentAddress.Address1 = dt.Rows[0]["FLDADDRESS1"].ToString();
                PermanentAddress.Address2 = dt.Rows[0]["FLDADDRESS2"].ToString();
                PermanentAddress.Address3 = dt.Rows[0]["FLDADDRESS3"].ToString();
                PermanentAddress.Address4 = dt.Rows[0]["FLDADDRESS4"].ToString();

                PermanentAddress.Country = dt.Rows[0]["FLDCOUNTRY"].ToString();
                PermanentAddress.State = dt.Rows[0]["FLDSTATE"].ToString();
                PermanentAddress.City = dt.Rows[0]["FLDCITY"].ToString();
                PermanentAddress.PostalCode = dt.Rows[0]["FLDPOSTALCODE"].ToString();
                //txtStdCode.Text = dt.Rows[0]["FLDSTDCODE"].ToString();
                txtPhoneNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber.Text = dt.Rows[0]["FLDPHONENUMBER"].ToString();
                txtEmail.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                txtMobileNumber.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber.Text = dt.Rows[0]["FLDMOBILENUMBER"].ToString();
                ViewState["EMPLOYEEPERMANENTADDRESSID"] = dt.Rows[0]["FLDEMPLOYEEADDRESSID"].ToString();
                ucPerRelation.SelectedQuick = dt.Rows[0]["FLDRELATIONNO"].ToString();

                LocalAddress.Address1 = dt.Rows[1]["FLDADDRESS1"].ToString();
                LocalAddress.Address2 = dt.Rows[1]["FLDADDRESS2"].ToString();
                LocalAddress.Address3 = dt.Rows[1]["FLDADDRESS3"].ToString();
                LocalAddress.Address4 = dt.Rows[1]["FLDADDRESS4"].ToString();

                LocalAddress.Country = dt.Rows[1]["FLDCOUNTRY"].ToString();
                LocalAddress.State = dt.Rows[1]["FLDSTATE"].ToString();
                LocalAddress.City = dt.Rows[1]["FLDCITY"].ToString();
                LocalAddress.PostalCode = dt.Rows[1]["FLDPOSTALCODE"].ToString();
                txtlocalPhoneNumber.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtlocalPhoneNumber.Text = dt.Rows[1]["FLDPHONENUMBER"].ToString();
                ViewState["EMPLOYEELOCALADDRESSID"] = dt.Rows[1]["FLDEMPLOYEEADDRESSID"].ToString();

                txtPhoneNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtPhoneNumber2.Text = dt.Rows[0]["FLDPHONENUMBER2"].ToString();
                txtMobileNumber2.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber2.Text = dt.Rows[0]["FLDMOBILENUMBER2"].ToString();
                txtMobileNumber3.ISDCode = dt.Rows[0]["FLDISDCODE"].ToString();
                txtMobileNumber3.Text = dt.Rows[0]["FLDMOBILENUMBER3"].ToString();
                ucAirport.SelectedAirport = dt.Rows[0]["FLDNEARESTAIRPORT"].ToString();

                txtLocalMobileNumber.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalMobileNumber.Text = dt.Rows[1]["FLDMOBILENUMBER"].ToString();
                txtLocalPhoneNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalPhoneNumber2.Text = dt.Rows[1]["FLDPHONENUMBER2"].ToString();
                txtLocalMobileNumber2.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalMobileNumber2.Text = dt.Rows[1]["FLDMOBILENUMBER2"].ToString();
                txtLocalMobileNumber3.ISDCode = dt.Rows[1]["FLDISDCODE"].ToString();
                txtLocalMobileNumber3.Text = dt.Rows[1]["FLDMOBILENUMBER3"].ToString();
                ucLocRelation.SelectedQuick = dt.Rows[1]["FLDRELATIONNO"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        UserControlCountry cntry = (UserControlCountry)PermanentAddress.FindControl("ddlCountry");
        if (ucAirport.Country != cntry.SelectedCountry)
        {
            ucAirport.Country = cntry.SelectedCountry;
            ucAirport.AirportList = PhoenixRegistersAirport.ListAirportByCountry(General.GetNullableInteger(cntry.SelectedCountry), null);
        }
    }
}
