using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Generic;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselAccountsExtraMeals : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsExtraMeals.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvExtraMeals')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsExtraMeals.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','VesselAccounts/VesselAccountsExtraMealsList.aspx'); return false;", "Add", "<i class=\"fas fa-plus-circle\"></i>", "EXTRAMEALSLIST");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','VesselAccounts/VesselAccountsExtraMealsRateEdit.aspx'); return false;", "Default_Rate", "<i class=\"fas fa-tasks\"></i>", "EXTRAMEALS");
            MenuExtraMeals.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlMonth.DataBind();
                ddlYear.DataBind();
                ddlMonth.SelectedMonth = DateTime.Today.Month.ToString();
                ddlYear.SelectedYear = DateTime.Today.Year;
                ddlAccountType.SelectedValue = "-1";
                gvExtraMeals.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvExtraMeals.SelectedIndexes.Clear();
        gvExtraMeals.EditIndexes.Clear();
        gvExtraMeals.DataSource = null;
        gvExtraMeals.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void gvExtraMeals_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvExtraMeals.CurrentPageIndex + 1;
            BindData();
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

        string[] alColumns = { "FLDFROMDATE", "FLDTODATE", "FLDACCOUNTTYPE", "FLDNOOFMANDAYS", "FLDSERVERDTO", "FLDRATE" };
        string[] alCaptions = { "FromDate", "ToDate", "Account Type", "Number of ManDays", "Served To", "Rate", "Total Amount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        int Month = int.Parse(ddlMonth.SelectedMonth.ToString() == "" ? DateTime.Today.Month.ToString() : ddlMonth.SelectedMonth);
        int year = int.Parse(ddlYear.SelectedYear.ToString() == "" ? DateTime.Today.Year.ToString() : ddlYear.SelectedYear.ToString());
        DataSet ds = PhoenixVesselAccountsExtraMeals.SearchExtraMeals(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                             , General.GetNullableInteger(ddlAccountType.SelectedValue)
                             , Month, year, General.GetNullableString(sortexpression), General.GetNullableInteger(sortdirection.ToString())
                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                             , gvExtraMeals.PageSize, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvExtraMeals", "Extra Meals", alCaptions, alColumns, ds);
        gvExtraMeals.DataSource = ds;
        gvExtraMeals.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void MenuExtraMeals_TabStripCommand(object sender, EventArgs e)
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

                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDFROMDATE", "FLDTODATE", "FLDACCOUNTTYPE", "FLDNOOFMANDAYS", "FLDSERVERDTO", "FLDRATE" };
                string[] alCaptions = { "FromDate", "ToDate", "Account Type", "Number of ManDays", "Served To", "Rate" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixVesselAccountsExtraMeals.SearchExtraMeals(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                              , General.GetNullableInteger(ddlAccountType.SelectedValue.ToString())
                              , Int32.Parse(ddlMonth.SelectedMonth.ToString())
                              , Int32.Parse(ddlYear.SelectedYear.ToString())
                              , General.GetNullableString(sortexpression), General.GetNullableInteger(sortdirection.ToString())
                              , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                              , iRowCount, ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("Extra Meals", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvExtraMeals_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName.ToUpper().Equals("SAVE"))
            //{
            //    Guid id = new Guid(((RadLabel)e.Item.FindControl("lblid")).Text);
            //    string script = "Openpopup('Filter','','../VesselAccounts/VesselAccountsExtraMealsEdit.aspx?purpose=EXTRAMEALSEDIT&extramealsid=" + id + "');";
            //    Rebind();
            //}
            //            else 
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblid")).Text);
                PhoenixVesselAccountsExtraMeals.DeleteExtraMeals(id, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
    protected void gvExtraMeals_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel id = ((RadLabel)e.Item.FindControl("lblid"));
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (id != null)
                    eb.Attributes.Add("onclick", "openNewWindow('Filter', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsExtraMealsEdit.aspx?purpose=EXTRAMEALSEDIT&extramealsid=" + id.Text + "');return false;");
            }

            LinkButton lbts = (LinkButton)e.Item.FindControl("lnkServedTo");
            UserControlToolTip ucts = (UserControlToolTip)e.Item.FindControl("ucServedTo");
            ucts.Position = ToolTipPosition.TopCenter;
            ucts.TargetControlId = lbts.ClientID;
        }
    }
}
