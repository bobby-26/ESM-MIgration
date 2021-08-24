using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLicenceRequestCRADetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
			PhoenixToolbar toolbarmain = new PhoenixToolbar();
			toolbarmain.AddButton("CRA", "CRA");
			toolbarmain.AddButton("Full Term", "FULLTERM");
			toolbarmain.AddButton("Back", "BACK");
			CrewLicReq.AccessRights = this.ViewState;
			CrewLicReq.MenuList = toolbarmain.Show();
			CrewLicReq.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            MenuCRA.AccessRights = this.ViewState;
            MenuCRA.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewLicenceRequestCRADetails.aspx?"+Request.QueryString.ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCRA')", "Print Grid", "icon_print.png", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();
            ViewState["PROCESSID"] = null;
        
            if (Request.QueryString["pid"] != null && Request.QueryString["pid"].ToString() != string.Empty)
                ViewState["PROCESSID"] = Request.QueryString["pid"].ToString();
            SetEmployeeDetails();
			EditCRADetails(Request.QueryString["pid"].ToString());
        }        
        BindData();        
    }

    protected void SetEmployeeDetails()
    {
        try
        {
            if (ViewState["PROCESSID"] != null)
            {
                DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceProcess(new Guid(ViewState["PROCESSID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtEmployeeName.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
                    txtEmployeeNumber.Text = dt.Rows[0]["FLDFILENO"].ToString();
					txtLicence.Text = dt.Rows[0]["FLDLICENCE"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
	protected void CrewLicReq_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("BACK"))
			{
				Response.Redirect("../Crew/CrewLicenceHandOver.aspx?pid=" + Request.QueryString["pid"],false); ;
			}
			else if (dce.CommandName.ToUpper().Equals("FULLTERM"))
			{
                Response.Redirect("../Crew/CrewLicenceRequestFullTermDetails.aspx?"+Request.QueryString.ToString(), false);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

    protected void EditCRADetails(string requeststatusid )
    {
        try
        {
			DataTable dt = PhoenixCrewLicenceRequest.EditCrewLicenceRequestDocStatus(General.GetNullableGuid(requeststatusid));
                if (dt.Rows.Count > 0)
                {
                    txtFlagStateProcessDate.Text = dt.Rows[0]["FLDFLAGSTATEPROCESSDATE"].ToString();
                    txtCRANumber.Text = dt.Rows[0]["FLDCRANUMBER"].ToString();
                    txtCRAExpiryDate.Text = dt.Rows[0]["FLDDATEOFEXPIRY"].ToString();
                    txtHandOverComments.Text = dt.Rows[0]["FLDHANDOVERREMARKS"].ToString();
                    txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
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
        string[] alColumns = {  "FLDCRAHANDEDOVERBYNAME", "FLDHANDOVERREMARKS", "FLDCRAHANDOVERDATE", "FLDISCRARECEIVEDNAME", 
                                 "FLDCRARECEIVEDBYCOMMENTS", "FLDCRARECEIVEDBYNAME", "FLDCRARECEIVEDDATE" };
        string[] alCaptions = { "Handed Over by", "Comments", "Date", "Received", "Comments", "Received by", "Date" };

        DataSet ds = PhoenixCrewLicenceRequest.ListCRADetails(General.GetNullableGuid(ViewState["PROCESSID"] == null ? null : ViewState["PROCESSID"].ToString()));

        General.SetPrintOptions("gvCRA", "CRA Details", alCaptions, alColumns, ds);

        Title1.Text = "CRA Details";


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCRA.DataSource = ds.Tables[0];
            gvCRA.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvCRA);
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

    private bool IsValidCRA(string flagstateprocessdate, string cranumber, string expirydate, string Comments)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableDateTime(flagstateprocessdate) == null)
            ucError.ErrorMessage = "Issued is required";
        else
        {
            if (DateTime.Parse(flagstateprocessdate) > DateTime.Today)
				ucError.ErrorMessage = "Issued should not be the future date";
        }

        if (string.IsNullOrEmpty(cranumber))
            ucError.ErrorMessage = "CRA No is required";

        if (string.IsNullOrEmpty(Comments))
            ucError.ErrorMessage = "Hand Over Comments is required";

        if (General.GetNullableDateTime(expirydate) == null)
            ucError.ErrorMessage = "Expiry is required";
        else
        {
            if(General.GetNullableDateTime(flagstateprocessdate) != null)
            {
                if(DateTime.Parse(expirydate) < DateTime.Parse(flagstateprocessdate))
                    ucError.ErrorMessage = "Expiry can not be less than Flag State Process Date";
            }
        }

        return (!ucError.IsError);
    }

    protected void MenuCRA_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["PROCESSID"] != null && ViewState["PROCESSID"].ToString() != string.Empty)
                {
                    if (!IsValidCRA(txtFlagStateProcessDate.Text, txtCRANumber.Text, txtCRAExpiryDate.Text, txtHandOverComments.Text))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixCrewLicenceRequest.InsertCRADetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["PROCESSID"].ToString()), General.GetNullableDateTime(txtFlagStateProcessDate.Text),
                        General.GetNullableString(txtCRANumber.Text), General.GetNullableDateTime(txtCRAExpiryDate.Text),
                        General.GetNullableString(txtHandOverComments.Text),
                        General.GetNullableString(txtRemarks.Text)
                        );

                    ucStatus.Text = "CRA Details updated successfully.";
					EditCRADetails(ViewState["PROCESSID"].ToString());
                    BindData();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the licence request from the list and then record the details.";
                    ucError.Visible = true;
                    return;
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

    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
		string[] alColumns = { "FLDCRAHANDEDOVERBYNAME", "FLDHANDOVERREMARKS", "FLDCRAHANDOVERDATE", "FLDISCRARECEIVEDNAME", 
                                 "FLDCRARECEIVEDBYCOMMENTS", "FLDCRARECEIVEDBYNAME", "FLDCRARECEIVEDDATE"};
        string[] alCaptions = { "Handed Over by", "Comments", "Date", "Received", "Comments", "Received by", "Date"};

        string sortexpression;
        int? sortdirection = 1;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixCrewLicenceRequest.ListCRADetails(General.GetNullableGuid(ViewState["PROCESSID"] == null ? null : ViewState["PROCESSID"].ToString()));

        if (ds.Tables.Count > 0)
            General.ShowExcel("CRA Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void gvCRA_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvCRA_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;

            _gridView.EditIndex = e.NewEditIndex;
            BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCRA_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string requeststatusid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestStatusId")).Text;
            string receivedcomments = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReceivedComments")).Text;
            CheckBox chk = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkReceivedYN"));
			if (!IsReceivedValid(chk.Checked, receivedcomments, requeststatusid))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateCRADetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(requeststatusid), chk.Checked ? 1 : 0, receivedcomments);           

            _gridView.EditIndex = -1;
            BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
	protected void gvCRA_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("SELECT"))
			{

				string lblReqStatusId= ((Label)_gridView.Rows[nCurrentRow].FindControl("lblReqStatusId")).Text;
				EditCRADetails(lblReqStatusId);
				BindData();
			
			}

		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    private bool IsReceivedValid(bool receivedYN, string comments,string reqstatusid)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!receivedYN)
            ucError.ErrorMessage = "Please tick the 'Received'.";

        if (comments.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

		if(General.GetNullableGuid(reqstatusid)==null)
			   ucError.ErrorMessage = "Please fill in the Handed over details and then  update the CRA recieved details";

        return (!ucError.IsError);
    }

    protected void gvCRA_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                if (e.Row.Cells.Count > 1)
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdEdit");

                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    if (drv["FLDISCRARECEIVED"].ToString() == "1")
                    {
                        if (db != null)
                            db.Visible = false;
                    }

                    Label lbtn = (Label)e.Row.FindControl("lblHandOverComments");
                    UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucHandOverCommentsTT");
                    if (lbtn != null)
                    {
                        lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                        lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                    }

                    lbtn = (Label)e.Row.FindControl("lblReceivedComments");
                    if (lbtn != null)
                    {
                        uct = (UserControlToolTip)e.Row.FindControl("ucReceivedCommentsTT");
                        lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                        lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                    }
                }
            }
        }
    }
}
