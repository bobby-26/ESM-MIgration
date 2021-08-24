using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOfficePortageBillPostingBreakUp : PhoenixBasePage
{
    string vslid = string.Empty, pbid = string.Empty, withbudget = string.Empty;

    private const int _firstEditCellIndex = 2;//cell edit instead of row edit

    // To keep track of the previous row Group Identifier    
    string strPreviousRowID = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            vslid = Request.QueryString["vslid"];
            pbid = Request.QueryString["pbid"];
            withbudget = Request.QueryString["withbudget"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("With Budget", "WITHBUDGET");
            toolbar.AddButton("Without Budget", "WITHOUTBUDGET");
            toolbar.AddButton("Post Voucher", "POSTVOUCHER");
            toolbar.AddButton("Arrears", "ARREARS");
            toolbar.AddButton("Back", "VOUCHER");
            MenuPB.AccessRights = this.ViewState;
            MenuPB.MenuList = toolbar.Show();


            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuPB1.AccessRights = this.ViewState;
            MenuPB1.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"], "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageButton("../Accounts/AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"], "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsOfficePortageBillPostingBreakUp.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"], "Clear Filter", "clear-filter.png", "CLEAR");
            MenuPBExcel.AccessRights = this.ViewState;
            MenuPBExcel.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                MenuPB.SelectedMenuIndex = 0;
                ViewState["DATE"] = null;
                ViewState["BUDGETASSIGNEDYN"] = "1";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvPB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["withbudget"] != null)
                {
                    if (withbudget.Equals("1"))
                    {
                        ViewState["BUDGETASSIGNEDYN"] = "1";
                        MenuPB.SelectedMenuIndex = 0;
                    }
                    if (withbudget.Equals("0"))
                    {
                        ViewState["BUDGETASSIGNEDYN"] = "0";
                        MenuPB.SelectedMenuIndex = 1;
                    }

                }
                ddldocument.DataSource = PhoenixAccountsOfficePortageBill.ListPortageBillDocumentType();
                ddldocument.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvPB.SelectedIndexes.Clear();
        gvPB.EditIndexes.Clear();
        gvPB.DataSource = null;
        gvPB.Rebind();
    }

    protected void MenuPB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("AccountsOfficePortageBillPosting.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("WITHOUTBUDGET"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["BUDGETASSIGNEDYN"] = "0";
                Rebind();
                //  BindData(byte.Parse(ViewState["BUDGETASSIGNEDYN"].ToString()));
                txtBudgetCode.Text = "";
                txtBudgetgroupId.Text = "";
                txtBudgetId.Text = "";
                txtBudgetName.Text = "";

            }
            else if (CommandName.ToUpper().Equals("WITHBUDGET"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["BUDGETASSIGNEDYN"] = "1";
                Rebind();
                //BindData(byte.Parse(ViewState["BUDGETASSIGNEDYN"].ToString()));
                txtBudgetCode.Text = "";
                txtBudgetgroupId.Text = "";
                txtBudgetId.Text = "";
                txtBudgetName.Text = "";
            }
            if (CommandName.ToUpper().Equals("POSTVOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsOfficePortageBillPostingDraft.aspx?pno=" + Request.QueryString["pno"] + "&vslid=" + Request.QueryString["vslid"] + "&pbid=" + Request.QueryString["pbid"]);
            }
            if (CommandName.ToUpper().Equals("ARREARS"))
            {
                Response.Redirect("AccountsOfficeWagesAdjustmentPosting.aspx?pbid=" + pbid + "&vslid=" + vslid + "&pno=" + Request.QueryString["pno"], true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPB1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string dtkey = string.Empty;
                string budgetid = txtBudgetId.Text;
                string ownerbudgetid = txtAccountCode1.Text.Trim();
                foreach (GridDataItem gv in gvPB.Items)
                {
                    RadCheckBox sel = (RadCheckBox)gv.FindControl("chkSelect");
                    if (sel.Checked == true)
                    {
                        dtkey += ((RadLabel)gv.FindControl("lbldtKey")).Text + ",";
                    }
                }
                if (!IsValidBudget(budgetid, ownerbudgetid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOfficePortageBill.UpdateOfficePortageBillPostingBreakUpBulk(int.Parse(vslid), new Guid(pbid), dtkey, int.Parse(budgetid), General.GetNullableGuid(txtownerbudgetMapidEdit.Text));
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPBExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string date = string.Empty;
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDCOMPONENTNAME", "FLDDESCRIPTION", "FLDACCOUNTCODEDESCRIPTION", "FLDBUDGETCODE", "FLDAMOUNT", "DOCUMENTTYPE" };
                string[] alCaptions = { "File No", "Name", "Rank", "Component", "Description", "Account Code", "Budget Code", "Amount(USD)", "Type" };

                DataTable dt = PhoenixAccountsOfficePortageBill.OfficePortageBillPostingBreakUpSearch(new Guid(pbid), int.Parse(vslid), byte.Parse(ViewState["BUDGETASSIGNEDYN"].ToString())
                    , txtfileno.Text, txtempname.Text, General.GetNullableInteger(ddlRank.SelectedRank)
                    , General.GetNullableString(txtdescripton.Text), General.GetNullableInteger(ddldocument.SelectedValue)
                    , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("PortageBill", dt, alColumns, alCaptions, sortdirection, sortexpression);

            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtdescripton.Text = "";
                ddldocument.SelectedValue = "";
                txtfileno.Text = "";
                txtempname.Text = "";

                ddlRank.SelectedRank = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        BindData(byte.Parse(ViewState["BUDGETASSIGNEDYN"].ToString()));
        btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30', false); ");
        imgShowAccount1.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + Convert.ToString(ViewState["PRINCIPAL"]) + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");
        if (ViewState["BUDGETASSIGNEDYN"].ToString() == "1")
            MenuPB.SelectedMenuIndex = 0;
        else
            MenuPB.SelectedMenuIndex = 1;
    }
    public void BindData(byte BudgetAssignedYN)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataTable dt = PhoenixAccountsOfficePortageBill.OfficePortageBillPostingBreakUpSearch(new Guid(pbid), int.Parse(vslid), BudgetAssignedYN
                            , txtfileno.Text, txtempname.Text, General.GetNullableInteger(ddlRank.SelectedRank)
                            , General.GetNullableString(txtdescripton.Text), General.GetNullableInteger(ddldocument.SelectedValue)
                            , sortexpression, sortdirection, Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvPB.PageSize, ref iRowCount, ref iTotalPageCount);
            strPreviousRowID = string.Empty;
            gvPB.DataSource = dt;
            gvPB.VirtualItemCount = iRowCount;

            if (dt.Rows.Count > 0)
            {
                txtvesselperiod.Text = dt.Rows[0]["FLDVESSELNAME"] + " - Period Of " + string.Format("{0:dd/MMM/yyyy}", dt.Rows[0]["FLDSTARTDATE"]) + " to " + string.Format("{0:dd/MMM/yyyy}", dt.Rows[0]["FLDENDDATE"]);
                ViewState["PRINCIPAL"] = dt.Rows[0]["FLDPRINCIPALACCOUNT"].ToString();
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPB_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            strPreviousRowID = drv["FLDEMPLOYEEID"].ToString();
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30', false); return false;");

            LinkButton sp = (LinkButton)e.Item.FindControl("cmdSplit");
            if (sp != null) sp.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsOfficePortageBillSplit.aspx?vslid=" + drv["FLDVESSELID"].ToString() + "&empid=" + drv["FLDEMPLOYEEID"].ToString() + "&pbid=" + drv["FLDPORTAGEBILLID"].ToString() + "&dtkey=" + drv["FLDDTKEY"].ToString() + "&amt=" + drv["FLDAMOUNT"].ToString() + "'); return false;");

            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtAccountDescription");
            if (tb1 != null) tb1.Attributes.Add("style", "display:none");
            tb1 = (RadTextBox)e.Item.FindControl("txtAccountId");
            if (tb1 != null) { tb1.Attributes.Add("style", "display:none"); }

            ImageButton ib2 = (ImageButton)e.Item.FindControl("btnShowAccountEdit");
            if (ib2 != null) ib2.Attributes.Add("onclick", "showPickList('spnPickListCompanyAccountEdit', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx?ignoreiframe=true', true); return false;");

            ImageButton ib3 = (ImageButton)e.Item.FindControl("imgShowAccount1Edit");
            if (ib3 != null && (RadTextBox)e.Item.FindControl("txtBudgetIdEdit") != null)
            {
                RadTextBox budgetid = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
                ib3.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + drv["FLDPRINCIPALID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + budgetid.Text + "', true); ");

            }

        }

    }

    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPB.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string pbid = ((RadLabel)e.Item.FindControl("lblPBId")).Text;
                string dtkey = ((RadLabel)e.Item.FindControl("lblDtKey")).Text;
                string accountid = ((RadTextBox)e.Item.FindControl("txtAccountId")).Text;
                string budgetid = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
                string ownerbudgetid = ((RadTextBox)e.Item.FindControl("txtownerbudgetMapidEdit")).Text;
                if (!IsValidBreakUp(accountid, budgetid, ownerbudgetid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOfficePortageBill.UpdateOfficePortageBillPostingBreakUp(int.Parse(vslid), new Guid(pbid), int.Parse(budgetid),
                                                                                        new Guid(dtkey), int.Parse(accountid), General.GetNullableGuid(ownerbudgetid));

                Rebind();
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidBreakUp(string accountid, string budgetid, string ownerbudgetid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(accountid) == null)
            ucError.ErrorMessage = "Account code is required.";

        if (!General.GetNullableInteger(budgetid).HasValue)
            ucError.ErrorMessage = "Budget code is required.";

        if (!General.GetNullableGuid(ownerbudgetid).HasValue)
            ucError.ErrorMessage = "Owner Budget code is required.";

        return (!ucError.IsError);
    }
    private bool IsValidBudget(string budgetid, string ownerbudgetid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(budgetid).HasValue)
            ucError.ErrorMessage = "Budget code is required.";
        if (ownerbudgetid == null || ownerbudgetid == "")
            ucError.ErrorMessage = "Owners Budget Code is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        int nRow = (int)ViewState["ACCOUNTIDROW"];

        ((RadTextBox)gvPB.Items[nRow].FindControl("txtBudgetIdEdit")).Text = "";
        ((RadTextBox)gvPB.Items[nRow].FindControl("txtBudgetCodeEdit")).Text = "";

    }
    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvPB$ctl00$ctl02$ctl01$chkSelectAll")
        {
            RadCheckBox chkAll = new RadCheckBox();
            foreach (GridHeaderItem headerItem in gvPB.MasterTableView.GetItems(GridItemType.Header))
            {
                chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkSelectAll"); // Get the header checkbox
            }
            if (chkAll != null)
            {
                foreach (GridDataItem gv in gvPB.Items)
                {
                    RadCheckBox sel = (RadCheckBox)gv.FindControl("chkSelect");
                    if (sel != null)
                    {
                        if (chkAll.Checked == true)
                            sel.Checked = true;
                        else
                            sel.Checked = false;
                    }
                }
            }
        }
    }
}
