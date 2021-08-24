using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionGFTRootCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionGFTRootCause.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvGFTRootCause')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionGFTRootCause.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuInspectionGFTRootCause.AccessRights = this.ViewState;
            MenuInspectionGFTRootCause.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindBasicFactor();
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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDROOTCAUSENAME" };
        string[] alCaptions = { "S.No", "Root Cause" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionGFTRootCause.RootCauseSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , null
                                                            , General.GetNullableString(txtRootCauseName.Text)
                                                            , sortexpression, sortdirection
                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvGFTRootCause.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );

        Response.AddHeader("Content-Disposition", "attachment; filename=GFTRootCause.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>GFT Root Cause</h3></td>");
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

    protected void InspectionGFTRootCause_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvGFTRootCause.Rebind();
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDROOTCAUSENAME" };
        string[] alCaptions = { "S.No", "Root Cause" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionGFTRootCause.RootCauseSearch(
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableGuid(ddlBasicFactor.SelectedValue)
                                                            , General.GetNullableString(txtRootCauseName.Text)
                                                            , sortexpression, sortdirection
                                                            , gvGFTRootCause.CurrentPageIndex + 1
                                                            , gvGFTRootCause.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            );

        General.SetPrintOptions("gvGFTRootCause", "GFT Root Cause", alCaptions, alColumns, ds);

        gvGFTRootCause.DataSource = ds;
        gvGFTRootCause.VirtualItemCount = iRowCount;
    }

    protected void Rebind()
    {
        gvGFTRootCause.SelectedIndexes.Clear();
        gvGFTRootCause.EditIndexes.Clear();
        gvGFTRootCause.DataSource = null;
        gvGFTRootCause.Rebind();
    }
    protected void gvGFTRootCause_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberAdd")).Text;
                string rootcausename = ((TextBox)e.Item.FindControl("txtRootCauseNameAdd")).Text;

                if (!IsValidData(serialnumber, rootcausename))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionGFTRootCause.InsertRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ddlBasicFactor.SelectedValue)
                    , General.GetNullableString(serialnumber)
                    , rootcausename
                    );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid rootcauseid = new Guid(item.GetDataKeyValue("FLDROOTCAUSEID").ToString());

                PhoenixInspectionGFTRootCause.DeleteRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                rootcauseid);
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string serialnumber = ((UserControlMaskNumber)e.Item.FindControl("txtSerialNumberEdit")).Text;
                string rootcausename = ((TextBox)e.Item.FindControl("txtRootCauseNameEdit")).Text;

                if (!IsValidData(serialnumber, rootcausename))
                {
                    ucError.Visible = true;
                    return;
                }
                GridDataItem item = e.Item as GridDataItem;
                Guid rootcauseid = new Guid(item.GetDataKeyValue("FLDROOTCAUSEID").ToString());

                PhoenixInspectionGFTRootCause.UpdateRootCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , General.GetNullableGuid(ddlBasicFactor.SelectedValue)
                                            , rootcauseid
                                            , General.GetNullableString(serialnumber)
                                            , rootcausename
                                            );
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvGFTRootCause_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
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

    protected void ddlBasicFactor_Changed(object sender, EventArgs e)
    {
        gvGFTRootCause.Rebind();
    }
    protected void BindBasicFactor()
    {
        ddlBasicFactor.DataSource = PhoenixInspectionGFTBasicFactor.ListBasicFactor(1, null);
        ddlBasicFactor.DataBind();
        ddlBasicFactor.DataTextField = "FLDBASICFACTORNAME";
        ddlBasicFactor.DataValueField = "FLDBASICFACTORID";
        ddlBasicFactor.DataBind();
    }

    private bool IsValidData(string serialnumber, string rootcause)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (serialnumber.Equals(""))
            ucError.ErrorMessage = "Serial Number is required.";

        if (rootcause.Equals(""))
            ucError.ErrorMessage = "Root Cause is required.";

        return (!ucError.IsError);

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }


    protected void gvGFTRootCause_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

