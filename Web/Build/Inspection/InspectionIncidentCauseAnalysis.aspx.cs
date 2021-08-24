using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionIncidentCauseAnalysis : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        gvImmediateCause.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
        gvBasicCause.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
        BindImmediateSubCause();
        BindBasicSubCause();

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (Request.QueryString["carid"] != null)
            {
                ViewState["CARID"] = Request.QueryString["carid"];
            }
            else
            {
                ViewState["CARID"] = null;
            }

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionIncidentCauseAnalysis.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()), "Export to Excel", "icon_xls.png", "ImmediateCauseExcel");
            toolbar.AddImageLink("javascript:CallPrint('gvImmediateCause')", "Print Grid", "icon_print.png", "RootCausePrint");
            MenuRootCause.AccessRights = this.ViewState;
            MenuRootCause.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Inspection/InspectionIncidentCauseAnalysis.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()), "Export to Excel", "icon_xls.png", "BasicCauseExcel");
            toolbar.AddImageLink("javascript:CallPrint('gvBasicCause')", "Print Grid", "icon_print.png", "BasicCausePrint");
            MenuBasicCause.AccessRights = this.ViewState;
            MenuBasicCause.MenuList = toolbar.Show();            
        }

        BindImmediateCause();
        BindBasicCause();
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        //if (ViewState["CARID"] == null)
        //{
        //    Response.Redirect("../Inspection/InspectionIncidentCAR.aspx");
        //}
        //else
        //{
            if (dce.CommandName.ToUpper().Equals("CAR"))
            {
                Response.Redirect("../Inspection/InspectionIncidentCAR.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()));
            }
            else if (dce.CommandName.ToUpper().Equals("ACTION"))
            {
                Response.Redirect("../Inspection/InspectionIncidentCorrectiveAction.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()));
            }
            else if (dce.CommandName.ToUpper().Equals("EXTENSIONREASON"))
            {
                Response.Redirect("../Inspection/InspectionIncidentCARExtensionReason.aspx?carid=" + (ViewState["CARID"] == null ? null : ViewState["CARID"].ToString()));
            }
        //}
    }

    protected void MenuBasicCause_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("BASICCAUSEEXCEL"))
            {
                string[] alColumns = { "FLDBASICCAUSENAME", "FLDSUBCAUSENAME", "FLDREASON" };
                string[] alCaptions = { "Main Cause", "Sub Cause", "Reason" };

                DataSet ds = PhoenixInspectionIncidentCAR.ListBasicCause(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(Filter.CurrentIncidentID));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Root Cause", ds.Tables[0], alColumns, alCaptions, null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRootCause_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("IMMEDIATECAUSEEXCEL"))
            {
                string[] alColumns = { "FLDIMMEDIATECAUSENAME", "FLDSUBCAUSENAME", "FLDREASON" };
                string[] alCaptions = { "Main Cause", "Sub Cause", "Reason" };

                DataSet ds = PhoenixInspectionIncidentCAR.ListImmediateCause(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(Filter.CurrentIncidentID));

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Immediate Cause", ds.Tables[0], alColumns, alCaptions, null, null);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindImmediateCause();
        BindBasicCause();
    }

    private void BindImmediateSubCause()
    {
        try
        {
            UserControlImmediateMainCause ucRootCause = new UserControlImmediateMainCause();
            UserControlImmediateSubCause ucSubRootCause = new UserControlImmediateSubCause();
            Label lblSubRootCauseIdEdit = new Label();

            if (ViewState["ICURRENTINDEX"] != null && (int)ViewState["ICURRENTINDEX"] != -1)
            {
                ucRootCause = (UserControlImmediateMainCause)gvImmediateCause.Rows[(int)ViewState["ICURRENTINDEX"]].FindControl("ucRootCauseEdit");
                ucSubRootCause = (UserControlImmediateSubCause)gvImmediateCause.Rows[(int)ViewState["ICURRENTINDEX"]].FindControl("ucSubRootCauseEdit");
                lblSubRootCauseIdEdit = (Label)gvImmediateCause.Rows[(int)ViewState["ICURRENTINDEX"]].FindControl("lblSubRootCauseIdEdit");

                if (ucRootCause != null)
                    ucSubRootCause.SubCauseList = PhoenixInspectionImmediateSubCause.ListSubCause(1,
                        General.GetNullableGuid(ucRootCause.SelectedMainCause));
            }
            else
            {
                ucRootCause = (UserControlImmediateMainCause)gvImmediateCause.FooterRow.FindControl("ucRootCauseAdd");
                ucSubRootCause = (UserControlImmediateSubCause)gvImmediateCause.FooterRow.FindControl("ucSubRootCauseAdd");

                if (ucRootCause != null)
                    ucSubRootCause.SubCauseList = PhoenixInspectionImmediateSubCause.ListSubCause(1,
                        General.GetNullableGuid(ucRootCause.SelectedMainCause));
            }

            if (lblSubRootCauseIdEdit != null && lblSubRootCauseIdEdit.Text != "")
            {
                ucSubRootCause.SelectedSubCause = lblSubRootCauseIdEdit.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindBasicSubCause()
    {
        try
        {
            UserControlBasicMainCause ucBasicCause = new UserControlBasicMainCause();
            UserControlBasicSubCause ucSubBasicCause = new UserControlBasicSubCause();
            Label lblSubBasicCauseIdEdit = new Label();

            if (ViewState["BCURRENTINDEX"] != null && (int)ViewState["BCURRENTINDEX"] != -1)
            {
                ucBasicCause = (UserControlBasicMainCause)gvBasicCause.Rows[(int)ViewState["BCURRENTINDEX"]].FindControl("ucBasicCauseEdit");
                ucSubBasicCause = (UserControlBasicSubCause)gvBasicCause.Rows[(int)ViewState["BCURRENTINDEX"]].FindControl("ucSubBasicCauseEdit");
                lblSubBasicCauseIdEdit = (Label)gvBasicCause.Rows[(int)ViewState["BCURRENTINDEX"]].FindControl("lblSubBasicCauseIdEdit");

                if (ucBasicCause != null)
                    ucSubBasicCause.SubCauseList = PhoenixInspectionBasicSubCause.ListSubCause(1, 
                        General.GetNullableGuid(ucBasicCause.SelectedMainCause));
            }
            else
            {
                ucBasicCause = (UserControlBasicMainCause)gvBasicCause.FooterRow.FindControl("ucBasicCauseAdd");
                ucSubBasicCause = (UserControlBasicSubCause)gvBasicCause.FooterRow.FindControl("ucSubBasicCauseAdd");

                if (ucBasicCause != null)
                    ucSubBasicCause.SubCauseList = PhoenixInspectionBasicSubCause.ListSubCause(1,
                        General.GetNullableGuid(ucBasicCause.SelectedMainCause));
            }

            if (lblSubBasicCauseIdEdit != null && lblSubBasicCauseIdEdit.Text != "")
            {
                ucSubBasicCause.SelectedSubCause = lblSubBasicCauseIdEdit.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindImmediateCause()
    {
        try
        {
            string[] alColumns = { "FLDIMMEDIATECAUSENAME", "FLDSUBCAUSENAME", "FLDREASON" };
            string[] alCaptions = { "Main Cause", "Sub Cause", "Reason" };

            DataSet ds = PhoenixInspectionIncidentCAR.ListImmediateCause(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(Filter.CurrentIncidentID));

            General.SetPrintOptions("gvImmediateCause", "Immediate Cause", alCaptions, alColumns, ds);
            if (Filter.CurrentSelectedIncidentMenu != null)
                gvImmediateCause.ShowFooter = false;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["ICURRENTINDEX"] != null && (int)ViewState["ICURRENTINDEX"] >= ds.Tables[0].Rows.Count)
                {
                    ViewState["ICURRENTINDEX"] = -1;
                    gvImmediateCause.SelectedIndex = -1;
                    gvImmediateCause.EditIndex = -1;
                }
                gvImmediateCause.DataSource = ds;
                gvImmediateCause.DataBind();
            }
            else
            {
                if (ViewState["ICURRENTINDEX"] != null && (int)ViewState["ICURRENTINDEX"] >= ds.Tables[0].Rows.Count)
                {
                    ViewState["ICURRENTINDEX"] = -1;
                    gvImmediateCause.SelectedIndex = -1;
                    gvImmediateCause.EditIndex = -1;
                }
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvImmediateCause);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindBasicCause()
    {
        try
        {
            string[] alColumns = { "FLDBASICCAUSENAME", "FLDSUBCAUSENAME", "FLDREASON" };
            string[] alCaptions = { "Main Cause", "Sub Cause", "Reason" };

            DataSet ds = PhoenixInspectionIncidentCAR.ListBasicCause(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(Filter.CurrentIncidentID));

            General.SetPrintOptions("gvBasicCause", "Root Cause", alCaptions, alColumns, ds);
            if (Filter.CurrentSelectedIncidentMenu != null)
                gvBasicCause.ShowFooter = false;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["BCURRENTINDEX"] != null && (int)ViewState["BCURRENTINDEX"] >= ds.Tables[0].Rows.Count)
                {
                    ViewState["BCURRENTINDEX"] = -1;
                    gvBasicCause.SelectedIndex = -1;
                    gvBasicCause.EditIndex = -1;
                }
                gvBasicCause.DataSource = ds;
                gvBasicCause.DataBind();
            }
            else
            {
                if (ViewState["BCURRENTINDEX"] != null && (int)ViewState["BCURRENTINDEX"] >= ds.Tables[0].Rows.Count)
                {
                    ViewState["BCURRENTINDEX"] = -1;
                    gvBasicCause.SelectedIndex = -1;
                    gvBasicCause.EditIndex = -1;
                }
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvBasicCause);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvImmediateCause_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["ICURRENTINDEX"] = -1;
        BindImmediateCause();
    }

    protected void gvImmediateCause_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCause(((UserControlImmediateMainCause)_gridView.FooterRow.FindControl("ucRootCauseAdd")).SelectedMainCause
                        , ((UserControlImmediateSubCause)_gridView.FooterRow.FindControl("ucSubRootCauseAdd")).SelectedSubCause
                        , ((TextBox)_gridView.FooterRow.FindControl("txtReasonAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertImmediateCause
                    (
                        new Guid(Filter.CurrentIncidentID)                        
                        , new Guid(((UserControlImmediateMainCause)_gridView.FooterRow.FindControl("ucRootCauseAdd")).SelectedMainCause)
                        , new Guid(((UserControlImmediateSubCause)_gridView.FooterRow.FindControl("ucSubRootCauseAdd")).SelectedSubCause)
                        , ((TextBox)_gridView.FooterRow.FindControl("txtReasonAdd")).Text
                    );

                BindImmediateCause();
                ((TextBox)_gridView.FooterRow.FindControl("txtReasonAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteImmediateCause(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsRootcauseid")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidCause(string causeid, string subcauseid, string reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(causeid) || causeid.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Main Cause is required.";

        if (string.IsNullOrEmpty(subcauseid) || subcauseid.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Sub Cause is required.";

        if (string.IsNullOrEmpty(reason))
            ucError.ErrorMessage = "Reason is required.";

        return (!ucError.IsError);
    }

    private void InsertImmediateCause(Guid incidentid, Guid immediatecauseid, Guid subimmediatecauseid, string reason)
    {
        PhoenixInspectionIncidentCAR.InsertImmediateCause(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            incidentid, immediatecauseid, subimmediatecauseid, reason);
    }

    private void UpdateImmediateCause(Guid incidentimmediatecauseid, Guid immediatecauseid, Guid subimmediatecauseid, string reason)
    {
        PhoenixInspectionIncidentCAR.UpdateImmediateCause(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            incidentimmediatecauseid, immediatecauseid, subimmediatecauseid, reason);
    }

    private void DeleteImmediateCause(Guid immediatecauseid)
    {
        PhoenixInspectionIncidentCAR.DeleteImmediateCause(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, immediatecauseid);
    }

    private void InsertBasicCause(Guid incidentid, Guid basiccauseid, Guid subbasiccauseid, string reason)
    {
        PhoenixInspectionIncidentCAR.InsertBasicCause(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, incidentid,
            basiccauseid, subbasiccauseid, reason);
    }

    private void UpdateBasicCause(Guid inspectionrootcauseid, Guid basiccauseid, Guid subbasiccauseid, string reason)
    {
        PhoenixInspectionIncidentCAR.UpdateBasicCause(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            inspectionrootcauseid, basiccauseid, subbasiccauseid, reason);
    }

    private void DeleteBasicCause(Guid inspectionrootcauseid)
    {
        PhoenixInspectionIncidentCAR.DeleteBasicCause(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, inspectionrootcauseid);
    }

    protected void gvImmediateCause_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindImmediateCause();
    }

    protected void gvImmediateCause_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            ViewState["ICURRENTINDEX"] = de.NewEditIndex;

            BindImmediateCause();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtReasonEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvImmediateCause_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidCause(
                     ((UserControlImmediateMainCause)_gridView.Rows[nCurrentRow].FindControl("ucRootCauseEdit")).SelectedMainCause
                    , ((UserControlImmediateSubCause)_gridView.Rows[nCurrentRow].FindControl("ucSubRootCauseEdit")).SelectedSubCause
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReasonEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateImmediateCause(
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsRootcauseidEdit")).Text)
                    , new Guid(((UserControlImmediateMainCause)_gridView.Rows[nCurrentRow].FindControl("ucRootCauseEdit")).SelectedMainCause)
                    , new Guid(((UserControlImmediateSubCause)_gridView.Rows[nCurrentRow].FindControl("ucSubRootCauseEdit")).SelectedSubCause)
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReasonEdit")).Text);

            _gridView.EditIndex = -1;
            ViewState["ICURRENTINDEX"] = -1;
            BindImmediateCause();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvImmediateCause_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvImmediateCause.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvImmediateCause_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvImmediateCause, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvImmediateCause_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlImmediateMainCause mc = (UserControlImmediateMainCause)e.Row.FindControl("ucRootCauseEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            
            if (mc != null)
                mc.SelectedMainCause = drv["FLDIMMEDIATECAUSEID"].ToString();

            UserControlImmediateSubCause sc = (UserControlImmediateSubCause)e.Row.FindControl("ucSubRootCauseEdit");
                        
            if (sc != null)
                sc.SelectedSubCause = drv["FLDSUBIMMEDIATECAUSEID"].ToString();

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");

            if (eb != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    eb.Visible = true;
                else
                    eb.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            
            if (sb != null) 
                sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            
            if (cb != null) 
                cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvBasicCause_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["BCURRENTINDEX"] = -1;
        BindBasicCause();
    }

    protected void gvBasicCause_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCause(((UserControlBasicMainCause)_gridView.FooterRow.FindControl("ucBasicCauseAdd")).SelectedMainCause
                        , ((UserControlBasicSubCause)_gridView.FooterRow.FindControl("ucSubBasicCauseAdd")).SelectedSubCause
                        , ((TextBox)_gridView.FooterRow.FindControl("txtBasicReasonAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertBasicCause
                    (
                        new Guid(Filter.CurrentIncidentID)                        
                        , new Guid(((UserControlBasicMainCause)_gridView.FooterRow.FindControl("ucBasicCauseAdd")).SelectedMainCause)
                        , new Guid(((UserControlBasicSubCause)_gridView.FooterRow.FindControl("ucSubBasicCauseAdd")).SelectedSubCause)
                        , ((TextBox)_gridView.FooterRow.FindControl("txtBasicReasonAdd")).Text
                    );

                BindBasicCause();
                ((TextBox)_gridView.FooterRow.FindControl("txtBasicReasonAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteBasicCause(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsBasiccauseid")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBasicCause_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindBasicCause();
    }

    protected void gvBasicCause_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            ViewState["BCURRENTINDEX"] = de.NewEditIndex;

            BindBasicCause();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtBasicReasonEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBasicCause_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidCause(
                     ((UserControlBasicMainCause)_gridView.Rows[nCurrentRow].FindControl("ucBasicCauseEdit")).SelectedMainCause
                    , ((UserControlBasicSubCause)_gridView.Rows[nCurrentRow].FindControl("ucSubBasicCauseEdit")).SelectedSubCause
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBasicReasonEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateBasicCause(
                    new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblInsBasiccauseidEdit")).Text)
                    , new Guid(((UserControlBasicMainCause)_gridView.Rows[nCurrentRow].FindControl("ucBasicCauseEdit")).SelectedMainCause)
                    , new Guid(((UserControlBasicSubCause)_gridView.Rows[nCurrentRow].FindControl("ucSubBasicCauseEdit")).SelectedSubCause)
                    , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBasicReasonEdit")).Text);

            _gridView.EditIndex = -1;
            ViewState["BCURRENTINDEX"] = -1;
            BindBasicCause();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBasicCause_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvBasicCause.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvBasicCause_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvBasicCause, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvBasicCause_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;                
            }

            UserControlBasicMainCause mc = (UserControlBasicMainCause)e.Row.FindControl("ucBasicCauseEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (mc != null) mc.SelectedMainCause = drv["FLDBASICCAUSEID"].ToString();

            UserControlBasicSubCause sc = (UserControlBasicSubCause)e.Row.FindControl("ucSubBasicCauseEdit");
            DataRowView drv1 = (DataRowView)e.Row.DataItem;
            if (sc != null) sc.SelectedSubCause = drv["FLDSUBBASICCAUSEID"].ToString();

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    eb.Visible = true;
                else
                    eb.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false; 
            }

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
}
