using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewNewApplicantImportantRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Crew/CrewNewApplicantImportantRemarks.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("javascript:CallPrint('repDiscussion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuShowExcel.AccessRights = this.ViewState;
        MenuShowExcel.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACCESS"] = "0";
            SetEmployeePrimaryDetails();
            repDiscussion.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        if (General.GetNullableInteger(Filter.CurrentNewApplicantSelection).HasValue)
            ViewState["ACCESS"] = PhoenixCrewManagement.EmployeeZoneAccessList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection).Value);
        if (ViewState["ACCESS"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Remarks", "POSTCOMMENT", ToolBarDirection.Right);
            MenuDiscussion.AccessRights = this.ViewState;
            MenuDiscussion.MenuList = toolbar.Show();
        }

        //BindData();
    }

    private void BindData()
    {
        string[] alColumns = { "NAME", "DESCRIPTION", "POSTEDDATE", "FLDDONEYN", "FLDDONEREMARKS", "ACTIONBY", "ACTIONDATE" };
        string[] alCaptions = { "Posted By", "Comments", "Date", "Completed", "Comments", "Action By", "Action Date" };


        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(GetCurrentEmployeeDTkey(), null, sortexpression, sortdirection
          , (int)ViewState["PAGENUMBER"], repDiscussion.PageSize, ref iRowCount, ref iTotalPageCount, "6");

        General.SetPrintOptions("repDiscussion", "Important Remarks", alCaptions, alColumns, ds);

        repDiscussion.DataSource = ds.Tables[0];
        //repDiscussion.DataBind();
        repDiscussion.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        //SetPageNavigator();
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
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
        string[] alColumns = { "NAME", "DESCRIPTION", "POSTEDDATE", "FLDDONEYN", "FLDDONEREMARKS", "ACTIONBY", "ACTIONDATE" };
        string[] alCaptions = { "Posted By", "Comments", "Date", "Completed", "Comments", "Action By", "Action Date" };

        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(GetCurrentEmployeeDTkey(), null, sortexpression, sortdirection
          , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount, "6");


        Response.AddHeader("Content-Disposition", "attachment; filename=Important Remarks.xls");
        Response.ContentType = "application/msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Important Remarks</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private bool IsDoneRemarksValid(bool completedYN, string doneRemarks)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!completedYN)
            ucError.ErrorMessage = "Please tick the 'Completed.'";

        if (doneRemarks.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCommonDiscussion.TransTypeDiscussionInsert(GetCurrentEmployeeDTkey()
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , txtNotesDescription.Text.Trim(), "6");

            BindData();
            repDiscussion.Rebind();
            txtNotesDescription.Text = "";
        }
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }
    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                dtkey = new Guid(dt.Rows[0]["FLDDTKEY"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return dtkey;
    }

    protected void GetRemarks(object sender, EventArgs e)
    {
        BindData();
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                //lblGroup.Text = dt.Rows[0]["FLDGROUP"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        repDiscussion.CurrentPageIndex = 0;
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void Rebind()
    {
        repDiscussion.SelectedIndexes.Clear();
        repDiscussion.EditIndexes.Clear();
        repDiscussion.DataSource = null;
        repDiscussion.Rebind();
    }
    protected void repDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : repDiscussion.CurrentPageIndex + 1;
        BindData();
    }
    protected void repDiscussion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && !e.Item.IsInEditMode)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdEdit");

            DataRowView drv = (DataRowView)item.DataItem;

            if (General.GetNullableDateTime(drv["ACTIONDATE"].ToString()).HasValue)
            {
                if (db != null)
                    db.Visible = false;
            }

            RadLabel lbtn = (RadLabel)item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)item.FindControl("ucRemarksTT");
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;

            RadLabel lbtn1 = (RadLabel)item.FindControl("lblDoneRemarks");
            UserControlToolTip uct1 = (UserControlToolTip)item.FindControl("ucDoneRemarksTT");

            uct1.Position = ToolTipPosition.TopCenter;
            uct1.TargetControlId = lbtn1.ClientID;

        }
    }
    protected void repDiscussion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "EDIT")
            {
                BindData();
            }
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string notesid = ((RadLabel)item.FindControl("lblNotesId")).Text;
                string doneremarks = ((RadTextBox)item.FindControl("txtDoneRemarks")).Text;
                CheckBox chk = ((CheckBox)item.FindControl("chkDoneYN"));
                if (!IsDoneRemarksValid(chk.Checked, doneremarks))
                {
                    ucError.Visible = true;
                    e.Canceled = true;
                    return;
                }

                PhoenixCommonDiscussion.TransTypeDiscussionUpdate(int.Parse(notesid), GetCurrentEmployeeDTkey()
                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , doneremarks.Trim(), chk.Checked ? 1 : 0);

                repDiscussion.EditIndexes.Clear();
                BindData();
                repDiscussion.Rebind();
            }
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
