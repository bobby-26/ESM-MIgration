using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionQuick : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionQuick.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvQuick')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersQuick.AccessRights = this.ViewState;
            MenuRegistersQuick.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["QuickCodeType"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                string module = Request.QueryString["module"].ToString();
                ucQuickType.QuickTypeGroup = module;
                if (Request.QueryString["quickcodetype"] != null)
                {
                    ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
                    ucQuickType.QuickTypeShowYesNo = "0";
                    CaptionChange(module, ViewState["QuickCodeType"].ToString());
                    ucQuickType.Visible = false;
                    lblRegister.Visible = false;
                }
                else
                {
                    ucQuickType.bind();
                }
            }
            if (Request.QueryString["quickcodetype"] != null)
            {
                ViewState["QuickCodeType"] = Request.QueryString["quickcodetype"].ToString();
                ucQuickType.SelectedQuickType = ViewState["QuickCodeType"].ToString();
            }
            BindFirstValue();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindFirstValue()
    {
        string quicktype = null;
        if (ViewState["QuickCodeType"] == null || ViewState["QuickCodeType"].ToString() == "")
        {
            if (ucQuickType.SelectedQuickType == "")
            {
                ucQuickType.QuickTypeShowYesNo = "1";
                string yesno = ucQuickType.QuickTypeShowYesNo;
                DataSet ds1 = PhoenixRegistersQuick.ListQuickType(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ucQuickType.QuickTypeGroup, Convert.ToInt32(yesno));
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataRow drActivity = ds1.Tables[0].Rows[0];
                    quicktype = drActivity["FLDQUICKTYPECODE"].ToString();
                    ucQuickType.SelectedQuickType = drActivity["FLDQUICKTYPECODE"].ToString();
                    ucQuickType.bind();
                    ViewState["QuickCodeType"] = drActivity["FLDQUICKTYPECODE"].ToString();
                }
            }
        }
    }

    public void CaptionChange(string module, string quicktypecode)
    {
        ucQuickType.Enabled = "false";
        DataSet dsedit = new DataSet();
        dsedit = PhoenixRegistersQuick.ListQuickTypeEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, module,
                                                        Convert.ToInt32(quicktypecode));

        if (dsedit.Tables.Count > 0)
        {
            DataRow drquick = dsedit.Tables[0].Rows[0];
            ucTitle.Text = General.GetMixedCase(drquick["FLDQUICKTYPENAME"].ToString());
            ViewState["MODULENAME"] = ucTitle.Text;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = new string[3];
        string[] alCaptions = new string[3];

        if (ViewState["QuickCodeType"].Equals("47"))
        {
            alColumns[0] = "FLDSHORTNAME";
            alColumns[1] = "FLDQUICKNAME";
            alColumns[2] = "FLDVESSELTYPELIST";

            alCaptions[0] = "Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Vessel Type";
        }
        else
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDSHORTNAME";
            alColumns[1] = "FLDQUICKNAME";

            alCaptions[0] = "Code";
            alCaptions[1] = "Name";
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCommonRegisters.QuickSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ViewState["QuickCodeType"].ToString(), sortexpression, sortdirection,
                    1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

        if (ViewState["MODULENAME"] != null)
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ViewState["MODULENAME"].ToString() + ".xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>" + ViewState["MODULENAME"].ToString() + "</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");

        }
        else
        {
            Response.AddHeader("Content-Disposition", "attachment; filename=\"Other Register.xls\"");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Other Register</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");

        }
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
    protected void Rebind()
    {
        gvQuick.SelectedIndexes.Clear();
        gvQuick.EditIndexes.Clear();
        gvQuick.DataSource = null;
        gvQuick.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[3];
        string[] alCaptions = new string[3];

        if (ViewState["QuickCodeType"].Equals("47"))
        {
            alColumns[0] = "FLDSHORTNAME";
            alColumns[1] = "FLDQUICKNAME";
            alColumns[2] = "FLDVESSELTYPELIST";

            alCaptions[0] = "Code";
            alCaptions[1] = "Name";
            alCaptions[2] = "Vessel Type";

            gvQuick.Columns[2].Visible = true;
            gvQuick.Columns[1].ItemStyle.Width = 500;
            gvQuick.Columns[2].ItemStyle.Width = 350;
        }
        else
        {
            alColumns = new string[2];
            alCaptions = new string[2];

            alColumns[0] = "FLDSHORTNAME";
            alColumns[1] = "FLDQUICKNAME";

            alCaptions[0] = "Code";
            alCaptions[1] = "Name";

            gvQuick.Columns[2].Visible = false;
            gvQuick.Columns[1].ItemStyle.Width = 800;
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCommonRegisters.QuickSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ViewState["QuickCodeType"].ToString(), sortexpression, sortdirection,
                    gvQuick.CurrentPageIndex + 1,
                    gvQuick.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvQuick", "Registers", alCaptions, alColumns, ds);

        gvQuick.DataSource = ds;
        gvQuick.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }


    protected void cmdSort_Click(object sender, EventArgs e)
    {
        LinkButton ib = (LinkButton)sender;
        Rebind();
    }

    protected void gvQuick_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {

                    if (!IsValidQuick(((RadTextBox)e.Item.FindControl("txtQuickNameAdd")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    InsertQuick(
                    ucQuickType.SelectedQuickType,
                        ((RadTextBox)e.Item.FindControl("txtQuickNameAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtShortNameAdd")).Text
                    );
                    gvQuick.Rebind();
                    ((RadTextBox)e.Item.FindControl("txtQuickNameAdd")).Focus();
                }
            }
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("UPDATE"))
                {
                    UpdateQuick(
                           ucQuickType.SelectedQuickType,
                            Int32.Parse(((RadLabel)e.Item.FindControl("lblQuickCodeEdit")).Text),
                             ((RadTextBox)e.Item.FindControl("txtQuickNameEdit")).Text,
                             ((RadTextBox)e.Item.FindControl("txtShortNameEdit")).Text
                         );
                    Rebind();
                }

                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    DeleteQuick(Int32.Parse(((RadLabel)e.Item.FindControl("lblQuickCode")).Text));
                    Rebind();
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvQuick_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        if (e.Item is GridEditableItem)
        {
            if (Request.QueryString["quickcodetype"] != null)
            {
                if (Request.QueryString["quickcodetype"].ToString() == "64")
                {
                    //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                    //{
                    LinkButton lnkCode = (LinkButton)e.Item.FindControl("lnkQuickCodeHeader");
                    if (lnkCode != null) lnkCode.Text = "Cancel Code";

                    LinkButton lnkName = (LinkButton)e.Item.FindControl("lblQuickNameHeader");
                    if (lnkName != null) lnkName.Text = "Cancel Reason";
                    // }
                }
                else if (Request.QueryString["quickcodetype"].ToString() == "65")
                {
                    //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                    //{
                    LinkButton lnkCode = (LinkButton)e.Item.FindControl("lnkQuickCodeHeader");
                    if (lnkCode != null) lnkCode.Text = "Approval Code";

                    LinkButton lnkName = (LinkButton)e.Item.FindControl("lblQuickNameHeader");
                    if (lnkName != null) lnkName.Text = "Approval Authority";
                    //  }
                }
            }
        }

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.RowState.Equals(DataControlRowState.Edit))
            //{
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            // }

            ImageButton cmdMap = (ImageButton)e.Item.FindControl("cmdTypeMapping");

            if (cmdMap != null)
            {
                cmdMap.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionDeficiencyCategoryVesselTypeMapping.aspx?categoryid=" + drv["FLDQUICKCODE"].ToString() + "&category=" + drv["FLDQUICKNAME"].ToString() + "');return true;");

                if (ucQuickType.SelectedQuickType.Equals("47"))
                {
                    cmdMap.Visible = true;
                }
                else
                {
                    cmdMap.Visible = false;
                }
            }

            RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
            if (lblType != null)
            {
                UserControlToolTip ucToolTipType = (UserControlToolTip)e.Item.FindControl("ucToolTipType");
                ucToolTipType.Position = ToolTipPosition.TopCenter;
                ucToolTipType.TargetControlId = lblType.ClientID;
                //lblType.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipType.ToolTip + "', 'visible');");
                //lblType.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipType.ToolTip + "', 'hidden');");
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

    protected void ucQuick_changed(object sender, EventArgs e)
    {
        ViewState["QuickCodeType"] = ucQuickType.SelectedQuickType;
        Rebind();
    }

    private void InsertQuick(string Quicktypecode, string Quickname, string Shortname)
    {

        PhoenixRegistersQuick.InsertQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickname, Shortname);
    }

    private void UpdateQuick(string Quicktypecode, int Quickcode, string Quickname, string shortname)
    {
        if (!IsValidQuick(Quickname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersQuick.UpdateQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(Quicktypecode), Quickcode, Quickname, shortname);
        ucStatus.Text = "Information updated";
    }

    private bool IsValidQuick(string Quickname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //  GridView _gridView = gvQuick;

        if (Quickname.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Quick Name is required.";
        }
        return (!ucError.IsError);
    }

    private void DeleteQuick(int Quickcode)
    {
        PhoenixRegistersQuick.DeleteQuick(0, Quickcode);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvQuick.Rebind();
    }

    protected void gvQuick_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
