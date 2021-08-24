using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InspectionInspectorDeficiencySummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["DEFICIENCYID"] = "";
                ViewState["SOURCEID"] = "";
                ViewState["SOURCEID"] = Request.QueryString["SOURCEID"];
                ViewState["VESSELID"] = Request.QueryString["VESSELID"];
            }
                BindDeficiency();
                if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
                {
                   BindData();
                }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDeficiency()
    {
        try
        {
            DataSet ds = PhoenixInspectionDeficiency.ListDeficiencies(new Guid(ViewState["SOURCEID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
              //  gvDeficiency.DataSource = ds;
               // gvDeficiency.DataBind();

                if (ViewState["DEFICIENCYID"].ToString() == "")
                {
                    ViewState["DEFICIENCYID"] = ds.Tables[0].Rows[0]["FLDDEFICIENCYID"].ToString();
                    SetRowSelection();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                //ShowNoRecordsFound(dt, gvDeficiency);
            }

            gvDeficiency.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
               // BindPageURL(nCurrentRow);
                SetRowSelection();
                BindSource();
                BindData();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    

    private void SetRowSelection()
    {
        //gvDeficiency.SelectedIndex = -1;
        //for (int i = 0; i < gvDeficiency.Rows.Count; i++)
        //{
        //    if (gvDeficiency.DataKeys[i].Value.ToString().Equals(ViewState["DEFICIENCYID"].ToString()))
        //    {
        //        gvDeficiency.SelectedIndex = i;
        //    }
        //}
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblDeficiencyid = (RadLabel)gvDeficiency.Items[rowindex].FindControl("lblDeficiencyid");
            if (lblDeficiencyid != null)
            {
                ViewState["DEFICIENCYID"] = lblDeficiencyid.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindSource()
    {
        int? deftype = null;
        if (rblDeficiencyType.SelectedValue == "1" || rblDeficiencyType.SelectedValue == "2")
            deftype = 1;
        else
            deftype = 2;

        DataSet ds = PhoenixInspectionDeficiency.ListDeficiencySource(
              null
            , General.GetNullableInteger(ucVessel.SelectedVessel)
            , null
            , deftype
            );
        ddlSchedule.DataSource = ds;
        ddlSchedule.DataTextField = "FLDINSPECTIONSCHEDULENAME";
        ddlSchedule.DataValueField = "FLDINSPECTIONSCHEDULEID";
        ddlSchedule.DataBind();
        ddlSchedule.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    public void BindData()
    {
        if (ViewState["DEFICIENCYID"] != null && ViewState["DEFICIENCYID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionAuditNonConformity.EditAuditNC(new Guid(ViewState["DEFICIENCYID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //rblDeficiencyType.Enabled = false;
                if (dr["FLDRAISEDFROM"].ToString() == "3") //open reports
                {
                    rblDeficiencyType.Items.Clear();
                    rblDeficiencyType.DataSource = null;
                    rblDeficiencyType.Items.Insert(0, new ButtonListItem("NC", "2"));
                    rblDeficiencyType.Items.Insert(1, new ButtonListItem("Major NC", "1"));
                    //rblDeficiencyType.Enabled = false;
                    //ddlSchedule.Enabled = false;
                    //txtChecklistRef.Enabled = false;
                }
                if (dr["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MAJ"))
                {
                    rblDeficiencyType.SelectedValue = "1";
                }
                else if (dr["FLDNONCONFORMITYTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 173, "MIN"))
                {
                    rblDeficiencyType.SelectedValue = "2";
                }
                else
                {
                    rblDeficiencyType.SelectedValue = "3";
                }

                ViewState["RAISEDFROM"] = dr["FLDRAISEDFROM"].ToString();
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.bind();
                //ucVessel.Enabled = false;
                ucDate.Text = dr["FLDDATE"].ToString();
                BindSource();
                if (dr["FLDREVIEWSCHEDULEID"] != null && dr["FLDREVIEWSCHEDULEID"].ToString() != string.Empty)
                    ddlSchedule.SelectedValue = dr["FLDREVIEWSCHEDULEID"].ToString();
                else
                    ddlSchedule.SelectedIndex = 0;
                ucNonConformanceCategory.SelectedQuick = dr["FLDNCCATEGORYID"].ToString();
                txtChecklistRef.Text = dr["FLDCHECKLISTREFERENCENUMBER"].ToString();
                txtDesc.Text = dr["FLDCOMPREHENSIVEDESCRIPTION"].ToString();
                txtInspectorComments.Text = dr["FLDINSPECTORCOMMENTS"].ToString();
                txtMasterComments.Text = dr["FLDFOLLOWUPREMARKS"].ToString();
                txtOfficeRemarks.Text = dr["FLDEXTENSIONREMARKS"].ToString();
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ddlStatus.bind();
                //setEnabledDisabled();
                ucCompletionDate.Text = dr["FLDNCCOMPLETIONDATE"].ToString();
                ucCloseoutDate.Text = dr["FLDCLOSEDATE"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEOUTBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDCLOSEOUTREMARKS"].ToString();
                txtCancelReason.Text = dr["FLDNCCANCELREASON"].ToString();
                ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
                txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
                if (dr["FLDNCISRCACOMPLETED"].ToString() == "1")
                {
                    //chkRCAcompleted.Checked = false;
                }
                else
                {
                    //chkRCAcompleted.Checked = false;
                }
                if (dr["FLDNCISRCAREQUIRED"].ToString() == "0")
                {
                    //chkRCANotrequired.Checked = false;
                    //chkRCAcompleted.Enabled = false;
                    //chkRCAcompleted.Checked = false;
                    //ucRcaTargetDate.Enabled = false;
                }
                else
                {
                    //chkRCAcompleted.Enabled = false;
                    //ucRcaTargetDate.Enabled = false;
                    ucRcaTargetDate.Text = dr["FLDTARGETDATE"].ToString();

                }

            }

            DataSet ds1 = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["DEFICIENCYID"].ToString()));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                if (dr["FLDOBSERVATIONTYPE"].ToString().Equals("1"))
                    rblDeficiencyType.SelectedValue = "3";
                else if (dr["FLDOBSERVATIONTYPE"].ToString().Equals("2"))
                    rblDeficiencyType.SelectedValue = "4";
                //if (dr["FLDRAISEDFROM"].ToString() == "4")
                //{
                //    ddlSchedule.Enabled = false;
                //    txtChecklistRef.Enabled = false;
                //}
                ViewState["RAISEDFROM"] = dr["FLDRAISEDFROM"].ToString();

                //rblDeficiencyType.Enabled = false;
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.bind();
                //ucVessel.Enabled = false;
                ucDate.Text = dr["FLDDATE"].ToString();
                BindSource();
                if (dr["FLDINSPECTIONSCHEDULEID"] != null && dr["FLDINSPECTIONSCHEDULEID"].ToString() != string.Empty)
                    ddlSchedule.SelectedValue = dr["FLDINSPECTIONSCHEDULEID"].ToString();
                else
                    ddlSchedule.SelectedIndex = 0;
                ucNonConformanceCategory.SelectedQuick = dr["FLDRISKCATEGORYID"].ToString();
                txtChecklistRef.Text = dr["FLDCHECKLISTREFERENCENUMBER"].ToString();
                txtDesc.Text = dr["FLDOBSERVATION"].ToString();
                txtInspectorComments.Text = dr["FLDCOMMENTS"].ToString();
                txtMasterComments.Text = dr["FLDOWNERCOMMENTS"].ToString();
                txtOfficeRemarks.Text = dr["FLDEXTENSIONREMARKS"].ToString();
                ddlStatus.SelectedHard = dr["FLDSTATUS"].ToString();
                ddlStatus.bind();
                //setEnabledDisabled();
                ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                ucCloseoutDate.Text = dr["FLDCLOSEDATE"].ToString();
                txtCloseOutByName.Text = dr["FLDCLOSEOUTBYNAME"].ToString();
                txtCloseOutByDesignation.Text = dr["FLDCLOSEOUTBYDESIGNATION"].ToString();
                txtCloseOutRemarks.Text = dr["FLDCLOSEOUTREMARKS"].ToString();
                txtCancelReason.Text = dr["FLDOBSCANCELREASON"].ToString();
                ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
                txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
                txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
                if (dr["FLDOBSISRCAREQUIRED"].ToString() == "1")
                {
                    //chkRCAcompleted.Checked = false;
                    //ucRcaTargetDate.Enabled = false;
                    ucRcaTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                }
                else
                {
                    //chkRCAcompleted.Checked = false;
                    //ucRcaTargetDate.Enabled = false;
                }
                if (dr["FLDOBSISRCAREQUIRED"].ToString() == "0")
                {
                    //chkRCANotrequired.Checked = false;
                    //chkRCAcompleted.Enabled = false;
                    //chkRCAcompleted.Checked = false;
                    //ucRcaTargetDate.Enabled = false;
                }
                else
                {
                    //chkRCAcompleted.Enabled = false;
                    //ucRcaTargetDate.Enabled = false;
                    ucRcaTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                }
            }
        }
    }
}
