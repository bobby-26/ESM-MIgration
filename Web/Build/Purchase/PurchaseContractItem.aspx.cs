using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;


public partial class PurchaseContractItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {        

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("General", "GENERAL");           
            toolbarsub.AddButton("Stock Item", "MAPPING");
            toolbarsub.AddButton("Discount", "DISCOUNT");
            toolbarsub.AddButton("Contract", "CONTRACT");           
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            MenuContractItem.MenuList = toolbarsub.Show(); 

           

            if (!IsPostBack)
            {
                ViewState["contractitemid"] = "0";
                Title1.Text = "Purchase Contract Item (" + PhoenixPurchaseContract.VendorName + ")";
                if (Request.QueryString["contractid"] != null)
                {
                    ViewState["contractid"] = Request.QueryString["contractid"].ToString();
                    ViewState["PAGEURL"] = "PurchaseContractItemGeneral.aspx";
                    ifMoreInfo.Attributes["src"] = "PurchaseContractItemGeneral.aspx?contractid=" + Request.QueryString["contractid"].ToString();
                }
                else
                {
                    ViewState["contractid"] = "0";
                    ViewState["PAGEURL"] = "PurchaseContractItemGeneral.aspx";
                    ifMoreInfo.Attributes["src"] = "PurchaseContractItemGeneral.aspx";
                }               
            }


            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseContractItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvContractItem')", "Print Grid", "icon_print.png", "");
            //toolbargrid.AddImageButton("../Purchase/PurchaseContractItem.aspx", "<b>Find</b>", "search.png", "FIND");

            MenuPurchaseContract.MenuList = toolbargrid.Show();
        
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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
    protected void MenuContractItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {

            if (dce.CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseContractItemGeneral.aspx"; ;
                if (ViewState["contractitemid"].ToString() =="0" )
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractItemGeneral.aspx";
                else
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractItemGeneral.aspx?contractitemid=" + ViewState["contractitemid"].ToString();
            }
            if (dce.CommandName.ToUpper().Equals("DISCOUNT"))
            {
                if (IsVolume(ViewState["contractitemid"].ToString()))
                {
                    ViewState["PAGEURL"] = "../Purchase/PurchaseContractItemDiscount.aspx"; ;
                    if (ViewState["contractitemid"].ToString() == "0")
                        ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractItemDiscount.aspx";
                    else
                        ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractItemDiscount.aspx?contractitemid=" + ViewState["contractitemid"].ToString();
                }
                    
            }
            if (dce.CommandName.ToUpper().Equals("MAPPING"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseContractItemMapping.aspx";
                if (ViewState["contractitemid"].ToString() == "0")
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractItemMapping.aspx";
                else
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractItemMapping.aspx?contractitemid=" + ViewState["contractitemid"].ToString();
            }
            if (dce.CommandName.ToUpper().Equals("CONTRACT"))
            {
                if (ViewState["contractid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseContract.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseContract.aspx?contractid=" + ViewState["contractid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsVolume(string contractitemid)
    {
        DataSet ds = new DataSet();
        if (General.GetNullableGuid(contractitemid) != null)
        {
            ds = PhoenixPurchaseContract.EditContractItem(new Guid(contractitemid));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["FLDDISCOUNTTYPE"].ToString() == "" || ds.Tables[0].Rows[0]["FLDDISCOUNTTYPE"].ToString() == "64")
                    return false;
            }
        }
        return true; 
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDPARTNUMBER", "FLDPARTNAME", "FLDCOMMENTS", "FLDMINQUANTITY", "FLDMAXQUANTITY" };
        string[] alCaptions = { "Number","Description ","Comments", "Min Quantity","Max Quantity"};
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseContract.ContractItemSearch(General.GetNullableGuid(""), General.GetNullableGuid(ViewState["contractid"].ToString()), null, null, null, sortexpression,
                        sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PurchaseContract.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.jpg' /></td>");
        Response.Write("<td><h3>Purchase Contract</h3></td>");
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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
        string[] alColumns = { "FLDPARTNUMBER", "FLDPARTNAME", "FLDCOMMENTS", "FLDMINQUANTITY", "FLDMAXQUANTITY" };
        string[] alCaptions = { "Number", "Description ", "Comments", "Min Quantity", "Max Quantity" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseContract.ContractItemSearch(General.GetNullableGuid(""), General.GetNullableGuid(ViewState["contractid"].ToString()), null, null, null, sortexpression, 
                        sortdirection, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvContractItem.DataSource = ds;
            gvContractItem.DataBind();
            if (ViewState["contractitemid"].ToString() == "0")
            {
                ViewState["contractitemid"] = ds.Tables[0].Rows[0]["FLDCONTRACTITEMID"].ToString();
                ifMoreInfo.Attributes["src"] = "PurchaseContractItemGeneral.aspx?contractid=" + ViewState["contractid"].ToString() + "&contractitemid=" + ViewState["contractitemid"].ToString();
                ViewState["PAGEURL"] = "PurchaseContractItemGeneral.aspx";
                PhoenixPurchaseContract.ItemName = ds.Tables[0].Rows[0]["FLDPARTNAME"].ToString();
            }
           

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvContractItem);
            PhoenixPurchaseContract.ItemName = " ";
        }
     
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvContractItem", "Vendor Contract Items", alCaptions, alColumns, ds);   
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

    protected void gvContractItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    

    protected void gvContractItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvContractItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvContractItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            gvContractItem.SelectedIndex = -1;
            gvContractItem.EditIndex = -1;
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

    protected void gvContractItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseContract.DeleteContractItem(new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblContractItemId")).Text));
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
            ViewState["contractitemid"] = e.CommandArgument.ToString();
            DataSet ds = new DataSet();
            ds = PhoenixPurchaseContract.EditContractItem(new Guid(ViewState["contractitemid"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                PhoenixPurchaseContract.ItemName = dr["FLDPARTNAME"].ToString();
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?contractid=" + ViewState["contractid"].ToString() + "&contractitemid=" + e.CommandArgument.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
          
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvContractItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvContractItem.SelectedIndex = e.NewSelectedIndex;
    }
    protected void gvContractItem_Sorting(object sender, GridViewSortEventArgs e)
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
