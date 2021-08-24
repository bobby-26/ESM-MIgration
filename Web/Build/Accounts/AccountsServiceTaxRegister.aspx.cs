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


public partial class AccountsServiceTaxRegister : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsServiceTaxRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSTaxRegister')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegister.AccessRights = this.ViewState;
            MenuRegister.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSTaxRegister.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        string[] alColumns = { "FLDSERVICETAXPAYMENTTYPE", "FLDSECTIONCODE", "FLDBASICRATE", "FLDPRIMARYEC", "FLDSECONDARYEC", "FLDSERVICETAX" };
        string[] alCaptions = { "Type of Payment", "Section Code", "Basic Rate", "Primary EC", "Secondary EC", "Service Tax Rate" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        dt = PhoenixAccountsServiceTaxRegister.ServiceTaxRegisterList(
            1,
            PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=ServiceTaxRegister.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Service Tax Register</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void gvSTaxRegister_Sorting(object sender, GridSortCommandEventArgs e)
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
    protected void MenuRegister_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
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
    protected void Rebind()
    {
        gvSTaxRegister.SelectedIndexes.Clear();
        gvSTaxRegister.EditIndexes.Clear();
        gvSTaxRegister.DataSource = null;
        gvSTaxRegister.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSERVICETAXPAYMENTTYPE", "FLDSECTIONCODE", "FLDBASICRATE", "FLDPRIMARYEC", "FLDSECONDARYEC", "FLDSERVICETAX" };
        string[] alCaptions = { "Type of Payment", "Section Code", "Basic Rate", "Primary EC", "Secondary EC", "Service Tax Rate" };

        DataTable dt = PhoenixAccountsServiceTaxRegister.ServiceTaxRegisterList(
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvSTaxRegister.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        DataSet ds = dt.DataSet;

        General.SetPrintOptions("gvSTaxRegister", "Service Tax Register", alCaptions, alColumns, ds);

        gvSTaxRegister.DataSource = dt;
        gvSTaxRegister.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
   
    protected void gvSTaxRegister_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSTaxRegister, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvSTaxRegister_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
           if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtPaymentType")).Text
                                , ((RadTextBox)e.Item.FindControl("txtSectionCode")).Text
                                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsServiceTaxRegister.ServiceTaxRegisterInsert(((RadTextBox)e.Item.FindControl("txtPaymentType")).Text
                                                    , ((RadTextBox)e.Item.FindControl("txtSectionCode")).Text
                                                    , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucBasicAdd")).Text)
                                                    , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucPrimaryECAdd")).Text)
                                                    , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucSecondaryECAdd")).Text)

                                                    );

                Rebind();
                ((RadTextBox)e.Item.FindControl("txtPaymentType")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(((RadLabel)e.Item.FindControl("lblPaymentTypeItem")).Text
                                , ((RadLabel)e.Item.FindControl("lblSectionCodeItem")).Text
                                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsServiceTaxRegister.ServiceTaxRegisterUpdate(new Guid(((RadLabel)e.Item.FindControl("lblPaymentId")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucBasicEdit")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucPrimaryECEdit")).Text)
                                                            , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("ucSecondaryECEdit")).Text)
                                                            );

                Rebind();

            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsServiceTaxRegister.ServiceTaxRegisterDelete(new Guid(((RadLabel)e.Item.FindControl("lblPaymentId")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSTaxRegister_RowDeleting(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }


    protected void gvSTaxRegister_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            // GridView _gridView = (GridView)sender;
            //  int nCurrentRow = e.RowIndex;

            //  _gridView.EditIndex = -1;
            Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }


    protected void gvSTaxRegister_ItemDataBound(Object sender, GridItemEventArgs e)
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


    private bool IsValidData(string paymenttype, string sectioncode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(paymenttype) == null)
            ucError.ErrorMessage = "Type of Payment is required.";

        //if (General.GetNullableString(sectioncode) == null)
        //    ucError.ErrorMessage = "Section Code is required.";


        return (!ucError.IsError);
    }



    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvSTaxRegister_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSTaxRegister.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

