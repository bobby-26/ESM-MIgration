using System;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DocumentManegementDashBoardUnreadUserList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManegementDashBoardUnreadUserList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvUsers')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuReadUnread.AccessRights = this.ViewState;
            MenuReadUnread.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                ViewState["READUNREAD"] = "UNREAD";
                ViewState["OFFICEVESSEL"] = 0;
                ViewState["PAGENUMBER"] = 1;
                ViewState["TOTALPAGECOUNT"] = "";
                
                gvUsers.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDEMPLOYEEID","FLDEMPLOYEECODE", "FLDNAME", "FLDGROUPRANK", "FLDUNREADCOUNT" };
            string[] alCaptions = { "Employee ID","Employee Code", "Name", "Group Rank", "Unread Section Count" };

               ds = PhoenixDocumentManagementDashBoard.ReadUnreadCrewMember(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                    , null
                    , null
                    , null
                    , General.GetNullableString(ViewState["READUNREAD"].ToString())
                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvUsers.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

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

            string[] alColumns = { "FLDEMPLOYEEID", "FLDEMPLOYEECODE", "FLDNAME", "FLDGROUPRANK", "FLDUNREADCOUNT" };
            string[] alCaptions = { "Employee ID", "Employee Code", "Name", "Group Rank", "Unread Section Count" };


            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixDocumentManagementDashBoard.ReadUnreadCrewMember(General.GetNullableInteger(ViewState["VESSELID"].ToString())
                  , null
                  , null
                  , null
                  , General.GetNullableString(ViewState["READUNREAD"].ToString())
                  , 1
                  , iRowCount
                  , ref iRowCount
                  , ref iTotalPageCount);

            General.ShowExcel("Document/Sections - Unread User List", ds.Tables[0], alColumns, alCaptions, null, null);
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvUsers_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton lnkCount = (LinkButton)e.Item.FindControl("lnkCount");
            RadLabel lblCode = (RadLabel)e.Item.FindControl("lblCode");
            RadLabel lblName = (RadLabel)e.Item.FindControl("lblName");

            if (lnkCount != null)
            {
                lnkCount.Attributes.Add("onclick", "javascript: top.openNewWindow('Section','"+ lblName.Text +" - Unread Section List','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDashBoardUserUnreadDocumentSectionList.aspx?EMPLOYEECODE="+ lblCode.Text + "'); return true;");
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}