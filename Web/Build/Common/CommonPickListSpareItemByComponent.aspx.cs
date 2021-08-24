using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class CommonPickListSpareItemByComponent : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbarmain = new PhoenixToolbar();
		toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
		MenuStockItem.MenuList = toolbarmain.Show();
       

        if (!IsPostBack)
		{
            ViewState["ISPOPUP"] = string.Empty;
            if ((Request.QueryString["txtnumber"] != null) && (Request.QueryString["txtnumber"] != ""))
                txtNumberSearch.Text = Request.QueryString["txtnumber"].ToString().TrimEnd("000.00.00.000".ToCharArray()).TrimEnd("__.__.__".ToCharArray());
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtStockItemNameSearch.Text = Request.QueryString["txtname"].ToString();
            if ((Request.QueryString["txtMakerRef"] != null) && (Request.QueryString["txtMakerRef"] != ""))
                txtMakerRef.Text = Request.QueryString["txtMakerRef"].ToString();

            if ((Request.QueryString["ispopup"] != null) && (Request.QueryString["ispopup"] != ""))
                ViewState["ISPOPUP"] = Request.QueryString["ispopup"].ToString();

            ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
            ViewState["COMPONENTID"] = "";
            if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
                ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
            BindTreeData();
            gvStockItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["framename"] != null)
            {
                ViewState["framename"] = Request.QueryString["framename"].ToString();
            }
        }     
	}


    protected void BindTreeData()
    {
        try
        {
            DataSet ds = new DataSet();

            int vesselid = -1;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixInventoryComponent.TreeComponentList(vesselid);
            tvwComponent.DataTextField = "FLDCOMPONENTNAME";
            tvwComponent.DataValueField = "FLDCOMPONENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTID";
            tvwComponent.RootText = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            tvwComponent.PopulateTree(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 	protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
			{
                ViewState["PAGENUMBER"] = 1;
				BindData();
                gvStockItem.Rebind();
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

            int vesselid = -1;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixCommonInventory.SpareItemByComponentSearch(ViewState["COMPONENTID"].ToString(),
                 txtNumberSearch.Text, txtStockItemNameSearch.Text, vesselid, General.GetNullableString(txtMakerRef.Text),
                sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);


            gvStockItem.DataSource = ds;
            gvStockItem.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
	}

    protected void gvStockItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStockItem.CurrentPageIndex + 1;
        BindData();
    }

    protected void tvwComponent_NodeClickEvent(object sender, EventArgs e)
    {
        try
        {
            txtNumberSearch.Text = "";
            txtStockItemNameSearch.Text = "";
            txtMakerRef.Text = "";
            ViewState["COMPONENTID"] = "";
            RadTreeNodeEventArgs args = (RadTreeNodeEventArgs)e;
            if (args.Node.Value.ToLower() != "_root")
            {
                string selectednode = args.Node.Value.ToString();
                string selectedvalue = args.Node.Text.ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTID"] = selectednode;
                BindData();
                gvStockItem.Rebind();

                //String script = String.Format("javascript:PaneResized();");
                //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStockItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lb = (RadLabel)e.Item.FindControl("lblIsInMarket");

            LinkButton lnkbtn = (LinkButton)e.Item.FindControl("lnkStockItemName");
            Int64 result = 0;

            if (Int64.TryParse(lb.Text, out result))
            {
                e.Item.ForeColor = (result == 0) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                lnkbtn.ForeColor = (result == 0) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            }
        }
    }

    protected void gvStockItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblStockItemNumber");
                nvc.Add(lbl.ID, lbl.Text);
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkStockItemName");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lblId = (RadLabel)e.Item.FindControl("lblStockItemId");
                nvc.Add(lblId.ID, lblId.Text);

            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnReloadList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                RadLabel lbl = (RadLabel)e.Item.FindControl("lblStockItemNumber");
                nvc.Set(nvc.GetKey(1), lbl.Text);
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkStockItemName");
                nvc.Set(nvc.GetKey(2), lb.Text.ToString());
                RadLabel lblId = (RadLabel)e.Item.FindControl("lblStockItemId");
                nvc.Set(nvc.GetKey(3), lblId.Text);
            }

            Filter.CurrentPickListSelection = nvc;
            if (!ViewState["ISPOPUP"].ToString().Equals(""))
            {
                string[] popup = ViewState["ISPOPUP"].ToString().Split(',');
                string refreshname = popup.Length > 1 ? popup[1] : string.Empty;
      
                Script = "top.closeTelerikWindow('" + popup[0] + "'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");";

                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //            "BookMarkScript", "top.closeTelerikWindow('" + popup[0] + "'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
            }
            else
               RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
        if (e.CommandName.ToUpper().Equals("PAGE"))
        {
            gvStockItem.Rebind();
        }

    }

    protected void gvStockItem_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
