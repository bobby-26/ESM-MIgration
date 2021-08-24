using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsOtherCrew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOtherCrew.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOtherCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOtherCrewFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsOtherCrew.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('filter','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsOtherCrewGeneral.aspx?" + "');return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuOtherCrew1.AccessRights = this.ViewState;
            MenuOtherCrew1.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DTKEY"] = null;
                gvOtherCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOtherCrew1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentVesselAccountsOtherCrewFilter = null;
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
        gvOtherCrew.SelectedIndexes.Clear();
        gvOtherCrew.EditIndexes.Clear();
        gvOtherCrew.DataSource = null;
        gvOtherCrew.Rebind();
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDSIGNONDATE", "FLDSIGNONPORT", "FLDREFERENCE", "FLDSIGNOFFDATE", "FLDSIGNOFFPORT" };
        string[] alCaptions = { "S.No.", "Name", "Sign On", "Sign On Port", "Account Type", "Sign Off", "Sign Off Port" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            NameValueCollection nvc = Filter.CurrentVesselAccountsOtherCrewFilter;
            DataSet ds = PhoenixVesselAccountsOtherCrew.SearchOtherCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                 , nvc != null ? nvc.Get("txtName") : null
                                 , General.GetNullableDateTime(nvc != null ? nvc["txtSignOnStartDate"] : string.Empty)
                                 , General.GetNullableDateTime(nvc != null ? nvc["txtSignOnEndDate"] : string.Empty)
                                 , General.GetNullableDateTime(nvc != null ? nvc["txtSignOffStartDate"] : string.Empty)
                                 , General.GetNullableDateTime(nvc != null ? nvc["txtSignOffEndDate"] : string.Empty)
                                 , General.GetNullableInteger(nvc != null ? nvc["ddlSignOnPort"] : string.Empty)
                                 , General.GetNullableInteger(nvc != null ? nvc["ddlSignOffPort"] : string.Empty)
                                 , General.GetNullableInteger(nvc != null ? nvc["ddlAccountType"] : string.Empty)
                                 , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkSignedOff"] : "1")
                                 , sortexpression
                                 , sortdirection
                                 , (int)ViewState["PAGENUMBER"]
                                 , gvOtherCrew.PageSize
                                 , ref iRowCount
                                 , ref iTotalPageCount
                                 );
            General.SetPrintOptions("gvOtherCrew", "Supernumerary Sign On/Off", alCaptions, alColumns, ds);
            gvOtherCrew.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["DTKEY"] == null)
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            }
            gvOtherCrew.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
        string[] alColumns = { "FLDROWNUMBER", "FLDNAME", "FLDSIGNONDATE", "FLDSIGNONPORT", "FLDREFERENCE", "FLDSIGNOFFDATE", "FLDSIGNOFFPORT" };
        string[] alCaptions = { "S.No.", "Name", "Sign On", "Sign On Port", "Account Type", "Sign Off", "Sign Off Port" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentVesselAccountsOtherCrewFilter;
        DataSet ds = PhoenixVesselAccountsOtherCrew.SearchOtherCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                , nvc != null ? nvc.Get("txtName") : null
                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignOnStartDate"] : string.Empty)
                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignOnEndDate"] : string.Empty)
                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignOffStartDate"] : string.Empty)
                                , General.GetNullableDateTime(nvc != null ? nvc["txtSignOffEndDate"] : string.Empty)
                                , General.GetNullableInteger(nvc != null ? nvc["ddlSignOnPort"] : string.Empty)
                                , General.GetNullableInteger(nvc != null ? nvc["ddlSignOffPort"] : string.Empty)
                                , General.GetNullableInteger(nvc != null ? nvc["ddlAccountType"] : string.Empty)
                                , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkSignedOff"] : "1")
                                , sortexpression
                                , sortdirection
                                , (int)ViewState["PAGENUMBER"]
                                , General.ShowRecords(null)
                                , ref iRowCount
                                , ref iTotalPageCount
                                );
        General.ShowExcel("Supernumerary Sign On/Off", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["DTKEY"] = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOtherCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOtherCrew.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOtherCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                string fldkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                RadLabel signoffdate = (RadLabel)e.Item.FindControl("lblSignOffDate");
                RadLabel signoffport = (RadLabel)e.Item.FindControl("lblSignoffPort");
                if (!IsValidConfirm(signoffdate, signoffport))
                {
                    ucError.Visible = true;
                    return;
                }
                int i = PhoenixVesselAccountsOtherCrew.ConfirmOtherCrewSignOff(new Guid(fldkey), int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
                if (i > 0)
                    ucStatus.Text = "Sign Off confirmed";

                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string fldkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
                PhoenixVesselAccountsOtherCrew.DeleteOtherCrew(new Guid(fldkey), int.Parse(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()));
                ViewState["DTKEY"] = null;
                Rebind();
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
    protected void gvOtherCrew_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdApprove");
            RadLabel signoffdate = (RadLabel)e.Item.FindControl("lblSignOffDate");
            RadLabel status = (RadLabel)e.Item.FindControl("lblStatus");
            RadLabel signoffport = (RadLabel)e.Item.FindControl("lblSignoffPort");
            RadLabel Dtkey = (RadLabel)e.Item.FindControl("lblDTKey");
            if (ab != null)
            {
                ab.Visible = SessionUtil.CanAccess(this.ViewState, ab.CommandName);
                if (status.Text.ToString() != "1")
                {
                    ab.Visible = false;
                    db.Visible = false;
                }
                else if (signoffdate == null || String.IsNullOrEmpty(signoffdate.Text) || signoffport == null || String.IsNullOrEmpty(signoffport.Text))
                {
                    ab.Visible = false;
                }
                ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm sign-off ?')");
            }
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                eb.Attributes.Add("onclick", "openNewWindow('filter', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsOtherCrewGeneral.aspx?DTKEY=" + Dtkey.Text + "');return false;");
            }
        }
    }
    private bool IsValidConfirm(RadLabel date, RadLabel SeaPort)
    {

        ucError.HeaderMessage = "Please update the following information before confirm Sign Off";
        if (date == null)
        {
            ucError.ErrorMessage = "Sign Off Date should be updated.";
        }
        else if (!General.GetNullableDateTime(date.Text).HasValue)
        {
            ucError.ErrorMessage = "Sign Off Date should be updated.";
        }
        if (SeaPort == null)
        {
            ucError.ErrorMessage = "Sign Off Sea Port should be updated.";
        }
        else if (String.IsNullOrEmpty(SeaPort.Text))
        {
            ucError.ErrorMessage = "Sign Off Sea Port should be updated.";
        }
        return (!ucError.IsError);
    }
}
