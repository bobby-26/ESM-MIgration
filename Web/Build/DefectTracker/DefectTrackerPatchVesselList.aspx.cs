using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;
using System.Xml;

public partial class DefectTrackerPatchVesselList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);


            PhoenixToolbar toolbaredit = new PhoenixToolbar();
            toolbaredit.AddButton("Add/Edit", "ADDEDIT");
            toolbaredit.AddButton("Vessel List", "VESSELLIST");
            MenuPatchDetail.AccessRights = this.ViewState;
            MenuPatchDetail.MenuList = toolbaredit.Show();
            MenuPatchDetail.SelectedMenuIndex = 1;


            ViewState["SESSIONID"] = Guid.NewGuid();
            ViewState["MESSAGEID"] = Guid.NewGuid();

            ViewState["PATCHPROJECTDTKEY"] = Request.QueryString["projectdtkey"].ToString();
            ViewState["PATCHDTKEY"] = Request.QueryString["dtkey"].ToString();
            GetProjectInformation();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void GetProjectInformation()
    {
        DataTable dt = PhoenixPatchTracker.PatchProjectEdit(General.GetNullableGuid(ViewState["PATCHPROJECTDTKEY"].ToString()));
        ViewState["CALLNUMBER"] = dt.Rows[0]["FLDCALLNUMBER"].ToString();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow gridveiw = (GridViewRow)chk.Parent.Parent;
        Label lblVesselID = (Label)gridveiw.FindControl("lblVesselID");
        if (chk.Checked)
        {
            PhoenixDefectTracker.PatchVesselListSave(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(ViewState["PATCHDTKEY"].ToString()), int.Parse(lblVesselID.Text), 1
                                                     );
        }
    }

    protected void MenuPatchDetail_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("ADDEDIT"))
        {
            Response.Redirect("DefectTrackerPatchAddEdit.aspx?dtkey=" + ViewState["PATCHDTKEY"].ToString() + "&projectdtkey=" + ViewState["PATCHPROJECTDTKEY"].ToString());
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixDefectTracker.PatchVesselList(General.GetNullableGuid(ViewState["PATCHDTKEY"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPatch.DataSource = ds;
            gvPatch.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPatch);
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            txtTotalPatchSent.Text = ds.Tables[1].Rows[0]["COUNTOFSENT"].ToString();
            txtTotalAcknowledgement.Text = ds.Tables[1].Rows[0]["COUNOFACKNOWLEDGED"].ToString();
        }
    }

    protected void gvPatch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvPatch_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvPatch.EditIndex = -1;
        gvPatch.SelectedIndex = -1;
        BindData();
    }

    protected void gvPatch_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPatch, "Edit$" + e.Row.RowIndex.ToString(), false);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvPatch, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
        SetKeyDownScroll(sender, e);
    }

    protected void gvPatch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            CheckBox chkSelect = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkSelectEdit");

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidValues(((TextBox)_gridView.Rows[nCurrentRow].FindControl("lblRemarksEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePatch(General.GetNullableGuid(ViewState["PATCHDTKEY"].ToString()),
                            Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselID")).Text),
                            General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateAcknowledged")).Text),
                            General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateSenton")).Text),
                            General.GetNullableInteger((chkSelect.Checked == true) ? "1" : "0"),
                            General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("lblRemarksEdit")).Text)
                            );
                _gridView.EditIndex = -1;
                BindData();
            }

            Label lblVesselID = (Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselID");

            if (e.CommandName.ToUpper().Equals("SENDEMAIL"))
            {
                PhoenixDefectTracker.PatchToSend(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(lblVesselID.Text),
                                                 new Guid(ViewState["PATCHDTKEY"].ToString()),
                                                 "SEP",
                                                 new Guid(ViewState["PATCHPROJECTDTKEY"].ToString()));
                ucStatus.Text = "Patch sent to vessel";
                _gridView.EditIndex = -1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPatch_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
    }

    protected void gvPatch_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
    }

    protected void gvPatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        //_gridView.EditIndex = -1;
        BindData();
    }

    protected void gvPatch_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        string dtkey = ViewState["PATCHDTKEY"].ToString();
        string projectdtkey = ViewState["PATCHPROJECTDTKEY"].ToString();

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");
            CheckBox chkEdit = (CheckBox)e.Row.FindControl("chkSelectEdit");
            Label lblVesselID = (Label)e.Row.FindControl("lblVesselID");
            Label lbldtkey = (Label)e.Row.FindControl("lbldtkey");
            DataRowView drv = (DataRowView)e.Row.DataItem;

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdSendemail");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are sure want to send the patch to vessel ?'); return false;");
                }
            }

            if (chk != null)
            {
                chk.Checked = drv["FLDSELECTEDYN"].ToString().Equals("1") ? true : false;
                if ((General.GetNullableInteger(drv["FLDSENTCOUNT"].ToString()) != null) && (General.GetNullableInteger(drv["FLDSENTCOUNT"].ToString()) > 0))
                {
                    chk.Checked = true;
                    chk.Enabled = false;
                }
            }


            if (chkEdit != null)
            {
                chkEdit.Checked = drv["FLDSELECTEDYN"].ToString().Equals("1") ? true : false;
                if ((General.GetNullableInteger(drv["FLDSENTCOUNT"].ToString()) != null) && (General.GetNullableInteger(drv["FLDSENTCOUNT"].ToString()) > 0))
                {
                    chkEdit.Checked = true;
                    chkEdit.Enabled = false;
                }
            }
            string callnumber = ViewState["CALLNUMBER"].ToString();
            ImageButton vn = (ImageButton)e.Row.FindControl("cmdMap");
            if (vn != null) vn.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'DefectTrackerPatchMailMap.aspx?callnumber=" + callnumber + "&vesselid=" + lblVesselID.Text + "&dtkey=" + dtkey + "&projectdtkey=" + projectdtkey + "'); return false;");
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPatch.EditIndex = -1;
        gvPatch.SelectedIndex = -1;
        BindData();
    }

    private void UpdatePatch(Guid? dtkey, int vesselid, DateTime? acknowledgedon, DateTime? senton, int? selectyn, string remarks)
    {

        PhoenixDefectTracker.PatchVesselUpdate(dtkey, vesselid, acknowledgedon, senton, selectyn, remarks);
        ucStatus.Text = "Patch information updated";
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private bool IsValidValues(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (remarks == "")
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }
}
