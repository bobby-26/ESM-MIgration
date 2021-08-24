using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.PayRoll;
using SouthNests.Phoenix.Registers;

public partial class PayRoll_PayRollPFSalarySingapore : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    int Employeeid = 0;
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
        if (string.IsNullOrWhiteSpace(Request.QueryString["employeeid"]) == false)
        {
            Employeeid = Convert.ToInt32(Request.QueryString["employeeid"]);

        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            bindtax();
            bindemployee();
            bindemployer();
            bindpfcontribution();
            GetEditData();
            ddlEmployee.SelectedValue = Employeeid.ToString();
        }
    }
    private void bindtax()
    {

        ddlPayroll.DataTextField = "FLDREVISION";
        ddlPayroll.DataValueField = "FLDPAYROLLTAXID";
        ddlPayroll.DataSource = PhoenixPayRollSingapore.PayRollTaxList();
        ddlPayroll.DataBind();
    }
    private void bindemployee()
    {

        ddlEmployee.DataTextField = "FLDNAME";
        ddlEmployee.DataValueField = "FLDEMPLOYEEID";
        ddlEmployee.DataSource = PhoenixPayRollSingapore.PayRollEmployeeList();
        ddlEmployee.DataBind();
        if (ddlEmployee.Items.Count > 0)
        {
            ddlEmployee.Items[0].Selected = true;
        }
    }
    private void bindemployer()
    {

        ddlEmployer.DataTextField = "FLDCOMPANYNAME";
        ddlEmployer.DataValueField = "FLDPAYROLLEMPLOYERID";
        ddlEmployer.DataSource = PhoenixPayRollSingapore.PayRollEmployerList();
        ddlEmployer.DataBind();
    }
    private void bindpfcontribution()
    {

        ddlPfcontribution.DataTextField = "FLDAGE";
        ddlPfcontribution.DataValueField = "FLDPAYROLLPFCONTRIBUTIONID";
        ddlPfcontribution.DataSource = PhoenixPayRollSingapore.PayRollPFContributionList();
        ddlPfcontribution.DataBind();
    }
    private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollSingapore.PayRollPFContributionSingaporeDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ddlPayroll.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ddlEmployer.SelectedValue = dr["FLDEMPLOYERID"].ToString();
                ddlEmployee.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                ddlPfcontribution.SelectedValue = dr["FLDPAYROLLPFCONTRIBUTIONID"].ToString();
                ucdate.Text = dr["FLDDATE"].ToString();
                txtNatureofsalary.Text = dr["FLDNATUREOFSALARY"].ToString();
                ucordinarywages.Text = dr["FLDORDINARYWAGES"].ToString();
                txtnatureofemployement.Text = dr["FLDNATUREOFEMPLOYMENT"].ToString();
                ucemployeestatus.Text = dr["FLDEMPLOYEESTATUS"].ToString();
                if (ds.Tables[0].Rows[0]["FLDISACTIVESALARY"].ToString() == "1")
                    chkactivesalary.Checked = true;
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
            Guid employerId = new Guid(ddlEmployer.SelectedItem.Value);
            int employeeId = Convert.ToInt32(ddlEmployee.SelectedItem.Value);
            Guid pfcontributionId = new Guid(ddlPfcontribution.SelectedItem.Value);
            Decimal ordinarywages = Convert.ToDecimal(ucordinarywages.Text);


            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollSingapore.PayRollPFContributionSingaporeInsert(usercode, payRollId, employerId, employeeId, pfcontributionId,
                    DateTime.Parse(ucdate.Text), txtNatureofsalary.Text, ordinarywages,
                    txtnatureofemployement.Text, int.Parse(ucemployeestatus.Text), chkactivesalary.Checked.Equals(true) ? 1 : 0
                    );
            }

            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollSingapore.PayRollPFContributionSingaporeUpdate(usercode, Id, payRollId, employerId, employeeId, pfcontributionId,
                    DateTime.Parse(ucdate.Text), txtNatureofsalary.Text, ordinarywages,
                    txtnatureofemployement.Text, int.Parse(ucemployeestatus.Text), chkactivesalary.Checked.Equals(true) ? 1 : 0
                    );
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
            ucError.ErrorMessage = "Pay Roll is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlEmployer.SelectedValue))
        {
            ucError.ErrorMessage = "Employer is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlEmployee.SelectedValue))
        {
            ucError.ErrorMessage = "Employee is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlPfcontribution.SelectedValue))
        {
            ucError.ErrorMessage = "PF Contribution is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucdate.Text))
        {
            ucError.ErrorMessage = "Date is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtNatureofsalary.Text))
        {
            ucError.ErrorMessage = "Nature of salary is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucordinarywages.Text))
        {
            ucError.ErrorMessage = "Ordinary Wages is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtnatureofemployement.Text))
        {
            ucError.ErrorMessage = "Nature of employement is mandatory";
        }

        return ucError.IsError;
    }
}