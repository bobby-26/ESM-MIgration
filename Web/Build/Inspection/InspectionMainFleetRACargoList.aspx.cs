using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;

public partial class InspectionMainFleetRACargoList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ucConfirmApprove.Visible = false;
        ucConfirmIssue.Visible = false;
        ucConfirmRevision.Visible = false;
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Inspection/InspectionMainFleetRACargoList.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvRiskAssessmentCargo')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Inspection/InspectionMainFleetRACargoList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
        toolbar.AddImageButton("../Inspection/InspectionMainFleetRACargoList.aspx", "New Template", "add.png", "ADD");
        MenuCargo.AccessRights = this.ViewState;
        MenuCargo.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["REFID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["COMPANYID"] = "";

            NameValueCollection nvcCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvcCompany.Get("QMS") != null && nvcCompany.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvcCompany.Get("QMS");
            }
            if (Request.QueryString["filter"] != null)
            {
                if (Request.QueryString["filter"].ToString() == "0")
                {
                    Filter.CurrentCargoRAFilter = null;
                }
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH");

            MenuCargoMain.AccessRights = this.ViewState;
            MenuCargoMain.MenuList = toolbarmain.Show();

        }

        BindData();
        SetPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string vesselid = "";
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
        string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Intended Work", "Type", "Activity / Conditions", "Revision No", "Status", "Approved By" };

        NameValueCollection nvc = Filter.CurrentCargoRAFilter;

        if (Filter.CurrentCargoRAFilter == null)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        DataSet ds = PhoenixInspectionRiskAssessmentCargo.PhoenixInspectionMainFleetRiskAssessmentCargoSearch(
                     nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , sortexpression, sortdirection
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    ,null
                    );

        General.SetPrintOptions("gvRiskAssessmentCargo", "Cargo", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRiskAssessmentCargo.DataSource = ds;
            gvRiskAssessmentCargo.DataBind();

            if (Filter.CurrentSelectedCargoRA == null)
            {
                Filter.CurrentSelectedCargoRA = ds.Tables[0].Rows[0]["FLDRISKASSESSMENTCARGOID"].ToString();
                gvRiskAssessmentCargo.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRiskAssessmentCargo);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string vesselid = "";
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            string[] alColumns = { "FLDNUMBER", "FLDVESSELNAME", "FLDDATE", "FLDINTENDEDWORKDATE", "FLDTYPE", "FLDACTIVITYCONDITIONS", "FLDREVISIONNO", "FLDSTATUSNAME", "FLDAPPROVEDBY" };
            string[] alCaptions = { "Ref Number", "Vessel", "Prepared", "Intended Work", "Type", "Activity / Conditions", "Revision No", "Status", "Approved By" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentCargoRAFilter;

            if (Filter.CurrentCargoRAFilter == null)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataSet ds = PhoenixInspectionRiskAssessmentCargo.PhoenixInspectionMainFleetRiskAssessmentCargoSearch(
                      nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkFrom"]) : null
                    , nvc != null ? General.GetNullableDateTime(nvc["ucDateIntendedWorkTo"]) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ucVessel"]) : General.GetNullableInteger(vesselid)
                    , sortexpression, sortdirection
                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , nvc != null ? General.GetNullableString(nvc["txtRefNo"].ToString()) : null
                    , nvc != null ? General.GetNullableInteger(nvc["ddlStatus"]) : null
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , nvc != null ? General.GetNullableInteger(nvc["ucTechFleet"]) : null
                    ,null
                    );

            General.ShowExcel("Cargo", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCargo_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


        if (dce.CommandName.ToUpper().Equals("ADD"))
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
            {
                Response.Redirect("../Inspection/InspectionRACargoDetails.aspx?status=", false);
            }
            else
            {
                Response.Redirect("../Inspection/InspectionRACargo.aspx?status=", false);
            }
        }
        else if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentCargoRAFilter = null;
            ViewState["PAGENUMBER"] = 1;
            txtnopage.Text = "";
            BindData();
            SetPageNavigator();
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

    protected void gvRiskAssessmentCargo_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvRiskAssessmentCargo.SelectedIndex = -1;
        gvRiskAssessmentCargo.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvRiskAssessmentCargo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label lblCargoID = (Label)e.Row.FindControl("lblRiskAssessmentCargoID");
                Label lblVesselid = (Label)e.Row.FindControl("lblVesselid");
                Label lblInstallcode = (Label)e.Row.FindControl("lblInstallcode");
                Label lblIsCreatedByOffice = (Label)e.Row.FindControl("lblIsCreatedByOffice");

                Image imgFlag = e.Row.FindControl("imgFlag") as Image;
                if (imgFlag != null && drv["FLDINTENDEDWORKDATEDUEYN"].ToString().Equals("3"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                    imgFlag.ToolTip = "Overdue";
                }
                else if (imgFlag != null && drv["FLDINTENDEDWORKDATEDUEYN"].ToString().Equals("2"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                    imgFlag.ToolTip = "Due within 2 Weeks";
                }
                else
                {
                    if (imgFlag != null) imgFlag.Visible = false;
                }

                if (drv["FLDCOPIEDFROMSTANDARDTEMPLATE"].ToString() == "1")
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "green.png";
                    imgFlag.ToolTip = "Copied from Standard Template";
                }


                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                ImageButton imgApprove = (ImageButton)e.Row.FindControl("imgApprove");
                ImageButton imgIssue = (ImageButton)e.Row.FindControl("imgIssue");
                ImageButton imgrevision = (ImageButton)e.Row.FindControl("imgrevision");
                ImageButton imgCopyTemplate = (ImageButton)e.Row.FindControl("imgCopyTemplate");
                ImageButton cmdRevision = (ImageButton)e.Row.FindControl("cmdRevision");
                Label lblStatusid = (Label)e.Row.FindControl("lblStatus");
                ImageButton imgProposeTemplate = (ImageButton)e.Row.FindControl("imgProposeTemplate");

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    imgProposeTemplate.Visible = true;
                }
                else
                {
                    imgProposeTemplate.Visible = false;
                }

                if (lblInstallcode != null)
                {
                    if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) > 0) // when RA is approved in vessel.
                    {
                        if (imgApprove != null)
                        {
                            imgApprove.ToolTip = "Emergency Override";
                            imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRAApprovalRemarks.aspx?RATEMPLATEID=" + lblCargoID.Text + "&TYPE=5','medium'); return true;");
                        }
                    }
                    else if (General.GetNullableInteger(lblInstallcode.Text) != null && int.Parse(lblInstallcode.Text) == 0) // when vessel created RA approved in office.
                    {
                        if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text != "" && int.Parse(lblIsCreatedByOffice.Text) > 0)
                        {
                            if (imgApprove != null)
                            {
                                imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionMainFleetRAApproval.aspx?RATEMPLATEID=" + lblCargoID.Text + "&TYPE=5','large'); return true;");
                            }
                        }
                    }
                }

                if (imgIssue != null)
                {
                    imgIssue.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRAMainFleetOfficeApprovalRemarks.aspx?RATEMPLATEID=" + lblCargoID.Text + "&TYPE=5','medium'); return true;");
                }
                if (imgCopyTemplate != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRACargoDetails.aspx?genericid=" + lblCargoID.Text + "&CopyType=1" + "'); return true;");
                    }
                    else
                    {
                        imgCopyTemplate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRACargo.aspx?genericid=" + lblCargoID.Text + "&CopyType=1" + "'); return true;");
                    }
                }

                if (imgProposeTemplate != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRACargoDetails.aspx?genericid=" + lblCargoID.Text + "&CopyType=2" + "'); return true;");
                    }
                    else
                    {
                        imgProposeTemplate.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Inspection/InspectionRACargo.aspx?genericid=" + lblCargoID.Text + "&CopyType=2" + "'); return true;");
                    }
                }

                //imgCopy.Visible = false;
                imgrevision.Visible = false;
                imgApprove.Visible = false;
                imgIssue.Visible = false;
                imgCopyTemplate.Visible = false;

                if (lblIsCreatedByOffice != null && lblIsCreatedByOffice.Text == "0")
                {
                    if (cmdRevision != null) cmdRevision.Visible = true;
                }
                else
                {
                    if (cmdRevision != null) cmdRevision.Visible = false;
                }

                if (lblStatusid.Text == "1")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = true;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = false;
                }
                else if (lblStatusid.Text == "2")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = false;
                    imgIssue.Visible = true;
                    imgCopyTemplate.Visible = false;
                }
                else if ((lblStatusid.Text == "4") && (lblVesselid.Text == "0"))
                {
                    imgIssue.Visible = true;
                }
                else if (lblStatusid.Text == "3")
                {
                    //imgCopy.Visible = true;
                    if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        imgrevision.Visible = true;
                    else
                        imgrevision.Visible = false;
                    imgApprove.Visible = false;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = true;
                }
                else if (lblStatusid.Text == "4")
                {
                    //imgCopy.Visible = false;
                    imgrevision.Visible = false;
                    imgApprove.Visible = true;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = false;
                }
                else if (lblStatusid.Text == "5")
                {
                    //imgCopy.Visible = true;
                    if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        imgrevision.Visible = true;
                    else
                        imgrevision.Visible = false;
                    imgApprove.Visible = false;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = true;
                }

                else if (lblStatusid.Text == "6")
                {
                    //imgCopy.Visible = true;
                    if (lblVesselid != null && lblVesselid.Text == "0" && drv["FLDACTIVEYN"].ToString() == "1" && PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                        imgrevision.Visible = true;
                    else
                        imgrevision.Visible = false;
                    imgApprove.Visible = false;
                    imgIssue.Visible = false;
                    imgCopyTemplate.Visible = true;
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Use RA";
                }
                else
                {
                    if (imgCopyTemplate != null) imgCopyTemplate.ToolTip = "Copy RA";
                }

                if (lblVesselid != null && lblVesselid.Text == "0")
                {
                    if (imgApprove != null) imgApprove.ToolTip = "Request for Approval";
                }
                else
                {
                    if (imgApprove != null) imgApprove.ToolTip = "Approve/Disapprove";
                }

                ImageButton cmdRACargo = (ImageButton)e.Row.FindControl("cmdRACargo");
                if (cmdRACargo != null)
                {
                    cmdRACargo.Visible = SessionUtil.CanAccess(this.ViewState, cmdRACargo.CommandName);
                    //cmdRACargo.Visible = false;

                    //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    //{
                    //    cmdRACargo.Attributes.Add("onclick", "Openpopup('RACargo', '', '../Reports/ReportsView.aspx?applicationcode=9&reportcode=RAGENERIC&genericid=" + lblCargoID.Text + "&showmenu=0&showword=NO&showexcel=NO&showpdf=NO&showprint=YES');return false;");
                    //}
                    //else
                    //{
                    cmdRACargo.Attributes.Add("onclick", "Openpopup('RACargo', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RACARGO&genericid=" + lblCargoID.Text + "&showmenu=0&showexcel=NO');return true;");
                    //}
                }

                if (cmdRevision != null)
                {
                    cmdRevision.Attributes.Add("onclick", "Openpopup('RACargoRevision', '', '../Inspection/InspectionRACargoRevisionList.aspx?genericid=" + lblCargoID.Text + "');return true;");
                }

                Label lblWorkActivity = (Label)e.Row.FindControl("lblWorkActivity");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucWorkActivity");
                if (uct != null)
                {
                    lblWorkActivity.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lblWorkActivity.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }

                Label lblApprovedBy = (Label)e.Row.FindControl("lblApprovedBy");
                UserControlToolTip ucApprovedBy = (UserControlToolTip)e.Row.FindControl("ucApprovedBy");
                if (ucApprovedBy != null)
                {
                    lblApprovedBy.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucApprovedBy.ToolTip + "', 'visible');");
                    lblApprovedBy.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucApprovedBy.ToolTip + "', 'hidden');");
                }

                if (imgApprove != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgApprove.CommandName)) imgApprove.Visible = false;
                }
                if (imgIssue != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgIssue.CommandName)) imgIssue.Visible = false;
                }
                if (imgrevision != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgrevision.CommandName)) imgrevision.Visible = false;
                }
                if (cmdRevision != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRevision.CommandName)) cmdRevision.Visible = false;
                }
                if (cmdRACargo != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdRACargo.CommandName)) cmdRACargo.Visible = false;
                }
                if (imgCopyTemplate != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgCopyTemplate.CommandName)) imgCopyTemplate.Visible = false;
                }
                if (imgProposeTemplate != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgProposeTemplate.CommandName)) imgProposeTemplate.Visible = false;
                }
            }
        }
    }

    protected void gvRiskAssessmentCargo_RowCommand(object sender, GridViewCommandEventArgs gce)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (gce.CommandName.ToUpper().Equals("SORT"))
                return;
            int nRow = int.Parse(gce.CommandArgument.ToString());
            Label lbl = (Label)_gridView.Rows[nRow].FindControl("lblRiskAssessmentCargoID");
            Label lblstatus = (Label)_gridView.Rows[nRow].FindControl("lblStatus");
            Label lblInstallcode = (Label)_gridView.Rows[nRow].FindControl("lblInstallcode");
            Label lblIsCreatedByOffice = (Label)_gridView.Rows[nRow].FindControl("lblIsCreatedByOffice");

            if (gce.CommandName.ToUpper().Equals("EDITROW"))
            {
                BindPageURL(nRow);
                SetRowSelection();
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    Response.Redirect("../Inspection/InspectionRACargoDetails.aspx?genericid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
                else
                {
                    Response.Redirect("../Inspection/InspectionRACargo.aspx?genericid=" + lbl.Text + "&status=" + lblstatus.Text, false);
                }
            }

            if (gce.CommandName.ToUpper().Equals("APPROVE"))
            {
                BindPageURL(nRow);
                SetRowSelection();
                if (lblInstallcode != null && lblInstallcode.Text == "0")
                {
                    //if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) > 0)
                    //{
                    //    ViewState["GENERICID"] = lbl.Text;
                    //    ucConfirmApprove.Visible = true;
                    //    ucConfirmApprove.Text = "Once the RA is approved, It cannot be edited. Are you sure to continue?";
                    //    return;
                    //}
                    if (lblIsCreatedByOffice != null && int.Parse(lblIsCreatedByOffice.Text) == 0)
                    {
                        PhoenixInspectionRiskAssessmentCargo.MainFleetApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(lbl.Text));
                        ucStatus.Text = "Approved Successfully";
                    }
                }
            }
            if (gce.CommandName.ToUpper().Equals("ISSUE"))
            {
                BindPageURL(nRow);
                SetRowSelection();
                //ViewState["GENERICID"] = lbl.Text;
                //ucConfirmIssue.Visible = true;
                //ucConfirmIssue.Text = "Once the RA is issued, It cannot be edited. Are you sure to continue?";
                //return;		
            }
            if (gce.CommandName.ToUpper().Equals("REVISION"))
            {
                BindPageURL(nRow);
                SetRowSelection();
                ViewState["GENERICID"] = lbl.Text;
                ucConfirmRevision.Visible = true;
                ucConfirmRevision.Text = "Are you sure you want to revise this RA.?";
                return;
            }

            if (gce.CommandName.ToUpper().Equals("VIEWREVISION"))
            {
                BindPageURL(nRow);
                SetRowSelection();
            }
            if (gce.CommandName.ToUpper().Equals("RAGENERIC"))
            {
                BindPageURL(nRow);
                SetRowSelection();
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
    protected void btnConfirmApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargo.ApproveCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    ucStatus.Text = "Approved Successfully";
                    BindData();
                    SetPageNavigator();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmIssue_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargo.IssueCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["GENERICID"].ToString()));
                    ucStatus.Text = "Issued Successfully";
                    BindData();
                    SetPageNavigator();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRiskAssessmentCargo.EditIndex = -1;
        gvRiskAssessmentCargo.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            Label lblRiskAssessmentCargoID = (Label)gvRiskAssessmentCargo.Rows[rowindex].FindControl("lblRiskAssessmentCargoID");
            if (lblRiskAssessmentCargoID != null)
            {
                Filter.CurrentSelectedCargoRA = lblRiskAssessmentCargoID.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvRiskAssessmentCargo.SelectedIndex = -1;
        for (int i = 0; i < gvRiskAssessmentCargo.Rows.Count; i++)
        {
            if (gvRiskAssessmentCargo.DataKeys[i].Value.ToString().Equals(Filter.CurrentSelectedCargoRA.ToString()))
            {
                gvRiskAssessmentCargo.SelectedIndex = i;
            }
        }
    }

    protected void btnConfirmRevision_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["GENERICID"] != null && ViewState["GENERICID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentCargo.ReviseCargo(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                    new Guid(ViewState["GENERICID"].ToString()));
                    ucStatus.Text = "RA is revised.";
                    BindData();
                    SetPageNavigator();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuCargoMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionMainFleetNonRoutineRAFilter.aspx", false);
        }
    }
}
