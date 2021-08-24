using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsCaptainCash : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                NameValueCollection nvc = Filter.CurrentCaptainCashOfficeFilter;

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Voucher", "VOUCHER");
                //toolbarmain.AddButton("Line Items", "LINEITEM");
                //toolbarmain.AddButton("View Draft", "VIEWDRAFT");
                //toolbarmain.AddButton("D11", "D11");
                //toolbarmain.AddButton("Captain Cash", "CAPTAINCASH");
                //MenuCaptainCashMain.AccessRights = this.ViewState;
                //MenuCaptainCashMain.MenuList = toolbarmain.Show();
                //MenuCaptainCashMain.SelectedMenuIndex = 0;

                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddImageButton("../Accounts/AccountsCaptainCash.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarmain.AddImageLink("javascript:CallPrint('gvCaptainCash')", "Print Grid", "icon_print.png", "PRINT");
                toolbarmain.AddImageButton("../Accounts/AccountsCaptainCash.aspx", "Find", "search.png", "FIND");
                toolbarmain.AddImageButton("../Accounts/AccountsCaptainCash.aspx", "Clear", "clear-filter.png", "CLEAR");
                MenuCaptainCash.AccessRights = this.ViewState;
                MenuCaptainCash.MenuList = toolbarmain.Show();

                ViewState["CTMID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["balanceid"] = null;
                ViewState["vesselid"] = null;
                if (Request.QueryString["balanceid"] != null)
                    ViewState["balanceid"] = Request.QueryString["balanceid"].ToString();

                if (Request.QueryString["vesselid"] != null)
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();

                ddlStatus.SelectedValue = nvc != null && !string.IsNullOrEmpty(nvc["ddlStatus"]) ? nvc["ddlStatus"] : "1";
                ddlVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableInteger(""), 1);
                ddlVessel.SelectedVessel = nvc != null && !string.IsNullOrEmpty(nvc["ddlVessel"]) ? nvc["ddlVessel"] : string.Empty;
                txtVoucherNo.Text = nvc != null && !string.IsNullOrEmpty(nvc["txtVoucherNo"]) ? nvc["txtVoucherNo"] : string.Empty;
                txtFromDate.Text = nvc != null && !string.IsNullOrEmpty(nvc["txtFromDate"]) ? nvc["txtFromDate"] : string.Empty;
                txtToDate.Text = nvc != null && !string.IsNullOrEmpty(nvc["txtToDate"]) ? nvc["txtToDate"] : string.Empty;

                gvCaptainCash.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void FilterChanged(object sender, EventArgs e)
    {

        ViewState["PAGENUMBER"] = 1;
        NameValueCollection criteria = new NameValueCollection();
        criteria.Clear();
        criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
        criteria.Add("ddlStatus", ddlStatus.SelectedValue);
        criteria.Add("txtVoucherNo", txtVoucherNo.Text);
        criteria.Add("txtFromdate", txtFromDate.Text);
        criteria.Add("txtToDate", txtToDate.Text);

        Filter.CurrentCaptainCashOfficeFilter = criteria;

        BindData();
    }
    protected void MenuCaptainCash_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTCODE", "FLDFROMDATE", "FLDTODATE", "FLDVOUCHERNUMBER", "FLDPOSEDDATE" };
                string[] alCaptions = { "Vessel Name", "Vessel Account Code", "From Date", "To Date", "Voucher Number", "Posted Date" };

                NameValueCollection nvc = Filter.CurrentCaptainCashOfficeFilter;

                DataSet ds = PhoenixAccountsCaptainCash.CaptainCashSearch(General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                            , General.GetNullableString(nvc != null ? nvc["txtVoucherNo"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromdate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , int.Parse(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , 1
                            , iRowCount, ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("Captain Cash", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVessel.SelectedVessel = string.Empty;
                txtFromDate.Text = string.Empty;
                txtVoucherNo.Text = string.Empty;
                txtToDate.Text = string.Empty;
                ddlStatus.SelectedValue = "1";
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("txtVoucherNo", txtVoucherNo.Text);
                criteria.Add("txtFromdate", txtFromDate.Text);
                criteria.Add("txtToDate", txtToDate.Text);

                Filter.CurrentCaptainCashOfficeFilter = criteria;
                //BindData();
                Rebind();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("txtVoucherNo", txtVoucherNo.Text);
                criteria.Add("txtFromdate", txtFromDate.Text);
                criteria.Add("txtToDate", txtToDate.Text);

                Filter.CurrentCaptainCashOfficeFilter = criteria;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCaptainCashMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            //if (dce.CommandName.ToUpper().Equals("VOUCHER"))
            //{
            //    Response.Redirect("../Accounts/AccountsCaptainCash.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            //}
            //if (dce.CommandName.ToUpper().Equals("LINEITEM"))
            //{
            //    Response.Redirect("../Accounts/AccountsCaptainCashVoucher.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            //}
            //if (dce.CommandName.ToUpper().Equals("VIEWDRAFT"))
            //{
            //    Response.Redirect("../Accounts/AccountsCaptainCashDraft.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            //}
            //if (dce.CommandName.ToUpper().Equals("D11"))
            //{
            //    Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=D11");
            //}
            //if (dce.CommandName.ToUpper().Equals("CAPTAINCASH"))
            //{
            //    Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=CAPTAINCASH");
            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDVESSELNAME", "FLDVESSELACCOUNTCODE", "FLDFROMDATE", "FLDTODATE", "FLDVOUCHERNUMBER", "FLDPOSEDDATE" };
            string[] alCaptions = { "Vessel Name", "Vessel Account Code", "From Date", "To Date", "Voucher Number", "Posted Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentCaptainCashOfficeFilter;

            DataSet ds = PhoenixAccountsCaptainCash.CaptainCashSearch(General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                                     , General.GetNullableString(nvc != null ? nvc["txtVoucherNo"] : string.Empty)
                                                                     , General.GetNullableDateTime(nvc != null ? nvc["txtFromdate"] : string.Empty)
                                                                     , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                                                                     , int.Parse(nvc != null ? nvc["ddlStatus"] : ddlStatus.SelectedValue)
                                                                     , sortexpression, sortdirection
                                                                     , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                     , gvCaptainCash.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvCaptainCash", "Captain Cash", alCaptions, alColumns, ds);
            gvCaptainCash.DataSource = ds;
            gvCaptainCash.VirtualItemCount = iRowCount;

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    gvCaptainCash.DataSource = ds;
            //    gvCaptainCash.DataBind();

            //    if (ViewState["balanceid"] == null)
            //    {
            //        ViewState["balanceid"] = ds.Tables[0].Rows[0]["FLDBALANCEID"].ToString();
            //        ViewState["vesselid"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            //        gvCaptainCash.SelectedIndex = 0;
            //    }
            //    SetRowSelection();
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];
            //    ShowNoRecordsFound(dt, gvCaptainCash);
            //}

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCaptainCash.SelectedIndexes.Clear();
        gvCaptainCash.EditIndexes.Clear();
        gvCaptainCash.DataSource = null;
        gvCaptainCash.Rebind();
    }
    protected void gvCaptainCash_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCaptainCash.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCaptainCash_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (!e.CommandName.ToUpper().Equals("SELECT"))
            {


                if (e.CommandName.ToUpper().Equals("CONFIRM"))
                {
                    string balanceid = ((RadLabel)e.Item.FindControl("lblBalanceId")).Text;
                    string accountid = ((RadLabel)e.Item.FindControl("lblAccountId")).Text;

                    if (!IsValidCaptainCash(balanceid, accountid))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsCaptainCash.CaptainCashConfirm(new Guid(balanceid), int.Parse(accountid));
                    ucStatus.Text = "Confirmed Successfully";
                    ucStatus.Visible = true;
                    BindData();
                }
                if (e.CommandName.ToUpper().Equals("DRAFT"))
                {
                    string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                    string balanceid = ((RadLabel)e.Item.FindControl("lblBalanceId")).Text;
                    Response.Redirect("AccountsCaptainCashDraft.aspx?balanceid=" + balanceid + "&vesselid=" + vslid, true);
                }
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    Guid id = new Guid(((RadLabel)e.Item.FindControl("lblBalanceId")).Text);
                    string VesselId = ((RadLabel)e.Item.FindControl("lblEditVesselId")).Text;
                    string VesselAccountCode = ((RadComboBox)e.Item.FindControl("ddlAccountDetails")).Text;
                    //  string VesselAccountCode = ((DropDownList)e.Item.FindControl("ddlAccountDetails")).Text;
                    string balanceid = ((RadLabel)e.Item.FindControl("lblBalanceId")).Text;
                    string accountid = ((RadLabel)e.Item.FindControl("lblAccountId")).Text;
                    if (!IsValidCaptainCash(balanceid, accountid))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixAccountsCaptainCash.OfficeCaptainCashUpdate(id, int.Parse(accountid));
                    Rebind();
                }
                if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    string balanceid = ((RadLabel)e.Item.FindControl("lblBalanceId")).Text;

                    string vslid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                    PhoenixAccountsCaptainCash.DeleteOfficeCaptainCash(new Guid(balanceid), int.Parse(vslid));
                    Rebind();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCaptainCash_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton cd = (LinkButton)e.Item.FindControl("cmdConfirm");
            LinkButton dv = (LinkButton)e.Item.FindControl("cmdDraft");
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            RadLabel lblConfirmFlag = (RadLabel)e.Item.FindControl("lblConfirmFlag");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblfrom = (RadLabel)e.Item.FindControl("lblfrom");
            RadLabel lblTo = (RadLabel)e.Item.FindControl("lblTo");

            if (cd != null && dv != null && lblConfirmFlag != null)
            {
                if (lblConfirmFlag.Text == "0")
                {
                    cd.Visible = false;
                    dv.Visible = true;
                    del.Visible = true;
                }
                else
                {
                    cd.Visible = true;
                    dv.Visible = false;
                    del.Visible = false;
                }
            }

            if (ed != null && dv != null && lblConfirmFlag != null)
            {
                if (lblConfirmFlag.Text == "0")
                {
                    ed.Visible = false;
                    dv.Visible = true;
                    del.Visible = true;
                }
                else
                {
                    ed.Visible = true;
                    dv.Visible = false;
                    del.Visible = false;
                }
            }
            RadComboBox ddlAccountDetails = (RadComboBox)e.Item.FindControl("ddlAccountDetails");
            //   DropDownList ddlAccountDetails = (DropDownList)e.Item.FindControl("ddlAccountDetails");

            RadLabel lblEditVesselId = (RadLabel)e.Item.FindControl("lblEditVesselId");
            RadLabel lblEditAccountId = (RadLabel)e.Item.FindControl("lblEditAccountId");

            if (ddlAccountDetails != null && lblEditVesselId != null)
            {
                ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                    General.GetNullableInteger(lblEditVesselId.Text), 1);
                ddlAccountDetails.DataBind();

                ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                if (lblEditAccountId != null)
                    ddlAccountDetails.SelectedValue = lblEditAccountId.Text;

            }
        }


    }

    //protected void gvCaptainCash_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
    //       // ImageButton cv = (ImageButton)e.Row.FindControl("cmdView");
    //        ImageButton cd = (ImageButton)e.Row.FindControl("cmdConfirm");
    //        ImageButton dv = (ImageButton)e.Row.FindControl("cmdDraft");
    //        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //        Label lblConfirmFlag = (Label)e.Row.FindControl("lblConfirmFlag");
    //        Label lblVesselId = (Label)e.Row.FindControl("lblVesselId");
    //        Label lblfrom = (Label)e.Row.FindControl("lblfrom");
    //        Label lblTo = (Label)e.Row.FindControl("lblTo");
    //        //if (cv != null)
    //        //{
    //        //    cv.Attributes.Add("onclick", "parent.Openpopup('codehelpView', '', '../VesselAccounts/VesselAccountsPettyCashGeneral.aspx?fromdate=" + lblfrom.Text + "&todate=" + lblTo.Text + "&vesselid=" + lblVesselId.Text + "');return false;");
    //        //}
    //        if (cd != null && dv != null && lblConfirmFlag != null)
    //        {
    //            if (lblConfirmFlag.Text == "0")
    //            {
    //                cd.Visible = false;
    //                dv.Visible = true;
    //                del.Visible = true;
    //            }
    //            else
    //            {
    //                cd.Visible = true;
    //                dv.Visible = false;
    //                del.Visible = false;
    //            }
    //        }

    //        if (ed != null && dv != null && lblConfirmFlag != null)
    //        {
    //            if (lblConfirmFlag.Text == "0")
    //            {
    //                ed.Visible = false;
    //                dv.Visible = true;
    //                del.Visible = true;
    //            }
    //            else
    //            {
    //                ed.Visible = true;
    //                dv.Visible = false;
    //                del.Visible = false;
    //            }
    //        }

    //        DropDownList ddlAccountDetails = (DropDownList)e.Row.FindControl("ddlAccountDetails");

    //        Label lblEditVesselId = (Label)e.Row.FindControl("lblEditVesselId");
    //        Label lblEditAccountId = (Label)e.Row.FindControl("lblEditAccountId");

    //        if (ddlAccountDetails != null && lblEditVesselId != null)
    //        {
    //            ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
    //                General.GetNullableInteger(lblEditVesselId.Text), 1);
    //            ddlAccountDetails.DataBind();

    //            ddlAccountDetails.Items.Insert(0, new ListItem("--Select--", ""));

    //            if (lblEditAccountId != null)
    //                ddlAccountDetails.SelectedValue = lblEditAccountId.Text;

    //        }
    //    }
    //}
    //protected void gvCaptainCash_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (!e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            if (e.CommandName.ToUpper().Equals("SORT"))
    //                return;

    //            GridView _gridView = (GridView)sender;
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //            if (e.CommandName.ToUpper().Equals("CONFIRM"))
    //            {
    //                string balanceid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBalanceId")).Text;
    //                string accountid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountId")).Text;

    //                if (!IsValidCaptainCash(balanceid, accountid))
    //                {
    //                    ucError.Visible = true;
    //                    return;
    //                }
    //                PhoenixAccountsCaptainCash.CaptainCashConfirm(new Guid(balanceid), int.Parse(accountid));
    //                ucStatus.Text = "Confirmed Successfully";
    //                ucStatus.Visible = true;
    //                BindData();
    //            }
    //            if (e.CommandName.ToUpper().Equals("DRAFT"))
    //            {
    //                string vslid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
    //                string balanceid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBalanceId")).Text;
    //                Response.Redirect("AccountsCaptainCashDraft.aspx?balanceid=" + balanceid + "&vesselid=" + vslid, true);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvCaptainCash_Sorting(object sender, GridViewSortEventArgs se)
    //{
    //    ViewState["STORETYPEID"] = null;
    //    ViewState["SORTEXPRESSION"] = se.SortExpression;

    //    if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
    //        ViewState["SORTDIRECTION"] = 1;
    //    else
    //        ViewState["SORTDIRECTION"] = 0;
    //    BindData();
    //}
    //protected void gvCaptainCash_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        string balanceid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBalanceId")).Text;

    //        string vslid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
    //        PhoenixAccountsCaptainCash.DeleteOfficeCaptainCash(new Guid(balanceid), int.Parse(vslid));
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}
    //protected void gvCaptainCash_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        GridViewRow gr = _gridView.Rows[nCurrentRow];
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        string VesselId = ((Label)gr.FindControl("lblEditVesselId")).Text;
    //        string VesselAccountCode = ((DropDownList)gr.FindControl("ddlAccountDetails")).Text;
    //        if (!IsValidVesselAccountCode(VesselAccountCode))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixAccountsCaptainCash.OfficeCaptainCashUpdate(id, int.Parse(VesselAccountCode));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    private bool IsValidVesselAccountCode(string vesselaccountcode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(vesselaccountcode).HasValue)
            ucError.ErrorMessage = "Vessel account code is required";

        return (!ucError.IsError);
    }

    private bool IsValidCaptainCash(string BalanceId, string AccountId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableGuid(BalanceId).HasValue)
        {
            ucError.ErrorMessage = "Captain cash closing Id is Required";
        }
        if (!General.GetNullableInteger(AccountId).HasValue)
        {
            ucError.ErrorMessage = "Account code is required";
        }
        return (!ucError.IsError);
    }


}
