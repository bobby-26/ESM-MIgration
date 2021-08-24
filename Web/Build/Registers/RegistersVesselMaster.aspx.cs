using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class RegistersVesselMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            Response.Redirect("../Registers/RegistersVessel.aspx?Vesselid="+PhoenixSecurityContext.CurrentSecurityContext.VesselID, true);
        }
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        toolbar1.AddButton("Vessel Search", "SEARCH", ToolBarDirection.Right);
        toolbar1.AddButton("List", "LIST", ToolBarDirection.Right);

        MenuRegisters.AccessRights = this.ViewState;
        MenuRegisters.MenuList = toolbar1.Show();
        MenuRegisters.SelectedMenuIndex = 1;
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselMaster.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselMaster.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselMaster.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            toolbar.AddFontAwesomeButton("../Registers/RegistersVessel.aspx?NewMode=true", "Add", "<i class=\"fas fa-plus-circle\"></i>", "VESSEL");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','Pending Issues','" + Session["sitepath"] + "/Registers/RegistersVesselChatbox.aspx?launchedFrom=VMLIST');return false;", "Pending Issues", "<i class=\"fas fa-clipboard\" aria-hidden=\"true\"></i>", "PENDINGISSUES");
        }

        MenuRegistersVesselList.AccessRights = this.ViewState;
        MenuRegistersVesselList.MenuList = toolbar.Show();


    }
    protected void Rebind()
    {
        gvVesselList.SelectedIndexes.Clear();
        gvVesselList.EditIndexes.Clear();
        gvVesselList.DataSource = null;
        gvVesselList.Rebind();
    }
    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselList.CurrentPageIndex + 1;
        BindData();
    }
    protected void gvVesselList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && !e.Item.IsInEditMode)
        {
            string vesselId = ((RadLabel)e.Item.FindControl("lblVesselEditId")).Text;
            LinkButton ib = (LinkButton)e.Item.FindControl("lblVesselName");
            ib.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Vessel Particulars', '" + Session["sitepath"] + "/Registers/RegistersVessel.aspx?Vesselid=" + vesselId + "',false,1000,550);return false;");
        }
    }
    protected void RegistersVesselList_TabStripCommand(object sender, EventArgs e)
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
            txtSearchVesselList.Text = "";
            ddlActiveyn.SelectedIndex = -1;
            txtVesselCode.Text = "";
            ucVesselType.SelectedVesseltype = string.Empty;
            ucFlag.SelectedFlag = string.Empty;
            ucPrincipal.SelectedAddress = string.Empty;
            Rebind();
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDIMONUMBER", "FLDVESSELFLAG", "FLDVESSELTYPE", "FLDTAKEOVERDATE", "FLDESMHANDOVERDATE" };
        string[] alCaptions = { "Vessel Name", "Vessel Code", "IMO No.", "Flag", "Type", "Takeover date", "Handover date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonRegisters.VesselSearch(
                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , txtSearchVesselList.Text.Trim()
                                         , sortexpression
                                         , sortdirection
                                         , (int)ViewState["PAGENUMBER"]
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , General.GetNullableInteger(ddlActiveyn.SelectedValue)//, General.GetNullableInteger(nvc.Get("ddlActiveyn").ToString())
                                         , txtVesselCode.Text.Trim()//General.GetNullableString(nvc.Get("txtVesselCode").ToString())
                                         , General.GetNullableInteger(ucFlag.SelectedFlag)//General.GetNullableInteger(nvc.Get("ucFlag").ToString())
                                         , General.GetNullableInteger(ucVesselType.SelectedVesseltype)//General.GetNullableInteger(nvc.Get("ucVesselType").ToString())
                                         , General.GetNullableInteger(ucPrincipal.SelectedAddress));//General.GetNullableInteger(nvc.Get("ucPrincipal").ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel List</h3></td>");
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
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
            string[] alColumns = { "FLDVESSELNAME", "FLDVESSELCODE", "FLDIMONUMBER", "FLDVESSELFLAG", "FLDVESSELTYPE", "FLDTAKEOVERDATE", "FLDESMHANDOVERDATE" };
            string[] alCaptions = { "Vessel Name", "Vessel Code", "IMO No.", "Flag", "Type", "Takeover date", "Handover date" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCommonRegisters.VesselSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                         , txtSearchVesselList.Text.Trim(), sortexpression, sortdirection
                                         , (int)ViewState["PAGENUMBER"], gvVesselList.PageSize
                                         , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ddlActiveyn.SelectedValue)
                                         , txtVesselCode.Text.Trim(), General.GetNullableInteger(ucFlag.SelectedFlag)
                                         , General.GetNullableInteger(ucVesselType.SelectedVesseltype), General.GetNullableInteger(ucPrincipal.SelectedAddress));
            gvVesselList.DataSource = ds;
            General.SetPrintOptions("gvVesselList", "Vessel List", alCaptions, alColumns, ds);
            gvVesselList.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRegisters_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
        }
        else
        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Registers/RegistersVesselMaster.aspx");
        }

    }

    protected void gvVesselList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}