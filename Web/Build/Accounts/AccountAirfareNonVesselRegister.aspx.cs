using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountAirfareNonVesselRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountAirfareNonVesselRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAirfare')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountAirfareNonVesselRegister.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountAirfareNonVesselRegister.aspx", "Clear", "clear-filter.png", "CLEAR");
            MenuOpeningSummaryGrid.AccessRights = this.ViewState;
            MenuOpeningSummaryGrid.MenuList = toolbar.Show();

            PhoenixToolbar toolbarMain = new PhoenixToolbar();
            toolbarMain.AddButton(" Account ", "AIRFARE(A/C)CODE", ToolBarDirection.Right);
            toolbarMain.AddButton("Airfare", "AIRFARE",ToolBarDirection.Right);
            MenuAirfareMain.Title = "Accounts";
            MenuAirfareMain.AccessRights = this.ViewState;
            MenuAirfareMain.MenuList = toolbarMain.Show();
            MenuAirfareMain.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
               
                ViewState["PAGENUMBER"] = 1;
                gvAirfare.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
         //   BindData();
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAirfareMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("AIRFARE"))
            {
                Response.Redirect("../Accounts/AccountAirfareMarkupRegister.aspx");
            }
            if (CommandName.ToUpper().Equals("AIRFARE(A/C)CODE"))
            {
                Response.Redirect("../Accounts/AccountAirfareNonVesselRegister.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuOpeningSummaryGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                txtAccountcode.Text = "";
                ddlBillToCompany.SelectedCompany = "";
                Rebind();
              
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
        gvAirfare.SelectedIndexes.Clear();
        gvAirfare.EditIndexes.Clear();
        gvAirfare.DataSource = null;
        gvAirfare.Rebind();
    }
    protected void gvAirfare_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

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
                DataRowView drv = (DataRowView)e.Item.DataItem;
                UserControlCompany ddlCompanyEdit = (UserControlCompany)e.Item.FindControl("ddlCompanyEdit");
                if (ddlCompanyEdit != null) ddlCompanyEdit.SelectedCompany = drv["FLDBILLTOCOMPANYID"].ToString();
                ImageButton imgShowAccountEdit = (ImageButton)e.Item.FindControl("imgShowAccountEdit");
                if (imgShowAccountEdit != null)
                    imgShowAccountEdit.Attributes.Add("onclick", "return showPickList('spnPickListAccountEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccount.aspx', true); ");


            }
            if (e.Item is GridFooterItem)
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }


                ImageButton ibtnshowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
                if (ibtnshowAccount != null)
                    ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccount.aspx', true); ");

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvAirfare_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

    //        ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (db != null)
    //            {
    //                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }
    //        }
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        UserControlCompany ddlCompanyEdit = (UserControlCompany)e.Row.FindControl("ddlCompanyEdit");
    //        if (ddlCompanyEdit != null) ddlCompanyEdit.SelectedCompany = drv["FLDBILLTOCOMPANYID"].ToString();
    //        ImageButton imgShowAccountEdit = (ImageButton)e.Row.FindControl("imgShowAccountEdit");
    //        if (imgShowAccountEdit != null)
    //            imgShowAccountEdit.Attributes.Add("onclick", "return showPickList('spnPickListAccountEdit', 'codehelp1', '', '"+Session["sitepath"]+"/Common/CommonPickListAccount.aspx', true); ");


    //    }

    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (db != null)
    //        {
    //            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                db.Visible = false;
    //        }


    //        ImageButton ibtnshowAccount = (ImageButton)e.Row.FindControl("imgShowAccount");
    //        if (ibtnshowAccount != null)
    //            ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListAccount', 'codehelp1', '', '"+Session["sitepath"]+"/Common/CommonPickListAccount.aspx', true); ");

    //    }
    //}

    //protected void gvAirfare_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindData();
    //}

    protected void gvAirfare_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountId")).Text
                                        , ((UserControlCompany)e.Item.FindControl("ddlCompanyAdd")).SelectedCompany
                                        ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsAirfareNonVessel.AirfareNonvesselregisterinsert(int.Parse(((RadTextBox)e.Item.FindControl("txtAccountId")).Text)
                                                                            , int.Parse(((UserControlCompany)e.Item.FindControl("ddlCompanyAdd")).SelectedCompany)
                                                                            , (((CheckBox)e.Item.FindControl("chkActiveYN")).Checked) ? 1 : 0
                                                                              );

                ucStatus.Text = "Airfare Account Code inserted";
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountIdEdit")).Text
                    , ((UserControlCompany)e.Item.FindControl("ddlCompanyEdit")).SelectedCompany
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsAirfareNonVessel.AirfareNonvesselregisterUpdate(new Guid(((RadLabel)e.Item.FindControl("lblairfarenonvesselregisterIdEdit")).Text)
                                                                            , int.Parse(((RadTextBox)e.Item.FindControl("txtAccountIdEdit")).Text)
                                                                            , int.Parse(((UserControlCompany)e.Item.FindControl("ddlCompanyEdit")).SelectedCompany)
                                                                            , (((CheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                                                                            );
                
                  ucStatus.Text = "Airfare Account Code updated";


                Rebind();


            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixAccountsAirfareNonVessel.AirfareNonvesselregisterdelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(((RadLabel)e.Item.FindControl("lblairfarenonvesselregisterId")).Text));
               
                ucStatus.Text = "Airfare Account Code  deleted";
                Rebind();

            }

            else if (e.CommandName.ToUpper().Equals("SELECT"))
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
    //protected void gvAirfare_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        int iRowno;
    //        iRowno = int.Parse(e.CommandArgument.ToString());

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            if (!IsValidData(((TextBox)_gridView.FooterRow.FindControl("txtAccountId")).Text
    //                                , ((UserControlCompany)_gridView.FooterRow.FindControl("ddlCompanyAdd")).SelectedCompany
    //                                ))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            PhoenixAccountsAirfareNonVessel.AirfareNonvesselregisterinsert(int.Parse(((TextBox)_gridView.FooterRow.FindControl("txtAccountId")).Text)
    //                                                                        , int.Parse(((UserControlCompany)_gridView.FooterRow.FindControl("ddlCompanyAdd")).SelectedCompany)
    //                                                                        , (((CheckBox)_gridView.FooterRow.FindControl("chkActiveYN")).Checked) ? 1 : 0
    //                                                                          );

    //            ucStatus.Text = "Airfare Account Code inserted";
    //        }
    //        else if (e.CommandName.ToUpper().Equals("SAVE"))
    //        {
    //            if (!IsValidData(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAccountIdEdit")).Text
    //                , ((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ddlCompanyEdit")).SelectedCompany
    //                ))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            PhoenixAccountsAirfareNonVessel.AirfareNonvesselregisterUpdate(new Guid(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblairfarenonvesselregisterIdEdit")).Text)
    //                                                                        , int.Parse(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAccountIdEdit")).Text)
    //                                                                        , int.Parse(((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ddlCompanyEdit")).SelectedCompany)
    //                                                                        , (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
    //                                                                        );

    //            ucStatus.Text = "Airfare Account Code updated";
    //            _gridView.EditIndex = -1;
    //            BindData();
    //            SetPageNavigator();
    //        }
    //        else if (e.CommandName.ToUpper().Equals("DELETE"))
    //        {
    //            PhoenixAccountsAirfareNonVessel.AirfareNonvesselregisterdelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                                            , new Guid(((Literal)_gridView.Rows[nCurrentRow].FindControl("lblairfarenonvesselregisterId")).Text));

    //            ucStatus.Text = "Airfare Account Code  deleted";
    //        }

    //        else if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            BindPageURL(iRowno);
    //            SetRowSelection();
    //        }

    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvAirfare_RowCreated(object sender, GridViewRowEventArgs e)
    //{

    //}

    protected void gvAirfare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAirfare.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void gvAirfare_RowUpdating(object sender, GridViewUpdateEventArgs e)
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


    protected void gvAirfare_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvAirfare.SelectedIndexes.Add(e.NewSelectedIndex);
        BindPageURL(e.NewSelectedIndex);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCOMPANYNAME", "FLDMARKUP" };
        string[] alCaptions = { "Account", "Description", "Company", "Markup" };

        DataSet ds = new DataSet();

        ds = PhoenixAccountsAirfareNonVessel.AirfareNonvesselregistersearch(General.GetNullableString(txtAccountcode.Text), General.GetNullableString(ddlBillToCompany.SelectedCompany), (int)ViewState["PAGENUMBER"],
                                                        gvAirfare.PageSize,
                                                          ref iRowCount,
                                                          ref iTotalPageCount);

        General.SetPrintOptions("gvAirfare", "Airfare Account Code", alCaptions, alColumns, ds);

        gvAirfare.DataSource = ds;
        gvAirfare.VirtualItemCount = iRowCount;

      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
       
        BindData();
        
    }

   
    private bool IsValidData(string accountcode, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(accountcode) == null)
            ucError.ErrorMessage = "Account code is required.";
        if (General.GetNullableInteger(company) == null)
            ucError.ErrorMessage = "Company is required.";


        return (!ucError.IsError);
    }
   
    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvAirfare.Items[rowindex];
            RadLabel lbl = ((RadLabel)gvAirfare.Items[rowindex].FindControl("lblairfarenonvesselregisterId"));
            if (lbl != null)
                ViewState["OpeningSummaryId"] = lbl.Text;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   

    private void SetRowSelection()
    {
        gvAirfare.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvAirfare.Items)
        {
            if (item.GetDataKeyValue("FLDAIRFARENONVESSELREGISTERID").ToString().Equals(ViewState["lblairfarenonvesselregisterId"].ToString()))
            {
                gvAirfare.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDCOMPANYNAME", "FLDMARKUP" };
        string[] alCaptions = { "Account", "Description", "Company", "Markup" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsAirfareNonVessel.AirfareNonvesselregistersearch(General.GetNullableString(txtAccountcode.Text), General.GetNullableString(ddlBillToCompany.SelectedCompany), (int)ViewState["PAGENUMBER"],
                                                          PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                          ref iRowCount,
                                                          ref iTotalPageCount);



        Response.AddHeader("Content-Disposition", "attachment; filename=AirfareAccountCode.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Airfare Account Code</h3></td>");
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
}

