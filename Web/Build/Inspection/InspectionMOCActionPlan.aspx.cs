using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class InspectionMOCActionPlan : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvMOCActionPlan.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvMOCActionPlan.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "BACK");
            MenuMOCRequest.MenuList = toolbarmain.Show();
            ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
            ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();
            ViewState["MOCREQUIREDID"] = "";
            ViewState["DepartmentAdd"] = "";
            ViewState["DepartmentEdit"] = "";
            ViewState["txtActionToBeTakenAdd"] = "";
        }
        BindActionPlan();
    }
    protected void MOCRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionManagementOfChangeList.aspx?MOCID=" + new Guid((ViewState["MOCID"]).ToString()), false);
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindActionPlan()
    {
        string[] alColumns = { "FLDACTIONTOBETAKEN", "FLDPERSONINCHARGE", "FLDDOCUMENTATTACHMENT", "FLDTARGETDATE", "FLDCOMPLETIONDATE" };
        string[] alCaptions = { "Actions to be taken", "Person In Charge", "Documents to be uploaded as evidence", "Target date", "Completion date" };

        DataSet ds = PhoenixInspectionMOCActionPlan.MOCActionPlanList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , new Guid((ViewState["MOCID"]).ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMOCActionPlan.DataSource = ds;
            gvMOCActionPlan.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvMOCActionPlan);
        }
    }
    protected void gvMOCActionPlan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindActionPlan();
        ViewState["DepartmentEdit"] = "";
    }
    protected void gvMOCActionPlan_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string deparmenttypeid = "";
                DataSet ds;
                if (((UserControlDepartment)_gridView.FooterRow.FindControl("ucDepartmentadd")).SelectedDepartment != "Dummy")
                {
                    ds = PhoenixRegistersDepartment.EditDepartment(int.Parse(((UserControlDepartment)_gridView.FooterRow.FindControl("ucDepartmentadd")).SelectedDepartment));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        deparmenttypeid = dr["FLDDEPARTMENTTYPEID"].ToString();
                    }
                }

                if (!IsValidMOCActionPlan(((TextBox)_gridView.FooterRow.FindControl("txtActionToBeTakenAdd")).Text
                                        , ((UserControlDate)_gridView.FooterRow.FindControl("txtTargetdateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                //PhoenixInspectionMOCActionPlan.MOCActionPlanInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                                                      , General.GetNullableGuid((ViewState["MOCREQUIREDID"]).ToString())
                //                                                      , new Guid((ViewState["MOCID"]).ToString())
                //                                                      , int.Parse(ViewState["Vesselid"].ToString())
                //                                                      , General.GetNullableGuid(null)
                //                                                      , General.GetNullableGuid(null)
                //                                                      , ViewState["txtActionToBeTakenAdd"].ToString()
                //                                                      , (deparmenttypeid == "2") ? General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtOfficeId")).Text) : General.GetNullableInteger(((TextBox)_gridView.FooterRow.FindControl("txtCrewId")).Text)
                //                                                      , (deparmenttypeid == "2") ? ((TextBox)_gridView.FooterRow.FindControl("txtName")).Text : ((TextBox)_gridView.FooterRow.FindControl("txtCrewName")).Text
                //                                                      , (deparmenttypeid == "2") ? ((TextBox)_gridView.FooterRow.FindControl("txtRank")).Text : ((TextBox)_gridView.FooterRow.FindControl("txtCrewRank")).Text
                //                                                      , ((TextBox)_gridView.FooterRow.FindControl("txtDocumentsAdd")).Text
                //                                                      , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("txtTargetdateAdd")).Text)
                //                                                      , General.GetNullableDateTime(null)
                //                                                      , General.GetNullableInteger(((UserControlDepartment)_gridView.FooterRow.FindControl("ucDepartmentadd")).SelectedDepartment)
                //                                                      , General.GetNullableInteger(deparmenttypeid)
                //                                                      , ((TextBox)_gridView.FooterRow.FindControl("txtremarksadd")).Text
                //                                                      , General.GetNullableDateTime(null)
                //                                                      , null
                //                                                      , General.GetNullableDateTime(null)
                //                                                      , General.GetNullableInteger(null)
                //                                                      , General.GetNullableInteger(null)
                //                                                      , General.GetNullableInteger(null));
                //BindActionPlan();
                //((TextBox)_gridView.FooterRow.FindControl("txtActionToBeTakenAdd")).Text = "";
                //((UserControlDepartment)_gridView.FooterRow.FindControl("ucDepartmentadd")).SelectedDepartment = "Dummy";
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOCActionPlan.MOCActionPlanDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMOCActionPlanid")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMOCActionPlan_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindActionPlan();
    }
    protected void gvMOCActionPlan_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindActionPlan();
    }
    protected void gvMOCActionPlan_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string deparmenttypeid = "";
            DataSet ds;
            if (((UserControlDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDepartmentedit")).SelectedDepartment != "Dummy")
            {
                ds = PhoenixRegistersDepartment.EditDepartment(int.Parse(((UserControlDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDepartmentedit")).SelectedDepartment));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    deparmenttypeid = dr["FLDDEPARTMENTTYPEID"].ToString();
                }
            }

            if (!IsValidMOCActionPlan(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtActionToBeTakenEdit")).Text
                                        , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtTargetdateEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionMOCActionPlan.MOCActionPlanUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , General.GetNullableGuid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMOCActionPlanid")).Text)
                                                                     , new Guid((ViewState["MOCID"]).ToString())
                                                                     , int.Parse(ViewState["Vesselid"].ToString())
                                                                     , General.GetNullableGuid(null)
                                                                     , General.GetNullableGuid(null)
                                                                     , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtActionToBeTakenEdit")).Text
                                                                     , (deparmenttypeid == "2") ? General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOfficeIdEdit")).Text) : General.GetNullableInteger(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCrewIdEdit")).Text)
                                                                     , (deparmenttypeid == "2") ? ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNameEdit")).Text : ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCrewNameEdit")).Text
                                                                     , (deparmenttypeid == "2") ? ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRankEdit")).Text : ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCrewRankEdit")).Text
                                                                     , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDocumentsEdit")).Text
                                                                     , DateTime.Parse(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtTargetdateEdit")).Text)
                                                                     , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtCompletiondateEdit")).Text)
                                                                     , General.GetNullableInteger(((UserControlDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDepartmentedit")).SelectedDepartment)
                                                                     , General.GetNullableInteger(deparmenttypeid)
                                                                     , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtremarksedit")).Text)
                                                                     , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtRescheduledateEdit")).Text)
                                                                     , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRescheduleremarksEdit")).Text)
                                                                     , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtcloseddateEdit")).Text)
                                                                     , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblcompletedby")).Text)
                                                                     , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblclosedby")).Text)
                                                                     , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblverfication")).Text));


            _gridView.EditIndex = -1;
            BindActionPlan();
            ViewState["DepartmentEdit"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void gvMOCActionPlan_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            ImageButton imgOfficeEdit = (ImageButton)e.Row.FindControl("imgOfficeEdit");
            HtmlControl actionplanofficeedit = (HtmlControl)e.Row.FindControl("actionplanofficeedit");
            HtmlControl actionplancrewedit = (HtmlControl)e.Row.FindControl("actionplancrewedit");
            ImageButton imgPersonInChargeEdit = (ImageButton)e.Row.FindControl("imgPersonInChargeEdit");
            Label lbldepartmentid = (Label)e.Row.FindControl("lbldepartmentid");
            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            Label lblVesselid = (Label)e.Row.FindControl("lblVesselId");
            UserControlDepartment ucDepartmentEdit = (UserControlDepartment)e.Row.FindControl("ucDepartmentEdit");
            TextBox txtCrewNameEdit = (TextBox)e.Row.FindControl("txtCrewNameEdit");
            TextBox txtCrewRankEdit = (TextBox)e.Row.FindControl("txtCrewRankEdit");
            TextBox txtCrewIdEdit = (TextBox)e.Row.FindControl("txtCrewIdEdit");
            TextBox txtNameEdit = (TextBox)e.Row.FindControl("txtNameEdit");
            TextBox txtRankEdit = (TextBox)e.Row.FindControl("txtRankEdit");
            TextBox txtOfficeIdEdit = (TextBox)e.Row.FindControl("txtOfficeIdEdit");
            TextBox txtOfficeEmailEdit = (TextBox)e.Row.FindControl("txtOfficeEmailEdit");

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                        att.ImageUrl = Session["images"] + "/no-attachment.png";
                    att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.QUALITY + "&type=MOCACTIONPLAN&cmdname=MOCACTIONPLANUPLOAD&VESSELID=" + lblVesselid.Text + "'); return true;");
                }
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                if ((ViewState["DepartmentEdit"].ToString() == ""))
                {
                    ucDepartmentEdit.SelectedDepartment = lbldepartmentid.Text;
                }
                else
                {
                    ucDepartmentEdit.SelectedDepartment = ViewState["DepartmentEdit"].ToString();
                }
                string deparmenttypeid = "";
                DataSet ds;
                ds = PhoenixRegistersDepartment.EditDepartment(int.Parse(ucDepartmentEdit.SelectedDepartment));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    deparmenttypeid = dr["FLDDEPARTMENTTYPEID"].ToString();
                }
                if (ucDepartmentEdit.SelectedDepartment != lbldepartmentid.Text)
                {
                    txtCrewNameEdit.Text = "";
                    txtCrewRankEdit.Text = "";
                    txtCrewIdEdit.Text = "";
                    txtNameEdit.Text = "";
                    txtRankEdit.Text = "";
                    txtOfficeIdEdit.Text = "";
                    txtOfficeEmailEdit.Text = "";
                }


                if (deparmenttypeid == "2")
                {
                    actionplancrewedit.Visible = false;
                    if (imgOfficeEdit != null)
                    {
                        imgOfficeEdit.Attributes.Add("onclick", "return showPickList('spnOfficeEdit', 'codehelp1', '', '../Common/CommonPickListUser.aspx?Departmentid=" + ucDepartmentEdit.SelectedDepartment + "&MOC=true', true);");
                    }
                }
                if (deparmenttypeid == "1")
                {
                    actionplanofficeedit.Visible = false;
                    if (imgPersonInChargeEdit != null)
                    {
                        imgPersonInChargeEdit.Attributes.Add("onclick", "return showPickList('spnPersonInChargeactionplanEdit', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                                                        + ViewState["Vesselid"].ToString().ToString() + "', true); ");
                    }
                }
                if ((ucDepartmentEdit.SelectedDepartment == "0") || (ucDepartmentEdit.SelectedDepartment == "Dummy"))
                {
                    actionplancrewedit.Visible = false;
                    actionplanofficeedit.Visible = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton imgPIC = (ImageButton)e.Row.FindControl("imgPIC");
            ImageButton imgOffice = (ImageButton)e.Row.FindControl("imgOffice");
            HtmlControl actionplanoffice = (HtmlControl)e.Row.FindControl("actionplanoffice");
            UserControlDepartment ucDepartmentAdd = (UserControlDepartment)e.Row.FindControl("ucDepartmentAdd");
            HtmlControl actionplancrew = (HtmlControl)e.Row.FindControl("actionplancrew");
            ImageButton imgPersonInCharge = (ImageButton)e.Row.FindControl("imgPersonInCharge");
            TextBox txtCrewId = (TextBox)e.Row.FindControl("txtCrewId");
            TextBox txtActionToBeTakenAdd = (TextBox)e.Row.FindControl("txtActionToBeTakenAdd");

            if ((ViewState["DepartmentAdd"].ToString() != null) || (ViewState["DepartmentAdd"].ToString() != ""))
            {
                ucDepartmentAdd.SelectedDepartment = ViewState["DepartmentAdd"].ToString();
                txtActionToBeTakenAdd.Text = ViewState["txtActionToBeTakenAdd"].ToString();
            }
            string deparmenttypeid = "";
            DataSet ds;
            if (ucDepartmentAdd.SelectedDepartment != "")
            {
                ds = PhoenixRegistersDepartment.EditDepartment(int.Parse(ucDepartmentAdd.SelectedDepartment));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    deparmenttypeid = dr["FLDDEPARTMENTTYPEID"].ToString();
                }
            }
            if (deparmenttypeid == "2")
            {
                actionplancrew.Visible = false;
                if (imgOffice != null)
                {
                    imgOffice.Attributes.Add("onclick", "return showPickList('spnOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?Departmentid=" + ucDepartmentAdd.SelectedDepartment + "&MOC=true', true);");
                }

            }
            if (deparmenttypeid == "1")
            {
                actionplanoffice.Visible = false;
                if (imgPersonInCharge != null)
                {
                    imgPersonInCharge.Attributes.Add("onclick", "return showPickList('spnPersonInChargeactionplan', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                        + ViewState["Vesselid"].ToString().ToString() + "', true); ");
                }
            }
            if ((ucDepartmentAdd.SelectedDepartment == "0") || (ucDepartmentAdd.SelectedDepartment == "Dummy"))
            {
                actionplanoffice.Visible = false;
                actionplancrew.Visible = false;
            }
        }
    }
    private bool IsValidMOCActionPlan(string ActionToBeTaken, string Targetdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ActionToBeTaken.Equals(""))
            ucError.ErrorMessage = "Actions to be taken is Required";

        if (General.GetNullableDateTime(Targetdate) == null)
            ucError.ErrorMessage = "Target date is Required";

        return (!ucError.IsError);
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
    protected void gvMOCActionPlan_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
           && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
           && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvMOCActionPlan, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }
    public void ucDepartmentAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        UserControlDepartment ucDepartmentAdd = gvMOCActionPlan.FooterRow.FindControl("ucDepartmentAdd") as UserControlDepartment;
        TextBox txtActionToBeTakenAdd = gvMOCActionPlan.FooterRow.FindControl("txtActionToBeTakenAdd") as TextBox;

        ViewState["DepartmentAdd"] = ucDepartmentAdd.SelectedDepartment;
        ViewState["txtActionToBeTakenAdd"] = txtActionToBeTakenAdd.Text;
        BindActionPlan();
    }
    public void ucDepartmentEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        int nCurrentRow = gvMOCActionPlan.SelectedRow.RowIndex;
        UserControlDepartment ucDepartmentEdit = gvMOCActionPlan.Rows[nCurrentRow].FindControl("ucDepartmentEdit") as UserControlDepartment;

        ViewState["DepartmentEdit"] = ucDepartmentEdit.SelectedDepartment;
        BindActionPlan();
    }
}


