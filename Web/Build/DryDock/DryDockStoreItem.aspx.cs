using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;


public partial class DryDockStoreItem : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH");
        toolbarmain.AddButton("Save", "SAVE");

        MenuStockItem.MenuList = toolbarmain.Show();
		

		if (!IsPostBack)
		{
            ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
            ddlStockClass.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)PhoenixHardTypeCode.STORETYPE);
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
		}
		BindData();
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
            else if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridViewRow row in gvStoreItem.Rows)
                {
                    UserControlMaskNumber lbl = (UserControlMaskNumber)row.FindControl("txtQuantity");
                    if (lbl == null)
                    {
                        ucError.ErrorMessage = "No Store Item to save";
                        ucError.Visible = true;
                        return;
                    }
                    decimal qty;
                    if (decimal.TryParse(lbl.Text, out qty) && qty > 0)
                    {

                    }
                }
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

            int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixCommonInventory.StoreItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                vesselid, txtNumberSearch.Text, txtStockItemNameSearch.Text,
                null, null, General.GetNullableInteger(ddlStockClass.SelectedHard), null, null,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount,0, "");

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvStoreItem.DataSource = ds;
				gvStoreItem.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvStoreItem);
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

	protected void gvStoreItem_RowCommand(object sender, GridViewCommandEventArgs e)
	{
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


	}
    protected void gvStoreItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

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
		gvStoreItem.SelectedIndex = -1;
		gvStoreItem.EditIndex = -1;
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

    protected void gvStoreItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
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
}
