using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

using SouthNests.Phoenix.Common;


public partial class CommonPickListSpareType : PhoenixBasePage
{
	 

	protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH");
		MenuStockItem.MenuList = toolbarmain.Show();
		MenuStockItem.SetTrigger(pnlStockItem);


		if (!IsPostBack)
		{
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
		}
		BindData();
        SetPageNavigator();
	}

 	protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SEARCH"))
			{
                ViewState["PAGENUMBER"] = 1;
				BindData();
				SetPageNavigator();
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
			int iTotalPageCount = 10;
			DataSet ds;
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());      

            ds = PhoenixCommonInventory.SpareTypeSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
				 txtNumberSearch.Text, txtStockItemNameSearch.Text,
				null,null,null,sortexpression, sortdirection,
			   Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
			   ref iRowCount,
			   ref iTotalPageCount);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvStockItem.DataSource = ds;
				gvStockItem.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvStockItem);
			}

			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

    protected void cmdSearch_Click(object sender, EventArgs e)
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

	protected void gvStockItem_RowCommand(object sender, GridViewCommandEventArgs e)
	{
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        

        GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		string Script = "";
		NameValueCollection nvc;

		if (Request.QueryString["mode"] == "custom")
		{

			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			Script += "fnReloadList('codehelp1','ifMoreInfo');";
			Script += "</script>" + "\n";

			nvc = new NameValueCollection();

            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemNumber");
			nvc.Add(lbl.ID, lbl.Text);
			LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkStockItemName");
			nvc.Add(lb.ID, lb.Text.ToString());
            Label lblId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemId");
            nvc.Add(lblId.ID, lblId.Text);
            
		}
		else
		{

			Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
			Script += "fnClosePickList('codehelp1','ifMoreInfo');";
			Script += "</script>" + "\n";

			nvc = Filter.CurrentPickListSelection;

            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemNumber");
			nvc.Set(nvc.GetKey(1), lbl.Text);
			LinkButton lb = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkStockItemName");
			nvc.Set(nvc.GetKey(2), lb.Text.ToString());
            Label lblId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemId");
            nvc.Set(nvc.GetKey(3), lblId.Text);
		}

		Filter.CurrentPickListSelection = nvc;
		Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

	}

	protected void gvStockItem_RowEditing(object sender, GridViewEditEventArgs e)
	{
	}

    protected void gvStockItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
    }

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		int result;
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

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		gvStockItem.SelectedIndex = -1;
		gvStockItem.EditIndex = -1;
		if (ce.CommandName == "prev")
			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
		else
			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

		BindData();
		SetPageNavigator();
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
		{
			return true;
		}

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



    protected void gvStockItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
       
        BindData();
    }


	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

		BindData();
		SetPageNavigator();
	}
}
