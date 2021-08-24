using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewReimbursementContract : PhoenixBasePage
{
    string empid = string.Empty;
    string planid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            empid = Request.QueryString["empid"];
            if (!string.IsNullOrEmpty(Request.QueryString["planid"]))
                planid = Request.QueryString["planid"];
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Contract", "CONTRACT");
            toolbar.AddButton("Reimbursement/Deduction", "REIM");
            toolbar.AddButton("Contract Paper", "SHOWREPORT");
            toolbar.AddButton("Revise Contract", "REVISION");
            toolbar.AddButton("Contract Lock History", "LOCKHISTORY");
            MenuReim.AccessRights = this.ViewState;
            MenuReim.MenuList = toolbar.Show();
            MenuReim.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                ViewState["CONTRACTID"] = null;

                EditContractDetails();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuReim_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (Request.QueryString["app"] == "0")
                {
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWCONTRACT&contractid=" + (new Guid(ViewState["CONTRACTID"].ToString()))
                        + "&showmenu=0&empid=" + Request.QueryString["empid"] + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&planid=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                }
                else
                {
                    Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=APPOINTMENTLETTER&contractid=" +
                      (new Guid(ViewState["CONTRACTID"].ToString())) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                      + "&rnkid=" + Request.QueryString["rnkid"] + "&vslid=" + Request.QueryString["vslid"] + "&date=" + Request.QueryString["date"] + "&planid=" + planid + "&accessfrom=1&showword=no&showexcel=no", false);
                }
            }
            else if (CommandName.ToUpper().Equals("CONTRACT"))
            {
                Response.Redirect("CrewContract.aspx?" + Request.QueryString.ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("REVISION"))
            {
                Response.Redirect("CrewContractRevision.aspx?" + Request.QueryString.ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("LOCKHISTORY"))
            {
                Response.Redirect("CrewContractLockHistory.aspx?" + Request.QueryString.ToString(), false);
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
            DataTable dt = PhoenixCrewContract.EditCrewContract(int.Parse(empid), General.GetNullableGuid(Request.QueryString["cid"]), int.Parse(Request.QueryString["vslid"]));
            if (dt.Rows.Count > 0)
            {
                ViewState["CONTRACTID"] = dt.Rows[dt.Rows.Count - 1]["FLDCONTRACTID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataTable dt = PhoenixCrewReimbursement.ListReimbursementContract(int.Parse(empid), null, byte.Parse(chkShowInactive.Checked ? "0" : "1"));
        gvRem.DataSource = dt;
    }
    protected void Rebind()
    {
        gvRem.SelectedIndexes.Clear();
        gvRem.EditIndexes.Clear();
        gvRem.DataSource = null;
        gvRem.Rebind();

        gvRemUnapp.SelectedIndexes.Clear();
        gvRemUnapp.EditIndexes.Clear();
        gvRemUnapp.DataSource = null;
        gvRemUnapp.Rebind();
    }
    protected void BindData(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvRem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvRemUnapp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindDataReimbursement();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataReimbursement()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewReimbursement.SearchCrewReimbursement(General.GetNullableInteger(Request.QueryString["vslid"])
                                                            , General.GetNullableInteger(Request.QueryString["empid"]), null, 0, null
                                                            , sortexpression, sortdirection
                                                            , (int)ViewState["PAGENUMBER"], gvRemUnapp.PageSize
                                                            , ref iRowCount, ref iTotalPageCount);
        gvRemUnapp.DataSource = dt;
        gvRemUnapp.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvRem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string id = ((RadLabel)e.Item.FindControl("lblid")).Text.Trim();
                string chk = ((RadCheckBox)e.Item.FindControl("chkContract")).Checked==true? "1" :"0";
                PhoenixCrewReimbursement.UpdateReimbursementContract(new Guid(id), General.GetNullableGuid(chk=="1" ? ViewState["CONTRACTID"].ToString() : string.Empty));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
        }
    }
}
