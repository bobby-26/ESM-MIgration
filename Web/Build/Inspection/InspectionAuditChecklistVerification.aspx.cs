using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionAuditChecklistVerification : PhoenixBasePage
{
    Guid? verificationid = null;
    Guid? checklistid = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Page.Header.DataBind(); 
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSectionmit.Attributes.Add("style", "display:none;");

            if (!IsPostBack)
            {
                VesselConfiguration();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["REVIEWSCHEDULEID"] != null && Request.QueryString["REVIEWSCHEDULEID"].ToString() != "")
                {
                    ViewState["REVIEWSCHEDULEID"] = Request.QueryString["REVIEWSCHEDULEID"].ToString();
                }
                else
                    ViewState["REVIEWSCHEDULEID"] = "";

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                }
                else
                    ViewState["VESSELID"] = "";
            }
            ucError.ErrorMessage = "";
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void Rebind()
    {
        gvChecklistVerification.SelectedIndexes.Clear();
        gvChecklistVerification.EditIndexes.Clear();
        gvChecklistVerification.DataSource = null;
        gvChecklistVerification.Rebind();
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixInspectionAuditChecklistVerification.InspectionAuditChecklistVerificationSearch(
                                                                new Guid(ViewState["REVIEWSCHEDULEID"].ToString())
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);


        gvChecklistVerification.DataSource = ds;
        ViewState["ROWCOUNT"] = iRowCount;
        gvChecklistVerification.VirtualItemCount = iRowCount;

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSectionmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvChecklistVerification.Rebind();

    }

    private bool IsValidVerification(string remarks, string checklistid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(checklistid) == null)
            ucError.ErrorMessage = "Select the checklist";

        if (General.GetNullableString(remarks) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    private bool IsValidVerificationAdd(string remarks, string checklistid, string verificationid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(checklistid) == null)
            ucError.ErrorMessage = "Select the checklist.";
        else
        {
            if (General.GetNullableGuid(verificationid) == null)
                ucError.ErrorMessage = "Please edit the existing row for the checklist.";
        }

        if (General.GetNullableString(remarks) == null)
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    public class GridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentCategoryName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblChecklist")).Text;
                string previousCategoryName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblChecklist")).Text;

                if (currentCategoryName == previousCategoryName)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                         previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;

                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                         previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }
            }
        }
    }


    protected void gvChecklistVerification_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                verificationid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVerificationId")).Text);
                checklistid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblChecklistId")).Text);

                if (verificationid != null) // if the checklist is already inserted in table
                {
                    if (IsValidVerification(((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text, checklistid.ToString()))
                    {
                        PhoenixInspectionAuditChecklistVerification.UpdateInspectionAuditChecklistVerification(
                                            General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVerificationLineItemId")).Text)
                                            , int.Parse(ViewState["VESSELID"].ToString())
                                            , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkIsDoneEdit")).Checked ? "1" : "0")
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text));

                        Rebind();
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    if (IsValidVerification(((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text, checklistid.ToString()))
                    {
                        PhoenixInspectionAuditChecklistVerification.InsertInspectionAuditChecklistVerification(
                                            General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
                                            , General.GetNullableGuid(verificationid.ToString())
                                            , int.Parse(ViewState["VESSELID"].ToString())
                                            , new Guid(checklistid.ToString())
                                            , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkIsDoneEdit")).Checked ? "1" : "0")
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text));
                        Rebind();
                    }
                    else
                    {
                        ucError.Visible = true;
                        return;
                    }
                }
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (IsValidVerificationAdd(((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text, checklistid.ToString(), verificationid.ToString()))
                {
                    if (ViewState["REVIEWSCHEDULEID"] != null && ViewState["REVIEWSCHEDULEID"].ToString() != "")
                    {
                        PhoenixInspectionAuditChecklistVerification.InsertInspectionAuditChecklistVerification(
                                            General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
                                            , General.GetNullableGuid(verificationid.ToString())
                                            , int.Parse(ViewState["VESSELID"].ToString())
                                            , new Guid(checklistid.ToString())
                                            , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkIsDoneAdd")).Checked ? "1" : "0")
                                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text));
                        Rebind();
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionAuditChecklistVerification.DeleteInspectionAuditChecklistVerification(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVerificationLineItemId")).Text));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("CHECKLISTCLEAR"))
            {
                foreach (GridDataItem rows in gvChecklistVerification.Items)
                {
                    RadRadioButton rdbtnl = (RadRadioButton)rows.FindControl("rdbUser");
                    if (rdbtnl != null)
                    {
                        if (rdbtnl.Checked == true)
                        {
                            rdbtnl.Checked = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvChecklistVerification_ItemDataBound(object sender, GridItemEventArgs e)
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

            GridDecorator.MergeRows(gvChecklistVerification, e);
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

    protected void gvChecklistVerification_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void rdbUser_CheckedChanged(object sender, EventArgs e)
    {
        RadRadioButton rb = (RadRadioButton)sender;
        string currentrbid = rb.ClientID;
        foreach (GridDataItem rows in gvChecklistVerification.Items)
        {
            RadRadioButton rdbtnl = (RadRadioButton)rows.FindControl("rdbUser");
            if (rdbtnl != null && rdbtnl.ClientID != currentrbid)
            {
                if (rdbtnl.Checked == true)
                {
                    rdbtnl.Checked = false;
                }
            }
        }
        if (rb != null && rb.Checked == true)
        {
            GridDataItem row = (GridDataItem)rb.Parent.Parent;
            CheckBox chkIsDoneAdd = row.Parent.Controls[1].Controls[0].FindControl("chkIsDoneAdd") as CheckBox;
            //CheckBox chkIsDoneAdd = (CheckBox)row.FindControl("chkIsDoneAdd");
            if (chkIsDoneAdd != null)
            {
                chkIsDoneAdd.Enabled = true;
            }
            RadTextBox txtRemarksAdd = row.Parent.Controls[1].Controls[0].FindControl("txtRemarksAdd") as RadTextBox;
            //RadTextBox txtRemarksAdd = (RadTextBox)row.FindControl("txtRemarksAdd");
            if (txtRemarksAdd != null)
            {
                txtRemarksAdd.Enabled = true;
            }
            verificationid = General.GetNullableGuid(((RadLabel)row.FindControl("lblVerificationId")).Text);
            checklistid = General.GetNullableGuid(((RadLabel)row.FindControl("lblChecklistId")).Text);
        }
    }
}
