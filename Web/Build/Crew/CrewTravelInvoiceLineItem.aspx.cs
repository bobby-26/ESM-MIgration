using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
public partial class CrewTravelInvoiceLineItem : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Save", "SAVE");
			MenuHopItem.AccessRights = this.ViewState;
			MenuHopItem.MenuList = toolbar.Show();
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
			//ViewState["ACTIVITYID"] = null;
			if (ViewState["TOTALPAGECOUNT"] == null)
				ViewState["TOTALPAGECOUNT"] = 1;
			if (Request.QueryString["agentid"] != null && Request.QueryString["agentid"] != string.Empty)
				ViewState["AGENTID"] = Request.QueryString["agentid"];
			if (Request.QueryString["currencyid"] != null && Request.QueryString["currencyid"] != string.Empty)
				ViewState["CURRENCYID"] = Request.QueryString["currencyid"];
			if (Request.QueryString["travelid"] != null && Request.QueryString["travelid"] != string.Empty)
				ViewState["TRAVELID"] = Request.QueryString["travelid"];
			if (Request.QueryString["travelid"] != null && Request.QueryString["travelid"] != string.Empty)
				ViewState["TRAVELID"] = Request.QueryString["travelid"];
			if (Request.QueryString["invoicecode"] != null && Request.QueryString["invoicecode"] != string.Empty)
				ViewState["INVOICECODE"] = Request.QueryString["invoicecode"];
			InvoiceEdit();
		}
		BindData();
		SetPageNavigator();
		txtVendorId.Attributes.Add("style", "visibility:hidden");
		txtCurrencyId.Attributes.Add("style", "visibility:hidden");
	}

	
	protected void gvHopList_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName.ToUpper().Equals("SORT"))
			return;


		SetPageNavigator();
	}
	protected void MenuHopItem_TabStripCommand(object sender, EventArgs e)
	{
		String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
		String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		try
		{
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
               
				StringBuilder strrouteid = new StringBuilder();
				foreach (GridViewRow gvr in gvHopList.Rows)
				{
					if (((CheckBox)gvr.FindControl("chkCheckedLineItem")).Checked == true)
					{

                        string routeid = ((Label)gvr.FindControl("lblRouteID")).Text;
                        strrouteid.Append(routeid);
                        strrouteid.Append(",");

					}
				}
                if (strrouteid.Length > 1)
				{
                    strrouteid.Remove(strrouteid.Length - 1, 1);
				}
                if (strrouteid.ToString() == "")
				{
					ucError.ErrorMessage = "Please Select Atleast One Line Item";
					ucError.Visible = true;
					return;
				}
               DataSet ds= PhoenixCrewTravelQuoteLine.InsertCrewTravelInvoiceLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					new Guid(ViewState["TRAVELID"].ToString()), Convert.ToInt32(ViewState["CURRENCYID"].ToString()), Convert.ToInt32(ViewState["AGENTID"].ToString()),
                    strrouteid.ToString(), new Guid(ViewState["INVOICECODE"].ToString()));
               foreach (DataRow dr in ds.Tables[0].Rows)
               {

                   PhoenixAccountsInvoice.InvoiceLineItemInsertAviation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), new Guid(dr["FLDDTKEY"].ToString()));

                   PhoenixAccountsPOStaging.OrderFormStagingInsertAviation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(dr["FLDDTKEY"].ToString()));
               }

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;
		DataSet ds = new DataSet();
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;

		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


		ds = PhoenixCrewTravelQuoteLine.CrewTravelRequestSearch(General.GetNullableInteger(ViewState["AGENTID"].ToString())
															  ,General.GetNullableInteger(ViewState["CURRENCYID"].ToString())
															   ,null
															   , sortexpression
															   , sortdirection
															   , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
															   , ref iRowCount
															   , ref iTotalPageCount);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvHopList.DataSource = ds;
				gvHopList.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvHopList);
			}
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		
		
	}



	protected void InvoiceEdit()
	{
		if (ViewState["INVOICECODE"] != null)
		{

			DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
			if (dsInvoice.Tables.Count > 0)
			{
				DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
				txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
				txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();
				txtVendorId.Text = drInvoice["FLDADDRESSCODE"].ToString();
				txtVendorCode.Text = drInvoice["FLDCODE"].ToString();
				txtVenderName.Text = drInvoice["FLDNAME"].ToString();
				txtCurrencyId.Text = drInvoice["FLDCURRENCY"].ToString();
				txtCurrencyName.Text = drInvoice["FLDCURRENCYCODE"].ToString();
				ViewState["SHORTNAME"] = drInvoice["FLDSHORTNAME"].ToString();
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

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;
		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
		BindData();
		SetPageNavigator();
	}

	

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		gvHopList.SelectedIndex = -1;
		gvHopList.EditIndex = -1;
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
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
		{
			ViewState["ROWCOUNT"] = 0;
			lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
		}
		else
		{
			lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
		}

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

	public bool IsValidLineItem()
	{
		ucError.HeaderMessage = "Please provide the following required information";
		return (!ucError.IsError);
	}


	public StateBag ReturnViewState()
	{
		return ViewState;
	}
}
