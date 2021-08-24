using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegistersReasonsSignOn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersReasonsSignOn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReasonSignOn')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersReasonsSignOn.AccessRights = this.ViewState;
        MenuRegistersReasonsSignOn.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvReasonSignOn.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDABBREVIATION", "FLDREASON", "FLDGROUP" };
        string[] alCaptions = { "Code","Reason", "Group" };
        string sortexpression;
        int sortdirection;
        
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersreasonssignon.reasonssignonSearch("", "", ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvReasonSignOn.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ReasonsSignOn.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>SignOn Reason</h3></td>");
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

    protected void RegistersReasonSignOn_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvReasonSignOn.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDABBREVIATION", "FLDREASON", "FLDGROUP" };
        string[] alCaptions = { "Code", "Reason", "Group" };
        
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;
                
        DataSet ds = PhoenixRegistersreasonssignon.reasonssignonSearch("", "", ""
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvReasonSignOn.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvReasonSignOn", "SignOn Reason", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvReasonSignOn.DataSource = ds;
            gvReasonSignOn.VirtualItemCount = iRowCount;
        }
        else
        {
            gvReasonSignOn.DataSource = "";
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {     
        BindData();
        gvReasonSignOn.Rebind();
    }

    private void InsertReasonsSignOn(string reason, string abbreviation, string group)
    {
        if (!IsValidReason(reason, abbreviation, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersreasonssignon.Insertreasonssignon(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reason, abbreviation, group);
    }

    private void UpdateReasonsSignOn(int reasonId, string reason, string abbreviation, string group)
    {
        if (!IsValidReason(reason, abbreviation, group))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersreasonssignon.Updatereasonssignon(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reasonId, reason, abbreviation, group);
        ucStatus.Text = "SignOn reason information updated";
    }

    private bool IsValidReason(string reason, string abbreviation, string group)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (abbreviation.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (reason.Trim().Equals(""))
            ucError.ErrorMessage = "Reason is required.";
    
        if (group.Trim().Equals(""))
            ucError.ErrorMessage = "Group is required.";

        return (!ucError.IsError);
    }

    private void DeleteReasonsSignOn(int reasonId)
    {
        PhoenixRegistersreasonssignon.Deletereasonssignon(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reasonId);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvReasonSignOn_ItemDataBound(object sender, GridItemEventArgs e)
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
        if (e.Item is GridFooterItem)
        {
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
        }
    }

    protected void gvReasonSignOn_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertReasonsSignOn(
                    ((RadTextBox)e.Item.FindControl("txtReasonAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtAbbreviationAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtGroupAdd")).Text);
                BindData();
                gvReasonSignOn.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteReasonsSignOn(Int32.Parse(((RadLabel)e.Item.FindControl("lblReasonID")).Text));
                BindData();
                gvReasonSignOn.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateReasonsSignOn(
              Int16.Parse(((RadLabel)e.Item.FindControl("lblReasonIDEDIT")).Text),
              ((RadTextBox)e.Item.FindControl("txtReasonEdit")).Text,
              ((RadTextBox)e.Item.FindControl("txtAbbreviationEdit")).Text,
              ((RadTextBox)e.Item.FindControl("txtGroupEdit")).Text);
                BindData();
                gvReasonSignOn.Rebind();
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

    protected void gvReasonSignOn_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReasonSignOn.CurrentPageIndex + 1;
        BindData();
    }
}
