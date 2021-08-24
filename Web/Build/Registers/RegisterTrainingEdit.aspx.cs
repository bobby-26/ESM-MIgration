using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Text;

public partial class Registers_RegisterTrainingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        TabstripTrainingaddmenu.MenuList = toolbargrid.Show();

        if (!Page.IsPostBack)
        {
            DataTable dt1 = PhoenixRegisterDrill.vessellist();

            radcbappliesto.DataSource = dt1;
            radcbappliesto.DataBind();

            DataTable dt2 = PhoenixRegisterDrill.ListOfVessels();
            radexcludedvessels.DataSource = dt2;
            radexcludedvessels.DataBind();
            ViewState["TRAININGID"] = General.GetNullableGuid(Request.QueryString["trainingid"]);
            Guid? TrainingID = General.GetNullableGuid(ViewState["TRAININGID"].ToString());

            DataTable dt = PhoenixRegisterTraining.TrainingEditList(TrainingID);
            if (dt.Rows.Count > 0)
            {
                RadTrainingnameentry.Text = dt.Rows[0]["FLDTRAININGNAME"].ToString();
                tbfrequencyentry.Text = dt.Rows[0]["FLDFREQUENCY"].ToString();
                RadRadioButtonfrequencytype.SelectedValue = dt.Rows[0]["FLDFREQUENCYTYPE"].ToString();
                General.RadBindCheckBoxList(radcbappliesto, dt.Rows[0]["FLDAPPLIESTO"].ToString());
                RadRadioButtontype.SelectedValue = dt.Rows[0]["FLDTYPE"].ToString();
                RadRadioButtonfixedorvariable.SelectedValue = dt.Rows[0]["FLDFIXEDORVARIABLE"].ToString();
                RadRadiophotoynlist.SelectedValue = dt.Rows[0]["FLDPHOTOYN"].ToString();
                RadRadioButtondashboardynlist.SelectedValue = dt.Rows[0]["FLDDASHBOARDYN"].ToString();
                General.RadBindComboBoxCheckList(radexcludedvessels, dt.Rows[0]["FLDEXCLUDEDVESSELS"].ToString());

            }

        }
    }

    protected void Trainingaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                string Trainingname = General.GetNullableString(RadTrainingnameentry.Text);
                int? frequency = General.GetNullableInteger(tbfrequencyentry.Text);
                string frequencytype = RadRadioButtonfrequencytype.SelectedValue;
                string appliesto = General.GetNullableString(General.RadCheckBoxList(radcbappliesto));
                string fixedorvariable = RadRadioButtonfixedorvariable.SelectedValue;
                string type = RadRadioButtontype.SelectedValue;
              
                string photoyn = RadRadiophotoynlist.SelectedValue;
                string showondashboard = RadRadioButtondashboardynlist.SelectedValue;
                if (!IsValidTrainingDetails(Trainingname, frequency, appliesto, frequencytype))
                {
                    ucError.Visible = true;
                    return;
                }
                string newapplicabletype = string.Empty;
                Guid? TrainingId = General.GetNullableGuid(ViewState["TRAININGID"].ToString());
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string excludedvessels = string.Empty;
                excludedvessels = General.GetNullableString(GetCheckedItemsvalues(radexcludedvessels, excludedvessels));
                PhoenixRegisterTraining.TrainingUpdate(rowusercode
                                                , Trainingname
                                                , frequency
                                                , frequencytype
                                                , appliesto
                                                , photoyn
                                                , showondashboard
                                                , fixedorvariable
                                                , type
                                                , TrainingId
                                                , excludedvessels
                                               );
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    private bool IsValidTrainingDetails(string Training, int? frequency, string appliesto, string frequencytype)
    {

        ucError.HeaderMessage = "Provide the following required information";


        if (Training == null)
        {
            ucError.ErrorMessage = "Training name.";
        }
        if (frequency == null)
        {
            ucError.ErrorMessage = "Frequency.";
        }
        if (appliesto == null)
        {
            ucError.ErrorMessage = "Vessel types to which the drill is applicable.";
        }
        if (frequencytype == "Months" && frequency > 12)
        {
            ucError.ErrorMessage = "Valid Frequency (1-12)";
        }

        if (frequencytype == "Weeks" && frequency > 52)
        {
            ucError.ErrorMessage = "Valid Frequency (1-52)";
        }

        return (!ucError.IsError);
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
}