using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewChangePlanFilter : PhoenixBasePage
{
    string strVesselId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        PhoenixToolbar SubMenutoolbar = new PhoenixToolbar();
        SubMenutoolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        CCPSubMenu.AccessRights = this.ViewState;
        CCPSubMenu.MenuList = SubMenutoolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Crew/CrewChangePlanFilter.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCCPlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Crew/CrewChangePlanFilter.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Crew/CrewChangePlanFilter.aspx", "Travel Request", "<i class=\"fas fa-plane-departure-it\"></i>", "CREWCHANGEREQUEST");
        if (ddlVessel.SelectedVessel != "" && ddlVessel.SelectedVessel != "Dummy")
        {
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersVesselChatbox.aspx?launchedFrom=CREWCHANGEPLAN&vesselid=" + ddlVessel.SelectedVessel + "')", "Communication", "<i class=\"fas fa-comments\"></i>", "COMMUNICATION");
        }
        else
        {
            toolbargrid.AddFontAwesomeButton("", "Communication", "<i class=\"fas fa-comments\"></i>", "COMMUNICATION");
        }
        toolbargrid.AddFontAwesomeButton("../Crew/CrewCostEvaluationRequest.aspx?VESSELID=" + ddlVessel.SelectedVessel, "Port Cost Analysis", "<i class=\"fas fa-file-alt-dv\"></i>", "PORTCOST");
        toolbargrid.AddFontAwesomeButton("../Crew/CrewChangePlanFilter.aspx", "Planned Vessel List", "<i class=\"fas fa-tasks-Planned\"></i>", "PLANNED");
        MenuCrewChangePlan.AccessRights = this.ViewState;
        MenuCrewChangePlan.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = -1;
            txtTentativeDate.Text = DateTime.Now.Date.ToString();
            gvCCPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDRANKNAME", "FLDNAME", "FLDEXPECTEDJOINDATE", "FLDSEAPORTNAME", "FLDOFFSIGNERNAME", "FLDPDSTATUS", "FLDRELIEFDUEDATE" };
            string[] alCaptions = { "Rank", "Name", "Planned Relief", "Planned Port", "Off-Signer", "PD Status", "Relief Due" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentCrewChangePlanFilterSelection;
            DataSet ds = PhoenixCrewChangePlan.SearchCrewChangePlan(General.GetNullableInteger(ddlVessel.SelectedVessel), byte.Parse(Request.QueryString["access"] != null ? "1" : "0"), sortexpression, sortdirection,
                                                                      (int)ViewState["PAGENUMBER"],
                                                                      gvCCPlan.PageSize,
                                                                      ref iRowCount,
                                                                      ref iTotalPageCount,
                                                                      General.GetNullableInteger(lblNoofdays.Text));

            General.SetPrintOptions("gvCCPlan", "Crew Change Plan", alCaptions, alColumns, ds);
            gvCCPlan.DataSource = ds;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    if (!SessionUtil.CanAccess(this.ViewState, "SAVE"))
            //        CCPSubMenu.Visible = false;
            //    else
            //        CCPSubMenu.Visible = true;
            //}
            //else
            //    CCPSubMenu.Visible = false;
            gvCCPlan.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCPlan.CurrentPageIndex + 1;
        BindData();
    }
    protected void Rebind()
    {
        gvCCPlan.SelectedIndexes.Clear();
        gvCCPlan.EditIndexes.Clear();
        gvCCPlan.DataSource = null;
        gvCCPlan.Rebind();
    }
    protected void CCPSubMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSearch(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                NameValueCollection nvc = Filter.CurrentCrewChangePlanFilterSelection;
                if (!IsValidSave(General.GetNullableString(txtTentativeDate.Text.Trim()), ddlPort.SelectedSeaport))
                {
                    ucError.Visible = true;
                    return;
                }
                CrewChangeDateAndPortUpdate();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidSave(string TentativeDate, string Seaport)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (TentativeDate.Trim() == string.Empty)
            ucError.ErrorMessage = "Date of Crew Change is Required";
        if (Seaport.Trim() == "" || Seaport.Trim() == "Dummy")
            ucError.ErrorMessage = "Port is Required";
        return (!ucError.IsError);
    }
    private bool IsValidSearch(string Vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (Vessel.Trim() == string.Empty || Vessel.Trim().ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Please Select Vessel";
        return (!ucError.IsError);
    }
    private void CrewChangeDateAndPortUpdate()
    {
        string sCrewPlanId = "";
        foreach (GridDataItem gv in gvCCPlan.Items)
        {
            RadLabel lblCrewPlanId = (RadLabel)gv.FindControl("lblCrewPlanId");
            if (lblCrewPlanId.Text.Trim() != string.Empty)
                sCrewPlanId += lblCrewPlanId.Text.Trim() + ",";
        }
        PhoenixCrewChangePlan.CrewChangeDateAndPortUpdate(sCrewPlanId
            , General.GetNullableDateTime(txtTentativeDate.Text.Trim())
            , General.GetNullableInteger(ddlPort.SelectedSeaport));
        ucStatus.Text = "Updated Sucessfully";
        ucStatus.Visible = true;
        Rebind();
    }
    protected void ChangePlan_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {

                if (!IsValidSearch(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }

                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDRANKNAME", "FLDNAME", "FLDEXPECTEDJOINDATE", "FLDSEAPORTNAME", "FLDOFFSIGNERNAME", "FLDPDSTATUS", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "Rank", "Name", "Planned Relief", "Planned Port", "Off-Signer", "PD Status", "Relief Due" };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CurrentCrewChangePlanFilterSelection;
                DataSet ds = PhoenixCrewChangePlan.SearchCrewChangePlan(int.Parse(ddlVessel.SelectedVessel), byte.Parse(Request.QueryString["access"] != null ? "1" : "0"), sortexpression, sortdirection,
                                                                          (int)ViewState["PAGENUMBER"],
                                                                          General.ShowRecords(null),
                                                                          ref iRowCount,
                                                                          ref iTotalPageCount,
                                                                          General.GetNullableInteger(lblNoofdays.Text));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Change Plan", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!IsValidSearch(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                NameValueCollection nvc = new NameValueCollection();
                if (Filter.CurrentCrewChangePlanFilterSelection != null)
                    nvc = Filter.CurrentCrewChangePlanFilterSelection;
                nvc.Clear();
                nvc.Add("ucRank", string.Empty);//ucRank.selectedlist);
                nvc.Add("ucVessel", ddlVessel.SelectedVessel);
                nvc.Add("ucZone", string.Empty);//ucZone.selectedlist);
                Filter.CurrentCrewChangePlanFilterSelection = nvc;
                Rebind();

            }

            if (CommandName.ToUpper().Equals("CREWCHANGEREQUEST"))
            {
                if (!IsValidSearch(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Crew/CrewChangeTravel.aspx?vessel=" + ddlVessel.SelectedVessel + (Request.QueryString["access"] != null ? "?access=1" : string.Empty)
                                        + "&port=" + ddlPort.SelectedSeaport
                                        + "&date=" + General.GetDateTimeToString(txtTentativeDate.Text)
                                        + "&from=crewchange", false);
            }

            if (CommandName.ToUpper().Equals("COMMUNICATION"))
            {
                if (!IsValidSearch(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
            }

            if (CommandName.ToUpper().Equals("PLANNED"))
            {
                ddlVessel.PlannedDays = lblNoofdays.Text.Trim();
                ddlVessel.bind();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeId");
            RadLabel rankid = (RadLabel)e.Item.FindControl("lblRankId");
            RadLabel vesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            LinkButton sg = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton md = (LinkButton)e.Item.FindControl("cmdMedical");
            LinkButton cu = (LinkButton)e.Item.FindControl("cmdCourse");
            LinkButton jp = (LinkButton)e.Item.FindControl("imgJoiningLetter");
            LinkButton ckl = (LinkButton)e.Item.FindControl("cmdChkList");
            LinkButton imgNok = (LinkButton)e.Item.FindControl("imgNok");
            LinkButton wgr = (LinkButton)e.Item.FindControl("cmdWorkGear");
            if (!SessionUtil.CanAccess(this.ViewState, sg.CommandName)) sg.Visible = false;
            if (!SessionUtil.CanAccess(this.ViewState, md.CommandName)) md.Visible = false;
            if (!SessionUtil.CanAccess(this.ViewState, cu.CommandName)) cu.Visible = false;
            if (!SessionUtil.CanAccess(this.ViewState, jp.CommandName)) jp.Visible = false;
            if (!SessionUtil.CanAccess(this.ViewState, wgr.CommandName)) wgr.Visible = false;
            if (ckl != null) ckl.Visible = SessionUtil.CanAccess(this.ViewState, ckl.CommandName);
            LinkButton imgSuitableCheck = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            imgSuitableCheck.Attributes.Add("onclick", "parent.openNewWindow('codehelp', '', '" + Session["sitepath"] + "/Crew/CrewSuitabilityCheck.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&personalmaster=true');return false;");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmployee");
            LinkButton lnkoffsigner = (LinkButton)e.Item.FindControl("lnkOffSigner");
            LinkButton con = (LinkButton)e.Item.FindControl("cmdGenContract");
          

            if (!string.IsNullOrEmpty(empid.Text))
            {
                sg.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewMissingLicenceRequest.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&jdate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                imgNok.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewFamilyNok.aspx?empid=" + empid.Text + "');return false;");
                cu.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewCourseMissing.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&jdate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&rankname=" + drv["FLDRANKNAME"] + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                con.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewContract.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&date=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&planid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                md.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewMedicalSlip.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                jp.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewLettersAndForms.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "&rnkid=" + rankid.Text + "');return false;");
                ckl.Attributes.Add("onclick", "parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=4&reportcode=CHECKLIST&employeeid=" + empid.Text + "&vesselid=" + vesselid.Text + "&joindate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&rankid=" + rankid.Text + "&planid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                wgr.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewWorkGearNeededItem.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&newreqid=" + 1 + "&r=1&ntbr=0&ext=0&inact=0&crewplan=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
            }
            else
            {
                sg.Visible = false;
            }

            LinkButton act = (LinkButton)e.Item.FindControl("imgActivity");

            if (drv["FLDNEWAPP"].ToString() == "1")
            {
                lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                act.Attributes["style"] = "opacity: .30;filter: alpha(opacity=30);";
                act.ToolTip = "Seaferer needs to be approved prior proceeding";
            }
            else
            {
                act.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&r=1&ntbr=0&ext=0&inact=0&crewplan=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "&so=1&prde=1');return false;");
                lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                if (drv["FLDOFFSIGNERID"].ToString() != string.Empty)
                {
                    lnkoffsigner.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
                }
                else
                {
                    lnkoffsigner.Visible = false;
                }
            }
          

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPDStatus");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            RadLabel lblPr = (RadLabel)e.Item.FindControl("lblProceedRemarks");
            UserControlToolTip uctpr = (UserControlToolTip)e.Item.FindControl("ucToolTipProceedRemarks");
            uctpr.Position = ToolTipPosition.TopCenter;
            uctpr.TargetControlId = lbtn.ClientID;

            LinkButton lnkOffSigner = (LinkButton)e.Item.FindControl("lnkOffSigner");
            //UserControlToolTip ucToolTipOffSigner = (UserControlToolTip)e.Item.FindControl("ucToolTipOffSigner");
            //ucToolTipOffSigner.Position = ToolTipPosition.TopCenter;
            //ucToolTipOffSigner.TargetControlId = lbtn.ClientID;

            RadLabel lblOffSignerRank = (RadLabel)e.Item.FindControl("lblOffSignerRank");
            //UserControlToolTip ucToolTipOffSignerRank = (UserControlToolTip)e.Item.FindControl("ucToolTipOffSignerRank");
            //ucToolTipOffSignerRank.Position = ToolTipPosition.TopCenter;
            //ucToolTipOffSignerRank.TargetControlId = lbtn.ClientID;

            RadLabel lblRelifDueDate = (RadLabel)e.Item.FindControl("lblRelifDueDate");
            UserControlToolTip ucToolTipRelifdueDate = (UserControlToolTip)e.Item.FindControl("ucToolTipRelifdueDate");
            ucToolTipRelifdueDate.Position = ToolTipPosition.TopCenter;
            ucToolTipRelifdueDate.TargetControlId = lbtn.ClientID;

            LinkButton imgUnallocatedVesselExp = (LinkButton)e.Item.FindControl("imgUnAllocatedVslExp");
            if (imgUnallocatedVesselExp != null)
            {
                imgUnallocatedVesselExp.Visible = SessionUtil.CanAccess(this.ViewState, imgUnallocatedVesselExp.CommandName);
                imgUnallocatedVesselExp.Attributes.Add("onclick", "openNewWindow('CrewUnallocatedVesselExpensesEmployee', '', '" + Session["sitepath"] + "/Crew/CrewUnallocatedVesselExpensesEmployee.aspx?empid=" + empid.Text + "');return false;");
            }
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void CCPMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FILTER"))
        {
            Response.Redirect("CrewChangePlanFilter.aspx", false);
        }
    }
    protected void gvCCPlan_ItemCommand(object sender, GridCommandEventArgs e)
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
}
