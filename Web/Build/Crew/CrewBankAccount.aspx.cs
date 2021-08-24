using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewBankAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        CrewMainPersonal.AccessRights = this.ViewState;
        CrewMainPersonal.MenuList = toolbarmain.Show();

        toolbar.AddFontAwesomeButton("../Crew/CrewBankAccount.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewBankAccount')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','Bank Account','Crew/CrewBankAccountList.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWBANKACCOUNT");
        MenuCrewBankAccount.AccessRights = this.ViewState;
        MenuCrewBankAccount.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            SetEmployeePrimaryDetails();
            gvCrewBankAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void gvCrewBankAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDACCOUNTTYPENAME", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDBANKNAME", "FLDBANKIFSCCODE", "FLDINTERMEDIATEBANK" };
        string[] alCaptions = { "Account Type", "Beneficiary", "Account No.", "Seafarer Bank", "IFSC Code", "Intermediate Bank" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewBankAccount.CrewBankAccountSearch(
            Int32.Parse(Filter.CurrentCrewSelection.ToString())
            , sortexpression, sortdirection
            , 1
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount, General.GetNullableInteger(chkShowOnlyActive.Checked == true ? "1" : "0"));
        General.ShowExcel("Bank Account", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void CrewBankAccount_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();

        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;       
        gvCrewBankAccount.Rebind();
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDACCOUNTTYPENAME", "FLDACCOUNTNAME", "FLDACCOUNTNUMBER", "FLDBANKNAME", "FLDBANKIFSCCODE", "FLDINTERMEDIATEBANK" };
        string[] alCaptions = { "Account Type", "Beneficiary", "Account No.", "Seafarer Bank", "IFSC Code", "Intermediate Bank" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewBankAccount.CrewBankAccountSearch(
            Int32.Parse(Filter.CurrentCrewSelection.ToString())
            , sortexpression, sortdirection
            , 1
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount, General.GetNullableInteger(chkShowOnlyActive.Checked == true ? "1" : "0"));
        General.SetPrintOptions("gvCrewBankAccount", "Bank Account", alCaptions, alColumns, ds);
        gvCrewBankAccount.DataSource = ds;
        gvCrewBankAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewBankAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper() == "UPDATE")
            {
                string bankaccountid = ((RadLabel)eeditedItem.FindControl("lblCrewBankAccountId")).Text;
                PhoenixCrewManagement.UpdateEmployeeBankDefault(int.Parse(Filter.CurrentCrewSelection.ToString()), new Guid(bankaccountid));
                BindData();
                gvCrewBankAccount.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvCrewBankAccount_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = sender as GridView;
    //        Label l = (Label)_gridView.Rows[de.RowIndex].FindControl("lblCrewBankAccountId");
    //        PhoenixCrewBankAccount.DeleteCrewBankAccount(new Guid(l.Text));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}
    //protected void gvCrewBankAccount_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string bankaccountid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewBankAccountId")).Text;
    //        PhoenixCrewManagement.UpdateEmployeeBankDefault(int.Parse(Filter.CurrentCrewSelection.ToString()), new Guid(bankaccountid));
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void gvCrewBankAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            //LinkButton sg = (LinkButton)item.FindControl("cmdGenContract");

            //if (sg != null)
            //{
            //    sg.Visible = SessionUtil.CanAccess(this.ViewState, sg.CommandName);
            //    sg.Attributes.Add("onclick", "openNewWindow('chml', 'Contract', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsReportContract.aspx?EmpId=" + drv["FLDEMPLOYEEID"].ToString() + "&SignonoffId=" + drv["FLDSIGNONOFFID"].ToString() + "');return false;");
            //}
            LinkButton db = (LinkButton)item.FindControl("cmdDefault");
            LinkButton cmdEdit = (LinkButton)item.FindControl("cmdEdit");
            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            RadLabel l = (RadLabel)item.FindControl("lblCrewBankAccountId");
            RadLabel app = (RadLabel)item.FindControl("lblApproved");
            int lockyn = 0;
            if (app != null && app.Text != "")
                lockyn = int.Parse(app.Text.Trim());
            LinkButton lb = (LinkButton)item.FindControl("lnkAccountType");
            if (lb != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (lockyn >= 2)
                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountEdit.aspx?id=" + l.Text + "');return false;");
                    else
                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountList.aspx?id=" + l.Text + "');return false;");
                }
                else
                {
                    if (lockyn > 0)
                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountEdit.aspx?id=" + l.Text + "');return false;");
                    else
                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountList.aspx?id=" + l.Text + "');return false;");
                }
            }
            if (cmdEdit != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (lockyn >= 2)
                        cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountEdit.aspx?id=" + l.Text + "');return false;");
                    else
                        cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountList.aspx?id=" + l.Text + "');return false;");
                }
                else
                {
                    if (lockyn > 0)
                        cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountEdit.aspx?id=" + l.Text + "');return false;");
                    else
                        cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', 'Bank Account', '" + Session["sitepath"] + "/Crew/CrewBankAccountList.aspx?id=" + l.Text + "');return false;");
                }
            }
        }
    }

    //protected void gvCrewBankAccount_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDefault");
    //            //  db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //            Label l = (Label)e.Row.FindControl("lblCrewBankAccountId");
    //            Label app = (Label)e.Row.FindControl("lblApproved");
    //            int lockyn = 0;
    //            if (app != null && app.Text != "")
    //                lockyn = int.Parse(app.Text.Trim());
    //            LinkButton lb = (LinkButton)e.Row.FindControl("lnkAccountType");
    //            if (lb != null)
    //            {
    //                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
    //                {
    //                    if (lockyn >= 2)
    //                        lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewBankAccountEdit.aspx?id=" + l.Text + "');return false;");
    //                    else
    //                        lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewBankAccountList.aspx?id=" + l.Text + "');return false;");
    //                }
    //                else
    //                {
    //                    if (lockyn > 0)
    //                        lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewBankAccountEdit.aspx?id=" + l.Text + "');return false;");
    //                    else
    //                        lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'CrewBankAccountList.aspx?id=" + l.Text + "');return false;");
    //                }
    //            }
    //        }
    //    }


    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCrewBankAccount.Rebind();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
