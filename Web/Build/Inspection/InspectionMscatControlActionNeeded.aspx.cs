using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionMscatControlActionNeeded : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMscatControlActionNeeded.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvControlActionNeeded')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Copy','','" + Session["sitepath"] + "/Inspection/InspectionMSCATMapping.aspx?type=4',null);return true;", "Map Basic Cause", "<i class=\"fas fa-tasks\"></i>", "Map");
            MenuInspectionControlActionNeeds.AccessRights = this.ViewState;
            MenuInspectionControlActionNeeds.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvControlActionNeeded.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindContactType();
                BindImmediateCause();
                BindBasicCause();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindImmediateCause()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionImmediateCause.ListImmediateCause(General.GetNullableGuid(ddlContactType.SelectedValue));
        ddlImmediateCause.DataSource = ds;
        ddlImmediateCause.DataTextField = "FLDIMMEDIATECAUSE";
        ddlImmediateCause.DataValueField = "FLDIMMEDIATECAUSEID";
        ddlImmediateCause.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDCONTROLACTIONNEEDED" };
        string[] alCaptions = { "S.No", "Control Action Needs" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionMscatControlActionNeeded.ControlActionNeededSearch(
                                                                               General.GetNullableGuid(ddlBasicCause.SelectedValue),
                                                                               General.GetNullableGuid(ddlImmediateCause.SelectedValue),
                                                                               General.GetNullableGuid(ddlContactType.SelectedValue),
                                                                               General.GetNullableInteger(ddlcause.SelectedHard),
                                                                               sortexpression,
                                                                               sortdirection,
                                                                               1,
                                                                               iRowCount,
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ControlActionNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Control Action Needs</h3></td>");
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
    protected void InspectionControlActionNeeds_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDCONTROLACTIONNEEDED" };
        string[] alCaptions = { "S.No", "Control Action Needs" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionMscatControlActionNeeded.ControlActionNeededSearch(
                                                                                General.GetNullableGuid(ddlBasicCause.SelectedValue),
                                                                                General.GetNullableGuid(ddlImmediateCause.SelectedValue),
                                                                                General.GetNullableGuid(ddlContactType.SelectedValue),
                                                                                General.GetNullableInteger(ddlcause.SelectedHard),
                                                                                sortexpression,
                                                                                sortdirection,
                                                                                gvControlActionNeeded.CurrentPageIndex + 1,
                                                                                gvControlActionNeeded.PageSize,
                                                                                ref iRowCount,
                                                                                ref iTotalPageCount);

        General.SetPrintOptions("gvControlActionNeeded", "Control Action Needs", alCaptions, alColumns, ds);

        gvControlActionNeeded.DataSource = ds;
        gvControlActionNeeded.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;

    }
    protected void Rebind()
    {
        gvControlActionNeeded.SelectedIndexes.Clear();
        gvControlActionNeeded.EditIndexes.Clear();
        gvControlActionNeeded.DataSource = null;
        gvControlActionNeeded.Rebind();
    }
    protected void gvControlActionNeeded_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidControlActionNeeded(((RadTextBox)e.Item.FindControl("txtControlActionNeededAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertControlActionNeeded(((RadTextBox)e.Item.FindControl("txtControlActionNeededAdd")).Text
                    , ((TextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                    , int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text)
                    );
                ((RadTextBox)e.Item.FindControl("txtControlActionNeededAdd")).Focus();
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidControlActionNeeded(((RadTextBox)e.Item.FindControl("txtControlActionNeededEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateControlActionNeeded(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblControlActionNeededIdEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtControlActionNeededEdit")).Text
                    , ((TextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                    , int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text)
                    );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMscatControlActionNeeded.DeleteControlActionNeeded(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblControlActionNeededId")).Text));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvControlActionNeeded_ItemDataBound(object sender, GridItemEventArgs e)
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

    private bool IsValidControlActionNeeded(string controlactionneeded, string sequencenumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(sequencenumber))
            ucError.ErrorMessage = "Serial Number is required.";

        if (controlactionneeded.Trim().Equals(""))
            ucError.ErrorMessage = "Control Action Needs is required.";

        return (!ucError.IsError);
    }

    private void InsertControlActionNeeded(string controlactionneeded, string description, int serialnumber)
    {
        PhoenixInspectionMscatControlActionNeeded.InsertControlActionNeeded(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , controlactionneeded
            , description
            , serialnumber
            );
    }

    private void UpdateControlActionNeeded(Guid? controlactionneededid, string controlactionneeded, string description, int serialnumber)
    {
        PhoenixInspectionMscatControlActionNeeded.UpdateControlActionNeeded(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , controlactionneededid
            , controlactionneeded
            , description
            , serialnumber
            );
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void ddlcause_TextChanged(object sender, EventArgs e)
    {
        BindContactType();
        BindImmediateCause();
        BindBasicCause();
        BindData();
    }

    protected void ddlContactType_TextChanged(object sender, EventArgs e)
    {
        BindImmediateCause();
        BindBasicCause();
        BindData();
    }

    protected void ddlImmediateCause_indexchanged(object sender, EventArgs e)
    {
        BindBasicCause();
        gvControlActionNeeded.Rebind();
    }

    protected void ddlBasicCause_TextChanged(object sender, EventArgs e)
    {
        gvControlActionNeeded.Rebind();
    }

    protected void BindBasicCause()
    {
        ddlBasicCause.Items.Clear();
        ddlBasicCause.DataSource = PhoenixInspectionBasicCause.ListBasicCause(General.GetNullableGuid(ddlImmediateCause.SelectedValue));
        ddlBasicCause.DataTextField = "FLDBASICCAUSE";
        ddlBasicCause.DataValueField = "FLDBASICCAUSEID";
        ddlBasicCause.DataBind();
    }

    protected void BindContactType()
    {
        ddlContactType.Items.Clear();
        ddlContactType.DataSource = PhoenixInspectionContractType.ListContactType(General.GetNullableInteger(ddlcause.SelectedHard));
        ddlContactType.DataTextField = "FLDCONTACTTYPE";
        ddlContactType.DataValueField = "FLDCONTACTTYPEID";
        ddlContactType.DataBind();
    }

    protected void gvControlActionNeeded_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvControlActionNeeded.CurrentPageIndex + 1;
        BindData();
    }
}

