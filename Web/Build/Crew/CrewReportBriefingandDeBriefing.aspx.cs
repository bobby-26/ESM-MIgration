using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewReportBriefingandDeBriefing : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Crew/CrewReportBriefingandDeBriefing.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvBriefDebrief')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Crew/CrewReportBriefingandDeBriefing.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuTravelStatus.AccessRights = this.ViewState;
        MenuTravelStatus.MenuList = toolbargrid.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
        MenuTravel.AccessRights = this.ViewState;
        MenuTravel.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTROW"] = null;
            gvBriefDebrief.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        
    }
    protected void Rebind()
    {
        gvBriefDebrief.SelectedIndexes.Clear();
        gvBriefDebrief.EditIndexes.Clear();
        gvBriefDebrief.DataSource = null;
        gvBriefDebrief.Rebind();
    }
    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {            
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }

    protected void MenuTravelStatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDFILENO"
                                         , "FLDEMPLOYEENAME"
                                         , "FLDRANKNAME"
                                         , "FLDZONENAME"
                                         , "FLDTRAININGBATCHNAME"
                                         ,"FLDSTATUS"
				                         , "FLDLASTVESSALNAME"
				                         , "FLDSIGNONDATE"
				                         , "FLDSIGNOFFDATE"
				                         , "FLDPRESSENTVESSELNAME"
				                         ,  "FLDLOCATION"
				                         ,"FLDBRIEFINGDETAIL"
                                     };
                string[] alCaptions = {  "File Number"
                                          ,"Employee Name"
                                          ,"Rank"
                                          ,"Zone"
                                          ,"Training Batch"
                                          ,"Status"
                                          ,"Last vessel Name"
                                          ,"Sign On Date"
                                          ,"Sign Off Date"
                                          ,"Pressent Vessel Name"
                                          ,"Location"
                                          ,"Briefing Detail"};
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewBriefingDebriefing.ReportCrewBriefingDebriefingSearch(General.GetNullableString(txtfilenumber.Text)
                                                                                  , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                                  , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                                                                                  , (ucStatus.SelectedHard) == "," ? null : General.GetNullableString(ucStatus.SelectedHard)
                                                                                  , (ucBatchList.SelectedList) == "," ? null : General.GetNullableString(ucBatchList.SelectedList)
                                                                                  , (ucCompanyName.SelectedCompany) == "," ? null : General.GetNullableString(ucCompanyName.SelectedCompany)
                                                                                  , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucCompanyName.SelectedCompany)
                                                                                  , General.GetNullableDateTime(txtsugnonStartDate.Text)
                                                                                  , General.GetNullableDateTime(txtsignonenddate.Text)
                                                                                  , (ucbriefingid.SelectedBriefingTopic) == "," ? null : General.GetNullableString(ucbriefingid.SelectedBriefingTopic)
                                                                                  , (RadMcUserTD.SelectedValue) == "," ? null : General.GetNullableString(RadMcUserTD.SelectedValue)
                                                                                  , General.GetNullableDateTime(txtsignoffstartdate.Text)
                                                                                  , General.GetNullableDateTime(txtsignoffenddate.Text)
                                                                                  , sortexpression
                                                                                  , sortdirection
                                                                                  , 1
                                                                                  , iRowCount
                                                                                  , ref iRowCount
                                                                                  , ref iTotalPageCount
                                                                                   );

                General.ShowExcel("Crew Briefing and De-Briefing", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVessel.SelectedVessel = "";
                ucStatus.SelectedHard = "";
                ucBatchList.SelectedList = "";
                ucCompanyName.SelectedCompany = "";
                ucPool.SelectedPool = "";
                txtsugnonStartDate.Text = "";
                txtsignonenddate.Text = "";
                ucbriefingid.SelectedBriefingTopic = "";
                RadMcUserTD.SelectedValue = "";
                RadMcUserTD.SelectedUser = "";
                RadMcUserTD.SelectedEmail = "";
                RadMcUserTD._selectedemail = "";
                txtsignoffstartdate.Text = "";
                txtsignoffenddate.Text = "";
                txtfilenumber.Text = "";
                ucRank.selectedlist = "";

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDFILENO"
                                         , "FLDEMPLOYEENAME"
                                         , "FLDRANKNAME"
                                         , "FLDZONENAME"
                                         , "FLDTRAININGBATCHNAME"
                                         ,"FLDSTATUS"
				                         , "FLDLASTVESSALNAME"				                        
				                         , "FLDSIGNOFFDATE"
				                         , "FLDPRESSENTVESSELNAME"
                                         , "FLDSIGNONDATE"
				                         ,  "FLDLOCATION"
				                         ,"FLDBRIEFINGDETAIL"
                              };
        string[] alCaptions = {  "File Number"
                                          ,"Employee Name"
                                          ,"Rank"
                                          ,"Zone"
                                          ,"Training Batch"
                                          ,"Status"
                                          ,"Last Vessel"
                                          ,"Sign Off Date"                                                  
                                          ,"Onboard"
                                          ,"Sign On Date"                                 
                                          ,"Location"
                                          ,"Briefing Detail"
                              };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewBriefingDebriefing.ReportCrewBriefingDebriefingSearch(General.GetNullableString(txtfilenumber.Text)
                                                                          , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                          , (ucVessel.SelectedVessel) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel)
                                                                          , (ucStatus.SelectedHard) == "," ? null : General.GetNullableString(ucStatus.SelectedHard)
                                                                          , (ucBatchList.SelectedList) == "," ? null : General.GetNullableString(ucBatchList.SelectedList)
                                                                          , (ucCompanyName.SelectedCompany) == "," ? null : General.GetNullableString(ucCompanyName.SelectedCompany)
                                                                          , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucCompanyName.SelectedCompany)
                                                                          , General.GetNullableDateTime(txtsugnonStartDate.Text)
                                                                          , General.GetNullableDateTime(txtsignonenddate.Text)
                                                                          , (ucbriefingid.SelectedBriefingTopic) == "," ? null : General.GetNullableString(ucbriefingid.SelectedBriefingTopic)
                                                                          , (RadMcUserTD.SelectedValue) == "," ? null : General.GetNullableString(RadMcUserTD.SelectedValue)
                                                                          , General.GetNullableDateTime(txtsignoffstartdate.Text)
                                                                          , General.GetNullableDateTime(txtsignoffenddate.Text)
                                                                          , sortexpression
                                                                          , sortdirection
                                                                          , (int)ViewState["PAGENUMBER"]
                                                                          , gvBriefDebrief.PageSize
                                                                          , ref iRowCount
                                                                          , ref iTotalPageCount
                                                                           );
        General.SetPrintOptions("gvBriefDebrief", "Crew Briefing and De-Briefing", alCaptions, alColumns, ds);

        gvBriefDebrief.DataSource = ds;
        gvBriefDebrief.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    
    protected void gvBriefDebrief_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvBriefDebrief, "Select$" + e.Item.RowIndex);
            e.Item.ToolTip = "Click to select this row.";
        }

        if (e.Item is GridEditableItem)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblBriefingId");
            HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            img.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            img.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            img.Attributes.Add("onclick", "parent.Openpopup('MoreInfo','','../Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=BRIEFING','xlarge')");

            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (string.IsNullOrEmpty(lblR.Text.Trim()))
            {
                uct.Visible = false;
                img.Src = Session["images"] + "/no-remarks.png";
            }
        }
    }

    protected void gvBriefDebrief_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvBriefDebrief_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvBriefDebrief_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBriefDebrief.CurrentPageIndex + 1;

        BindReport();

    }
         
}
