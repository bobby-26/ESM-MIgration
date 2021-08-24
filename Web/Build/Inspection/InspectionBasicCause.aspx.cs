using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionBasicCause : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionBasicCause.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBasicCause')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Copy','','" + Session["sitepath"] + "/Inspection/InspectionMSCATMapping.aspx?type=3'); return false;", "Map Immediate Type", "<i class=\"fas fa-tasks\"></i>", "Map");
            MenuInspectionBasicCause.AccessRights = this.ViewState;
            MenuInspectionBasicCause.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                
                BindContactType();
                BindImmediateCause();
                gvBasicCause.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
       // ddlImmediateCause.Items.Insert(0,new ListItem("--Select--", "Dummy"));
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSERIALNUMBER", "FLDBASICCAUSE", "FLDHARDNAME" };
        string[] alCaptions = { "S.No", "Basic Cause", "Type" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionBasicCause.BasicCauseSearch(
                                                            General.GetNullableGuid(ddlImmediateCause.SelectedValue),
                                                            General.GetNullableGuid(ddlContactType.SelectedValue),
                                                            General.GetNullableInteger(ddlcause.SelectedHard),
                                                            1,
                                                            iRowCount,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,sortexpression,sortdirection);


        Response.AddHeader("Content-Disposition", "attachment; filename=BasicCause.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Basic Cause</h3></td>");
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

    protected void InspectionBasicCause_TabStripCommand(object sender, EventArgs e)
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
        string[] alColumns = { "FLDSERIALNUMBER", "FLDBASICCAUSE", "FLDHARDNAME" };
        string[] alCaptions = { "S.No", "Basic Cause", "Type" };
        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionBasicCause.BasicCauseSearch( General.GetNullableGuid(ddlImmediateCause.SelectedValue),
                                                            General.GetNullableGuid(ddlContactType.SelectedValue),
                                                            General.GetNullableInteger(ddlcause.SelectedHard),
                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                            gvBasicCause.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,sortexpression, sortdirection);

        General.SetPrintOptions("gvBasicCause", "Basic Cause", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBasicCause.DataSource = ds;
            gvBasicCause.VirtualItemCount = iRowCount;
        }
        else
        {
            gvBasicCause.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
     }

    private bool IsValidData(string basiccause, string type, string sequencenumber)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(sequencenumber))
            ucError.ErrorMessage = "Serial Number is required.";

        if (basiccause.Equals(""))
            ucError.ErrorMessage = "Basic Cause is required.";
            
        if(General.GetNullableInteger(type) == null)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);

    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvBasicCause.Rebind();
    }
    
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    //protected void gvBasicCause_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

    //        ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
    //        {
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (db != null)
    //            {
    //                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }

    //        }

    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
    //        if (db != null)
    //        {
    //            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
    //                db.Visible = false;
    //        }
    //    }

    //    UserControlHard ucHardType = (UserControlHard)e.Row.FindControl("ddlTypeEdit");
    //    DataRowView drvHardType = (DataRowView)e.Row.DataItem;
    //    if (ucHardType != null)
    //    {
    //        ucHardType.SelectedHard = drvHardType["FLDTYPE"].ToString();
    //    }
    //}

    protected void ddlContactType_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindImmediateCause();
        BindData();
        gvBasicCause.Rebind();
    }
    protected void ddlcause_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindContactType();
        BindImmediateCause();
        BindData();
        gvBasicCause.Rebind();
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

    protected void gvBasicCause_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBasicCause.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvBasicCause_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtBasicCauseAdd")).Text,
                                 ((UserControlHard)e.Item.FindControl("ddlTypeAdd")).SelectedHard,
                                 ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionBasicCause.InsertBasicCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    ((RadTextBox)e.Item.FindControl("txtBasicCauseAdd")).Text,
                    int.Parse(((UserControlHard)e.Item.FindControl("ddlTypeAdd")).SelectedHard),
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text,
                    int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberAdd")).Text));

                BindData();
                gvBasicCause.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid basiccauseid = new Guid(item.GetDataKeyValue("FLDBASICCAUSEID").ToString());
                PhoenixInspectionBasicCause.DeleteBasicCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                basiccauseid);
                BindData();
                gvBasicCause.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtBasicCauseEdit")).Text,
                                  ((UserControlHard)e.Item.FindControl("ddlTypeEdit")).SelectedHard,
                                  ((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                GridDataItem item = e.Item as GridDataItem;
                Guid basiccauseid = new Guid(item.GetDataKeyValue("FLDBASICCAUSEID").ToString());

                PhoenixInspectionBasicCause.UpdateBasicCause(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        ((RadTextBox)e.Item.FindControl("txtBasicCauseEdit")).Text,
                                                        int.Parse(((UserControlHard)e.Item.FindControl("ddlTypeEdit")).SelectedHard),
                                                        ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text,
                                                        basiccauseid,
                                                        int.Parse(((UserControlMaskNumber)e.Item.FindControl("txtSequenceNumberEdit")).Text));
                BindData();
                gvBasicCause.Rebind();
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

    protected void gvBasicCause_ItemDataBound(object sender, GridItemEventArgs e)
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

            UserControlHard ucHardType = (UserControlHard)e.Item.FindControl("ddlTypeEdit");
            DataRowView drvHardType = (DataRowView)e.Item.DataItem;
            if (ucHardType != null)
            {
                ucHardType.SelectedHard = drvHardType["FLDTYPE"].ToString();
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

    protected void gvBasicCause_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void ddlImmediateCause_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvBasicCause.Rebind();
    }
}

