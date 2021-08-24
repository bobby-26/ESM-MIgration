using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionMachineryDamageRootCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageRootCause.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRootCause')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageRootCause.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuInspectionRootCause.AccessRights = this.ViewState;
            MenuInspectionRootCause.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvRootCause.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDROOTCAUSENAME", "FLDOTHERSYESNO" };
        string[] alCaptions = { "S.No", "Root Cause", "Others Yes/No" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionMachineryDamageRootCause.RootCauseSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(txtRootCauseName.Text)
                                                                    , sortexpression, sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=MachineryDamageRootCause.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Machinery Damage Root Cause</h3></td>");
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

    protected void InspectionRootCause_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvRootCause.CurrentPageIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                gvRootCause.Rebind();
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
        gvRootCause.SelectedIndexes.Clear();
        gvRootCause.EditIndexes.Clear();
        gvRootCause.DataSource = null;
        gvRootCause.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSERIALNUMBER", "FLDROOTCAUSENAME", "FLDOTHERSYESNO" };
        string[] alCaptions = { "S.No", "Root Cause", "Others Yes/No" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionMachineryDamageRootCause.RootCauseSearch(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(txtRootCauseName.Text)
                                                                    , sortexpression, sortdirection
                                                                    , gvRootCause.CurrentPageIndex + 1
                                                                    , gvRootCause.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    );

        General.SetPrintOptions("gvRootCause", "Machinery Damage RootCause Cause", alCaptions, alColumns, ds);

        gvRootCause.DataSource = ds;
        gvRootCause.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvRootCause_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberAdd")).Text;
                string rootcausename = ((RadTextBox)e.Item.FindControl("txtRootCauseNameAdd")).Text;
                string othersyn = ((CheckBox)e.Item.FindControl("chkOthersYNAdd")).Checked == true ? "1" : "0";

                if (!IsValidData(rootcausename))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionMachineryDamageRootCause.InsertRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableInteger(serialnumber)
                    , rootcausename
                    , null
                    , General.GetNullableInteger(othersyn)
                    );
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid rootcauseid = new Guid(item.GetDataKeyValue("FLDROOTCAUSEID").ToString());

                PhoenixInspectionMachineryDamageRootCause.DeleteRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                rootcauseid);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid rootcauseid = new Guid(item.GetDataKeyValue("FLDROOTCAUSEID").ToString());

                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberEdit")).Text;
                string rootcausename = ((RadTextBox)e.Item.FindControl("txtRootCauseNameEdit")).Text;
                string othersyn = ((CheckBox)e.Item.FindControl("chkOthersYNEdit")).Checked == true ? "1" : "0";

                if (!IsValidData(rootcausename))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionMachineryDamageRootCause.UpdateRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , rootcauseid
                                            , General.GetNullableInteger(serialnumber)
                                            , rootcausename
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
    protected void gvRootCause_ItemDataBound(object sender, GridItemEventArgs e)
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
    private bool IsValidData(string rootcause)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (rootcause.Equals(""))
            ucError.ErrorMessage = "Root Cause is required.";

        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvRootCause_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRootCause.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvRootCause_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        Rebind();
    }
}

