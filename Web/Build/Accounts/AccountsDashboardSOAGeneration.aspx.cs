using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardSOAGeneration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsDashboardSOAGeneration.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDebitReference')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageLink("../Accounts/AccountsDashboardSOAGeneration.aspx", "Find", "search.png", "FIND");
            //toolbar.AddImageButton("../Accounts/AccountsDashboardSOAGeneration.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuDebitReference.AccessRights = this.ViewState;
            MenuDebitReference.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            //    txtVendorId.Attributes.Add("style", "visibility:hidden");
            //    txtVenderName.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            if (!IsPostBack)
            {
                vesselAccount();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["type"] = "";
                ViewState["status"] = "";
                ViewState["month"] = "";
                ViewState["year"] = "";
                ViewState["Condition"] = "";

                if (Request.QueryString["Condition"] != null)
                    ViewState["Condition"] = Request.QueryString["Condition"].ToString();

                if (ViewState["Condition"].ToString() == "1")
                {
                    if (Request.QueryString["type"] != null)
                        ViewState["type"] = Request.QueryString["type"].ToString();

                    if (Request.QueryString["status"] != null)
                        ViewState["status"] = Request.QueryString["status"].ToString();
                }
                else
                {
                    if (Request.QueryString["type"] != null && Request.QueryString["type"] == "MR")
                        ViewState["type"] = "Monthly Report";

                    if (Request.QueryString["type"] != null && Request.QueryString["type"] == "NBR")
                        ViewState["type"] = "Non-Budgeted Report";

                    if (Request.QueryString["type"] != null && Request.QueryString["type"] == "PR")
                        ViewState["type"] = "Predelivery Report";

                    if (Request.QueryString["type"] != null && Request.QueryString["type"] == "DD")
                        ViewState["type"] = "Dry Docking Report";

                    if (Request.QueryString["status"] != null && Request.QueryString["status"] == "FLC")
                        ViewState["status"] = "1st Level Checked";

                    if (Request.QueryString["status"] != null && Request.QueryString["status"] == "SLC")
                        ViewState["status"] = "2nd Level Checked";

                    if (Request.QueryString["status"] != null && Request.QueryString["status"] == "PLC")
                        ViewState["status"] = "Pending";
                }
                if (Request.QueryString["month"] != null)
                    ViewState["month"] = Request.QueryString["month"].ToString();

                if (Request.QueryString["year"] != null)
                    ViewState["year"] = Request.QueryString["year"].ToString();

                // ImgSupplierPickList.Enabled = true;
                //  ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=128&txtsupcode=" + txtVendorCode.Text + "', true); ");
                BindLatestYearMonth();

                gvDebitReference.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTCODEDESCRIPTION", "FLDTYPE", "FLDHARDNAME", "FLDQUICKNAME", "FLDDEBITNOTEREFERENCE", "FLDSTATUS", "FLDISQUERY", "FLDURL" };
        string[] alCaptions = { "Account Code", "Account Description", "Type", "Month", "Year", "Debit Note Reference", "Status", "With Queries", "URL" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixDashboardAccounts.DebitNoteReferenceSearchForSOAGenerationDashboard(
                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , sortexpression
                                    , sortdirection
                                    , (int)ViewState["PAGENUMBER"]
                                    , gvDebitReference.PageSize
                                    , ref iRowCount
                                    , ref iTotalPageCount
                                    , null
                                    , General.GetNullableString(ViewState["status"].ToString())
                                    , null
                                    , General.GetNullableInteger(ViewState["month"].ToString())
                                    , General.GetNullableInteger(ViewState["year"].ToString())
                                    , General.GetNullableString(ViewState["type"].ToString())
                                    , null);


        Response.AddHeader("Content-Disposition", "attachment; filename=SOAGeneration.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>SOA Generation</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["DebitNoteReferenceid"] != null && ViewState["DebitNoteReferenceid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsDebitNoteReferenceHistory.aspx?DebitNoteReferenceid=" + ViewState["DebitNoteReferenceid"] + "&qfrom=SOAGeneration");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDebitReference_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                //criteria.Add("rblStatus", rblStatus.SelectedValue);
                //criteria.Add("ddlReportType", ddlReportType.SelectedValue);
                //criteria.Add("ucYear", ucYear.SelectedQuick);
                //criteria.Add("ucMonth", ucMonth.SelectedHard);
                //criteria.Add("rblAccount", rblAccount.SelectedValue);
                //criteria.Add("txtVendorId", txtVendorId.Text);

                Filter.CurrentSOAGeneration = criteria;
                ViewState["PAGENUMBER"] = 1;

                gvDebitReference.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentSOAGeneration = null;

                gvDebitReference.Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTCODEDESCRIPTION", "FLDTYPE", "FLDHARDNAME", "FLDQUICKNAME", "FLDDEBITNOTEREFERENCE", "FLDSTATUS", "FLDISQUERY", "FLDURL" };
        string[] alCaptions = { "Account Code", "Account Description", "Type", "Month", "Year", "Debit Note Reference", "Status", "With Queries", "URL" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds = new DataSet();

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixDashboardAccounts.DebitNoteReferenceSearchForSOAGenerationDashboard(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , sortexpression
                                    , sortdirection
                                    , (int)ViewState["PAGENUMBER"]
                                    , gvDebitReference.PageSize
                                    , ref iRowCount
                                    , ref iTotalPageCount
                                    , null
                                    , General.GetNullableString(ViewState["status"].ToString())
                                    , null
                                    , General.GetNullableInteger(ViewState["month"].ToString())
                                    , General.GetNullableInteger(ViewState["year"].ToString())
                                    , General.GetNullableString(ViewState["type"].ToString())
                                    , null);

        General.SetPrintOptions("gvDebitReference", "Country", alCaptions, alColumns, ds);
        gvDebitReference.DataSource = ds;
        gvDebitReference.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvDebitReference_Sorting(object sender, GridViewSortEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private bool IsValidData(string accountcode, string type, string month, string year, string reference)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(accountcode) == null)
            ucError.ErrorMessage = "Account code is required.";
        if (type == "Dummy")
            ucError.ErrorMessage = "Type is required.";
        if (General.GetNullableInteger(month) == null)
            ucError.ErrorMessage = "Month is required.";
        if (General.GetNullableInteger(year) == null)
            ucError.ErrorMessage = "Year is required.";
        if (General.GetNullableString(reference) == null)
            ucError.ErrorMessage = "Debit note reference is required.";

        return (!ucError.IsError);
    }

    private void SetRowSelection()
    {

        for (int i = 0; i < gvDebitReference.Items.Count; i++)
        {
            if (gvDebitReference.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["DebitNoteReferenceid"].ToString()))
            {
                gvDebitReference.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {

        RadLabel lbl = ((RadLabel)gvDebitReference.Items[rowindex].FindControl("lblDebitReferenceId"));
        if (lbl != null)
            ViewState["DebitNoteReferenceid"] = lbl.Text;
    }


    protected void AccountSelection(object sender, EventArgs e)
    {
        gvDebitReference.Rebind();
    }

    //protected void cmdSearchAccount_Click(object sender, EventArgs e)
    //{
    //    vesselAccount();

    //    gvDebitReference.Rebind();
    //    ViewState["ACCOUNTID"] = rblAccount.SelectedValue.ToString();
    //}


    protected void vesselAccount()
    {
        DataSet ds = new DataSet();
        try
        {
            //ds = PhoenixAccountsSOAGeneration.SOAGenerationVesselAccount(txtAccountSearch.Text);

            //ds.Tables[0].Columns.Add("FLDaccoandept");

            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    dr["FLDaccoandept"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];
            //}

            //rblAccount.DataTextField = "FLDaccoandept";
            //rblAccount.DataValueField = "FLDACCOUNTID";
            //rblAccount.DataSource = ds;
            //rblAccount.DataBind();
            //if (ViewState["ACCOUNTID"] != null && ViewState["ACCOUNTID"].ToString() != string.Empty)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        if ((dr["FLDACCOUNTID"].ToString().Trim()) == (ViewState["ACCOUNTID"].ToString().Trim()))
            //        {
            //            rblAccount.SelectedValue = ViewState["ACCOUNTID"].ToString();
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            //if (dce.CommandName.ToUpper().Equals("BILLING"))
            //{
            //    Response.Redirect("../Accounts/ccountsDebitReferenceUpdateSOAGeneration.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + Request.QueryString["VesselId"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"], true);
            //}
            //else if (dce.CommandName.ToUpper().Equals("REPORT"))
            //{
            //    Response.Redirect("../Accounts/AccountsSoaCheckingVesselReports.aspx?debitnotereference=" + ViewState["debitnotereference"] + "&accountid=" + ViewState["accountid"] + "&Ownerid=" + ViewState["Ownerid"] + "&voucherdetailid=" + ViewState["FLDVOUCHERDETAILID"] + "&VesselId=" + Request.QueryString["VesselId"].ToString() + "&debitnoteid=" + ViewState["debitnotereferenceid"], true);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindLatestYearMonth()
    {
        DataSet ds = PhoenixAccountsSOAGeneration.BindLatestYearMonth();
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    NameValueCollection nvc = Filter.CurrentSOAWithSupporting;
        //    if (nvc != null)
        //    {
        //        //ucYear.SelectedQuick = nvc.Get("ucYear").ToString();
        //        ucYear.SelectedQuick = nvc.Get("ucYear").ToString() == "Dummy" ? "0" : nvc.Get("ucYear").ToString();
        //        //string month = nvc.Get("ucMonth").ToString();                
        //        ucMonth.SelectedHard = nvc.Get("ucMonth").ToString() == "Dummy" ? "0" : nvc.Get("ucMonth").ToString();
        //    }
        //    else
        //    {
        //        DataRow dr = ds.Tables[0].Rows[0];
        //        ViewState["CURRENTYEARCODE"] = dr["FLDYEAR"].ToString();
        //        ViewState["CURRENTMONTHCODE"] = dr["FLDMONTH"].ToString();
        //        ucYear.SelectedQuick = dr["FLDYEAR"].ToString();
        //        ucMonth.SelectedHard = dr["FLDMONTH"].ToString();
        //    }
        //}
    }

    protected void gvDebitReference_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebitReference.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDebitReference_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountCode")).Text
                                    , ((RadComboBox)e.Item.FindControl("ddlType")).SelectedValue
                                    , ((UserControlHard)e.Item.FindControl("ucMonth")).SelectedHard
                                    , ((UserControlQuick)e.Item.FindControl("ucYear")).SelectedQuick
                                    , ((RadTextBox)e.Item.FindControl("txtDebitNoteRef")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , ((RadTextBox)e.Item.FindControl("txtAccountCode")).Text
                                                                            , ((RadComboBox)e.Item.FindControl("ddlType")).SelectedValue
                                                                            , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucMonth")).SelectedHard)
                                                                            , ((RadTextBox)e.Item.FindControl("txtDebitNoteRef")).Text
                                                                            , "Pending"
                                                                            , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucYear")).SelectedQuick)
                                                                            , ((RadTextBox)e.Item.FindControl("txtURL")).Text);

                gvDebitReference.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountCodeEdit")).Text
                    , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue
                    , ((UserControlHard)e.Item.FindControl("ucMonthEdit")).SelectedHard
                    , ((UserControlQuick)e.Item.FindControl("ucYearEdit")).SelectedQuick
                    , ((RadTextBox)e.Item.FindControl("txtDebitReferenceEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text)
                                                                            , ((RadTextBox)e.Item.FindControl("txtAccountCodeEdit")).Text
                                                                            , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue
                                                                            , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucMonthEdit")).SelectedHard)
                                                                            , ((RadTextBox)e.Item.FindControl("txtDebitReferenceEdit")).Text
                                                                            , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucYearEdit")).SelectedQuick)
                                                                            , ((RadTextBox)e.Item.FindControl("txtURLEdit")).Text);




                gvDebitReference.SelectedIndexes.Clear();
                gvDebitReference.EditIndexes.Clear();

                gvDebitReference.Rebind();
            }


            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                LinkButton lblStatementReference = (LinkButton)e.Item.FindControl("lnkStatementReference");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                // RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");
                RadLabel lblOwnerID = (RadLabel)e.Item.FindControl("lblOwnerID");
                Filter.CurrentOwnerBudgetCodeSelection = null;

                Response.Redirect("../Accounts/AccountsDashboardSoaGenerationLineItems.aspx?debitnotereference=" + lblStatementReference.Text + "&accountid=" + lblAccountId.Text + "&Ownerid=" + lblOwnerID.Text + "&debitnoteid=" + lblDebitReferenceId.Text + "&status=" + ViewState["status"].ToString() + "&type=" + ViewState["type"].ToString() + "&year=" + ViewState["year"].ToString() + "&month=" + ViewState["month"].ToString(), true);
            }

            else if (e.CommandName.ToUpper().Equals("1STCHECK"))
            {

                PhoenixAccountsDebitNoteReference.DebitNoteReference1stCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

                gvDebitReference.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("2NDCHECK"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReference2ndCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

                gvDebitReference.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("PUBLISHED"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReferencePublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

                gvDebitReference.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UNPUBLISHED"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceUNPublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

                gvDebitReference.Rebind();
                ucStatus.Text = "Debit Note Reference Unpublished.";
            }

            if (e.CommandName.ToUpper().Equals("STATUS"))
            {
                RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");
                Response.Redirect("../Accounts/AccountsSOAGenerationHistory.aspx?DebitNoteReferenceid=" + lblDebitReferenceId.Text + "&qfrom=DebitNoteReference");
            }
            if (e.CommandName.ToUpper().Equals("BULKSAVE"))
            {
                RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
                RadLabel lblAccount = (RadLabel)e.Item.FindControl("lblAccountCodeDescriptin");
                Response.Redirect("../Accounts/AccountsDashboardDebitReferenceUpdateSOAGeneration.aspx?debitnotereferenceID=" + lblDebitReferenceId.Text + "&AccountID=" + lblAccountId.Text + "&AccountCode=" + lblAccountCode.Text +
                    "&AccountDesc=" + lblAccount.Text + "&status=" + ViewState["status"].ToString() + "&type=" + ViewState["type"].ToString() + "&year=" + ViewState["year"].ToString() + "&month=" + ViewState["month"].ToString(), true);
            }

            if (e.CommandName.ToUpper().Equals("QUERY"))
            {
                RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblOwnerID = (RadLabel)e.Item.FindControl("lblOwnerID");
                LinkButton lblStatementReference = (LinkButton)e.Item.FindControl("lnkStatementReference");
                string scriptpopup = String.Format(
                        "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSoaGenerationQueriesView.aspx?debitnoteid=" + lblDebitReferenceId.Text + "&accountid=" + lblAccountId.Text + "&Ownerid=" + lblOwnerID.Text + "&debitnotereference=" + lblStatementReference.Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");
                LinkButton lblStatementReference = (LinkButton)e.Item.FindControl("lnkStatementReference");
                PhoenixAccountsSOAGeneration.SOAGenerationDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(lblDebitReferenceId.Text), lblStatementReference.Text);

                gvDebitReference.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("RESET"))
            {
                PhoenixAccountsSOAGeneration.SOAGenerationPendingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

                gvDebitReference.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UNTAGVOUCHERS"))
            {
                PhoenixAccountsSoaChecking.UnTagAllVoucherLineItems(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));
                ucStatus.Text = "Vouchers have been Un-Tagged";
            }
            if (e.CommandName.ToUpper().Equals("SOAURL"))
            {
                LinkButton lnk = (LinkButton)e.Item.FindControl("lblURLlink");
                string scriptpopup = String.Format(
                         "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/" + lnk.Text + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }

            if (e.CommandName.ToUpper().Equals("SELECTROW"))
            {

                RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");
                ViewState["DebitNoteReferenceid"] = lblDebitReferenceId.Text;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDebitReference_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                {
                    eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                    if ((drv["FLDSTATUS"].ToString().ToUpper().Equals("FINALIZED") || drv["FLDSTATUS"].ToString().ToUpper().Equals("CANCELLED")) || drv["FLDSTATUS"].ToString().ToUpper().Equals("PUBLISHED"))
                        eb.Visible = false;
                }
                ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton img1stCheck = (ImageButton)e.Item.FindControl("img1stCheck");
                ImageButton img2ndCheck = (ImageButton)e.Item.FindControl("img2ndCheck");
                ImageButton cmdPublished = (ImageButton)e.Item.FindControl("cmdPublished");
                ImageButton cmdUnPublished = (ImageButton)e.Item.FindControl("cmdUnPublished");
                ImageButton cmdUntagVouchers = (ImageButton)e.Item.FindControl("cmdUntagVouchers");
                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                ImageButton cmdReset = (ImageButton)e.Item.FindControl("cmdReset");
                ImageButton cmdBulkSave = (ImageButton)e.Item.FindControl("cmdBulkSave");

                LinkButton lnkQury = (LinkButton)e.Item.FindControl("lnkQueriesYN");
                RadLabel lblQury = (RadLabel)e.Item.FindControl("lblQueriesYN");

                if (cmdDelete != null)
                    cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


                ImageButton cmdGeneratePdf = (ImageButton)e.Item.FindControl("imgGeneratePdf");
                if (cmdGeneratePdf != null) cmdGeneratePdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Accounts/AccountsSOAGenerationSubledgerType.aspx?pdfGen=1&debitnotereference=" + drv["FLDDEBITNOTEREFERENCE"].ToString() + "&debitnotereferenceid=" + drv["FLDDEBINOTEREFERENCEID"].ToString() + "&accountid="
                                        + drv["FLDACCOUNTID"].ToString() + "&Ownerid=" + drv["FLDOWNERID"].ToString() + "&dtkey=" + drv["FLDDTKEY"].ToString() + "&description=" + drv["FLDACCOUNTCODEDESCRIPTION"].ToString() + "&year=" + drv["FLDYEAR"].ToString() + "&month=" + drv["FLDMONTH"].ToString() + "&URL=" + drv["FLDURL"].ToString() + "');return true;");

                if (drv != null && drv["FLDISQUERY"].ToString() == "Yes")
                {
                    lnkQury.Visible = true;
                    lblQury.Visible = false;
                }
                else
                {
                    lnkQury.Attributes.Add("style", "display:none");
                    lblQury.Visible = true;
                }


                if (drv["FLDSTATUS"].ToString() != "Pending")
                {
                    if (cmdUntagVouchers != null)
                        cmdUntagVouchers.Visible = false;
                    if (cmdDelete != null)
                        cmdDelete.Visible = false;
                    if (cmdBulkSave != null)
                        cmdBulkSave.Visible = false;
                }
                if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
                {
                    if (cmdReset != null)
                    {
                        if (drv["FLDSTATUS"].ToString() == "1st Level Checked" || drv["FLDSTATUS"].ToString() == "2nd Level Checked")
                        {
                            cmdReset.Visible = true;

                            string msg = "SOA Status will be updated to Pending.";
                            msg = msg + "Combined PDF and Additional Owner attachments, if already generated, will be deleted.";
                            msg = msg + "Reports that have been verified will be reset to Not Verified. 1st Level and 2nd Level Checker will have to re - confirm SOA";

                            cmdReset.Attributes.Add("onclick", "return fnConfirmDelete(event,'" + msg + "'); return false;");
                        }
                    }
                }

                ImageButton ibtnshowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
                if (ibtnshowAccount != null)
                    ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");

                if ((drv["FLDSTATUS"].ToString().ToUpper().Equals("PUBLISHED") || drv["FLDSTATUS"].ToString().ToUpper().Equals("CANCELLED")))
                {
                    if (img1stCheck != null)
                        img1stCheck.Visible = false;
                    if (img2ndCheck != null)
                        img2ndCheck.Visible = false;
                    if (cmdPublished != null)
                    {
                        cmdPublished.Visible = false;
                        if (drv["FLDSTATUS"].ToString().ToUpper().Equals("PUBLISHED"))
                        {
                            cmdUnPublished.Visible = true;
                        }
                    }
                }

                ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    //att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    //    + PhoenixModule.ACCOUNTS + "&Status=" + drv["FLDSTATUS"].ToString() + "'); return false;");
                    att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Accounts/AccountsStatementRefFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                   + PhoenixModule.ACCOUNTS + "&Status=" + drv["FLDSTATUS"].ToString() + "&debitnotereferenceid=" + drv["FLDDEBINOTEREFERENCEID"].ToString() + "'); return false;");
                }

                RadComboBox ddlStatus = (RadComboBox)e.Item.FindControl("ddlStatusEdit");

                if (drv != null && ddlStatus != null)
                {
                    ddlStatus.SelectedValue = drv["FLDSTATUS"].ToString();
                }
                RadComboBox ddlTypeEdit = (RadComboBox)e.Item.FindControl("ddlTypeEdit");
                UserControlHard ucMonthEdit = (UserControlHard)e.Item.FindControl("ucMonthEdit");
                UserControlQuick ucYearEdit = (UserControlQuick)e.Item.FindControl("ucYearEdit");

                if (drv != null && ddlTypeEdit != null)
                {
                    ddlTypeEdit.SelectedValue = drv["FLDTYPE"].ToString();
                }
                if (drv != null && ucMonthEdit != null)
                    ucMonthEdit.SelectedHard = drv["FLDMONTH"].ToString();

                if (drv != null && ucYearEdit != null)
                    ucYearEdit.SelectedQuick = drv["FLDYEAR"].ToString();


                LinkButton lblURL = (LinkButton)e.Item.FindControl("lblURLlink");
                UserControlToolTip ucToolTiplblURL = (UserControlToolTip)e.Item.FindControl("ucToolTiplblURL");

                if (lblURL != null)
                {
                    lblURL.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTiplblURL.ToolTip + "', 'visible');");
                    lblURL.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTiplblURL.ToolTip + "', 'hidden');");
                }

                if (img1stCheck != null)
                {
                    if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) == null)
                    {
                        img1stCheck.Visible = true;
                    }
                    else
                        img1stCheck.Visible = false;
                }

                if (img2ndCheck != null)
                {
                    if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) != null
                        && General.GetNullableDateTime(drv["FLD2NDCHECKDATE"].ToString()) == null)
                    {
                        img2ndCheck.Visible = true;
                    }
                    else
                        img2ndCheck.Visible = false;
                }

                ImageButton imgPublised = (ImageButton)e.Item.FindControl("cmdPublished");
                ImageButton imgedit = (ImageButton)e.Item.FindControl("cmdEdit");

                if (imgPublised != null)
                {
                    if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) != null
                         && General.GetNullableDateTime(drv["FLD2NDCHECKDATE"].ToString()) != null
                        && General.GetNullableDateTime(drv["FLDPUBLISHEDDATE"].ToString()) == null)
                    {
                        imgPublised.Visible = true;
                    }
                    else
                    {
                        imgPublised.Visible = false;
                    }
                }
                if (General.GetNullableString(drv["FLDSTATUS"].ToString()) == "Published")
                {
                    imgedit.Visible = false;
                }
            }

            if (e.Item is GridFooterItem)
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                UserControlHard ddlHard = (UserControlHard)e.Item.FindControl("ucMonth");
                ddlHard.HardList = PhoenixRegistersHard.ListHard(1, 55, byte.Parse("1"), "");

                ImageButton ibtnshowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
                if (ibtnshowAccount != null)
                    ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");

                UserControlQuick ucYear = (UserControlQuick)e.Item.FindControl("ucYear");
                ucYear.bind();
                ucYear.SelectedQuick = "189";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}
