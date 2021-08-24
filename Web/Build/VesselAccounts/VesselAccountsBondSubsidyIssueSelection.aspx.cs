using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;


public partial class VesselAccountsBondSubsidyIssueSelection :  PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvStoreItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvStoreItem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
	protected void Page_Load(object sender, EventArgs e)
	{
        SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH");
        MenuStockItem.AccessRights = this.ViewState;
		MenuStockItem.MenuList = toolbarmain.Show();
		MenuStockItem.SetTrigger(pnlStockItem);      

		if (!IsPostBack)
		{
            if ((Request.QueryString["txtnumber"] != null) && (Request.QueryString["txtnumber"] != ""))
                txtNumberSearch.Text = Request.QueryString["txtnumber"].ToString().TrimEnd("00.00.00".ToCharArray()).TrimEnd("__.__.__".ToCharArray());
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtStockItemNameSearch.Text = Request.QueryString["txtname"].ToString();
            ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
            ddlStockClass.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)PhoenixHardTypeCode.STORETYPE);
            if (Request.QueryString["storeclass"] != null)
            {
                ddlStockClass.SelectedHard = Request.QueryString["storeclass"];
                ddlStockClass.Enabled = false;
            }
            if (Request.QueryString["storetype"] != null)
            {
                ddlStockClass.SelectedHard = Request.QueryString["storetype"];               
            }
            if (Request.QueryString["accountfor"] != null && Request.QueryString["accountfor"].ToString() == "")
            {

                ucError.ErrorMessage = "Select Staff name for issue Bonded Store item";
                ucError.Visible = true;                
            }

            ViewState["ACCOUNTFOR"] = Request.QueryString["accountfor"];

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
			
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsOrderForm.SearchOrderFormStoreItem(null,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtNumberSearch.Text, txtStockItemNameSearch.Text,
                null, null, General.GetNullableInteger(ddlStockClass.SelectedHard), 0, sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);

            if (dt.Rows.Count > 0)
			{
                gvStoreItem.DataSource = dt;
				gvStoreItem.DataBind();               
			}
			else
			{                
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

    protected void gvStoreItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            UserControlMaskNumber qty = ((UserControlMaskNumber)e.Row.FindControl("txtQuantity"));
            if (qty == null && SessionUtil.CanAccess(this.ViewState, ed.CommandName))
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvStoreItem, "Edit$" + e.Row.RowIndex.ToString(), false);
            SetKeyDownScroll(sender, e);
            Int64 result = 0;
            if (Int64.TryParse(drv["FLDISINMARKET"].ToString(), out result))
            {
                e.Row.ForeColor = (result == 0) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }
            TextBox txtremarks = (TextBox)e.Row.FindControl("txtRemarks");
            if(txtremarks!=null)
            {
                if (ViewState["ACCOUNTFOR"].ToString() == "-1" || ViewState["ACCOUNTFOR"].ToString() == "-2")
                    txtremarks.Attributes.Add("class", "input_mandatory");
                else
                    txtremarks.Attributes.Add("class", "input");
            }
            UserControlMaskNumber cans = ((UserControlMaskNumber)e.Row.FindControl("txtNoofCans"));
            if (cans != null && (ViewState["ACCOUNTFOR"].ToString() == "-1" || ViewState["ACCOUNTFOR"].ToString() == "-2"))
            {
                cans.Enabled = false;
                cans.CssClass = "readonlytextbox";
            }
            else if(cans != null)
            {
                cans.Enabled = true;
                cans.CssClass = "input";
            }
        }
	}
    protected void gvStoreItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvStoreItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantity")).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantity")).Text;
            string storeitem = _gridView.DataKeys[nCurrentRow].Value.ToString();
            string date = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDate")).Text;
            string  remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarks")).Text;
            string rob = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRob")).Text;
            string cans = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtNoofCans")).Text;
            if (IsValidIssue(date, quantity, rob,remarks))
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("StoreItemId", storeitem);
                nvc.Add("Quantity", quantity);
                nvc.Add("IssueDate", date);
                nvc.Add("NoofCans", cans);
                nvc.Add("Remarks", remarks);
                Filter.CurrentPickListSelection = nvc;
                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            else if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(quantity))
            {
                ucError.Visible = true;
                return;
            }            
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
    private bool IsValidIssue(string date, string quantity, string rob,string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        decimal resultDecimal;
        if (Request.QueryString["accountfor"] == null || Request.QueryString["accountfor"].ToString() == "")
        {

            ucError.ErrorMessage = "Select Staff name for issue Bonded Store item";
        }
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Sold Quantity is required.";
        }
        else if (decimal.Parse(quantity) <= 0)
        {
            ucError.ErrorMessage = "Sold Quantity should not be zero or negative";
        }
        if (General.GetNullableDecimal(quantity).HasValue && !General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Issue Date is required.";
        }
        else if (General.GetNullableDecimal(quantity).HasValue && DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        if (General.GetNullableDecimal(quantity).HasValue && decimal.TryParse(quantity, out resultDecimal) && resultDecimal > decimal.Parse(rob == string.Empty ? "0" : rob))
            ucError.ErrorMessage = "Please check your stock. Quantity cannot be greater than ROB.";

        if (ViewState["ACCOUNTFOR"].ToString() == "-1" || ViewState["ACCOUNTFOR"].ToString() == "-2")
        {
            if (string.IsNullOrEmpty(remarks))
                ucError.ErrorMessage = "Remarks is Required.";
        }
        return (!ucError.IsError);
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }
}
