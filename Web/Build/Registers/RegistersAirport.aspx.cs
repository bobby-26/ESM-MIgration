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
public partial class RegistersAirport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersAirport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAirport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersAirport.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersAirport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersAirport.AccessRights = this.ViewState;
            MenuRegistersAirport.MenuList = toolbar.Show();
 
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAirport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAirport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAirport.CurrentPageIndex + 1;
            BindData();
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
        string[] alColumns = { "FLDAIRPORTCODE", "FLDAIRPORTNAME", "FLDCOUNTRYNAME", "FLDCITYNAME", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Country", "City", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersAirport.AirportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry), txtSearch.Text, txtAirportCode.Text, null, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Airport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Airport</h3></td>");
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

    protected void RegistersAirport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvAirport.CurrentPageIndex = 0;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucCountry.SelectedCountry = "";
                txtAirportCode.Text = "";
                txtSearch.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
            string[] alColumns = { "FLDAIRPORTCODE", "FLDAIRPORTNAME", "FLDCOUNTRYNAME", "FLDCITYNAME", "FLDACTIVE" };
            string[] alCaptions = { "Code", "Name", "Country", "City", "Active Y/N" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixRegistersAirport.AirportSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry), txtSearch.Text, txtAirportCode.Text, null, sortexpression, sortdirection,
                int.Parse(ViewState["PAGENUMBER"].ToString()),
                gvAirport.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvAirport", "Airport", alCaptions, alColumns, ds);

            gvAirport.DataSource = ds;
            gvAirport.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvAirport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidAirport(((RadTextBox)e.Item.FindControl("txtAirportcodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtAirportNameAdd")).Text,
                    ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertAirport(
                    ((RadTextBox)e.Item.FindControl("txtAirportcodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtAirportNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtcityAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtcityidAdd")).Text,
                    ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0
                );

                ((RadTextBox)e.Item.FindControl("txtAirportNameAdd")).Focus();
                ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).DataBind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidAirport(((RadTextBox)e.Item.FindControl("txtairportcodeEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtairportNameEdit")).Text,
                 ((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdateAirport(
                    Int16.Parse(((RadLabel)e.Item.FindControl("lblairportidEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtairportcodeEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtairportNameEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtcityEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtcityidEdit")).Text,
                    ((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNedit")).Checked.Equals(true)) ? 1 : 0
                );
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["AIRPORTID"] = ((RadLabel)e.Item.FindControl("lblAirportid")).Text;
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
    protected void Rebind()
    {
        gvAirport.SelectedIndexes.Clear();
        gvAirport.EditIndexes.Clear();
        gvAirport.DataSource = null;
        gvAirport.Rebind();
    }
    private void InsertAirport(string Airportcode, string Airportname,string city, string cityid, string countryid, int isactive)
    {
        try
        {
            PhoenixRegistersAirport.InsertAirport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Airportcode, Airportname, city, General.GetNullableInteger(cityid), General.GetNullableInteger(countryid), isactive);
            Rebind();
            ucStatus.Text = "Airport information Added";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateAirport(int Airportid, string Airportcode, string Airportname, string city, string cityid, string countryid, int isactive)
    {
        try
        {
            PhoenixRegistersAirport.UpdateAirport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Airportid, Airportcode, Airportname, city, General.GetNullableInteger(cityid), General.GetNullableInteger(countryid), isactive);
            Rebind();
            ucStatus.Text = "Airport information updated";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidAirport(string Airportcode,string Airportname,string countryid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvAirport;
        Int16 result;
        if (Airportcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Airportname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        if (countryid == null || !Int16.TryParse(countryid, out result))
            ucError.ErrorMessage = "Country is required.";

        return (!ucError.IsError);
    }

    private void DeleteAirport(int Airportcode)
    {
        try
        {
            PhoenixRegistersAirport.DeleteAirport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Airportcode);
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAirport_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item.IsInEditMode)
        {
            string AirportCode = ((RadTextBox)e.Item.FindControl("txtairportcodeEdit")).Text;
            string AirportName = ((RadTextBox)e.Item.FindControl("txtairportNameEdit")).Text;
            UserControlCountry ddlCountry = (UserControlCountry)e.Item.FindControl("ucCountryEdit");
            string CountryId = ((RadLabel)e.Item.FindControl("lblCountryId")).Text;

            if (CountryId != null)
            {
                ddlCountry.SelectedCountry = CountryId;
            }
            LinkButton ib1 = (LinkButton)e.Item.FindControl("btncitynameEdit");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListCityEdit', 'City List', '', '" + Session["sitepath"] + "/Common/CommonPickListCity.aspx?countryid=" + CountryId + "',true);");
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton ib1 = (LinkButton)e.Item.FindControl("btncitynameAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListCityAdd', 'City List', '', '" + Session["sitepath"] + "/Common/CommonPickListCity.aspx?countryid=" + null + "',true);");
        }
    }

    protected void gvAirport_SortCommand(object sender, GridSortCommandEventArgs e)
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
            DeleteAirport(Int32.Parse(ViewState["AIRPORTID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
