using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollEmployeePFAmountSingapore : PhoenixBasePage
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
            bindemployee();
            bindpfcontribution();
        }
    }
    private void bindemployee()
    {

        ddlEmployee.DataTextField = "FLDNAME";
        ddlEmployee.DataValueField = "FLDEMPLOYEEID";
        ddlEmployee.DataSource = PhoenixPayRollSingapore.PayRollEmployeeList();
        ddlEmployee.DataBind();
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
            DataSet ds = PhoenixPayRollSingapore.PayRollEmployeePFAmountSingaporeDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count  > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ddlEmployee.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                ddlPfcontribution.SelectedValue = dr["FLDPAYROLLPFCONTRIBUTIONID"].ToString();

                txtmonth.Text = dr["FLDMONTH"].ToString();
                txtyear.Text = dr["FLDYEAR"].ToString();
                ucdate.Text = dr["FLDDATE"].ToString();
                ucowamt.Text = dr["FLDOWAMOUNT"].ToString();
                ucowcptamt.Text = dr["FLDOWCPFAMOUNT"].ToString();
                ucowemployeramt.Text = dr["FLDOWEMPLOYERAMOUNT"].ToString();
                ucowemployeeamt.Text = dr["FLDOWEMPLOYEEAMOUNT"].ToString();

                ucawamt.Text = dr["FLDAWAMOUNT"].ToString();
                ucawcpfamt.Text = dr["FLDAWCPFAMOUNT"].ToString();
                ucawemployeramt.Text = dr["FLDAWEMPLOYERAMOUNT"].ToString();
                ucawemployeeamt.Text = dr["FLDAWEMPLOYEEAMOUNT"].ToString();

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
            int employeeId = Convert.ToInt32(ddlEmployee.SelectedItem.Value);
            Guid pfcontributionId = new Guid(ddlPfcontribution.SelectedItem.Value);

            Decimal owamt = Convert.ToDecimal(ucowamt.Text);
            Decimal owcptamt = Convert.ToDecimal(ucowcptamt.Text);
            Decimal owemployeramt = Convert.ToDecimal(ucowemployeramt.Text);
            Decimal owemployeeamt = Convert.ToDecimal(ucowemployeeamt.Text);

            Decimal awamt = Convert.ToDecimal(ucawamt.Text);
            Decimal awcpfamt = Convert.ToDecimal(ucawcpfamt.Text);
            Decimal awemployeramt = Convert.ToDecimal(ucawemployeramt.Text);
            Decimal awemployeeamt = Convert.ToDecimal(ucawemployeeamt.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollSingapore.PayRollEmployeePFAmountSingaporeInsert(usercode,  employeeId, pfcontributionId,
                  txtmonth.Text,int.Parse( txtyear.Text), DateTime.Parse(ucdate.Text), owamt, owcptamt, owemployeramt,
                  owemployeeamt, awamt, awcpfamt, awemployeramt, awemployeeamt
                   );
            }

			if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollSingapore.PayRollEmployeePFAmountSingaporeUpdate(usercode, Id, employeeId, pfcontributionId,
                  txtmonth.Text, int.Parse(txtyear.Text), DateTime.Parse(ucdate.Text), owamt, owcptamt, owemployeramt,
                  owemployeeamt, awamt, awcpfamt, awemployeramt, awemployeeamt
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
     
        if (string.IsNullOrWhiteSpace(ddlEmployee.SelectedValue))
        {
            ucError.ErrorMessage = "Employee is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ddlPfcontribution.SelectedValue))
        {
            ucError.ErrorMessage = "PF Contribution is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtmonth.Text))
        {
            ucError.ErrorMessage = "Month is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtyear.Text))
        {
            ucError.ErrorMessage = "Year is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucdate.Text))
        {
            ucError.ErrorMessage = "Date is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucowamt.Text))
        {
            ucError.ErrorMessage = "OW Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucowcptamt.Text))
        {
            ucError.ErrorMessage = "OW CPF Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucowemployeramt.Text))
        {
            ucError.ErrorMessage = "OW Employer Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucowemployeeamt.Text))
        {
            ucError.ErrorMessage = "OW Employee Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucawamt.Text))
        {
            ucError.ErrorMessage = "AW Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucawcpfamt.Text))
        {
            ucError.ErrorMessage = "AW CPF Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucawemployeramt.Text))
        {
            ucError.ErrorMessage = "AW Employer Amount is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucawemployeeamt.Text))
        {
            ucError.ErrorMessage = "AW Employee Amount is mandatory";
        }
        return ucError.IsError;
    }
}