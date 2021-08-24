using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;


public partial class AccountsInterCompanyTransferContraVoucherMaster : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsInterCompanyTransferContraVoucherMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucher')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageLink("javascript:Openpopup('Filter','','../Accounts/AccountsInterCompanyTransferContraVoucherMaster.aspx'); return false;", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
         //   MenuOrderForm.SetTrigger(pnlOrderForm);

            if (!IsPostBack)
            {
                Session["New"] = "N";
               

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["PAGEURL"] = null;

                gvInterCompanyContraVoucherMaster.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["offsettinglineitemid"] != null && Request.QueryString["offsettinglineitemid"] != string.Empty)
                    ViewState["OFFSETTINGLINEITEMID"] = Request.QueryString["offsettinglineitemid"].ToString();
                if (Request.QueryString["offsettingvoucherid"] != null && Request.QueryString["offsettingvoucherid"] != string.Empty)
                {
                    ViewState["offsettingvoucherid"] = Request.QueryString["offsettingvoucherid"];
                }
                if (Request.QueryString["voucherid"] != null)
                {
                    ViewState["voucherid"] = Request.QueryString["voucherid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVoucher.aspx?voucherid=" + ViewState["voucherid"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&interlineitemisposted=" + ViewState["interlineitemisposted"] + "&interlineitemisposted=" + ViewState["offsetisposted"] + "&offsettingvoucherid=" + ViewState["offsettingvoucherid"];
                }
                if (Request.QueryString["contravoucherid"] != null)
                {
                    ViewState["contravoucherid"] = Request.QueryString["contravoucherid"].ToString();
                }
                MenuOrderFormMain.SelectedMenuIndex = 1;
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Line Items", "LINEITEMS", ToolBarDirection.Right);
            toolbarmain.AddButton("Contra Voucher", "VOUCHER", ToolBarDirection.Right);
            toolbarmain.AddButton("Inter-Company Entries", "INTERCOMPANYENTRIES", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
           // MenuOrderFormMain.SetTrigger(pnlOrderForm);
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInterCompanyContraVoucherMaster_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("INTERCOMPANYENTRIES"))
            {
                Response.Redirect("../Accounts/AccountsInterCompanyTransferEntriesList.aspx?voucherid=" + ViewState["voucherid"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"], false);
            }
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVoucher.aspx?voucherid=" + ViewState["voucherid"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&interlineitemisposted=" + ViewState["offsetisposted"];
            }
            if (CommandName.ToUpper().Equals("LINEITEMS") && ViewState["voucherid"] != null && ViewState["voucherid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsInterCompanyTransferContraVoucherLineItemDetails.aspx?qvouchercode=" + ViewState["voucherid"] + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&interlineitemisposted=" + ViewState["offsetisposted"] + "&offsettingvoucherid=" + ViewState["offsettingvoucherid"] + "&contravoucherid=" + ViewState["contravoucherid"], false);
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 1;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alCaptions = { 
                                "Voucher Number",
                                "Voucher Date", 
                                "Reference No",                                                               
                                "Sub Voucher Type", 
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"                              
                              };

        string[] alColumns = { 
                                "FLDVOUCHERNUMBER", 
                                "FLDVOUCHERDATE", 
                                "FLDREFERENCEDOCUMENTNO",                                                            
                                "FLDSUBVOUCHERTYPE",
                                "FLDVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"                             
                             };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        Guid offsettinglineitemid = Guid.Empty;
        if (ViewState["OFFSETTINGLINEITEMID"] != null)
            offsettinglineitemid = (Guid)General.GetNullableGuid(ViewState["OFFSETTINGLINEITEMID"].ToString());

        NameValueCollection nvc = Filter.CurrentOffSettingEntriesSelection;
       
        ds = PhoenixAccountsContraVoucher.ContraVoucherSearch(
                                                                offsettinglineitemid
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                , null, null, null
                                                                , null
                                                                , null
                                                                , null, null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount );

        Response.AddHeader("Content-Disposition", "attachment; filename=InterCompanyContraVoucherList.xls");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvInterCompanyContraVoucherMaster.Rebind();


            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        Guid offsettinglineitemid = Guid.Empty;
        if (ViewState["OFFSETTINGLINEITEMID"] != null)
            offsettinglineitemid = (Guid)General.GetNullableGuid(ViewState["OFFSETTINGLINEITEMID"].ToString());

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
                                        , gvInterCompanyContraVoucherMaster.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount
                                        );


        string[] alCaptions = { 
                                "Voucher Number",
                                "Voucher Date", 
                                "Reference No",                                                               
                                "Sub Voucher Type", 
                                "Voucher Year",
                                "Voucher Status",
                                "Locked YN"                              
                              };

        string[] alColumns = { 
                                "FLDVOUCHERNUMBER", 
                                "FLDVOUCHERDATE", 
                                "FLDREFERENCEDOCUMENTNO",                                                            
                                "FLDSUBVOUCHERTYPE",
                                "FLDVOUCHERYEAR",
                                "FLDVOUCHERSTATUS",
                                "FLDLOCKEDYNDESCRIPTION"                             
                             };
        General.SetPrintOptions("gvVoucher", "Contra Voucher List", alCaptions, alColumns, ds);
        gvInterCompanyContraVoucherMaster.DataSource = ds;
        gvInterCompanyContraVoucherMaster.VirtualItemCount = iRowCount;
     

        if (ds.Tables[0].Rows.Count > 0)
        {
         
            if (ViewState["voucherid"] == null)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDVOUCHERID"].ToString();                
               // gvInterCompanyContraVoucherMaster.SelectedIndex = 0;
            }
            if (ViewState["contravoucherid"] == null)
            {
                ViewState["contravoucherid"] = ds.Tables[0].Rows[0]["FLDCONTRAVOUCHERID"].ToString();
            }
            SetRowSelection();
           // BindPageURL(gvInterCompanyContraVoucherMaster.SelectedIndex);
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&interlineitemisposted=" + ViewState["offsetisposted"] + "&offsettingvoucherid=" + ViewState["offsettingvoucherid"];
            }                        
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVoucher.aspx?offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&interlineitemisposted=" + ViewState["offsetisposted"] + "&offsettingvoucherid=" + ViewState["offsettingvoucherid"] + "&voucherid=";
            }
         
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvInterCompanyContraVoucherMaster_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInterCompanyContraVoucherMaster.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        gvInterCompanyContraVoucherMaster.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvInterCompanyContraVoucherMaster.Items)
        {
            if (item.GetDataKeyValue("FLDCONTRAVOUCHERID").ToString().Equals(ViewState["contravoucherid"].ToString()))
            {
                //gvInterCompanyContraVoucherMaster.SelectedIndex = i;
              
                RadLabel lblContraVoucherId = (RadLabel)gvInterCompanyContraVoucherMaster.Items[item.ItemIndex].FindControl("lblContraVoucherId");
                if (lblContraVoucherId != null)
                    PhoenixAccountsContraVoucher.ContraVoucherNumber = lblContraVoucherId.Text;
            }
        }
    }

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {
    //        gvInterCompanyContraVoucherMaster.SelectedIndex = -1;
    //        ViewState["SORTEXPRESSION"] = ib.CommandName;
    //        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvInterCompanyContraVoucherMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    //protected void gvInterCompanyContraVoucherMaster_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void gvInterCompanyContraVoucherMaster_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}


    protected void gvInterCompanyContraVoucherMaster_ItemDataBound(object sender, GridItemEventArgs e)
    {

            if (e.Item is GridDataItem)
            {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
            RadLabel lblIsPosted = (RadLabel)e.Item.FindControl("lblIsPosted");
            int offsetisposted = int.Parse(((lblIsPosted != null) && (lblIsPosted.Text != "")) ? lblIsPosted.Text : "0");
        }
    }

    //protected void gvInterCompanyContraVoucherMaster_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (cmdEdit != null)
    //            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
    //        Label lblIsPosted = (Label)e.Row.FindControl("lblIsPosted");
    //        int offsetisposted = int.Parse(((lblIsPosted != null) && (lblIsPosted.Text != "")) ? lblIsPosted.Text : "0");
    //    }
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
   
    }

   
   
    protected void gvInterCompanyContraVoucherMaster_ItemCommand(object sender, GridCommandEventArgs e)
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
            SetRowSelection();
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvInterCompanyContraVoucherMaster.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            //gvInterCompanyContraVoucherMaster.SelectedIndex = 0;
            Session["New"] = "N";
            BindPageURL(0);
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            if (rowindex >= 0)
            {
                GridDataItem item = (GridDataItem)gvInterCompanyContraVoucherMaster.Items[rowindex];
                LinkButton lnkVoucherId = ((LinkButton)gvInterCompanyContraVoucherMaster.Items[rowindex].FindControl("lnkVoucherId"));
                RadLabel lblIsPosted = (RadLabel)gvInterCompanyContraVoucherMaster.Items[rowindex].FindControl("lblIsPosted");
                RadLabel lblContraVoucherId = (RadLabel)gvInterCompanyContraVoucherMaster.Items[rowindex].FindControl("lblContraVoucherId");
                if (lblIsPosted != null)
                {
                    ViewState["offsetisposted"] = lblIsPosted.Text;
                }
                if (lnkVoucherId != null)
                {
                    ViewState["voucherid"] = lnkVoucherId.CommandArgument;
                    // PhoenixAccountsContraVoucher.ContraVoucherNumber = lnkVoucherId.Text;
                }
                if (lblContraVoucherId  != null)
                {
                    ViewState["contravoucherid"] = lblContraVoucherId.Text;
                }
            }
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferContraVoucher.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&offsettinglineitemid=" + ViewState["OFFSETTINGLINEITEMID"] + "&offsettingvoucherid=" + ViewState["offsettingvoucherid"] + "&interlineitemisposted=" + ViewState["offsetisposted"];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInterCompanyContraVoucherMaster_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInterCompanyContraVoucherMaster.SelectedIndexes.Add(e.NewSelectedIndex);
        ViewState["voucherid"] = ((LinkButton)gvInterCompanyContraVoucherMaster.Items[e.NewSelectedIndex].FindControl("lnkVoucherId")).CommandArgument;
        BindPageURL(e.NewSelectedIndex);
    }
}
