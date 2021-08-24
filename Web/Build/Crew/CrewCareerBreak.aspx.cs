using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewCareerBreak : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewCareerBreak.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCB')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersQuick.AccessRights = this.ViewState;
            MenuRegistersQuick.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCB.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCB.SelectedIndexes.Clear();
        gvCB.EditIndexes.Clear();
        gvCB.DataSource = null;
        gvCB.Rebind();
    }
   
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDFROMDATE", "FLDTODATE", "FLDREMARKS" };
        string[] alCaptions = { "File No", "Employee Name", "Rank", "From Date", "To Date", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null) sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixCrewCareerBreak.SearchEmployeeCareerBreak(null, null, null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvCB.PageSize, ref iRowCount, ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCB", "Career Break", alCaptions, alColumns, ds);
        gvCB.DataSource = dt;
        gvCB.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    
    protected void gvCB_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
      
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
            HtmlGenericControl gc = (HtmlGenericControl)e.Item.FindControl("spnPickListEmployeeAdd");
            LinkButton emp = (LinkButton)e.Item.FindControl("btnShowEmployeeAdd");
            if (emp != null) emp.Attributes.Add("onclick", "showPickList('" + gc.ClientID + "', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListEmployee.aspx', false); return false;");

        }
    }
    private bool IsValidCareerBreak(string EmployeeId, string FromDate)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(EmployeeId).HasValue)
            ucError.ErrorMessage = "Employee is required.";

        if (!General.GetNullableDateTime(FromDate).HasValue)
            ucError.ErrorMessage = "From Date is required.";

        return (!ucError.IsError);
    }
    protected void RegistersQuick_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }      
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDFROMDATE", "FLDTODATE", "FLDREMARKS" };
                string[] alCaptions = { "File No", "Employee Name", "Rank", "From Date", "To Date", "Remarks" };
                string sortexpression;
                int? sortdirection = null;
                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataTable dt = PhoenixCrewCareerBreak.SearchEmployeeCareerBreak(null, null, null, sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);           
                General.ShowExcel("Crew Carrer", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCB_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string EmployeeId = ((RadTextBox)e.Item.FindControl("txtEmployeeIdAdd")).Text;
                string FromDate = ((UserControlDate)e.Item.FindControl("txtFromDateAdd")).Text; ;
                string ToDate = ((UserControlDate)e.Item.FindControl("txtToDateAdd")).Text;
                string Remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
                if (!IsValidCareerBreak(EmployeeId, FromDate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewCareerBreak.InsertEmployeeCareerBreak(int.Parse(EmployeeId), DateTime.Parse(FromDate), General.GetNullableDateTime(ToDate), Remarks);
                Rebind();
                
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid dtkey = new Guid(((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                string FromDate = ((UserControlDate)e.Item.FindControl("txtFromDate")).Text; ;
                string ToDate = ((UserControlDate)e.Item.FindControl("txtToDate")).Text;
                string Remarks = ((RadTextBox)e.Item.FindControl("txtRemarks")).Text;
                if (!IsValidCareerBreak("1", FromDate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewCareerBreak.UpdateEmployeeCareerBreak(dtkey, DateTime.Parse(FromDate), General.GetNullableDateTime(ToDate), Remarks);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string dtkey = ((RadLabel)e.Item.FindControl("lbldtkey")).Text;
                PhoenixCrewCareerBreak.DeleteEmployeeCareerBreak(new Guid(dtkey));
                Rebind();
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
    protected void gvCB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCB.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
