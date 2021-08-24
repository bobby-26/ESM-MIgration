using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.PayRoll;
using System.Web.UI;


public partial class PayRoll_PayRollDeductions80ggainindia : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid Id = Guid.Empty;
    int Employeeid = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = new Guid(Request.QueryString["id"]);
        }
        if (string.IsNullOrWhiteSpace(Request.QueryString["employeeid"]) == false)
        {
            Employeeid = Convert.ToInt32(Request.QueryString["employeeid"]);

        }
        ShowToolBar();

        if (IsPostBack == false)
        {
           
            bindtax();
            bindemployer();
            bindemployee();
            GetEditData();
            ddlemploye.SelectedValue = Employeeid.ToString();
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        if (Id == Guid.Empty)
        {
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        else
        {
            toolbarmain.AddButton("Update", "UPDATE", ToolBarDirection.Right);
        }
        gvTabStrip.MenuList = toolbarmain.Show();
    }
    private void bindtax()
    {
       
        ddltax.DataTextField = "FLDREVISION";
        ddltax.DataValueField = "FLDPAYROLLTAXID";
        ddltax.DataSource = PhoenixPayRollIndia.PayRollTaxIndiaList();
        ddltax.DataBind();
    }
    private void bindemployer()
    {
        ddlemployer.DataTextField = "FLDNAMEOFEMPLOYER";
        ddlemployer.DataValueField = "FLDPAYROLLEMPLOYERID";
        ddlemployer.DataSource = PhoenixPayRollIndia.PayRollEmployerList();
        ddlemployer.DataBind();
    }
    private void bindemployee()
    {
        ddlemploye.DataTextField = "FLDNAME";
        ddlemploye.DataValueField = "FLDEMPLOYEEID";
        ddlemploye.DataSource = PhoenixPayRollIndia.PayRollEmployeeList();
        ddlemploye.DataBind();
    }
    private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollIndia.Deductions80ggaindiaDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ddltax.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ddlemployer.SelectedValue = dr["FLDPAYROLLEMPLOYERID"].ToString();
                ddlemploye.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                txtnameofdone.Text = dr["FLDNAMEOFDONEE"].ToString();
                txtaddress.Text = dr["FLDADDRESS"].ToString();
                txtcity.Text = dr["FLDCITYTOWNDISTRICT"].ToString();
                ddlcountry.SelectedCountry = dr["FLDCOUNTRY"].ToString();
                ucState.SelectedState = dr["FLDSTATE"].ToString();
                txtpincode.Text = dr["FLDPINCODE"].ToString();
                txtpannumber.Text = dr["FLDPANNUMBER"].ToString();
                txteliamt.Text = dr["FLDELIGIBLEAMOUNT"].ToString();
                txtamount.Text = dr["FLDAMOUNT"].ToString();
                txtpercentage.Text = dr["FLDPERCENTAGE"].ToString();
            }
        }
    }
    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
    	try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

          
                if (IsValidReport())
                {
                    ucError.Visible = true;
                    return;
                }

            Guid payRollId = new Guid(ddltax.SelectedItem.Value);
            Guid employerId = new Guid(ddlemployer.SelectedItem.Value);
            int employeeId = Convert.ToInt32(ddlemploye.SelectedItem.Value);

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixPayRollIndia.Deductions80ggaindiaInsert(usercode, payRollId, employerId, employeeId, txtnameofdone.Text,
                    txtaddress.Text, txtcity.Text, int.Parse(ddlcountry.SelectedCountry), int.Parse(ucState.SelectedState), txtpincode.Text, txtpannumber.Text, decimal.Parse(txteliamt.Text), decimal.Parse(txtamount.Text), decimal.Parse(txtpercentage.Text));
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.Deductions80ggaindiaUpdate(usercode, Id, payRollId, employerId, employeeId, txtnameofdone.Text,
                    txtaddress.Text, txtcity.Text, int.Parse(ddlcountry.SelectedCountry), int.Parse(ucState.SelectedState), txtpincode.Text, txtpannumber.Text, decimal.Parse(txteliamt.Text), decimal.Parse(txtamount.Text), decimal.Parse(txtpercentage.Text));
            }
           //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList();", true);
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCountry_Changed(object sender, EventArgs e)
    {
        ucState.SelectedState = "";
        ucState.Country = ddlcountry.SelectedCountry;
        ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlcountry.SelectedCountry));
        // Rebind();
    }
    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please Provide the following details";
        if (string.IsNullOrWhiteSpace(ddltax.SelectedValue))
        {
            ucError.ErrorMessage = "PayRoll Tax is mandatory";
        }

        if (string.IsNullOrWhiteSpace(ddlemployer.SelectedValue))
        {
            ucError.ErrorMessage = "Employer is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlemploye.SelectedValue))
        {
            ucError.ErrorMessage = "Employee is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtnameofdone.Text))
        {
            ucError.ErrorMessage = "Name Of Done is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtaddress.Text))
        {
            ucError.ErrorMessage = "Address is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtcity.Text))
        {
            ucError.ErrorMessage = "City is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlcountry.SelectedCountry))
        {
            ucError.ErrorMessage = "Country is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucState.SelectedState))
        {
            ucError.ErrorMessage = "State is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtpincode.Text))
        {
            ucError.ErrorMessage = "Pin Code is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtpannumber.Text))
        {
            ucError.ErrorMessage = "Pan Number is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txteliamt.Text))
        {
            ucError.ErrorMessage = "Eligible Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtamount.Text))
        {
            ucError.ErrorMessage = "Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtpercentage.Text))
        {
            ucError.ErrorMessage = "Percentage is mandatory";
        }
        return ucError.IsError;
    }
}