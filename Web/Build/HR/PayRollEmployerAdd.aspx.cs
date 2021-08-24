using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollEmployer : PhoenixBasePage
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
            ddlPayRollListPopulate();
            ddlEmployeeListPopulate();
            //GetStateList();
            GetEditData();
        }
    }

    private void ddlPayRollListPopulate()
    {
        ddlPayroll.DataSource = PhoenixPayRollIndia.PayRollTaxIndiaList();
        ddlPayroll.DataTextField = "FLDREVISION";
        ddlPayroll.DataValueField = "FLDPAYROLLTAXID";
        ddlPayroll.DataBind();
    }

    private void ddlEmployeeListPopulate()
    {
        ddlEmployee.DataSource = PhoenixPayRollIndia.PayRollEmployeeList();
        ddlEmployee.DataTextField = "FLDNAME";
        ddlEmployee.DataValueField = "FLDEMPLOYEEID";
        ddlEmployee.DataBind();
    }

    private void GetStateList()
    {
        ddlState.SelectedState = "";
        ddlState.StateList = PhoenixRegistersState.ListState(usercode, General.GetNullableInteger("97"));
    }

    private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollIndia.PayRollEmployerDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count  > 0)
            {
      //          FLDPAYROLLEMPLOYERID ,
		    //FLDPAYROLLTAXID ,
		    //FLDEMPLOYEEID ,
		    //FLDNAMEOFEMPLOYER ,
		    //FLDNATUREOFEMPLOYMENT ,
		    //FLDADDRESSOFEMPLOYER ,
		    //FLDTOWNCITY ,
		    //FLDSTATE ,
		    //FLDPINCODE ,
		    //FLDTANNUMBEROFEMPLOYER
                DataRow dr = ds.Tables[0].Rows[0];
                ddlPayroll.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ddlEmployee.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                txtNatureOfEmployement.Text = dr["FLDNATUREOFEMPLOYMENT"].ToString();
                txtName.Text = dr["FLDNAMEOFEMPLOYER"].ToString();
                txtAddress.Text = dr["FLDADDRESSOFEMPLOYER"].ToString();
                txtState.Text = dr["FLDSTATE"].ToString();
                txtCity.Text = dr["FLDTOWNCITY"].ToString();
                txtPincode.Text = dr["FLDPINCODE"].ToString();
                txtTanNo.Text = dr["FLDTANNUMBEROFEMPLOYER"].ToString();
            }
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

            Guid payRollId = new Guid(ddlPayroll.SelectedItem.Value);
            int employeeId = Convert.ToInt32(ddlEmployee.SelectedItem.Value);
            //int state = Convert.ToInt32(ddlState.SelectedState);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollIndia.PayRollEmployerInsert(usercode, payRollId, employeeId, txtName.Text, txtNatureOfEmployement.Text, 
                    txtAddress.Text, txtCity.Text, txtState.Text, txtPincode.Text, txtTanNo.Text);
            }

			if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.PayRollEmployerUpdate(usercode, Id, payRollId, employeeId, txtName.Text, txtNatureOfEmployement.Text,
                    txtAddress.Text, txtCity.Text, txtState.Text, txtPincode.Text, txtTanNo.Text);
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList();", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please add the following details";

        if (string.IsNullOrWhiteSpace(ddlPayroll.SelectedValue))
        {
            ucError.ErrorMessage = "Payroll is mandatory";
        }

        if (string.IsNullOrWhiteSpace(ddlEmployee.SelectedValue))
        {
            ucError.ErrorMessage = "Employee is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            ucError.ErrorMessage = "Name is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtAddress.Text))
        {
            ucError.ErrorMessage = "Address is mandatory";
        }

    

        if (string.IsNullOrWhiteSpace(txtState.Text))
        {
            ucError.ErrorMessage = "State is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtPincode.Text))
        {
            ucError.ErrorMessage = "Pin code is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtTanNo.Text))
        {
            ucError.ErrorMessage = "TAN is mandatory";
        }

        return ucError.IsError;
    }
}