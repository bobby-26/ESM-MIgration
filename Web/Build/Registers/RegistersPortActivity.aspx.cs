using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersPortActivity : PhoenixBasePage
{
   protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersPortActivity.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPortActivity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersPortActivity.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersPortActivity.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersPortActivity.AccessRights = this.ViewState;
            MenuRegistersPortActivity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPortActivity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        string[] alColumns = { "FLDPORTACTIVITYNAME", "FLDACTIVE" };
        string[] alCaptions = { "Name", "Active Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersPortActivity.PortActivitySearch(0, txtSearch.Text,null, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=PortActivity.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Port Activity</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
   
    protected void RegistersPortActivity_TabStripCommand(object sender, EventArgs e)
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
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                txtSearch.Text = "";
                Rebind();
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
        gvPortActivity.SelectedIndexes.Clear();
        gvPortActivity.EditIndexes.Clear();
        gvPortActivity.DataSource = null;
        gvPortActivity.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPORTACTIVITYNAME", "FLDACTIVE" };
        string[] alCaptions = { "Name", "Active Y/N" };
       
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersPortActivity.PortActivitySearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, txtSearch.Text,null, sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvPortActivity.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvPortActivity", "Port Activity", alCaptions, alColumns, ds);

        gvPortActivity.DataSource = ds;
        gvPortActivity.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvPortActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidPortActivity(((RadTextBox)e.Item.FindControl("txtPortActivityNameAdd")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                InsertPortActivity(
                    ((RadTextBox)e.Item.FindControl("txtPortActivityNameAdd")).Text,
                     (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtPortActivityNameAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidPortActivity(((RadTextBox)e.Item.FindControl("txtPortActivityNameEdit")).Text))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                UpdatePortActivity(
                     Int32.Parse(((RadLabel)e.Item.FindControl("lblPortActivityCodeEdit")).Text),
                     ((RadTextBox)e.Item.FindControl("txtPortActivityNameEdit")).Text,
                    (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked.Equals(true)) ? 1 : 0
                 );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["PortActivityCode"] = ((RadLabel)e.Item.FindControl("lblPortActivityCode")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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
    protected void gvPortActivity_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
        
        if (e.Item is GridFooterItem)
        {
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
        }
    }
    private void InsertPortActivity(string PortActivityname, int isactive)
    {
        PhoenixRegistersPortActivity.InsertPortActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PortActivityname, isactive);
    }

    private void UpdatePortActivity(int PortActivitycode, string PortActivityname, int isactive)
    {
        PhoenixRegistersPortActivity.UpdatePortActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PortActivitycode, PortActivityname, isactive);
        ucStatus.Text = "Port Activity information updated successfully";
    }

    private bool IsValidPortActivity(string PortActivityname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvPortActivity;

        if (PortActivityname.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    private void DeletePortActivity(int PortActivitycode)
    {
        PhoenixRegistersPortActivity.DeletePortActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PortActivitycode);
    }
    protected void gvPortActivity_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPortActivity.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPortActivity_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeletePortActivity(Int32.Parse(ViewState["PortActivityCode"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
