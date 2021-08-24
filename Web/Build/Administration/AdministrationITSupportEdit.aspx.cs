using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AdministrationITSupportEdit : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Header.DataBind();
        ViewState["SHORTCODE"] = "";
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("History", "HISTORY",ToolBarDirection.Right);
        toolbar.AddButton("Edit", "BUGEDIT", ToolBarDirection.Right);
        

        MenuBugComment.AccessRights = this.ViewState;
        MenuBugComment.MenuList = toolbar.Show();
        MenuBugComment.SelectedMenuIndex = 1;

        PhoenixToolbar toolbarbugedit = new PhoenixToolbar();
        toolbarbugedit.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarbugedit.AddButton("New", "NEW", ToolBarDirection.Right);
        
        MenuITSupportEdit.AccessRights = this.ViewState;
        MenuITSupportEdit.MenuList = toolbarbugedit.Show();

        if (!IsPostBack)
        {
            string id = Request.QueryString["dtkey"].ToString();
            ViewState["ITSUPPORTDTKEY"] = id;
            BugEdit(id);
        }
 
        BindData();
    }

    

    private void BugEdit(string id)
    {
        DataTable dt = PhoenixAdministrationITSupport.EditITSupport(General.GetNullableGuid(id));
        
        DataRow dr = dt.Rows[0];

        MenuBugComment.Title = "Edit - [" + dr["FLDBUGID"].ToString() + "] - " + dr["FLDLOGGEDBY"].ToString() + " (" + General.GetDateTimeToString(dr["FLDCREATEDDATE"]) + ")";
        ViewState["ITSUPPORTDTKEY"] = dr["FLDDTKEY"].ToString();
        lblBugID.Text = dr["FLDBUGID"].ToString();
        lblUniqueID.Text = id;
        lblCreatedOn.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"]);
        lblclosedon.Text = General.GetDateTimeToString(dr["FLDCLOSEDON"]);
        lblClosedBy.Text = dr["FLDCLOSEDBYNAME"].ToString();
        lblClosedByID.Text = dr["FLDCLOSEDBY"].ToString();
        ddlITStatus.BugDTKey = new Guid(id);
        ddlITStatus.BugId = int.Parse(dr["FLDBUGID"].ToString());        
        ddlCategoryType.SelectedCategory = dr["FLDCATEGORYID"].ToString();
        ddlITTeam.SelectedITTeam = dr["FLDITTEAMID"].ToString();
        ddlDepartmentList.SelectedDepartment = dr["FLDDEPARTMENTID"].ToString();
        ddlITStatus.SelectedITStatus = dr["FLDSTATUSID"].ToString();
        txtActionTaken.Text = dr["FLDACTIONTAKEN"].ToString();
        txtCallType.Text = dr["FLDCALLTYPE"].ToString();
        txtLoggedBy.Text = dr["FLDLOGGEDBY"].ToString();
        txtSystemName.Text = dr["FLDSYSTEMNAME"].ToString();
        txtRemarks.Text = dr["FLDREMARKS"].ToString();
        lblStatusShortcode.Text = dr["FLDSHORTCODE"].ToString();
        if (dr["FLDSHORTCODE"].ToString().ToUpper() == "OPN" || dr["FLDSHORTCODE"].ToString().ToUpper() == "ROP")
        {
            lblclosedon.Text = "";
            ddlITTeam.CssClass = "input";
            ddlITTeam.Width = "120px";
        }
    }
    protected void ddlITStatus_TextChanged(object sender, EventArgs e)
    {
        if (ddlITStatus.SelectedITStatus == "1" || ddlITStatus.SelectedITStatus == "4") //open and reopen
        {
            ddlITTeam.SelectedITTeam = "";
            ddlITTeam.CssClass = "input";
            ddlITTeam.Width = "120px";
        }
        else if (ddlITStatus.SelectedITStatus == "2" || ddlITStatus.SelectedITStatus == "3") // inprogress and closed
        {
            ddlITTeam.CssClass = "input_mandatory";
        }
    }
    protected void MenuBugComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BUGEDIT"))
            {
                Response.Redirect("../Administration/AdministrationITSupportEdit.aspx?dtkey=" + ViewState["ITSUPPORTDTKEY"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Administration/AdministrationITSupportHistory.aspx?dtkey=" + ViewState["ITSUPPORTDTKEY"].ToString(), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuITSupportEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (!ValidateBugId())
            {
                ucError.Visible = true;
                return;
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Response.Redirect("AdministrationITSupportAdd.aspx", false);
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
              
 
                
             PhoenixAdministrationITSupport.UpdateITSupport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                  Convert.ToInt32 (lblBugID.Text),
                                                  General.GetNullableDateTime(lblCreatedOn.Text),
                                                  txtLoggedBy.Text,
                                                  txtSystemName.Text,
                                                  ddlDepartmentList.SelectedValue,
                                                  Convert.ToInt32(ddlCategoryType.SelectedValue),
                                                  General.GetNullableInteger(ddlITTeam.SelectedITTeam),
                                                  txtCallType.Text,
                                                  Convert.ToInt32(ddlITStatus.SelectedValue),
                                                  txtActionTaken.Text,
                                                  General.GetNullableDateTime(lblclosedon.Text),
                                                 lblClosedByID.Text,
                                                  txtRemarks.Text,
                                                  General.GetNullableGuid (lblUniqueID.Text));
           
               
                ucStatus.Text = "Issue changes saved";
                ddlITStatus.ITStatusList =PhoenixAdministrationITSupport.GetNextITSupportStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["ITSUPPORTDTKEY"].ToString()));
                BugEdit(ViewState["ITSUPPORTDTKEY"].ToString());

                if (Request.QueryString["norefresh"] == null)
                {
                    String script = "javascript:fnReloadList('code1');";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

    private bool ValidateBugId()
    {
        if (General.GetNullableString(ddlDepartmentList.SelectedDepartment) == null)
            ucError.ErrorMessage = "Department is required";

        if (General.GetNullableString (ddlITStatus.SelectedITStatus) == null)
            ucError.ErrorMessage = "Status is required";

        if (General.GetNullableString (ddlCategoryType .SelectedCategory ) == null)
            ucError.ErrorMessage = "Category is required";

        if (General.GetNullableString(ddlITTeam.SelectedITTeam) == null && (ddlITStatus.SelectedITStatus == "2" || ddlITStatus.SelectedITStatus == "3"))
            ucError.ErrorMessage = "Attended By Name is required";
            
        return !ucError.IsError;
    }

    private void BindData()
    {
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
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
}
