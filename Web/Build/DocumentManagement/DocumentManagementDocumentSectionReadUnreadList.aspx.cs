using System;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagementDocumentSectionReadUnreadList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentSectionReadUnreadList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvUsers')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuReadUnread.AccessRights = this.ViewState;
            MenuReadUnread.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["DOCUMENTID"] = "";
                ViewState["SECTIONID"] = "";
                ViewState["READUNREAD"] = "UNREAD";
                ViewState["OFFICEVESSEL"] = 0;
                ViewState["PAGENUMBER"] = 1;
                ViewState["TOTALPAGECOUNT"] = "";
                ViewState["COMPANYID"] = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

                lblDepartment.Visible = false;
                ddlDepartment.Visible = false;

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != "")
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != "")
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();

                BindDepartment();
                BindVessel();
                ucVessel.SelectedIndex = 0;
                gvUsers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);              
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    ucVessel.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cbReadUnread_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbReadUnread.SelectedValue.Equals("1"))
        {
            ViewState["READUNREAD"] = "READ";
            gvUsers.Rebind();
        }
        if (cbReadUnread.SelectedValue.Equals("2"))
        {
            ViewState["READUNREAD"] = "ALL";
            gvUsers.Rebind();
        }
        else
        {
            ViewState["READUNREAD"] ="UNREAD";
            gvUsers.Rebind();            
        }
    }
    protected void BindDepartment()
    {
        ddlDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 2, null);
        ddlDepartment.DataBind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            //string a = null;
            DataSet ds = new DataSet();

            string[] alColumns = { "FLDEMPLOYEECODE", "FLDNAME", "FLDDEPARTMENTNAME", "FLDGROUPRANK","FLDREADDATE" };
            string[] alCaptions = { "Code", "Name", "Department", "Group Rank / Designation","Read Date" };

            if (ViewState["OFFICEVESSEL"].ToString().Equals("0"))
            {
                ds = PhoenixDocumentManagementDashBoard.ReadUnreadCrewMember( General.GetNullableInteger(ucVessel.SelectedValue)
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                    , General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                    , General.GetNullableString(ViewState["READUNREAD"].ToString())
                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvUsers.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

                gvUsers.Columns[2].Visible = false;

                if (ViewState["READUNREAD"].ToString().Equals("UNREAD"))
                {
                    gvUsers.Columns[4].Visible = false;
                }
                else
                {
                    gvUsers.Columns[4].Visible = true;
                }
            }

            else
            {
                ds = PhoenixDocumentManagementDashBoard.ReadUnreadOfficeStaff(General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                     , General.GetNullableInteger(ddlDepartment.SelectedValue)
                     , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                     , General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                     , General.GetNullableString(ViewState["READUNREAD"].ToString())
                     , int.Parse(ViewState["PAGENUMBER"].ToString())
                     , gvUsers.PageSize
                     , ref iRowCount
                     , ref iTotalPageCount);

                gvUsers.Columns[2].Visible = true;

                if (ViewState["READUNREAD"].ToString().Equals("UNREAD"))
                {
                    gvUsers.Columns[4].Visible = false;
                }
                else
                {
                    gvUsers.Columns[4].Visible = true;
                }
            }

            General.SetPrintOptions("gvUsers", "Document/Sections - Read/Unread User List", alCaptions, alColumns, ds);

            gvUsers.DataSource = ds;
            gvUsers.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvUsers.Rebind();
    }

    protected void gvUsers_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
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
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvUsers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUsers.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuReadUnread_TabStripCommand(object sender, EventArgs e)
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
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDEMPLOYEECODE", "FLDNAME", "FLDDEPARTMENTNAME", "FLDGROUPRANK", "FLDREADDATE" };
            string[] alCaptions = { "Code", "Name", "Department", "Grouprank / Designation", "Read Date" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            if (ViewState["OFFICEVESSEL"].ToString().Equals("0"))
            {
                ds = PhoenixDocumentManagementDashBoard.ReadUnreadCrewMember(General.GetNullableInteger(ucVessel.SelectedValue)
                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                    , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                    , General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                    , General.GetNullableString(ViewState["READUNREAD"].ToString())
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);

            }

            else
            {
                ds = PhoenixDocumentManagementDashBoard.ReadUnreadOfficeStaff(General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                     , General.GetNullableInteger(ddlDepartment.SelectedValue)
                     , General.GetNullableGuid(ViewState["DOCUMENTID"].ToString())
                     , General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                     , General.GetNullableString(ViewState["READUNREAD"].ToString())
                     , 1
                     , iRowCount
                     , ref iRowCount
                     , ref iTotalPageCount);

            }

            General.ShowExcel("Document/Sections - Read/Unread User List", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cbOfficeVessel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if(cbOfficeVessel.SelectedValue.Equals("1"))
            {
                ViewState["OFFICEVESSEL"] = 1;
                lblVessel.Visible = false;
                ucVessel.SelectedValue = string.Empty;
                ucVessel.Visible = false;
                lblDepartment.Visible = true;
                ddlDepartment.Visible = true;
                cbReadUnread_SelectedIndexChanged(sender, e);
                gvUsers.Rebind();             
            }
            else
            {
                ViewState["OFFICEVESSEL"] = 0;
                lblVessel.Visible = true;
                ucVessel.Visible = true;
                ddlDepartment.ClearSelection();
                lblDepartment.Visible = false;
                ddlDepartment.Visible = false;
                cbReadUnread_SelectedIndexChanged(sender, e);
                gvUsers.Rebind();
            }
            
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }



    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvUsers.Rebind();
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvUsers.Rebind();
    }
    protected void BindVessel()
    {
        DataSet dt = new DataSet();
        dt = PhoenixRegistersVessel.ListVessel();
        ucVessel.DataSource = dt; 
        ucVessel.DataBind();
       
    }
}