using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.PayRoll;
using System.Web.UI;
using Telerik.Web.UI;

public partial class PayRoll_PayRollAdvanceTaxindia : PhoenixBasePage
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
            DataSet ds = PhoenixPayRollIndia.PayRollAdvanceTaxindiaDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ddltax.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ddlemployer.SelectedValue = dr["FLDPAYROLLEMPLOYERID"].ToString();
                ddlemploye.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                txtbsrcode.Text = dr["FLDBSRCODE"].ToString();
                txtdateofdeposit.Text = dr["FLDDATEOFDEPOSIT"].ToString();
                txtrefno.Text = dr["FLDREFERENCENUMBER"].ToString();


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
           // int refno = Convert.ToInt32(txtrefno.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixPayRollIndia.PayRollAdvanceTaxindiaInsert(usercode, payRollId, employerId, employeeId,
                 txtbsrcode.Text, DateTime.Parse(txtdateofdeposit.Text), int.Parse(txtrefno.Text));
                 
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.PayRollAdvanceTaxindiaUpdate(usercode, Id, payRollId, employerId, employeeId, txtbsrcode.Text,
                    DateTime.Parse(txtdateofdeposit.Text), int.Parse(txtrefno.Text));
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
        if (string.IsNullOrWhiteSpace(txtbsrcode.Text))
        {
            ucError.ErrorMessage = "BSR code is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtdateofdeposit.Text))
        {
            ucError.ErrorMessage = "Date Of Deposit is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtrefno.Text))
        {
            ucError.ErrorMessage = "Reference No is mandatory";
        }
        return ucError.IsError;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}