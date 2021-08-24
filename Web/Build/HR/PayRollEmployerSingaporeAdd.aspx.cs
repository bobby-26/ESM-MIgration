using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.PayRoll;
using SouthNests.Phoenix.Registers;

using System.Web.UI;

public partial class PayRoll_PayRollEmployerSingapore : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid Id = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["id"]) == false)
        {
            Id = new Guid(Request.QueryString["id"]);
        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            GetEditData();
           
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

  
    private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollSingapore.EmployerSingaporeDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];


                txtcompanyName.Text = dr["FLDCOMPANYNAME"].ToString();
           
                txtAddress.Text = dr["FLDADDRESSOFEMPLOYER"].ToString();
                txtCity.Text = dr["FLDTOWNCITY"].ToString();
                ddlcountry.SelectedCountry = dr["FLDCOUNTRY"].ToString();
                ucState.SelectedState = dr["FLDSTATE"].ToString();
                txtPincode.Text = dr["FLDPINCODE"].ToString();
                txtCSNNo.Text = dr["FLDCSNNUMBER"].ToString();
                txtunique.Text = dr["FLDUNIQUEENTITYNUMBER"].ToString();
                txtsingpass.Text = dr["FLDSINGPASS"].ToString();
                
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

       
            if (CommandName.ToUpper().Equals("SAVE"))
                {

                PhoenixPayRollSingapore.EmployerSingaporeInsert(usercode, txtcompanyName.Text,
                         txtAddress.Text, txtCity.Text, int.Parse(ddlcountry.SelectedCountry), int.Parse(ucState.SelectedState), txtPincode.Text, txtCSNNo.Text, txtunique.Text, txtsingpass.Text);
                }
                if (CommandName.ToUpper().Equals("UPDATE"))
                {
                PhoenixPayRollSingapore.EmployerSingaporeUpdate(usercode, Id, txtcompanyName.Text,

                     txtAddress.Text, txtCity.Text, int.Parse(ddlcountry.SelectedCountry), int.Parse(ucState.SelectedState), txtPincode.Text, txtCSNNo.Text, txtunique.Text, txtsingpass.Text);
            }
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
        //ucState.Country = ddlcountry.SelectedCountry;
        ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlcountry.SelectedCountry));

    }
    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please add the following details";
       

        if (string.IsNullOrWhiteSpace(txtcompanyName.Text))
        {
            ucError.ErrorMessage = "Company Name is mandatory";
        }

        
  
        if (string.IsNullOrWhiteSpace(txtAddress.Text))
        {
            ucError.ErrorMessage = "Address is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtCity.Text))
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
        if (string.IsNullOrWhiteSpace(txtPincode.Text))
        {
            ucError.ErrorMessage = "Pin code is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtCSNNo.Text))
        {
            ucError.ErrorMessage = "CSN no. is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtunique.Text))
        {
            ucError.ErrorMessage = "Unique entity no. is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtsingpass.Text))
        {
            ucError.ErrorMessage = "Sign Pass is mandatory";
        }

        return ucError.IsError;
    }
}