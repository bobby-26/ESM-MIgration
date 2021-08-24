using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollPerQuisite : PhoenixBasePage
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
            ddlPayRollListPopulate();
            ddlEmployeeListPopulate();
            ddlEmployerPopulate();
            GetEditData();
            ddlEmployee.SelectedValue = Employeeid.ToString();
        }
    }

	 private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollIndia.PayRollPerQuisiteDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count  > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlPayroll.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ddlEmployee.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                ddlEmployer.SelectedValue = dr["FLDPAYROLLEMPLOYERID"].ToString();
                txtNatureOfSalary.Text = dr["FLDNATUREOFSALARY"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtAmount.Text = dr["FLDAMOUNT"].ToString();
            }
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

    private void ddlEmployerPopulate()
    {
        ddlEmployer.DataSource = PhoenixPayRollIndia.PayRollEmployerList();
        ddlEmployer.DataTextField = "FLDNAMEOFEMPLOYER"; 
        ddlEmployer.DataValueField = "FLDPAYROLLEMPLOYERID";
        ddlEmployer.DataBind();
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
            Guid employerId = new Guid(ddlEmployer.SelectedItem.Value);
            int employeeId = Convert.ToInt32(ddlEmployee.SelectedItem.Value);
            Decimal amount = Convert.ToDecimal(txtAmount.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollIndia.PayRollPerQuisiteInsert(usercode, payRollId, employerId ,employeeId, txtNatureOfSalary.Text, txtDescription.Text, amount);
            }

            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.PayRollPerQuisiteUpdate(usercode, Id, txtNatureOfSalary.Text, txtDescription.Text, amount);
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

    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please add the following details";

        if (string.IsNullOrWhiteSpace(ddlPayroll.SelectedValue))
        {
            ucError.ErrorMessage = "Payroll is mandatory";
        }

        if (string.IsNullOrWhiteSpace(ddlEmployer.SelectedValue))
        {
            ucError.ErrorMessage = "Employer is mandatory";
        }

        if (string.IsNullOrWhiteSpace(ddlEmployee.SelectedValue))
        {
            ucError.ErrorMessage = "Employee is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtNatureOfSalary.Text))
        {
            ucError.ErrorMessage = "Nature of Salary is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtDescription.Text))
        {
            ucError.ErrorMessage = "Description is mandatory";
        }

        if (string.IsNullOrWhiteSpace(txtAmount.Text))
        {
            ucError.ErrorMessage = "Amount is mandatory";
        }

        return ucError.IsError;
    }
}