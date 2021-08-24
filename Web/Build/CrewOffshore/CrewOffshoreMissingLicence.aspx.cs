using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewOffshoreMissingLicence : PhoenixBasePage
{
    string strEmployeeId = string.Empty, strRankId = string.Empty, strVesselId = string.Empty, strTrainingMatrixId = string.Empty;
    string strJoinDate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            strEmployeeId = Request.QueryString["empid"];
            strRankId = Request.QueryString["rnkid"];
            strVesselId = Request.QueryString["vslid"];
            strJoinDate = Request.QueryString["jdate"];

            if (Request.QueryString["trainingmatrixid"] != null)
            {
                strTrainingMatrixId = Request.QueryString["trainingmatrixid"];
            }

            if (!IsPostBack)
            {
                rblPaymentBy.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 89, 0, "CMP,EMP");
                rblPaymentBy.DataTextField = "FLDHARDNAME";
                rblPaymentBy.DataValueField = "FLDHARDCODE";
                rblPaymentBy.DataBind();

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Initiate Licence Req", "REQUEST");
                CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();
                SetInformation();
                SetEmployeePrimaryDetails();

                txtPlannedJoinDate.Text = strJoinDate;
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
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("REQUEST"))
            {
                string csvLicence = string.Empty, csvType = string.Empty;
                foreach (GridViewRow gv in gvMissingLicence.Rows)
                {
                    CheckBox ck = (CheckBox)gv.FindControl("chkSelect");
                    if (ck.Checked && ck.Enabled)
                    {
                        csvLicence += ((Label)gv.FindControl("lblDocumentId")).Text + ",";
                        csvType += ((Label)gv.FindControl("lblType")).Text + ",";
                    }
                }
                if (!IsValidateRequest(csvLicence.TrimEnd(',')))
                {
                    ucError.Visible = true;
                    return;
                }
                if (gvMissingLicence.Rows.Count == 1 && gvMissingLicence.Rows[0].Cells.Count == 1)
                {
                    ucError.ErrorMessage = "No Licence found to Initiate Request";
                    ucError.Visible = true;
                    return;
                }
                ucConfirm.Visible = true;
                ucConfirm.Text = "You will not be able to make any changes. Are you sure you want to Initiate Licence Request ?";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void InitiateLicenceRequest(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
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
                PhoenixCrewLicenceRequest.InsertCrewLicenceProcess(int.Parse(strEmployeeId),
                    General.GetNullableInteger(ViewState["flag"].ToString()), General.GetNullableInteger(strVesselId), General.GetNullableInteger(strRankId),
                    General.GetNullableDateTime(txtCrewChangeDate.Text), General.GetNullableDateTime(txtCraDate.Text),
                    General.GetNullableInteger(rblPaymentBy.SelectedValue), int.Parse(ddlLiabilitycompany.SelectedCompany.ToString()), txtRemarks.Text, csvLicence.TrimEnd(','), csvType.TrimEnd(',')
                    , General.GetNullableGuid(Request.QueryString["crewplanid"]), csvMissingyn.TrimEnd(','));
                ucStatus.Text = "Licence Request Initiated";
                BindData();
                BindCancelledData();
                txtCraDate.Text = "";
                txtCrewChangeDate.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            DataTable dt = PhoenixCrewOffshoreCrewChange.ListLicenceMissing(int.Parse(strVesselId), int.Parse(strEmployeeId), int.Parse(strRankId), DateTime.Parse(strJoinDate), General.GetNullableInteger(strTrainingMatrixId));
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                CheckBox ck = e.Row.FindControl("chkSelect") as CheckBox;
                if (drv["FLDISREQ"].ToString() == "1") ck.Enabled = false;
            }
        }
    }
    private void SetInformation()
    {
        DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(strVesselId));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            txtVesselFlag.Text = ds.Tables[0].Rows[0]["FLDFLAGNAME"].ToString();
            ViewState["flag"] = ds.Tables[0].Rows[0]["FLDFLAG"].ToString();
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
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(strEmployeeId));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKPOSTEDNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidateRequest(string csvLicence)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        DateTime dt;
        if (string.IsNullOrEmpty(csvLicence))
            ucError.ErrorMessage = "select atleast one or more licence.";

        if (General.GetNullableInteger(rblPaymentBy.SelectedValue) == null)
            ucError.ErrorMessage = "Payment By is required.";

        if (txtCrewChangeDate.Text == null)
            ucError.ErrorMessage = "Crew Change date is required.";
        else if (DateTime.TryParse(txtCrewChangeDate.Text, out dt) == false)
            ucError.ErrorMessage = "Crew Change date  is not in correct format";
        else if (DateTime.TryParse(txtCrewChangeDate.Text, out dt) && DateTime.Compare(dt, DateTime.Now) < 0)
            ucError.ErrorMessage = "Crew Change Date should be later than current date";
        if (txtCraDate.Text != null)
        {
            if (DateTime.TryParse(txtCraDate.Text, out dt) == false)
                ucError.ErrorMessage = "CRA date  is not in correct format";
        }
        if (ddlLiabilitycompany.SelectedCompany.ToUpper().Equals("DUMMY"))
        {
            ucError.ErrorMessage = "Bill to Company is required";
        }
        return (!ucError.IsError);
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
            NameValueCollection nvc = Filter.CurrentLicenceRequestFilterSelection;
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
                                                                        , General.GetNullableInteger(strEmployeeId));
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            //General.SetPrintOptions("gvLicReq", "Licence Request", alCaptions, alColumns, ds);

            if (dt.Rows.Count > 0)
            {
                gvLicReq.DataSource = dt;
                gvLicReq.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvLicReq);

            }
            //ViewState["ROWCOUNT"] = iRowCount;
            //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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
}
