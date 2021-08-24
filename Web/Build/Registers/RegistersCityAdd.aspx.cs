using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class Registers_RegistersCityAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();

            ViewState["CountryID"] = null;

            if (Request.QueryString["CountryID"] != null)
            {
                ViewState["CountryID"] = Request.QueryString["CountryID"].ToString();

                ucState.Country = Request.QueryString["CountryID"].ToString();
                ucState.StateList = PhoenixRegistersState.ListState(1, General.GetNullableInteger(Request.QueryString["CountryID"].ToString()));

                if (Request.QueryString["CityID"] != null)
                {
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    ViewState["CITYID"] = Request.QueryString["CityID"].ToString();
                    CityEdit(Int32.Parse(Request.QueryString["CityID"].ToString()));
                }
                else
                {
                    //toolbar.AddButton("New", "NEW");
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                }
                MenuCity.AccessRights = this.ViewState;
                MenuCity.MenuList = toolbar.Show();

                BindCountry();
            }
        }
    }

    protected void BindCountry()
    {
        DataSet ds = PhoenixRegistersCountry.EditCountry(int.Parse(ViewState["CountryID"].ToString()));
        DataRow dr = ds.Tables[0].Rows[0];
        txtCountry.Text = dr["FLDCOUNTRYNAME"].ToString();
    }

    private void CityEdit(int Cityid)
    {
        try
        {
            DataSet ds = PhoenixRegistersCity.EditCity(Cityid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucState.SelectedState = dr["FLDSTATEID"].ToString();
                txtCity.Text = dr["FLDCITYNAME"].ToString();
                chkInactive.Checked = dr["FLDACTIVEYN"].ToString() == "1" ? true : false;

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
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='City'>" + "\n";
            scriptClosePopup += "fnReloadList('City');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='CityNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["CITYID"] != null)
                {
                    if (!IsValidCity())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersCity.UpdateCity(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , int.Parse(ViewState["CITYID"].ToString())
                                                    , txtCity.Text
                                                    , int.Parse(ViewState["CountryID"].ToString())
                                                    , int.Parse(ucState.SelectedState)
                                                    , chkInactive.Checked.Equals(true) ? 1 : 0);

                    ucStatus.Text = "Information Updated";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "City", scriptClosePopup);
                }
                else
                {
                    if (!IsValidCity())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersCity.InsertCity(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , txtCity.Text
                                                    , int.Parse(ViewState["CountryID"].ToString())
                                                    , int.Parse(ucState.SelectedState)
                                                    , chkInactive.Checked.Equals(true) ? 1 : 0);

                    ucStatus.Text = "Information Added";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "CityNew", scriptClosePopup);
                }
            }
            if (CommandName.ToUpper().Equals("NEW"))
                Reset();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidCity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucState.SelectedState) == null)
            ucError.ErrorMessage = "State is required";

        if (General.GetNullableString(txtCity.Text) == null)
            ucError.ErrorMessage = "City is required";

        return (!ucError.IsError);

    }

    private void Reset()
    {
        ViewState["CITYID"] = null;
        ucState.SelectedState = "";
        txtCity.Text = "";
        chkInactive.Checked = false;
    }

    protected void ddlcountrylist_TextChangedEvent(object sender, EventArgs e)
    {
        if (ViewState["CountryID"] != null)
        {
            ucState.Country = ViewState["CountryID"].ToString();
            ucState.StateList = PhoenixRegistersState.ListState(1, General.GetNullableInteger(ddlcountrylist.SelectedCountry));
        }
    }
}