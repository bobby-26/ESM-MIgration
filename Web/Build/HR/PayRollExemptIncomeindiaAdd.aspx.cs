using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PayRoll;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PayRoll_PayRollExemptIncomeindiaAdd : PhoenixBasePage
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
    private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollIndia.PayRollExemptIncomeindiaDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ddltax.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ddlemployer.SelectedValue = dr["FLDPAYROLLEMPLOYERID"].ToString();
                ddlemploye.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                txtsection.Text = dr["FLDSECTION"].ToString();
                txthead.Text = dr["FLDHEAD"].ToString();
                txtdescription.Text = dr["FLDHEADDESCRIPTION"].ToString();
                txtamt.Text = dr["FLDAMOUNT"].ToString();

            }
        }
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

                PhoenixPayRollIndia.PayRollExemptIncomeindiaInsert(usercode, payRollId, employerId, employeeId, txtsection.Text,
                    txthead.Text, txtdescription.Text, decimal.Parse(txtamt.Text));
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.PayRollExemptIncomeindiaUpdate(usercode, Id, payRollId, employerId, employeeId, txtsection.Text,
                    txthead.Text, txtdescription.Text, decimal.Parse(txtamt.Text));
            }
             // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList();", true);
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
        if (string.IsNullOrWhiteSpace(txtsection.Text))
        {
            ucError.ErrorMessage = "Section is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txthead.Text))
        {
            ucError.ErrorMessage = "Head is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtdescription.Text))
        {
            ucError.ErrorMessage = "Description is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtamt.Text))
        {
            ucError.ErrorMessage = "Amount is mandatory";
        }
    
        return ucError.IsError;
    }
}