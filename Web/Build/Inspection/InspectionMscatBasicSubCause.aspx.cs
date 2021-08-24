using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionMscatBasicSubCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMscatBasicSubCause.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBasicSubCause')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuInspectionBasicSubCause.AccessRights = this.ViewState;
            MenuInspectionBasicSubCause.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                gvBasicSubCause.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        //ddlImmediateCause.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDBASICSUBCAUSE" };
        string[] alCaptions = { "S.No", "Basic Subcause" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionMscatBasicSubCause.BasicSubCauseSearch(
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

        Response.AddHeader("Content-Disposition", "attachment; filename=Basicsubcause.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Basic Sub Cause</h3></td>");
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

    protected void InspectionBasicSubCause_TabStripCommand(object sender, EventArgs e)
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

    protected void Rebind()
    {
        gvBasicSubCause.SelectedIndexes.Clear();
        gvBasicSubCause.EditIndexes.Clear();
        gvBasicSubCause.DataSource = null;
        gvBasicSubCause.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSERIALNUMBER", "FLDBASICSUBCAUSE" };
        string[] alCaptions = { "S.No", "Basic Subcause" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionMscatBasicSubCause.BasicSubCauseSearch(
                                                                    General.GetNullableGuid(ddlBasicCause.SelectedValue),
                                                                    General.GetNullableGuid(ddlImmediateCause.SelectedValue),
                                                                    General.GetNullableGuid(ddlContactType.SelectedValue),
                                                                    General.GetNullableInteger(ddlcause.SelectedHard),
                                                                    sortexpression,
                                                                    sortdirection,
                                                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                   gvBasicSubCause.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);
        General.SetPrintOptions("gvBasicSubCause", "Basic Sub Cause", alCaptions, alColumns, ds);

        gvBasicSubCause.DataSource = ds;
        gvBasicSubCause.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvBasicSubCause_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidBasicSubCause(((RadTextBox)e.Item.FindControl("txtBasicSubCauseAdd")).Text,
                                        ((RadTextBox)e.Item.FindControl("txtSequenceNumberAdd")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertBasicSubCause(General.GetNullableGuid(ddlBasicCause.SelectedValue)
                    , ((RadTextBox)e.Item.FindControl("txtBasicSubCauseAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtSequenceNumberAdd")).Text
                    );
                ((RadTextBox)e.Item.FindControl("txtBasicSubCauseAdd")).Focus();

                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidBasicSubCause(((RadTextBox)e.Item.FindControl("txtBasicSubCauseEdit")).Text,
                                    ((RadTextBox)e.Item.FindControl("txtSequenceNumberEdit")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateBasicSubCause(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblBasicSubCauseIdEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtBasicSubCauseEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtSequenceNumberEdit")).Text
                    );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMscatBasicSubCause.DeleteBasicSubCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblBasicSubCauseId")).Text));
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
    protected void gvBasicSubCause_ItemDataBound(object sender, GridItemEventArgs e)
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

    private bool IsValidBasicSubCause(string basicsubcause, string serialnumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ddlBasicCause.SelectedValue) == null)
            ucError.ErrorMessage = "No Basic cause is selected.";

        if (serialnumber.Trim().Equals(""))
            ucError.ErrorMessage = "Serial Number is required.";

        if (basicsubcause.Trim().Equals(""))
            ucError.ErrorMessage = "Basic subcause is required.";

        return (!ucError.IsError);
    }


    private void InsertBasicSubCause(Guid? basiccauseid, string basicsubcause, string description, string serialnumber)
    {
        PhoenixInspectionMscatBasicSubCause.InsertBasicSubCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , basiccauseid
            , basicsubcause
            , description
            , serialnumber
            );
    }

    private void UpdateBasicSubCause(Guid? basicsubcauseid, string basicsubcause, string description, string serialnumber)
    {
        PhoenixInspectionMscatBasicSubCause.UpdateBasicSubCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , basicsubcauseid
            , basicsubcause
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
        Rebind();
    }

    protected void ddlContactType_TextChanged(object sender, EventArgs e)
    {
        BindImmediateCause();
        BindBasicCause();
        Rebind();
    }

    protected void ddlImmediateCause_indexchanged(object sender, EventArgs e)
    {
        //gvBasicCause.SelectedIndex = -1;
        //gvBasicCause.EditIndex = -1;
        BindBasicCause();
        Rebind();
    }

    protected void ddlBasicCause_TextChanged(object sender, EventArgs e)
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
        //  ddlContactType.Items.Insert(0, new ListItem("--Select--", "Dummy"));
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

    protected void gvBasicSubCause_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBasicSubCause.CurrentPageIndex + 1;

        BindData();
    }
}

