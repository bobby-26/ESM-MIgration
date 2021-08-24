using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersCrewStandardAirfare : PhoenixBasePage
{
    string city = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Registers/RegistersCrewStandardAirfare.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Registers/RegistersCrewStandardAirfare.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuCity.AccessRights = this.ViewState;
            MenuCity.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCrewStandardAirfare.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewAirfare')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuCrewAirfare.AccessRights = this.ViewState;
            MenuCrewAirfare.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERCITY"] = 1;
                ViewState["SORTEXPRESSIONCITY"] = null;
                ViewState["SORTDIRECTIONCITY"] = null;
                ViewState["CURRENTCITYID"] = null;
                city = " ";

                gvCity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvCrewAirfare.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCity_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBERCITY"] = 1;
                ViewState["CURRENTCITYID"] = null;
                gvCity.CurrentPageIndex = 0;
                BindCity();
                gvCity.Rebind();

                ViewState["PAGENUMBER"] = 1;
                gvCrewAirfare.CurrentPageIndex = 0;
                BindData();
                gvCrewAirfare.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlCountry.SelectedCountry = "";
                ddlState.SelectedState = "";
                txtCitySearch.Text = "";
                ViewState["CURRENTCITYID"] = null;
                ViewState["PAGENUMBERCITY"] = 1;
                gvCity.CurrentPageIndex = 0;
                BindCity();
                gvCity.Rebind();

                ViewState["PAGENUMBER"] = 1;
                gvCrewAirfare.CurrentPageIndex = 0;
                BindData();
                gvCrewAirfare.Rebind();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCrewAirfare_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
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

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDFROMCITYNAME", "FLDTOCITYNAME", "FLDAIRFARE" };
            string[] alCaptions = { "From", "To", "Airfare" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (General.GetNullableInteger(ddlCountry.SelectedCountry) == null && General.GetNullableInteger(ddlState.SelectedState) == null)
            {
                ds = PhoenixRegistersCrewStandardAirfare.CrewStandardAirfareSearch(General.GetNullableInteger(ViewState["CURRENTCITYID"] != null ? ViewState["CURRENTCITYID"].ToString() : "")
                          , null
                          , sortexpression, sortdirection,
                          (int)ViewState["PAGENUMBER"],
                          gvCrewAirfare.PageSize,
                          ref iRowCount,
                          ref iTotalPageCount);
            }
            else
            {
                ds = PhoenixRegistersCrewStandardAirfare.CrewStandardAirfareSearch(General.GetNullableInteger(ViewState["CURRENTCITYID"] != null ? ViewState["CURRENTCITYID"].ToString() : "0")
                          , null
                          , sortexpression, sortdirection,
                          (int)ViewState["PAGENUMBER"],
                          gvCrewAirfare.PageSize,
                          ref iRowCount,
                          ref iTotalPageCount);
            }

            if (ds.Tables.Count > 0)
                General.ShowExcel("Crew Standard Airfare", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCity()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSIONCITY"] == null) ? null : (ViewState["SORTEXPRESSIONCITY"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTIONCITY"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCITY"].ToString());

        DataSet ds = PhoenixRegistersCity.CitySearch(General.GetNullableString(txtCitySearch.Text) != null ? txtCitySearch.Text : city
            , General.GetNullableInteger(ddlCountry.SelectedCountry)
            , General.GetNullableInteger(ddlState.SelectedState)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBERCITY"]
            , gvCity.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        gvCity.DataSource = ds;
        gvCity.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["CURRENTCITYID"] == null)
                ViewState["CURRENTCITYID"] = ds.Tables[0].Rows[0]["FLDCITYID"].ToString();

            SetRowSelection();
        }
        else
        {
            ViewState["CURRENTCITYID"] = null;
        }

        ViewState["ROWCOUNTCITY"] = iRowCount;
        ViewState["TOTALPAGECOUNTCITY"] = iTotalPageCount;
    }

    private void SetRowSelection()
    {
        try
        {
            foreach (GridDataItem item in gvCity.Items)
            {
                if (item.GetDataKeyValue("FLDCITYID").ToString() == ViewState["CURRENTCITYID"].ToString())
                {
                    gvCity.SelectedIndexes.Clear();

                    gvCity.SelectedIndexes.Add(item.ItemIndex);
                }
            }

        }

        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvCity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERCITY"] = ViewState["PAGENUMBERCITY"] != null ? ViewState["PAGENUMBERCITY"] : gvCity.CurrentPageIndex + 1;

        BindCity();
    }

    protected void gvCity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblCityId = (RadLabel)e.Item.FindControl("lblCityId");
                ViewState["CURRENTCITYID"] = lblCityId.Text;

                BindData();
                gvCrewAirfare.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERCITY"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCity_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null)
            {
                sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);
            }

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null)
            {
                cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFROMCITYNAME", "FLDTOCITYNAME", "FLDAIRFARE" };
        string[] alCaptions = { "From", "To", "Airfare" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();

        if (General.GetNullableInteger(ddlCountry.SelectedCountry) == null && General.GetNullableInteger(ddlState.SelectedState) == null)
        {
            ds = PhoenixRegistersCrewStandardAirfare.CrewStandardAirfareSearch(General.GetNullableInteger(ViewState["CURRENTCITYID"] != null ? ViewState["CURRENTCITYID"].ToString() : "")
                      , null
                      , sortexpression, sortdirection,
                      (int)ViewState["PAGENUMBER"],
                      gvCrewAirfare.PageSize,
                      ref iRowCount,
                      ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixRegistersCrewStandardAirfare.CrewStandardAirfareSearch(General.GetNullableInteger(ViewState["CURRENTCITYID"] != null ? ViewState["CURRENTCITYID"].ToString() : "0")
                      , null
                      , sortexpression, sortdirection,
                      (int)ViewState["PAGENUMBER"],
                      gvCrewAirfare.PageSize,
                      ref iRowCount,
                      ref iTotalPageCount);
        }

        General.SetPrintOptions("gvCrewAirfare", "Crew Standard Airfare", alCaptions, alColumns, ds);

        gvCrewAirfare.DataSource = ds;
        gvCrewAirfare.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvCrewAirfare_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewAirfare.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvCrewAirfare_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string fromcityidadd = ViewState["CURRENTCITYID"] == null ? "" : ViewState["CURRENTCITYID"].ToString();
                string tocityidadd = ((UserControlMultiColumnCity)e.Item.FindControl("txtToCityIdAdd")).SelectedValue;
                string airfareadd = ((UserControlMaskNumber)e.Item.FindControl("txtAirfareAdd")).Text;

                InsertCrewAirfare(fromcityidadd, tocityidadd, airfareadd);
                BindData();
                gvCrewAirfare.Rebind();
            }
            if (e.CommandName == "Page")
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

    protected void gvCrewAirfare_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null)
            {
                sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);
            }

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null)
            {
                cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }
        }
        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlMultiColumnCity ucToCity = (UserControlMultiColumnCity)e.Item.FindControl("txtToCityIdEdit");
            if (ucToCity != null)
            {
                ucToCity.SelectedValue = drv["FLDTOCITYID"].ToString();
                ucToCity.Text = drv["FLDTOCITYNAME"].ToString();
            }
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

    protected void gvCrewAirfare_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string airfareid = ((RadLabel)e.Item.FindControl("lblCrewAirfareId")).Text;
            DeleteCrewAirfare(airfareid);
            BindData();
            gvCrewAirfare.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewAirfare_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string airfareid = ((RadLabel)e.Item.FindControl("lblCrewAirfareIdEdit")).Text;
            string fromcityidedit = ((RadLabel)e.Item.FindControl("lblFromCityIdEdit")).Text;
            string tocityidedit = ((UserControlMultiColumnCity)e.Item.FindControl("txtToCityIdEdit")).SelectedValue;
            string airfareedit = ((UserControlMaskNumber)e.Item.FindControl("txtAirfareEdit")).Text;

            if (!IsValidCrewAirfare(fromcityidedit, tocityidedit, airfareedit))
            {
                ucError.Visible = true;
                return;
            }

            UpdateCrewAirfare(airfareid, fromcityidedit, tocityidedit, airfareedit);
            ucStatus.Text = "Information Updated";

            BindData();
            gvCrewAirfare.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void InsertCrewAirfare(string fromcityid, string tocityid, string airfare)
    {
        if (!IsValidCrewAirfare(fromcityid, tocityid, airfare))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixRegistersCrewStandardAirfare.InsertCrewStandardAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                         , Convert.ToInt16(fromcityid)
                                                         , Convert.ToInt16(tocityid)
                                                         , Convert.ToDecimal(airfare));
    }

    private void UpdateCrewAirfare(string airfareid, string fromcityid, string tocityid, string airfare)
    {
        PhoenixRegistersCrewStandardAirfare.UpdateCrewStandardAirfare(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , new Guid(airfareid)
                                                 , Convert.ToInt16(fromcityid)
                                                 , Convert.ToInt16(tocityid)
                                                 , Convert.ToDecimal(airfare)
                                                 );
    }

    private void DeleteCrewAirfare(string airfareid)
    {
        PhoenixRegistersCrewStandardAirfare.DeleteCrewStandardAirfare(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(airfareid));
    }

    private bool IsValidCrewAirfare(string fromcityid, string tocityid, string airfare)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(fromcityid) == null)
            ucError.ErrorMessage = "From city is required.";

        if (General.GetNullableInteger(tocityid) == null)
            ucError.ErrorMessage = "To city is required.";

        if (General.GetNullableInteger(fromcityid) != null && General.GetNullableInteger(tocityid) != null)
        {
            if (int.Parse(fromcityid) == int.Parse(tocityid))
                ucError.ErrorMessage = "From and To city cannot be same.";
        }

        if (General.GetNullableDecimal(airfare) == null)
            ucError.ErrorMessage = "Airfare is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvCrewAirfare.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
        ViewState["CURRENTCITYID"] = null;
    }
    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ViewState["CURRENTCITYID"] = null;
    }


}
