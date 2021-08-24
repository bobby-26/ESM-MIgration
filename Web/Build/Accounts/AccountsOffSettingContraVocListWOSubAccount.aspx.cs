using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;



public partial class AccountsOffSettingContraVocListWOSubAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    

        {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsOffSettingContraVocListWOSubAccount.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvContraVoucherList')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("../Accounts/AccountsOffSettingContraVocListWOSubAccount.aspx", "Add", "add.png", "ADD");

        MenuContraVoucherList.AccessRights = this.ViewState;
        MenuContraVoucherList.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {



            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;



            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
            {
                ViewState["voucherid"] = Request.QueryString["voucherid"];
            }
            if (Request.QueryString["offsettinglineitemid"] != null && Request.QueryString["offsettinglineitemid"] != string.Empty)
            {
                ViewState["offsettinglineitemid"] = Request.QueryString["offsettinglineitemid"];
            }
            gvContraVoucherList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
        //SetPageNavigator();

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = {
                                "Voucher Number",
                                "Voucher Date",
                                "Reference No",
                                "Company Name",
                                "Voucher Type",
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"
                              };

        string[] alColumns = {
                                "FLDVOUCHERNUMBER",
                                "FLDVOUCHERDATE",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDCOMPANYNAME",
                                "FLDSUBVOUCHERTYPE",
                                "FLDVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"
                             };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds;
        Guid offsettinglineitemid = Guid.Empty;
        if (ViewState["offsettinglineitemid"] != null)
            offsettinglineitemid = (Guid)General.GetNullableGuid(ViewState["offsettinglineitemid"].ToString());
        //if (Filter.CurrentOffSettingEntriesSelection != null)
        //{           
        NameValueCollection nvc = Filter.CurrentOffSettingEntriesSelection;

        ds = PhoenixAccountsContraVoucher.ContraVoucherSearch(
                                                                offsettinglineitemid
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvContraVoucherList.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                 );

        General.SetPrintOptions("gvContraVoucherList", "Contra Voucher List", alCaptions, alColumns, ds);

        gvContraVoucherList.DataSource = ds;
        gvContraVoucherList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDCONTRAVOUCHERID"].ToString();
                //Session["CONTRAVOUCHERID"] = ViewState["voucherid"].ToString();               
                // gvContraVoucherList.SelectedIndex = 0;
            }

            SetRowSelection();
            // BindPageURL(gvContraVoucherList.SelectedIndex);            
        }



    }
    protected void gvContraVoucherList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvContraVoucherList.CurrentPageIndex + 1;
            BindData();
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

        string[] alCaptions = {
                                "Voucher Number",
                                "Voucher Date",
                                "Reference No",
                                "Company Name",
                                "Voucher Type",
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"
                              };

        string[] alColumns = {
                                "FLDVOUCHERNUMBER",
                                "FLDVOUCHERDATE",
                                "FLDREFERENCEDOCUMENTNO",
                                "FLDCOMPANYNAME",
                                "FLDSUBVOUCHERTYPE",
                                "FLDVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"
                             };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        Guid offsettinglineitemid = Guid.Empty;
        if (ViewState["offsettinglineitemid"] != null)
            offsettinglineitemid = (Guid)General.GetNullableGuid(ViewState["offsettinglineitemid"].ToString());

        DataSet ds = PhoenixAccountsContraVoucher.ContraVoucherSearch(
                                                                    offsettinglineitemid
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , null
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                     );

        Response.AddHeader("Content-Disposition", "attachment; filename=OffSettingContraVoucherEntriesList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Off-Setting Contra Voucher Line Items</h3></td>");
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
        foreach (DataRow dr in (Session["printDs"] != null ? (DataSet)Session["printDs"] : ds).Tables[0].Rows)
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
    protected void MenuContraVoucherList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("ADD"))
        {
            ClientScript.RegisterStartupScript(GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = '../Accounts/AccountsOffSettingContraVocMasterWOSubAccount.aspx?" + "offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&contravoucherid=" + ViewState["voucherid"] + "';  </script>");
            BindData();
           
        }
    }
    protected void gvContraVoucherList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvContraVoucherList.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["voucherid"] = ((LinkButton)gvContraVoucherList.Items[e.NewSelectedIndex].FindControl("lnkVoucherId")).CommandArgument;
        BindPageURL(e.NewSelectedIndex);
    }

    protected void gvContraVoucherList_ItemDataBound(object sender, GridItemEventArgs e)
    {
     
        if (e.Item is GridDataItem)
        {
            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                RadLabel lblIsPosted = (RadLabel)e.Item.FindControl("lblIsPosted");
                ImageButton cmdPost = (ImageButton)e.Item.FindControl("cmdPost");
                ImageButton cmdRePost = (ImageButton)e.Item.FindControl("cmdRePost");
                if (lblIsPosted != null)
                {
                    if (lblIsPosted.Text.ToString() == "1" && cmdPost != null)
                        cmdPost.Visible = false;
                    if (lblIsPosted.Text.ToString() == "0" && cmdRePost != null)
                        cmdRePost.Visible = false;
                }

            }
        }
        if (e.Item is GridDataItem)
        {
            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                LinkButton lnkVoucherId = (LinkButton)e.Item.FindControl("lnkVoucherId");
                if (lnkVoucherId != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherId.CommandName)) lnkVoucherId.Visible = false;
                }
                RadLabel lblIsPosted = (RadLabel)e.Item.FindControl("lblIsPosted");

                int posted = int.Parse(((lblIsPosted != null) && (lblIsPosted.Text != "")) ? lblIsPosted.Text : "0");
                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    if (posted == 1)
                        cmdEdit.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
                }
                // cmdEdit.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsOffSettingContraVocSubAccount.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&callfrom=OFFSET" + "&offsetisposted=" + posted + "');return false;");
                cmdEdit.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsOffSettingContraVocWOSubAccount.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&callfrom=OFFSET" + "&offsetisposted=" + posted + "');return false;");


            }

        }
    }
    //protected void gvContraVoucherList_ItemDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {

    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            Label lblIsPosted = (Label)e.Row.FindControl("lblIsPosted");
    //            ImageButton cmdPost = (ImageButton)e.Row.FindControl("cmdPost");
    //            ImageButton cmdRePost = (ImageButton)e.Row.FindControl("cmdRePost");
    //            if (lblIsPosted != null)
    //            {
    //                if (lblIsPosted.Text.ToString() == "1" && cmdPost != null)
    //                    cmdPost.Visible = false;
    //                if (lblIsPosted.Text.ToString() == "0" && cmdRePost != null)
    //                    cmdRePost.Visible = false;
    //            }

    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            LinkButton lnkVoucherId = (LinkButton)e.Row.FindControl("lnkVoucherId");
    //            if (lnkVoucherId != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherId.CommandName)) lnkVoucherId.Visible = false;
    //            }
    //            Label lblIsPosted = (Label)e.Row.FindControl("lblIsPosted");

    //            int posted = int.Parse(((lblIsPosted != null) &&(lblIsPosted.Text!="")) ? lblIsPosted.Text : "0");
    //            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //            if (cmdEdit != null)
    //            {
    //                if (posted == 1)
    //                    cmdEdit.Visible = false;
    //                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
    //            }
    //            cmdEdit.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsOffSettingContraVocSubAccount.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&callfrom=OFFSET" + "&offsetisposted=" +posted+ "');return false;");               
    //        }

    //    }
    //}
    //protected void gvContraVoucherList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}
    //protected void gvContraVoucherList_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindData();
    //}
    //protected void gvContraVoucherList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;

    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}

    protected void gvContraVoucherList_ItemCommand(object sender, GridCommandEventArgs e)
    {


        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            int nCurrentRow = e.Item.ItemIndex;
            BindPageURL(nCurrentRow);
            SetRowSelection();
        }
        if (e.CommandName.ToUpper().Equals("POST"))
        {
            try
            {
                if (ViewState["voucherid"] != null)
                    PhoenixAccountsContraVoucher.ContraVoucherPost(int.Parse(ViewState["voucherid"].ToString()));
                ucStatus.Text = "Contra voucher is posted";
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                //BindData();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        else if (e.CommandName.ToUpper().Equals("REPOST"))
        {

            if (ViewState["voucherid"] != null)
                PhoenixAccountsContraVoucher.ContraVoucherRePost(int.Parse(ViewState["voucherid"].ToString()));
            ucStatus.Text = "Contra voucher is Reposted";
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);

        }
    }
    //protected void gvContraVoucherList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;
    //    if (e.CommandName.ToUpper().Equals("EDIT"))
    //    {
    //        int iRowno;
    //        iRowno = int.Parse(e.CommandArgument.ToString());
    //        BindPageURL(iRowno);
    //    }
    //    if (e.CommandName.ToUpper().Equals("POST"))
    //    {
    //        try
    //        {
    //            if (ViewState["voucherid"] != null)
    //                PhoenixAccountsContraVoucher.ContraVoucherPost(int.Parse(ViewState["voucherid"].ToString()));
    //            ucStatus.Text = "Contra voucher is posted";
    //            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
    //            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
    //            //BindData();
    //        }
    //        catch (Exception ex)
    //        {
    //            ucError.ErrorMessage = ex.Message;
    //            ucError.Visible = true;
    //        }
    //    }
    //    else if (e.CommandName.ToUpper().Equals("REPOST"))
    //    {

    //        if (ViewState["voucherid"] != null)
    //            PhoenixAccountsContraVoucher.ContraVoucherRePost(int.Parse(ViewState["voucherid"].ToString()));
    //        ucStatus.Text = "Contra voucher is Reposted";
    //        String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
    //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);


    //    }
    //}
    protected void gvContraVoucherList_SortCommand(object sender, GridSortCommandEventArgs e)
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



    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvContraVoucherList.Items[rowindex];
            RadLabel lblVoucherLineId = ((RadLabel)gvContraVoucherList.Items[rowindex].FindControl("lblVoucherLineId"));
            LinkButton lnkVoucherId = ((LinkButton)gvContraVoucherList.Items[rowindex].FindControl("lnkVoucherId"));
            RadLabel lblIsPosted = (RadLabel)gvContraVoucherList.Items[rowindex].FindControl("lblIsPosted");
            if (lblVoucherLineId != null)
            {
                ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();

            }
            if (lblIsPosted != null && lblIsPosted.Text != "")
                ViewState["OFFSETISPOSTED"] = lblIsPosted.Text;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvContraVoucherList.Rebind();
        if (Session["New"] != null && Session["New"].ToString() == "Y")
        {

            Session["New"] = "N";
            BindPageURL(0);
        }
    }

    private void SetRowSelection()
    {
        gvContraVoucherList.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvContraVoucherList.Items)
        {
            if (item.GetDataKeyValue("FLDCONTRAVOUCHERID").ToString().Equals(ViewState["voucherid"].ToString()))
            {
                gvContraVoucherList.SelectedIndexes.Add(item.ItemIndex);
                RadLabel lblVoucherLineId = ((RadLabel)gvContraVoucherList.Items[item.ItemIndex].FindControl("lblVoucherLineId"));
                if (lblVoucherLineId != null)
                    ViewState["VOUCHERLINEITEMID"] = lblVoucherLineId.Text.ToString();
            }
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvContraVoucherList_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            PhoenixAccountsContraVoucher.ContraVoucherDelete(int.Parse(ViewState["voucherid"].ToString()));

            ucStatus.Text = "Contra voucher is Deleted";
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            gvContraVoucherList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



}
