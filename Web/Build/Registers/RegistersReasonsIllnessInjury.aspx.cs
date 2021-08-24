using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersReasonsIllnesInjury : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersReasonsIllnessInjury.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvReasonIllnessInjury')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuRegistersReasonsIllnessInjury.AccessRights = this.ViewState;
        MenuRegistersReasonsIllnessInjury.MenuList = toolbar.Show();
        toolbar = new PhoenixToolbar();
        MenuTitle.AccessRights = this.ViewState;
        MenuTitle.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvReasonIllnessInjury.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDREASON", "FLDCATEGORY", "FLDILLNESSYN" };
        string[] alCaptions = { "Reason", "Category", "Illness / Injury Case" };
        string sortexpression;
        int sortdirection ;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersreasonsillnesinjury.reasonsillnesinjurySearch("", "", null
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvReasonIllnessInjury.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ReasonIllnesInjury.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Illness/Injury</h3></td>");
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

    protected void RegistersReasonIllnessInjury_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDREASON", "FLDCATEGORY", "FLDILLNESSYN" };
        string[] alCaptions = { "Reason", "Category", "Illness / Injury Case" };
       
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int sortdirection ;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;          

        DataSet ds = PhoenixRegistersreasonsillnesinjury.reasonsillnesinjurySearch("", "", null
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvReasonIllnessInjury.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvReasonIllnessInjury", "Illness/Injury", alCaptions, alColumns, ds);
       
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvReasonIllnessInjury.DataSource = ds;
            gvReasonIllnessInjury.VirtualItemCount = iRowCount;
        }
        else
        {
            gvReasonIllnessInjury.DataSource = "";
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {        
        BindData();
        gvReasonIllnessInjury.Rebind();
    }

    private void InsertReasonsIllnessInjury(string Reason, string Category, int Illness)
    {
        if (!IsValidReason(Reason, Category))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersreasonsillnesinjury.Insertreasonsillnesinjury(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Reason, Category, Illness);
    }

    private void UpdateReasonsIllnessInjury(int reasonId, string reason, string category, int illness)
    {
        if (!IsValidReason(reason, category))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersreasonsillnesinjury.Updatereasonsillnesinjury(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reasonId, reason, category, illness);
        ucStatus.Text = "Illness/Injury reason information updated";
    }

    private void DeleteReasonsIllnessInjury(int reasonId)
    {
        PhoenixRegistersreasonsillnesinjury.Deletereasonsillnesinjury(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, reasonId);
    }

    private bool IsValidReason(string reason, string category)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (reason.Trim().Equals(""))
            ucError.ErrorMessage = "Reason is required.";

        if (category.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvReasonIllnessInjury_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertReasonsIllnessInjury(
                    ((RadTextBox)e.Item.FindControl("txtReasonAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtCategoryAdd")).Text,
                    ((RadCheckBox)e.Item.FindControl("chkIllnessAdd")).Checked==true ? 1 : 0);
                BindData();
                gvReasonIllnessInjury.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteReasonsIllnessInjury(Int32.Parse(((RadLabel)e.Item.FindControl("lblReasonID")).Text));
                BindData();
                gvReasonIllnessInjury.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                UpdateReasonsIllnessInjury(
               Int16.Parse(((RadLabel)e.Item.FindControl("lblReasonIDEDIT")).Text),
               ((RadTextBox)e.Item.FindControl("txtReasonEdit")).Text,
               ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text,
               ((RadCheckBox)e.Item.FindControl("chkIllnessEdit")).Checked==true ? 1 : 0);
                BindData();
                gvReasonIllnessInjury.Rebind();
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

    protected void gvReasonIllnessInjury_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReasonIllnessInjury.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvReasonIllnessInjury_ItemDataBound(object sender, GridItemEventArgs e)
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

            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
            if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
        }
       
    }
}
