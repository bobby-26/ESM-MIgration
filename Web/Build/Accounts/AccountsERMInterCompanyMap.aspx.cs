using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsERMInterCompanyMap : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvERMInterCompanyMap.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvERMInterCompanyMap.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsERMInterCompanyMap.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvERMInterCompanyMap')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsERMInterCompanyMap.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsERMInterCompanyMap.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuInterCompanyMap.AccessRights = this.ViewState;
            MenuInterCompanyMap.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvERMInterCompanyMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();

            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuInterCompanyMap_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                XAccount.Text = string.Empty;
                ERMDescription.Text = string.Empty;
                PhoenixCode.Text = string.Empty;
                PhoenixDescription.Text = string.Empty;
                ERMBookName.Text = string.Empty;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvERMInterCompanyMap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                int iRowno;
                iRowno = e.Item.ItemIndex;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (Validate((((RadTextBox)e.Item.FindControl("txtPhoenixCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtPhoenixAccDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMBookNameAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDadd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAccAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMAccDescAdd")).Text)))
                {
                    PhoenixAccountsERMInterCompanyMap.ERMInterCompanyMapInsert((((RadTextBox)e.Item.FindControl("txtPhoenixCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtPhoenixAccDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMBookNameAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDadd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAccAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMAccDescAdd")).Text)
                                                          );
                    ucStatus.Text = "Account Saved Successfully";
                    gvERMInterCompanyMap.Rebind();
                }

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (Validate((((RadTextBox)e.Item.FindControl("txtPhoenixCodeEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtPhoenixAccDescEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMBookNameEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAccEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMAccDescEdit")).Text)))
                {

                    PhoenixAccountsERMInterCompanyMap.ERMInterCompanyMapUpdate((((RadTextBox)e.Item.FindControl("txtPhoenixCodeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtPhoenixAccDescEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtERMBookNameEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtZIDEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXAccEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtERMAccDescEdit")).Text),
                                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkeyEdit")).Text)
                                                              );

                    ucStatus.Text = "Account Updated Successfully";
                    gvERMInterCompanyMap.Rebind();
                }
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsERMInterCompanyMap.ERMInterCompanyMapDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkey")).Text));
                gvERMInterCompanyMap.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool Validate(string PhoenixCode
                                                    , string PhoenixDesc
                                                    , string ERMBookName
                                                    , string ZID
                                                    , string XAcc
                                                    , string ERMDesc)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (string.IsNullOrEmpty(PhoenixCode))
        //    ucError.ErrorMessage = "Phoenix Account Code is required.";
        //if (string.IsNullOrEmpty(PhoenixDesc))
        //    ucError.ErrorMessage = "Phoenix Account Description is required.";
        //if (string.IsNullOrEmpty(ERMBookName))
        //    ucError.ErrorMessage = "ERM Book Name is required.";
        //if (string.IsNullOrEmpty(ZID))
        //    ucError.ErrorMessage = "ZID is required.";
        //if (string.IsNullOrEmpty(XAcc))
        //    ucError.ErrorMessage = "X Account is required.";
        //if (string.IsNullOrEmpty(ERMDesc))
        //    ucError.ErrorMessage = "ERM Account Description is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDPHOENIXCODE", "FLDPHOENIXDESCRIPTION", "FLDERMBOOKNAME", "FLDZID", "FLDXACC", "FLDERMDESCRIPTION" };
        string[] alCaptions = { "Phoenix Account Code", "Phoenix Account Description", "ERM Book Name", "ZID", "X Account", "ERM Account Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsERMInterCompanyMap.ERMInterCompanyMapSearch(XAccount.Text, ERMDescription.Text, PhoenixCode.Text, PhoenixDescription.Text, ERMBookName.Text, (int)ViewState["PAGENUMBER"],
                                                    gvERMInterCompanyMap.PageSize, ref iRowCount, ref iTotalPageCount
                                                   );

        General.SetPrintOptions("gvERMInterCompanyMap", "ERM Other Sub Account", alCaptions, alColumns, ds);

        gvERMInterCompanyMap.DataSource = ds;
        gvERMInterCompanyMap.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvERMInterCompanyMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvERMInterCompanyMap.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Rebind()
    {
        gvERMInterCompanyMap.EditIndexes.Clear();
        gvERMInterCompanyMap.SelectedIndexes.Clear();
        gvERMInterCompanyMap.DataSource = null;
        gvERMInterCompanyMap.Rebind();
    }
    protected void gvERMInterCompanyMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridHeaderItem)
            {
                ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvERMInterCompanyMap_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvERMInterCompanyMap, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }


    protected void gvERMInterCompanyMap_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvERMInterCompanyMap_Sorting(object sender, GridSortCommandEventArgs e)
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDPHOENIXCODE", "FLDPHOENIXDESCRIPTION", "FLDERMBOOKNAME", "FLDZID", "FLDXACC", "FLDERMDESCRIPTION" };
        string[] alCaptions = { "Phoenix Account Code", "Phoenix Account Description", "ERM Book Name", "ZID", "X Account", "ERM Account Description" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsERMInterCompanyMap.ERMInterCompanyMapSearch(XAccount.Text, ERMDescription.Text, PhoenixCode.Text, PhoenixDescription.Text, ERMBookName.Text, (int)ViewState["PAGENUMBER"],
                                             PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount
                                            );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ERMOtherSubAccount.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ERM Other Sub Account</h3></td>");
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
