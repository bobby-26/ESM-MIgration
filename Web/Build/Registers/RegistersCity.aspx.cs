using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;
public partial class RegistersCity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCity.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCity.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCity.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Registers/RegistersCity.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCITY");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
				ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDSTATENAME", "FLDCITYNAME", "FLDACTIVE" };
        string[] alCaptions = { "State", "City", "Active Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCity.CitySearch(
                txtSearch.Text,
                General.GetNullableInteger(ddlcountrylist.SelectedCountry) == null ? 0 : General.GetNullableInteger(ddlcountrylist.SelectedCountry),
                General.GetNullableInteger(ucState.SelectedState) == null ? null : General.GetNullableInteger(ucState.SelectedState),
                sortexpression, sortdirection,
                1,
                gvCity.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("City", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlcountrylist.SelectedCountry = "";
                ucState.SelectedState = "";
                txtSearch.Text = "";
                ucState.SelectedState = "";
                ucState.Country = ddlcountrylist.SelectedCountry;
                ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlcountrylist.SelectedCountry));
                ViewState["PAGENUMBER"] = 1;
                Rebind(); 
            }
            if (CommandName.ToUpper().Equals("ADDCITY"))
            {
                if (ddlcountrylist.SelectedCountry != "" && ddlcountrylist.SelectedCountry != "Dummy")
                {
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('City','','" + Session["sitepath"] + "/Registers/RegistersCityAdd.aspx?CountryID=" + ddlcountrylist.SelectedCountry + "');");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    ucError.ErrorMessage = "Country is Required";
                    ucError.Visible = true;
                }
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
        gvCity.SelectedIndexes.Clear();
        gvCity.EditIndexes.Clear();
        gvCity.DataSource = null;
        gvCity.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTATENAME", "FLDCITYNAME", "FLDACTIVE" };
        string[] alCaptions = {"State","City", "Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersCity.CitySearch(
                    txtSearch.Text,
                    General.GetNullableInteger(ddlcountrylist.SelectedCountry) == null ? 0 : General.GetNullableInteger(ddlcountrylist.SelectedCountry),
                    General.GetNullableInteger(ucState.SelectedState) == null ? null : General.GetNullableInteger(ucState.SelectedState),
                    sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvCity.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvCity", "City", alCaptions, alColumns, ds);

        gvCity.DataSource = ds;
        gvCity.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvCity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CITYCODE"] = ((RadLabel)e.Item.FindControl("lblCityCode")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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
    protected void gvCity_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem) 
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            RadLabel lblcitycode = (RadLabel)e.Item.FindControl("lblCityCode");
            RadLabel lblCountryId = (RadLabel)e.Item.FindControl("lblCountryId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCityName");
            if (lb != null) lb.Attributes.Add("onclick", "openNewWindow('City', '','" + Session["sitepath"] + "/Registers/RegistersCityAdd.aspx?CountryID=" + lblCountryId.Text +  "&CityID=" + lblcitycode.Text + "')");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('City', '','" + Session["sitepath"] + "/Registers/RegistersCityAdd.aspx?CountryID=" + lblCountryId.Text + "&CityID=" + lblcitycode.Text + "'); return false;");
            }
        }       
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ddlCountry_Changed(object sender, EventArgs e)
    {
        ucState.SelectedState = "";
        ucState.Country = ddlcountrylist.SelectedCountry;
        ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlcountrylist.SelectedCountry));
        Rebind();
    }

    protected void ucState_Changed(object sender, EventArgs e)
    {
        Rebind();
    }

    private void DeleteCity(int cityid)
    {
        PhoenixRegistersCity.DeleteCity(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, cityid);
    }

    protected void gvCity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCity.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCity_SortCommand(object sender, GridSortCommandEventArgs e)
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
            DeleteCity(Int32.Parse(ViewState["CITYCODE"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
