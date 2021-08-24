using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionImmediateCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionImmediateCause.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvImmedaiteCause')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Copy','','" + Session["sitepath"] + "/Inspection/InspectionMSCATMapping.aspx?type=2'); return false;", "Map Contact Type", "<i class=\"fas fa-tasks\"></i>", "Map");            
            MenuInspectionImmediateCause.AccessRights = this.ViewState;
            MenuInspectionImmediateCause.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindContactType();
                gvImmedaiteCause.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDSERIALNUMBER", "FLDIMMEDIATECAUSE", "FLDCATEGORYNAME" };
        string[] alCaptions = { "S.No", "Immediate Cause", "Category" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionImmediateCause.ImmediateCauseSearch(General.GetNullableGuid(ddlContactType.SelectedValue),
                                                                    General.GetNullableInteger(ddlcause.SelectedHard),
                                                                    sortexpression,
                                                                    sortdirection,
                                                                    1,
                                                                    iRowCount,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=ImmediateCause.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Immediate Cause</h3></td>");
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDIMMEDIATECAUSE", "FLDCATEGORYNAME" };
        string[] alCaptions = { "S.No", "Immediate Cause", "Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        
        DataSet ds = new DataSet();
        
        ds = PhoenixInspectionImmediateCause.ImmediateCauseSearch(General.GetNullableGuid(ddlContactType.SelectedValue), 
                                                                    General.GetNullableInteger(ddlcause.SelectedHard),
                                                                    sortexpression, 
                                                                    sortdirection,
                                                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                    gvImmedaiteCause.PageSize,
                                                                    ref iRowCount,
                                                                    ref iTotalPageCount);
        General.SetPrintOptions("gvImmedaiteCause", "Immediate Cause", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvImmedaiteCause.DataSource = ds;
            gvImmedaiteCause.VirtualItemCount = iRowCount;
        }
        else
        {
            gvImmedaiteCause.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
     }

    private bool IsValidImmediateCause(string immediatecause, string category, string sequencenumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(sequencenumber))
            ucError.ErrorMessage = "Serial Number is required.";

        if (immediatecause.Trim().Equals(""))
            ucError.ErrorMessage = "Immediate cause is required.";

        if (General.GetNullableInteger(category) == null)
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }


    private void InsertImmediateCause(string immediatecause, string description, int category, int serialnumber)
    {
        PhoenixInspectionImmediateCause.InsertImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode            
            , immediatecause
            , description
            , category
            , serialnumber
            );
    }

    private void UpdateImmediateCause(Guid? immediatecauseid, string immediatecause, string description, int category, int serialnumber)
    {
        PhoenixInspectionImmediateCause.UpdateImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , immediatecauseid
            , immediatecause
            , description
            , category
            , serialnumber
            );
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {       
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvImmedaiteCause.Rebind();
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void ddlContactType_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvImmedaiteCause.Rebind();
    }
    protected void ddlcause_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindContactType();
        BindData();
        gvImmedaiteCause.Rebind();
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

    protected void gvImmedaiteCause_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidImmediateCause(((RadTextBox)e.Item.FindControl("txtImmediateCauseAdd")).Text,
                                ((UserControlHard)e.Item.FindControl("ucCategoryAdd")).SelectedHard,
                                ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertImmediateCause(((RadTextBox)e.Item.FindControl("txtImmediateCauseAdd")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text
                    , int.Parse(((UserControlHard)e.Item.FindControl("ucCategoryAdd")).SelectedHard)
                    , int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text)
                    );
                BindData();
                gvImmedaiteCause.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidImmediateCause(((RadTextBox)e.Item.FindControl("txtImmediateCauseEdit")).Text,
                                ((UserControlHard)e.Item.FindControl("ucCategoryEdit")).SelectedHard,
                                ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateImmediateCause(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblImmediateCauseIdEdit")).Text)
                    , ((RadTextBox)e.Item.FindControl("txtImmediateCauseEdit")).Text
                    , ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text
                    , int.Parse(((UserControlHard)e.Item.FindControl("ucCategoryEdit")).SelectedHard)
                    , int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text)
                    );
                BindData();
                gvImmedaiteCause.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionImmediateCause.DeleteImmediateCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblImmediateCauseId")).Text));
                BindData();
                gvImmedaiteCause.Rebind();
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

    protected void gvImmedaiteCause_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            
            UserControlHard ucCategory = (UserControlHard)e.Item.FindControl("ucCategoryEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCategory != null)
            {
                ucCategory.SelectedHard = drv["FLDCATEGORY"].ToString();
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

    protected void gvImmedaiteCause_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvImmedaiteCause_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvImmedaiteCause.CurrentPageIndex + 1;
        BindData();
    }
}

