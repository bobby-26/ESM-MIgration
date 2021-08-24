using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Text;
using Telerik.Web.UI;

public partial class PurchaseGroupedStoreItemList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvStoreItem.Items)
        {
            Page.ClientScript.RegisterForEventValidation(gvStoreItem.UniqueID, "Edit$" + r.RowIndex.ToString());
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolSave = new PhoenixToolbar();
			toolSave.AddButton( "Send", "SAVE",ToolBarDirection.Right);
            //toolSave.AddButton("Deck", "DECK",ToolBarDirection.Left);
            //toolSave.AddButton("Engine", "ENGINE", ToolBarDirection.Left);
            //toolSave.AddButton("Galley", "GALLEY", ToolBarDirection.Left);
            //toolSave.AddButton("Safety", "SAFETY", ToolBarDirection.Left);

            menuSaveDetails.MenuList = toolSave.Show();
            menuSaveDetails.SelectedMenuIndex = 1;
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseGroupedStoreItemList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
			toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreItem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
			toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseGroupedStoreItemList.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
			MenuStoreItemControl.MenuList = toolbargrid.Show();
			
			if (!IsPostBack)
			{
                gvStoreItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["SESSIONID"] = Guid.NewGuid();

				ViewState["storeType"] = "0";
                if (Request.QueryString["StoreType"] != null)
                {
                    ViewState["storeType"] = Request.QueryString["StoreType"].ToString();
                }

                Bindvessel();
			}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void menuSaveDetails_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
			if (CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidSearch(ddlvessel.SelectedItem.Text))
				{
					ucError.Visible = true;
					return;
				}
				int storetype = int.Parse(ViewState["storeType"].ToString());
				PhoenixPurchaseGroupedStore.groupedStoresBulkupdate(int.Parse(ddlvessel.SelectedValue));
				Approvalrequest();
				ucStatus.Text = "Request sent.";

			}
            else if (CommandName.ToUpper().Equals("DECK"))
            {
                menuSaveDetails.SelectedMenuIndex = 1;
                ViewState["storeType"] = "1";
                gvStoreItem.CurrentPageIndex = 0;
                gvStoreItem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("ENGINE"))
            {
                menuSaveDetails.SelectedMenuIndex = 2;
                ViewState["storeType"] = "2";
                gvStoreItem.CurrentPageIndex = 0;
                gvStoreItem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("GALLEY"))
            {
                menuSaveDetails.SelectedMenuIndex = 3;
                ViewState["storeType"] = "3";
                gvStoreItem.CurrentPageIndex = 0;
                gvStoreItem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("SAFETY"))
            {
                menuSaveDetails.SelectedMenuIndex = 4;
                ViewState["storeType"] = "4";
                gvStoreItem.CurrentPageIndex = 0;
                gvStoreItem.Rebind();
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

		string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY" };
		string[] alCaptions = { "Number", "Name", "Unit", "Quantity" };

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		string storenumber = txtNumber.Text;
		string storename = txtName.Text;
		string sessionid = ViewState["SESSIONID"].ToString();
		int storetype = int.Parse(ViewState["storeType"].ToString());
		try
		{

			DataSet ds = PhoenixPurchaseGroupedStore.groupedStoreSearch(storetype, storenumber, storename
				, sortexpression, sortdirection, gvStoreItem.CurrentPageIndex + 1,
                gvStoreItem.PageSize,
                ref iRowCount,
				ref iTotalPageCount,
				General.GetNullableInteger(ddlvessel.SelectedValue)
				);
			General.SetPrintOptions("gvStoreItem", "Quarterly Stores", alCaptions, alColumns, ds);

            gvStoreItem.DataSource = ds;
            gvStoreItem.VirtualItemCount = iRowCount;
			
			ViewState["ROWCOUNT"] = iRowCount;
			ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
		}
		catch(Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void GroupedStoresInsert(string sessionid,string groupedstoreId,string storeid, string quantity,string type )
	{
		try
		{
			if (!IsValidSearch(ddlvessel.SelectedItem.Text))
			{
				ucError.Visible = true;
				return;
			}
			if (quantity == "" || Convert.ToDecimal(quantity) <= 0)
			{
				return;
			}

			PhoenixPurchaseGroupedStore.GroupedStoresInsert(new Guid(sessionid),new Guid(groupedstoreId), new Guid(storeid)
							, Convert.ToDecimal(quantity), int.Parse(type), int.Parse(ddlvessel.SelectedValue));


		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

	protected void MenuStoreItemControl_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
                gvStoreItem.Rebind();
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
	private void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = {"FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY"};
		string[] alCaptions = {"Number", "Name","Unit", "Quantity"};

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = General.ShowRecords(null);
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		string storenumber = txtNumber.Text;
		string storename = txtName.Text;
		string sessionid = ViewState["SESSIONID"].ToString();
		int storetype = int.Parse(ViewState["storeType"].ToString());
		try
		{
			DataSet ds = PhoenixPurchaseGroupedStore.groupedStoreSearch(storetype, storenumber, storename
				, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
				iRowCount,
				ref iRowCount,
				ref iTotalPageCount,
				General.GetNullableInteger(ddlvessel.SelectedValue)
				);

			DataTable dt = ds.Tables[0];

			General.ShowExcel("Quarterly Stores", dt, alColumns, alCaptions, null, null);

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void Approvalrequest()
	{
		DataTable dt;
		if (!IsValidSearch(ddlvessel.SelectedItem.Text))
		{
			ucError.Visible = true;
			return;
		}
		dt = PhoenixPurchaseGroupedStore.ApprovalReqInsert(int.Parse(ddlvessel.SelectedValue));
		DataRow dr = dt.Rows[0];
		StringBuilder mailbody = new StringBuilder();
        string toEmail = dr["FLDSUPTEMAIL"].ToString();
        string ccEmail = dr["FLDFLEETMANAGEREMAIL"].ToString();
        string vesselname = dr["FLDVESSELNAME"].ToString();
		string sessionid = dr["FLDSESSIONID"].ToString();
		string subject = vesselname + "- Quarterly Stores Approval'";
		string url = "<a href='" + Session["sitepath"] + "/" + "Purchase/PurchaseQuarterlyStoresApproval.aspx?sessionId=" + sessionid+"'>here.</a>";
		mailbody.Append("Dear Sir, <br><br> This is quarterly stores approval request for " + vesselname + ".");
		mailbody.Append("<br>To approve and create requisitions, please click ");
		mailbody.Append(url);
		mailbody.Append("<br><br>For more details , please login into <a href = '" + Session["sitepath"] + "'>" + Session["sitepath"] + "</a>");
		mailbody.Append("<br><br>Note : This is an automated email , please DO NOT reply to this email ID.");
		mailbody.Append("<br><br>Thanks and Regards,");
		mailbody.Append("<br>"+ dr["FLDSENDERNAME"].ToString());
		mailbody.Append("<br>Email :" + dr["FLDSENDEREMAIL"].ToString());

		PhoenixMail.SendMail(toEmail.Replace(";", ",").TrimEnd(','),
							ccEmail.Replace(";", ",").TrimEnd(','),
							null,
							subject,
							mailbody.ToString(),
							true,
							System.Net.Mail.MailPriority.Normal,
							"",
							null,
							null);
	}
	private bool IsValidSearch(string Vessel)
	{
		if (Vessel.Trim() == string.Empty || Vessel.Trim().ToUpper() == "DUMMY")
			ucError.ErrorMessage = "Please Select Vessel";
		return (!ucError.IsError);
	}

	private void Bindvessel()
	{
		DataSet ds = new DataSet();
		ds = PhoenixPurchaseGroupedStore.AccessVesselList();
		ddlvessel.DataSource = ds.Tables[0];
		ddlvessel.DataValueField = "FLDVESSELID";
		ddlvessel.DataTextField = "FLDVESSELNAME";
		ddlvessel.DataBind();

		if (Filter.GroupedStoreVesselSelected != null)
		{
			ddlvessel.SelectedValue = Filter.GroupedStoreVesselSelected;
		}
		else
		{
			Filter.GroupedStoreVesselSelected = ddlvessel.SelectedValue;
		}
	}

    protected void gvStoreItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStoreItem_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
        }
    }

    protected void gvStoreItem_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string sessionid = ViewState["SESSIONID"].ToString();
                GroupedStoresInsert
                    (
                        sessionid,
                        ((Label)e.Item.FindControl("lblGroupedstoreId")).Text,
                        ((Label)e.Item.FindControl("lblStoreItemId")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text,
                        ((Label)e.Item.FindControl("lblType")).Text
                   );
                gvStoreItem.SelectedIndexes.Clear();
                BindData();
                gvStoreItem.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreItem_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string sessionid = ViewState["SESSIONID"].ToString();
            GroupedStoresInsert
                (
                    sessionid,
                    ((Label)e.Item.FindControl("lblGroupedstoreId")).Text,
                    ((Label)e.Item.FindControl("lblStoreItemId")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text,
                    ((Label)e.Item.FindControl("lblType")).Text
               );
            gvStoreItem.SelectedIndexes.Clear();
            BindData();
            gvStoreItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreItem_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

    protected void ddlvessel_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        Filter.GroupedStoreVesselSelected = ddlvessel.SelectedValue;
        BindData();
        gvStoreItem.Rebind();
    }
}
