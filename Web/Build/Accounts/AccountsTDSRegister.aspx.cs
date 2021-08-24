using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using System.Web.Profile;
using Telerik.Web.UI;


public partial class AccountsTDSRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsTDSRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvTDSRegister')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegister.AccessRights = this.ViewState;
            MenuRegister.MenuList = toolbar.Show();
           // MenuRegister.SetTrigger(pnlStateEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvTDSRegister.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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

        DataTable dt = new DataTable();
        string[] alColumns = { "FLDPAYMENTTYPE", "FLDSECTIONCODE", "FLDTAX", "FLDSURCHARGE", "FLDEDUCATIONCESS", "FLDTDS", "FLDEFFECTIVEDATE" };
        string[] alCaptions = { "Type of Payment", "Section Code", "Tax", "Surcharge", "Edu. Cess", "TDS", "Effective Date" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        dt = PhoenixAccountsTDSRegister.TDSRegisterList(
            1,
            PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=TDSRegister.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>TDS Register</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(alColumns[i].ToString() == "FLDEFFECTIVEDATE" ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }


    protected void MenuRegister_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvTDSRegister.EditIndex = -1;
              //  gvTDSRegister.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        string[] alColumns = { "FLDPAYMENTTYPE", "FLDSECTIONCODE", "FLDTAX", "FLDSURCHARGE", "FLDEDUCATIONCESS", "FLDTDS", "FLDEFFECTIVEDATE" };
        string[] alCaptions = { "Type of Payment", "Section Code", "Tax", "Surcharge", "Edu. Cess", "TDS", "Effective Date" };

        DataTable dt = PhoenixAccountsTDSRegister.TDSRegisterList(
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           gvTDSRegister.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvTDSRegister", "TDS Register", alCaptions, alColumns, ds);

     
            gvTDSRegister.DataSource = dt;
            gvTDSRegister.VirtualItemCount=iRowCount;
     
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvTDSRegister_Sorting(object sender, GridSortCommandEventArgs e)
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
    protected void gvTDSRegister_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTDSRegister, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvTDSRegister_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
         if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtPaymentType")).Text
                                , ((RadTextBox)e.Item.FindControl("txtSectionCode")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucTaxAdd")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucSurchargeAdd")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucEduCessAdd")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucTDSAdd")).Text
                                , ((UserControlDate)e.Item.FindControl("ucEffectiveDate")).Text
                                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsTDSRegister.TDSRegisterInsert(((RadTextBox)e.Item.FindControl("txtPaymentType")).Text
                                                    , ((RadTextBox)e.Item.FindControl("txtSectionCode")).Text
                                                    , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucTaxAdd")).Text)
                                                    , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucSurchargeAdd")).Text)
                                                    , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucEduCessAdd")).Text)
                                                    , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucTDSAdd")).Text)
                                                    , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucEffectiveDate")).Text)
                                                    );

                Rebind();
                ((RadTextBox)e.Item.FindControl("txtPaymentType")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(((RadLabel)e.Item.FindControl("lblPaymentTypeItem")).Text
                                , ((RadLabel)e.Item.FindControl("lblSectionCodeItem")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucTaxEdit")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucSurchargeEdit")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucEduCessEdit")).Text
                                , ((UserControlDecimal)e.Item.FindControl("ucTDSEdit")).Text
                                , ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text
                                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsTDSRegister.TDSRegisterUpdate(new Guid(((RadLabel)e.Item.FindControl("lblPaymentId")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucTaxEdit")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucSurchargeEdit")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucEduCessEdit")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucTDSEdit")).Text)
                                                            , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text)
                                                            );

                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsTDSRegister.TDSRegisterDelete(new Guid(((RadLabel)e.Item.FindControl("lblPaymentId")).Text));
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

    protected void gvTDSRegister_RowDeleting(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }

  
    protected void gvTDSRegister_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            // int nCurrentRow = e.RowIndex;

            //  _gridView.EditIndex = -1;
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void Rebind()
    {
        gvTDSRegister.SelectedIndexes.Clear();
        gvTDSRegister.EditIndexes.Clear();
        gvTDSRegister.DataSource = null;
        gvTDSRegister.Rebind();
    }

    protected void gvTDSRegister_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        ImageButton add = (ImageButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

     

        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }

    }

   
 
    private bool IsValidData(string paymenttype, string sectioncode, string tax, string surcharge, string educess, string tds, string effectiveDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(paymenttype) == null)
            ucError.ErrorMessage = "Type of Payment is required.";

        if (General.GetNullableString(sectioncode) == null)
            ucError.ErrorMessage = "Section Code is required.";

        if (General.GetNullableDecimal(tax) == null)
            ucError.ErrorMessage = "Tax is required.";

        if (General.GetNullableDecimal(surcharge) == null)
            ucError.ErrorMessage = "Surcharge is required.";

        if (General.GetNullableDecimal(educess) == null)
            ucError.ErrorMessage = "Edu. Cess is required.";

        if (General.GetNullableDecimal(tds) == null)
            ucError.ErrorMessage = "TDS is required.";

        if (General.GetNullableDateTime(effectiveDate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        return (!ucError.IsError);
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvTDSRegister_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTDSRegister.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

