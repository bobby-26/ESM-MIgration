using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using System.Configuration;
using Telerik.Web.UI;

public partial class RegistersContractCBAWage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("List", "LIST");
            toolbar2.AddButton("CBA Component Wages", "CLOSE");
            MenuRevision.MenuList = toolbar2.Show();
            MenuRevision.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                ViewState["COMPONENTID"] = string.Empty;
                if (Request.QueryString["RevisionId"] != null)
                    Editrevision(Request.QueryString["RevisionId"].ToString());
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersContractCBAWage.aspx?RevisionId=" + Request.QueryString["RevisionId"].ToString() + "&pagenumber=" + Request.QueryString["pagenumber"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuContract.AccessRights = this.ViewState;
            MenuContract.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Registers/RegistersContractCBAWage.aspx?RevisionId=" + Request.QueryString["RevisionId"].ToString() + "&pagenumber=" + Request.QueryString["pagenumber"].ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvSubCBA')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            SubContract.AccessRights = this.ViewState;
            SubContract.MenuList = toolbar1.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Editrevision(string revision)
    {
        Guid Revisionid = new Guid(revision);
        DataTable dt = PhoenixRegistersContract.EditCBARevision(Revisionid);
        if (dt.Rows.Count > 0)
        {
            ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
            txtUnion.Text = dt.Rows[0]["FLDNAME"].ToString();
            txtHistory.Text = dt.Rows[0]["FLDREVISIONNODESC"].ToString();
            txtRevisionNo.Text = dt.Rows[0]["FLDREVISIONNO"].ToString();
            ddlMonths.Items.Clear();
            ddlMonths.Items.Add(new DropDownListItem("--Select--", ""));
            if (dt.Rows[0]["FLDWAGEEXPYEAR"].ToString() == "1")
            {
                for (int i = 12; i <= (12 * 15); i = i + 12)
                {
                    ddlMonths.Items.Add(new DropDownListItem((i / 12).ToString(), i.ToString()));
                }
            }
        }
    }
    protected void Contract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDRANKNAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDMODIFIEDBY", "FLDMODIFIEDDATE" };
                string[] alCaptions = { "Rank", "Name", "Amount", "Modified By", "Modified On" };
                DataTable dt = PhoenixRegistersContract.ListCBAWageContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString())
                                , null, General.GetNullableInteger(ddlRank.SelectedRank), General.GetNullableInteger(ddlMonths.SelectedValue), null);

                Response.AddHeader("Content-Disposition", "attachment; filename=CBAComponentWages.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("");
                Response.Write("<tr><td ><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td><td colspan='" + (alColumns.Length - 1).ToString() + "'>     <b><h13> CBA component wages</h13></b></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5>UNION                              -" + txtUnion.Text + "</h5></td></tr>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write("<b>" + alCaptions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</TABLE>");
                Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SubContract_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string ComponentName = string.Empty;
                //if (gvWage.SelectedIndex != -1)
                //{
                //    ComponentName = ((Label)gvWage.Rows[gvWage.SelectedIndex].FindControl("lblComponentName")).Text;
                //}

                string[] alColumns = { "FLDRANKNAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDMODIFIEDBY", "FLDMODIFIEDDATE" };
                string[] alCaptions = { "Rank", "Name", "Amount", "Modified By", "Modified On" };
                DataTable dt = PhoenixRegistersContract.ListCBAWageContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString())
                , ViewState["COMPONENTID"].ToString() == string.Empty ? Guid.Empty : new Guid(ViewState["COMPONENTID"].ToString()),
                General.GetNullableInteger(ddlRank.SelectedRank), General.GetNullableInteger(ddlMonths.SelectedValue)
                , (byte?)General.GetNullableInteger(txtRevisionNo.Text));

                Response.AddHeader("Content-Disposition", "attachment; filename=CBASubComponent.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("");
                Response.Write("<tr><td ><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td><td colspan='" + (alColumns.Length - 1).ToString() + "'><b><h13> CBA Sub component wages</h13></b></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5>UNION                              -" + txtUnion.Text + "</h5></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5>Sub component for main component   -" + ComponentName + "</h5></td></tr>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr>");
                for (int i = 0; i < alCaptions.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write("<b>" + alCaptions[i] + "</b>");
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dr in dt.Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < alColumns.Length; i++)
                    {
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(string)) ? "<td  class='text'>" : "<td>");
                        Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }

                Response.Write("</TABLE>");
                Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
                Response.End();
                //  General.ShowExcel("Main Component", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataSub(Guid? ComponentId)
    {
        string[] alColumns = { "FLDUNIONNAME", "FLDRANKNAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDMODIFIEDBY", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Union", "Rank", "Name", "Amount", "Modified By", "Modified On" };
        DataTable dt = PhoenixRegistersContract.ListCBAWageContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString())
                                                                    , ComponentId, General.GetNullableInteger(ddlRank.SelectedRank)
                                                                    , General.GetNullableInteger(ddlMonths.SelectedValue)
                                                                    , (byte?)General.GetNullableInteger(txtRevisionNo.Text));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSubCBA", "CBA Sub component wages", alCaptions, alColumns, ds);
        gvSubCBA.DataSource = dt;
        gvSubCBA.VirtualItemCount = dt.Rows.Count;
    }
    protected void Revision_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Registers/RegistersContractCBARevision.aspx?Union=" + ViewState["ADDRESSCODE"].ToString() + "&pagenumber=" + Request.QueryString["pagenumber"].ToString(), true);
                MenuRevision.SelectedMenuIndex = 0;
            }
            else if (CommandName.ToUpper().Equals("COMPONENT"))
            {
                MenuRevision.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindWageData()
    {
        string[] alColumns = { "FLDUNIONNAME", "FLDRANKNAME", "FLDCOMPONENTNAME", "FLDAMOUNT", "FLDMODIFIEDBY", "FLDMODIFIEDDATE" };
        string[] alCaptions = { "Union", "Rank", "Name", "Amount", "Modified By", "Modified On" };

        DataTable dt = PhoenixRegistersContract.ListCBAWageContract(General.GetNullableInteger(ViewState["ADDRESSCODE"].ToString())
            , null, General.GetNullableInteger(ddlRank.SelectedRank), General.GetNullableInteger(ddlMonths.SelectedValue), (byte?)General.GetNullableInteger(txtRevisionNo.Text));

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvWage", "CBA Component Wages", alCaptions, alColumns, ds);
        gvWage.DataSource = dt;
        gvWage.VirtualItemCount = dt.Rows.Count;
        if (dt.Rows.Count > 0)
        {
            if (!IsPostBack || ViewState["COMPONENTID"].ToString() == string.Empty)
            {
                string componentid = dt.Rows[0]["FLDCOMPONENTID"].ToString();
                string lblComponentName = dt.Rows[0]["FLDCOMPONENTNAME"].ToString();
                lblSubComponent.Text = "Sub component for main component - " + lblComponentName;
                ViewState["COMPONENTID"] = componentid;
            }
        }
    }
    protected void gvSubCBA_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindDataSub(ViewState["COMPONENTID"].ToString() == string.Empty ? Guid.Empty : new Guid(ViewState["COMPONENTID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSubCBA_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
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
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (drv["FLDWAGEID"].ToString() == string.Empty)
                    db.Visible = false;
            }

        }
    }
    protected void gvSubCBA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string wagerankid = ((RadLabel)e.Item.FindControl("lblWageId")).Text;
                PhoenixRegistersContract.DeleteCBAContractWage(new Guid(wagerankid));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string componentId = ((RadLabel)e.Item.FindControl("lblComponentEditId")).Text;
                string val = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                if (!IsValidWage(ddlRank.SelectedRank, val))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertCBAContractWage(int.Parse(ViewState["ADDRESSCODE"].ToString()), int.Parse(ddlRank.SelectedRank)
                    , new Guid(componentId), decimal.Parse(val), General.GetNullableInteger(ddlMonths.SelectedValue));
                Rebind();
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
        gvWage.SelectedIndexes.Clear();
        gvWage.EditIndexes.Clear();
        gvWage.DataSource = null;
        gvWage.Rebind();
        gvSubCBA.SelectedIndexes.Clear();
        gvSubCBA.EditIndexes.Clear();
        gvSubCBA.DataSource = null;
        gvSubCBA.Rebind();

    }
    protected void gvWage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindWageData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWage_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
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
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (drv["FLDWAGEID"].ToString() == string.Empty)
                    db.Visible = false;
            }

        }
    }
    protected void gvWage_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string componentid = ((RadLabel)e.Item.FindControl("lblComponentId")).Text;
                string lblComponentName = ((RadLabel)e.Item.FindControl("lblComponentName")).Text;
                lblSubComponent.Text = "Sub component for main component - " + lblComponentName;
                ViewState["COMPONENTID"] = componentid;
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string wagerankid = ((RadLabel)e.Item.FindControl("lblWageId")).Text;
                PhoenixRegistersContract.DeleteCBAContractWage(new Guid(wagerankid));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string componentId = ((RadLabel)e.Item.FindControl("lblComponentEditId")).Text;
                string val = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                if (!IsValidWage(ddlRank.SelectedRank, val))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertCBAContractWage(int.Parse(ViewState["ADDRESSCODE"].ToString()), int.Parse(ddlRank.SelectedRank)
                    , new Guid(componentId), decimal.Parse(val), General.GetNullableInteger(ddlMonths.SelectedValue));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidWage(string rank, string val)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal resultDec;
        int resultInt;
        string[] value = val.Split('.');
        if (!int.TryParse(ViewState["ADDRESSCODE"].ToString(), out resultInt))
            ucError.ErrorMessage = "Union is required.";
        if (!int.TryParse(rank, out resultInt))
            ucError.ErrorMessage = "Rank is required.";
        if (!decimal.TryParse(val, out resultDec))
            ucError.ErrorMessage = "Amount is required.";
        else if (value[0].ToString().Length >= 8)
            ucError.ErrorMessage = "Amount max upto 9999999.99";
        return (!ucError.IsError);
    }
    protected void ddlRank_TextChanged(object sender, EventArgs e)
    {

        ViewState["COMPONENTID"] = string.Empty;
        Rebind();
        //BindDataSub(ViewState["COMPONENTID"].ToString() == string.Empty ? Guid.Empty : new Guid(ViewState["COMPONENTID"].ToString()));
        ddlRank.Focus();
    }
    //protected void gvSubCBA_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        string wagerankid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWageId")).Text;
    //        PhoenixRegistersContract.DeleteCBAContractWage(new Guid(wagerankid));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    gvSubCBA.EditIndex = -1;
    //    gvSubCBA.SelectedIndex = -1;
    //    BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
    //}
    //protected void gvWage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        GridViewRow gv = _gridView.Rows[nCurrentRow];
    //        string componentId = ((Label)gv.FindControl("lblComponentEditId")).Text;
    //        string val = ((UserControlMaskNumber)gv.FindControl("txtAmount")).Text;
    //        if (!IsValidWage(ddlRank.SelectedRank, val))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixRegistersContract.InsertCBAContractWage(int.Parse(ViewState["ADDRESSCODE"].ToString()), int.Parse(ddlRank.SelectedRank)
    //            , new Guid(componentId), decimal.Parse(val), General.GetNullableInteger(ddlMonths.SelectedValue));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindWageData();
    //}
    //protected void gvWage_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("SELECT"))
    //        {
    //            _gridView.EditIndex = -1;
    //            gvSubCBA.EditIndex = -1;
    //            _gridView.SelectedIndex = nCurrentRow;
    //            string componentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblComponentId")).Text;
    //            string lblComponentName = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblComponentName")).Text;
    //            lblSubComponent.Text = "Sub component for main component - " + lblComponentName;
    //            ViewState["COMPONENTID"] = componentid;

    //            BindDataSub(new Guid(componentid));
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvWage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        string wagerankid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWageId")).Text;
    //        PhoenixRegistersContract.DeleteCBAContractWage(new Guid(wagerankid));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    gvWage.EditIndex = -1;
    //    //ViewState["SELECTEDINDEX"] = 0;
    //    ViewState["COMPONENTID"] = string.Empty;
    //    BindWageData();
    //    BindDataSub(ViewState["COMPONENTID"].ToString() == string.Empty ? Guid.Empty : new Guid(ViewState["COMPONENTID"].ToString()));
    //}
    //protected void gvWage_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
    //            && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
    //        {
    //            DataRowView drv = (DataRowView)e.Row.DataItem;
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (db != null)
    //            {
    //                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            }
    //            if (drv["FLDWAGEID"].ToString() == string.Empty)
    //                db.Visible = false;
    //        }
    //        else
    //            e.Row.Attributes["onclick"] = "";
    //    }
    //}    //protected void gvSubCBA_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    try
    //    {
    //        gvWage.EditIndex = -1;
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = e.NewEditIndex;
    //        BindWageData();
    //        BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
    //        ((TextBox)((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtAmount")).FindControl("txtNumber")).Focus();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}    //protected void gvSubCBA_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {

    //        GridViewRow gv = _gridView.Rows[nCurrentRow];
    //        string componentId = ((Label)gv.FindControl("lblComponentEditId")).Text;
    //        string val = ((UserControlMaskNumber)gv.FindControl("txtAmount")).Text;
    //        if (!IsValidWage(ddlRank.SelectedRank, val))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixRegistersContract.InsertCBAContractWage(int.Parse(ViewState["ADDRESSCODE"].ToString()), int.Parse(ddlRank.SelectedRank)
    //            , new Guid(componentId), decimal.Parse(val), General.GetNullableInteger(ddlMonths.SelectedValue));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindDataSub(new Guid(ViewState["COMPONENTID"].ToString()));
    //}    //protected void gvSubCBA_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
    //            && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
    //        {
    //            DataRowView drv = (DataRowView)e.Row.DataItem;
    //            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //            if (drv["FLDWAGEID"].ToString() == string.Empty)
    //                db.Visible = false;
    //        }
    //    }      
    //}

}
