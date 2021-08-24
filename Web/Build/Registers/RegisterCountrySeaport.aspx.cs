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
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegisterCountrySeaport : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    if (!IsPostBack)
    //    {
    //        BindData();
    //        SetPageNavigator();
    //    }

    //    foreach (GridViewRow r in gvSeaport.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvSeaport.UniqueID, "Select$" + r.RowIndex.ToString());
    //        }
    //    }
    //    BindAirport();
    //    //BindData();
    //    //SetPageNavigator();
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton(Session["sitepath"] + "/Registers/RegisterCountrySeaport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSeaport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton(Session["sitepath"] + "/Registers/RegisterCountrySeaport.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton(Session["sitepath"] + "/Registers/RegisterCountrySeaport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersSeaport.AccessRights = this.ViewState;
            MenuRegistersSeaport.MenuList = toolbar.Show();
            //MenuRegistersSeaport.SetTrigger(pnlSeaportEntry);

            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Country Visa", "COUNTRYVISA");
            //toolbarmain.AddButton("Visa Details", "VISA");
            //MenuregistersSeaPortMain.AccessRights = this.ViewState;
            //MenuregistersSeaPortMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = null;
                ViewState["COUNTRYID"] = null;
                ViewState["COUNTRYNAME"] = null;
                //ddlCountrySearch.CountryList = PhoenixRegistersCountry.ListCountry(1, 1);
                //ddlCountrySearch.DataBind();

                if (Request.QueryString["countryid"] != null)
                {
                    ViewState["COUNTRYID"] = Request.QueryString["countryid"].ToString();
                    LblCountryID.Text = ViewState["COUNTRYID"].ToString();
                    ViewState["COUNTRYNAME"] = Request.QueryString["countryname"].ToString();
                    LblCountryName.Text = ViewState["COUNTRYNAME"].ToString();
                }

                //BindAirport();
            }
            //BindData();
            //BindAirport();

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
        string[] alColumns = { "FLDSEAPORTCODE", "FLDSEAPORTNAME", "FLDCOUNTRYNAME", "FLDAIRPORTNAME", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Country", "Airport", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCommonRegisters.SeaportSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSearch.Text, txtSeaportCode.Text,
            General.GetNullableInteger(LblCountryID.Text) == null ? 0 : General.GetNullableInteger(LblCountryID.Text),
            null, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Seaport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Seaport Register</h3></td>");
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

    protected void RegistersSeaport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvSeaport.EditIndex = -1;
                ViewState["CURRENTINDEX"] = -1;
                //gvSeaport.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtSeaportCode.Text = "";
                txtSearch.Text = "";
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSEAPORTCODE", "FLDSEAPORTNAME", "FLDCOUNTRYNAME", "FLDAIRPORTNAME", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Country", "Airport", "Active Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonRegisters.SeaportSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSeaportCode.Text, txtSearch.Text,
            General.GetNullableInteger(LblCountryID.Text) == null ? General.GetNullableInteger(ViewState["COUNTRYID"].ToString()) : General.GetNullableInteger(LblCountryID.Text),
            null, sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvSeaport.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvSeaport", "Registers", alCaptions, alColumns, ds);

        if (ViewState["CURRENTINDEX"] != null && (int)ViewState["CURRENTINDEX"] >= ds.Tables[0].Rows.Count)
        {
            ViewState["CURRENTINDEX"] = -1;
            gvSeaport.SelectedIndexes.Clear();
            gvSeaport.EditIndexes.Clear();
        }
        gvSeaport.DataSource = ds;
        gvSeaport.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void Rebind()
    {
        gvSeaport.EditIndexes.Clear();
        gvSeaport.SelectedIndexes.Clear();
        gvSeaport.DataSource = null;
        gvSeaport.Rebind();
    }
    private void InsertSeaport(string Seaportcode, string Seaportname, string remarks, string airportname, int isactive)
    {

        PhoenixRegistersSeaport.InsertSeaport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Seaportcode, Seaportname, remarks, airportname, isactive);
    }

    private void UpdateSeaport(int Seaportid, string Seaportcode, string Seaportname, string airportname, int isactive)
    {

        PhoenixRegistersSeaport.UpdateSeaport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Seaportid, Seaportcode, Seaportname, airportname, isactive);
        ucStatus.Text = "Sea Port information updated";
    }

    private bool IsValidSeaport(string Seaportcode, string Seaportname, string airportname, string country)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;

        if (Seaportcode.Trim().Equals(""))
            ucError.ErrorMessage = "Seaport Code is required.";

        if (Seaportname.Trim().Equals(""))
            ucError.ErrorMessage = "Seaport Name is required.";

        if (country.Trim().Equals("") || !Int16.TryParse(country, out result))
            ucError.ErrorMessage = "Country is required.";

        if (airportname.Trim().Equals("") || !Int16.TryParse(airportname, out result))
            ucError.ErrorMessage = "Airport is required.";

        return (!ucError.IsError);
    }

    private void DeleteSeaport(int Seaportcode)
    {
        PhoenixRegistersSeaport.DeleteSeaport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Seaportcode);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private bool IsValidCountry()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (LblCountryID.Text == "" || LblCountryID.Text.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Country is required.";

        return (!ucError.IsError);
    }

    protected void gvSeaport_NeedDataSource(object sender,GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSeaport.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvSeaport_ItemDataBound(object sender,GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton edit = (LinkButton)item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton save = (LinkButton)item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton add = (LinkButton)item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

            LinkButton portcomments = (LinkButton)item.FindControl("cmdPortComments");
            if (portcomments != null) portcomments.Visible = SessionUtil.CanAccess(this.ViewState, portcomments.CommandName);

            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            UserControlAirport ucAirport = (UserControlAirport)item.FindControl("Airport1");
            DataRowView drvAirport = (DataRowView)item.DataItem;
            RadLabel airportid = (RadLabel)item.FindControl("lblAirportId");
            if (ucAirport != null)
            {
                ucAirport.AirportList = PhoenixRegistersAirport.ListAirportByCountry(
                    General.GetNullableInteger(LblCountryID.Text) == null ? null : General.GetNullableInteger(LblCountryID.Text), 1);
                ucAirport.SelectedAirport = drvAirport["FLDAIRPORTID"].ToString();
            }

            if (!e.Item.IsInEditMode)
            {
                LinkButton lnk = (LinkButton)item.FindControl("lnkSeaportName");
                lnk.Attributes.Add("onclick", "javascript:openNewWindow('country','','" + Session["sitepath"] + "/Registers/RegistersSeaPortCountryVisa.aspx?countryid=" + LblCountryID.Text + "'); return false;");
            }
            if (portcomments != null)
            {
                portcomments.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Registers/RegistersSeaPortRemarks.aspx?seaportid=" + drvAirport["FLDSEAPORTID"].ToString() + "'); return false;");
            }
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footer = (GridFooterItem)e.Item;
            UserControlAirport ucAirport = (UserControlAirport)footer.FindControl("Airport2");

            if (ucAirport != null)
            {
                ucAirport.Country = General.GetNullableInteger(LblCountryID.Text) == null ? null : General.GetNullableInteger(ViewState["COUNTRYID"].ToString()).ToString();
                ucAirport.AirportList = PhoenixRegistersAirport.ListAirportByCountry(int.Parse(ucAirport.Country), 1);
            }
        }

    }

    protected void gvSeaport_ItemCommand(object sender,GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footer = (GridFooterItem)e.Item;
                if (!IsValidSeaport(((RadTextBox)footer.FindControl("txtSeaportcodeAdd")).Text,
                     ((RadTextBox)footer.FindControl("txtSeaportNameAdd")).Text,
                     ((UserControlAirport)footer.FindControl("Airport2")).SelectedAirport,
                     LblCountryID.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSeaport(
                    ((RadTextBox)footer.FindControl("txtSeaportcodeAdd")).Text,
                    ((RadTextBox)footer.FindControl("txtSeaportNameAdd")).Text,
                    null,
                    ((UserControlAirport)footer.FindControl("Airport2")).SelectedAirport,
                    (((CheckBox)footer.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0

                );
                Rebind();
                ((RadTextBox)footer.FindControl("txtSeaportNameAdd")).Focus();
                ((UserControlAirport)footer.FindControl("Airport2")).DataBind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidSeaport(((RadTextBox)item.FindControl("txtSeaportcodeEdit")).Text,
                     ((RadTextBox)item.FindControl("txtSeaportNameEdit")).Text,
                     ((UserControlAirport)item.FindControl("Airport1")).SelectedAirport,
                     LblCountryID.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateSeaport(
                    Int16.Parse(((RadLabel)item.FindControl("lblSeaportidEdit")).Text),
                    ((RadTextBox)item.FindControl("txtSeaportcodeEdit")).Text,
                    ((RadTextBox)item.FindControl("txtSeaportNameEdit")).Text,
                    ((UserControlAirport)item.FindControl("Airport1")).SelectedAirport,
                    (((CheckBox)item.FindControl("chkActiveYNedit")).Checked) ? 1 : 0

                 );
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSeaport(Int32.Parse(((RadLabel)e.Item.FindControl("lblSeaportid")).Text));
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
}
