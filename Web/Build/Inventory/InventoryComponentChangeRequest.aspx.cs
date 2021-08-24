using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryComponentChangeRequest : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentChangeRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentFilter.aspx?go=ChangeRequest", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentChangeRequest.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentChangeRequestGeneral.aspx?Type=New&Mode=New&tv=1", "Add", "<i class=\"fas fa-plus\"></i>", "ADD");
            MenuComponentChangeRequest.AccessRights = this.ViewState;
            MenuComponentChangeRequest.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPONENTID"] = null;

                gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDSERIALNUMBER", "FLDCLASSCODE", "FLDREQTYPENAME", "FLDREMARKS" };
            string[] alCaptions = { "Number", "Name", "Serial Number", "Class Code", "Request Type", "Remarks" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentComponentFilterCriteria;

            DataSet ds = PhoenixInventoryComponentChangeRequest.ChangeRequestComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                           , nvc != null ? nvc.Get("txtNumber").ToString() : null
                                                                                           , nvc != null ? nvc.Get("txtName").ToString() : null
                                                                                           , null
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("txtVendorId").ToString() : string.Empty)
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("txtMakerid").ToString() : string.Empty)
                                                                                           , null
                                                                                           , General.GetNullableInteger(nvc != null ? nvc.Get("txtComponentClassId").ToString() : string.Empty)
                                                                                           , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("chkCrtitcal").ToString() : string.Empty)
                                                                                           , nvc != null ? nvc.Get("txtClassCode").ToString() : null
                                                                                           , sortexpression
                                                                                           , sortdirection
                                                                                           , gvComponent.CurrentPageIndex + 1
                                                                                           , gvComponent.PageSize
                                                                                           , ref iRowCount
                                                                                           , ref iTotalPageCount);

            General.ShowExcel("Component - Change Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentChangeRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {

            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentComponentFilterCriteria = null;
                gvComponent.Rebind();
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
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDSERIALNUMBER", "FLDCLASSCODE", "FLDREQTYPENAME", "FLDREMARKS" };
            string[] alCaptions = { "Number", "Name", "Serial Number", "Class Code", "Request Type", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            NameValueCollection nvc = Filter.CurrentComponentFilterCriteria;

            ds = PhoenixInventoryComponentChangeRequest.ChangeRequestComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                    , nvc != null ? nvc.Get("txtNumber").ToString() : null
                                                                                    , nvc != null ? nvc.Get("txtName").ToString() : null
                                                                                    , null
                                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtVendorId").ToString() : string.Empty)
                                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtMakerid").ToString() : string.Empty)
                                                                                    , null
                                                                                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtComponentClassId").ToString() : string.Empty)
                                                                                    , (byte?)General.GetNullableInteger(nvc != null ? nvc.Get("chkCrtitcal").ToString() : string.Empty)
                                                                                    , nvc != null ? nvc.Get("txtClassCode").ToString() : null
                                                                                    , sortexpression
                                                                                    , sortdirection
                                                                                    , gvComponent.CurrentPageIndex + 1
                                                                                    , gvComponent.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);

            General.SetPrintOptions("gvComponent", "Component Change Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvComponent.DataSource = ds;
                gvComponent.VirtualItemCount = iRowCount;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvComponent.DataSource = "";
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //private void SetRowSelection()
    //{
    //    gvComponent.SelectedIndex = -1;
    //    if (ViewState["COMPONENTID"] != null)
    //    {
    //        for (int i = 0; i < gvComponent.Rows.Count; i++)
    //        {
    //            if (gvComponent.DataKeys[i].Value.ToString().Equals(ViewState["COMPONENTID"].ToString()))
    //            {
    //                gvComponent.SelectedIndex = i;
    //                PhoenixInventoryComponentChangeRequest.ComponentNumber = ((LinkButton)gvComponent.Rows[i].FindControl("lnkStockItemName")).Text;
    //                ViewState["DTKEY"] = ((Label)gvComponent.Rows[gvComponent.SelectedIndex].FindControl("lbldtkey")).Text;
    //            }
    //        }
    //    }
    //}


    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) db.Visible = false;
                Image img = (Image)e.Item.FindControl("imgFlag");
                if (drv["FLDISCRITICAL"].ToString() == "1")
                {
                    img.Visible = true;
                    img.ImageUrl = Session["images"] + "/red-symbol.png";
                    img.ToolTip = "Critical Component";
                }
            }
            LinkButton ap = (LinkButton)e.Item.FindControl("cmdApprove");
            if (ap != null)
            {
                ap.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Are you sure want to approve this record?'); return false;");
                ap.Visible = SessionUtil.CanAccess(this.ViewState, ap.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) ap.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "APPROVE")
            {
                string dtkey = e.CommandArgument.ToString();

                PhoenixInventoryComponentChangeRequest.ApproveChangeRequestComponent(new Guid(dtkey), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ViewState["COMPONENTID"] = null;
            }

            if(e.CommandName.ToUpper() == "DELETE")
            {
                string componetnid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                PhoenixInventoryComponentChangeRequest.DeleteChangeRequestComponent(new Guid(componetnid), PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(dtkey));
                ViewState["COMPONENTID"] = null;
            }

            if(e.CommandName.ToUpper() == "SELECT")
            {

                string compid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                string parentid = ((RadLabel)e.Item.FindControl("lblParentId")).Text;
                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                string reqtype = ((RadLabel)e.Item.FindControl("lblReqType")).Text;
                if (reqtype == "0")
                    Response.Redirect("../Inventory/InventoryComponentChangeRequestGeneral.aspx?tv=1&Type=New&Mode=Edit&parent=" + parentid + "&COMPONENTID=" + compid, false);
                else
                    Response.Redirect("../Inventory/InventoryComponentChangeRequestGeneral.aspx?tv=1&Type=Edit&Mode=Edit&COMPONENTID=" + compid, false);
            }
            gvComponent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
