using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionContactType : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionContactType.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvContactType')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Copy', '','" + Session["sitepath"] + "/Inspection/InspectionMSCATMapping.aspx?type=1',null);return true;", "Map Accident Description", "<i class=\"fas fa-tasks\"></i>", "Map");
            MenuInspectionContactType.AccessRights = this.ViewState;
            MenuInspectionContactType.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvContactType.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDCONTACTTYPE" };
        string[] alCaptions = { "S.No", "Contact Type / Undesirable Event" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionContractType.ContactTypeSearch(General.GetNullableInteger(ddlcause.SelectedHard), sortexpression, sortdirection,
           1,
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ContactType.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Contact Type</h3></td>");
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
    protected void InspectionContactType_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDCONTACTTYPE" };
        string[] alCaptions = { "S.No", "Contact Type / Undesirable Event" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionContractType.ContactTypeSearch(General.GetNullableInteger(ddlcause.SelectedHard), sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvContactType.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvContactType", "ContactType", alCaptions, alColumns, ds);
        gvContactType.DataSource = ds;
        gvContactType.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void Rebind()
    {
        gvContactType.SelectedIndexes.Clear();
        gvContactType.EditIndexes.Clear();
        gvContactType.DataSource = null;
        gvContactType.Rebind();
    }

    protected void gvContactType_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvContactType.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvContactType_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvContactType_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidContactType(((RadTextBox)e.Item.FindControl("txtContactTypeAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertContactType(
                    ((RadTextBox)e.Item.FindControl("txtContactTypeAdd")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text)
                    );
                ((RadTextBox)e.Item.FindControl("txtContactTypeAdd")).Focus();

                ucStatus.Text = "Contact type updated";
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidContactType(((RadTextBox)e.Item.FindControl("txtContactTypeEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateContactType(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblContactTypeIdEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtContactTypeEdit")).Text
                    , int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text)
                    );
                ucStatus.Text = "Contact type updated";
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionContractType.DeleteContactType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblContactTypeId")).Text));
                ucStatus.Text = "Contact type deleted";
                Rebind();
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

    protected void gvContactType_ItemDataBound(object sender, GridItemEventArgs e)
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

            RadLabel lblContactTypeId = (RadLabel)e.Item.FindControl("lblContactTypeId");
            RadLabel lblContactType = (RadLabel)e.Item.FindControl("lblContactType");

            //LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdMap");
            //if (cmdMap != null)
            //{
            //    cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
            //    cmdMap.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionUndesirableEventWorstCase.aspx?contacttypeid=" + lblContactTypeId.Text + "&contacttype="+ lblContactType.Text + "'); return true;");
            //}

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

        UserControlHard ucHardCause = (UserControlHard)e.Item.FindControl("ddlCauseEdit");
        DataRowView drvHardCause = (DataRowView)e.Item.DataItem;
        if (ucHardCause != null)
        {
            ucHardCause.SelectedHard = drvHardCause["FLDCAUSE"].ToString();
        }
    }

    private bool IsValidContactType(string ContactType, string sequencenumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(sequencenumber))
            ucError.ErrorMessage = "Serial Number is required.";

        if (ContactType.Trim().Equals(""))
            ucError.ErrorMessage = "Contact type is required.";

        return (!ucError.IsError);
    }

    private void InsertContactType(string contacttype, int serialnumber)
    {
        PhoenixInspectionContractType.InsertContactType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , contacttype
            , serialnumber
            );
    }

    private void UpdateContactType(Guid? contacttypeid, string contacttype, int serialnumber)
    {
        PhoenixInspectionContractType.UpdateContactType(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , contacttypeid
            , contacttype
            , serialnumber
            );
        ucStatus.Text = "Contact type is updated";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ddlcause_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}

