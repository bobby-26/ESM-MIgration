using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionMscatSubControlActionNeeded : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMscatSubControlActionNeeded.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSubControlActionNeeded')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuInspectionSubControlActionNeeds.AccessRights = this.ViewState;
            MenuInspectionSubControlActionNeeds.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvSubControlActionNeeded.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                BindContactType();
                BindImmediateCause();
                BindBasicCause();
                BindControlActions();
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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDSUBCONTROLACTIONNEEDED" };
        string[] alCaptions = { "S.No", "SubControl Action Needs" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionMscatSubControlActionNeeded.SubControlActionNeededSearch(
                                                                            General.GetNullableGuid(ddlControlActionNeeded.SelectedValue),
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


        Response.AddHeader("Content-Disposition", "attachment; filename=SubcontrolActionNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sub Control Action Needs</h3></td>");
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

    protected void InspectionSubControlActionNeeds_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDSUBCONTROLACTIONNEEDED" };
        string[] alCaptions = { "S.No", "SubControl Action Needs" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionMscatSubControlActionNeeded.SubControlActionNeededSearch(
                                                                            General.GetNullableGuid(ddlControlActionNeeded.SelectedValue),
                                                                            General.GetNullableGuid(ddlBasicCause.SelectedValue),
                                                                            General.GetNullableGuid(ddlImmediateCause.SelectedValue),
                                                                            General.GetNullableGuid(ddlContactType.SelectedValue),
                                                                            General.GetNullableInteger(ddlcause.SelectedHard),
                                                                            sortexpression,
                                                                            sortdirection,
                                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                            gvSubControlActionNeeded.PageSize,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount);

        General.SetPrintOptions("gvSubControlActionNeeded", "Sub Control Action Needs", alCaptions, alColumns, ds);

        gvSubControlActionNeeded.DataSource = ds;
        gvSubControlActionNeeded.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void Rebind()
    {
        gvSubControlActionNeeded.SelectedIndexes.Clear();
        gvSubControlActionNeeded.EditIndexes.Clear();
        gvSubControlActionNeeded.DataSource = null;
        gvSubControlActionNeeded.Rebind();
    }
    protected void gvSubControlActionNeeded_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsSubValidControlActionNeeded(((RadTextBox)e.Item.FindControl("txtSubControlActionNeededAdd")).Text,
                                                ((RadTextBox)e.Item.FindControl("txtSequenceNumberAdd")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSubControlActionNeeded(General.GetNullableGuid(ddlControlActionNeeded.SelectedValue)
                    , ((RadTextBox)e.Item.FindControl("txtSubControlActionNeededAdd")).Text
                    , ((TextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtSequenceNumberAdd")).Text
                    );
                ((RadTextBox)e.Item.FindControl("txtSubControlActionNeededAdd")).Focus();
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsSubValidControlActionNeeded(((RadTextBox)e.Item.FindControl("txtSubControlActionNeededEdit")).Text,
                            ((RadTextBox)e.Item.FindControl("txtSequenceNumberEdit")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateSubControlActionNeeded(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblSubControlActionNeededIdEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtSubControlActionNeededEdit")).Text
                    , ((TextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtSequenceNumberEdit")).Text
                    );
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMscatSubControlActionNeeded.DeleteSubControlActionNeeded(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblSubControlActionNeededId")).Text));
                Rebind();
            }

            if (e.CommandName == "Page")
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
    protected void gvSubControlActionNeeded_ItemDataBound(object sender, GridItemEventArgs e)
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

    private bool IsSubValidControlActionNeeded(string subcontrolactionneeded, string serialnumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (serialnumber.Trim().Equals(""))
            ucError.ErrorMessage = "Serial Number is required.";

        if (subcontrolactionneeded.Trim().Equals(""))
            ucError.ErrorMessage = "SubControl Action Needed is required.";

        return (!ucError.IsError);
    }


    private void InsertSubControlActionNeeded(Guid? controlactionneededid, string subcontrolactionneeded, string description, string serialnumber)
    {
        PhoenixInspectionMscatSubControlActionNeeded.InsertSubControlActionNeeded(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , controlactionneededid
            , subcontrolactionneeded
            , description
            , serialnumber
            );
    }

    private void UpdateSubControlActionNeeded(Guid? subcontrolactionneededid, string subcontrolactionneeded, string description, string serialnumber)
    {
        PhoenixInspectionMscatSubControlActionNeeded.UpdateSubControlActionNeeded(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , subcontrolactionneededid
            , subcontrolactionneeded
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
        BindControlActions();
        Rebind();
    }

    protected void ddlContactType_TextChanged(object sender, EventArgs e)
    {
        BindImmediateCause();
        BindBasicCause();
        BindControlActions();
        Rebind();
    }

    protected void ddlImmediateCause_indexchanged(object sender, EventArgs e)
    {
        //gvBasicCause.SelectedIndex = -1;
        //gvBasicCause.EditIndex = -1;
        BindBasicCause();
        BindControlActions();
        Rebind();
    }

    protected void ddlBasicCause_TextChanged(object sender, EventArgs e)
    {
        BindControlActions();
        Rebind();
    }

    protected void ddlControlActionNeeded_TextChanged(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void BindContactType()
    {
        ddlContactType.Items.Clear();
        ddlContactType.DataSource = PhoenixInspectionContractType.ListContactType(General.GetNullableInteger(ddlcause.SelectedHard));
        ddlContactType.DataTextField = "FLDCONTACTTYPE";
        ddlContactType.DataValueField = "FLDCONTACTTYPEID";
        ddlContactType.DataBind();
        // ddlContactType.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindBasicCause()
    {
        ddlBasicCause.Items.Clear();
        ddlBasicCause.DataSource = PhoenixInspectionBasicCause.ListBasicCause(General.GetNullableGuid(ddlImmediateCause.SelectedValue));
        ddlBasicCause.DataTextField = "FLDBASICCAUSE";
        ddlBasicCause.DataValueField = "FLDBASICCAUSEID";
        ddlBasicCause.DataBind();
        //ddlBasicCause.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void BindControlActions()
    {
        ddlControlActionNeeded.Items.Clear();
        ddlControlActionNeeded.DataSource = PhoenixInspectionMscatControlActionNeeded.ListControlActionNeeded(General.GetNullableGuid(ddlBasicCause.SelectedValue));
        ddlControlActionNeeded.DataTextField = "FLDCONTROLACTIONNEEDED";
        ddlControlActionNeeded.DataValueField = "FLDCONTROLACTIONNEEDEDID";
        ddlControlActionNeeded.DataBind();
        //ddlControlActionNeeded.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void gvSubControlActionNeeded_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSubControlActionNeeded.CurrentPageIndex + 1;

        BindData();
    }
}


