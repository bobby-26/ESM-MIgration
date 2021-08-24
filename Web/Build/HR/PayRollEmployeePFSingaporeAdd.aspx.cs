using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.PayRoll;

public partial class PayRoll_PayRollEmployeePFSingapore : PhoenixBasePage
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
            bindtax();
        }
    }
    private void bindtax()
    {

        ddlPayroll.DataTextField = "FLDREVISION";
        ddlPayroll.DataValueField = "FLDPAYROLLTAXID";
        ddlPayroll.DataSource = PhoenixPayRollSingapore.PayRollTaxList();
        ddlPayroll.DataBind();
    }
    private void GetEditData()
    {
        if (Id != Guid.Empty)
        {
            DataSet ds = PhoenixPayRollSingapore.PayRollEmployeePFSingaporeDetail(usercode, Id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count  > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlPayroll.SelectedValue = dr["FLDPAYROLLTAXID"].ToString();
                ucminagevalue.Text = dr["FLDMINAGEVALUE"].ToString();
                ucmaxagevalue.Text = dr["FLDMAXAGEVALUE"].ToString();
                ucminwagevalue.Text = dr["FLDMINWAGEVALUE"].ToString();
                ucmaxwagevalue.Text = dr["FLDMAXWAGEVALUE"].ToString();
                txtformula.Text = dr["FLDFORMULA"].ToString();
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
            Decimal minwagevalue = Convert.ToDecimal(ucminwagevalue.Text);
            Decimal maxwagevalue = Convert.ToDecimal(ucmaxwagevalue.Text);


            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollSingapore.PayRollEmployeePFSingaporeInsert(usercode, payRollId,int.Parse(ucminagevalue.Text), int.Parse(ucmaxagevalue.Text),
                    minwagevalue, maxwagevalue, txtformula.Text);
            }

			if (CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixPayRollSingapore.PayRollEmployeePFSingaporeUpdate(usercode, Id, payRollId, int.Parse(ucminagevalue.Text), int.Parse(ucmaxagevalue.Text),
                   minwagevalue, maxwagevalue, txtformula.Text);
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

        if (string.IsNullOrWhiteSpace(ucminagevalue.Text))
        {
            ucError.ErrorMessage = "Minimum Age Value is mandatory";
        }

        if (string.IsNullOrWhiteSpace(ucmaxagevalue.Text))
        {
            ucError.ErrorMessage = "Maximum Age Value is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucminwagevalue.Text))
        {
            ucError.ErrorMessage = "Minimum Wage Value is mandatory";
        }
        if (string.IsNullOrWhiteSpace(ucmaxwagevalue.Text))
        {
            ucError.ErrorMessage = "Maximum Wage Value is mandatory";
        }

        return ucError.IsError;
    }
}