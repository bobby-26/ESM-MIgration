using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Accounts_AccountsProjectLineItemConfirmationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ucFormType.HardTypeCode = ((int)PhoenixHardTypeCode.FORMTYPE).ToString();
                ViewState["PROJECTID"] = null;

                if (Request.QueryString["id"] != null)
                {
                    ViewState["PROJECTID"] = Request.QueryString["id"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TOTALPAGECOUNT"] = 1;
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemConfirmationAdd.aspx", "Filter", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsProjectLineItemConfirmationAdd.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOrderLineItem.AccessRights = this.ViewState;
            MenuOrderLineItem.MenuList = toolbargrid.Show();
            ProjectCodeEdit();
        //    BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        int nCurrentRow = e.Item.ItemIndex;

        if (e.CommandName.ToUpper() == "ADD")
        {
            string lblorderid = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblorderid")).Text;

            if (General.GetNullableGuid(lblorderid) != null)
            {
                PhoenixAccountProjectConfirmation.InsertConfirmationLineItem(new Guid(lblorderid), General.GetNullableInteger(ViewState["PROJECTID"].ToString()),General.GetNullableInteger(ddlType.SelectedValue));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null,true);", true);
                Rebind();
            }
        }

        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void DisableType(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "1") //Purchase Form
        {
            ucFormType.Enabled = true;
            ucFormType.CssClass = "input";
        }
        else
        {
            ucFormType.Enabled = false;
            ucFormType.SelectedHard = "";
        }

    }


    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucFormType.SelectedHard = "";
                txtCreatedDate.Text = "";
                txtFormNumber.Text = "";
                txtFormDescription.Text = "";
             //   txtnopage.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ProjectCodeEdit()
    {
        if (ViewState["PROJECTID"] != null)
        {

            DataTable dt = PhoenixAccountProject.ProjectEdit(int.Parse(ViewState["PROJECTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["ACCOUNTID"] = dt.Rows[0]["FLDACCOUNTID"].ToString();
            }
        }
    }


    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = new DataTable();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //default desc order
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string accountid = (ViewState["ACCOUNTID"] == null) ? null : (ViewState["ACCOUNTID"].ToString());

        dt = PhoenixAccountProjectConfirmation.ConfirmationLineItemAddSearch(General.GetNullableInteger(ViewState["PROJECTID"].ToString())
                                                               , General.GetNullableInteger(ddlType.SelectedValue)
                                                               , General.GetNullableInteger(ucFormType.SelectedHard)
                                                               , General.GetNullableString(txtFormNumber.Text.Trim())
                                                               , General.GetNullableDateTime(txtCreatedDate.Text)
                                                               , General.GetNullableString(txtFormDescription.Text)
                                                               , General.GetNullableInteger(accountid)
                                                               , sortexpression
                                                               , sortdirection
                                                               , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);


            gvGrid.DataSource = dt;
        gvGrid.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    //private void SetPageNavigator()
    //{
    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
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

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvGrid.SelectedIndex = -1;
    //        gvGrid.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    try
    //    {
    //        dt.Rows.Add(dt.NewRow());
    //        gv.DataSource = dt;
    //        gv.DataBind();

    //        int colcount = gv.Columns.Count;
    //        gv.Rows[0].Cells.Clear();
    //        gv.Rows[0].Cells.Add(new TableCell());
    //        gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //        gv.Rows[0].Cells[0].Font.Bold = true;
    //        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void Rebind()
    {
        gvGrid.SelectedIndexes.Clear();
        gvGrid.EditIndexes.Clear();
        gvGrid.DataSource = null;
        gvGrid.Rebind();
    }
    protected void gvGrid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGrid.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}