using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewFamilyTravelRequest : PhoenixBasePage
{
    private static string strEmployeeId;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ucConfirm.Visible = false;

                PhoenixToolbar toolbar = new PhoenixToolbar();

                if (Request.QueryString["from"].ToString() == "familynok")
                {
                    toolbar.AddButton("Back", "BACK");
                    //toolbar.AddButton("Crew Change", "CREWCHANGE");
					CrewMenu.AccessRights = this.ViewState;
                    CrewMenu.MenuList = toolbar.Show();

                    toolbar = new PhoenixToolbar();
                    toolbar.AddButton("Copy", "COPYBREAKUP");
					MenuBreakUpAssign.AccessRights = this.ViewState;
                    MenuBreakUpAssign.MenuList = toolbar.Show();

                    //toolbar = new PhoenixToolbar();
                    //toolbar.AddButton("Save", "SAVE");
                    //toolbar.AddButton("Generate Travel Req.", "GENERATETRAVELREQ");
                    //MenuGenerateTravel.MenuList = toolbar.Show();

                    strEmployeeId = Filter.CurrentCrewSelection;
                }

                if (Request.QueryString["from"].ToString() == "travel")
                {
                    strEmployeeId = Request.QueryString["empid"].ToString();

                    toolbar.AddButton("Travel Request", "TRAVELREQUEST");
                    CrewMenu.MenuList = toolbar.Show();

                    toolbar = new PhoenixToolbar();
                    toolbar.AddButton("Copy", "COPYBREAKUP");
					MenuBreakUpAssign.AccessRights = this.ViewState;
                    MenuBreakUpAssign.MenuList = toolbar.Show();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["EDITROW"] = "0";
                SetInformation();
            }
            BindData();
            SetPageNavigator();
            BindDataTravelBreakUp();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("CrewFamilyNok.aspx", false);
            }

            if (dce.CommandName.ToUpper().Equals("TRAVELREQUEST"))
            {
                Response.Redirect("CrewTravelRequest.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GenerateTravel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("GENERATETRAVELREQ"))
            {
                ucConfirm.Visible = true;
                ucConfirm.Text = "You will not be able to make any changes. Are you sure you want to generate Travel Request?";
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label lblTravelPlannedYN = (Label)e.Row.FindControl("lblTravelPlannedYN");

                CheckBox chkOffSigner = (CheckBox)e.Row.FindControl("chkOffSigner");
                CheckBox chkOnSigner = (CheckBox)e.Row.FindControl("chkOnSigner");

                if (lblTravelPlannedYN.Text.Trim() == "1")
                {
                    chkOnSigner.Enabled = false;
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

    private bool IsValidTrvelRequest(string strPortId, string dateofcrewchange, string strFamilylist)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(strPortId) == null)
            ucError.ErrorMessage = "The Port is mandatory";

        if (General.GetNullableDateTime(dateofcrewchange) == null)
            ucError.ErrorMessage = "Date of Crew Change is required.";
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ need to work @@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //if (strFamilylist.Trim() == "")
        //{
        //    if (General.GetNullableDateTime(dateofcrewchange) != null && General.GetNullableInteger(strPortId) != null)
        //    {
        //        DataSet ds = PhoenixCrewTravelRequest.SearchTravelRequest(
        //            int.Parse(Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString())
        //            , int.Parse(strPortId)
        //            , General.GetNullableDateTime(dateofcrewchange),
        //            General.GetNullableGuid(""),
        //            null, 0,
        //            1,                      //  Page number
        //            General.ShowRecords(null),
        //            ref iRowCount,
        //            ref iTotalPageCount);

        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            ucError.ErrorMessage = "Please Select Atleast One Employee";
        //        }
        //    }
        //}

        return (!ucError.IsError);
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                PhoenixCrewTravelRequest.UpdateFamilyTravel(
                        int.Parse(strEmployeeId));

                ucStatus.Text = "Travel Request has been Generated.";

                BindData();
                SetPageNavigator();
                BindDataTravelBreakUp();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void SetInformation()
    {
        DataSet ds = PhoenixCrewTravelRequest.FamilyTravelVesselList(int.Parse(strEmployeeId));

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            ViewState["vessel"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            txtDateOfCrewChange.Text = String.Format("{0:dd/MM/yyyy}", General.GetNullableDateTime(ds.Tables[0].Rows[0]["FLDDATEOFCREWCHANGE"].ToString()));
            ddlPort.SelectedSeaport = ds.Tables[0].Rows[0]["FLDCREWCHANGEPORT"].ToString();
        }
        else
        {
            ViewState["vessel"] = "";
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelRequest.SearchFamilyTravelRequest(
                Convert.ToInt32(strEmployeeId),
                General.GetNullableGuid(Request.QueryString["travelid"] != null ? Request.QueryString["travelid"].ToString() : ""),
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCCT.DataSource = ds;
                gvCCT.DataBind();

                //txtPort.Text = ds.Tables[0].Rows[0]["FLDCREWCHANGEPORTNAME"].ToString();
                ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                if (gvCCT.SelectedIndex != -1)
                {
                    gvCCT_SelectedIndexChanging(null, new GridViewSelectEventArgs(gvCCT.SelectedIndex));
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCCT);
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

    protected void BindDataTravelBreakUp()
    {
        try
        {
            DataSet ds = PhoenixCrewTravelRequest.SearchTravelBreakUp(
                General.GetNullableGuid(ViewState["TRAVELREQUESTID"] == null ? "" : ViewState["TRAVELREQUESTID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTBreakUp.DataSource = ds;
                gvCTBreakUp.DataBind();
                MenuBreakUpAssign.Visible = true;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCTBreakUp);
                MenuBreakUpAssign.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCT_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCCT_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["TRAVELREQUESTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestId")).Text;
            BindDataTravelBreakUp();
        }
        else if (e.CommandName.ToUpper() == "INSTRUCTION")
        {
            String scriptpopup = String.Format("javascript:parent.Openpopup('codehelp1', '', '../Common/CommonDiscussion.aspx?travelinstruction=true');");

            string lblDTKey = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;

            PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
            objdiscussion.dtkey = new Guid(lblDTKey);
            objdiscussion.userid = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            objdiscussion.title = PhoenixCrewConstants.TRAVELINSTRUCTIONS;
            objdiscussion.type = PhoenixCrewConstants.TRAVELREQUESTINSTRUCTION;
            PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

            gvCCT.SelectedIndex = nCurrentRow;

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
        }
        else if (e.CommandName.ToUpper() == "DELETE")
        {
            string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestId")).Text;
            string employeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
            string onsigneryn = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOnSignerYN")).Text;

            if (General.GetNullableGuid(requestid) != null)
            {
                PhoenixCrewTravelRequest.DeleteTravelRequest(new Guid(requestid));
                BindData();
                SetPageNavigator();
                gvCCT_SelectedIndexChanging(gvCCT, new GridViewSelectEventArgs(gvCCT.SelectedIndex));
                gvCCT.EditIndex = -1;
                BindDataTravelBreakUp();
            }
        }
    }

    protected void gvCCT_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbtn = (Label)e.Row.FindControl("lblOtherVisa");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucOtherVisaTT");
                if (lbtn != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }

                string onsigneryn = ((Label)e.Row.FindControl("lblOnSignerYN")).Text;

                UserControlDate departuredate = ((UserControlDate)e.Row.FindControl("txtDepartureDate"));
                UserControlDate arrivaldate = ((UserControlDate)e.Row.FindControl("txtArrivalDate"));
                
                Label travelrequestid = ((Label)e.Row.FindControl("lblTravelRequestId"));
                Label lblTravelReqNo = ((Label)e.Row.FindControl("lblTravelReqNo"));

                if (travelrequestid != null && General.GetNullableGuid(travelrequestid.Text) == null)
                {
                    CheckBox chkAssignedTo = (CheckBox)e.Row.FindControl("chkAssignedTo");
                    chkAssignedTo.Enabled = false;
                }

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                ImageButton dbEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                ImageButton cmdAddInstruction = (ImageButton)e.Row.FindControl("cmdAddInstruction");
				if (!SessionUtil.CanAccess(this.ViewState, dbEdit.CommandName)) dbEdit.Visible = false;
                if (lblTravelReqNo != null && lblTravelReqNo.Text != "")
                {
                    db.Visible = false;
                    dbEdit.Visible = false;
                    cmdAddInstruction.Visible = true;
                }
                else if (lblTravelReqNo != null && lblTravelReqNo.Text == "")
                {
                    if (cmdAddInstruction != null) cmdAddInstruction.Visible = false;
                }

                Label lbl = (Label)e.Row.FindControl("lblTravelRequestId");
                HtmlImage img = (HtmlImage)e.Row.FindControl("imgRemarks");
                if (img != null)
                {
                    img.Attributes.Add("onclick", "Openpopup('MoreInfo','','CrewTravelRequestMoreInfo.aspx?id=" + lbl.Text + "')");
                    Label lblR = (Label)e.Row.FindControl("lblRemarks");
                    if (string.IsNullOrEmpty(lblR.Text.Trim())) img.Src = Session["images"] + "/no-remarks.png";
                }
            }
           

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCT_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        if (e.NewSelectedIndex > gvCCT.Rows.Count - 1)
        {
            e.NewSelectedIndex = e.NewSelectedIndex - 1;
        }

        e.NewSelectedIndex = (e.NewSelectedIndex == -1) ? 0 : e.NewSelectedIndex;

        gvCCT.SelectedIndex = e.NewSelectedIndex;
        //gvCCT.EditIndex = -1;

        CheckBox chkAssignedTo = (CheckBox)gvCCT.Rows[e.NewSelectedIndex].FindControl("chkAssignedTo");
        Label lblOnSignerYN = (Label)gvCCT.Rows[e.NewSelectedIndex].FindControl("lblOnSignerYN");
        Label lblTravelRequestId = (Label)gvCCT.Rows[e.NewSelectedIndex].FindControl("lblTravelRequestId");

        if (lblTravelRequestId != null)
        {
            ViewState["TRAVELREQUESTID"] = lblTravelRequestId.Text;

            foreach (GridViewRow gvr in gvCCT.Rows)
            {
                CheckBox chkAssignedToOther = (CheckBox)gvr.FindControl("chkAssignedTo");
                Label lblOnSignerYNOther = (Label)gvr.FindControl("lblOnSignerYN");
                Label lblTravelRequestIdOther = (Label)gvr.FindControl("lblTravelRequestId");

                if (lblOnSignerYNOther.Text == lblOnSignerYN.Text && General.GetNullableGuid(lblTravelRequestIdOther.Text) != null)
                {
                    chkAssignedToOther.Enabled = true;
                }
                else
                {
                    chkAssignedToOther.Enabled = false;
                    chkAssignedToOther.Checked = false;
                }
            }

            if (chkAssignedTo != null)
            {
                chkAssignedTo.Checked = false;
                chkAssignedTo.Enabled = false;
            }
        }
        else
            gvCCT.SelectedIndex = -1;
    }

    protected void gvCCT_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        ViewState["TRAVELREQUESTID"] = ((Label)_gridView.Rows[de.NewEditIndex].FindControl("lblTravelRequestId")).Text;

        BindData();
        SetPageNavigator();

        gvCCT_SelectedIndexChanging(sender, new GridViewSelectEventArgs(de.NewEditIndex));
        BindDataTravelBreakUp();
    }

    protected void gvCCT_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();

            gvCCT_SelectedIndexChanging(sender, new GridViewSelectEventArgs(e.RowIndex));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCT_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            //string employeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text;
            string travelrequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestId")).Text;
            //string crewplanid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewPlanId")).Text;
            //string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text;
            string onsigneryn = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOnSignerYN")).Text;
            //string crewchangeport = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewChangePort")).Text;
            string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDate")).Text;
            string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDate")).Text;
            string origin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtoriginIdEdit")).Text;
            string destination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdedit")).Text;
            //string dateofcrewchange = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDateOfCrewChange")).Text;

            if (!IsValidCrewChangeRequest(onsigneryn, departuredate, arrivaldate, origin, destination))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelRequest.UpdateTravelRequest(
                new Guid(travelrequestid),
                General.GetNullableDateTime(departuredate), General.GetNullableDateTime(arrivaldate),
               General.GetNullableInteger(origin), General.GetNullableInteger(destination), int.Parse(PhoenixCommonRegisters.GetHardCode(1, 185, "PEL")));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
        ViewState["TRAVELREQUESTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestId")).Text;
        BindDataTravelBreakUp();

        gvCCT_SelectedIndexChanging(sender, new GridViewSelectEventArgs(nCurrentRow));
    }

    private bool IsValidCrewChangeRequest(string onsigneryn, string departuredate, string arrivaldate, string origin, string destination)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (onsigneryn == "1")
        {
            if (General.GetNullableDateTime(arrivaldate) == null)
                ucError.ErrorMessage = "Arrival Date is required";

            if (origin.Trim() == "")
                ucError.ErrorMessage = "Origin is required.";
        }
        else
        {
            if (General.GetNullableDateTime(departuredate) == null)
                ucError.ErrorMessage = "Departure Date is required.";

            if (destination.Trim() == "")
                ucError.ErrorMessage = "Destination is required.";
        }

        return (!ucError.IsError);
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCCT.EditIndex = -1;
        gvCCT.SelectedIndex = -1;
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
        gvCCT.SelectedIndex = -1;
        gvCCT.EditIndex = -1;
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
            return true;

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



    protected void BreakUpAssign_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("COPYBREAKUP"))
            {
                StringBuilder stremployeelist = new StringBuilder();
                StringBuilder strtravelrequestlist = new StringBuilder();

                foreach (GridViewRow gvr in gvCCT.Rows)
                {
                    CheckBox chkAssignedTo = (CheckBox)gvr.FindControl("chkAssignedTo");

                    if (chkAssignedTo.Checked)
                    {
                        Label lblEmployeeId = (Label)gvr.FindControl("lblEmployeeId");
                        Label lblCrewPlanId = (Label)gvr.FindControl("lblTravelRequestId");

                        stremployeelist.Append(lblEmployeeId.Text);
                        stremployeelist.Append(",");

                        strtravelrequestlist.Append(lblCrewPlanId.Text);
                        strtravelrequestlist.Append(",");
                    }
                }

                if (stremployeelist.Length > 1)
                {
                    stremployeelist.Remove(stremployeelist.Length - 1, 1);
                }

                if (strtravelrequestlist.Length > 1)
                {
                    strtravelrequestlist.Remove(strtravelrequestlist.Length - 1, 1);
                }

                Label lblCopyingEmployeeId = (Label)gvCCT.Rows[gvCCT.SelectedIndex].FindControl("lblEmployeeId");
                Label lblCopyingRequestId = (Label)gvCCT.Rows[gvCCT.SelectedIndex].FindControl("lblTravelRequestId");
                Label lblVesselId = (Label)gvCCT.Rows[gvCCT.SelectedIndex].FindControl("lblVesselId");

                PhoenixCrewTravelRequest.CopyTravelBreakUp(stremployeelist.ToString()
                    , strtravelrequestlist.ToString()
                    , int.Parse(lblCopyingEmployeeId.Text)
                    , new Guid(lblCopyingRequestId.Text)
                    , int.Parse(lblVesselId.Text));
            }

            gvCCT_SelectedIndexChanging(null, new GridViewSelectEventArgs(gvCCT.SelectedIndex));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvCTBreakUp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
			
            if (e.Row.RowIndex == 0)
            {
                if (db != null)
                    db.Visible = false;
            }

            if (ViewState["EDITROW"].ToString() == "1")
            {
                TextBox txtorigin = (TextBox)e.Row.FindControl("txtOriginIdBreakup");
                TextBox txtdestination = (TextBox)e.Row.FindControl("txtDestinationIdBreakup");
                ImageButton btnorigin = (ImageButton)e.Row.FindControl("btnShowOriginbreakup");
                ImageButton btndestination = (ImageButton)e.Row.FindControl("btnShowDestinationbreakup");
                TextBox txtoriginname = (TextBox)e.Row.FindControl("txtOriginNameBreakup");
                TextBox txtdestinationname = (TextBox)e.Row.FindControl("txtDestinationNameBreakup");

                UserControlDate txtDepartureDate = (UserControlDate)e.Row.FindControl("txtDepartureDate");
                UserControlDate txtArrivalDate = (UserControlDate)e.Row.FindControl("txtArrivalDate");
                UserControlQuick ucPurpose = (UserControlQuick)e.Row.FindControl("ucPurpose");


                TextBox txtoriginold = (TextBox)e.Row.FindControl("txtOriginIdOldBreakup");
                TextBox txtdestinationold = (TextBox)e.Row.FindControl("txtDestinationIdOldBreakup");
                TextBox txtoriginoldname = (TextBox)e.Row.FindControl("txtOriginNameOldBreakup");
                ImageButton btnoriginold = (ImageButton)e.Row.FindControl("btnShowOriginoldbreakup");
                ImageButton btndestinationold = (ImageButton)e.Row.FindControl("btnShowDestinationOldbreakup");

                UserControlDate txtDepartureDateOld = (UserControlDate)e.Row.FindControl("txtDepartureDateOld");
                UserControlDate txtArrivalDateOld = (UserControlDate)e.Row.FindControl("txtArrivalDateOld");
                UserControlQuick ucPurposeOld = (UserControlQuick)e.Row.FindControl("ucPurposeOld");

                ImageButton cmdRowSave = (ImageButton)e.Row.FindControl("cmdRowSave");
                ImageButton cmdSave = (ImageButton)e.Row.FindControl("cmdSave");

                if (txtorigin != null)
                {
                    txtorigin.Visible = false;
                    txtArrivalDate.Visible = false;
                    txtDepartureDate.Visible = false;
                    txtdestination.Visible = false;
                    ucPurpose.Visible = false;
                    btnorigin.Visible = false;
                    btndestination.Visible = false;
                    txtoriginname.Visible = false;
                    txtdestinationname.Visible = false;


                    txtoriginoldname.CssClass = "input_mandatory";
                    txtoriginold.Visible = false;
                    btnoriginold.Visible = false;
                    cmdRowSave.Visible = true;
                    cmdSave.Visible = false;

                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    txtArrivalDateOld.Text = drv["FLDARRIVALDATE"].ToString();
                    txtDepartureDateOld.Text = drv["FLDDEPARTUREDATE"].ToString();
                }
            }
            if (ViewState["EDITROW"].ToString() == "0")
            {
                UserControlAirport txtOriginadd = (UserControlAirport)e.Row.FindControl("ucOrigin");
                UserControlAirport txtDestinationadd = (UserControlAirport)e.Row.FindControl("ucDestination");

                UserControlAirport txtOriginOldadd = (UserControlAirport)e.Row.FindControl("ucOriginOld");
                UserControlAirport txtDestinationOldadd = (UserControlAirport)e.Row.FindControl("ucDestinationOld");

                DataRowView drvnew = (DataRowView)e.Row.DataItem;

                if (txtOriginadd != null)
                {
                    txtOriginOldadd.SelectedAirport = drvnew["FLDORIGINID"].ToString();
                    txtDestinationadd.SelectedAirport = drvnew["FLDDESTINATIONID"].ToString();
                }


            }

            Label lblTravelReqNo = ((Label)e.Row.FindControl("lblTravelReqNo"));

            ImageButton dbEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            ImageButton cmdTravelBreakUp = (ImageButton)e.Row.FindControl("cmdTravelBreakUp");

            if (lblTravelReqNo != null && lblTravelReqNo.Text != "")
            {
                db.Visible = false;
                dbEdit.Visible = false;
                cmdTravelBreakUp.Visible = false;
            }
        }
    }

    protected void gvCTBreakUp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper() == "DELETE")
        {
            string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakUpId")).Text;

            //Request.QueryString["employeelist"] = Request.QueryString["employeelist"].ToString().Replace("," + employeeid, "");
            //Request.QueryString["crewplanidlist"] = Request.QueryString["crewplanidlist"].ToString().Replace("," + crewplanid, "");
            //Request.QueryString["strOnSignerYN"] = "";

            if (General.GetNullableGuid(breakupid) != null)
            {
                PhoenixCrewTravelRequest.DeleteTravelBreakUp(new Guid(breakupid));

                BindDataTravelBreakUp();
            }
        }

        if (e.CommandName.ToUpper() == "SAVE")
        {
            string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakUpIdEdit")).Text;
            string purposeid = ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucPurposeOld")).SelectedQuick;
            string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateOld")).Text;
            string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateOld")).Text;
            string origin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginIdOldBreakup")).Text;
            string destination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdOldBreakup")).Text;

            if (!IsValidTravelBreakUp(departuredate, arrivaldate, origin, destination, purposeid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelRequest.UpdateTravelBreakUp(
                new Guid(breakupid),
                int.Parse(purposeid), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
               General.GetNullableInteger(origin),General.GetNullableInteger(destination));

            _gridView.EditIndex = -1;
            BindDataTravelBreakUp();
        }

        if (e.CommandName.ToUpper() == "EDITROW")
        {
            ViewState["EDITROW"] = "1";
            gvCTBreakUp.EditIndex = nCurrentRow;
            //gvCTBreakUp.SelectedIndex = nCurrentRow;
            BindDataTravelBreakUp();
            //gvCTBreakUp_RowDataBound(sender, new GridViewRowEventArgs(gvCCT.Rows[nCurrentRow]));
        }
    }

    protected void gvCTBreakUp_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        ViewState["EDITROW"] = "0";

        BindDataTravelBreakUp();
    }

    protected void gvCTBreakUp_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataTravelBreakUp();
    }

    protected void gvCTBreakUp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindDataTravelBreakUp();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string employeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeIdEdit")).Text;
            string vesselid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselIdEdit")).Text;
            string travelrequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTravelRequestIdEdit")).Text;
            string breakupid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBreakUpIdEdit")).Text;
            string onsigneryn = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOnSignerYNEdit")).Text;

            string purposeid = ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucPurpose")).SelectedQuick;
            string departuredate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDate")).Text;
            string arrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDate")).Text;
            string origin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginIdBreakup")).Text;
            string destination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdBreakup")).Text;

            string oldpurposeid = ((UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucPurposeOld")).SelectedQuick;
            string olddeparturedate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDepartureDateOld")).Text;
            string oldarrivaldate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtArrivalDateOld")).Text;
            string oldorigin = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOriginIdOldBreakup")).Text;
            string olddestination = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDestinationIdOldBreakup")).Text;           


            if (!IsValidTravelBreakUp(olddeparturedate, oldarrivaldate, oldorigin, olddestination, oldpurposeid
                , departuredate, arrivaldate, origin, destination, purposeid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelRequest.InsertTravelBreakUp(
                new Guid(breakupid), new Guid(travelrequestid), General.GetNullableInteger(employeeid), General.GetNullableInteger(vesselid),
                int.Parse(onsigneryn), int.Parse(oldpurposeid), DateTime.Parse(olddeparturedate),
                DateTime.Parse(oldarrivaldate),General.GetNullableInteger(oldorigin),General.GetNullableInteger(olddestination),
                int.Parse(purposeid), DateTime.Parse(departuredate), DateTime.Parse(arrivaldate),
                General.GetNullableInteger(origin),General.GetNullableInteger(destination));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindDataTravelBreakUp();
    }

    private bool IsValidTravelBreakUp(string olddeparturedate, string oldarrivaldate, string oldorigin, string olddestination, string oldpurpose
        , string departuredate, string arrivaldate, string origin, string destination, string purpose)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(oldarrivaldate) == null)
            ucError.ErrorMessage = "End Date is required";

        if (oldorigin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";

        if (General.GetNullableDateTime(olddeparturedate) == null)
            ucError.ErrorMessage = "Start Date is required.";

        if (olddestination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "End Date is required";

        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Start Date is required.";

        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableInteger(purpose) == null || General.GetNullableInteger(oldpurpose) == null)
            ucError.ErrorMessage = "Purpose is required.";

       

        return (!ucError.IsError);
    }

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination, string purposeid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "End Date is required";

        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Start Date is required.";

        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableInteger(purposeid) == null)
            ucError.ErrorMessage = "Purpose is required.";

        return (!ucError.IsError);
    }
}
