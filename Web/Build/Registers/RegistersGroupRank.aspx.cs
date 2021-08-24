using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersGroupRank : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersGroupRank.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegistersGroupRank')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersGroupRank.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersGroupRank.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersGroupRank.AccessRights = this.ViewState;
            MenuRegistersGroupRank.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvRegistersGroupRank.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

        string[] alColumns = { "FLDRANKCODE", "FLDGROUPRANK", "FLDACTIVE", "FLDJHAYN", "FLDHSEQADASHBOARDYN" };
        string[] alCaptions = { "Code", "Name","Active YN", "JHA", "HSEQA Dashboard YN" };
        string sortexpression; 
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersGroupRank.GroupRankSearch(General.GetNullableString(txtRankShortCode.Text)
                                                           , General.GetNullableString(txtRankName.Text)
                                                           , sortexpression, sortdirection
                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                           , gvRegistersGroupRank.PageSize
                                                           , ref iRowCount
                                                           , ref iTotalPageCount
                                                           );

        Response.AddHeader("Content-Disposition", "attachment; filename=InspectionEvent.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inspection Event</h3></td>");
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

    protected void MenuRegistersGroupRank_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRegistersGroupRank.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtRankName.Text = "";
                txtRankShortCode.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        string[] alColumns = { "FLDRANKCODE", "FLDGROUPRANK", "FLDACTIVE", "FLDJHAYN", "FLDHSEQADASHBOARDYN" };
        string[] alCaptions = { "Code", "Name", "Active YN", "JHA", "HSEQA Dashboard YN" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixRegistersGroupRank.GroupRankSearch(General.GetNullableString(txtRankName.Text)
                                                     , General.GetNullableString(txtRankShortCode.Text)
                                                     , sortexpression, sortdirection
                                                     , gvRegistersGroupRank.CurrentPageIndex + 1
                                                     , gvRegistersGroupRank.PageSize
                                                     , ref iRowCount
                                                     , ref iTotalPageCount
                                                    );


        General.SetPrintOptions("gvRegistersGroupRank", "Group Rank", alCaptions, alColumns, ds);

        gvRegistersGroupRank.DataSource = ds;
        gvRegistersGroupRank.VirtualItemCount = iRowCount;

    }

    protected void Rebind()
    {
        gvRegistersGroupRank.SelectedIndexes.Clear();
        gvRegistersGroupRank.EditIndexes.Clear();
        gvRegistersGroupRank.DataSource = null;
        gvRegistersGroupRank.Rebind();
    }

    protected void gvRegistersGroupRank_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string shortcode = ((RadTextBox)e.Item.FindControl("txtGroupRankShortCodeAdd")).Text;
                string eventname = ((RadTextBox)e.Item.FindControl("txtGroupRankNameAdd")).Text;
               
                if (!IsValidData(shortcode, eventname))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersGroupRank.GroupRankInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , eventname
                    , shortcode
                    , (((CheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                    , (((CheckBox)e.Item.FindControl("chkJHAYNAdd")).Checked) ? 1 : 0
                    , General.GetNullableInteger(((RadMaskedTextBox)e.Item.FindControl("txtSortLevelAdd")).Text)
                    , (((CheckBox)e.Item.FindControl("chkHSEQAYNNAdd")).Checked) ? 1 : 0
                    );

                gvRegistersGroupRank.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersGroupRank.GroupRankDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(((RadLabel)e.Item.FindControl("lblGroupRankId")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string shortcode = ((RadTextBox)e.Item.FindControl("txtGroupRankShortCodeEdit")).Text;
                string eventname = ((RadTextBox)e.Item.FindControl("txtGroupRankNameEdit")).Text;
                if (!IsValidData(shortcode, eventname))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersGroupRank.GroupRankUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , int.Parse(((RadLabel)e.Item.FindControl("lblGroupRankIdEdit")).Text)
                                            , eventname
                                            , shortcode
                                            , (((CheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                                            , (((CheckBox)e.Item.FindControl("chkJHAYNEdit")).Checked) ? 1 : 0
                                            , General.GetNullableInteger(((RadMaskedTextBox)e.Item.FindControl("txtSortLevelEdit")).Text)
                                            , (((CheckBox)e.Item.FindControl("chkHSEQAYN")).Checked) ? 1 : 0);

                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidData(string EventShortCode, string InspectionEvent)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (EventShortCode.Equals(""))
            ucError.ErrorMessage = "Code is required";

        if (InspectionEvent.Equals(""))
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);

    }

    protected void gvRegistersGroupRank_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRegistersGroupRank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegistersGroupRank.CurrentPageIndex + 1;

        BindData();
    }
}
