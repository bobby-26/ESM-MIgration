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
public partial class RegistersSeaport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersSeaport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSeaport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersSeaport.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersSeaport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersSeaport.AccessRights = this.ViewState;
            MenuRegistersSeaport.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Visa Details", "VISA", ToolBarDirection.Right);
            toolbarmain.AddButton("Country Visa", "COUNTRYVISA",ToolBarDirection.Right);
            
            MenuregistersSeaPortMain.AccessRights = this.ViewState;
            MenuregistersSeaPortMain.MenuList = toolbarmain.Show();
            MenuregistersSeaPortMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["countryid"] != null)
                    ddlCountrySearch.SelectedCountry = Request.QueryString["countryid"].ToString();

                gvSeaport.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDSEAPORTCODE", "FLDSEAPORTNAME", "FLDAIRPORTNAME", "FLDCITYNAME", "FLDEUSEAPORT", "FLDISECA", "FLDOPNSCRBRALWD", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Airport", "City", "EU Regulation Y/N", "ECA Y/N", "OPN scrubber Y/N", "Active Y/N" };
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
            General.GetNullableInteger(ddlCountrySearch.SelectedCountry) == null ? 0 : General.GetNullableInteger(ddlCountrySearch.SelectedCountry), 
            null, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Seaport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sea Port</h3></td>");
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
                ViewState["CURRENTINDEX"] = -1;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlCountrySearch.SelectedCountry = "";
                txtSeaportCode.Text = "";
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
    protected void Rebind()
    {
        gvSeaport.SelectedIndexes.Clear();
        gvSeaport.EditIndexes.Clear();
        gvSeaport.DataSource = null;
        gvSeaport.Rebind();
    }
    //private void BindAirport()
    //{
    //    try
    //    {
    //        UserControlAirport ucAirport = new UserControlAirport();
    //        RadLabel airportid = new RadLabel();

    //        if (ViewState["CURRENTINDEX"] != null && (int)ViewState["CURRENTINDEX"] != -1)
    //        {
    //            ucAirport = (UserControlAirport)gvSeaport.Items[(int)ViewState["CURRENTINDEX"]].FindControl("Airport1");
    //            airportid = (RadLabel)gvSeaport.Items[(int)ViewState["CURRENTINDEX"]].FindControl("lblAirportId");

    //            ucAirport.AirportList = PhoenixRegistersAirport.ListAirportByCountry(
    //                    General.GetNullableInteger(ddlCountrySearch.SelectedCountry) == null ? null : General.GetNullableInteger(ddlCountrySearch.SelectedCountry), 1);
    //        }
    //        else
    //        {
    //            ucAirport = (UserControlAirport)gvSeaport.FindControl("Airport2");

    //            DataSet ds = PhoenixRegistersAirport.ListAirportByCountry(
    //                    General.GetNullableInteger(ddlCountrySearch.SelectedCountry) == null ? null : General.GetNullableInteger(ddlCountrySearch.SelectedCountry), 1);

    //            ucAirport.AirportList = ds;
    //        }

    //        if (airportid != null && airportid.Text != "")
    //        {
    //            ucAirport.SelectedAirport = airportid.Text;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //        return;
    //    }
    //}

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSEAPORTCODE", "FLDSEAPORTNAME", "FLDAIRPORTNAME", "FLDCITYNAME", "FLDEUSEAPORT", "FLDISECA", "FLDOPNSCRBRALWD", "FLDACTIVE" };
        string[] alCaptions = { "Code", "Name", "Airport", "City", "EU Regulation Y/N", "ECA Y/N", "OPN scrubber Y/N", "Active Y/N" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonRegisters.SeaportSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSeaportCode.Text, txtSearch.Text,
            General.GetNullableInteger(ddlCountrySearch.SelectedCountry) == null ? 0 : General.GetNullableInteger(ddlCountrySearch.SelectedCountry), 
            null, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvSeaport.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvSeaport", "Sea Port", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["CURRENTINDEX"] != null && (int)ViewState["CURRENTINDEX"] >= ds.Tables[0].Rows.Count)
            {
                ViewState["CURRENTINDEX"] = -1;
            }
        }
        else
        {
            if (ViewState["CURRENTINDEX"] != null && (int)ViewState["CURRENTINDEX"] >= ds.Tables[0].Rows.Count)
            {
                ViewState["CURRENTINDEX"] = -1;
            }
        }
        gvSeaport.DataSource = ds;
        gvSeaport.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvSeaport_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
				if (!IsValidSeaport(((RadTextBox)e.Item.FindControl("txtSeaportcodeAdd")).Text,
					 ((RadTextBox)e.Item.FindControl("txtSeaportNameAdd")).Text,
					 ((UserControlAirport)e.Item.FindControl("Airport2")).SelectedAirport,
                     ddlCountrySearch.SelectedCountry)
                     )
				{
                    e.Canceled = true;
					ucError.Visible = true;
					return;
				}
                InsertSeaport(
                    ((RadTextBox)e.Item.FindControl("txtSeaportcodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtSeaportNameAdd")).Text,
                    null,
                    ((UserControlAirport)e.Item.FindControl("Airport2")).SelectedAirport,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkEURegulationAdd")).Checked.Equals(true)) ? 1 : 0,
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("ddlCityAdd")).Text),
                    (((RadCheckBox)e.Item.FindControl("chkECAAdd")).Checked.Equals(true)) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkopnscrubynAdd")).Checked.Equals(true)) ? 1 : 0
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtSeaportNameAdd")).Focus();
                ((UserControlAirport)e.Item.FindControl("Airport2")).DataBind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
				if (!IsValidSeaport(((RadTextBox)e.Item.FindControl("txtSeaportcodeEdit")).Text,
					 ((RadTextBox)e.Item.FindControl("txtSeaportNameEdit")).Text,
					 ((UserControlAirport)e.Item.FindControl("Airport1")).SelectedAirport,
                     ddlCountrySearch.SelectedCountry))
				{
                    e.Canceled = true;
                    ucError.Visible = true;
					return;
				}
                UpdateSeaport(
                    Int16.Parse(((RadLabel)e.Item.FindControl("lblSeaportidEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtSeaportcodeEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtSeaportNameEdit")).Text,
                    ((UserControlAirport)e.Item.FindControl("Airport1")).SelectedAirport,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNedit")).Checked.Equals(true)) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkEURegulationEdit")).Checked.Equals(true)) ? 1 : 0,
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("ddlCityEdit")).Text),
                    (((RadCheckBox)e.Item.FindControl("chkECAEdit")).Checked.Equals(true)) ? 1 : 0,
                    (((RadCheckBox)e.Item.FindControl("chkopnscrubynEdit")).Checked.Equals(true)) ? 1 : 0
                 );
                ViewState["CURRENTINDEX"] = -1;
                Rebind();
            }            
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["Seaportid"] = ((RadLabel)e.Item.FindControl("lblSeaportid")).Text;
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
    protected void gvSeaport_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        LinkButton portcomments = (LinkButton)e.Item.FindControl("cmdPortComments");
        if (portcomments != null) portcomments.Visible = SessionUtil.CanAccess(this.ViewState, portcomments.CommandName);

        if (e.Item is GridEditableItem)
        {
            //LinkButton lnk = (LinkButton)e.Item.FindControl("lnkSeaportName");
            //lnk.Attributes.Add("onclick", "openNewWindow('country','', '" + Session["sitepath"] + "RegistersSeaPortCountryVisa.aspx?countryid=" + ddlCountrySearch.SelectedCountry + "'); return false;");
          
            if (portcomments != null)
            {
                portcomments.Attributes.Add("onclick", "openNewWindow('codehelp1','Remarks', '" + Session["sitepath"] + "/Registers/RegistersSeaPortRemarks.aspx?seaportid=" + DataBinder.Eval(e.Item.DataItem, "FLDSEAPORTID").ToString() + "'); return false;");
            }

            LinkButton ImgShowCharterAgent = (LinkButton)e.Item.FindControl("btnShowddlCityEdit");

            if (ImgShowCharterAgent != null)
            {
                ImgShowCharterAgent.Attributes.Add("onclick", "return showPickList('spnPickListddlCityEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCity.aspx?countryid=" + ddlCountrySearch.SelectedCountry + "', true); ");
            }
        }
        if (e.Item is GridEditableItem)
        {
            UserControlAirport ucAirport = (UserControlAirport)e.Item.FindControl("Airport1");
            DataRowView drvAirport = (DataRowView)e.Item.DataItem;
            if (ucAirport != null) ucAirport.SelectedAirport = DataBinder.Eval(e.Item.DataItem, "FLDAIRPORTID").ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ImgShowCharterAgent = (LinkButton)e.Item.FindControl("btnShowddlCityAdd");

            if (ImgShowCharterAgent != null)
            {
                ImgShowCharterAgent.Attributes.Add("onclick", "return showPickList('spnPickListddlCityAdd', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCity.aspx?countryid=" + ddlCountrySearch.SelectedCountry + "', true); ");
            }
            GridFooterItem footer = (GridFooterItem)e.Item;
            UserControlAirport ucAirport = (UserControlAirport)footer.FindControl("Airport2");

            if (ucAirport != null)
            {
                ucAirport.AirportList = PhoenixRegistersAirport.ListAirportByCountry(
                           General.GetNullableInteger(ddlCountrySearch.SelectedCountry) == null ? null : General.GetNullableInteger(ddlCountrySearch.SelectedCountry), 1);
            }
        }
    }
    private void InsertSeaport(string Seaportcode, string Seaportname, string remarks, string airportname, int isactive, int EURegulation, int? cityid, int? ECAYN, int? OpnScrubberYN)
    {
        PhoenixRegistersSeaport.InsertSeaport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Seaportcode, Seaportname, remarks,airportname,isactive,EURegulation, cityid, ECAYN, OpnScrubberYN);
    }

    private void UpdateSeaport(int Seaportid, string Seaportcode, string Seaportname, string airportname, int isactive, int EURegulation,int? cityid, int? ECAYN, int? OpnScrubberYN)
    {
        PhoenixRegistersSeaport.UpdateSeaport(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Seaportid, Seaportcode, Seaportname, airportname, isactive, EURegulation, cityid, ECAYN, OpnScrubberYN);
        ucStatus.Text = "Sea Port information updated";
    }

    private bool IsValidSeaport(string Seaportcode,string Seaportname,string airportname, string country)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        RadGrid _gridView = gvSeaport;

        if (Seaportcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Seaportname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

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
    protected void MenuregistersSeaPortMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VISA"))
            {
                if (!IsValidCountry())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("../Registers/RegistersSeaPortVisaDocuments.aspx?countryid=" + ddlCountrySearch.SelectedCountry, true);
                }
            }

            if (CommandName.ToUpper().Equals("COUNTRYVISA"))
            {
                if (!IsValidCountry())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Response.Redirect("../Registers/RegistersSeaPortCountryVisa.aspx?countryid=" + ddlCountrySearch.SelectedCountry, true);
                }
            }

            if (CommandName.ToUpper().Equals("COMMENTS"))
            {
                if (ViewState["seaportid"] == null)
                {
                    ucError.ErrorMessage = "Please Select a Sea Port";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    String scriptpopup = String.Format(
                            "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "RegistersSeaPortRemarks.aspx?seaportid=" + ViewState["seaportid"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidCountry()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlCountrySearch.SelectedCountry == "" || ddlCountrySearch.SelectedCountry.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Country is required.";

        return (!ucError.IsError);
    }

    protected void gvSeaport_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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
    protected void gvSeaport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSeaport.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteSeaport(Int32.Parse(ViewState["Seaportid"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlCountrySearch_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }
}
