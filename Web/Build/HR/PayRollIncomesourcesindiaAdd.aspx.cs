using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.PayRoll;
using System.Web.UI;


public partial class PayRollIncomesourcesindia : PhoenixBasePage
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

        ddltax.DataSource = PhoenixPayRollIndia.PayRollTaxIndiaList();
        ddltax.DataTextField = "FLDREVISION";
        ddltax.DataValueField = "FLDPAYROLLTAXID";
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
            DataSet ds = PhoenixPayRollIndia.IncomesourcesindiaDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ddltax.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ddlemployer.SelectedValue = dr["FLDPAYROLLEMPLOYERID"].ToString();
                ddlemploye.SelectedValue = dr["FLDEMPLOYEEID"].ToString();
                txtincomehead.Text = dr["FLDINCOMEHEAD"].ToString();
                txtamount.Text = dr["FLDAMOUNT"].ToString();
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

                PhoenixPayRollIndia.IncomesourcesindiaInsert(usercode, payRollId, employerId, employeeId, txtincomehead.Text, decimal.Parse(txtamount.Text));
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollIndia.IncomesourcesindiaUpdate(usercode, Id, payRollId, employerId, employeeId, txtincomehead.Text, decimal.Parse(txtamount.Text));
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
        if (string.IsNullOrWhiteSpace(txtincomehead.Text))
        {
            ucError.ErrorMessage = "Income Head is mandatory";
        }
        if (string.IsNullOrWhiteSpace(txtamount.Text))
        {
            ucError.ErrorMessage = "Amount is mandatory";
        }
        return ucError.IsError;
    }
}