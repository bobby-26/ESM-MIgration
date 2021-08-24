using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLicenceRequestFullTermDetails : PhoenixBasePage
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
			CrewLicReq.SelectedMenuIndex = 1;

          
            PhoenixToolbar toolbar = new PhoenixToolbar();
			ViewState["licenceid"] = "";
            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewLicenceRequestFullTermDetails.aspx?" + Request.QueryString.ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvFullTerm')", "Print Grid", "icon_print.png", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();
            ViewState["PROCESSID"] = null;

            if (Request.QueryString["pid"] != null && Request.QueryString["pid"].ToString() != string.Empty)
             ViewState["PROCESSID"] = Request.QueryString["pid"].ToString();
            SetEmployeeDetails();
           
             PhoenixCrewLicenceRequest.AutoCorrectLicenceReqStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["PROCESSID"].ToString()));
           
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
				Response.Redirect("../Crew/CrewLicenceHandOver.aspx?pid=" + Request.QueryString["pid"], false); ;
			}
			else if (dce.CommandName.ToUpper().Equals("CRA"))
			{
                Response.Redirect("../Crew/CrewLicenceRequestCRADetails.aspx?" +Request.QueryString.ToString(), false);

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
        string[] alColumns = {"FLDRECDFULLTERMDATE", "FLDFULLTERMHANDEDOVERBYNAME", "FLDFULLTERMHANDEDOVERBYCOMMENTS", "FLDFULLTERMHANDOVERDATE", "FLDISFULLTERMRECEIVEDNAME", 
                                 "FLDFULLTERMRECEIVEDCOMMENTS", "FLDFULLTERMRECEIVEDBYNAME", "FLDFULLTERMRECEIVEDDATE" };
        string[] alCaptions = { "Received", "Handed Over by", "Comments", "Date", "Received", "Comments", "Received by", "Date" };

        DataSet ds = PhoenixCrewLicenceRequest.ListFullTermDetails(General.GetNullableGuid(ViewState["PROCESSID"] == null ? null : ViewState["PROCESSID"].ToString()));

        General.SetPrintOptions("gvFullTerm", "Full Term Details", alCaptions, alColumns, ds);

        Title1.Text = "Full Term Details";


        if (ds.Tables[0].Rows.Count > 0)
        {
            gvFullTerm.DataSource = ds.Tables[0];
            gvFullTerm.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvFullTerm);
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

	protected void gvFullTerm_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			
            if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                _gridView.EditIndex = -1;
                string recieveddate = ((UserControlDate)_gridView.FooterRow.FindControl("txtFulltermsRecievedDateAdd")).Text;
                string handedovercomments = ((TextBox)_gridView.FooterRow.FindControl("txtHandOverCommentsAdd")).Text;

                if (!IsValidPendinDocs(recieveddate, handedovercomments))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewLicenceRequest.InsertFullTermDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         new Guid(ViewState["PROCESSID"].ToString()), General.GetNullableDateTime(recieveddate),
                         General.GetNullableString(handedovercomments));

                ucStatus.Text = "Full Term Details updated successfully.";

                BindData();
                
            }
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    private bool IsValidPendinDocs(string fulltermrecddate, string handedovercomments)
    {
       
        if (General.GetNullableDateTime(fulltermrecddate) == null)
            ucError.ErrorMessage = "Received is required.";
        else
        {
            if (DateTime.Parse(fulltermrecddate) > DateTime.Today)
                ucError.ErrorMessage = "Received should not be the future date.";
        }

        if (string.IsNullOrEmpty(handedovercomments))
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
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
        string[] alColumns = {"FLDRECDFULLTERMDATE",  "FLDFULLTERMHANDEDOVERBYNAME",  "FLDFULLTERMHANDEDOVERBYCOMMENTS", "FLDFULLTERMHANDOVERDATE", "FLDISFULLTERMRECEIVEDNAME", 
                                 "FLDFULLTERMRECEIVEDCOMMENTS", "FLDFULLTERMRECEIVEDBYNAME", "FLDFULLTERMRECEIVEDDATE" };
        string[] alCaptions = { "Received", "Handed Over by", "Comments", "Date", "Received", "Comments", "Received by", "Date" };

        string sortexpression;
        int? sortdirection = 1;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewLicenceRequest.ListFullTermDetails(General.GetNullableGuid(ViewState["PROCESSID"] == null ? null : ViewState["PROCESSID"].ToString()));

        if (ds.Tables.Count > 0)
            General.ShowExcel("Full Term Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void gvFullTerm_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvFullTerm_RowEditing(object sender, GridViewEditEventArgs e)
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

    protected void gvFullTerm_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string lblFullTermId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblFullTermId")).Text;
            string receivedcomments = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReceivedComments")).Text;
            CheckBox chk = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkReceivedYN"));
            if (!IsReceivedValid(chk.Checked, receivedcomments))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewLicenceRequest.UpdateFullTermDetails(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(lblFullTermId), chk.Checked ? 1 : 0, receivedcomments);

            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsReceivedValid(bool receivedYN, string comments)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!receivedYN)
            ucError.ErrorMessage = "Please tick the 'Received'.";

        if (comments.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }

    protected void gvFullTerm_RowDataBound(object sender, GridViewRowEventArgs e)
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
