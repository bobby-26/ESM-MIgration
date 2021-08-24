using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class CommonPickListStockItemWithLocation : PhoenixBasePage
{
    private void Page_Init(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "MaskedEditFix", "(function(){try{var n=Sys.Extended.UI.MaskedEditBehavior.prototype,t=n._ExecuteNav;n._ExecuteNav=function(n){var i=n.type;i==\"keydown\"&&(n.type=\"keypress\"),t.apply(this,arguments),n.type=i}}catch(i){return}})();", true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (Request.QueryString["mode"] == "multi")
            toolbarmain.AddButton("Save", "ADD", ToolBarDirection.Right);
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuStockItem.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            txtNumberSearch.Text = Request.QueryString["stockno"];
            txtComponentNo.Text = Request.QueryString["compno"];
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
                gvStockItem.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                NameValueCollection nvc = new NameValueCollection();

                string strItemNumber = string.Empty;
                string strItemName = string.Empty;
                string strItemId = string.Empty;
                string strLocation = string.Empty;
                string strSpareItemLocation = string.Empty;

                foreach (GridDataItem gr in gvStockItem.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        strItemNumber += ((RadLabel)gr.FindControl("lblStockItemNumber")).Text + ",";
                        strItemName += ((LinkButton)gr.FindControl("lnkStockItemName")).Text + ",";
                        strItemId += ((RadLabel)gr.FindControl("lblStockItemId")).Text + ",";
                        strLocation += ((RadLabel)gr.FindControl("lblLocationId")).Text + ",";
                        strSpareItemLocation += ((RadLabel)gr.FindControl("lblSpareItemLocationId")).Text + ",";
                    }
                }
                if (strItemId == string.Empty)
                {
                    ucError.ErrorMessage = "Select atleast one or more parts";
                    ucError.Visible = true;
                    return;
                }
                nvc.Add("lblStockItemNumber", strItemNumber.TrimEnd(','));
                nvc.Add("lnkStockItemName", strItemName.TrimEnd(','));
                nvc.Add("lblStockItemId", strItemId.TrimEnd(','));
                nvc.Add("lblLocationId", strLocation.TrimEnd(','));
                nvc.Add("lblSpareItemLocationId", strSpareItemLocation.TrimEnd(','));
                Filter.CurrentPickListSelection = nvc;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"BookMarkScript", "refreshpr(); fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
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

            int vesselid;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixInventoryComponentSpareItem.ComponentStockItemComponentSearch(Request.QueryString["componentid"]
                                                     , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                     , General.GetNullableString(txtNumberSearch.TextWithLiterals.TrimEnd('.'))
                                                     , General.GetNullableString(txtStockItemNameSearch.Text)
                                                     ,  General.GetNullableString(txtComponentName.Text)
                                                     , General.GetNullableString(txtComponentNo.TextWithLiterals.TrimEnd('.'))
                                                     , sortexpression
                                                     , sortdirection
                                                     , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                     , gvStockItem.PageSize
                                                     , ref iRowCount
                                                     , ref iTotalPageCount);

            gvStockItem.DataSource = ds;
            gvStockItem.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Request.QueryString["mode"] == null || Request.QueryString["mode"] != "multi")
                    gvStockItem.Columns[1].Visible = false;
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

    protected void gvStockItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc = new NameValueCollection();

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
            return;
        }

        if (Request.QueryString["mode"] == "custom" || Request.QueryString["mode"] == "multi")
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
            RadLabel lblLocationId = (RadLabel)e.Item.FindControl("lblLocationId");
            nvc.Add(lblLocationId.ID, lblLocationId.Text);
            RadLabel lblSpareItemLocationId = (RadLabel)e.Item.FindControl("lblSpareItemLocationId");
            nvc.Add(lblSpareItemLocationId.ID, lblSpareItemLocationId.Text);
        }
        else
        {

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
        Script += "</script>" + "\n";

        nvc = Filter.CurrentPickListSelection;

        RadLabel lbl = (RadLabel)e.Item.FindControl("lblStockItemNumber");
        nvc.Set(nvc.GetKey(1), lbl.Text);
        LinkButton lb = (LinkButton)e.Item.FindControl("lnkStockItemName");
        nvc.Set(nvc.GetKey(2), lb.Text.ToString());
        RadLabel lblId = (RadLabel)e.Item.FindControl("lblStockItemId");
        nvc.Set(nvc.GetKey(3), lblId.Text);
        RadLabel lblLocationId = (RadLabel)e.Item.FindControl("lblLocationId");
        nvc.Set(nvc.GetKey(3), lblLocationId.Text);
        RadLabel lblSpareItemLocationId = (RadLabel)e.Item.FindControl("lblSpareItemLocationId");
        nvc.Set(nvc.GetKey(3), lblSpareItemLocationId.Text);
    }      
        Filter.CurrentPickListSelection = nvc;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
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

    protected void Rebind()
    {
        gvStockItem.SelectedIndexes.Clear();
        gvStockItem.EditIndexes.Clear();
        gvStockItem.DataSource = null;
        gvStockItem.Rebind();
    }

    protected void gvStockItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStockItem.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvStockItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Image img = (Image)e.Item.FindControl("imgFlag");

                if (drv["MINQTYFLAGE"].ToString() == "1")
                {
                    img.Visible = true;
                    img.ToolTip = "ROB is less than Minimum Level";
                }
                else
                    img.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}