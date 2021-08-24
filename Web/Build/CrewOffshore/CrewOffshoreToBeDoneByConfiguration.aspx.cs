using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreToBeDoneByConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreToBeDoneByConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvConfig')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            //MenuTitle.AccessRights = this.ViewState;
            //MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alCaptions = { "To be Done by", "Before/After Joining", "Days", "CBT/TrainingCourse" };
        string[] alColumns = { "FLDTOBEDONENAME", "FLDTYPEDESC", "FLDDAYS", "FLDCBTORTCNAME" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreToBeDoneByConfiguration.ToBeDoneByConfigurationSearch((int)ViewState["PAGENUMBER"],
                    gvConfig.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.ShowExcel("To be Done by Configuration", ds.Tables[0], alColumns, alCaptions, null, null);
    }
    protected void MenuShowExcel_TabStripCommand(object sender, EventArgs e)
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alCaptions = { "To be Done by", "Before/After Joining", "Days", "CBT/TrainingCourse" };
        string[] alColumns = { "FLDTOBEDONENAME", "FLDTYPEDESC", "FLDDAYS", "FLDCBTORTCNAME" };
        DataSet ds = new DataSet();

        ds = PhoenixCrewOffshoreToBeDoneByConfiguration.ToBeDoneByConfigurationSearch((int)ViewState["PAGENUMBER"],
                 gvConfig.PageSize,
                 ref iRowCount,
                 ref iTotalPageCount);

        General.SetPrintOptions("gvConfig", "To be Done by Configuration", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvConfig.DataSource = ds;
            gvConfig.VirtualItemCount = iRowCount;
        }
        else
        {
            gvConfig.DataSource = "";
        }
    }   
    private void UpdateConfig(string Type, int Quickcode, string Quickname, string shortname, string Days,string cbtortc)
    {
        PhoenixCrewOffshoreToBeDoneByConfiguration.UpdateToBeDoneByConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableInteger(Type), Quickcode, Quickname, shortname, General.GetNullableInteger(Days),General.GetNullableInteger(cbtortc));
        ucStatus.Text = "Details updated Sucessfully";
    }
    private bool IsValidConfig(string Type, string Days,string cbtortc)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (Type.Trim().Equals(""))
            ucError.ErrorMessage = "Before/After Joining is required.";

        if (Type.Trim().Equals("2") && Days.Trim().Equals(""))
            ucError.ErrorMessage = "Days is required.";
        if(Days.Trim().Equals("0"))
            ucError.ErrorMessage = "Please enter valid Days.";

        if (General.GetNullableInteger(cbtortc) == null)
            ucError.ErrorMessage = "CBT/TrainingCourse is required.";
        return (!ucError.IsError);
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadComboBox  ddlType = (RadComboBox)sender;
            GridDataItem Item = (GridDataItem)ddlType.NamingContainer;
            UserControlNumber ucDays = (UserControlNumber)Item.FindControl("ucDays");

            if (ddlType.SelectedValue.Trim().Equals("2"))
            {
                if (ucDays != null)
                    ucDays.CssClass = "input_mandatory";
            }
            else
            {
                if (ucDays != null)
                    ucDays.CssClass = "input";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void gvConfig_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName.ToUpper()=="UPDATE")
        {
            if (!IsValidConfig(((RadComboBox)e.Item.FindControl("ddltype")).Text
               , ((UserControlNumber)e.Item.FindControl("ucDays")).Text
               , ((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick))
            {
                ucError.Visible = true;
                return;
            }
            UpdateConfig(
                   ((RadComboBox)e.Item.FindControl("ddltype")).SelectedValue,
                    Int32.Parse(((RadLabel)e.Item.FindControl("lblQuickCodeEdit")).Text),
                     ((RadLabel)e.Item.FindControl("lblNameEdit")).Text,
                      ((RadLabel)e.Item.FindControl("lblShortNameEdit")).Text,
                      ((UserControlNumber)e.Item.FindControl("ucDays")).Text,
                      ((UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit")).SelectedQuick
                 );
            BindData();
            gvConfig.Rebind();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvConfig_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvConfig.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvConfig_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView dr = (DataRowView)e.Item.DataItem;
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


                RadComboBox ddltype = (RadComboBox)e.Item.FindControl("ddltype");
                RadLabel lblTypeId = (RadLabel)e.Item.FindControl("lblTypeId");
                UserControlNumber ucDays = (UserControlNumber)e.Item.FindControl("ucDays");
                if (ddltype != null && lblTypeId != null)
                {
                    if (ddltype.SelectedValue == "")
                        ddltype.SelectedValue = lblTypeId.Text.Trim();
                    if (ddltype.SelectedValue.Trim() == "2")
                        ucDays.CssClass = "input_mandatory";
                    else
                        ucDays.CssClass = "input";
                }

                UserControlQuick ucTypeofTraining = (UserControlQuick)e.Item.FindControl("ucTypeofTrainingEdit");
                RadLabel lblcbtortc = (RadLabel)e.Item.FindControl("lblcbtortc");
                if (ucTypeofTraining != null)
                {
                    ucTypeofTraining.bind();
                    ucTypeofTraining.SelectedQuick = dr["FLDCBTORTC"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
