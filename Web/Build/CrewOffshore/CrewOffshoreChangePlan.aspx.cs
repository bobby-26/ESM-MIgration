using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;

public partial class CrewOffshoreChangePlan : PhoenixBasePage
{   
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
                       
            if (!IsPostBack)
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../CrewOffshore/CrewOffshoreChangePlan.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvCCPlan')", "Print Grid", "icon_print.png", "PRINT");
                MenuCrewChangePlan.AccessRights = this.ViewState;
                MenuCrewChangePlan.MenuList = toolbargrid.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;

                if (Request.QueryString["hidemenu"] != null && Request.QueryString["hidemenu"].ToString() != "")
                {
                    ucTitle.ShowMenu = "false";
                }
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

    protected void ChangePlan_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string[] alColumns = { "FLDVESSELNAME", "FLDRANKNAME", "FLDNAME", "FLDEXPECTEDJOINDATE", "FLDSEAPORTNAME", "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDPDSTATUS", "FLDPLANNEDREMARKS", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "Vessel", "Rank", "Name", "Planned Relief", "Planned Port", "Off-Signer", "Off-Signer Rank", "PD Status", "Proceed Remarks", "Relief Due" };

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
                DataTable dt = PhoenixCrewOffshoreCrewChange.CrewOffshoreCrewChangeSearch(string.Empty, sortexpression, sortdirection,
                                                                          (int)ViewState["PAGENUMBER"],
                                                                          General.ShowRecords(null),
                                                                          ref iRowCount,
                                                                          ref iTotalPageCount);
              
                General.ShowExcel("Crew Change Plan", dt, alColumns, alCaptions, sortdirection, sortexpression);
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
        if (dce.CommandName.ToUpper().Equals("RELIEFPLAN"))
        {
            Response.Redirect("../Crew/CrewPlanRelievee.aspx", false);
        }
        else if (dce.CommandName.ToUpper().Equals("PENDINGAPPROVAL"))
        {
            Response.Redirect("../Crew/CrewPD.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
        }
        else if (dce.CommandName.ToUpper().Equals("CREWCHANGE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreChangePlan.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
        }      
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDRANKNAME", "FLDNAME", "FLDEXPECTEDJOINDATE", "FLDSEAPORTNAME", "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDPDSTATUS", "FLDPLANNEDREMARKS", "FLDRELIEFDUEDATE" };
            string[] alCaptions = { "Vessel", "Rank", "Name", "Planned Relief", "Planned Port", "Off-Signer", "Off-Signer Rank", "PD Status", "Proceed Remarks", "Relief Due" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentCrewChangePlanFilterSelection;
            DataTable dt = PhoenixCrewOffshoreCrewChange.CrewOffshoreCrewChangeSearch(string.Empty, sortexpression, sortdirection,
                                                                           (int)ViewState["PAGENUMBER"],
                                                                           General.ShowRecords(null),
                                                                           ref iRowCount,
                                                                           ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCCPlan", "Crew Change Plan", alCaptions, alColumns, ds);

            if (dt.Rows.Count > 0)
            {

                gvCCPlan.DataSource = dt;
                gvCCPlan.DataBind();                
            }
            else
            {                                
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

    protected void gvCCPlan_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;

        GridView _gridView = (GridView)sender;

        try
        {

            if (e.CommandName.ToUpper() == "CREWCHANGEREQUEST")
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucRank", string.Empty);//ucRank.selectedlist);
                criteria.Add("ucVessel", vesselid);
                criteria.Add("ucZone", string.Empty);//ucZone.selectedlist);
                Filter.CurrentCrewChangePlanFilterSelection = criteria;

                Response.Redirect("../Crew/CrewChangeRequest.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty) + (Request.QueryString["access"] != null ? "&access=1" : string.Empty), false);
            }
            else if (e.CommandName.ToUpper() == "APPROVETRAVEL")
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                string crewplanid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewPlanId")).Text;
                PhoenixCrewOffshoreCrewChange.ApproveTravel(General.GetNullableGuid(crewplanid));
                BindData();
                SetPageNavigator();
            }
            else if (e.CommandName.ToUpper() == "APPROVESIGNON")
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                string crewplanid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewPlanId")).Text;
                string employeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
                string rankid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRankId")).Text;
                string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;

                PhoenixCrewOffshoreCrewChange.ApproveSignOn(new Guid(crewplanid),
                    General.GetNullableInteger(vesselid), General.GetNullableInteger(rankid), General.GetNullableInteger(employeeid));
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvCCPlan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataRowView drv = (DataRowView)e.Row.DataItem;

            Label lbtn = (Label)e.Row.FindControl("lblPDStatus");
            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipAddress");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            Label lblPr = (Label)e.Row.FindControl("lblProceedRemarks");
            UserControlToolTip uctpr = (UserControlToolTip)e.Row.FindControl("ucToolTipProceedRemarks");
            lblPr.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctpr.ToolTip + "', 'visible');");
            lblPr.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctpr.ToolTip + "', 'hidden');");

            Label empid = (Label)e.Row.FindControl("lblEmployeeId");
            Label rankid = (Label)e.Row.FindControl("lblRankId");
            Label vesselid = (Label)e.Row.FindControl("lblVesselId");

            ImageButton sg = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton md = (ImageButton)e.Row.FindControl("cmdMedical");
            ImageButton cu = (ImageButton)e.Row.FindControl("cmdCourse");
            LinkButton lb = (LinkButton)e.Row.FindControl("lnkEmployee");
            LinkButton lnkoffsigner = (LinkButton)e.Row.FindControl("lnkOffSigner");            

            if (!SessionUtil.CanAccess(this.ViewState, sg.CommandName)) sg.Visible = false;
            if (!SessionUtil.CanAccess(this.ViewState, md.CommandName)) md.Visible = false;
            if (!SessionUtil.CanAccess(this.ViewState, cu.CommandName)) cu.Visible = false;

            if (!string.IsNullOrEmpty(empid.Text))
            {

                sg.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../CrewOffshore/CrewOffshoreMissingLicence.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&jdate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString() + "');return false;");
                cu.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../CrewOffshore/CrewOffshoreCourseMissing.aspx?empid=" + empid.Text + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&jdate=" + drv["FLDEXPECTEDJOINDATE"].ToString() + "&rankname=" + drv["FLDRANKNAME"] + "&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString() + "');return false;");
                md.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../Crew/CrewMedicalSlip.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "');return false;");                
            }
            else
            {
                sg.Visible = false;
                cu.Visible = false;
                md.Visible = false;
            }

            ImageButton act = (ImageButton)e.Row.FindControl("imgActivity");            

            if (drv["FLDNEWAPP"].ToString() == "1")
            {
                lb.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                act.Attributes["style"] = "opacity: .30;filter: alpha(opacity=30);";
                act.ToolTip = "Seaferer needs to be approved prior proceeding";
            }
            else
            {
                if (!string.IsNullOrEmpty(empid.Text))
                    act.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&vslid=" + vesselid.Text + "&r=1&ntbr=0&ext=0&inact=0&crewplan=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "&so=1&prde=1" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString() + "');return false;");
                else
                    act.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../Crew/CrewActivityGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "&vslid=" + vesselid.Text + "&r=1&ntbr=0&ext=0&inact=0&crewplan=1&crewplanid=" + drv["FLDCREWPLANID"].ToString() + "&so=1&prde=1" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty) + "&trainingmatrixid=" + drv["FLDTRAININGMATRIXID"].ToString() + "');return false;");
                lb.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                if (drv["FLDOFFSIGNERID"].ToString() != string.Empty)
                {
                    lnkoffsigner.Attributes.Add("onclick", "javascript:parent.Openpopup('chml','','../Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
                }
                else
                {
                    lnkoffsigner.Visible = false;
                }
            }

            Label lblPDStatusID = (Label)e.Row.FindControl("lblPDStatusID");
            ImageButton cmdIniTravel = (ImageButton)e.Row.FindControl("cmdIniTravel");
            ImageButton cmdApproveTravel = (ImageButton)e.Row.FindControl("cmdApproveTravel");
            ImageButton cmdApproveSignOn = (ImageButton)e.Row.FindControl("cmdApproveSignOn");

            if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "AWA")) // Proposed
            {
                if (cmdApproveTravel != null) cmdApproveTravel.Visible = true;
                if (cmdIniTravel != null) cmdIniTravel.Visible = false;
                if (cmdApproveSignOn != null) cmdApproveSignOn.Visible = false;
                if (act != null) act.Visible = false;
            }
            else if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "AFT")) // Approved for Travel
            {
                if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                if (cmdIniTravel != null) cmdIniTravel.Visible = true;
                if (!string.IsNullOrEmpty(empid.Text))
                {
                    if (cmdApproveSignOn != null) cmdApproveSignOn.Visible = true;
                }
                if (act != null) act.Visible = false;
            }
            //if (lblPDStatusID != null && lblPDStatusID.Text == PhoenixCommonRegisters.GetHardCode(1, 99, "AFS")) // Approved for sign on
            else
            {
                if (cmdApproveTravel != null) cmdApproveTravel.Visible = false;
                if (cmdApproveSignOn != null) cmdApproveSignOn.Visible = false;
                if (!string.IsNullOrEmpty(empid.Text))
                {
                    if (act != null) act.Visible = true;
                }
                if (cmdIniTravel != null) cmdIniTravel.Visible = true;
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
