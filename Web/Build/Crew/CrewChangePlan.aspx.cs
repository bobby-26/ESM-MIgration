using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewChangePlan : PhoenixBasePage
{
    string strVesselId = string.Empty;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCCPlan.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
			SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Vessel List", "FILTER");
			CCPMenu.AccessRights = this.ViewState;
            CCPMenu.MenuList = toolbar.Show();
           
            strVesselId = Filter.CurrentCrewChangePlanFilterSelection["ucVessel"];
            PhoenixToolbar SubMenutoolbar = new PhoenixToolbar();
            SubMenutoolbar.AddButton("Save", "SAVE");
            CCPSubMenu.AccessRights = this.ViewState;
            CCPSubMenu.MenuList = SubMenutoolbar.Show();
            if (!IsPostBack)
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Crew/CrewChangePlan.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvCCPlan')", "Print Grid", "icon_print.png", "PRINT");
                MenuCrewChangePlan.AccessRights = this.ViewState;
                MenuCrewChangePlan.MenuList = toolbargrid.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                SetInformation();
                txtTentativeDate.Text = DateTime.Now.Date.ToString();
            }
            BindData();
            SetPageNavigator();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CCPSubMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSave(General.GetNullableString(txtTentativeDate.Text.Trim()), ddlPort.SelectedSeaport))
                {
                    ucError.Visible = true;
                    return;
                }
                CrewChangeDateAndPortUpdate();
                BindData();
                SetPageNavigator();
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
    private void CrewChangeDateAndPortUpdate()
    {
        string sCrewPlanId = "";
        foreach (GridViewRow gv in gvCCPlan.Rows)
        {
            Label lblCrewPlanId = (Label)gv.FindControl("lblCrewPlanId");
            if (lblCrewPlanId.Text.Trim() != string.Empty)
                sCrewPlanId += lblCrewPlanId.Text.Trim() + ",";
        }
        PhoenixCrewChangePlan.CrewChangeDateAndPortUpdate(sCrewPlanId
            , General.GetNullableDateTime(txtTentativeDate.Text.Trim())
            , General.GetNullableInteger(ddlPort.SelectedSeaport));
        ucStatus.Text = "Updated Sucessfully";
        ucStatus.Visible = true;
    }
    protected void ChangePlan_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
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
                DataSet ds = PhoenixCrewChangePlan.SearchCrewChangePlan(int.Parse(strVesselId), byte.Parse(Request.QueryString["access"] != null ? "1" : "0"), sortexpression, sortdirection,
                                                                          (int)ViewState["PAGENUMBER"],
                                                                          General.ShowRecords(null),
                                                                          ref iRowCount,
                                                                          ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Change Plan", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CCPMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FILTER"))
        {
            Response.Redirect("CrewChangePlanFilter.aspx", false);
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
            DataSet ds = PhoenixCrewChangePlan.SearchCrewChangePlan(int.Parse(strVesselId), byte.Parse(Request.QueryString["access"] != null ? "1" : "0"), sortexpression, sortdirection,
                                                                      (int)ViewState["PAGENUMBER"],
                                                                      General.ShowRecords(null),
                                                                      ref iRowCount,
                                                                      ref iTotalPageCount);

            General.SetPrintOptions("gvCCPlan", "Crew Change Plan", alCaptions, alColumns, ds); 

            if (ds.Tables[0].Rows.Count > 0)
            {

                gvCCPlan.DataSource = ds;
                gvCCPlan.DataBind();                
            }
            else
            {                
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCCPlan);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCPlan_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label empid = (Label)e.Row.FindControl("lblEmployeeId");
                Label rankid = (Label)e.Row.FindControl("lblRankId");
                Label vesselid = (Label)e.Row.FindControl("lblVesselId");
                ImageButton sg = (ImageButton)e.Row.FindControl("cmdEdit");
                ImageButton md = (ImageButton)e.Row.FindControl("cmdMedical");
                ImageButton cu = (ImageButton)e.Row.FindControl("cmdCourse");
                ImageButton jp = (ImageButton)e.Row.FindControl("imgJoiningLetter");
                ImageButton ckl = (ImageButton)e.Row.FindControl("cmdChkList");
				if (!SessionUtil.CanAccess(this.ViewState, sg.CommandName)) sg.Visible = false;
				if (!SessionUtil.CanAccess(this.ViewState, md.CommandName)) md.Visible = false;
				if (!SessionUtil.CanAccess(this.ViewState, cu.CommandName)) cu.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, jp.CommandName)) jp.Visible = false;
                if (ckl != null) ckl.Visible = SessionUtil.CanAccess(this.ViewState, ckl.CommandName);
                LinkButton lb = (LinkButton)e.Row.FindControl("lnkEmployee");
                LinkButton lnkoffsigner = (LinkButton)e.Row.FindControl("lnkOffSigner");
                ImageButton con = (ImageButton)e.Row.FindControl("cmdGenContract");
                ImageButton wgr = (ImageButton)e.Row.FindControl("cmdWorkGear");
                if (!string.IsNullOrEmpty(empid.Text))
                {

                    sg.Attributes.Add("onclick", "parent.Openpopup('chml', '', 'CrewMissingLicence.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&jdate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    cu.Attributes.Add("onclick", "parent.Openpopup('chml', '', 'CrewCourseMissing.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&jdate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&rankname=" + drv["FLDRANKNAME"] + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                    con.Attributes.Add("onclick", "parent.Openpopup('chml', '', 'CrewContract.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&date=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&planid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                 //  if (General.GetNullableGuid(drv["FLDMEDREQUESTID"].ToString()) == null)
                    md.Attributes.Add("onclick", "parent.Openpopup('chml', '', 'CrewMedicalSlip.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                     jp.Attributes.Add("onclick", "parent.Openpopup('chml', '', 'CrewLettersAndForms.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "&rnkid=" + rankid.Text + "');return false;");
                     ckl.Attributes.Add("onclick", "parent.Openpopup('CrewPage','','../Reports/ReportsView.aspx?applicationcode=4&reportcode=CHECKLIST&employeeid=" + empid.Text + "&vesselid=" + vesselid.Text + "&joindate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&rankid=" + rankid.Text + "&planid=" + drv["FLDCREWPLANID"].ToString() + "'); return false;");
                }
                else
                {
                    sg.Visible = false;
                }
               
                ImageButton act = (ImageButton)e.Row.FindControl("imgActivity");
                
                if (drv["FLDNEWAPP"].ToString() == "1")
                {
                    lb.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");                    
                    act.Attributes["style"] = "opacity: .30;filter: alpha(opacity=30);";
                    act.ToolTip = "Seaferer needs to be approved prior proceeding";
                }
                else
                {
                    act.Attributes.Add("onclick", "parent.Openpopup('chml', '', 'CrewActivityGeneral.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&r=1&ntbr=0&ext=0&inact=0&crewplan=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "&so=1&prde=1');return false;");
                    lb.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                    if (drv["FLDOFFSIGNERID"].ToString() != string.Empty)
                    {
                        lnkoffsigner.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewPersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
                    }
                    else
                    {
                        lnkoffsigner.Visible = false;
                    }
                }
                if (wgr != null)
                {
                    wgr.Visible = SessionUtil.CanAccess(this.ViewState, wgr.CommandName);
                    wgr.Attributes.Add("onclick", "parent.Openpopup('chml', '', 'CrewWorkGearIssueGeneral.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&r=1&ntbr=0&ext=0&inact=0&crewplan=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "');return false;");
                }
                Label lbtn = (Label)e.Row.FindControl("lblPDStatus");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

                Label lblPr = (Label)e.Row.FindControl("lblProceedRemarks");
                UserControlToolTip uctpr = (UserControlToolTip)e.Row.FindControl("ucToolTipProceedRemarks");
                lblPr.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctpr.ToolTip + "', 'visible');");
                lblPr.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctpr.ToolTip + "', 'hidden');");

                ImageButton imgUnallocatedVesselExp = (ImageButton)e.Row.FindControl("imgUnAllocatedVslExp");

                if (imgUnallocatedVesselExp != null)
                {
                    imgUnallocatedVesselExp.Visible = SessionUtil.CanAccess(this.ViewState, imgUnallocatedVesselExp.CommandName);
                    imgUnallocatedVesselExp.Attributes.Add("onclick", "Openpopup('CrewUnallocatedVesselExpensesEmployee', '', '../Crew/CrewUnallocatedVesselExpensesEmployee.aspx?empid=" + empid.Text + "');return false;");
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }
    protected void gvCCPlan_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();        
    }
    private void SetInformation()
    {
        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(strVesselId));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();          
        }
    }
    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }
    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCCPlan.SelectedIndex = -1;
            gvCCPlan.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    
}
