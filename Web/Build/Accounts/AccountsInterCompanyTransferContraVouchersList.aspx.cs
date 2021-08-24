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

public partial class AccountsInterCompanyTransferContraVouchersList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvInterCompanyContraVouchersList')", "Print Grid", "icon_print.png", "PRINT");
        if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty)
            ViewState["callfrom"] = Request.QueryString["callfrom"];
        if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
            toolbargrid.AddImageLink("../Accounts/AccountsInterCompanyTransferContraVouchersList.aspx", "Add", "add.png", "ADD");

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
            if (Request.QueryString["offsettingvoucherid"] != null && Request.QueryString["offsettingvoucherid"] != string.Empty)
            {
                ViewState["offsettingvoucherid"] = Request.QueryString["offsettingvoucherid"];
            }
            if (Request.QueryString["offsettinglineitemid"] != null && Request.QueryString["offsettinglineitemid"] != string.Empty)
            {
                ViewState["offsettinglineitemid"] = Request.QueryString["offsettinglineitemid"];
            }

            gvInterCompanyContraVouchersList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
      
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
       
        if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
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
                                        , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                        , gvInterCompanyContraVouchersList.PageSize, ref iRowCount, ref iTotalPageCount
                                 );
        else
            ds = PhoenixAccountsContraVoucher.ContraVoucherSearch(                                         
                                          PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                        , gvInterCompanyContraVouchersList.PageSize, ref iRowCount, ref iTotalPageCount,1
                                 );

        General.SetPrintOptions("gvInterCompanyContraVouchersList", "Contra Voucher List", alCaptions, alColumns, ds);
        gvInterCompanyContraVouchersList.DataSource = ds;
        gvInterCompanyContraVouchersList.VirtualItemCount = iRowCount;
     
        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDCONTRAVOUCHERID"].ToString();
                //Session["CONTRAVOUCHERID"] = ViewState["voucherid"].ToString();
              //  gvInterCompanyContraVouchersList.SelectedIndex = 0;
            }
         
            SetRowSelection();
        //    BindPageURL(0);
        }
  

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvInterCompanyContraVouchersList.SelectedIndexes.Clear();
        gvInterCompanyContraVouchersList.EditIndexes.Clear();
        gvInterCompanyContraVouchersList.DataSource = null;
        gvInterCompanyContraVouchersList.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
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

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        Guid offsettinglineitemid = Guid.Empty;
        if (ViewState["offsettinglineitemid"] != null)
            offsettinglineitemid = (Guid)General.GetNullableGuid(ViewState["offsettinglineitemid"].ToString());
        if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
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
                                            , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount
                                     );
        else
            ds = PhoenixAccountsContraVoucher.ContraVoucherSearch(                                         
                                          PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null
                                        , null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                                        , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount, 1
                                 );

        Response.AddHeader("Content-Disposition", "attachment; filename=InterCompanyContraVoucherEntriesList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Contra Voucher List</h3></td>");
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
            ClientScript.RegisterStartupScript(GetType(), "Load", "<script type='text/javascript'>window.parent.location.href = '../Accounts/AccountsInterCompanyTransferContraVoucherMaster.aspx?" + "offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&contravoucherid=" + ViewState["voucherid"] + "&offsettingvoucherid=" + ViewState["offsettingvoucherid"] + "';  </script>");
            BindData();
        }    
    }


    protected void gvInterCompanyContraVouchersList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInterCompanyContraVouchersList.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["voucherid"] = ((LinkButton)gvInterCompanyContraVouchersList.Items[e.NewSelectedIndex].FindControl("lnkVoucherId")).CommandArgument;
        BindPageURL(e.NewSelectedIndex);
    }

    //protected void gvInterCompanyContraVouchersList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{
    //    gvInterCompanyContraVouchersList.SelectedIndex = e.NewSelectedIndex;
    //    ViewState["voucherid"] = ((LinkButton)gvInterCompanyContraVouchersList.Rows[e.NewSelectedIndex].FindControl("lnkVoucherId")).CommandArgument;
    //    BindPageURL(e.NewSelectedIndex);
    //}
    protected void gvInterCompanyContraVouchersList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            // total = 0.00;
            RadLabel lblContext = (RadLabel)e.Item.FindControl("lblContext");
            RadLabel lblTarget = (RadLabel)e.Item.FindControl("lblTarget");
            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
            {
                if (lblContext != null && lblTarget != null)
                {
                    lblContext.Visible = false;
                    lblTarget.Visible = true;
                }
            }
            else
            {
                if (lblContext != null && lblTarget != null)
                {
                    lblContext.Visible = true;
                    lblTarget.Visible = false;
                }
            }

        }
        if (e.Item is GridDataItem)
        {
            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                RadLabel lblIsPosted = (RadLabel)e.Item.FindControl("lblIsPosted");
                ImageButton cmdPost = (ImageButton)e.Item.FindControl("cmdPost");
                if (lblIsPosted != null)
                {
                    if (lblIsPosted.Text.ToString() == "1" && cmdPost != null)
                        cmdPost.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, cmdPost.CommandName)) cmdPost.Visible = false;
                }
            }
        }
        if (e.Item is GridDataItem)
        {

            RadLabel lblContextCompany = (RadLabel)e.Item.FindControl("lblContextCompany");
            RadLabel lblTargetCompany = (RadLabel)e.Item.FindControl("lblTargetCompany");
            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
            {
                if (lblContextCompany != null && lblTargetCompany != null)
                {

                    lblContextCompany.Visible = false;
                    lblTargetCompany.Visible = true;

                }
            }
            else
            {
                if (lblContextCompany != null && lblTargetCompany != null)
                {
                    lblContextCompany.Visible = true;
                    lblTargetCompany.Visible = false;

                }
            }
            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                LinkButton lnkVoucherId = (LinkButton)e.Item.FindControl("lnkVoucherId");
                RadLabel lblIsPosted = (RadLabel)e.Item.FindControl("lblIsPosted");
                RadLabel lblVoucherNumber = (RadLabel)e.Item.FindControl("lblVoucherNumber");
                ImageButton cmdPost = (ImageButton)e.Item.FindControl("cmdPost");
                if (lnkVoucherId != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherId.CommandName)) lnkVoucherId.Visible = false;
                }
                //lnkVoucherId.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsInterCompanyTransferContraVoucherDetails.aspx?voucherid=" + lnkVoucherId.CommandArgument + "');return false;");

                int posted = int.Parse(((lblIsPosted != null) && (lblIsPosted.Text != "")) ? lblIsPosted.Text : "0");
                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    if (posted == 1)
                        cmdEdit.Visible = false;
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
                }
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Accounts/AccountsInterCompanyTransferContraVoucher.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&callfrom=INTER" + "&interlineitemisposted=" + posted + "',false,1000,500);");
              //  cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Accounts/AccountsOffSettingContraVocSubAccount.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&callfrom=OFFSET" + "&offsetisposted=" + posted + "',false,1000,500);");

                if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
                {

                }
                else
                {
                    if (lnkVoucherId != null)
                        lnkVoucherId.Visible = false;
                    if (lblVoucherNumber != null)
                        lblVoucherNumber.Visible = true;
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;
                    if (cmdPost != null)
                        cmdPost.Visible = false;
                }

            }

        }
    }

    //protected void gvInterCompanyContraVouchersList_ItemDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        // total = 0.00;
    //        Label lblContext = (Label)e.Row.FindControl("lblContext");
    //        Label lblTarget = (Label)e.Row.FindControl("lblTarget");       
    //        if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
    //        {
    //            if (lblContext != null && lblTarget != null)
    //            {
    //                lblContext.Visible = false;
    //                lblTarget.Visible = true;
    //            }
    //        }
    //        else
    //        {
    //            if (lblContext != null && lblTarget != null)
    //            {
    //                lblContext.Visible = true;
    //                lblTarget.Visible = false;
    //            }
    //        }

    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            Label lblIsPosted = (Label)e.Row.FindControl("lblIsPosted");
    //            ImageButton cmdPost = (ImageButton)e.Row.FindControl("cmdPost");
    //            if (lblIsPosted != null)
    //            {
    //                if (lblIsPosted.Text.ToString() == "1" && cmdPost != null)
    //                    cmdPost.Visible = false;
    //                if (!SessionUtil.CanAccess(this.ViewState, cmdPost.CommandName)) cmdPost.Visible = false;
    //            }
    //        }
    //    }
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {

    //        Label lblContextCompany = (Label)e.Row.FindControl("lblContextCompany");           
    //        Label lblTargetCompany = (Label)e.Row.FindControl("lblTargetCompany");
    //        if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
    //        {
    //            if (lblContextCompany != null && lblTargetCompany != null)
    //            {

    //                lblContextCompany.Visible = false;                   
    //                lblTargetCompany.Visible = true;

    //            }
    //        }
    //        else
    //        {
    //            if (lblContextCompany != null && lblTargetCompany != null)
    //            {                    
    //                lblContextCompany.Visible = true;                    
    //                lblTargetCompany.Visible = false;

    //            }
    //        }             
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            LinkButton lnkVoucherId = (LinkButton)e.Row.FindControl("lnkVoucherId");
    //            Label lblIsPosted = (Label)e.Row.FindControl("lblIsPosted");
    //            Label lblVoucherNumber = (Label)e.Row.FindControl("lblVoucherNumber");
    //            ImageButton cmdPost = (ImageButton)e.Row.FindControl("cmdPost");
    //            if (lnkVoucherId != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, lnkVoucherId.CommandName)) lnkVoucherId.Visible = false;
    //            }
    //            //lnkVoucherId.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsInterCompanyTransferContraVoucherDetails.aspx?voucherid=" + lnkVoucherId.CommandArgument + "');return false;");

    //            int posted = int.Parse(((lblIsPosted != null) && (lblIsPosted.Text != "")) ? lblIsPosted.Text : "0");
    //            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //            if (cmdEdit != null)
    //            {
    //                if (posted == 1)
    //                    cmdEdit.Visible = false;
    //                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
    //            }
    //            cmdEdit.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'AccountsInterCompanyTransferContraVoucher.aspx?voucherid=" + lnkVoucherId.CommandArgument + "&offsettinglineitemid=" + ViewState["offsettinglineitemid"] + "&callfrom=INTER" + "&interlineitemisposted=" + posted + "');return false;");
    //            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "OTHER")
    //            {

    //            }
    //            else
    //            {
    //                if (lnkVoucherId != null)
    //                    lnkVoucherId.Visible = false;
    //                if (lblVoucherNumber != null)
    //                    lblVoucherNumber.Visible = true;
    //                if (cmdEdit != null)
    //                    cmdEdit.Visible = false;
    //                if (cmdPost != null)
    //                    cmdPost.Visible = false;
    //            }

    //        }

    //    }
    //}
    protected void gvInterCompanyContraVouchersList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInterCompanyContraVouchersList.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvInterCompanyContraVouchersList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}
    //protected void gvInterCompanyContraVouchersList_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindData();
    //}
    //protected void gvInterCompanyContraVouchersList_RowUpdating(object sender, GridViewUpdateEventArgs e)
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


    protected void gvInterCompanyContraVouchersList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
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
                Rebind();

            }
            if (e.CommandName.ToUpper().Equals("POST"))
            {

              
                if (ViewState["voucherid"] != null)
                        PhoenixAccountsContraVoucher.ContraVoucherPost(int.Parse(ViewState["voucherid"].ToString()));
                    ucStatus.Text = "Contra voucher is posted";
                    String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
               
             }
            Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void gvInterCompanyContraVouchersList_RowCommand(object sender, GridViewCommandEventArgs e)
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
    //}
    protected void gvInterCompanyContraVouchersList_SortCommand(object sender, GridSortCommandEventArgs e)
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
            GridDataItem item = (GridDataItem)gvInterCompanyContraVouchersList.Items[rowindex];
            RadLabel lblVoucherLineId = ((RadLabel)gvInterCompanyContraVouchersList.Items[rowindex].FindControl("lblVoucherLineId"));
            LinkButton lnkVoucherId = ((LinkButton)gvInterCompanyContraVouchersList.Items[rowindex].FindControl("lnkVoucherId"));
            RadLabel lblIsPosted = (RadLabel)gvInterCompanyContraVouchersList.Items[rowindex].FindControl("lblIsPosted ");
            if (lblVoucherLineId != null)
            {
                ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();
                //Session["CONTRAVOUCHERID"] = lnkVoucherId.CommandArgument.ToString();
            }
            if (lblIsPosted != null && lblIsPosted.Text != "")
                ViewState["INTERISPOSTED"] = lblIsPosted.Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //private void BindPageURL(int rowindex)
    //{
    //    try
    //    {
    //        Label lblVoucherLineId = ((Label)gvInterCompanyContraVouchersList.Rows[rowindex].FindControl("lblVoucherLineId"));
    //        LinkButton lnkVoucherId = ((LinkButton)gvInterCompanyContraVouchersList.Rows[rowindex].FindControl("lnkVoucherId"));
    //        Label lblIsPosted = (Label)gvInterCompanyContraVouchersList.Rows[rowindex].FindControl("lblIsPosted ");
    //        if (lblVoucherLineId != null)
    //        {
    //            ViewState["OFFSETTINGLINEITEMID"] = lblVoucherLineId.Text.ToString();
    //            //Session["CONTRAVOUCHERID"] = lnkVoucherId.CommandArgument.ToString();
    //        }
    //        if (lblIsPosted != null && lblIsPosted.Text != "")
    //           ViewState["INTERISPOSTED"] = lblIsPosted.Text;           
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        
        gvInterCompanyContraVouchersList.Rebind();
        if (Session["New"] != null && Session["New"].ToString() == "Y")
        {

            Session["New"] = "N";
            BindPageURL(0);

        }
    }
    private void SetRowSelection()
    {
        gvInterCompanyContraVouchersList.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvInterCompanyContraVouchersList.Items)
        {
            if (item.GetDataKeyValue("FLDCONTRAVOUCHERID").ToString().Equals(ViewState["voucherid"].ToString()))
            {
               
                RadLabel lblVoucherLineId = ((RadLabel)gvInterCompanyContraVouchersList.Items[item.ItemIndex].FindControl("lblVoucherLineId"));
                if (lblVoucherLineId != null)
                    ViewState["VOUCHERLINEITEMID"] = lblVoucherLineId.Text.ToString();
            }
        }
    }
    //private void SetRowSelection()
    //{
    //    gvInterCompanyContraVouchersList.SelectedIndex = -1;
    //    for (int i = 0; i < gvInterCompanyContraVouchersList.Rows.Count; i++)
    //    {
    //        if (gvInterCompanyContraVouchersList.DataKeys[i].Value.ToString().Equals(ViewState["voucherid"].ToString()))
    //        {
    //            gvInterCompanyContraVouchersList.SelectedIndex = i;
    //            Label lblVoucherLineId = ((Label)gvInterCompanyContraVouchersList.Rows[i].FindControl("lblVoucherLineId"));
    //            if (lblVoucherLineId != null)
    //                ViewState["VOUCHERLINEITEMID"] = lblVoucherLineId.Text.ToString();
    //        }
    //    }
    //}
  
}
