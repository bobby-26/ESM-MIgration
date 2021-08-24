using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewPlan : PhoenixBasePage
{
    string strEmployeeId = string.Empty;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvPlan.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        BindOffSigner();
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                ViewState["CVSL"] = -1;
                ViewState["CRNK"] = -1;
                SetEmployeePrimaryDetails();
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
    protected void gvPlan_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToString().ToUpper() == "ADD")
        {

            try
            {
                _gridView.EditIndex = -1;
                ViewState["CURRENTINDEX"] = -1;
                string vesselid = ((UserControlVessel)_gridView.FooterRow.FindControl("ddlVesselAdd")).SelectedVessel;
                string rankid = ((UserControlRank)_gridView.FooterRow.FindControl("ddlRankAdd")).SelectedRank;
                string offsignerid = ((UserControlCrewOnboard)_gridView.FooterRow.FindControl("ddlOffSignerAdd")).SelectedCrew;
                string expjoindate = ((UserControlDate)_gridView.FooterRow.FindControl("txtExpJoinDateAdd")).Text;
                string offsignerremarks = ((TextBox)_gridView.FooterRow.FindControl("txtOffSignerRemarkAdd")).Text;
                string seaportid = ((UserControlSeaport)_gridView.FooterRow.FindControl("ddlSeaPortAdd")).SelectedSeaport;
                string relieverremarks = ((TextBox)_gridView.FooterRow.FindControl("txtRelieverRemarkAdd")).Text;

                if (!IsPlanValid(vesselid, rankid, expjoindate, seaportid))
                {
                    ucError.Visible = true;
                    if (_gridView.EditIndex > -1)
                    {
                        _gridView.EditIndex = -1;                        
                    }
                    return;
                }              
                PhoenixCrewPlanning.InsertCrewPlan(int.Parse(strEmployeeId),int.Parse(vesselid),int.Parse(rankid),General.GetNullableInteger(offsignerid),DateTime.Parse(expjoindate)
                                                                , int.Parse(seaportid), offsignerremarks, relieverremarks);
                BindData();
                SetPageNavigator();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
    }
    protected void gvPlan_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["onclick"] = _jsDouble;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            if (e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) || e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                e.Row.Attributes["onclick"] = "";
            }
            UserControlVessel ucVessel = (UserControlVessel)e.Row.FindControl("ddlVesselEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ucVessel != null) ucVessel.SelectedVessel = drv["FLDVESSELID"].ToString();


            UserControlCrewOnboard ucCrewOnboard = (UserControlCrewOnboard)e.Row.FindControl("ddlOffSignerEdit");
            DataRowView drvCrewOnboard = (DataRowView)e.Row.DataItem;
            if (ucCrewOnboard != null) ucCrewOnboard.SelectedCrew = drvCrewOnboard["FLDOFFSIGNERID"].ToString();

            UserControlRank ucRank = (UserControlRank)e.Row.FindControl("ddlRankEdit");
            DataRowView drvRank = (DataRowView)e.Row.DataItem;
            if (ucRank != null) ucRank.SelectedRank = drvRank["FLDRANKID"].ToString();

            UserControlSeaport ucSeaPort = (UserControlSeaport)e.Row.FindControl("ddlSeaPortEdit");
            DataRowView drvSeaPort = (DataRowView)e.Row.DataItem;
            if (ucSeaPort != null) ucSeaPort.SelectedSeaport = drvSeaPort["FLDSEAPORTID"].ToString();
        }
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    UserControlCrewOnboard cob = e.Row.FindControl("ddlOffSignerAdd") as UserControlCrewOnboard;
        //    UserControlRank rank = e.Row.FindControl("ddlRankAdd") as UserControlRank;
        //    UserControlVessel vsl = e.Row.FindControl("ddlVesselAdd") as UserControlVessel;
        //    cob.OnboardList = PhoenixCrewPlanning.ListCrewOnboard(General.GetNullableInteger(vsl.SelectedVessel), General.GetNullableInteger(rank.SelectedRank));
        //}
    }
    protected void gvPlan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
        ViewState["CURRENTINDEX"] = -1;
        ViewState["CVSL"] = -1;
        ViewState["CRNK"] = -1;
    }
    protected void gvPlan_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            ViewState["CURRENTINDEX"] = e.NewEditIndex;
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex >= 0)
            {
                int nCurrentRow = _gridView.EditIndex;
                string vesselid = ((UserControlVessel)_gridView.Rows[nCurrentRow].FindControl("ddlVesselEdit")).SelectedVessel;
                string rankid = ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ddlRankEdit")).SelectedRank;
                string offsignerid = ((UserControlCrewOnboard)_gridView.Rows[nCurrentRow].FindControl("ddlOffSignerEdit")).SelectedCrew;
                string expjoindate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpJoinDateEdit")).Text;
                string offsignerremarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOffSignerRemarkEdit")).Text;
                string seaportid = ((UserControlSeaport)_gridView.Rows[nCurrentRow].FindControl("ddlSeaPortEdit")).SelectedSeaport;
                string relieverremarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRelieverRemarkEdit")).Text;               
                if (!IsPlanValid(vesselid, rankid, expjoindate, seaportid))
                {
                    ucError.Visible = true;
                    return;
                }
            }            
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);
            _gridView.EditIndex = e.NewEditIndex;
            
            BindData();
            ViewState["CVSL"] = ((UserControlVessel)_gridView.Rows[e.NewEditIndex].FindControl("ddlVesselEdit")).SelectedVessel;
            ViewState["CRNK"] = ((UserControlRank)_gridView.Rows[e.NewEditIndex].FindControl("ddlRankEdit")).SelectedRank;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPlan_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string vesselid = ((UserControlVessel)_gridView.Rows[nCurrentRow].FindControl("ddlVesselEdit")).SelectedVessel;
            string rankid = ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ddlRankEdit")).SelectedRank;
            string offsignerid = ((UserControlCrewOnboard)_gridView.Rows[nCurrentRow].FindControl("ddlOffSignerEdit")).SelectedCrew;
            string expjoindate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpJoinDateEdit")).Text;
            string offsignerremarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOffSignerRemarkEdit")).Text;
            string seaportid = ((UserControlSeaport)_gridView.Rows[nCurrentRow].FindControl("ddlSeaPortEdit")).SelectedSeaport;
            string relieverremarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRelieverRemarkEdit")).Text;
            string crewplanid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewPlanIdEdit")).Text;
            if (!IsPlanValid(vesselid, rankid, expjoindate, seaportid))
            {
                ucError.Visible = true;                
                return;
            }

            PhoenixCrewPlanning.UpdateCrewPlan(new Guid(crewplanid), int.Parse(vesselid), General.GetNullableInteger(offsignerid), DateTime.Parse(expjoindate)
                                                            , General.GetNullableInteger(seaportid), offsignerremarks, relieverremarks,null,null);
            ViewState["CURRENTINDEX"] = -1;
            ViewState["CVSL"] = -1;
            ViewState["CRNK"] = -1;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        SetPageNavigator();
        BindData();
    }
    protected void gvPlan_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string crewplanid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCrewPlanId")).Text;
            string empid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmpId")).Text;
            PhoenixCrewPlanning.DeleteCrewPlan(new Guid(crewplanid), int.Parse(empid));
            BindData();
            SetPageNavigator();
            ViewState["CURRENTINDEX"] = -1;
            ViewState["CVSL"] = -1;
            ViewState["CRNK"] = -1;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                
                if (dt.Rows[0]["FLDPRESENTVESSELID"].ToString() != string.Empty)
                {
                    lblDateStatus.Text = "Relief Due";
                    txtDOA.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDRELEFDUEDATE"].ToString()));
                }
                else
                {
                    lblDateStatus.Text = "DOA";
                    txtDOA.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDDOA"].ToString()));
                }
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewPlanning.SearchCrewPlan(Convert.ToInt32(strEmployeeId), sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      General.ShowRecords(null),
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {

                gvPlan.DataSource = ds;
                gvPlan.DataBind();                
            }
            else
            {                
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPlan);
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
    private bool IsPlanValid(string strVesselId, string strRankId, string strJoinDate, string strSeaPort)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;
        if (strVesselId == "Dummy") strVesselId = string.Empty;
        if (strRankId == "Dummy") strRankId = string.Empty;
        if (strSeaPort == "Dummy") strSeaPort = string.Empty;

        if (strVesselId.Trim() == "")
            ucError.ErrorMessage = "Vessel Name is required";

        if (strRankId.Trim() == "")
            ucError.ErrorMessage = "Rank is required";

        //if (strOffSigner.Trim() == "")
        //    ucError.ErrorMessage = "Off-Signer is required";

        if (strJoinDate == null || !DateTime.TryParse(strJoinDate, out  resultdate))
            ucError.ErrorMessage = "Exp.Join Date is required";
        else if (DateTime.TryParse(strJoinDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Exp.Join Date should be later than current date";
        }
        if (strSeaPort.Trim() == "")
            ucError.ErrorMessage = "Sea Port is required";

        return (!ucError.IsError);

    }
    protected void BindOffSigner()
    {
        UserControlVessel vsl = new UserControlVessel();
        UserControlRank rank = new UserControlRank();
        UserControlCrewOnboard cob = new UserControlCrewOnboard();
        
        int? VesselId = null;
        int? RankId = null;
        if (ViewState["CURRENTINDEX"] != null && (int)ViewState["CURRENTINDEX"] != -1)
        {
            vsl = gvPlan.Rows[(int)ViewState["CURRENTINDEX"]].FindControl("ddlVesselEdit") as UserControlVessel;
            rank = gvPlan.Rows[(int)ViewState["CURRENTINDEX"]].FindControl("ddlRankEdit") as UserControlRank;
            VesselId = General.GetNullableInteger(vsl.SelectedVessel);
            RankId = General.GetNullableInteger(rank.SelectedRank);
            cob = gvPlan.Rows[(int)ViewState["CURRENTINDEX"]].FindControl("ddlOffSignerEdit") as UserControlCrewOnboard;
        }
        else
        {
            vsl = gvPlan.FooterRow.FindControl("ddlVesselAdd") as UserControlVessel;
            rank = gvPlan.FooterRow.FindControl("ddlRankAdd") as UserControlRank;
            VesselId = General.GetNullableInteger(vsl.SelectedVessel);
            RankId = General.GetNullableInteger(rank.SelectedRank);
            cob = gvPlan.FooterRow.FindControl("ddlOffSignerAdd") as UserControlCrewOnboard;            
        }
        
        if (VesselId.HasValue)
        {
            bool bind = false;
            if (int.Parse(ViewState["CVSL"].ToString()) != VesselId.Value)
            {
                ViewState["CVSL"] = VesselId.Value;
                bind = true;
            }
            if (RankId.HasValue && int.Parse(ViewState["CRNK"].ToString()) != RankId.Value)
            {
                ViewState["CRNK"] = RankId.Value;
                bind = true;
            }
            if (bind)
                cob.OnboardList = PhoenixCrewManagement.ListCrewOnboard(VesselId.Value, RankId);
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
            gvPlan.SelectedIndex = -1;
            gvPlan.EditIndex = -1;
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
