using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class RegistersFleet : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersFleet.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFleet')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','Registers/RegisterFleetFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersFleet.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersFleet.AccessRights = this.ViewState;
            MenuRegistersFleet.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvFleet.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFLEETCODE", "FLDFLEETDESCRIPTION"};
        string[] alCaptions = { "Code", "Description " };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentFleetFilter;

        ds = PhoenixRegistersFleet.FleetSearch((nvc != null ? nvc["txtFleetCode"] : string.Empty)
                                               , (nvc != null ? nvc["txtSearch"] : string.Empty)
                                               , General.GetNullableInteger(nvc != null ? nvc["ddlFleetType"] : string.Empty)
                                               , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                               , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                               , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : string.Empty)
                                               , General.GetNullableInteger(nvc != null ? nvc["ucFlag"] : string.Empty)
                                               , sortexpression
                                               , sortdirection
                                               , 1
                                               , iRowCount
                                               , ref iRowCount
                                               , ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Fleet", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersFleet_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvFleet.CurrentPageIndex = 0;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
             if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentFleetFilter = null;
                Rebind();
            } 
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void Rebind()
    {
        gvFleet.SelectedIndexes.Clear();
        gvFleet.EditIndexes.Clear();
        gvFleet.DataSource = null;
        gvFleet.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFLEETCODE", "FLDFLEETDESCRIPTION" };
        string[] alCaptions = { "Code", "Description " };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentFleetFilter;

        DataSet ds = PhoenixRegistersFleet.FleetSearch((nvc != null ? nvc["txtFleetCode"] : string.Empty)
                                                       , (nvc != null ? nvc["txtSearch"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlFleetType"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucPrincipal"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : string.Empty)
                                                       , General.GetNullableInteger(nvc != null ? nvc["ucFlag"] : string.Empty)
                                                       , sortexpression
                                                       , sortdirection
                                                       , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                       , gvFleet.PageSize
                                                       , ref iRowCount
                                                       , ref iTotalPageCount);

        General.SetPrintOptions("gvFleet", "Fleet", alCaptions, alColumns, ds);

        gvFleet.DataSource = ds;
        gvFleet.VirtualItemCount = iRowCount;
     
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFleet_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidFleet(((RadTextBox)e.Item.FindControl("txtFleetCodeAdd")).Text,
                  ((RadTextBox)e.Item.FindControl("txtFleetDescriptionAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertFleet(
                    ((RadTextBox)e.Item.FindControl("txtFleetCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtFleetDescriptionAdd")).Text
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtFleetCodeAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidFleet(((RadTextBox)e.Item.FindControl("txtFleetCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtFleetDescriptionEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateFleet(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblFleetidEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtFleetCodeEdit")).Text,
                     ((RadTextBox)e.Item.FindControl("txtFleetDescriptionEdit")).Text
                 );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["Fleetid"] = ((RadLabel)e.Item.FindControl("lblFleetid")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName.ToUpper().Equals("CREWFLEET"))
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                                 "Script", "openNewWindow('BookMarkScript', '', 'Registers/RegisterCrewFleet.aspx?fid=" + 
                                 ((RadLabel)e.Item.FindControl("lblFleetid")).Text + "&FleetName="+ 
                                 ((LinkButton)e.Item.FindControl("lnkFleetdescription")).Text + "');", true);
            }
            else if (e.CommandName.ToUpper().Equals("TECHFLEET"))
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                                 "Script", "openNewWindow('BookMarkScript', '', 'Registers/RegisterTechFleet.aspx?fid=" + 
                                 ((RadLabel)e.Item.FindControl("lblFleetid")).Text + "&FleetName=" +
                                 ((LinkButton)e.Item.FindControl("lnkFleetdescription")).Text + "');", true);
            }
            else if (e.CommandName.ToUpper().Equals("ACCOUNTSFLEET"))
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                                 "Script", "openNewWindow('BookMarkScript', '', 'Registers/RegisterAccountsFleet.aspx?fid=" + 
                                 ((RadLabel)e.Item.FindControl("lblFleetid")).Text + "&FleetName=" +
                                 ((LinkButton)e.Item.FindControl("lnkFleetdescription")).Text + "');", true);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFleet_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            RadLabel l = (RadLabel)e.Item.FindControl("lblFleetid");
            if (db != null)
            {
               db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    private void InsertFleet(string Fleetcode, string Fleetdescription)
    {
        PhoenixRegistersFleet.InsertFleet(Fleetcode, Fleetdescription);
    }

    private void UpdateFleet(int Fleetid, string Fleetcode, string Fleetdescription)
    {
        PhoenixRegistersFleet.UpdateFleet(Fleetid, Fleetcode, Fleetdescription);
        ucStatus.Text = "Fleet information updated successfully";
    }

    private bool IsValidFleet(string Fleetcode, string Fleetdescription)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvFleet;

        if (Fleetcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Fleetdescription.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }

    private void DeleteFleet(int Fleetcode)
    {
        PhoenixRegistersFleet.DeleteFleet(Fleetcode);
    }

    protected void gvFleet_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFleet.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFleet_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteFleet(Int32.Parse(ViewState["Fleetid"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
