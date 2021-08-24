using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;


public partial class AccountsInternalMonthlyBillingEmployeeList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvMonthlyBillingCrew.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvMonthlyBillingCrew.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }       
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvMonthlyBillingCrew')", "Print Grid", "icon_print.png", "PRINT");
        MenuCrew.AccessRights = this.ViewState;
        MenuCrew.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
           // ucConfirm.Visible = false;        


            cmdHiddenPick.Attributes.Add("style", "visibility:hidden;");          
            

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState ["VESSELID"]  = null;

            ViewState["txtBudgetIdEdit"] = null;
            ViewState["txtBudgetgroupIdEdit"] = null;
            ViewState["txtBudgetNameEdit"] = null;
            ViewState["txtBudgetCodeEdit"] = null;
            ViewState["txtOwnerBudgetIdEdit"] = null;
            ViewState["txtOwnerBudgetgroupIdEdit"] = null;
            ViewState["txtOwnerBudgetNameEdit"] = null;
            ViewState["txtOwnerBudgetCodeEdit"] = null;
            ViewState["ISPOSTED"] = null;

            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetId.Attributes.Add("style", "visibility:hidden");
            txtBudgetName.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetName.Attributes.Add("style", "visibility:hidden");
            txtOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden");

            if (Request.QueryString["PORTAGEBILLID"] != null && Request.QueryString["PORTAGEBILLID"].ToString() != "")
                ViewState["PORTAGEBILLID"] = Request.QueryString["PORTAGEBILLID"].ToString();
            else
                ViewState["PORTAGEBILLID"] = "";

            if (Request.QueryString["MONTHLYBILLINGITEMID"] != null && Request.QueryString["MONTHLYBILLINGITEMID"].ToString() != "")
                ViewState["MONTHLYBILLINGITEMID"] = Request.QueryString["MONTHLYBILLINGITEMID"].ToString();
            else
                ViewState["MONTHLYBILLINGITEMID"] = "";
            if (Request.QueryString["POSTED"] != null && Request.QueryString["POSTED"].ToString() != "")
                ViewState["POSTED"] = Request.QueryString["POSTED"].ToString();
            else
                ViewState["POSTED"] = "0";
            if (Request.QueryString["internalmonthlybillingid"] != null && Request.QueryString["internalmonthlybillingid"].ToString() != "")
                ViewState["INTERNALMONTHLYBILLINGID"] = Request.QueryString["internalmonthlybillingid"].ToString();
            else
                ViewState["INTERNALMONTHLYBILLINGID"] = "";

            gvMonthlyBillingCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        BindBillingItemData();
        BindData();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Line Items", "LINEITEM", ToolBarDirection.Right);
        if (ViewState["ISPOSTED"].ToString() !="1")
            toolbar.AddButton("Update", "UPDATE",ToolBarDirection.Right);
       
        MenuBudgetTab.Title = "Billing Item Crew List";
        MenuBudgetTab.AccessRights = this.ViewState;
        MenuBudgetTab.MenuList = toolbar.Show();
       
        if (ViewState["VESSELID"] != null)
        {
            btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
        }
        if (ViewState["VESSELID"] != null)
        {
            btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");         //+ "&budgetid=" + lblbudgetid.Text       
        }

        ViewState["VesselId"] = Request.QueryString["vesselId"];//From the query string
       // SetPageNavigator();        
    }

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                if (txtBudgetId.Text == null || txtBudgetId.Text == "")
                {
                    ucError.ErrorMessage = "Select vessel budget code";
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInternalBilling.BillingItemBudgetCodeUpdateAllEmployee(
                            new Guid(ViewState["PORTAGEBILLID"].ToString()),
                            new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString()),
                            General.GetNullableInteger(txtBudgetId.Text),
                            General.GetNullableGuid(txtOwnerBudgetId.Text));

                    BindData();
                    txtBudgetId.Text = null;
                    txtBudgetCode.Text = null;
                    txtOwnerBudgetCode.Text = null;
                    txtOwnerBudgetId.Text  = null;
                    
                
             }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"] + "&internalmonthlybillingid=" + ViewState["INTERNALMONTHLYBILLINGID"] + "&vesselID=" + ViewState["VesselId"], false);
            //Response.Redirect("../Accounts/AccountsInternalMonthlyBillingLineItem.aspx?PORTAGEBILLID=" + ViewState["PORTAGEBILLID"] + "&internalmonthlybillingid=" + ViewState["INTERNALMONTHLYBILLINGID"], false);
        }
    }
    private void BindBillingItemData()
    {
        DataSet ds = PhoenixAccountsInternalBilling.BillingItemEmployeeEdit(new Guid(ViewState["PORTAGEBILLID"].ToString()),
                         new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtBillingItem.Text = ds.Tables[0].Rows[0]["FLDBILLINGITEMDESCRIPTION"].ToString();
            txtRate.Text = ds.Tables[0].Rows[0]["FLDAMOUNT"].ToString();
            ViewState["ISPOSTED"] = ds.Tables[0].Rows[0]["FLDISPOSTED"].ToString();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Serial Number", "Rank", "Name", "Sign On Date", "Sign Off Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsInternalBilling.BillingItemEmployeeSearch(
                                                           new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                           , new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString())
                                                           , sortexpression
                                                           , sortdirection
                                                           , (int)ViewState["PAGENUMBER"]
                                                           , gvMonthlyBillingCrew.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount);

        General.SetPrintOptions("gvMonthlyBillingCrew", "Crew List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
        

            gvMonthlyBillingCrew.DataSource = ds;
            gvMonthlyBillingCrew.VirtualItemCount = iRowCount;

            if (ViewState["MONTHLYBILLINGEMPLOYEEID"] == null)
            {
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = ds.Tables[0].Rows[0]["FLDMONTHLYBILLINGEMPLOYEEID"].ToString();
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            }

            string str = ViewState["ISPOSTED"].ToString();
            if (str != null && str.ToString() == "1")
                gvMonthlyBillingCrew.Columns[7].Visible = false;
            //SetRowSelection();
        }
        
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDSIGNONDATE", "FLDSIGNOFFDATE" };
        string[] alCaptions = { "Serial Number", "Rank", "Name", "Sign On Date", "Sign Off Date" };

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsInternalBilling.BillingItemEmployeeSearch(
                                                            new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                          , new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString())
                                                          , sortexpression
                                                          , sortdirection
                                                          , (int)ViewState["PAGENUMBER"]
                                                          , iRowCount
                                                          , ref iRowCount
                                                          , ref iTotalPageCount);

        General.ShowExcel("Crew List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
   
    protected void MenuCrew_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void gvMonthlyBillingCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMonthlyBillingCrew.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMonthlyBillingCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null)
                {
                    if (ViewState["POSTED"].ToString() == "1")
                        cmdDelete.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
                }
            }
            if (e.Item is GridEditableItem)
            {
                RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
                RadTextBox txtBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
                RadTextBox txtBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
                RadTextBox txtBudgetCodeEdit = (RadTextBox)e.Item.FindControl("txtBudgetCodeEdit");
                RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
                RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
                RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
                RadTextBox txtOwnerBudgetCodeEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit");

                if (ViewState["txtBudgetIdEdit"] != null && txtBudgetIdEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text = (ViewState["txtBudgetIdEdit"].ToString());
                if (ViewState["txtBudgetgroupIdEdit"] != null && txtBudgetgroupIdEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit")).Text = (ViewState["txtBudgetgroupIdEdit"].ToString());
                if (ViewState["txtBudgetNameEdit"] != null && txtBudgetNameEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtBudgetNameEdit")).Text = (ViewState["txtBudgetNameEdit"].ToString());
                if (ViewState["txtBudgetCodeEdit"] != null && txtBudgetCodeEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtBudgetCodeEdit")).Text = (ViewState["txtBudgetCodeEdit"].ToString());
                if (ViewState["txtOwnerBudgetIdEdit"] != null && txtOwnerBudgetIdEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text = (ViewState["txtOwnerBudgetIdEdit"].ToString());
                if (ViewState["txtOwnerBudgetgroupIdEdit"] != null && txtOwnerBudgetgroupIdEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit")).Text = (ViewState["txtOwnerBudgetgroupIdEdit"].ToString());
                if (ViewState["txtOwnerBudgetNameEdit"] != null && txtOwnerBudgetNameEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit")).Text = (ViewState["txtOwnerBudgetNameEdit"].ToString());
                if (ViewState["txtOwnerBudgetCodeEdit"] != null && txtOwnerBudgetCodeEdit != null)
                    ((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit")).Text = (ViewState["txtOwnerBudgetCodeEdit"].ToString());
            }
            if (e.Item is GridEditableItem)
            {
                RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdEdit");
                RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
                RadLabel lblPOBudgetId = (RadLabel)e.Item.FindControl("lblPOBudgetId");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
                ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
                RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
                RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
                RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
                if (txtOwnerBudgetNameEdit != null)
                    txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
                if (txtOwnerBudgetIdEdit != null)
                    txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
                if (txtOwnerBudgetgroupIdEdit != null)
                    txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
                if (ib1 != null && ViewState["VESSELID"] != null)
                {
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                    if (!SessionUtil.CanAccess(this.ViewState, ib1.CommandName)) ib1.Visible = false;
                }
                if (ibtnShowOwnerBudgetEdit != null && ViewState["VESSELID"] != null)
                {
                    ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + (ViewState["txtBudgetIdEdit"] != null ? ViewState["txtBudgetIdEdit"].ToString() : txtBudgetIdEdit.Text) + "', true); ");         //+ "&budgetid=" + lblbudgetid.Text       
                    if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void gvMonthlyBillingCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
      
        //int iRowno = int.Parse(e.CommandArgument.ToString());
        
        //if (e.CommandName.ToUpper().Equals("SORT"))
        //    return;
        //if (e.CommandName.ToUpper().Equals("SELECT"))
        //{
        //    BindCurentCrewValue(iRowno);
        //}
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                RadLabel lblMonthlyBillingEmployeeId = ((RadLabel)e.Item.FindControl("lblMonthlyBillingEmployeeId"));
                if (lblMonthlyBillingEmployeeId != null)
                    ViewState["MONTHLYBILLINGEMPLOYEEID"] = lblMonthlyBillingEmployeeId.Text;
                else
                    ViewState["MONTHLYBILLINGEMPLOYEEID"] = "";
                RadLabel lblName = ((RadLabel)e.Item.FindControl("lblName"));
                if (lblName != null)
                    ViewState["EMPLOYEENAME"] = lblName.Text;

                if (ViewState["PORTAGEBILLID"] != null && ViewState["PORTAGEBILLID"].ToString() != "" && ViewState["MONTHLYBILLINGITEMID"] != null && ViewState["MONTHLYBILLINGITEMID"].ToString() != "" && ViewState["MONTHLYBILLINGEMPLOYEEID"] != null && ViewState["MONTHLYBILLINGEMPLOYEEID"].ToString() != "")
                {
                    //ucConfirm.Visible = true;
                    //ucConfirm.Text = "Do you want to delete this Crew '" + ViewState["EMPLOYEENAME"] + "' from this 'Billing Item'?";
                    //return;
                    ucConfirmmsg.Visible = true;
                    RadWindowManager1.RadConfirm("Do you want to delete this Crew'" + " " + ViewState["EMPLOYEENAME"] + " " + "' from this 'Billing Item'?", "DeleteRecord", 320, 150, null, "Confirm");
                    
                    return;
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                PhoenixAccountsInternalBilling .BillingItemBudgetCodeUpdate(
                        new Guid(((RadLabel)e.Item.FindControl("lblMonthlyBillingLineItemIdEdit")).Text.ToString()),
                        General .GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text),
                        General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text));
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    //protected void gvMonthlyBillingCrew_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    ViewState["txtBudgetIdEdit"] = null;
    //    ViewState["txtBudgetgroupIdEdit"] = null;
    //    ViewState["txtBudgetNameEdit"] = null;
    //    ViewState["txtBudgetCodeEdit"] = null;
    //    ViewState["txtOwnerBudgetIdEdit"] = null;
    //    ViewState["txtOwnerBudgetgroupIdEdit"] = null;
    //    ViewState["txtOwnerBudgetNameEdit"] = null;
    //    ViewState["txtOwnerBudgetCodeEdit"] = null;

    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;


    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void gvMonthlyBillingCrew_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        PhoenixAccountsInternalBilling.BillingItemBudgetCodeUpdate(
    //                                new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMonthlyBillingLineItemIdEdit")).Text.ToString()),
    //                                General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetIdEdit")).Text),
    //                                General.GetNullableGuid(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerBudgetIdEdit")).Text));

    //        _gridView.EditIndex = -1;
    //        BindData();
    //        SetPageNavigator();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

   

    //protected void gvMonthlyBillingCrew_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvMonthlyBillingCrew.SelectedIndex = e.NewSelectedIndex;
    //    BindCurentCrewValue(e.NewSelectedIndex);

    //}

    protected void gvMonthlyBillingCrew_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
     
    }
    
    //private void BindCurentCrewValue(int rowindex)
    //{
    //    try
    //    {
    //        Label lblMonthlyBillingEmployeeId = ((Label)gvMonthlyBillingCrew.Rows[rowindex].FindControl("lblMonthlyBillingEmployeeId"));
    //        if (lblMonthlyBillingEmployeeId != null)
    //            ViewState["MONTHLYBILLINGEMPLOYEEID"] = lblMonthlyBillingEmployeeId.Text;
    //        else
    //            ViewState["MONTHLYBILLINGEMPLOYEEID"] = "";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void ucConfirmmsg_Click(object sender, EventArgs e)
    {
        try
        {

         //   UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
                PhoenixAccountsInternalBilling.BillingItemEmployeeListUpdate(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , new Guid(ViewState["PORTAGEBILLID"].ToString())
                                                               , new Guid(ViewState["MONTHLYBILLINGITEMID"].ToString())
                                                               , new Guid(ViewState["MONTHLYBILLINGEMPLOYEEID"].ToString()));
                ViewState["MONTHLYBILLINGEMPLOYEEID"] = null;
                BindData();
          //  }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            //return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
      //  vessel budget edit
        if (Filter.CurrentPickListSelection.Keys[3].ToString().Contains("txtBudgetIdEdit"))
            ViewState["txtBudgetIdEdit"] = Filter.CurrentPickListSelection.Get(3);
        if (Filter.CurrentPickListSelection.Keys[4].ToString().Contains("txtBudgetgroupIdEdit"))
            ViewState["txtBudgetgroupIdEdit"] = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[2].ToString().Contains("txtBudgetNameEdit"))
            ViewState["txtBudgetNameEdit"] = Filter.CurrentPickListSelection.Get(2);
        if (Filter.CurrentPickListSelection.Keys[1].ToString().Contains("txtBudgetCodeEdit"))
            ViewState["txtBudgetCodeEdit"] = Filter.CurrentPickListSelection.Get(1);
        //owner budget edit
        if (Filter.CurrentPickListSelection.Keys[3].ToString().Contains("txtOwnerBudgetIdEdit"))
            ViewState["txtOwnerBudgetIdEdit"] = Filter.CurrentPickListSelection.Get(3);
        if (Filter.CurrentPickListSelection.Keys[4].ToString().Contains("txtOwnerBudgetgroupIdEdit"))
            ViewState["txtOwnerBudgetgroupIdEdit"] = Filter.CurrentPickListSelection.Get(4);

        if (Filter.CurrentPickListSelection.Keys[2].ToString().Contains("txtOwnerBudgetNameEdit"))
            ViewState["txtOwnerBudgetNameEdit"] = Filter.CurrentPickListSelection.Get(2);
        if (Filter.CurrentPickListSelection.Keys[1].ToString().Contains("txtOwnerBudgetCodeEdit"))
            ViewState["txtOwnerBudgetCodeEdit"] = Filter.CurrentPickListSelection.Get(1);

        Rebind();        
    }
    protected void Rebind()
    {
        gvMonthlyBillingCrew.SelectedIndexes.Clear();
        gvMonthlyBillingCrew.EditIndexes.Clear();
        gvMonthlyBillingCrew.DataSource = null;
        gvMonthlyBillingCrew.Rebind();
    }

}
