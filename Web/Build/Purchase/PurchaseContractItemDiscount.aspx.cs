using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseContractItemDiscount : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            toolbarmain.AddButton("New", "NEW");
            toolbarmain.AddButton("Save", "SAVE");
            MenuFormGeneral.MenuList = toolbarmain.Show();
            MenuFormGeneral.SetTrigger(pnlFormGeneral);      
            if (!IsPostBack)
            {
                Title1.Text = "Item Discount (" + PhoenixPurchaseContract.ItemName + ")";
                if (Request.QueryString["contractitemid"] != null)
                    ViewState["contractitemid"] = Request.QueryString["contractitemid"].ToString();
                else
                    ViewState["contractitemid"] = "0";
                ViewState["SaveStatus"] = "New";

            }
           
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }

            BindData();
            SetPageNavigator();
            ViewState["SaveStatus"] = "New";
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

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseContract.ContractItemDiscoutSearch(General.GetNullableGuid( ViewState["contractitemid"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvContractItemDiscount.DataSource = ds;
            gvContractItemDiscount.DataBind();
            if (ViewState["contractitemdiscountid"] == null)
            {
                ViewState["contractitemdiscountid"] = ds.Tables[0].Rows[0]["FLDCONTRACTITEMDISCOUNTID"].ToString();
                ViewState["SaveStatus"] = "Edit";
                BindItem();
            }

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvContractItemDiscount);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;



    }
    private void BindItem()
    {
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseContract.EditContractItemDiscount(new Guid(ViewState["contractitemdiscountid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtVolume.Text = String.Format("{0:####}",dr["FLDVOLUME"]);
            txtDiscount.Text =String.Format("{0:##.00}", dr["FLDDISCOUNT"]);         

        }
    }
    private void ClearText()
    {
        txtVolume.Text = "";
        txtDiscount.Text = "";    
    }
    protected void MenuFormGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidForm())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["SaveStatus"].ToString().Equals("New"))
                {
                    InsertContractItemDiscount();
                }
                else if (ViewState["SaveStatus"].ToString().Equals("Edit"))
                {
                    UpdateContractItemDiscount();
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
               
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["SaveStatus"] = "New";
                ClearText();
            }
           
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void InsertContractItemDiscount()
    {
        PhoenixPurchaseContract.InsertContractItemDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["contractitemid"].ToString()),
                     General.GetNullableDecimal(txtVolume.Text), General.GetNullableDecimal(txtDiscount.Text));
    }
    private void UpdateContractItemDiscount()
    {
        PhoenixPurchaseContract.UpdateContractItemDiscount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["contractitemdiscountid"].ToString()),
                 General.GetNullableDecimal(txtVolume.Text), General.GetNullableDecimal(txtDiscount.Text));
    }
    private bool IsValidForm()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (txtVolume.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Volume is required.";
        if (txtDiscount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Discount is required.";  
        if(General.GetNullableGuid(ViewState["contractitemid"].ToString())==null)
            ucError.ErrorMessage = "Contract item is required.";  
        return (!ucError.IsError);
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvContractItemDiscount_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }



    protected void gvContractItemDiscount_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvContractItemDiscount_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvContractItemDiscount_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }

       
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvContractItemDiscount.SelectedIndex = -1;
            gvContractItemDiscount.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    }

    protected void gvContractItemDiscount_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseContract.DeleteContractItemDiscount(new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblItemDiscountId")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void onPurchaseOrder(object sender, CommandEventArgs e)
    {
        try
        {
            ViewState["contractitemdiscountid"] = e.CommandArgument.ToString();
            ViewState["SaveStatus"] = "Edit";
            BindItem();
            //ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?contractid=" + e.CommandArgument.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvContractItemDiscount_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }
}
