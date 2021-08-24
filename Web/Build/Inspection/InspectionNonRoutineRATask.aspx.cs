using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionNonRoutineRATask : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionNonRoutineRATask.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvShipboardRATask')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuShipBoardRATasks.AccessRights = this.ViewState;
        MenuShipBoardRATasks.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            Session["New"] = "N";
            if (Session["CHECKED_ITEMS"] != null)
                Session.Remove("CHECKED_ITEMS");


            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            gvShipboardRATask.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != "")
                ViewState["RAID"] = Request.QueryString["RAID"].ToString();
            else
                ViewState["RAID"] = string.Empty;

        }
    }   
    protected void MenuShipBoardRATasks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvShipboardRATask.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            InspectionFilter.CurrentShipBoardRATaskFilter = null;
            BindData();
            gvShipboardRATask.Rebind();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTASK", "FLDPICNAME", "FLDESTIMATEDFINISHDATE"};
        string[] alCaptions = { "Task", "Responsibility", "Target" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        
        DataSet ds = PhoenixInspectionRiskAssessmentMachineryExtn.NonRoutineRATasksSearch(General.GetNullableGuid(ViewState["RAID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvShipboardRATask.CurrentPageIndex + 1
                                                                , gvShipboardRATask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=PendingTaskList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pending RA Tasks</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDTASK", "FLDPICNAME", "FLDESTIMATEDFINISHDATE" };
        string[] alCaptions = { "Task", "Responsibility", "Target" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionRiskAssessmentMachineryExtn.NonRoutineRATasksSearch(General.GetNullableGuid(ViewState["RAID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvShipboardRATask.CurrentPageIndex + 1
                                                                , gvShipboardRATask.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                );

        General.SetPrintOptions("gvShipboardRATask", "Pending RA Tasks", alCaptions, alColumns, ds);

        gvShipboardRATask.DataSource = ds;
        gvShipboardRATask.VirtualItemCount = iRowCount;
    }

    protected void gvShipboardRATask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipboardRATask.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvShipboardRATask_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        RadLabel lblRaMachinaryId = (RadLabel)e.Item.FindControl("lblSourceId");

        if (eb != null)
            eb.Attributes.Add("onclick", "javascript:openNewWindow('Task', '', '" + Session["sitepath"] + "/Inspection/InspectionPendingRATaskEdit.aspx?RAMACHINERYID=" + lblRaMachinaryId.Text + "');return true;");
    }

    protected void gvShipboardRATask_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
}