using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionPNIRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        if (!IsPostBack)
        {
           
            ViewState["empid"] = string.Empty;
            ViewState["pniid"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                ViewState["empid"] = Request.QueryString["empid"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["PNIID"]))
            {
                ViewState["pniid"] = Request.QueryString["PNIID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["REFNO"]))
            {
                Title1.Text = "P&I Remarks" + "(" + Request.QueryString["REFNO"] + ")";
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            SetEmployeePrimaryDetails();
           
            
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Medical Case", "MEDICALCASE");
        toolbarmain.AddButton("Remarks", "REMARKS");
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbarmain.Show();
        CrewMenu.SelectedMenuIndex = 1;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Post Remarks", "POSTCOMMENT",ToolBarDirection.Right);
        MenuDiscussion.AccessRights = this.ViewState;
        MenuDiscussion.MenuList = toolbar.Show();
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
        BindData();
    }

    private void BindData()
    {
        DataSet ds;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionPNI.PNIRemarksSearch(General.GetNullableGuid(ViewState["pniid"].ToString()), sortdirection
          , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Title1.Text = "General Remarks";
        repDiscussion.DataSource = ds.Tables[0];
        repDiscussion.VirtualItemCount = iRowCount;
       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

     
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("MEDICALCASE"))
        {
            if (ViewState["pniid"] != null && ViewState["pniid"].ToString() != "")
            {
                Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIID=" + ViewState["pniid"], true);
            }

        }
    }

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        try
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
                PhoenixInspectionPNI.InsertPNIRemarks(new Guid(ViewState["pniid"].ToString())
                                                    , txtNotesDescription.Text
                                                    , General.GetNullableDateTime(txtFollowupDate.Text));
                ucStatus.Text = "Remarks Updated.";
                BindData();
               

            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;

        }
    }

    //protected void AllowNonmandatory(object sender, EventArgs e)
    //{
    //    if (chkDisable.Checked == true)
    //    {
    //        txtDOA.CssClass = "input";
    //        txtFollowupDate.CssClass = "input";
    //        txtNotesDescription.Focus();
    //    }
    //    else
    //    {
    //        txtDOA.CssClass = "input_mandatory";
    //        txtFollowupDate.CssClass = "input_mandatory";
    //        if (General.GetNullableString(txtDOA.Text) == null)
    //            txtDOA.FindControl("txtDate").Focus();
    //        else
    //            txtFollowupDate.FindControl("txtDate").Focus();
    //    }
    //}

    private bool IsCommentValid(string strComment)
    {
        //DateTime resultdate, resultDOA;

        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";
        if (General.GetNullableDateTime(txtFollowupDate.Text) == null)
            ucError.ErrorMessage = "Crew Follow Up Date is required.";
      
        return (!ucError.IsError);
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

   

  

   

    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {
            DataTable dt = PhoenixIntegrationQuality.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
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

    //private void DAO()
    //{
    //    DataTable dtDAOEdit = PhoenixCrewDateOfAvailability.DateOfAvailabilityEdit(General.GetNullableInteger(Filter.CurrentCrewSelection));

    //    if (dtDAOEdit.Rows.Count > 0)
    //    {
    //        ViewState["DOAID"] = dtDAOEdit.Rows[0]["FLDDOAID"].ToString();

    //        txtDOA.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDDOA"].ToString());
    //        //txtFollowupDate.Text = General.GetDateTimeToString(dtDAOEdit.Rows[0]["FLDFOLLOWUPDATE"].ToString()); 
    //    }
    //}

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixIntegrationQuality.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

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

    //private bool IsCrewOnboard()
    //{
    //    DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

    //    string crewstatusid = dt.Rows[0]["FLDEMPLOYEESTATUS"].ToString();

    //    if (crewstatusid == "216" || crewstatusid == "220")
    //        return true;
    //    else
    //        return false;
    //}

    //private void EnableFollowup()
    //{
    //    if (IsCrewOnboard())
    //    {
    //        txtDOA.Enabled = false;
    //        txtDOA.Text = "";
    //        txtFollowupDate.Enabled = false;
    //        txtFollowupDate.Text = "";
    //    }
    //    else
    //    {
    //        txtDOA.Enabled = true;
    //        txtFollowupDate.Enabled = true;
    //    }
    //}
}
