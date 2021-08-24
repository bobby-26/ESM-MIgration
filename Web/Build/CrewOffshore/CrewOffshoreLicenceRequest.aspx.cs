using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;

public partial class CrewOffshoreLicenceRequest : PhoenixBasePage
{
    private string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                strEmployeeId = Filter.CurrentCrewSelection;
            else if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                strEmployeeId = Filter.CurrentNewApplicantSelection;

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreLicenceRequest.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), "Search", "search.png", "SEARCH");
            MenuCrewLicence.AccessRights = this.ViewState;
            MenuCrewLicence.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Initiate Licence Req", "REQUEST");
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["empid"] = strEmployeeId;
                ViewState["vsltype"] = string.Empty;
                ViewState["Charterer"] = string.Empty;

                rblPaymentBy.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 89, 0, "CMP,EMP");
                rblPaymentBy.DataTextField = "FLDHARDNAME";
                rblPaymentBy.DataValueField = "FLDHARDCODE";
                rblPaymentBy.DataBind();

                if (Request.QueryString["personalmaster"] != null && Request.QueryString["personalmaster"].ToString() != "")
                    SetEmployeePrimaryDetails();

                if (Request.QueryString["newapplicant"] != null && Request.QueryString["newapplicant"].ToString() != "")
                    SetNewApplicantPrimaryDetails();

                ucVessel.Enabled = true;

                if (Request.QueryString["rankid"] != null && Request.QueryString["rankid"].ToString() != "")
                {
                    ddlRank.SelectedValue = Request.QueryString["rankid"];
                    ViewState["rankid"] = Request.QueryString["rankid"];
                }
                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                {
                    ucVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                    ucVessel.bind();
                    SetVesselType(null, null);
                }
                else if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["vesselid"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.bind();
                    SetVesselType(null, null);
                }
                if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != "")
                {
                    ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();
                }

                if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
                    ViewState["trainingmatrixid"] = Request.QueryString["trainingmatrixid"].ToString();

                BindTrainingMatrix();
            }
            BindData();
            BindCancelledData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewLicence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("REQUEST"))
            {
                if (!IsValidLicenceRequest())
                {
                    ucError.Visible = true;
                    return;
                }
                string csvLicence = string.Empty, csvType = string.Empty, csvMissingyn = string.Empty;
                foreach (GridViewRow gv in gvMissingLicence.Rows)
                {
                    CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
                    if (ck.Checked && ck.Enabled)
                    {
                        csvLicence += ((Label)gv.FindControl("lblDocumentId")).Text + ",";
                        csvType += ((Label)gv.FindControl("lblType")).Text + ",";
                        csvMissingyn += ((Label)gv.FindControl("lblMissingYN")).Text + ",";
                    }
                }
                PhoenixCrewLicenceRequest.InsertCrewLicenceProcess(int.Parse(ViewState["empid"].ToString())
                    , null
                    , General.GetNullableInteger(ucVessel.SelectedVessel)
                    , General.GetNullableInteger(ddlRank.SelectedValue)
                    , General.GetNullableDateTime(ucDate.Text)
                    , null
                    , General.GetNullableInteger(rblPaymentBy.SelectedValue)
                    , PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.Value
                    , null
                    , csvLicence.TrimEnd(',')
                    , csvType.TrimEnd(',')
                    , General.GetNullableGuid(Request.QueryString["crewplanid"]), csvMissingyn.TrimEnd(','));

                ucStatus.Text = "Licence Request Initiated Successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindTrainingMatrix()
    {
        ddlTrainingMatrix.Items.Clear();
        ddlTrainingMatrix.DataSource = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(
                                            General.GetNullableInteger(ViewState["vsltype"].ToString()),
                                            General.GetNullableInteger(ddlRank.SelectedValue),
                                            General.GetNullableInteger(ViewState["Charterer"].ToString()));
        ddlTrainingMatrix.DataTextField = "FLDMATRIXNAME";
        ddlTrainingMatrix.DataValueField = "FLDMATRIXID";
        ddlTrainingMatrix.DataBind();
        ddlTrainingMatrix.Items.Insert(0, new ListItem("--Select--", "Dummy"));

        if (Request.QueryString["trainingmatrixid"] != null && Request.QueryString["trainingmatrixid"].ToString() != "")
        {
            if (ddlTrainingMatrix.Items.FindByValue(Request.QueryString["trainingmatrixid"].ToString()) != null)
                ddlTrainingMatrix.SelectedValue = Request.QueryString["trainingmatrixid"].ToString();
        }
        else
        {
            if (ddlTrainingMatrix.Items.Count == 2)
                ddlTrainingMatrix.SelectedIndex = 1;

        }
    }

    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixCrewOffshoreLicence.ListCrewOffshoreMissingLicence(General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                                                            , int.Parse(ViewState["empid"].ToString())
                                                            , General.GetNullableInteger(ucVessel.SelectedVessel));
            if (dt.Rows.Count > 0)
            {
                gvMissingLicence.DataSource = dt;
                gvMissingLicence.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvMissingLicence);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMissingLicence_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    protected void BindCancelledData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewLicenceRequest.CrewLicenceProcessHandOverSearch(null
                                                                        , null
                                                                        , null
                                                                        , null
                                                                        , sortexpression, sortdirection
                                                                        , 1, General.ShowRecords(null)
                                                                        , ref iRowCount, ref iTotalPageCount
                                                                        , null
                                                                        , null
                                                                        , null
                                                                        , General.GetNullableInteger(ViewState["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                gvLicReq.DataSource = dt;
                gvLicReq.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvLicReq);

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLicReq_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT") return;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToString().ToUpper() == "CANCEL")
            {
                _gridView.SelectedIndex = nCurrentRow;

                Label lblPID = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessId"));
                ViewState["PID"] = lblPID.Text;
                PhoenixCrewLicenceRequest.LicenceProcessCancel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(lblPID.Text));

                BindCancelledData();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLicReq_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;

            _gridView.EditIndex = e.NewEditIndex;

            BindCancelledData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLicReq_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLicReq_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdCancel");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel the request ?')");
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.SelectedValue = dt.Rows[0]["FLDRANKPOSTED"].ToString();
                ViewState["rankid"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SetVesselType(object sender, EventArgs e)
    {
        ViewState["Charterer"] = "";
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null || General.GetNullableInteger(Request.QueryString["vesselid"]) != null)
        {
            string vesselid = General.GetNullableInteger(ucVessel.SelectedVessel) == null ? Request.QueryString["vesselid"] : ucVessel.SelectedVessel;
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(vesselid));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucVesselType.SelectedVesseltype = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["Charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
            }
            BindTrainingMatrix();
        }
    }

    protected void ddlRank_Changed(object sender, EventArgs e)
    {
        BindTrainingMatrix();
    }

    public void SetNewApplicantPrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();

                ddlRank.DataSource = PhoenixCrewManagement.CrewRankDeptList(General.GetNullableInteger(dt.Rows[0]["FLDGROUP"].ToString()), null);
                ddlRank.DataBind();
                ddlRank.SelectedValue = dt.Rows[0]["FLDRANK"].ToString();
                ViewState["rankid"] = dt.Rows[0]["FLDRANKPOSTED"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private bool IsValidLicenceRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Expected Joining Date is required.";

        if (General.GetNullableInteger(ddlTrainingMatrix.SelectedValue) == null)
            ucError.ErrorMessage = "Training Matrix is required.";

        if (General.GetNullableInteger(rblPaymentBy.SelectedValue) == null)
            ucError.ErrorMessage = "Payment By is required.";

        string csvLicence = string.Empty;
        foreach (GridViewRow gv in gvMissingLicence.Rows)
        {
            CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
            if (ck.Checked && ck.Enabled)
            {
                csvLicence += ((Label)gv.FindControl("lblDocumentId")).Text + ",";
            }
        }
        if (string.IsNullOrEmpty(csvLicence.TrimEnd(',')))
            ucError.ErrorMessage = "select atleast one or more licence.";

        if (gvMissingLicence.Rows.Count == 1 && gvMissingLicence.Rows[0].Cells.Count == 1)
        {
            ucError.ErrorMessage = "No Licence found to Initiate Request";
        }
        if (!PhoenixSecurityContext.CurrentSecurityContext.UserRegisteredCompanyId.HasValue)
            ucError.ErrorMessage = "Registered Company is not configured for the User, Configure and try initiating the request.";
        return (!ucError.IsError);
    }
}
