using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;

public partial class Inspection_InspectionTrainingScheduleReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!Page.IsPostBack)
        {
          
            confirm.Attributes.Add("style", "display:none");
            ViewState["TRAININGSCHEDULEID"] = General.GetNullableGuid(Request.QueryString["Trainingscheduleid"]);
            ViewState["LAUNCH"] = General.GetNullableString(Request.QueryString["l"]);
            ViewState["E"]= General.GetNullableString(Request.QueryString["e"]);
            ViewState["OR"] = Request.QueryString["ownerreport"];
            Guid? TrainingScheduleId = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());
            DataTable scenariotable = PhoenixRegisterTrainingScenario.TrainingScenariodropdownlist(TrainingScheduleId);
            Radddlscenarioedit.DataSource = scenariotable;
            Radddlscenarioedit.DataTextField = "FLDSCENARIO";
            Radddlscenarioedit.DataValueField = "FLDSCENARIOID";
            Radddlscenarioedit.DataBind();
            DataTable dt1 = PhoenixInspectionTrainingSchedule.Getflddtkey(TrainingScheduleId);
            Guid? flddtkey = General.GetNullableGuid(dt1.Rows[0]["FLDDTKEY"].ToString());
            ViewState["DTKEY"] = flddtkey;
            attachments.Attributes["src"] = "" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                            + PhoenixModule.QUALITY;
            int attachment = 0;
            int photoyn = 0;
            PhoenixInspectionTrainingSchedule.PhotoYN(TrainingScheduleId, ref photoyn);
            PhoenixInspectionTrainingSchedule.Attachmentyn(flddtkey, ref attachment);
            if (attachment == 0 & photoyn==1)
            {

                Noattachmentreasontitle.Visible = true;
                Noattachmentreasontitle1.Visible = true;
                reasonfornoattachments.Visible = true;
            }
            else
            {

                Noattachmentreasontitle.Visible = false;
                Noattachmentreasontitle1.Visible = false;
                reasonfornoattachments.Visible = false;
                reasonfornoattachments.Text = string.Empty;

            }
            DataTable dt = PhoenixInspectionTrainingSchedule.TrainingScheduleEditList(TrainingScheduleId);

            if (dt.Rows.Count > 0)
            {
                radlbltrainingname.Text = dt.Rows[0]["FLDTRAININGNAME"].ToString();
                radlblinterval.Text = dt.Rows[0]["FLDFREQUENCY"].ToString() + "  " + dt.Rows[0]["FLDFREQUENCYTYPE"].ToString();
                radlblfixedorvariable.Text = dt.Rows[0]["FLDFIXEDORVARIABLE"].ToString();
                radlbltype.Text = dt.Rows[0]["FLDTYPE"].ToString();
                if (dt.Rows[0]["FLDTRAININGONBOARDDUEDATE"] != null)
                {
                    radlblduedate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDTRAININGONBOARDDUEDATE"].ToString());
                }
                if (dt.Rows[0]["FLDTRAININGONBOARDLASTDONEDATE"] != null)
                {
                    if(General.GetNullableString( dt.Rows[0]["FLDSUBMISSIONSTATUS"].ToString()) !=null)
                        radlastdonedate.Text = dt.Rows[0]["FLDTRAININGONBOARDLASTDONEDATE"].ToString();

                    lastdonecheck.Text = General.GetDateTimeToString(dt.Rows[0]["FLDTRAININGONBOARDLASTDONEDATE"].ToString());
                }
                if (dt.Rows[0]["FLDSCENARIOID"] != null)
                {
                    BindCheckBoxList(Radddlscenarioedit, dt.Rows[0]["FLDSCENARIOID"].ToString());
                }
                if (dt.Rows[0]["FLDREMARKS"] != null)
                {
                    tbremarksentry.Text = dt.Rows[0]["FLDREMARKS"].ToString();
                }

                if (dt.Rows[0]["FLDREASON"] != null)
                {
                    reasonfornoattachments.Text = dt.Rows[0]["FLDREASON"].ToString();
                }
            }
        }

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Confirm", "Toggle2", ToolBarDirection.Right);
        if(ViewState["E"] == null)
            toolbargrid.AddButton("Save", "Toggle1", ToolBarDirection.Right);
        Tabstriptrainingschedulereportmenu.MenuList = toolbargrid.Show();

        if (ViewState["OR"] != null)
        {
            Tabstriptrainingschedulereportmenu.Visible = false;
            instructions.Visible = false;
            attachments.Attributes["src"] = "" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"] + "&U=Y" + "&mod="
                           + PhoenixModule.QUALITY;
        }
    }

    public static void BindCheckBoxList(RadComboBox cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.Items.FindItemByValue(item) != null)
                    cbl.Items.FindItemByValue(item).Checked = true;
            }
        }

    }

    protected void trainingschedulereportmenu_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("TOGGLE1"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? TrainingScheduleId = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());
                string scenario = string.Empty;
                string scenarioids = string.Empty;
                scenario = General.GetNullableString(GetCheckedItems(Radddlscenarioedit, scenario));
                scenarioids = General.GetNullableString(GetCheckedItemsvalues(Radddlscenarioedit, scenarioids));
                string remarks = General.GetNullableString(tbremarksentry.Text);
                DateTime? trainingdonedate = General.GetNullableDateTime(radlastdonedate.Text);
                int? statusid = General.GetNullableInteger("1");
                string status = "Draft";
                DateTime? previouslastdone = General.GetNullableDateTime(lastdonecheck.Text);

                string reason = General.GetNullableString(reasonfornoattachments.Text);
                if (!IsValidTrainingScheduleDetails(trainingdonedate, statusid, previouslastdone,null))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid? drillid = null;

                PhoenixInspectionTrainingSchedule.TrainingScheduleReport(rowusercode, TrainingScheduleId, scenario, scenarioids, remarks, trainingdonedate, status, statusid, reason, ref drillid);
              
                if (ViewState["LAUNCH"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                          "BookMarkScript", "closeTelerikWindow('Report', 'wo',true);", true);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                          "BookMarkScript", "closeTelerikWindow('Report', 'wo',true);", true);
            }

            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {

                RadWindowManager1.RadConfirm("Once Training is reported it cannot be edited. Click on Ok to  report  Or click on Cancel to continue editing?", "confirm", 320, 150, null, "Confirm");


            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected static string GetCheckedItems(RadComboBox comboBox, string checkednames)
    {

        var sb = new StringBuilder();
        var collection = comboBox.CheckedItems;

        if (collection.Count != 0)
        {
            foreach (var item in collection)
                sb.Append(item.Text + ",");
            checkednames = sb.ToString();
        }

        return checkednames;

    }

    protected static string GetCheckedItemsvalues(RadComboBox comboBox, string checkednames)
    {

        var sb = new StringBuilder();
        var collection = comboBox.CheckedItems;

        if (collection.Count != 0)
        {


            foreach (var item in collection)
                sb.Append(item.Value + ",");


            checkednames = sb.ToString();
        }

        return checkednames;


    }

    private bool IsValidTrainingScheduleDetails(DateTime? Trainingdonedate, int? statusid, DateTime? x, string edit)
    {

        ucError.HeaderMessage = "Please provide the following required information";


        if (statusid == 1 && Trainingdonedate == null)
        {
            ucError.ErrorMessage = "Training Done Date";
        }


        if (edit == null)
        {
            if (statusid == 2 && Trainingdonedate == null)
            {

                ucError.ErrorMessage = "Training Done Date";
            }
            if (Trainingdonedate != null && x != null)
            {
                if ((x > Trainingdonedate))
                {

                    ucError.ErrorMessage = "Training Done Date cannot be less than or Same as previous done date ";
                }
            }
        }
        if (Trainingdonedate > DateTime.Now.Date)
        {

            ucError.ErrorMessage = "Training Done Date Cannot be Greater than Today. Enter valid date.";
        }
        return (!ucError.IsError);
    }
    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {

            int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            Guid? TrainingScheduleId = General.GetNullableGuid(ViewState["TRAININGSCHEDULEID"].ToString());
            string scenario = string.Empty;
            string scenarioids = string.Empty;
            scenario = General.GetNullableString(GetCheckedItems(Radddlscenarioedit, scenario));
            scenarioids = General.GetNullableString(GetCheckedItemsvalues(Radddlscenarioedit, scenarioids));
            string remarks = General.GetNullableString(tbremarksentry.Text);
            DateTime? Trainingdonedate = General.GetNullableDateTime(radlastdonedate.Text);
            int? statusid = General.GetNullableInteger("2");
            string status = "Confirm";
            DateTime? previouslastdone = General.GetNullableDateTime(lastdonecheck.Text);
            string edit = null;
            if (ViewState["E"] != null)
            {
                edit = ViewState["E"].ToString();
            }
            string reason = General.GetNullableString(reasonfornoattachments.Text);
            if (!IsValidTrainingScheduleDetails(Trainingdonedate, statusid, previouslastdone, edit))
            {
                ucError.Visible = true;
                return;
            }

            Guid? TrainingID = null;

            PhoenixInspectionTrainingSchedule.TrainingScheduleReport(rowusercode, TrainingScheduleId, scenario, scenarioids, remarks, Trainingdonedate, status, statusid, reason, ref TrainingID);

            if (ViewState["E"] == null)
            {
                PhoenixInspectionTrainingSchedule.TrainingScheduleInsertAfterReport(rowusercode, TrainingScheduleId, Trainingdonedate);
            }
            if (ViewState["LAUNCH"] != null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                         "BookMarkScript", "closeTelerikWindow('Report', 'wo',true);", true);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                         "BookMarkScript", "closeTelerikWindow('Report', 'wo',true);", true);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            Page_Load(sender, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
