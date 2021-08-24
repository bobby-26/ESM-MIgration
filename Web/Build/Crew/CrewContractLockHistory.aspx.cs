using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Reports;

public partial class CrewContractLockHistory : PhoenixBasePage
{
    string contid = string.Empty;
    string planid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Contract", "CONTTRACT");
            toolbar.AddButton("Reimbursement/Deduction", "REIM");
            toolbar.AddButton("Contract Paper", "CONTRACTPAPER");
            toolbar.AddButton("Revise Contract", "REVISION");
            toolbar.AddButton("Contract Lock History", "LOCKHISTORY");
            MenuCrewContract.MenuList = toolbar.Show();
            MenuCrewContract.SelectedMenuIndex = 4;

            contid = Request.QueryString["cid"];
            if (!string.IsNullOrEmpty(Request.QueryString["planid"]))
                planid = Request.QueryString["planid"];
            EditContractDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvContractLockHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
          
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataTable dt = PhoenixCrewContract.ListCrewContractLock(new Guid(contid));
        gvContractLockHistory.DataSource = dt;
    }
    protected void CrewContract_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            DataTable dt = PhoenixReportsCommon.LogoBind();
            if (CommandName.ToUpper().Equals("CONTRACTPAPER"))
            {
                if (dt.Rows[0]["FLDLICENCECODE"].ToString() == "ESM")
                {
                    if (Request.QueryString["app"] == "0")
                    {
                        Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWCONTRACT&contractid=" +
                            (new Guid(contid)) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                            + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&plan=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                    }
                    else
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=APPOINTMENTLETTER&contractid=" +
                          (new Guid(contid)) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                          + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&plan=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                    }
                }
                else
                {
                    if (Request.QueryString["app"] == "0")
                    {
                        Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWCONTRACT&contractid=" +
                            (new Guid(contid)) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                            + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&plan=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                    }
                    else
                    {
                        Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=APPOINTMENTLETTEROTHER&contractid=" +
                          (new Guid(contid)) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                          + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&plan=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                    }
                }
            }
            else if (CommandName.ToUpper().Equals("REIM"))
            {
                Response.Redirect("CrewReimbursementContract.aspx?" + Request.QueryString.ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("REVISION"))
            {
                Response.Redirect("CrewContractRevision.aspx?" + Request.QueryString.ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("CONTTRACT"))
            {
                Response.Redirect("CrewContract.aspx?" + Request.QueryString.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditContractDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(Request.QueryString["empid"]), General.GetNullableGuid(contid), int.Parse(Request.QueryString["vslid"]));
            if (dt.Rows.Count > 0)
            {
                txtDate.Text = string.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDPAYDATE"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
}
