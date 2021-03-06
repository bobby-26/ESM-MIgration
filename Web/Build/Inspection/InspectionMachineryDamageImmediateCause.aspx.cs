using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionMachineryDamageImmediateCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageImmediateCause.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvImmediateCause')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageImmediateCause.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuInspectionImmediateCause.AccessRights = this.ViewState;
            MenuInspectionImmediateCause.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvImmediateCause.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDIMMEDIATECAUSENAME", "FLDOTHERSYESNO" };
        string[] alCaptions = { "S.No", "Immediate Cause", "Others Yes/No" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionMachineryDamageImmediateCause.ImmediateCauseSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(txtImmediateCauseName.Text)
                                                                    , sortexpression, sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=MachineryDamageImmediateCause.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Machinery Damage Immediate Cause</h3></td>");
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

    protected void InspectionImmediateCause_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvImmediateCause.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                gvImmediateCause.Rebind();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSERIALNUMBER", "FLDIMMEDIATECAUSENAME", "FLDOTHERSYESNO" };
        string[] alCaptions = { "S.No", "Immediate Cause", "Others Yes/No" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionMachineryDamageImmediateCause.ImmediateCauseSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(txtImmediateCauseName.Text)
                                                                    , sortexpression, sortdirection
                                                                    , gvImmediateCause.CurrentPageIndex + 1
                                                                    , gvImmediateCause.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    );

        General.SetPrintOptions("gvImmediateCause", "Machinery Damage ImmediateCause", alCaptions, alColumns, ds);

        gvImmediateCause.DataSource = ds;
        gvImmediateCause.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void Rebind()
    {
        gvImmediateCause.SelectedIndexes.Clear();
        gvImmediateCause.EditIndexes.Clear();
        gvImmediateCause.DataSource = null;
        gvImmediateCause.Rebind();
    }
    protected void gvImmediateCause_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string serialnumber = "";
                UserControlMaskNumber ucserial = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberAdd"));
                if (ucserial != null)
                    serialnumber = ucserial.Text;
                string immediatecausename = ((RadTextBox)e.Item.FindControl("txtImmediateCauseNameAdd")).Text;
                string othersyn = ((CheckBox)e.Item.FindControl("chkOthersYNAdd")).Checked == true ? "1" : "0";

                if (!IsValidData(immediatecausename))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionMachineryDamageImmediateCause.InsertImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(serialnumber)
                    , immediatecausename
                    , null
                    , General.GetNullableInteger(othersyn)
                    );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid immediatecauseid = new Guid(item.GetDataKeyValue("FLDIMMEDIATECAUSEID").ToString());

                PhoenixInspectionMachineryDamageImmediateCause.DeleteImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                immediatecauseid);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberEdit")).Text;
                string immediatecausename = ((RadTextBox)e.Item.FindControl("txtImmediateCauseNameEdit")).Text;
                string othersyn = ((CheckBox)e.Item.FindControl("chkOthersYNEdit")).Checked == true ? "1" : "0";

                if (!IsValidData(immediatecausename))
                {
                    ucError.Visible = true;
                    return;
                }
                GridDataItem item = e.Item as GridDataItem;
                Guid immediatecauseid = new Guid(item.GetDataKeyValue("FLDIMMEDIATECAUSEID").ToString());

                PhoenixInspectionMachineryDamageImmediateCause.UpdateImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , immediatecauseid
                                            , General.GetNullableInteger(serialnumber)
                                            , immediatecausename
                                            , null
                                            , General.GetNullableInteger(othersyn));
                Rebind();
            }         
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvImmediateCause_ItemDataBound(object sender, GridItemEventArgs e)
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

    private bool IsValidData(string immediatecause)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (immediatecause.Equals(""))
            ucError.ErrorMessage = "Immediate Cause is required.";

        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvImmediateCause_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvImmediateCause.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvImmediateCause_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }
}

