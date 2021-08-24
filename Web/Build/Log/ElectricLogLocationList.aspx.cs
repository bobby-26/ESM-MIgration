using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ElectronicLog_ElectricLogLocationList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../Log/ElectricLogLocationList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvLocation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        //toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCounterLog.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

        toolbar.AddFontAwesomeButton("javascript:openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Log/ElectronicLogLocationRegister.aspx? '); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

        MenugvElogTank.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            gvLocation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvLocation.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixElog.ListLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                      , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                      , gvLocation.CurrentPageIndex + 1
                                      , gvLocation.PageSize
                                      , ref iRowCount
                                      , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Location", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenugvElogTank_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (CommandName.ToUpper().Equals("FIND"))
            //{
            //    ViewState["PAGENUMBER"] = 1;
            //}
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:parent.openNewWindow('Location','','" + Session["sitepath"] + "/Log/ElectronicLogLocationRegister.aspx');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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

        string[] alColumns = { "FLDCODE", "FLDNAME" };
        string[] alCaptions = { "Code", "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixElog.ListLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , gvLocation.CurrentPageIndex + 1
                                                , gvLocation.PageSize
                                                , ref iRowCount
                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvLocation", "Location", alCaptions, alColumns, ds);

        gvLocation.DataSource = ds;
        gvLocation.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvLocation_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lblLocationId = (RadLabel)e.Item.FindControl("lblLocationId");
                ViewState["LocId"] = lblLocationId.Text;
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //RadLabel lblLocationId = (RadLabel)e.Item.FindControl("lblLocationId");
                //PhoenixElog.DeleteLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblLocationId.Text));
                throw new MissingMethodException();

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //CheckBoxList ddlLocationTo = (CheckBoxList)e.Item.FindControl("ucLocation");
                //RadTextBox txtCode = (RadTextBox)e.Item.FindControl("txtCodeEdit");
                //RadTextBox txtName = (RadTextBox)e.Item.FindControl("txtNameEdit");
                //RadTextBox txtVariant = (RadTextBox)e.Item.FindControl("txtVariantEdit");
                //PhoenixElog.EditLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtVariant.Text, txtCode.Text, txtName.Text, PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["LocId"].ToString()));
                //string strWrk = string.Empty;
                //if (ddlLocationTo.Items != null)
                //{
                //    foreach (ListItem item in ddlLocationTo.Items)
                //    {
                //        if (item.Selected == true)
                //        {
                //            strWrk = strWrk + item.Value.ToString() + ",";
                //        }
                //    }
                //    strWrk = strWrk.TrimEnd(',');
                //}
                //PhoenixElog.InsertMappingLocation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["LocId"].ToString()), strWrk, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            gvLocation.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLocation_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (e.Item.IsInEditMode)
        {
        }
    }

    protected void ucState_Changed(object sender, EventArgs e)
    {

    }

    protected void gvLocation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

    protected void chkLocationList()
    {


    }
}