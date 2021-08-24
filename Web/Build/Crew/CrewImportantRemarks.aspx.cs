using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewImportantRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);        
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddFontAwesomeButton("../Crew/CrewImportantRemarks.aspx?empid=" + Request.QueryString["empid"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvDiscussion')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuShowExcel.AccessRights = this.ViewState;
        MenuShowExcel.MenuList = toolbar1.Show();

        ViewState["ACCESS"] = "0";
        if (General.GetNullableInteger(Filter.CurrentCrewSelection).HasValue)
            ViewState["ACCESS"] = PhoenixCrewManagement.EmployeeZoneAccessList(General.GetNullableInteger(Filter.CurrentCrewSelection).Value);
        if (ViewState["ACCESS"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Remarks", "POSTCOMMENT", ToolBarDirection.Right);
            MenuDiscussion.AccessRights = this.ViewState;
            MenuDiscussion.MenuList = toolbar.Show();
        }

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;

            SetEmployeePrimaryDetails();
        }
        BindData();
    }
    
    protected void gvDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDiscussion.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvDiscussion_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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


    protected void gvDiscussion_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string notesid = ((RadLabel)e.Item.FindControl("lblNotesId")).Text;
            string doneremarks = ((RadTextBox)e.Item.FindControl("txtDoneRemarks")).Text;
            CheckBox chk = ((CheckBox)e.Item.FindControl("chkDoneYN"));
            if (!IsDoneRemarksValid(chk.Checked, doneremarks))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCommonDiscussion.TransTypeDiscussionUpdate(int.Parse(notesid), GetCurrentEmployeeDTkey()
                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , doneremarks.Trim(), chk.Checked ? 1 : 0);

            BindData();
            gvDiscussion.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDiscussion_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db != null) if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (General.GetNullableDateTime(drv["ACTIONDATE"].ToString()).HasValue)
            {
                if (db != null)
                    db.Visible = false;
            }


            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarksTT");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }

            lbtn = (RadLabel)e.Item.FindControl("lblDoneRemarks");
            if (lbtn != null)
            {
                uct = (UserControlToolTip)e.Item.FindControl("ucDoneRemarksTT");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
        }
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

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(GetCurrentEmployeeDTkey()
                    , null
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , "6");

        if (ds.Tables.Count > 0)
            General.ShowExcel("Crew Important Remarks", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }
    
    private void BindData()
    {
        string[] alColumns = { "NAME", "DESCRIPTION", "POSTEDDATE", "FLDDONEYN", "FLDDONEREMARKS", "ACTIONBY", "ACTIONDATE" };
        string[] alCaptions = { "Posted By", "Comments", "Date", "Completed", "Comments", "Action By", "Action Date" };

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCommonDiscussion.TransTypeDiscussionSearch(GetCurrentEmployeeDTkey()
                        , null
                        , sortexpression
                        , sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvDiscussion.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , "6");

        General.SetPrintOptions("gvDiscussion", "Crew Important Remarks", alCaptions, alColumns, ds);

        gvDiscussion.DataSource = ds.Tables[0];
        gvDiscussion.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

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
            gvDiscussion.Rebind();
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
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
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

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
                lblGroup.Text = dt.Rows[0]["FLDGROUP"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
