using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryStoreType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreTypeFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuGridStoreType.AccessRights = this.ViewState;  
            MenuGridStoreType.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvStoreType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Vessel", "VESSEL");
                toolbarmain.AddButton("Attachment", "ATTACHMENT");
                MenuStoreType.AccessRights = this.ViewState;  
                MenuStoreType.MenuList = toolbarmain.Show();

                ViewState["ISTREENODECLICK"] = false; 
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["STORETYPEID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["STORETYPEID"] != null)
                {
                    ViewState["STORETYPEID"] = Request.QueryString["STORETYPEID"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreTypeGeneral.aspx?STORETYPEID=" + Request.QueryString["STORETYPEID"].ToString();
                }
                MenuStoreType.SelectedMenuIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuStoreType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            PhoenixInventorySpareItem objinventorystockitem = new PhoenixInventorySpareItem();

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreTypeGeneral.aspx?STORETYPEID=";
            }
            else if (CommandName.ToUpper().Equals("VESSEL"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreTypeVesselMapping.aspx?STORETYPEID=";
            }
            else if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
            }

            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
            }
            else
            {
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["STORETYPEID"];
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSTORETYPENUMBER", "FLDSTORETYPENAME", "UNITNAME", "PREFERREDVENDOR" };
            string[] alCaptions = { "Number", "Name", "Unit", "Prefer vendor" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            DataSet ds = PhoenixCommonInventory.StoreTypeSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);



            Response.AddHeader("Content-Disposition", "attachment; filename=StoreType.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>StoreType</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGridStoreType_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvStoreType.ToString();
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string[] alColumns = { "FLDSTORETYPENUMBER", "FLDSTORETYPENAME", "UNITNAME", "PREFERREDVENDOR" };
            string[] alCaptions = { "Number", "Name", "Unit", "Prefer vendor" };
        

            DataSet ds;

            if (Filter.CurrentStockTypeFilterCriteria != null)
            {
                NameValueCollection nvc = Filter.CurrentStockTypeFilterCriteria;

                ds = PhoenixCommonInventory.StoreTypeSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   nvc.Get("txtNumber").ToString(), nvc.Get("txtName").ToString(), null, null,
                   General.GetNullableInteger(nvc.Get("txtVendorId").ToString()), null,
                   null, null, sortexpression, sortdirection,
                   gvStoreType.CurrentPageIndex + 1,
                   gvStoreType.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixCommonInventory.StoreTypeSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                     null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                     gvStoreType.CurrentPageIndex + 1,
                     gvStoreType.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount);
            }


            General.SetPrintOptions("gvStoreType", "Store Type", alCaptions, alColumns, ds);


            if (ds.Tables[0].Rows.Count > 0)
            {
                gvStoreType.DataSource = ds;
                gvStoreType.VirtualItemCount = iRowCount;

                if (ViewState["STORETYPEID"] == null)
                {
                    ViewState["STORETYPEID"] = ds.Tables[0].Rows[0][0].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                    gvStoreType.SelectedIndexes.Clear();
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                {
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inventory/InventoryStoreTypeGeneral.aspx?STORETYPEID=";
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"].ToString().Contains("CommonFileAttachment.aspx"))
                {
                    ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKEY"] + "&MOD=" + PhoenixModule.INVENTORY;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["STORETYPEID"];
                }

                if ((bool)ViewState["ISTREENODECLICK"] == false)
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ifMoreInfo.Attributes["src"] = "../Inventory/InventoryStoreTypeGeneral.aspx";
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

    private void DeleteStoreTypeItem(string storetypeitemid)
    {
        try
        {
            PhonixInventoryStoreType.DeleteStoreType(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(storetypeitemid));
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
            gvStoreType.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
       gvStoreType.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvStoreType.Items)
        {
            if (item.GetDataKeyValue("FLDSTORETYPEID").ToString() == ViewState["STORETYPEID"].ToString())
            {
                gvStoreType.SelectedIndexes.Add(item.ItemIndex);
                PhonixInventoryStoreType.StoretypeItemNumber = ((LinkButton)item.FindControl("lnkStoryTypeName")).Text;
                ViewState["DTKEY"] = ((RadLabel)item.FindControl("lbldtkey")).Text;
            }
        }
    }

    protected void gvStoreType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvStoreType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                    LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                    if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                    LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                    if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteStoreTypeItem(((RadLabel)e.Item.FindControl("lblStoreTypeId")).Text);
                ViewState["STORETYPEID"] = null;
                BindData();
                gvStoreType.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreType_SortCommand(object sender, GridSortCommandEventArgs e)
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
