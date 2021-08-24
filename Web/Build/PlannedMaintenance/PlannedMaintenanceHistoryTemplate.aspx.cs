using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;
using Telerik.Web.Spreadsheet;

public partial class PlannedMaintenanceHistoryTemplate : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplate.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar1.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceHistoryTemplate.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuHistoryTemplate.AccessRights = this.ViewState;
            MenuHistoryTemplate.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //ViewState["COMPONENTJOBID"] = Request.QueryString["COMPONENTJOBID"].ToString();
                //ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                gvMaintenanceForm.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuHistoryTemplate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "FIND")
            {
                gvMaintenanceForm.CurrentPageIndex = 0;
                BindData();
                gvMaintenanceForm.Rebind();
            }
            else if (CommandName.ToUpper()=="CLEAR")
            {
                gvMaintenanceForm.CurrentPageIndex = 0;
                txtTemplateName.Text = "";
                BindData();
                gvMaintenanceForm.Rebind();
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
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? sortdirection = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataTable dt = PhoenixPlannedMaintenanceHistoryTemplate.PMSHistoryTemplateSearch(txtTemplateName.Text
                                                                                               , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                               , null
                                                                                               , sortexpression
                                                                                               , sortdirection
                                                                                               , gvMaintenanceForm.CurrentPageIndex + 1
                                                                                               , gvMaintenanceForm.PageSize
                                                                                               , ref iRowCount, ref iTotalPageCount);

            gvMaintenanceForm.DataSource = dt;
            gvMaintenanceForm.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidNamee(string formname)
    {
        ucError.HeaderMessage = "Please enter a valid data.";

        if (string.IsNullOrEmpty(formname))
            ucError.ErrorMessage = "Enter name of the maintenance form name.";

        return (!ucError.IsError);
    }

    protected void gvMaintenanceForm_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            GridFooterItem footerItem = (GridFooterItem)gvMaintenanceForm.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadTextBox name = (RadTextBox)footerItem.FindControl("lblFormName");
                RadTextBox frmNo = (RadTextBox)footerItem.FindControl("txtFormNoAdd");
                UserControlMakerModel model = (UserControlMakerModel)footerItem.FindControl("ucModel");
                if (!IsValidName(name.Text,frmNo.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceHistoryTemplate.InsertMaintenanceForm(name.Text, General.GetNullableInteger(model.SelectedValue), frmNo.Text);
                BindData();
                gvMaintenanceForm.Rebind();
            }

              if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string formid = ((RadLabel)e.Item.FindControl("lblFormEdit")).Text;
                string formname = ((RadTextBox)e.Item.FindControl("txtformname")).Text;
                string modelid = ((UserControlMakerModel)e.Item.FindControl("ucmodeledit")).SelectedValue;
                string frmNo = ((RadTextBox)e.Item.FindControl("txtFormNo")).Text;
                if (!IsValidName(formname,frmNo))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceHistoryTemplate.UpdateMaintenanceForm(new Guid(formid),formname,
                                                                                General.GetNullableInteger(modelid),
                                                                                frmNo);
                gvMaintenanceForm.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("REPORTS"))
            {
                RadLabel formid = ((RadLabel)e.Item.FindControl("lblFormID"));
                RadLabel lblFormName = ((RadLabel)e.Item.FindControl("lblFormName"));
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateReports.aspx?FORMID=" + formid.Text + "&FORMNAME=" + lblFormName.Text);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel formid = ((RadLabel)e.Item.FindControl("lblFormID"));

                PhoenixPlannedMaintenanceHistoryTemplate.DeleteMaintenanceForm(new Guid(formid.Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidName(string name,string frmNo)
    {
        ucError.HeaderMessage = "Please enter a valid data.";

        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Form name is required";

        if (string.IsNullOrEmpty(frmNo))
            ucError.ErrorMessage = "Form number is required";

        return (!ucError.IsError);
    }

    protected void gvMaintenanceForm_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvMaintenanceForm_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        { 
       
            GridFooterItem footerItem = (GridFooterItem)gvMaintenanceForm.MasterTableView.GetItems(GridItemType.Footer)[0];
            LinkButton db = (LinkButton)footerItem.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)  
                {
                    db.Attributes.Add("style", "display:none");
                }
            }
        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton comjob = (LinkButton)e.Item.FindControl("cmdComjob");
            if (comjob != null)
            {
                comjob.Visible = SessionUtil.CanAccess(this.ViewState, comjob.CommandName);
                comjob.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceFormComponentJobMapList.aspx?formid=" + drv["FLDFORMID"].ToString() + "'); return false;");
            }
            LinkButton templ = (LinkButton)e.Item.FindControl("cmdExcelTemplate");
            if (templ != null)
            {
                templ.Visible = SessionUtil.CanAccess(this.ViewState, templ.CommandName);
                templ.Attributes.Add("onclick", "javascript:openNewWindow('ExcelTemplate','Excel Template','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceHistoryExcelTemplate.aspx?fid=" + drv["FLDFORMID"].ToString() + "'); return false;");
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)  //issue
                {
                    eb.Attributes.Add("style", "display:none");
                }
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) 
                sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }
        }
    }
}
