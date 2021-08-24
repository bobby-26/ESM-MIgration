using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Accounts_AccountsProjectCodeFundRequestOther : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolmain = new PhoenixToolbar();

            toolmain.AddButton("Fund Request", "FUNDREQUEST", ToolBarDirection.Right);
            toolmain.AddButton("Line Item", "LINEITEM", ToolBarDirection.Right);
            toolmain.AddButton("Project", "PROJECTCODE", ToolBarDirection.Right);

            Menu.AccessRights = this.ViewState;
            Menu.MenuList = toolmain.Show();
            Menu.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Create FundRequest<br/>(Others)", "OTHERS", ToolBarDirection.Right);
            toolbarmain.AddButton("Create FundRequest<br/>(Ship Owner)", "SHIPOWNER", ToolBarDirection.Right);

            MenuLineItem.AccessRights = this.ViewState;
            MenuLineItem.MenuList = toolbarmain.Show();
            MenuLineItem.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PROJECTID"] = null;

                if (Request.QueryString["id"] != null)
                {
                    ViewState["PROJECTID"] = Request.QueryString["id"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsProjectCodeFundRequestOther.aspx", "Export to Excel", "icon_xls.png", "Excel"); ;
            toolbargrid.AddImageLink("javascript:CallPrint('gvFundRequest')", "Print Grid", "icon_print.png", "PRINT");
            //toolbargrid.AddImageLink("../Accounts/AccountsProjectCodeFundRequestOther.aspx", "Filter", "search.png", "FIND");
            //toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemConfirmation.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            //toolbargrid.AddImageButton("../Accounts/AccountsProjectCodeDebitCreditNoteGenerateOthers.aspx", "Add", "Add.png", "ADD");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsProjectCodeDebitCreditNoteGenerateOthers.aspx?OfficeDebitCreditNoteId=&ID="+ ViewState["PROJECTID"].ToString() + "')", "Add", "add.png", "ADD");

            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();

            //BindData();
            ProjectCodeEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Menu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (ViewState["PROJECTID"] != null)
            {
                if (CommandName.ToUpper().Equals("PROJECTCODE"))
                {
                    Response.Redirect("../Accounts/AccountsProjectList.aspx?id=" + ViewState["PROJECTID"].ToString());
                }
                else if (CommandName.ToUpper().Equals("LINEITEM"))
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString(), true);
                }
                else if (CommandName.ToUpper().Equals("FUNDREQUEST"))
                {
                    Response.Redirect("../Accounts/AccountsProjectCodeFundRequest.aspx?id=" + ViewState["PROJECTID"].ToString(), true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (ViewState["PROJECTID"] != null)
            {
                if (CommandName.ToUpper().Equals("OTHERS"))
                {
                    Response.Redirect("../Accounts/AccountsProjectCodeFundRequestOther.aspx?id=" + ViewState["PROJECTID"].ToString());
                }
                if (CommandName.ToUpper().Equals("SHIPOWNER"))
                {
                    Response.Redirect("../Accounts/AccountsProjectCodeFundRequest.aspx?id=" + ViewState["PROJECTID"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsProjectCodeDebitCreditNoteGenerateOthers.aspx?id=" + ViewState["PROJECTID"].ToString() + " ');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.ProjectCodeConfirmationListFilter = null;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                if (ViewState["PROJECTID"] != null)
                {
                    Response.Redirect("../Accounts/AccountsProjectLineItemConfirmation.aspx?id=" + ViewState["PROJECTID"].ToString());
                }
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDTYPE", "FLDBANKDESCRIPTION", "FLDLINEITEM", "FLDLINEITEMALL", "FLDAMOUNTINUSD", "FLDRECEIVEDAMOUNT", "FLDREMARKS" };
                string[] alCaptions = { "Vessel Name", "Date", "Ref:No", "Code", "Type", "Bank Description", "Lineitem", "Lineitem all", "Amount(USD)", "Received Amount", "Remarks" };

                string sortexpression;
                int? sortdirection = 1;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixAccountProjectPurchaseLineItem.ProjectcodeOfficeFundRequestList(null, null, null, null, null, null, null, null, null, null, null,null
                                                                         , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                         , General.ShowRecords(null)
                                                                         , ref iRowCount, ref iTotalPageCount                                                                        
                                                                        , General.GetNullableInteger(ViewState["PROJECTID"].ToString()
                                                                         ));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDTYPE", "FLDBANKDESCRIPTION", "FLDLINEITEM", "FLDLINEITEMALL", "FLDAMOUNTINUSD", "FLDRECEIVEDAMOUNT", "FLDREMARKS" };
            string[] alCaptions = { "Vessel Name", "Date", "Ref:No", "Code", "Type", "Bank Description", "Lineitem", "Lineitem all", "Amount(USD)","Received Amount","Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["PROJECTID"] != null)
            {
                DataTable dt = PhoenixAccountProjectPurchaseLineItem.ProjectcodeOfficeFundRequestList(null, null, null, null, null, null, null, null, null, null, null, null
                                                                         , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                         , General.ShowRecords(null)
                                                                         , ref iRowCount, ref iTotalPageCount
                                                                        , General.GetNullableInteger(ViewState["PROJECTID"].ToString()
                                                                         ));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt.Copy());

                General.SetPrintOptions("gvFundRequest", "Fund Request(Others)", alCaptions, alColumns, ds);


                gvFundRequest.DataSource = dt;
                gvFundRequest.VirtualItemCount = iRowCount;

            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ProjectCodeEdit()
    {
        DataSet ds = PhoenixAccountProjectPurchaseLineItem.FundRequestEdit(General.GetNullableInteger(ViewState["PROJECTID"].ToString()));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtOutFlow1.Text = dr["FLDPURCHASEAMOUNT"].ToString();
                txtEstOutFlow2.Text = dr["FLDAWAITINGCONFIRMATIONAMOUNT"].ToString();
                txtFundReqRev1.Text = dr["FLDVOUCHERLINEITEMAMOUNT"].ToString();
                txtFundReqRev2.Text = dr["FLDFUNDREQUESTAMOUNT"].ToString();
                txtEstFundPosition1.Text = dr["FLDFUNDRECEIVEDAMOUNT"].ToString();
                txtEstFundPosition2.Text = dr["FLDFUNDRECEIVEDAMOUNT"].ToString();
            }
        }
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //RadGrid _gridView = (RadGrid)sender;
            //int nCurrentRow = e.Item.ItemIndex;
            //if (e.CommandName.ToUpper().Equals("UPDATE"))
            //{
            //    string txtEstimatedAmountEdit = ((TextBox)_gridView.Items[nCurrentRow].FindControl("txtEstimatedAmountEdit")).Text;
            //    string lblConfirmationLineItemId = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblConfirmationLineItemIdEdit")).Text;
            //    if (General.GetNullableGuid(lblConfirmationLineItemId) != null)
            //    {
            //        PhoenixAccountProjectConfirmationLineItem.ConfirmationLineItemUpdate(General.GetNullableDecimal(txtEstimatedAmountEdit), new Guid(lblConfirmationLineItemId));
            //    }
            //    Rebind();
            //    ProjectCodeEdit();
            //}
            //if (e.CommandName.ToUpper().Equals("DELETE"))
            //{
            //    string lblConfirmationLineItemId = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblConfirmationLineItemId")).Text;
            //    string lblConfirmationType = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblConfirmationType")).Text;
            //    if (General.GetNullableGuid(lblConfirmationLineItemId) != null && General.GetNullableInteger(lblConfirmationType) != null)
            //    {
            //        PhoenixAccountProjectConfirmationLineItem.DeleteConfirmationLineiTem(new Guid(lblConfirmationLineItemId), General.GetNullableInteger(lblConfirmationType));
            //    }

            //    Rebind();
            //    ProjectCodeEdit();
            //}
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ProjectCodeEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = de.RowIndex;
    //        string lblConfirmationLineItemId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblConfirmationLineItemId")).Text;
    //        string lblConfirmationType = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblConfirmationType")).Text;
    //        if (General.GetNullableGuid(lblConfirmationLineItemId) != null && General.GetNullableInteger(lblConfirmationType) != null)
    //        {
    //            PhoenixAccountProjectConfirmationLineItem.DeleteConfirmationLineiTem(new Guid(lblConfirmationLineItemId), General.GetNullableInteger(lblConfirmationType));
    //        }

    //        Rebind();
    //        ProjectCodeEdit();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvLineItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    try
    //    {
    //        int nCurrentRow = e.RowIndex;

    //        string txtEstimatedAmountEdit = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEstimatedAmountEdit")).Text;
    //        string lblConfirmationLineItemId = ((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblConfirmationLineItemIdEdit")).Text;
    //        if (General.GetNullableGuid(lblConfirmationLineItemId) != null)
    //        {
    //            PhoenixAccountProjectConfirmationLineItem.ConfirmationLineItemUpdate(General.GetNullableDecimal(txtEstimatedAmountEdit), new Guid(lblConfirmationLineItemId));
    //        }
    //        _gridView.EditIndex = -1;
    //        Rebind();
    //        ProjectCodeEdit();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvLineItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }

    //protected void gvLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvLineItem_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvLineItem, "Edit$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        {
            ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
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
        gvFundRequest.SelectedIndexes.Clear();
        gvFundRequest.EditIndexes.Clear();
        gvFundRequest.DataSource = null;
        gvFundRequest.Rebind();
    }

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFundRequest.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}