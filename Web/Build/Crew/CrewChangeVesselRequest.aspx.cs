using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
public partial class CrewChangeVesselRequest : PhoenixBasePage
{
    string strVesselId = string.Empty;
    string strPortId = string.Empty;
    string strDate = string.Empty;

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
            toolbar.AddButton("Port Cost Detail", "DETAIL");
            CCPMenu.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Next", "NEXT");
            CrewChangeRequestMenu.AccessRights = this.ViewState;
            CrewChangeRequestMenu.MenuList = toolbar.Show();

            strVesselId = Filter.CurrentCrewChangePlanFilterSelection["ucVessel"];           

            if (!IsPostBack)
            {
                ViewState["REQUESTID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;

                if (Request.QueryString["REQUESTID"] != null)
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();

                SetInformation();
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

    protected void CCPMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FILTER"))
            {
                Response.Redirect("CrewChangePlanFilter.aspx", false);
            }
            if (dce.CommandName.ToUpper().Equals("DETAIL"))
            {
                Response.Redirect("CrewCostEvaluationRequestGeneral.aspx?REQUESTID="+ViewState["REQUESTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewChangeRequest_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("NEXT"))
            {

                StringBuilder stremployeelist = new StringBuilder();
                StringBuilder strOnSignerYN = new StringBuilder();

                foreach (GridViewRow gvr in gvCCPlan.Rows)
                {
                    CheckBox chkOnSigner = (CheckBox)gvr.FindControl("chkOnSigner");
                    CheckBox chkOffSigner = (CheckBox)gvr.FindControl("chkOffSigner");

                    if (chkOnSigner.Checked)
                    {
                        Label lblOnSigner = (Label)gvr.FindControl("lblEmployeeId");
                        Label lblCrewPlanIdOnSigner = (Label)gvr.FindControl("lblCrewPlanId");

                        stremployeelist.Append(lblOnSigner.Text);
                        stremployeelist.Append(",");

                        strOnSignerYN.Append("1");
                        strOnSignerYN.Append(",");
                    }

                    if (chkOffSigner.Checked)
                    {
                        Label lblOffSigner = (Label)gvr.FindControl("lblOffSignerId");
                        Label lblCrewPlanIdOffSigner = (Label)gvr.FindControl("lblCrewPlanId");

                        stremployeelist.Append(lblOffSigner.Text);
                        stremployeelist.Append(",");

                        strOnSignerYN.Append("0");
                        strOnSignerYN.Append(",");
                    }
                }
                if (stremployeelist.Length > 1)
                {
                    stremployeelist.Remove(stremployeelist.Length - 1, 1);
                }

                if (strOnSignerYN.Length > 1)
                {
                    strOnSignerYN.Remove(strOnSignerYN.Length - 1, 1);
                }

                if (!IsValidTrvelRequest(stremployeelist.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                foreach (GridViewRow gvRow in gvCCPlan.Rows)
                {
                    StringBuilder stremployeelistInsert = new StringBuilder();
                    StringBuilder strOnSignerYNInsert = new StringBuilder();


                    if (stremployeelist.ToString().Trim() != "")
                    {
                        CheckBox chkOnSignerInsert = (CheckBox)gvRow.FindControl("chkOnSigner");
                        CheckBox chkOffSignerInsert = (CheckBox)gvRow.FindControl("chkOffSigner");

                        Label lblCrewPlanIdOnSignerInsert = (Label)gvRow.FindControl("lblCrewPlanId");

                        if (chkOnSignerInsert.Checked)
                        {
                            Label lblOnSignerInsert = (Label)gvRow.FindControl("lblEmployeeId");

                            stremployeelistInsert.Append(lblOnSignerInsert.Text);
                            stremployeelistInsert.Append(",");

                            strOnSignerYNInsert.Append("1");
                            strOnSignerYNInsert.Append(",");
                        }

                        if (chkOffSignerInsert.Checked)
                        {
                            Label lblOffSignerInsert = (Label)gvRow.FindControl("lblOffSignerId");
                            Label lblCrewPlanIdOffSignerInsert = (Label)gvRow.FindControl("lblCrewPlanId");

                            stremployeelistInsert.Append(lblOffSignerInsert.Text);
                            stremployeelistInsert.Append(",");

                            strOnSignerYNInsert.Append("0");
                            strOnSignerYNInsert.Append(",");
                        }

                        if (stremployeelistInsert.Length > 1)
                        {
                            stremployeelistInsert.Remove(stremployeelistInsert.Length - 1, 1);
                        }
                        if (strOnSignerYNInsert.Length > 1)
                        {
                            strOnSignerYNInsert.Remove(strOnSignerYNInsert.Length - 1, 1);
                        }

                        if (stremployeelistInsert.Length > 1 || strOnSignerYNInsert.Length > 1)
                        {
                            DataTable dt = PhoenixCrewCostEvaluationRequest.EditCrewCostEvaluationRequest(new Guid(ViewState["REQUESTID"].ToString()));


                            if (dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.Rows[0];

                                strPortId = dr["FLDSEAPORTID"].ToString();
                                strDate = dr["FLDCREWCHANGEDATE"].ToString();

                                PhoenixCrewTravelRequest.InsertTravelRequest(
                                    stremployeelistInsert.ToString()
                                    , General.GetNullableGuid(lblCrewPlanIdOnSignerInsert.Text.ToString())
                                    , strOnSignerYNInsert.ToString()
                                    , int.Parse(Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString())
                                    , int.Parse(strPortId)
                                    , DateTime.Parse(strDate)
                                    , int.Parse(dr["FLDTRAVELREASONID"].ToString() == "" ? "1" : dr["FLDTRAVELREASONID"].ToString())
                                    , General.GetNullableInteger(dr["FLDCITYID"].ToString()));
                            }
                        }
                    }
                }
                Response.Redirect("CrewChangeTravel.aspx?employeelist=" + stremployeelist.ToString()
                    + "&port=" + strPortId
                    + "&date=" + strDate
                    + "&vessel=" + Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString()
                    + "&from=crewcost"
                    + "&costrequestid="+ViewState["REQUESTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidTrvelRequest(string strEmployeelist)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtCrewChangeDate.Text) == null)
            ucError.ErrorMessage = "Date of Crew Change is required.";

        else if (!DateTime.TryParse(txtCrewChangeDate.Text, out resultDate))
        {
            ucError.ErrorMessage = "Date of Crew Change is not Valid.";
        }

        if (General.GetNullableString(txtPort.Text) == null)
            ucError.ErrorMessage = "Crew Change Port is required";

        //if (General.GetNullableString(txtCrewChangeReason.Text) == null)
        //    ucError.ErrorMessage = "Crew Change Reason is required";

        if (General.GetNullableString(txtCity.Text) == null)
            ucError.ErrorMessage = "City is required";

        if (strEmployeelist.Trim() == "")
        {
            if (General.GetNullableDateTime(txtCrewChangeDate.Text) != null && General.GetNullableInteger(strPortId) != null)
            {
                DataSet ds = PhoenixCrewTravelRequest.SearchTravelRequest(
                    int.Parse(Filter.CurrentCrewChangePlanFilterSelection["ucVessel"].ToString())
                    , int.Parse(strPortId)
                    , General.GetNullableDateTime(txtCrewChangeDate.Text),
                    General.GetNullableGuid(""),
                    null, 0,
                    1,                      //  Page number
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    ucError.ErrorMessage = "Please Select Atleast One Employee";
                }
            }
        }

        return (!ucError.IsError);
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

            NameValueCollection nvc = Filter.CurrentCrewChangePlanFilterSelection;

            DataSet ds = PhoenixCrewChangePlan.SearchCrewChangePlan(
                int.Parse(strVesselId), byte.Parse(Request.QueryString["access"] != null ? "1" : "0")
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount);

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
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    Label empid = (Label)e.Row.FindControl("lblEmployeeId");
                    Label rankid = (Label)e.Row.FindControl("lblRankId");
                    Label vesselid = (Label)e.Row.FindControl("lblVesselId");
                    Label lb = (Label)e.Row.FindControl("lblEmployee");
                    Label lblOffSigner = (Label)e.Row.FindControl("lblOffSignerId");
                    CheckBox chkOffSigner = (CheckBox)e.Row.FindControl("chkOffSigner");
                    Label lblOnSigner = (Label)e.Row.FindControl("lblEmployeeId");
                    CheckBox chkOnSigner = (CheckBox)e.Row.FindControl("chkOnSigner");
                    Label lblOnSignerCrewChange = (Label)e.Row.FindControl("lblOnSignerCrewChange");
                    Label lblOffSignerCrewChange = (Label)e.Row.FindControl("lblOffSignerCrewChange");
                    Label lblOnSignerCrewChangenotreq = (Label)e.Row.FindControl("lblOnSignerCrewChangeNotReq");
                    Label lblOffSignerCrewChangenotreq = (Label)e.Row.FindControl("lblOffSignerCrewChangeNotReq");

                    if (lblOffSigner.Text.Trim() == "")
                    {
                        chkOffSigner.Enabled = false;
                    }
                    if (lblOnSigner.Text.Trim() == "")
                    {
                        chkOnSigner.Enabled = false;
                    }

                    if (lblOffSignerCrewChange.Text.Trim() == "1")
                    {
                        chkOffSigner.Enabled = false;
                    }
                    if (lblOnSignerCrewChange.Text.Trim() == "1")
                    {
                        chkOnSigner.Enabled = false;
                    }
                    if (lblOnSignerCrewChangenotreq.Text.Trim() == "1")
                    {
                        chkOnSigner.Enabled = true;
                        chkOnSigner.Checked = true;
                    }
                    if (lblOffSignerCrewChangenotreq.Text.Trim() == "1")
                    {
                        chkOffSigner.Enabled = true;
                        chkOffSigner.Checked = true;
                    }
                    LinkButton lnkoffsigner = (LinkButton)e.Row.FindControl("lnkOffSigner");
                    if (drv["FLDOFFSIGNERID"].ToString() != string.Empty && lnkoffsigner != null)
                    {
                        lnkoffsigner.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewPersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNERID"].ToString() + "'); return false;");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCCPlan_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "SELECT")
            {
                GridView _gridView = sender as GridView;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                LinkButton lnkEmp = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkEmployee");
                Label empid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId");

                string script = "Openpopup('Filter','','../Crew/CrewPersonalAddress.aspx?empid=" + empid.Text + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        //DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(strVesselId));

        DataTable dt = PhoenixCrewCostEvaluationRequest.EditCrewCostEvaluationRequest(new Guid(ViewState["REQUESTID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtVessel.Text = dr["FLDVESSELNAME"].ToString();
            txtPort.Text = dr["FLDSEAPORTNAME"].ToString();
            txtCrewChangeDate.Text = dr["FLDCREWCHANGEDATE"].ToString();
            txtCity.Text = dr["FLDCITYNAME"].ToString();
            txtCrewChangeReason.Text = dr["FLDCITYNAME"].ToString();
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
