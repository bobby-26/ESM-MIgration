using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;


public partial class AccountsProjectBillingChargingGeneral : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvCountry.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvCountry.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsProjectBillingChargingGeneral.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCountry')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../Accounts/AccountsProjectBillingChargingFilter.aspx", "Find", "search.png", "FIND");
        //    toolbar.AddImageLink("javascript:Openpopup('codehelp1','','AccountsProjectBillingChargingFilter.aspx')", "Find", "search.png", "FIND");
            toolbar.AddImageLink("javascript:openNewWindow('codehelpactivity','','Accounts/AccountsProjectBillingChargingFilter.aspx')", "Find", "search.png", "FIND");
            //toolbar.AddImageButton("../Registers/RegistersCountry.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Voucher Posting", "VOUCHERPOSTING", ToolBarDirection.Right);
            toolbarmain.AddButton("General", "GENERAL",ToolBarDirection.Right);
            MenuProjectBillingMain.Title = "General";
            MenuProjectBillingMain.AccessRights = this.ViewState;
            MenuProjectBillingMain.MenuList = toolbarmain.Show();

            MenuProjectBillingMain.SelectedMenuIndex = 1; 
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = null;
                ViewState["COMPANYID"] = null;

                gvCountry.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
          //  BindData();
           
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    BindData();
    //    SetPageNavigator();
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPROJECTBILLINGNAME", "FLDPROJECTBILLINGGROUP", "FLDVESSELNAME", "FLDISSUEDATE", "FLDISSUEDQTY", "FLDBILLEDQTY", "FLDSELLINGAMOUNT", "FLDCOMPANY", "FLDVOUCHERNUMBER"};
        string[] alCaptions = { "Project Billing Name", "Project Billing Group", "Vessel Issued", "Date Issued", "Issued Quantity", "Billed Quantity", "Rate", "Billing  Company", "Voucher Number"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentSelectedProjectBillingVoucher != null)
        {

            NameValueCollection nvc = Filter.CurrentSelectedProjectBillingVoucher;

            ds = PhoenixAccountsProjectBilling.ProjectBillingVoucherPostSearch(nvc != null ? General.GetNullableString(nvc.Get("txtProjectBillingName")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("billinggrouplist")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("ucVessel")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtIssueFromDate")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtIssueToDate")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlLiabilitycompany")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("voucherstatus")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("cancelled")) : null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"],
                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                ref iRowCount,
                ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixAccountsProjectBilling.ProjectBillingVoucherPostSearch(null
                , null
                , null
                , null
                , null
                , null
                , null
                , 0
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                ref iRowCount,
                ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=ProjectBillingCharging.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Project Billing Charging</h3></td>");
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
               
                ViewState["PAGENUMBER"] = 1;
                BindData();
              
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                
                BindData();
              
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuProjectBillingMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                
            }
            else if (CommandName.ToUpper().Equals("VOUCHERPOSTING"))
            {
                StringBuilder strprojectbillingissueid = new StringBuilder();
                GridDataItem headerItem = gvCountry.MasterTableView.GetItems(GridItemType.Header)[0] as GridDataItem;
             //   GridView gv = (GridView)gvCountry;
                string company = "";
                string vesselid = "";
                string companyid = "";
                foreach (GridDataItem row in gvCountry.MasterTableView.Items)

         //           foreach (GridViewRow row in gv.Rows)
                {

                    CheckBox chk1 = (CheckBox)row.FindControl("chckPostVoucher");

                    if (chk1 != null && chk1.Checked == true)
                    {
                        company = ((RadLabel)row.FindControl("lblBillingCompany")).Text.ToString();
                        vesselid = ((RadLabel)row.FindControl("lblVesselId")).Text.ToString();
                        companyid = ((RadLabel)row.FindControl("lblBillingCompanyId")).Text.ToString();
                        strprojectbillingissueid.Append(((RadLabel)row.FindControl("lblProjectBillingIssueId")).Text.ToString());
                        strprojectbillingissueid.Append(",");

                    }
                }
                if (strprojectbillingissueid.Length > 1)
                    Response.Redirect("../Accounts/AccountsProjectBillingChargingPostVoucher.aspx?projectbillingissueid=" + strprojectbillingissueid + "&company=" + company + "&VESSELID=" + vesselid + "&companyid=" + companyid);
                MenuProjectBillingMain.SelectedMenuIndex = 0;
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
        string[] alColumns = { "FLDACCOUNTCODE", "FLDESMBUDGETCODE", "FLDOWNERBUDGETCODE", "FLDVOUCHERDATE", "FLDDEBITNOTEREFERENCE", "FLDPHOENIXVOUCHER", "FLDREFERENCE", "FLDAMOUNT", "FLDDESCRIPTION", "FLDACCOUNTDESCRIPTION", "FLDBUDGETCODEDESCRIPTION" };
        string[] alCaptions = { "Account Code", "ESM Budget Code", "Owner Budget Code", "Voucher Date", "Debit Note Reference", "Phoenix Voucher", "Reference", "Amount", "Description", "Account Description", "Budget Code Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds = new DataSet();
        
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentSelectedProjectBillingVoucher != null)
        {

            NameValueCollection nvc = Filter.CurrentSelectedProjectBillingVoucher;

            ds = PhoenixAccountsProjectBilling.ProjectBillingVoucherPostSearch( nvc != null ?General.GetNullableString(nvc.Get("txtProjectBillingName")) :null 
                ,nvc != null ? General.GetNullableString(nvc.Get("billinggrouplist")):null 
                ,nvc != null ? General.GetNullableString(nvc.Get("ucVessel")):null
                ,nvc != null ? General.GetNullableDateTime(nvc.Get("txtIssueFromDate")):null
                ,nvc != null ? General.GetNullableDateTime(nvc.Get("txtIssueToDate")):null
                ,nvc != null ? General.GetNullableInteger(nvc.Get("ddlLiabilitycompany")):null
                , nvc != null ? General.GetNullableInteger(nvc.Get("voucherstatus")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("cancelled")) : null
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"],
              gvCountry.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixAccountsProjectBilling.ProjectBillingVoucherPostSearch(null
                , null
                , null
                , null
                , null
                , null
                , null
                , 0
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"],
             gvCountry.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
        }

        
        General.SetPrintOptions("gvCountry", "Country", alCaptions, alColumns, ds);

        gvCountry.DataSource = ds;
        gvCountry.VirtualItemCount = iRowCount;

        

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void gvCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();

    //}

    protected void gvCountry_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCountry_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCountry.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCountry.SelectedIndexes.Clear();
        gvCountry.EditIndexes.Clear();
        gvCountry.DataSource = null;
        gvCountry.Rebind();
    }
    protected void gvCountry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }         
             if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixAccountsProjectBilling.ProjectBillingVoucherCancel(new Guid(((RadLabel)e.Item.FindControl("lblProjectBillingIssueId")).Text.ToString()));

                Rebind();

            }

            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                //int nCurrentRow = e.Item.ItemIndex;
                RadLabel lblVesselId = ((RadLabel)e.Item.FindControl("lblVesselId"));

                RadLabel lblcompanyid = ((RadLabel)e.Item.FindControl("lblBillingCompanyId"));
                ViewState["VESSELID"] = lblVesselId.Text;
                ViewState["COMPANYID"] = lblcompanyid.Text;

                Rebind();
                
            }


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void gvCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {

    //        }

    //        else if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            _gridView.SelectedIndex = nCurrentRow;
    //            Label lblVesselId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId"));

    //            Label lblcompanyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBillingCompanyId"));
    //            ViewState["VESSELID"] =lblVesselId.Text;
    //            ViewState["COMPANYID"] = lblcompanyid.Text;

    //            BindData();
    //            SetPageNavigator();
    //        }

    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixAccountsProjectBilling.ProjectBillingVoucherCancel(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblProjectBillingIssueId")).Text.ToString()));  
    //        }

    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}
    //protected void gvCountry_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCountry, "Select$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}
    //protected void gvCountry_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void gvCountry_RowEditing(object sender, GridViewEditEventArgs de)
    //{

    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindData();
    //    SetPageNavigator();

    //}

    //protected void gvCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

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

    protected void gvCountry_ItemDataBound(object sender, GridItemEventArgs e)
    {



        if (e.Item is GridDataItem)
        {
            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");                   
                }                          

            }
            
        }
        if (e.Item is GridDataItem)
        {
            RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblStatus");
            if (lblstatus != null && lblstatus.Text == "1")
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if(db!=null)
                db.Attributes.Add("style", "visibility:hidden");
                CheckBox chk = (CheckBox)e.Item.FindControl("chckPostVoucher");
                if(chk!=null)
                chk.Attributes.Add("style", "visibility:hidden");
            }
            RadLabel vesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel companyid = (RadLabel)e.Item.FindControl("lblBillingCompanyId");
            RadLabel voucher = (RadLabel)e.Item.FindControl("lblVoucherNumber");

            if (vesselid != null && companyid != null && voucher != null && ViewState["VESSELID"] != null)
            {
                if (ViewState["VESSELID"].ToString() == vesselid.Text && ViewState["COMPANYID"].ToString() == companyid.Text && (voucher.Text == null || voucher.Text == ""))
                {
                    CheckBox chk1 = (CheckBox)e.Item.FindControl("chckPostVoucher");
                    chk1.Checked = true;

                }
            }
        }
        
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
      
        BindData();
       
    }

   
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
      
    }

    //protected void chckPostVoucher_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox chk = (CheckBox)sender;
    //    if (chk.Checked == true)
    //    {
    //        GridViewRow gr = (GridViewRow)chk.Parent.Parent;
    //        Label vesselid = (Label)gr.FindControl("lblVesselId");
    //        Label companyid = (Label)gr.FindControl("lblBillingCompanyId");
    //        GridView gv = (GridView)gvCountry;

    //        ViewState["VESSELID"] = vesselid.Text;
    //        ViewState["COMPANYID"] = companyid.Text;

    //        foreach (GridViewRow row in gv.Rows)
    //        {
    //            CheckBox chk1 = (CheckBox)row.FindControl("chckPostVoucher");
    //            if (chk1 != null)
    //            {
    //                if (vesselid.Text == ((Label)row.FindControl("lblVesselId")).Text && companyid.Text == ((Label)row.FindControl("lblBillingCompanyId")).Text && (((Label)row.FindControl("lblVoucherNumber")).Text == null || ((Label)row.FindControl("lblVoucherNumber")).Text == ""))
    //                {
    //                    if (!chk1.Checked)
    //                        chk1.Checked = true;
    //                }
    //                else
    //                    chk1.Checked = false;
    //            }
    //        }
    //    }

    //}
}
