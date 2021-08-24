using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersWorkingGearType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkingGeraType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearType.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersWorkingGearType.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            
            MenuRegistersWorkingGearType.AccessRights = this.ViewState;
            MenuRegistersWorkingGearType.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWorkingGeraType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alCaptions = { "Code", "Name" };
        string[] alColumns = { "FLDWORKINGGEARTYPECODE", "FLDWORKINGGEARTYPENAME" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersWorkingGearType.WorkingGearTypeSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                               , txtWorkingGearTypeName.Text.Trim()
                                                               , txtWorkingGearTypeCode.Text.Trim()
                                                               , sortexpression
                                                               , sortdirection
                                                               , (int)ViewState["PAGENUMBER"]
                                                               , gvWorkingGeraType.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);
        General.ShowExcel("Working Gear Type", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);


    }

    protected void RegistersWorkingGearType_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;            
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvWorkingGeraType.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtWorkingGearTypeCode.Text = "";
                txtWorkingGearTypeName.Text = "";
                BindData();
                gvWorkingGeraType.Rebind();
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
        string[] alColumns = { "FLDWORKINGGEARTYPECODE", "FLDWORKINGGEARTYPENAME" };
        string[] alCaptions = { "Code", "Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersWorkingGearType.WorkingGearTypeSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , txtWorkingGearTypeName.Text.Trim()
                                                                    , txtWorkingGearTypeCode.Text.Trim()
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , (int)ViewState["PAGENUMBER"]
                                                                    , gvWorkingGeraType.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

        General.SetPrintOptions("gvWorkingGeraType", "Working Gear Type", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWorkingGeraType.DataSource = ds;
            gvWorkingGeraType.VirtualItemCount = iRowCount;
        }
        else
        {
            gvWorkingGeraType.DataSource = "";
        }
    }
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvWorkingGeraType.Rebind();
    }
    

    private void InsertWorkingGearType(string WorkingGearTypeCode, string WorkingGearTypeName)
    {

        PhoenixRegistersWorkingGearType.InsertWorkingGearType(PhoenixSecurityContext.CurrentSecurityContext.UserCode, WorkingGearTypeCode, WorkingGearTypeName);
    }

    private void UpdateWorkingGearType(int WorkingGearTypeId, string WorkingGearTypeCode, string WorkingGearTypeName)
    {
        if (!IsValidWorkingGearType(WorkingGearTypeCode,WorkingGearTypeName))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearType.UpdateWorkingGearType(PhoenixSecurityContext.CurrentSecurityContext.UserCode, WorkingGearTypeId, WorkingGearTypeCode, WorkingGearTypeName);
        ucStatus.Text = "Information updated";
    }

    private bool IsValidWorkingGearType(string WorkingGearTypeCode, string WorkingGearTypeName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (WorkingGearTypeCode.Trim().Equals(""))
        {
            ucError.ErrorMessage = "WorkingGearType Code is required.";
        }

        if (WorkingGearTypeName.Trim().Equals(""))
        {
            ucError.ErrorMessage = "WorkingGearType Name is required.";
        }


        return (!ucError.IsError);
    }

    private void DeleteWorkingGearType(int WorkingGearTypeId)
    {
        PhoenixRegistersWorkingGearType.DeleteWorkingGearType(PhoenixSecurityContext.CurrentSecurityContext.UserCode, WorkingGearTypeId);
    }

 
    protected void gvWorkingGeraType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidWorkingGearType(((RadTextBox)e.Item.FindControl("txtWorkingGearTypeCodeAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtWorkingGearTypeNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertWorkingGearType(
                    ((RadTextBox)e.Item.FindControl("txtWorkingGearTypeCodeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtWorkingGearTypeNameAdd")).Text
                );
                BindData();
                gvWorkingGeraType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteWorkingGearType(Int32.Parse(((RadLabel)e.Item.FindControl("lblWorkingGearTypeId")).Text));
                BindData();
                gvWorkingGeraType.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateWorkingGearType(
                   Int32.Parse(((RadLabel)e.Item.FindControl("lblWorkingGearTypeIdEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtWorkingGearTypeCodeEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtWorkingGearTypeNameEdit")).Text

                );               
                BindData();
                gvWorkingGeraType.Rebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvWorkingGeraType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkingGeraType.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvWorkingGeraType_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        }
       
        if (e.Item is GridHeaderItem)
        {
            if (Request.QueryString["quickcodetype"] != null)
            {
                if (Request.QueryString["quickcodetype"].ToString() == "64")
                {
                    LinkButton lnkCode = (LinkButton)e.Item.FindControl("lnkWorkingGearTypeCode");
                    if (lnkCode != null) lnkCode.Text = "Cancel Code";

                    LinkButton lnkName = (LinkButton)e.Item.FindControl("lblWorkingGearTypeNameHeader");
                    if (lnkName != null) lnkName.Text = "Cancel Reason";

                }
                else if (Request.QueryString["quickcodetype"].ToString() == "65")
                {
                    //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                    //{
                        LinkButton lnkCode = (LinkButton)e.Item.FindControl("lnkWorkingGearTypeCode");
                        if (lnkCode != null) lnkCode.Text = "Approval Code";

                        LinkButton lnkName = (LinkButton)e.Item.FindControl("lblWorkingGearTypeNameHeader");
                        if (lnkName != null) lnkName.Text = "Approval Authority";
                    //}
                }
            }
        }
       
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
}
