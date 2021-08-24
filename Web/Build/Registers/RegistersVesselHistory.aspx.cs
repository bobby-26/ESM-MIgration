using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Registers/RegistersVesselHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvVesselHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar1.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersVesselHistoryList.aspx')", "Amend Vessel Details", "<i class=\"fas fa-plus\"></i>", "VESSELHISTORY");
        MenuRegistersVesselList.AccessRights = this.ViewState;
        MenuRegistersVesselList.MenuList = toolbar1.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

        toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
        toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
        toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
        toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
        //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
        toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);

        MenuVesselList.AccessRights = this.ViewState;
        MenuVesselList.MenuList = toolbar.Show();
        
        MenuVesselList.SelectedMenuIndex = 0;

        if (!IsPostBack) //  < i class="fas fa-plus"></i>
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Filter.CurrentVesselMasterFilter));
            if (ds.Tables[0].Rows.Count > 0)
                txtVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvVesselHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void Rebind()
    {
        gvVesselHistory.SelectedIndexes.Clear();
        gvVesselHistory.EditIndexes.Clear();
        gvVesselHistory.DataSource = null;
        gvVesselHistory.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDWEF", "FLDVALIDUNTIL", "FLDREMARKS", "FLDVESSELNAME", "FLDVESSELCODE", "FLDFLAGNAME", "FLDOWNERNAME", "FLDPRINCIPALNAME", "FLDMANAGERNAME", "FLDWAGESCALENAME", "FLDWAGESCALERATINGNAME", "FLDSENIORITYSCALENAME", "FLDACTIVE" };
        string[] alCaptions = { "W.E.F.", "Valid Until", "Remarks", "Name", "Code", "Flag", "Owner", "Principal", "Manager", "Officer Wage Scale", "Ratings Wage Scale", "Seniority Wage Scale", "Active/In-Active" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersVessel.VesselHistorySearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , int.Parse(Filter.CurrentVesselMasterFilter)
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselHistory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel History</h3></td>");
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

    protected void RegistersVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWEF", "FLDVALIDUNTIL", "FLDREMARKS", "FLDVESSELNAME", "FLDVESSELCODE", "FLDFLAGNAME", "FLDOWNERNAME", "FLDPRINCIPALNAME", "FLDMANAGERNAME", "FLDWAGESCALENAME", "FLDWAGESCALERATINGNAME", "FLDSENIORITYSCALENAME", "FLDACTIVE" };
        string[] alCaptions = { "W.E.F.", "Valid Until", "Remarks", "Name", "Code", "Flag", "Owner", "Principal", "Manager", "Officer Wage Scale", "Ratings Wage Scale", "Seniority Wage Scale", "Active/In-Active" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersVessel.VesselHistorySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(Filter.CurrentVesselMasterFilter)
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvVesselHistory.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvVesselHistory", "Vessel History", alCaptions, alColumns, ds);
        gvVesselHistory.DataSource = ds;
        gvVesselHistory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvVesselHistory_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarksTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            lbtn = (RadLabel)e.Item.FindControl("lblAmendedBy");
            uct = (UserControlToolTip)e.Item.FindControl("ucAmendedByTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            lbtn = (RadLabel)e.Item.FindControl("lblOwner");
            uct = (UserControlToolTip)e.Item.FindControl("ucOwnerTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            lbtn = (RadLabel)e.Item.FindControl("lblManager");
            uct = (UserControlToolTip)e.Item.FindControl("ucManagerTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            lbtn = (RadLabel)e.Item.FindControl("lblPrincipal");
            uct = (UserControlToolTip)e.Item.FindControl("ucPrincipalTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            lbtn = (RadLabel)e.Item.FindControl("lblWageScale");
            uct = (UserControlToolTip)e.Item.FindControl("ucWageScaleTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            lbtn = (RadLabel)e.Item.FindControl("lblRatingsWageScale");
            uct = (UserControlToolTip)e.Item.FindControl("ucRatingsWageScaleTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            lbtn = (RadLabel)e.Item.FindControl("lblSeniorityWageScale");
            uct = (UserControlToolTip)e.Item.FindControl("ucSeniorityWageScaleTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;


            lbtn = (RadLabel)e.Item.FindControl("lblESMStdComp");
            uct = (UserControlToolTip)e.Item.FindControl("ucESMStdCompTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;
        } 
    }
    protected void gvVesselHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
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
    protected void gvVesselHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselHistory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }
}
