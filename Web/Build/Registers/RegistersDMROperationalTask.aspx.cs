using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDMROperationalTask : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersDMROperationalTask.aspx", "Export to Excel","<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOperationalTask')", "Print Grid","<i class=\"fas fa-print\"></i>", "PRINT");
            MenuOperationalTask.AccessRights = this.ViewState;
            MenuOperationalTask.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvOperationalTask.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOperationalTask_TabStripCommand(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationalTask_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text),
                    (((RadTextBox)e.Item.FindControl("txtTaskNameAdd")).Text),
                    ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text))
                    return;

                PhoenixRegistersDMROperationalTask.DMROperationalTaskInsert(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadTextBox)e.Item.FindControl("txtTaskNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderAdd")).Text)
                     , ((RadCheckBox)e.Item.FindControl("chkDpActivityYNAdd")).Checked == true ? 1 : 0
                     , ((RadCheckBox)e.Item.FindControl("chkDistanceApplicableAdd")).Checked == true ? 1 : 0);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["OperationalTaskId"] = ((RadLabel)e.Item.FindControl("lblOperationalTaskId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!checkvalue((((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text),
                    (((RadTextBox)e.Item.FindControl("txtTaskNameEdit")).Text),
                    ((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text))
                    return;

                PhoenixRegistersDMROperationalTask.DMROperationalTaskUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblOperationalTaskIdEdit")).Text),
                    ((RadTextBox)e.Item.FindControl("txtTaskNameEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSortOrderEdit")).Text),
                    ((RadCheckBox)e.Item.FindControl("chkDpActivityYNEdit")).Checked == true ? 1 : 0,
                    ((RadCheckBox)e.Item.FindControl("chkDistanceApplicabledit")).Checked == true ? 1 : 0);

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool checkvalue(string shortName, string taskName, string sortOrder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((shortName == null) || (shortName == ""))
            ucError.ErrorMessage = "Short name is required.";

        if ((taskName == null) || (taskName == ""))
            ucError.ErrorMessage = "Operation task name is required.";

        if ((sortOrder == null) || (sortOrder == ""))
            ucError.ErrorMessage = "Sort order is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDTASKNAME","FLDDPACTIVITYYN", "FLDSORTORDER","FLDDISTANCEAPPLICABLE" };
        string[] alCaptions = { "Short Code", "Description", "DP Activity Y/N", "Sort Order", "Distance Applicable" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskSearch("",
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            gvOperationalTask.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvOperationalTask", "Vessel Status", alCaptions, alColumns, ds);
        gvOperationalTask.DataSource = ds;
        gvOperationalTask.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvOperationalTask_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationalTask_Sorting(object sender, GridViewSortEventArgs se)
    {
        
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSHORTNAME", "FLDTASKNAME", "FLDDPACTIVITYYN", "FLDSORTORDER", "FLDDISTANCEAPPLICABLE" };
        string[] alCaptions = { "Short Code", "Description", "DP Activity Y/N", "Sort Order", "Distance Applicable" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersDMROperationalTask.DMROperationalTaskSearch("",
            sortexpression, sortdirection, 1,
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"Vessel Status.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel Status</h3></td>");
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

    protected void gvOperationalTask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOperationalTask.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOperationalTask.SelectedIndexes.Clear();
        gvOperationalTask.EditIndexes.Clear();
        gvOperationalTask.DataSource = null;
        gvOperationalTask.Rebind();
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegistersDMROperationalTask.DMROperationalTaskDelete(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["OperationalTaskId"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
