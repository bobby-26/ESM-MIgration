using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class AccountsERMVoucherPrefix : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsERMVoucherPrefix.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvERMVoucherprefix')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsERMVoucherPrefix.aspx", "Find", "search.png", "FIND");
          
            toolbargrid.AddImageButton("../Accounts/AccountsERMVoucherPrefix.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
         //   toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1', '', '../Accounts/ERMVoucherPrefixAdd.aspx?PrefixId=" + "" + "'); return false;", "Add", "add.png", "ADD");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelpactivity','','Accounts/ERMVoucherPrefixAdd.aspx?PrefixId=" + "" + "')", "Add", "add.png", "ADD");

            MenuERMVoucherprefix.AccessRights = this.ViewState;
            MenuERMVoucherprefix.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            Menutab.Title = "ERM Voucher Prefix";
            Menutab.AccessRights = this.ViewState;
            Menutab.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvERMVoucherprefix.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
       //     BindData();
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvERMVoucherprefix_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alCaptions = { 
                               "Company"	
                               ,"Database"
                               ,"ERM"		
                               ,"Phoenix"	
                               ,"Phoenix TRN"
                               ,"X Access"	
                               ,"Z Time"		
                               ,"ZU Time"	
                               ,"ZID"		
                               ,"X Type TRN"	
                               ,"X TRN"		
                               ,"X Action"	
                               ,"X Description"		
                               ,"X Num"		
                               ,"X Inc"		
                               ,"Z Active"	
                               ,"Column1"	
                               ,"Database2"
                                                             
                              };

        string[] alColumns = { 
                                "FLDCOMPANY"	
                               ,"FLDDATABASE"
                               ,"FLDERM"		
                               ,"FLDPHOENIX"	
                               ,"FLDPHOENIXTRN"
                               ,"FLDXACCESS"	
                               ,"FLDZTIME"		
                               ,"FLDZUTIME"	
                               ,"FLDZID"		
                               ,"FLDXTYPETRN"	
                               ,"FLDXTRN"		
                               ,"FLDXACTION"	
                               ,"FLDXDESC"		
                               ,"FLDXNUM"		
                               ,"FLDXINC"		
                               ,"FLDZACTIVE"	
                               ,"FLDCOLUMN1"	
                               ,"FLDDATABASE2"                          
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

        ds = PhoenixAccountsERMVesselAccruedExpensess.ERMVoucherPrefixSearch(General.GetNullableString(txtcompany.Text.Trim())
                                                        , General.GetNullableString(txtDatabase1.Text.Trim())
                                                        , General.GetNullableString(txtERM.Text.Trim())
                                                        , General.GetNullableString(txtPhoenix.Text.Trim())
                                                        , General.GetNullableString(txtPhoenixTRN.Text.Trim())
                                                        , General.GetNullableString(txtXAccess.Text.Trim())
                                                        , General.GetNullableString(txtXTime.Text.Trim())
                                                        , General.GetNullableString(txtZUTime.Text.Trim())
                                                        , General.GetNullableString(txtZID.Text.Trim())
                                                        , General.GetNullableString(txtXtypeTRN.Text.Trim())
                                                        , General.GetNullableString(txtXTRN.Text.Trim())
                                                        , General.GetNullableString(txtXAction.Text.Trim())
                                                        , General.GetNullableString(txtXDescription.Text.Trim())
                                                        , General.GetNullableString(txtXNumber.Text.Trim())
                                                        , General.GetNullableString(txtXInc.Text.Trim())
                                                        , General.GetNullableString(txtZActive.Text.Trim())
                                                        , General.GetNullableString(txtColumn1.Text.Trim())
                                                        , General.GetNullableString(txtDatabase2.Text.Trim())
                                                   , (int)ViewState["PAGENUMBER"]
                                                   , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                   , ref iRowCount, ref iTotalPageCount
                                                   
                                                   );



        Response.AddHeader("Content-Disposition", "attachment; filename=ERMVoucherPrefix.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ERM Voucher Prefix</h3></td>");
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
                Response.Write("<td align=left>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuERMVoucherprefix_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
               
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtcompany.Text = string.Empty;
                txtDatabase1.Text = string.Empty;
                txtERM.Text = string.Empty;
                txtPhoenix.Text = string.Empty;
                txtPhoenixTRN.Text = string.Empty;
                txtXAccess.Text = string.Empty;
                txtXTime.Text = string.Empty;
                txtZUTime.Text = string.Empty;
                txtZID.Text = string.Empty;
                txtXtypeTRN.Text = string.Empty;
                txtXTRN.Text = string.Empty;
                txtXAction.Text = string.Empty;
                txtXDescription.Text = string.Empty;
                txtXNumber.Text = string.Empty;
                txtXInc.Text = string.Empty;
                txtZActive.Text = string.Empty;
                txtColumn1.Text = string.Empty;
                txtDatabase2.Text = string.Empty;
                
                BindData();
               
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void gvERMVoucherprefix_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvERMVoucherprefix.CurrentPageIndex + 1;
            BindData();
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

        ds = PhoenixAccountsERMVesselAccruedExpensess.ERMVoucherPrefixSearch(General.GetNullableString(txtcompany.Text.Trim())
                                                         , General.GetNullableString(txtDatabase1.Text.Trim())
                                                         , General.GetNullableString(txtERM.Text.Trim())
                                                         , General.GetNullableString(txtPhoenix.Text.Trim())
                                                         , General.GetNullableString(txtPhoenixTRN.Text.Trim())
                                                         , General.GetNullableString(txtXAccess.Text.Trim())
                                                         , General.GetNullableString(txtXTime.Text.Trim())
                                                         , General.GetNullableString(txtZUTime.Text.Trim())
                                                         , General.GetNullableString(txtZID.Text.Trim())
                                                         , General.GetNullableString(txtXtypeTRN.Text.Trim())
                                                         , General.GetNullableString(txtXTRN.Text.Trim())
                                                         , General.GetNullableString(txtXAction.Text.Trim())
                                                         , General.GetNullableString(txtXDescription.Text.Trim())
                                                         , General.GetNullableString(txtXNumber.Text.Trim())
                                                         , General.GetNullableString(txtXInc.Text.Trim())
                                                         , General.GetNullableString(txtZActive.Text.Trim())
                                                         , General.GetNullableString(txtColumn1.Text.Trim())
                                                         , General.GetNullableString(txtDatabase2.Text.Trim())
                                                         , (int)ViewState["PAGENUMBER"]
                                                         , gvERMVoucherprefix.PageSize
                                                         , ref iRowCount, ref iTotalPageCount

                                                    );

        string[] alCaptions = { 
                               "Company"	
                               ,"Database"
                               ,"ERM"		
                               ,"Phoenix"	
                               ,"Phoenix TRN"
                               ,"X Access"	
                               ,"Z Time"		
                               ,"ZU Time"	
                               ,"ZID"		
                               ,"X Type TRN"	
                               ,"X TRN"		
                               ,"X Action"	
                               ,"X Description"		
                               ,"X Num"		
                               ,"X Inc"		
                               ,"Z Active"	
                               ,"Column1"	
                               ,"Database2"
                                                             
                              };

        string[] alColumns = { 
                                "FLDCOMPANY"	
                               ,"FLDDATABASE"
                               ,"FLDERM"		
                               ,"FLDPHOENIX"	
                               ,"FLDPHOENIXTRN"
                               ,"FLDXACCESS"	
                               ,"FLDZTIME"		
                               ,"FLDZUTIME"	
                               ,"FLDZID"		
                               ,"FLDXTYPETRN"	
                               ,"FLDXTRN"		
                               ,"FLDXACTION"	
                               ,"FLDXDESC"		
                               ,"FLDXNUM"		
                               ,"FLDXINC"		
                               ,"FLDZACTIVE"	
                               ,"FLDCOLUMN1"	
                               ,"FLDDATABASE2"                          
                             };
        General.SetPrintOptions("gvERMVoucherprefix", "ERM Voucher Prefix", alCaptions, alColumns, ds);

     
            gvERMVoucherprefix.DataSource = ds;
        gvERMVoucherprefix.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {
    //        gvERMVoucherprefix.SelectedIndex = -1;
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

    //protected void gvERMVoucherprefix_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    //protected void gvERMVoucherprefix_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void gvERMVoucherprefix_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    protected void gvERMVoucherprefix_ItemDataBound(object sender, GridItemEventArgs e)

    {


        if (e.Item is GridDataItem)
        {
            if (!e.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Equals(DataControlRowState.Edit))
            {
                LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkCompany");
                lbtn.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Accounts/ERMVoucherPrefixAdd.aspx?PrefixId=" + lbtn.CommandArgument + "'); return false;");
            }
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                ImageButton imgbtn = (ImageButton)e.Item.FindControl("cmdEdit");
                imgbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Accounts/ERMVoucherPrefixAdd.aspx?PrefixId=" + imgbtn.CommandArgument + "',false);");

             //   imgbtn.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../Accounts/ERMVoucherPrefixAdd.aspx?PrefixId=" + imgbtn.CommandArgument + "'); return false;");
            }
            
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
       
    }
    protected void gvERMVoucherprefix_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            int iRowno = e.Item.ItemIndex;
         }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            PhoenixAccountsERMVesselAccruedExpensess.ERMVoucherPrefixDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkey")).Text));
        }
        Rebind();
    }
    protected void Rebind()
    {
        gvERMVoucherprefix.SelectedIndexes.Clear();
        gvERMVoucherprefix.EditIndexes.Clear();
        gvERMVoucherprefix.DataSource = null;
        gvERMVoucherprefix.Rebind();
    }
    //protected void gvERMVoucherprefix_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;
    //    if (e.CommandName.ToUpper().Equals("EDIT"))
    //    {
    //        int iRowno;
    //        iRowno = int.Parse(e.CommandArgument.ToString());
    //    }
    //    if (e.CommandName.ToUpper().Equals("DELETE"))
    //    {
    //        PhoenixAccountsERMVesselAccruedExpensess.ERMVoucherPrefixDelete(General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lbldtkey")).Text));
    //    }
    //    BindData();
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
       
    }

    protected void gvERMVoucherprefix_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
      
        gvERMVoucherprefix.SelectedIndexes.Add(e.NewSelectedIndex);
    }
}
